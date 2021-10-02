namespace Sexy.TodLib
{
	internal class EffectSystem
	{
		public static EffectSystem gEffectSystem;

		public TodParticleHolder mParticleHolder;

		public TrailHolder mTrailHolder;

		public ReanimationHolder mReanimationHolder;

		public AttachmentHolder mAttachmentHolder;

		public EffectSystem()
		{
			mParticleHolder = null;
			mTrailHolder = null;
			mReanimationHolder = null;
			mAttachmentHolder = null;
		}

		public void Dispose()
		{
		}

		public void EffectSystemInitialize()
		{
			Debug.ASSERT(gEffectSystem == null);
			Debug.ASSERT(mParticleHolder == null && mTrailHolder == null && mReanimationHolder == null && mAttachmentHolder == null);
			gEffectSystem = this;
			mParticleHolder = new TodParticleHolder();
			mTrailHolder = new TrailHolder();
			mReanimationHolder = new ReanimationHolder();
			mAttachmentHolder = new AttachmentHolder();
			mParticleHolder.InitializeHolder();
			mTrailHolder.InitializeHolder();
			mReanimationHolder.InitializeHolder();
			mAttachmentHolder.InitializeHolder();
		}

		public void EffectSystemDispose()
		{
			if (mParticleHolder != null)
			{
				mParticleHolder.DisposeHolder();
				mParticleHolder.Dispose();
				mParticleHolder = null;
			}
			if (mTrailHolder != null)
			{
				mTrailHolder.DisposeHolder();
				mTrailHolder.Dispose();
				mTrailHolder = null;
			}
			if (mReanimationHolder != null)
			{
				mReanimationHolder.DisposeHolder();
				mReanimationHolder = null;
			}
			if (mAttachmentHolder != null)
			{
				mAttachmentHolder.DisposeHolder();
				mAttachmentHolder.Dispose();
				mAttachmentHolder = null;
			}
			gEffectSystem = null;
		}

		public void EffectSystemFreeAll()
		{
			mParticleHolder.mParticleSystems.Clear();
			for (int i = 0; i < mParticleHolder.mEmitters.Count; i++)
			{
				mParticleHolder.mEmitters[i].PrepareForReuse();
			}
			mParticleHolder.mEmitters.Clear();
			mParticleHolder.mParticles.Clear();
			mTrailHolder.mTrails.Clear();
			for (int j = 0; j < mReanimationHolder.mReanimations.Count; j++)
			{
				mReanimationHolder.mReanimations[j].PrepareForReuse();
			}
			mReanimationHolder.mReanimations.Clear();
			mAttachmentHolder.mAttachments.Clear();
		}

		public void ProcessDeleteQueue()
		{
			for (int num = mParticleHolder.mParticleSystems.Count - 1; num >= 0; num--)
			{
				if (mParticleHolder.mParticleSystems[num].mDead)
				{
					mParticleHolder.mParticleSystems[num].ParticleSystemDie();
					mParticleHolder.mParticleSystems[num].PrepareForReuse();
					mParticleHolder.mParticleSystems.RemoveAt(num);
				}
			}
			for (int i = 0; i < mReanimationHolder.mReanimations.Count; i++)
			{
				Reanimation reanimation = mReanimationHolder.mReanimations[i];
				if (reanimation.mDead)
				{
					mReanimationHolder.mReanimations[i].PrepareForReuse();
					mReanimationHolder.mReanimations.RemoveAt(i);
					i--;
				}
			}
			for (int j = 0; j < mTrailHolder.mTrails.Count; j++)
			{
				Trail trail = mTrailHolder.mTrails[j];
				if (trail.mDead)
				{
					mTrailHolder.mTrails.RemoveAt(j);
					j--;
				}
			}
			for (int num2 = mAttachmentHolder.mAttachments.Count - 1; num2 >= 0; num2--)
			{
				if (mAttachmentHolder.mAttachments[num2] == null || mAttachmentHolder.mAttachments[num2].mDead)
				{
					mAttachmentHolder.mAttachments[num2].PrepareForReuse();
					mAttachmentHolder.mAttachments.RemoveAt(num2);
				}
			}
		}

		public void Update()
		{
			foreach (TodParticleSystem mParticleSystem in mParticleHolder.mParticleSystems)
			{
				if (!mParticleSystem.mIsAttachment)
				{
					mParticleSystem.Update();
				}
			}
			foreach (Reanimation mReanimation in mReanimationHolder.mReanimations)
			{
				if (mReanimation != null && !mReanimation.mIsAttachment)
				{
					mReanimation.Update();
				}
			}
			foreach (Trail mTrail in mTrailHolder.mTrails)
			{
				if (!mTrail.mIsAttachment)
				{
					mTrail.Update();
				}
			}
		}
	}
}
