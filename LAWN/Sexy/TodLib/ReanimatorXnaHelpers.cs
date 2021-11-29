using System;
using System.Collections.Generic;

namespace Sexy.TodLib
{
	internal class ReanimatorXnaHelpers
	{
		public static float DEFAULT_FIELD_PLACEHOLDER = -99999f;

		public static short NO_BASE_POSE = -2;

		public static float EPSILON = 0.001f;

		public static int RENDER_GROUP_HIDDEN = -1;

		public static int RENDER_GROUP_NORMAL = 0;

		public static float SECONDS_PER_UPDATE = 0.033333f;

		public static ReanimatorDefinition[] gReanimatorDefArray = new ReanimatorDefinition[119];

		public static ReanimationParams[] gReanimationParamArray = null;

		public static int gReanimatorDefCount = 0;

		public static int gReanimationParamArraySize = 0;

		public static List<string> gReanimTrackIds = new List<string>();

		public static double mLoadingProgress;

		public static int mTotalResources = 118;

		public static int mLoadedResources;

		public static string ReanimatorTrackNameToId(string theName)
		{
			string text = Common.StringToLower(ref theName);
			int num = gReanimTrackIds.IndexOf(text);
			if (num == -1)
			{
				gReanimTrackIds.Add(text);
			}
			return text;
		}

		public static void ReanimationFillInMissingData(ref float thePrev, ref float theValue)
		{
			if (theValue == DEFAULT_FIELD_PLACEHOLDER)
			{
				theValue = thePrev;
			}
			else
			{
				thePrev = theValue;
			}
		}

		public static bool ReanimationLoadDefinition(string theFilename, ref ReanimatorDefinition theDefinition)
		{
			Reanimator reanimator = new Reanimator();
			theDefinition = reanimator.ParseReanimationFile("Content/" + theFilename + ".reanim");
			//TODO: Replace Atlas extractor with simple image loader
			theDefinition.ExtractImages();
			if (theDefinition == null)
			{
				return false;
			}
			for (int i = 0; i < theDefinition.mTrackCount; i++)
			{
				ReanimatorTrack reanimatorTrack = theDefinition.mTracks[i];
				if (reanimatorTrack.mName == "zombie_butter")
				{
					int num = 0;
					num++;
				}
				float thePrev = 0f;
				float thePrev2 = 0f;
				float thePrev3 = 0f;
				float thePrev4 = 0f;
				float thePrev5 = 1f;
				float thePrev6 = 1f;
				float thePrev7 = 0f;
				float thePrev8 = 1f;
				Image mImage = null;
				string mImageName = string.Empty;
				Font mFont = null;
				string mText = string.Empty;
				for (int j = 0; j < reanimatorTrack.mTransformCount; j++)
				{
					ReanimatorTransform reanimatorTransform = reanimatorTrack.mTransforms[j];
					ReanimationFillInMissingData(ref thePrev, ref reanimatorTransform.mTransX);
					ReanimationFillInMissingData(ref thePrev2, ref reanimatorTransform.mTransY);
					ReanimationFillInMissingData(ref thePrev3, ref reanimatorTransform.mSkewX);
					ReanimationFillInMissingData(ref thePrev4, ref reanimatorTransform.mSkewY);
					ReanimationFillInMissingData(ref thePrev5, ref reanimatorTransform.mScaleX);
					ReanimationFillInMissingData(ref thePrev6, ref reanimatorTransform.mScaleY);
					ReanimationFillInMissingData(ref thePrev7, ref reanimatorTransform.mFrame);
					ReanimationFillInMissingData(ref thePrev8, ref reanimatorTransform.mAlpha);
					reanimatorTransform.mSkewXCos = (float)Math.Cos(reanimatorTransform.mSkewX * (0f - TodCommon.DEG_TO_RAD));
					reanimatorTransform.mSkewXSin = (float)Math.Sin(reanimatorTransform.mSkewX * (0f - TodCommon.DEG_TO_RAD));
					reanimatorTransform.mSkewYCos = (float)Math.Cos(reanimatorTransform.mSkewY * (0f - TodCommon.DEG_TO_RAD));
					reanimatorTransform.mSkewYSin = (float)Math.Sin(reanimatorTransform.mSkewY * (0f - TodCommon.DEG_TO_RAD));
					if (reanimatorTransform.mImage == null)
					{
						reanimatorTransform.mImage = mImage;
						reanimatorTransform.mImageName = mImageName;
					}
					else
					{
						mImage = reanimatorTransform.mImage;
						mImageName = reanimatorTransform.mImageName;
					}
					if (reanimatorTransform.mFont == null)
					{
						reanimatorTransform.mFont = mFont;
					}
					else
					{
						mFont = reanimatorTransform.mFont;
					}
					if (string.IsNullOrEmpty(reanimatorTransform.mText))
					{
						reanimatorTransform.mText = mText;
					}
					else
					{
						mText = reanimatorTransform.mText;
					}
					reanimatorTrack.mTransforms[j] = reanimatorTransform;
				}
			}
			return true;
		}

