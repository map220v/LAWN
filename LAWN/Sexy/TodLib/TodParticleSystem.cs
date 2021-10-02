using System.Collections.Generic;

namespace Sexy.TodLib
{
	internal class TodParticleSystem
	{
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

		public static TodParticleSystem GetNewTodParticleSystem()
		{
			if (unusedObjects.Count > 0)
			{
				return unusedObjects.Pop();
			}
			return new TodParticleSystem();
		}

		public void PrepareForReuse()
		{
			Reset();
			unusedObjects.Push(this);
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
			int num = 0;
			while (true)
			{
				if (num >= theDefinition.mEmitterDefCount)
				{
					return;
				}
				TodEmitterDefinition todEmitterDefinition = theDefinition.mEmitterDefs[num];
				if (!Definition.FloatTrackIsSet(ref todEmitterDefinition.mCrossFadeDuration))
				{
					if (TodCommon.TestBit((uint)todEmitterDefinition.mParticleFlags, 7) && mParticleHolder.IsOverLoaded())
					{
						break;
					}
					TodParticleEmitter newTodParticleEmitter = TodParticleEmitter.GetNewTodParticleEmitter();
					newTodParticleEmitter.mActive = true;
					mParticleHolder.mEmitters.Add(newTodParticleEmitter);
					newTodParticleEmitter.TodEmitterInitialize(theX, theY, this, todEmitterDefinition);
					TodParticleEmitter item = newTodParticleEmitter;
					mEmitterList.Add(item);
				}
				num++;
			}
			ParticleSystemDie();
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
			List<TodParticleEmitter>.Enumerator enumerator = mEmitterList.GetEnumerator();
			while (enumerator.MoveNext())
			{
				TodParticleEmitter current = enumerator.Current;
				TodParticleEmitter todParticleEmitter = current;
				todParticleEmitter.Update();
				todParticleEmitter.Update();
				todParticleEmitter.Update();
				if (Definition.FloatTrackIsSet(ref todParticleEmitter.mEmitterDef.mCrossFadeDuration))
				{
					if (todParticleEmitter.mParticleList.Count > 0)
					{
						flag = true;
					}
				}
				else if (!todParticleEmitter.mDead)
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
			foreach (TodParticleEmitter mEmitter in mEmitterList)
			{
				mEmitter.Draw(g, doScale);
			}
		}

		public void SystemMove(float theX, float theY)
		{
			List<TodParticleEmitter>.Enumerator enumerator = mEmitterList.GetEnumerator();
			while (enumerator.MoveNext())
			{
				TodParticleEmitter current = enumerator.Current;
				TodParticleEmitter todParticleEmitter = current;
				todParticleEmitter.SystemMove(theX, theY);
			}
		}

		public void OverrideColor(string theEmitterName, SexyColor theColor)
		{
			List<TodParticleEmitter>.Enumerator enumerator = mEmitterList.GetEnumerator();
			while (enumerator.MoveNext())
			{
				TodParticleEmitter current = enumerator.Current;
				TodParticleEmitter todParticleEmitter = current;
				if (theEmitterName == null || string.Compare(theEmitterName, todParticleEmitter.mEmitterDef.mName) == 0)
				{
					todParticleEmitter.mColorOverride = theColor;
				}
			}
		}

		public void OverrideExtraAdditiveDraw(string theEmitterName, bool theEnableExtraAdditiveDraw)
		{
			List<TodParticleEmitter>.Enumerator enumerator = mEmitterList.GetEnumerator();
			while (enumerator.MoveNext())
			{
				TodParticleEmitter current = enumerator.Current;
				TodParticleEmitter todParticleEmitter = current;
				if (theEmitterName == null || string.Compare(theEmitterName, todParticleEmitter.mEmitterDef.mName) == 0)
				{
					todParticleEmitter.mExtraAdditiveDrawOverride = theEnableExtraAdditiveDraw;
				}
			}
		}

		public void OverrideImage(string theEmitterName, Image theImage)
		{
			List<TodParticleEmitter>.Enumerator enumerator = mEmitterList.GetEnumerator();
			while (enumerator.MoveNext())
			{
				TodParticleEmitter current = enumerator.Current;
				TodParticleEmitter todParticleEmitter = current;
				if (theEmitterName == null || string.Compare(theEmitterName, todParticleEmitter.mEmitterDef.mName) == 0)
				{
					todParticleEmitter.mImageOverride = theImage;
				}
			}
		}

		public void OverrideFrame(string theEmitterName, int theFrame)
		{
			List<TodParticleEmitter>.Enumerator enumerator = mEmitterList.GetEnumerator();
			while (enumerator.MoveNext())
			{
				TodParticleEmitter current = enumerator.Current;
				TodParticleEmitter todParticleEmitter = current;
				if (theEmitterName == null || string.Compare(theEmitterName, todParticleEmitter.mEmitterDef.mName) == 0)
				{
					todParticleEmitter.mFrameOverride = theFrame;
				}
			}
		}

		public void OverrideScale(string theEmitterName, float theScale)
		{
			List<TodParticleEmitter>.Enumerator enumerator = mEmitterList.GetEnumerator();
			while (enumerator.MoveNext())
			{
				TodParticleEmitter current = enumerator.Current;
				TodParticleEmitter todParticleEmitter = current;
				if (theEmitterName == null || string.Compare(theEmitterName, todParticleEmitter.mEmitterDef.mName) == 0)
				{
					todParticleEmitter.mScaleOverride = theScale;
				}
			}
		}

		public void CrossFade(string theCrossFadeEmitter)
		{
			TodEmitterDefinition todEmitterDefinition = FindEmitterDefByName(theCrossFadeEmitter);
			if (todEmitterDefinition == null)
			{
				Debug.OutputDebug(Common.StrFormat_("Can't find cross fade emitter: {0}\n", theCrossFadeEmitter));
				return;
			}
			if (!Definition.FloatTrackIsSet(ref todEmitterDefinition.mCrossFadeDuration))
			{
				Debug.OutputDebug(Common.StrFormat_("Can't cross fade without duration set: {0}\n", theCrossFadeEmitter));
				return;
			}
			if (mParticleHolder.mEmitters.Count + mEmitterList.Count > mParticleHolder.mEmitters.Capacity)
			{
				Debug.OutputDebug("Too many emitters to cross fade\n");
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
					TodParticleEmitter item = newTodParticleEmitter;
					mEmitterList.Add(item);
					todParticleEmitter2.CrossFadeEmitter(newTodParticleEmitter);
				}
			}
		}

		public TodParticleEmitter FindEmitterByName(string theEmitterName)
		{
			List<TodParticleEmitter>.Enumerator enumerator = mEmitterList.GetEnumerator();
			while (enumerator.MoveNext())
			{
				TodParticleEmitter current = enumerator.Current;
				TodParticleEmitter todParticleEmitter = current;
				if (string.Compare(todParticleEmitter.mEmitterDef.mName, theEmitterName) == 0)
				{
					return todParticleEmitter;
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
	}
}
