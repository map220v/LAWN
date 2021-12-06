using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Sexy.TodLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Sexy
{
	internal class ResourceManager : IDisposable
	{
		private const string DELAYED_GROUP_NAME = "DelayLoad_";

		public double mProgress;

		public int mTotalResources;

		public int mLoadedCount;

		protected Dictionary<string, BaseRes> mImageMap;

		protected Dictionary<string, BaseRes> mSoundMap;

		protected Dictionary<string, BaseRes> mFontMap;

		protected Dictionary<string, BaseRes> mMusicMap;

		protected Dictionary<string, BaseRes> mReanimMap;

		protected Dictionary<string, BaseRes> mParticleMap;

		protected Dictionary<string, BaseRes> mTrailMap;

		protected Dictionary<string, BaseRes> mLevelMap;

		protected string mError;

		protected bool mHasFailed;

		protected SexyAppBase mApp;

		protected string mCurResGroup;

		protected string mDefaultPath;

		protected string mDefaultIdPrefix;

		protected XMLParser mXMLParser;

		protected bool mAllowMissingProgramResources;

		protected bool mAllowAlreadyDefinedResources;

		protected bool mHadAlreadyDefinedError;

		protected Dictionary<string, List<BaseRes>> mResGroupMap;

		protected List<BaseRes> mCurResGroupList;

		protected List<BaseRes>.Enumerator mCurResGroupListItr;

		protected List<string> mLoadedGroups;

		protected ContentManager mContentManager;

		public static ContentManager mReanimContentManager;

		public static ContentManager mParticleContentManager;

		public static ContentManager[] mUnloadContentManager = new ContentManager[5];

		private ContentManager mBackgroundContentmanager;

		private SpriteFont arial;

		public static object DrawLocker = new object();

		private static SpriteBatch imageLoadSpritebatch = new SpriteBatch(GlobalStaticVars.g.GraphicsDevice);

		private BlendState imageLoadBlendAlpha = new BlendState
		{
			ColorWriteChannels = ColorWriteChannels.Alpha,
			AlphaDestinationBlend = Blend.Zero,
			ColorDestinationBlend = Blend.Zero,
			AlphaSourceBlend = Blend.One,
			ColorSourceBlend = Blend.One
		};

		private BlendState blendColorLoadState = new BlendState
		{
			ColorWriteChannels = (ColorWriteChannels.Red | ColorWriteChannels.Green | ColorWriteChannels.Blue),
			AlphaDestinationBlend = Blend.Zero,
			ColorDestinationBlend = Blend.Zero,
			AlphaSourceBlend = Blend.SourceAlpha,
			ColorSourceBlend = Blend.SourceAlpha
		};

		private ContentManager backDropContentManager;

		private int loadedBackdrop = -1;

		private List<BaseRes> unloadableResources = new List<BaseRes>();

		public ResourceManager(SexyAppBase theApp)
		{
			mApp = theApp;
			mHasFailed = false;
			mXMLParser = null;
			mCurResGroupList = null;
			mResGroupMap = new Dictionary<string, List<BaseRes>>();
			mImageMap = new Dictionary<string, BaseRes>();
			mSoundMap = new Dictionary<string, BaseRes>();
			mFontMap = new Dictionary<string, BaseRes>();
			mMusicMap = new Dictionary<string, BaseRes>();
			mLevelMap = new Dictionary<string, BaseRes>();
			mReanimMap = new Dictionary<string, BaseRes>();
			mParticleMap = new Dictionary<string, BaseRes>();
			mTrailMap = new Dictionary<string, BaseRes>();
			mLoadedGroups = new List<string>();
			mContentManager = mApp.mContentManager;
			for (int i = 0; i < mUnloadContentManager.Length; i++)
			{
				mUnloadContentManager[i] = new ContentManager(SexyAppBase.XnaGame.Services);
				mUnloadContentManager[i].RootDirectory = mApp.mContentManager.RootDirectory;
			}
			mReanimContentManager = new ContentManager(SexyAppBase.XnaGame.Services);
			mReanimContentManager.RootDirectory = mApp.mContentManager.RootDirectory;
			mParticleContentManager = new ContentManager(SexyAppBase.XnaGame.Services);
			mParticleContentManager.RootDirectory = mApp.mContentManager.RootDirectory;
			backDropContentManager = new ContentManager(SexyAppBase.XnaGame.Services);
			backDropContentManager.RootDirectory = mContentManager.RootDirectory;
			mAllowAlreadyDefinedResources = false;
			mAllowMissingProgramResources = false;
			mProgress = 0.0;
			mTotalResources = 0;
			mLoadedCount = 0;
			arial = mContentManager.Load<SpriteFont>("fonts/Arial");
		}

		public virtual string GetResourceDir()
		{
			return "";
		}

		private ContentManager GetContentManager(BaseRes res)
		{
			if (res.mUnloadGroup <= 0)
			{
				return mContentManager;
			}
			return mUnloadContentManager[res.mUnloadGroup];
		}

		~ResourceManager()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
		}

		public bool HadError()
		{
			return mHasFailed;
		}

		public bool IsGroupLoaded(string theResGroup)
		{
			return mLoadedGroups.Contains(theResGroup);
		}

		public bool LoadReanimation(string filename, ref ReanimatorDefinition def)
		{
			Reanimator reanimator = new Reanimator();
			//TODO: Automatically detect images that required to be loaded (instead of hardcoding every image in resources.xml)
			def = reanimator.ParseReanimationFile("Content/" + filename + ".reanim");
			def.ExtractImages();
			return true;
		}

		public bool LoadParticle(string filename, ref TodParticleDefinition def)
		{
			def = mParticleContentManager.Load<TodParticleDefinition>(filename);
			return true;
		}

		public bool LoadTrail(string filename, ref TrailDefinition def)
		{
			def = mContentManager.Load<TrailDefinition>(filename);
			return true;
		}

		public void ExtractReanimImages()
		{
			Dictionary<string, BaseRes>.Enumerator enumerator = mReanimMap.GetEnumerator();
			while (enumerator.MoveNext())
			{
				((ReanimRes)enumerator.Current.Value).mReanim.ExtractImages();
			}
		}

		public bool ParseResourcesFile(string theFilename)
		{
			mXMLParser = new XMLParser();
			if (!mXMLParser.OpenFile(Path.Combine(GetResourceDir(), theFilename)))
			{
				Fail("Resource file not found: " + theFilename);
			}
			XMLElement theElement = new XMLElement();
			while (!mXMLParser.HasFailed())
			{
				if (!mXMLParser.NextElement(ref theElement))
				{
					Fail(mXMLParser.GetErrorText());
				}
				if (theElement.mType == XMLElementType.TYPE_START)
				{
					if (theElement.mValue != "ResourceManifest")
					{
						break;
					}
					return DoParseResources();
				}
			}
			Fail("Expecting ResourceManifest tag");
			return DoParseResources();
		}

		private bool DoParseResources()
		{
			if (!mXMLParser.HasFailed())
			{
				while (true)
				{
					XMLElement theElement = new XMLElement();
					if (!mXMLParser.NextElement(ref theElement))
					{
						break;
					}
					if (theElement.mType == XMLElementType.TYPE_START)
					{
						if (!(theElement.mValue == "Resources"))
						{
							Fail("Invalid Section '" + theElement.mValue + "'");
							break;
						}
						mCurResGroup = theElement.mAttributes["id"];
						mResGroupMap.Add(mCurResGroup, new List<BaseRes>());
						mCurResGroupList = mResGroupMap[mCurResGroup];
						if (mCurResGroup.Length == 0)
						{
							Fail("No id specified.");
							break;
						}
						if (!ParseResources())
						{
							break;
						}
					}
					else if (theElement.mType == XMLElementType.TYPE_ELEMENT)
					{
						Fail("Element Not Expected '" + theElement.mValue + "'");
						break;
					}
				}
			}
			if (mXMLParser.HasFailed())
			{
				Fail(mXMLParser.GetErrorText());
			}
			mXMLParser = null;
			return !mHasFailed;
		}

		private bool ParseResources()
		{
			mDefaultPath = GetResourceDir();
			while (true)
			{
				XMLElement theElement = new XMLElement();
				if (!mXMLParser.NextElement(ref theElement))
				{
					return false;
				}
				if (theElement.mType == XMLElementType.TYPE_START)
				{
					if (theElement.mValue == "Image")
					{
						if (!ParseImageResource(theElement))
						{
							return false;
						}
						if (!mXMLParser.NextElement(ref theElement))
						{
							return false;
						}
						if (theElement.mType != XMLElementType.TYPE_END)
						{
							return Fail("Unexpected element found.");
						}
						continue;
					}
					if (theElement.mValue == "Reanim")
					{
						if (!ParseReanimResource(theElement))
						{
							return false;
						}
						if (!mXMLParser.NextElement(ref theElement))
						{
							return false;
						}
						if (theElement.mType != XMLElementType.TYPE_END)
						{
							return Fail("Unexpected element found.");
						}
						continue;
					}
					if (theElement.mValue == "Particle")
					{
						if (!ParseParticleResource(theElement))
						{
							return false;
						}
						if (!mXMLParser.NextElement(ref theElement))
						{
							return false;
						}
						if (theElement.mType != XMLElementType.TYPE_END)
						{
							return Fail("Unexpected element found.");
						}
						continue;
					}
					if (theElement.mValue == "Trail")
					{
						if (!ParseTrailResource(theElement))
						{
							return false;
						}
						if (!mXMLParser.NextElement(ref theElement))
						{
							return false;
						}
						if (theElement.mType != XMLElementType.TYPE_END)
						{
							return Fail("Unexpected element found.");
						}
						continue;
					}
					if (theElement.mValue == "Sound")
					{
						if (!ParseSoundResource(theElement))
						{
							return false;
						}
						if (!mXMLParser.NextElement(ref theElement))
						{
							return false;
						}
						if (theElement.mType != XMLElementType.TYPE_END)
						{
							return Fail("Unexpected element found.");
						}
						continue;
					}
					if (theElement.mValue == "Font")
					{
						if (!ParseFontResource(theElement))
						{
							return false;
						}
						if (!mXMLParser.NextElement(ref theElement))
						{
							return false;
						}
						if (theElement.mType != XMLElementType.TYPE_END)
						{
							return Fail("Unexpected element found.");
						}
						continue;
					}
					if (theElement.mValue == "Music")
					{
						if (!ParseMusicResource(theElement))
						{
							return false;
						}
						if (!mXMLParser.NextElement(ref theElement))
						{
							return false;
						}
						if (theElement.mType != XMLElementType.TYPE_END)
						{
							return Fail("Unexpected element found.");
						}
						continue;
					}
					if (theElement.mValue == "Level")
					{
						if (!ParseLevelResource(theElement))
						{
							return false;
						}
						if (!mXMLParser.NextElement(ref theElement))
						{
							return false;
						}
						if (theElement.mType != XMLElementType.TYPE_END)
						{
							return Fail("Unexpected element found.");
						}
						continue;
					}
					if (!(theElement.mValue == "SetDefaults"))
					{
						Fail("Invalid Section '" + theElement.mValue + "'");
						return false;
					}
					if (!ParseSetDefaults(theElement))
					{
						return false;
					}
					if (!mXMLParser.NextElement(ref theElement))
					{
						return false;
					}
					if (theElement.mType != XMLElementType.TYPE_END)
					{
						return Fail("Unexpected element found.");
					}
				}
				else
				{
					if (theElement.mType == XMLElementType.TYPE_ELEMENT)
					{
						Fail("Element Not Expected '" + theElement.mValue + "'");
						return false;
					}
					if (theElement.mType == XMLElementType.TYPE_END)
					{
						break;
					}
				}
			}
			return true;
		}

		private bool ParseCommonResource(XMLElement theElement, BaseRes theRes, Dictionary<string, BaseRes> theMap)
		{
			mHadAlreadyDefinedError = false;
			string text = theElement.mAttributes["path"];
			theRes.mXMLAttributes = theElement.mAttributes;
			theRes.mFromProgram = false;
			if (text.Length > 0 && text[0] == '!')
			{
				theRes.mPath = text;
				if (text == "!program")
				{
					theRes.mFromProgram = true;
				}
			}
			else
			{
				theRes.mPath = mDefaultPath + text;
			}
			Dictionary<string, string>.Enumerator enumerator = theElement.mAttributes.GetEnumerator();
			string text2 = mDefaultIdPrefix;
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Key == "id")
				{
					text2 = mDefaultIdPrefix + enumerator.Current.Value;
				}
			}
			theRes.mResGroup = mCurResGroup;
			theRes.mId = text2;
			enumerator = theElement.mAttributes.GetEnumerator();
			string text3 = null;
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Key == "unloadGroup")
				{
					text3 = enumerator.Current.Value;
				}
			}
			if (!string.IsNullOrEmpty(text3))
			{
				theRes.mUnloadGroup = int.Parse(text3);
			}
			enumerator = theElement.mAttributes.GetEnumerator();
			string value = null;
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Key == "surface")
				{
					value = enumerator.Current.Value;
				}
			}
			if (!string.IsNullOrEmpty(value))
			{
				((ImageRes)theRes).lowMemorySurfaceFormat = (SurfaceFormat)Enum.Parse(typeof(SurfaceFormat), value, true);
			}
			if (theMap.ContainsKey(text2))
			{
				mHadAlreadyDefinedError = true;
				return Fail("Resource already defined.");
			}
			theMap.Add(text2, theRes);
			mCurResGroupList.Add(theRes);
			return true;
		}

		private bool ParseSetDefaults(XMLElement theElement)
		{
			Dictionary<string, string>.Enumerator enumerator = theElement.mAttributes.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Key == "path")
				{
					mDefaultPath = GetResourceDir() + enumerator.Current.Value + "/";
				}
				if (enumerator.Current.Key == "idprefix")
				{
					mDefaultIdPrefix = enumerator.Current.Value;
				}
			}
			return true;
		}

		private bool ParseFontResource(XMLElement theElement)
		{
			FontRes fontRes = new FontRes();
			if (!ParseCommonResource(theElement, fontRes, mFontMap))
			{
				if (!mHadAlreadyDefinedError || !mAllowAlreadyDefinedResources)
				{
					fontRes.DeleteResource();
					return false;
				}
				mError = "";
				mHasFailed = false;
				FontRes fontRes2 = fontRes;
				fontRes = (FontRes)mFontMap[fontRes2.mId];
				fontRes.mPath = fontRes2.mPath;
				fontRes.mXMLAttributes = fontRes2.mXMLAttributes;
				fontRes2.DeleteResource();
			}
			fontRes.mFont = null;
			Dictionary<string, string>.Enumerator enumerator = theElement.mAttributes.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Key == "tags")
				{
					fontRes.mTags = enumerator.Current.Value;
				}
				if (enumerator.Current.Key == "isDefault")
				{
					fontRes.mDefault = true;
				}
			}
			if (fontRes.mPath.Substring(0, 5) == "!sys:")
			{
				fontRes.mSysFont = true;
				fontRes.mPath = fontRes.mPath.Substring(5);
				enumerator = theElement.mAttributes.GetEnumerator();
				fontRes.mSize = -1;
				fontRes.mBold = false;
				fontRes.mItalic = false;
				fontRes.mShadow = false;
				fontRes.mUnderline = false;
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Key == "size")
					{
						fontRes.mSize = Convert.ToInt32(enumerator.Current.Value, 10);
					}
					else if (enumerator.Current.Key == "bold")
					{
						fontRes.mBold = true;
					}
					else if (enumerator.Current.Key == "italic")
					{
						fontRes.mBold = true;
					}
					else if (enumerator.Current.Key == "shadow")
					{
						fontRes.mBold = true;
					}
					else if (enumerator.Current.Key == "underline")
					{
						fontRes.mBold = true;
					}
				}
				if (fontRes.mSize <= 0)
				{
					return Fail("SysFont needs point size");
				}
			}
			else
			{
				fontRes.mSysFont = false;
			}
			return true;
		}

		public bool ParseSoundResource(XMLElement theElement)
		{
			SoundRes soundRes = new SoundRes();
			if (!ParseCommonResource(theElement, soundRes, mSoundMap))
			{
				if (!mHadAlreadyDefinedError || !mAllowAlreadyDefinedResources)
				{
					soundRes.DeleteResource();
					return false;
				}
				mError = "";
				mHasFailed = false;
				SoundRes soundRes2 = soundRes;
				soundRes = (SoundRes)mSoundMap[soundRes2.mId];
				soundRes.mPath = soundRes2.mPath;
				soundRes.mXMLAttributes = soundRes2.mXMLAttributes;
				soundRes2.DeleteResource();
			}
			soundRes.mSoundId = -1;
			soundRes.mVolume = -1.0;
			soundRes.mPanning = 0;
			Dictionary<string, string>.Enumerator enumerator = theElement.mAttributes.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Key == "volume")
				{
					soundRes.mVolume = Convert.ToDouble(enumerator.Current.Value);
				}
				if (enumerator.Current.Key == "pan")
				{
					soundRes.mPanning = Convert.ToInt32(enumerator.Current.Value);
				}
			}
			return true;
		}

		public bool ParseMusicResource(XMLElement theElement)
		{
			MusicRes musicRes = new MusicRes();
			if (!ParseCommonResource(theElement, musicRes, mMusicMap))
			{
				if (!mHadAlreadyDefinedError || !mAllowAlreadyDefinedResources)
				{
					musicRes.DeleteResource();
					return false;
				}
				mError = "";
				mHasFailed = false;
				MusicRes musicRes2 = musicRes;
				musicRes = (MusicRes)mMusicMap[musicRes2.mId];
				musicRes.mPath = musicRes2.mPath;
				musicRes.mXMLAttributes = musicRes2.mXMLAttributes;
				musicRes2.DeleteResource();
			}
			musicRes.mSongId = -1;
			return true;
		}

		public bool ParseReanimResource(XMLElement theElement)
		{
			ReanimRes reanimRes = new ReanimRes();
			if (!ParseCommonResource(theElement, reanimRes, mReanimMap))
			{
				if (!mHadAlreadyDefinedError || !mAllowAlreadyDefinedResources)
				{
					reanimRes.DeleteResource();
					return false;
				}
				mError = "";
				mHasFailed = false;
				ReanimRes reanimRes2 = reanimRes;
				reanimRes = (ReanimRes)mReanimMap[reanimRes2.mId];
				reanimRes.mPath = reanimRes2.mPath;
				reanimRes.mXMLAttributes = reanimRes2.mXMLAttributes;
				reanimRes2.DeleteResource();
			}
			return true;
		}

		public bool ParseParticleResource(XMLElement theElement)
		{
			ParticleRes particleRes = new ParticleRes();
			if (!ParseCommonResource(theElement, particleRes, mParticleMap))
			{
				if (!mHadAlreadyDefinedError || !mAllowAlreadyDefinedResources)
				{
					particleRes.DeleteResource();
					return false;
				}
				mError = "";
				mHasFailed = false;
				ParticleRes particleRes2 = particleRes;
				particleRes = (ParticleRes)mParticleMap[particleRes2.mId];
				particleRes.mPath = particleRes2.mPath;
				particleRes.mXMLAttributes = particleRes2.mXMLAttributes;
				particleRes2.DeleteResource();
			}
			return true;
		}

		public bool ParseTrailResource(XMLElement theElement)
		{
			TrailRes trailRes = new TrailRes();
			if (!ParseCommonResource(theElement, trailRes, mTrailMap))
			{
				if (!mHadAlreadyDefinedError || !mAllowAlreadyDefinedResources)
				{
					trailRes.DeleteResource();
					return false;
				}
				mError = "";
				mHasFailed = false;
				TrailRes trailRes2 = trailRes;
				trailRes = (TrailRes)mTrailMap[trailRes2.mId];
				trailRes.mPath = trailRes2.mPath;
				trailRes.mXMLAttributes = trailRes2.mXMLAttributes;
				trailRes2.DeleteResource();
			}
			return true;
		}

		public bool ParseLevelResource(XMLElement theElement)
		{
			LevelRes levelRes = new LevelRes();
			if (!ParseCommonResource(theElement, levelRes, mLevelMap))
			{
				if (!mHadAlreadyDefinedError || !mAllowAlreadyDefinedResources)
				{
					levelRes.DeleteResource();
					return false;
				}
				mError = "";
				mHasFailed = false;
				LevelRes levelRes2 = levelRes;
				levelRes = (LevelRes)mLevelMap[levelRes2.mId];
				levelRes.mPath = levelRes2.mPath;
				levelRes.mXMLAttributes = levelRes2.mXMLAttributes;
				levelRes2.DeleteResource();
			}
			levelRes.mLevelNumber = Convert.ToInt32(levelRes.mId);
			return true;
		}

		public bool ParseImageResource(XMLElement theElement)
		{
			ImageRes imageRes = new ImageRes();
			if (!ParseCommonResource(theElement, imageRes, mImageMap))
			{
				if (!mHadAlreadyDefinedError || !mAllowAlreadyDefinedResources)
				{
					imageRes.DeleteResource();
					return false;
				}
				mError = "";
				mHasFailed = false;
				ImageRes imageRes2 = imageRes;
				imageRes = (ImageRes)mImageMap[imageRes2.mId];
				imageRes.mPath = imageRes2.mPath;
				imageRes.mXMLAttributes = imageRes2.mXMLAttributes;
				imageRes2.DeleteResource();
			}
			imageRes.mPalletize = !theElement.mAttributes.ContainsKey("nopal");
			imageRes.mA4R4G4B4 = theElement.mAttributes.ContainsKey("a4r4g4b4");
			imageRes.mDDSurface = theElement.mAttributes.ContainsKey("ddsurface");
			imageRes.mPurgeBits = (theElement.mAttributes.ContainsKey("nobits") || (mApp.Is3DAccelerated() && theElement.mAttributes.ContainsKey("nobits3d")) || (!mApp.Is3DAccelerated() && theElement.mAttributes.ContainsKey("nobits2d")));
			imageRes.mA8R8G8B8 = theElement.mAttributes.ContainsKey("a8r8g8b8");
			imageRes.mR5G6B5 = theElement.mAttributes.ContainsKey("r5g6b5");
			imageRes.mA1R5G5B5 = theElement.mAttributes.ContainsKey("a1r5g5b5");
			imageRes.mMinimizeSubdivisions = theElement.mAttributes.ContainsKey("minsubdivide");
			imageRes.mAutoFindAlpha = !theElement.mAttributes.ContainsKey("noalpha");
			Dictionary<string, string>.Enumerator enumerator = theElement.mAttributes.GetEnumerator();
			imageRes.mAlphaColor = 16777215u;
			imageRes.mRows = 1;
			imageRes.mCols = 1;
			AnimType animType = AnimType.AnimType_None;
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Key == "alphaimage")
				{
					imageRes.mAlphaImage = mDefaultPath + enumerator.Current.Value;
				}
				else if (enumerator.Current.Key == "alphacolor")
				{
					imageRes.mAlphaColor = Convert.ToUInt32(enumerator.Current.Value);
				}
				else if (enumerator.Current.Key == "variant")
				{
					imageRes.mVariant = enumerator.Current.Value;
				}
				else if (enumerator.Current.Key == "alphagrid")
				{
					imageRes.mAlphaGridImage = mDefaultPath + enumerator.Current.Value;
				}
				else if (enumerator.Current.Key == "rows")
				{
					imageRes.mRows = Convert.ToInt32(enumerator.Current.Value);
				}
				else if (enumerator.Current.Key == "cols")
				{
					imageRes.mCols = Convert.ToInt32(enumerator.Current.Value);
				}
				else if (enumerator.Current.Key == "languageSpecific")
				{
					imageRes.mLanguageSpecific = Convert.ToBoolean(enumerator.Current.Value);
				}
				else if (enumerator.Current.Key == "format")
				{
					string value = Convert.ToString((object)enumerator.Current.Value);
					imageRes.mFormat = (ImageRes.TextureFormat)Enum.Parse(typeof(ImageRes.TextureFormat), value, true);
				}
				else
				{
					if (!(enumerator.Current.Key == "anim"))
					{
						continue;
					}
					string value2 = enumerator.Current.Value;
					if (value2 == "none")
					{
						animType = AnimType.AnimType_None;
						continue;
					}
					if (value2 == "once")
					{
						animType = AnimType.AnimType_Once;
						continue;
					}
					if (value2 == "loop")
					{
						animType = AnimType.AnimType_Loop;
						continue;
					}
					if (!(value2 == "pingpong"))
					{
						Fail("Invalid animation type.");
						return false;
					}
					animType = AnimType.AnimType_PingPong;
				}
			}
			imageRes.mAnimInfo.mAnimType = animType;
			if (animType != 0)
			{
				int theNumCels = Math.Max(imageRes.mRows, imageRes.mCols);
				int theBeginFrameTime = 0;
				int theEndFrameTime = 0;
				enumerator = theElement.mAttributes.GetEnumerator();
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Key == "framedelay")
					{
						imageRes.mAnimInfo.mFrameDelay = Convert.ToInt32(enumerator.Current.Value);
					}
					else if (enumerator.Current.Key == "begindelay")
					{
						theBeginFrameTime = Convert.ToInt32(enumerator.Current.Value);
					}
					else if (enumerator.Current.Key == "enddelay")
					{
						theEndFrameTime = Convert.ToInt32(enumerator.Current.Value);
					}
					else if (enumerator.Current.Key == "perframedelay")
					{
						ReadIntVector(enumerator.Current.Value, ref imageRes.mAnimInfo.mPerFrameDelay);
					}
					else if (enumerator.Current.Key == "framemap")
					{
						ReadIntVector(enumerator.Current.Value, ref imageRes.mAnimInfo.mFrameMap);
					}
				}
				imageRes.mAnimInfo.Compute(theNumCels, theBeginFrameTime, theEndFrameTime);
			}
			return true;
		}

		private static void ReadIntVector(string theVal, ref List<int> theVector)
		{
			theVector.Clear();
			char[] separator = new char[1]
			{
				','
			};
			string[] array = theVal.Split(separator);
			for (int i = 0; i < array.Length; i++)
			{
				theVector.Add(Convert.ToInt32(array[i]));
			}
		}

		public void DeleteImage(string theName)
		{
			ReplaceImage(theName, null);
		}

		public void DeleteResources(Dictionary<string, BaseRes> theMap, string theGroup)
		{
			foreach (KeyValuePair<string, BaseRes> item in theMap)
			{
				if (theGroup == string.Empty || item.Value.mResGroup == theGroup)
				{
					item.Value.DeleteResource();
				}
			}
		}

		public void DeleteResources(string theGroup)
		{
			DeleteResources(mImageMap, theGroup);
			DeleteResources(mSoundMap, theGroup);
			DeleteResources(mFontMap, theGroup);
			mLoadedGroups.Remove(theGroup);
			Resources.ExtractResourcesByName(this, theGroup);
		}

		public void UnloadInitResources()
		{
			foreach (BaseRes unloadableResource in unloadableResources)
			{
				unloadableResource.DeleteResource();
			}
			unloadableResources.Clear();
		}

		public void UnloadBackground(string theGroup)
		{
			GC.Collect();
			DeleteResources(theGroup);
			mLoadedGroups.Remove(theGroup);
			GC.Collect();
			SexyAppBase.XnaGame.CompensateForSlowUpdate();
		}

		public bool ReplaceImage(string theId, Image theImage)
		{
			return true;
		}

		public Font GetFontThrow(string theRes)
		{
			if (mFontMap.ContainsKey(theRes))
			{
				return ((FontRes)mFontMap[theRes]).mFont;
			}
			return null;
		}

		public ReanimatorDefinition GetReanimThrow(string theRes)
		{
			if (mReanimMap.ContainsKey(theRes))
			{
				return ((ReanimRes)mReanimMap[theRes]).mReanim;
			}
			return null;
		}

		public TodParticleDefinition GetParticleThrow(string theRes)
		{
			if (mParticleMap.ContainsKey(theRes))
			{
				return ((ParticleRes)mParticleMap[theRes]).mParticle;
			}
			return null;
		}

		public TrailDefinition GetTrailThrow(string theRes)
		{
			if (mTrailMap.ContainsKey(theRes))
			{
				return ((TrailRes)mTrailMap[theRes]).mTrail;
			}
			return null;
		}

		public Image GetImageThrow(string theRes)
		{
			if (mImageMap.ContainsKey(theRes))
			{
				return ((ImageRes)mImageMap[theRes]).mImage;
			}
			return null;
		}

		public int GetSoundThrow(string theRes)
		{
			if (mSoundMap.ContainsKey(theRes))
			{
				return ((SoundRes)mSoundMap[theRes]).mSoundId;
			}
			return -1;
		}

		public int GetMusicThrow(string theRes)
		{
			if (mMusicMap.ContainsKey(theRes))
			{
				return ((MusicRes)mMusicMap[theRes]).mSongId;
			}
			return -1;
		}

		public int GetNumResources(string theResGroup, Dictionary<string, BaseRes> theResMap)
		{
			if (string.IsNullOrEmpty(theResGroup))
			{
				return theResMap.Count;
			}
			int num = 0;
			Dictionary<string, BaseRes>.Enumerator enumerator = theResMap.GetEnumerator();
			while (enumerator.MoveNext())
			{
				BaseRes value = enumerator.Current.Value;
				if (value.mResGroup == theResGroup && !value.mFromProgram)
				{
					num++;
				}
			}
			return num;
		}

		public int GetNumResourcesGroupNameStartsWith(string theResGroup, Dictionary<string, BaseRes> theResMap)
		{
			if (string.IsNullOrEmpty(theResGroup))
			{
				return theResMap.Count;
			}
			int num = 0;
			Dictionary<string, BaseRes>.Enumerator enumerator = theResMap.GetEnumerator();
			while (enumerator.MoveNext())
			{
				BaseRes value = enumerator.Current.Value;
				if (value.mResGroup.StartsWith(theResGroup))
				{
					num++;
				}
			}
			return num;
		}

		public int GetNumResourcesGroupNameStartsWith(string theResGroup)
		{
			return GetNumResourcesGroupNameStartsWith(theResGroup, mImageMap);
		}

		public int GetNumResources(string theResGroup)
		{
			return GetNumImages(theResGroup) + GetNumSounds(theResGroup) + GetNumFonts(theResGroup) + GetNumSongs(theResGroup);
		}

		public int GetNumImages(string theResGroup)
		{
			return GetNumResources(theResGroup, mImageMap);
		}

		public int GetNumSounds(string theResGroup)
		{
			return GetNumResources(theResGroup, mSoundMap);
		}

		public int GetNumFonts(string theResGroup)
		{
			return GetNumResources(theResGroup, mFontMap);
		}

		public int GetNumSongs(string theResGroup)
		{
			return GetNumResources(theResGroup, mMusicMap);
		}

		public int GetNumReanims(string theResGroup)
		{
			return GetNumResources(theResGroup, mReanimMap);
		}

		public int GetNumParticles(string theResGroup)
		{
			return GetNumResources(theResGroup, mParticleMap);
		}

		public int GetNumTrails(string theResGroup)
		{
			return GetNumResources(theResGroup, mTrailMap);
		}

		public void LoadAllResources()
		{
			mTotalResources = GetNumResources("");
			mTotalResources -= GetNumResourcesGroupNameStartsWith("DelayLoad_");
			mLoadedCount = 0;
			mProgress = 0.0;
			Dictionary<string, List<BaseRes>>.Enumerator enumerator = mResGroupMap.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (!(enumerator.Current.Key == "Levels") && !enumerator.Current.Key.StartsWith("DelayLoad_"))
				{
					StartLoadResources(enumerator.Current.Key);
					LoadResources(enumerator.Current.Key);
					if (HadError())
					{
						break;
					}
				}
			}
		}

		public void StartLoadResources(string theResGroup)
		{
			mError = "";
			mHasFailed = false;
			mCurResGroup = theResGroup;
			mCurResGroupList = mResGroupMap[theResGroup];
			mCurResGroupListItr = mCurResGroupList.GetEnumerator();
		}

		public bool LoadResources(string theResGroup)
		{
			mError = "";
			mHasFailed = false;
			StartLoadResources(theResGroup);
			while (LoadNextResource())
			{
			}
			if (!HadError())
			{
				mLoadedGroups.Add(theResGroup);
				return true;
			}
			return false;
		}

		public bool LoadNextResource()
		{
			if (HadError())
			{
				return false;
			}
			if (mCurResGroupList == null)
			{
				return false;
			}
			bool flag = false;
			bool flag2 = false;
			BaseRes baseRes = null;
			while (mCurResGroupListItr.MoveNext())
			{
				baseRes = mCurResGroupListItr.Current;
				if (baseRes.mFromProgram)
				{
					continue;
				}
				switch (baseRes.mType)
				{
				case ResType.ResType_Image:
				{
					ImageRes imageRes = (ImageRes)baseRes;
					if (imageRes.mImage != null)
					{
						continue;
					}
					flag = DoLoadImage(imageRes);
					flag2 = true;
					break;
				}
				case ResType.ResType_Reanim:
				{
					ReanimRes reanimRes = (ReanimRes)baseRes;
					if (reanimRes.mReanim != null)
					{
						continue;
					}
					flag = DoLoadReanim(ref reanimRes);
					flag2 = true;
					break;
				}
				case ResType.ResType_Particle:
				{
					ParticleRes particleRes = (ParticleRes)baseRes;
					if (particleRes.mParticle != null)
					{
						continue;
					}
					flag = DoLoadParticle(ref particleRes);
					flag2 = true;
					break;
				}
				case ResType.ResType_Trail:
				{
					TrailRes trailRes = (TrailRes)baseRes;
					if (trailRes.mTrail != null)
					{
						continue;
					}
					flag = DoLoadTrail(ref trailRes);
					flag2 = true;
					break;
				}
				case ResType.ResType_Sound:
				{
					SoundRes soundRes = (SoundRes)baseRes;
					if (soundRes.mSoundId != -1)
					{
						continue;
					}
					flag = DoLoadSound(soundRes);
					flag2 = true;
					break;
				}
				case ResType.ResType_Music:
				{
					MusicRes musicRes = (MusicRes)baseRes;
					if (musicRes.mSongId != -1)
					{
						continue;
					}
					flag = DoLoadMusic(musicRes);
					flag2 = true;
					break;
				}
				case ResType.ResType_Font:
				{
					FontRes fontRes = (FontRes)baseRes;
					if (fontRes.mFont != null)
					{
						continue;
					}
					flag = DoLoadFont(fontRes);
					flag2 = true;
					break;
				}
				}
				if (flag2)
				{
					break;
				}
			}
			if (flag)
			{
				mLoadedCount++;
				mProgress = (double)mLoadedCount / (double)mTotalResources;
				Debug.OutputDebug(mProgress.ToString());
			}
			return flag;
		}

		public bool LoadLevelBackgrounds(int levelNumber, out Image verticalBackground, out Image horizontalBackground)
		{
			Dictionary<string, BaseRes>.Enumerator enumerator = mLevelMap.GetEnumerator();
			if (loadedBackdrop != levelNumber)
			{
				backDropContentManager.Unload();
			}
			while (enumerator.MoveNext())
			{
				LevelRes levelRes = (LevelRes)enumerator.Current.Value;
				if (levelRes.mLevelNumber == levelNumber)
				{
					verticalBackground = new Image(backDropContentManager.Load<Texture2D>(levelRes.mPath + "v"));
					horizontalBackground = new Image(backDropContentManager.Load<Texture2D>(levelRes.mPath + "h"));
					loadedBackdrop = levelNumber;
					return true;
				}
			}
			verticalBackground = null;
			horizontalBackground = null;
			return false;
		}

		private Texture2D LoadTextureFromStream(string filename, bool premultiply, ImageRes.TextureFormat format, SurfaceFormat lowMemorySurfaceFormat)
		{
			Texture2D texture2D = null;
			GraphicsDevice graphicsDevice = GlobalStaticVars.g.GraphicsDevice;
			using (Stream stream = TitleContainer.OpenStream("Content\\" + filename + "." + format.ToString()))
			{
				texture2D = Texture2D.FromStream(graphicsDevice, stream);
			}
			bool flag = false;
			if (!premultiply)
			{
				return texture2D;
			}
			lock (DrawLocker)
			{
				if (texture2D.Width * texture2D.Height < 4194304)
				{
					RenderTarget2D renderTarget2D = null;
					try
					{
						flag = true;
						lock (SexyAppBase.SplashScreenDrawLock)
						{
							renderTarget2D = ((!Main.DO_LOW_MEMORY_OPTIONS) ? new RenderTarget2D(graphicsDevice, texture2D.Width, texture2D.Height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents) : new RenderTarget2D(graphicsDevice, texture2D.Width, texture2D.Height, false, lowMemorySurfaceFormat, DepthFormat.None, 0, RenderTargetUsage.DiscardContents));
							graphicsDevice.SetRenderTarget(renderTarget2D);
							graphicsDevice.Clear(Color.Black);
							imageLoadSpritebatch.Begin(SpriteSortMode.Immediate, blendColorLoadState);
							imageLoadSpritebatch.Draw(texture2D, texture2D.Bounds, Color.White);
							imageLoadSpritebatch.End();
							imageLoadSpritebatch.Begin(SpriteSortMode.Immediate, imageLoadBlendAlpha);
							imageLoadSpritebatch.Draw(texture2D, texture2D.Bounds, Color.White);
							imageLoadSpritebatch.End();
							graphicsDevice.SetRenderTarget(null);
							texture2D.Dispose();
							return renderTarget2D;
						}
					}
					catch (Exception ex)
					{
						flag = false;
						string message = ex.Message;
						if (renderTarget2D != null)
						{
							renderTarget2D.Dispose();
						}
					}
				}
			}
			if (!flag)
			{
				Color[] array = new Color[texture2D.Width * texture2D.Height];
				texture2D.GetData(array);
				for (int i = 0; i < array.Length; i++)
				{
					PremultiplyPixel(ref array[i]);
				}
				if (Main.DO_LOW_MEMORY_OPTIONS)
				{
					Texture2D texture2D2 = new Texture2D(graphicsDevice, texture2D.Width, texture2D.Height, false, SurfaceFormat.Bgra4444);
					Bgra4444[] array2 = new Bgra4444[array.Length];
					for (int j = 0; j < array2.Length; j++)
					{
						array2[j] = new Bgra4444(array[j].ToVector4());
					}
					texture2D2.SetData(array2);
					texture2D.Dispose();
					texture2D = texture2D2;
				}
				else
				{
					texture2D.SetData(array);
				}
			}
			return texture2D;
		}

		private void PremultiplyPixel(ref Color c)
		{
			c.R = (byte)((float)(c.R * c.A) / 255f);
			c.G = (byte)((float)(c.G * c.A) / 255f);
			c.B = (byte)((float)(c.B * c.A) / 255f);
		}

		private bool DoLoadImage(ImageRes theRes)
		{
			Texture2D texture2D;
			try
			{
				string text = (!theRes.mLanguageSpecific) ? (Path.GetDirectoryName(theRes.mPath) + Constants.ImageSubPath + Path.GetFileName(theRes.mPath)) : (Path.GetDirectoryName(theRes.mPath) + Constants.ImageSubPath + Constants.LanguageSubDir + "/" + Path.GetFileName(theRes.mPath));
				texture2D = ((theRes.mFormat != 0) ? LoadTextureFromStream(text, true, theRes.mFormat, theRes.lowMemorySurfaceFormat) : GetContentManager(theRes).Load<Texture2D>(text));
			}
			catch (Exception ex)
			{
				return Fail("Failed to load image: " + theRes.mPath + ex.Message);
			}
			theRes.mImage = new Image(texture2D, 0, 0, texture2D.Width, texture2D.Height);
			if (theRes.mAnimInfo.mAnimType != 0)
			{
				theRes.mImage.mAnimInfo = new AnimInfo(theRes.mAnimInfo);
			}
			theRes.mImage.mNumRows = theRes.mRows;
			theRes.mImage.mNumCols = theRes.mCols;
			if (theRes.mUnloadGroup > 0)
			{
				unloadableResources.Add(theRes);
			}
			ResourceLoadedHook(theRes);
			return true;
		}

		private bool DoLoadFont(FontRes fontRes)
		{
			Font font = new Font();
			if (fontRes.mSysFont)
			{
				return Fail("SysFont not supported");
			}
			if (fontRes.mDefault)
			{
				font.AddLayer(arial);
				fontRes.mFont = font;
				return true;
			}
			XmlReader xmlReader = XmlReader.Create(TitleContainer.OpenStream(fontRes.mPath));
			xmlReader.Read();
			while (xmlReader.Read())
			{
				if (xmlReader.NodeType != XmlNodeType.Element)
				{
					continue;
				}
				if (xmlReader.Name == "Offsets")
				{
					Vector2 zero = Vector2.Zero;
					while (xmlReader.NodeType != XmlNodeType.EndElement || !(xmlReader.Name == "Offsets"))
					{
						if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "Offset")
						{
							string value = xmlReader["offsetX"];
							string value2 = xmlReader["offsetY"];
							char c = xmlReader["character"].ToCharArray()[0];
							if (!string.IsNullOrEmpty(value))
							{
								zero.X = Convert.ToInt32(value);
							}
							if (!string.IsNullOrEmpty(value2))
							{
								zero.Y = Convert.ToInt32(value2);
							}
							font.AddCharacterOffset(c, zero);
						}
						xmlReader.Read();
					}
				}
				if (xmlReader.Name == "Layers")
				{
					string value3 = xmlReader["magic"];
					if (!string.IsNullOrEmpty(value3))
					{
						font.characterOffsetMagic = Convert.ToInt32(value3);
					}
					else
					{
						font.characterOffsetMagic = 0;
					}
					Vector2 zero2 = Vector2.Zero;
					while (xmlReader.NodeType != XmlNodeType.EndElement || !(xmlReader.Name == "Layers"))
					{
						if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "Layer")
						{
							if (!xmlReader.HasAttributes)
							{
								zero2 = Vector2.Zero;
							}
							else
							{
								string value4 = xmlReader["xOffset"];
								string value5 = xmlReader["yOffset"];
								if (!string.IsNullOrEmpty(value4))
								{
									zero2.X = Convert.ToInt32(value4);
								}
								if (!string.IsNullOrEmpty(value5))
								{
									zero2.Y = Convert.ToInt32(value5);
								}
							}
						}
						if (xmlReader.NodeType == XmlNodeType.Text)
						{
							string value6 = xmlReader.Value;
							font.AddLayer(mContentManager.Load<SpriteFont>(value6), zero2);
						}
						xmlReader.Read();
					}
				}
				if (xmlReader.Name == "Ascent")
				{
					xmlReader.Read();
					font.mAscent = -1 * Convert.ToInt32(xmlReader.Value);
				}
				if (xmlReader.Name == "Height")
				{
					xmlReader.Read();
					font.mHeight = -1 * Convert.ToInt32(xmlReader.Value);
				}
				if (xmlReader.Name == "SpaceChar")
				{
					xmlReader.Read();
					font.SpaceChar = xmlReader.Value;
				}
				if (xmlReader.Name == "StringWidthCachingEnabled")
				{
					xmlReader.Read();
					font.StringWidthCachingEnabled = Convert.ToBoolean(xmlReader.Value);
				}
			}
			fontRes.mFont = font;
			ResourceLoadedHook(fontRes);
			return true;
		}

		private bool DoLoadSound(SoundRes soundRes)
		{
			int freeSoundId = mApp.mSoundManager.GetFreeSoundId();
			if (freeSoundId < 0)
			{
				return Fail("Out of free sound ids");
			}
			if (!mApp.mSoundManager.LoadSound((uint)freeSoundId, soundRes.mPath))
			{
				return Fail("Failed to load sound: " + soundRes.mPath);
			}
			if (soundRes.mVolume >= 0.0)
			{
				mApp.mSoundManager.SetBaseVolume((uint)freeSoundId, soundRes.mVolume);
			}
			if (soundRes.mPanning != 0)
			{
				mApp.mSoundManager.SetBasePan((uint)freeSoundId, soundRes.mPanning);
			}
			soundRes.mSoundId = freeSoundId;
			ResourceLoadedHook(soundRes);
			return true;
		}

		private bool DoLoadMusic(MusicRes musicRes)
		{
			int freeMusicId = mApp.mMusicInterface.GetFreeMusicId();
			if (freeMusicId < 0)
			{
				return Fail("Out of free song ids");
			}
			if (!mApp.mMusicInterface.LoadMusic(freeMusicId, musicRes.mPath))
			{
				return Fail("Failed to load song: " + musicRes.mPath);
			}
			musicRes.mSongId = freeMusicId;
			ResourceLoadedHook(musicRes);
			return true;
		}

		private bool DoLoadReanim(ref ReanimRes reanimRes)
		{
			ReanimRes reanimRes2 = reanimRes;
			if (!LoadReanimation(reanimRes2.mPath, ref reanimRes2.mReanim))
			{
				return Fail("Failed to load reanim: " + reanimRes2.mPath);
			}
			ResourceLoadedHook(reanimRes);
			return true;
		}

		private bool DoLoadParticle(ref ParticleRes particleRes)
		{
			ParticleRes particleRes2 = particleRes;
			if (!LoadParticle(particleRes2.mPath, ref particleRes2.mParticle))
			{
				return Fail("Failed to load reanim: " + particleRes2.mPath);
			}
			ResourceLoadedHook(particleRes);
			return true;
		}

		private bool DoLoadTrail(ref TrailRes trailRes)
		{
			TrailRes trailRes2 = trailRes;
			if (!LoadTrail(trailRes2.mPath, ref trailRes2.mTrail))
			{
				return Fail("Failed to load reanim: " + trailRes2.mPath);
			}
			ResourceLoadedHook(trailRes);
			return true;
		}

		private void ResourceLoadedHook(BaseRes theRes)
		{
		}

		private bool Fail(string theErrorText)
		{
			if (!mHasFailed)
			{
				mHasFailed = true;
				if (mXMLParser == null)
				{
					mError = theErrorText;
					return false;
				}
				int currentLineNum = mXMLParser.GetCurrentLineNum();
				mError = theErrorText;
				if (currentLineNum > 0)
				{
					mError = mError + " on Line " + currentLineNum.ToString();
				}
				if (!string.IsNullOrEmpty(mXMLParser.GetFileName()))
				{
					mError = mError + " in File '" + mXMLParser.GetFileName() + "'";
				}
			}
			return false;
		}

		public bool TodLoadResources(string theGroup)
		{
			return TodLoadResources(theGroup, false);
		}

		public bool TodLoadResources(string theGroup, bool doUnpackAtlasImages)
		{
			if (IsGroupLoaded(theGroup))
			{
				return true;
			}
			PerfTimer perfTimer = default(PerfTimer);
			perfTimer.Start();
			StartLoadResources(theGroup);
			while (!GlobalStaticVars.gSexyAppBase.mShutdown && LoadNextResource())
			{
			}
			if (GlobalStaticVars.gSexyAppBase.mShutdown)
			{
				return false;
			}
			if (HadError())
			{
				GlobalStaticVars.gSexyAppBase.ShowResourceError(true);
				return false;
			}
			mLoadedGroups.Add(theGroup);
			Math.Max((int)perfTimer.GetDuration(), 0);
			return true;
		}
	}
}