		public static void ReanimationFreeDefinition(ref ReanimatorDefinition theDefinition)
		{
			if (theDefinition.mReanimAtlas != null)
			{
				theDefinition.mReanimAtlas.ReanimAtlasDispose();
				theDefinition.mReanimAtlas = null;
			}
			for (int i = 0; i < theDefinition.mTrackCount; i++)
			{
				ReanimatorTrack reanimatorTrack = theDefinition.mTracks[i];
				string b = null;
				for (int j = 0; j < reanimatorTrack.mTransformCount; j++)
				{
					ReanimatorTransform reanimatorTransform = reanimatorTrack.mTransforms[j];
					if (!string.IsNullOrEmpty(reanimatorTransform.mText) && reanimatorTransform.mText == b)
					{
						reanimatorTransform.mText = "";
					}
					else
					{
						b = reanimatorTransform.mText;
					}
					reanimatorTrack.mTransforms[j] = reanimatorTransform;
				}
			}
		}

		public static void ReanimatorLoadDefinitions(ref ReanimationParams[] theReanimationParamArray, int theReanimationParamArraySize)
		{
			gReanimationParamArraySize = theReanimationParamArraySize;
			gReanimationParamArray = theReanimationParamArray;
			gReanimatorDefCount = theReanimationParamArraySize;
			Array.Clear(gReanimatorDefArray, 0, gReanimatorDefArray.Length);
			for (int i = 0; i < gReanimationParamArraySize - 1; i++)
			{
				ReanimationParams reanimationParams = theReanimationParamArray[i];
				ReanimatorEnsureDefinitionLoaded(reanimationParams.mReanimationType, true);
				mLoadingProgress = (double)i / (double)(gReanimationParamArraySize - 1);
				mLoadedResources++;
			}
		}

		public static void ReanimatorFreeDefinitions()
		{
			for (int i = 0; i < gReanimatorDefCount; i++)
			{
				gReanimatorDefArray[i] = null;
			}
			gReanimatorDefArray = null;
			gReanimatorDefCount = 0;
			gReanimationParamArray = null;
			gReanimationParamArraySize = 0;
		}

		public static void ReanimatorEnsureDefinitionLoaded(ReanimationType theReanimType, bool theIsPreloading)
		{
			ReanimatorDefinition theDefinition = gReanimatorDefArray[(int)theReanimType];
			if (theDefinition == null || theDefinition.mTracks == null || theDefinition.mTrackCount == 0)
			{
				ReanimationParams reanimationParams = gReanimationParamArray[(int)theReanimType];
				if (!theIsPreloading || !GlobalStaticVars.gSexyAppBase.mShutdown)
				{
					PerfTimer perfTimer = default(PerfTimer);
					perfTimer.Start();
					ReanimationLoadDefinition(reanimationParams.mReanimFileName, ref theDefinition);
					int num = Math.Max((int)perfTimer.GetDuration(), 0);
					int num2 = 100;
					gReanimatorDefArray[(int)theReanimType] = theDefinition;
				}
			}
		}

		public static void ReanimationPreload(ReanimationType theReanimationType)
		{
		}

		public static void BlendTransform(out ReanimatorTransform theResult, ref ReanimatorTransform theTransform1, ref ReanimatorTransform theTransform2, float theBlendFactor)
		{
			theResult = ReanimatorTransform.GetNewReanimatorTransform();
			theResult.mTransX = TodCommon.FloatLerp(theTransform1.mTransX, theTransform2.mTransX, theBlendFactor);
			theResult.mTransY = TodCommon.FloatLerp(theTransform1.mTransY, theTransform2.mTransY, theBlendFactor);
			theResult.mScaleX = TodCommon.FloatLerp(theTransform1.mScaleX, theTransform2.mScaleX, theBlendFactor);
			theResult.mScaleY = TodCommon.FloatLerp(theTransform1.mScaleY, theTransform2.mScaleY, theBlendFactor);
			theResult.mAlpha = TodCommon.FloatLerp(theTransform1.mAlpha, theTransform2.mAlpha, theBlendFactor);
			float mSkewX = theTransform2.mSkewX;
			float mSkewY = theTransform2.mSkewY;
			while (mSkewX > theTransform1.mSkewX + 180f)
			{
				mSkewX -= 360f;
				mSkewX = theTransform1.mSkewX;
			}
			while (mSkewX < theTransform1.mSkewX - 180f)
			{
				mSkewX += 360f;
				mSkewX = theTransform1.mSkewX;
			}
			while (mSkewY > theTransform1.mSkewY + 180f)
			{
				mSkewY -= 360f;
				mSkewY = theTransform1.mSkewY;
			}
			while (mSkewY < theTransform1.mSkewY - 180f)
			{
				mSkewY += 360f;
				mSkewY = theTransform1.mSkewY;
			}
			theResult.mSkewX = TodCommon.FloatLerp(theTransform1.mSkewX, mSkewX, theBlendFactor);
			theResult.mSkewY = TodCommon.FloatLerp(theTransform1.mSkewY, mSkewY, theBlendFactor);
			theResult.mSkewXCos = (float)Math.Cos(theResult.mSkewX * (0f - TodCommon.DEG_TO_RAD));
			theResult.mSkewXSin = (float)Math.Sin(theResult.mSkewX * (0f - TodCommon.DEG_TO_RAD));
			theResult.mSkewYCos = (float)Math.Cos(theResult.mSkewY * (0f - TodCommon.DEG_TO_RAD));
			theResult.mSkewYSin = (float)Math.Sin(theResult.mSkewY * (0f - TodCommon.DEG_TO_RAD));
			theResult.mFrame = theTransform1.mFrame;
			theResult.mFont = theTransform1.mFont;
			theResult.mText = theTransform1.mText;
			theResult.mImage = theTransform1.mImage;
		}
	}
}
