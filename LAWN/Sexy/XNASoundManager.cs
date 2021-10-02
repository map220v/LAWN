using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Sexy
{
	internal class XNASoundManager : SoundManager
	{
		private const int ACTIVE_SOUNDS_LIMIT = 16;

		private const float SOUND_LIMIT = 0.65f;

		private bool mEnabled;

		private ContentManager mContent;

		private XNASoundEntry[] mSounds = new XNASoundEntry[XNASoundConstants.MAX_SOUNDS];

		private List<XNASoundInstance> mInstances;

		public XNASoundManager(SexyAppBase theApp)
		{
			mEnabled = false;
			mContent = theApp.mContentManager;
			mInstances = new List<XNASoundInstance>();
			for (int i = 0; i < XNASoundConstants.MAX_SOUNDS; i++)
			{
				mSounds[i] = null;
			}
		}

		public override void Release()
		{
			ReleaseSounds();
			for (int i = 0; i < mInstances.Count; i++)
			{
				mInstances[i].Release();
			}
		}

		public override void Enable(bool enable)
		{
			mEnabled = enable;
		}

		public override bool Initialized()
		{
			return true;
		}

		public override bool LoadSound(uint theSfxID, string theFilename)
		{
			SoundEffect mSound = mContent.Load<SoundEffect>(theFilename);
			XNASoundEntry xNASoundEntry = new XNASoundEntry();
			xNASoundEntry.mSound = mSound;
			mSounds[theSfxID] = xNASoundEntry;
			return true;
		}

		public override int LoadSound(string theFilename)
		{
			for (int i = 0; i < XNASoundConstants.MAX_SOUNDS; i++)
			{
				if (mInstances[i] == null)
				{
					if (LoadSound((uint)i, theFilename))
					{
						return i;
					}
					return -1;
				}
			}
			return -1;
		}

		public override void ReleaseSound(uint theSfxID)
		{
			mSounds[theSfxID].Dispose();
			mSounds[theSfxID] = null;
		}

		public override void SetVolume(double theVolume)
		{
			SetMasterVolume(theVolume);
		}

		public override bool SetBaseVolume(uint theSfxID, double theBaseVolume)
		{
			if (theBaseVolume < 0.0 || theBaseVolume > 1.0)
			{
				return false;
			}
			mSounds[theSfxID].mBaseVolume = (float)theBaseVolume;
			for (int i = 0; i < mInstances.Count; i++)
			{
				if (mInstances[i].SoundId == theSfxID)
				{
					mInstances[i].SetBaseVolume(mSounds[theSfxID].mBaseVolume);
				}
			}
			return true;
		}

		public override bool SetBasePan(uint theSfxID, int theBasePan)
		{
			if (theBasePan < -100 || theBasePan > 100)
			{
				return false;
			}
			mSounds[theSfxID].mBasePan = (float)theBasePan / 100f;
			for (int i = 0; i < mInstances.Count; i++)
			{
				if (mInstances[i].SoundId == theSfxID)
				{
					mInstances[i].SetBasePan(theBasePan);
				}
			}
			return true;
		}

		public override SoundInstance GetSoundInstance(uint theSfxID)
		{
			if (mInstances.Count >= 16)
			{
				return null;
			}
			SoundEffectInstance instance = mSounds[theSfxID].mSound.CreateInstance();
			XNASoundInstance newXNASoundInstance = XNASoundInstance.GetNewXNASoundInstance(theSfxID, instance);
			mInstances.Add(newXNASoundInstance);
			return newXNASoundInstance;
		}

		public override void ReleaseSounds()
		{
			for (uint num = 0u; num < XNASoundConstants.MAX_SOUNDS; num++)
			{
				ReleaseSound(num);
			}
		}

		public override void ReleaseChannels()
		{
		}

		public override double GetMasterVolume()
		{
			return SoundEffect.MasterVolume;
		}

		public override void SetMasterVolume(double theVolume)
		{
			SoundEffect.MasterVolume = (float)theVolume * 0.65f;
		}

		public override void Flush()
		{
		}

		public override void StopAllSounds()
		{
			for (int i = 0; i < mInstances.Count; i++)
			{
				mInstances[i].Stop();
			}
		}

		public override int GetFreeSoundId()
		{
			for (int i = 0; i < XNASoundConstants.MAX_SOUNDS; i++)
			{
				if (mSounds[i] == null)
				{
					return i;
				}
			}
			return -1;
		}

		public override int GetNumSounds()
		{
			int num = 0;
			for (int i = 0; i < XNASoundConstants.MAX_SOUNDS; i++)
			{
				if (mSounds[i] != null)
				{
					num++;
				}
			}
			return num;
		}

		public override void Update()
		{
			for (int num = mInstances.Count - 1; num >= 0; num--)
			{
				if (mInstances[num].IsReleased())
				{
					mInstances[num].PrepareForReuse();
					mInstances.RemoveAt(num);
				}
				else if (mInstances[num].IsDormant())
				{
					if (!mInstances[num].IsReleased())
					{
						mInstances[num].Release();
					}
					mInstances[num].PrepareForReuse();
					mInstances.RemoveAt(num);
				}
			}
		}
	}
}
