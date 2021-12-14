﻿using System;
using System.Collections.Generic;

namespace Sexy.TodLib
{
	public/*internal*/ class TodParticleSystem
	{
		public static TodParticleSystem GetNewTodParticleSystem()
		{
			TodParticleSystem result;
			if (TodParticleSystem.unusedObjects.Count > 0)
			{
				result = TodParticleSystem.unusedObjects.Pop();
			}
			else
			{
				result = new TodParticleSystem();
			}
			return result;
		}

		public void PrepareForReuse()
		{
			Reset();
			TodParticleSystem.unusedObjects.Push(this);
		}

		private TodParticleSystem()
		{
			Reset();
		}

		private void Reset()
		{
			mEffectType = ParticleEffect.PARTICLE_NONE;
			for (int i = 0; i < mEmitterList.Count; i++)
			{
				mEmitterList[i].PrepareForReuse();
			}
			mEmitterList.Clear();
			mParticleDef = null;
			mParticleHolder = null;
			mDead = false;
			mDontUpdate = false;
			mIsAttachment = false;
			mRenderOrder = 0;
			mActive = false;
		}

		public void Dispose()
		{
			ParticleSystemDie();
		}

		public void TodParticleInitializeFromDef(float theX, float theY, int theRenderOrder, TodParticleDefinition theDefinition, ParticleEffect theEffectType)
		{
			mParticleDef = theDefinition;
			mRenderOrder = theRenderOrder;
			mEffectType = theEffectType;
			for (int i = 0; i < theDefinition.mEmitterDefCount; i++)
			{
				TodEmitterDefinition todEmitterDefinition = theDefinition.mEmitterDefs[i];
				if (!Definition.FloatTrackIsSet(ref todEmitterDefinition.mCrossFadeDuration))
				{
					if (TodCommon.TestBit((uint)todEmitterDefinition.mParticleFlags, 7) && mParticleHolder.IsOverLoaded())
					{
						ParticleSystemDie();
						return;
					}
					TodParticleEmitter newTodParticleEmitter = TodParticleEmitter.GetNewTodParticleEmitter();
					newTodParticleEmitter.mActive = true;
					mParticleHolder.mEmitters.Add(newTodParticleEmitter);
					newTodParticleEmitter.TodEmitterInitialize(theX, theY, this, todEmitterDefinition);
					TodParticleEmitter todParticleEmitter = newTodParticleEmitter;
					mEmitterList.Add(todParticleEmitter);
				}
			}
		}

		public void ParticleSystemDie()
		{
			for (int i = 0; i < mEmitterList.Count; i++)
			{
				TodParticleEmitter todParticleEmitter = mEmitterList[i];
				todParticleEmitter.DeleteAll();
				todParticleEmitter.PrepareForReuse();
				mParticleHolder.mEmitters.Remove(todParticleEmitter);
			}
			mEmitterList.Clear();
			mDead = true;
		}

		public void Update()
		{
			if (mDontUpdate)
			{
				return;
			}
			bool flag = false;
			foreach (TodParticleEmitter todParticleEmitter in mEmitterList)
			{
				TodParticleEmitter todParticleEmitter2 = todParticleEmitter;
				todParticleEmitter2.Update();
				todParticleEmitter2.Update();
				todParticleEmitter2.Update();
				if (Definition.FloatTrackIsSet(ref todParticleEmitter2.mEmitterDef.mCrossFadeDuration))
				{
					if (todParticleEmitter2.mParticleList.Count > 0)
					{
						flag = true;
					}
				}
				else if (!todParticleEmitter2.mDead)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				mDead = true;
			}
		}

		public void Draw(Graphics g, bool doScale)
		{
			g.SetDrawMode(Graphics.DrawMode.DRAWMODE_NORMAL);
			foreach (TodParticleEmitter todParticleEmitter in mEmitterList)
			{
				todParticleEmitter.Draw(g, doScale);
			}
		}

		public void SystemMove(float theX, float theY)
		{
			foreach (TodParticleEmitter todParticleEmitter in mEmitterList)
			{
				TodParticleEmitter todParticleEmitter2 = todParticleEmitter;
				todParticleEmitter2.SystemMove(theX, theY);
			}
		}

		public void OverrideColor(string theEmitterName, SexyColor theColor)
		{
			foreach (TodParticleEmitter todParticleEmitter in mEmitterList)
			{
				TodParticleEmitter todParticleEmitter2 = todParticleEmitter;
				if (theEmitterName == null || string.Compare(theEmitterName, todParticleEmitter2.mEmitterDef.mName) == 0)
				{
					todParticleEmitter2.mColorOverride = theColor;
				}
			}
		}

		public void OverrideExtraAdditiveDraw(string theEmitterName, bool theEnableExtraAdditiveDraw)
		{
			foreach (TodParticleEmitter todParticleEmitter in mEmitterList)
			{
				TodParticleEmitter todParticleEmitter2 = todParticleEmitter;
				if (theEmitterName == null || string.Compare(theEmitterName, todParticleEmitter2.mEmitterDef.mName) == 0)
				{
					todParticleEmitter2.mExtraAdditiveDrawOverride = theEnableExtraAdditiveDraw;
				}
			}
		}

		public void OverrideImage(string theEmitterName, Image theImage)
		{
			foreach (TodParticleEmitter todParticleEmitter in mEmitterList)
			{
				TodParticleEmitter todParticleEmitter2 = todParticleEmitter;
				if (theEmitterName == null || string.Compare(theEmitterName, todParticleEmitter2.mEmitterDef.mName) == 0)
				{
					todParticleEmitter2.mImageOverride = theImage;
				}
			}
		}

		public void OverrideFrame(string theEmitterName, int theFrame)
		{
			foreach (TodParticleEmitter todParticleEmitter in mEmitterList)
			{
				TodParticleEmitter todParticleEmitter2 = todParticleEmitter;
				if (theEmitterName == null || string.Compare(theEmitterName, todParticleEmitter2.mEmitterDef.mName) == 0)
				{
					todParticleEmitter2.mFrameOverride = theFrame;
				}
			}
		}

		public void OverrideScale(string theEmitterName, float theScale)
		{
			foreach (TodParticleEmitter todParticleEmitter in mEmitterList)
			{
				TodParticleEmitter todParticleEmitter2 = todParticleEmitter;
				if (theEmitterName == null || string.Compare(theEmitterName, todParticleEmitter2.mEmitterDef.mName) == 0)
				{
					todParticleEmitter2.mScaleOverride = theScale;
				}
			}
		}

		public void CrossFade(string theCrossFadeEmitter)
		{
			TodEmitterDefinition todEmitterDefinition = FindEmitterDefByName(theCrossFadeEmitter);
			if (todEmitterDefinition == null)
			{
				Debug.OutputDebug<string>(Common.StrFormat_("Can't find cross fade emitter: {0}\n", theCrossFadeEmitter));
				return;
			}
			if (!Definition.FloatTrackIsSet(ref todEmitterDefinition.mCrossFadeDuration))
			{
				Debug.OutputDebug<string>(Common.StrFormat_("Can't cross fade without duration set: {0}\n", theCrossFadeEmitter));
				return;
			}
			if (mParticleHolder.mEmitters.Count + mEmitterList.Count > mParticleHolder.mEmitters.Capacity)
			{
				Debug.OutputDebug<string>("Too many emitters to cross fade\n");
				ParticleSystemDie();
				return;
			}
			for (int i = 0; i < mEmitterList.Count; i++)
			{
				TodParticleEmitter todParticleEmitter = mEmitterList[i];
				TodParticleEmitter todParticleEmitter2 = todParticleEmitter;
				if (todParticleEmitter2.mEmitterDef != todEmitterDefinition)
				{
					TodParticleEmitter newTodParticleEmitter = TodParticleEmitter.GetNewTodParticleEmitter();
					newTodParticleEmitter.TodEmitterInitialize(todParticleEmitter2.mSystemCenter.x, todParticleEmitter2.mSystemCenter.y, this, todEmitterDefinition);
					newTodParticleEmitter.mActive = true;
					mParticleHolder.mEmitters.Add(newTodParticleEmitter);
					TodParticleEmitter todParticleEmitter3 = newTodParticleEmitter;
					mEmitterList.Add(todParticleEmitter3);
					todParticleEmitter2.CrossFadeEmitter(newTodParticleEmitter);
				}
			}
		}

		public TodParticleEmitter FindEmitterByName(string theEmitterName)
		{
			foreach (TodParticleEmitter todParticleEmitter in mEmitterList)
			{
				TodParticleEmitter todParticleEmitter2 = todParticleEmitter;
				if (string.Compare(todParticleEmitter2.mEmitterDef.mName, theEmitterName) == 0)
				{
					return todParticleEmitter2;
				}
			}
			return null;
		}

		public TodEmitterDefinition FindEmitterDefByName(string theEmitterName)
		{
			for (int i = 0; i < mParticleDef.mEmitterDefCount; i++)
			{
				TodEmitterDefinition todEmitterDefinition = mParticleDef.mEmitterDefs[i];
				if (string.Compare(todEmitterDefinition.mName, theEmitterName) == 0)
				{
					return todEmitterDefinition;
				}
			}
			return null;
		}

		public void SaveToFile(Sexy.Buffer b)
		{
			b.WriteLong((int)mEffectType);
			b.WriteLong(mEmitterList.Count);
			foreach (TodParticleEmitter aEmitter in mEmitterList)
            {
				b.WriteLong(mParticleHolder.mEmitters.FindIndex((TodParticleEmitter tpe) => tpe == aEmitter));
            }
			b.WriteBoolean(mDead);
			b.WriteBoolean(mIsAttachment);
			b.WriteLong(mRenderOrder);
			b.WriteBoolean(mDontUpdate);
			b.WriteBoolean(mActive);
		}

		private int ltEmitterCount;
		private List<int> ltEmitterIdList = new List<int>();

		public void LoadFromFile(Sexy.Buffer b)
        {
			mEffectType = (ParticleEffect)b.ReadLong();
			ltEmitterCount = b.ReadLong();
			ltEmitterIdList.Clear();
			for (int i = 0; i < ltEmitterCount; i++)
            {
				ltEmitterIdList.Add(b.ReadLong());
			}
			mDead = b.ReadBoolean();
			mIsAttachment = b.ReadBoolean();
			mRenderOrder = b.ReadLong();
			mDontUpdate = b.ReadBoolean();
			mActive = b.ReadBoolean();
		}

		public void LoadingComplete()
        {
			mParticleDef = TodParticleGlobal.gParticleDefArray[(int)mEffectType];
			mParticleHolder = EffectSystem.gEffectSystem.mParticleHolder;
			mEmitterList.Clear();
			for (int i = 0; i < ltEmitterCount; i++)
			{
				mEmitterList.Add(mParticleHolder.mEmitters[ltEmitterIdList[i]]);
			}
		}

		public ParticleEffect mEffectType = ParticleEffect.PARTICLE_NONE;

		public TodParticleDefinition mParticleDef;

		public TodParticleHolder mParticleHolder;

		public List<TodParticleEmitter> mEmitterList = new List<TodParticleEmitter>();

		public bool mDead;

		public bool mIsAttachment;

		public int mRenderOrder;

		public bool mDontUpdate;

		public bool mActive;

		private static Stack<TodParticleSystem> unusedObjects = new Stack<TodParticleSystem>(50);
	}
}