using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Sexy.TodLib
{
	internal class Attachment
	{
		public AttachEffect[] mEffectArray = new AttachEffect[16];

		public int mNumEffects;

		public bool mDead;

		public bool mActive;

		public bool mUsesClipping;

		private static Stack<Attachment> unusedObjects = new Stack<Attachment>();

		private bool reused;

		public static void PreallocateMemory()
		{
			for (int i = 0; i < 500; i++)
			{
				new Attachment().PrepareForReuse();
			}
		}

		public static Attachment GetNewAttachment()
		{
			if (unusedObjects.Count > 0)
			{
				Attachment attachment = unusedObjects.Pop();
				attachment.Reset();
				attachment.reused = false;
				return attachment;
			}
			return new Attachment();
		}

		public void PrepareForReuse()
		{
			reused = true;
			mActive = false;
			unusedObjects.Push(this);
		}

		private Attachment()
		{
			Reset();
		}

		private void Reset()
		{
			mNumEffects = 0;
			mDead = false;
			mUsesClipping = false;
			for (int i = 0; i < mEffectArray.Length; i++)
			{
				if (mEffectArray[i] != null)
				{
					mEffectArray[i].PrepareForReuse();
					mEffectArray[i] = null;
				}
			}
			for (int j = 0; j < 16; j++)
			{
				mEffectArray[j] = AttachEffect.GetNewAttachEffect();
			}
		}

		public void Dispose()
		{
		}

		public void Update()
		{
			for (int i = 0; i < mNumEffects; i++)
			{
				AttachEffect attachEffect = mEffectArray[i];
				bool flag = false;
				switch (attachEffect.mEffectType)
				{
				case EffectType.EFFECT_REANIM:
				{
					List<Reanimation> mReanimation = EffectSystem.gEffectSystem.mReanimationHolder.mReanimations;
					Reanimation reanimation = (Reanimation)attachEffect.mEffectID;
					if (reanimation != null && reanimation.mActive && !reanimation.mDead)
					{
						reanimation.Update();
						if (!reanimation.mDead)
						{
							flag = true;
						}
					}
					break;
				}
				case EffectType.EFFECT_PARTICLE:
				{
					List<TodParticleSystem> mParticleSystems = EffectSystem.gEffectSystem.mParticleHolder.mParticleSystems;
					int num = mParticleSystems.IndexOf((TodParticleSystem)attachEffect.mEffectID);
					if (num == -1)
					{
						break;
					}
					TodParticleSystem todParticleSystem = mParticleSystems[num];
					if (todParticleSystem != null && !todParticleSystem.mDead)
					{
						todParticleSystem.Update();
						if (!todParticleSystem.mDead)
						{
							flag = true;
						}
					}
					break;
				}
				case EffectType.EFFECT_TRAIL:
				{
					List<Trail> mTrail = EffectSystem.gEffectSystem.mTrailHolder.mTrails;
					Trail trail = (Trail)attachEffect.mEffectID;
					if (trail != null && !trail.mDead)
					{
						trail.Update();
						if (!trail.mDead)
						{
							flag = true;
						}
					}
					break;
				}
				case EffectType.EFFECT_ATTACHMENT:
				{
					List<Attachment> mAttachment = EffectSystem.gEffectSystem.mAttachmentHolder.mAttachments;
					Attachment attachment = (Attachment)attachEffect.mEffectID;
					if (attachment.mActive)
					{
						attachment.Update();
						if (!attachment.mDead)
						{
							flag = true;
						}
					}
					break;
				}
				default:
					Debug.ASSERT(false);
					break;
				}
				if (flag)
				{
					continue;
				}
				int num2 = mNumEffects - i;
				if (num2 > 0)
				{
					for (int j = i; j < mEffectArray.Length - 1; j++)
					{
						if (j == i && mEffectArray[j] != null)
						{
							mEffectArray[j].PrepareForReuse();
						}
						mEffectArray[j] = mEffectArray[j + 1];
						if (j == mEffectArray.Length - 2)
						{
							mEffectArray[j + 1] = AttachEffect.GetNewAttachEffect();
						}
					}
					i--;
				}
				mNumEffects--;
			}
			if (mNumEffects == 0)
			{
				mDead = true;
			}
		}

		public void SetMatrix(ref SexyTransform2D theMatrix)
		{
			Debug.ASSERT(EffectSystem.gEffectSystem != null);
			for (int i = 0; i < mNumEffects; i++)
			{
				AttachEffect attachEffect = mEffectArray[i];
				SexyTransform2D sexyTransform2D = default(SexyTransform2D);
				sexyTransform2D.mMatrix = new Matrix
				{
					M11 = attachEffect.mOffset.mMatrix.M11 * theMatrix.mMatrix.M11 + attachEffect.mOffset.mMatrix.M12 * theMatrix.mMatrix.M21,
					M12 = attachEffect.mOffset.mMatrix.M11 * theMatrix.mMatrix.M12 + attachEffect.mOffset.mMatrix.M12 * theMatrix.mMatrix.M22,
					M13 = 0f,
					M14 = 0f,
					M21 = attachEffect.mOffset.mMatrix.M21 * theMatrix.mMatrix.M11 + attachEffect.mOffset.mMatrix.M22 * theMatrix.mMatrix.M21,
					M22 = attachEffect.mOffset.mMatrix.M21 * theMatrix.mMatrix.M12 + attachEffect.mOffset.mMatrix.M22 * theMatrix.mMatrix.M22,
					M23 = 0f,
					M24 = 0f,
					M31 = 0f,
					M32 = 0f,
					M33 = 1f,
					M34 = 0f,
					M41 = attachEffect.mOffset.mMatrix.M41 * theMatrix.mMatrix.M11 + attachEffect.mOffset.mMatrix.M42 * theMatrix.mMatrix.M21 + theMatrix.mMatrix.M41,
					M42 = attachEffect.mOffset.mMatrix.M41 * theMatrix.mMatrix.M12 + attachEffect.mOffset.mMatrix.M42 * theMatrix.mMatrix.M22 + theMatrix.mMatrix.M42,
					M43 = 0f,
					M44 = 1f
				};
				SexyTransform2D theMatrix2 = sexyTransform2D;
				switch (attachEffect.mEffectType)
				{
				case EffectType.EFFECT_PARTICLE:
				{
					List<TodParticleSystem> mParticleSystems = EffectSystem.gEffectSystem.mParticleHolder.mParticleSystems;
					TodParticleSystem todParticleSystem = null;
					if (mParticleSystems.Contains((TodParticleSystem)attachEffect.mEffectID))
					{
						todParticleSystem = (TodParticleSystem)attachEffect.mEffectID;
					}
					if (todParticleSystem != null)
					{
						todParticleSystem.SystemMove(theMatrix2.mMatrix.M41, theMatrix2.mMatrix.M42);
					}
					break;
				}
				case EffectType.EFFECT_TRAIL:
				{
					List<Trail> mTrail = EffectSystem.gEffectSystem.mTrailHolder.mTrails;
					Trail trail = (Trail)attachEffect.mEffectID;
					if (trail != null)
					{
						trail.mTrailCenter = new SexyVector2(theMatrix2.mMatrix.M41, theMatrix2.mMatrix.M42);
					}
					break;
				}
				case EffectType.EFFECT_REANIM:
				{
					List<Reanimation> mReanimation = EffectSystem.gEffectSystem.mReanimationHolder.mReanimations;
					Reanimation reanimation = (Reanimation)attachEffect.mEffectID;
					if (reanimation != null)
					{
						reanimation.mOverlayMatrix = theMatrix2;
					}
					break;
				}
				case EffectType.EFFECT_ATTACHMENT:
				{
					List<Attachment> mAttachments = EffectSystem.gEffectSystem.mAttachmentHolder.mAttachments;
					Attachment attachment = mAttachments[mAttachments.IndexOf((Attachment)attachEffect.mEffectID)];
					if (attachment != null)
					{
						attachment.SetMatrix(ref theMatrix2);
					}
					break;
				}
				}
			}
		}

		public void OverrideColor(SexyColor theColor)
		{
			Debug.ASSERT(EffectSystem.gEffectSystem != null);
			for (int i = 0; i < mNumEffects; i++)
			{
				AttachEffect attachEffect = mEffectArray[i];
				switch (attachEffect.mEffectType)
				{
				case EffectType.EFFECT_PARTICLE:
				{
					List<TodParticleSystem> mParticleSystem = EffectSystem.gEffectSystem.mParticleHolder.mParticleSystems;
					TodParticleSystem todParticleSystem = (TodParticleSystem)attachEffect.mEffectID;
					if (todParticleSystem != null)
					{
						todParticleSystem.OverrideColor("", theColor);
					}
					break;
				}
				case EffectType.EFFECT_REANIM:
				{
					List<Reanimation> mReanimation = EffectSystem.gEffectSystem.mReanimationHolder.mReanimations;
					Reanimation reanimation = (Reanimation)attachEffect.mEffectID;
					if (reanimation != null)
					{
						reanimation.mColorOverride = theColor;
					}
					break;
				}
				case EffectType.EFFECT_ATTACHMENT:
				{
					List<Attachment> mAttachment = EffectSystem.gEffectSystem.mAttachmentHolder.mAttachments;
					Attachment attachment = (Attachment)attachEffect.mEffectID;
					if (attachment != null)
					{
						attachment.OverrideColor(theColor);
					}
					break;
				}
				}
			}
		}

		public void OverrideScale(float theScale)
		{
			Debug.ASSERT(EffectSystem.gEffectSystem != null);
			for (int i = 0; i < mNumEffects; i++)
			{
				AttachEffect attachEffect = mEffectArray[i];
				switch (attachEffect.mEffectType)
				{
				case EffectType.EFFECT_PARTICLE:
				{
					List<TodParticleSystem> mParticleSystem = EffectSystem.gEffectSystem.mParticleHolder.mParticleSystems;
					TodParticleSystem todParticleSystem = (TodParticleSystem)attachEffect.mEffectID;
					if (todParticleSystem != null)
					{
						todParticleSystem.OverrideScale(null, theScale);
					}
					break;
				}
				case EffectType.EFFECT_REANIM:
				{
					List<Reanimation> mReanimation = EffectSystem.gEffectSystem.mReanimationHolder.mReanimations;
					Reanimation reanimation = (Reanimation)attachEffect.mEffectID;
					if (reanimation != null)
					{
						reanimation.OverrideScale(theScale, theScale);
					}
					break;
				}
				case EffectType.EFFECT_ATTACHMENT:
				{
					List<Attachment> mAttachment = EffectSystem.gEffectSystem.mAttachmentHolder.mAttachments;
					Attachment attachment = (Attachment)attachEffect.mEffectID;
					if (attachment != null)
					{
						attachment.OverrideScale(theScale);
					}
					break;
				}
				}
			}
		}

		public void Draw(Graphics g, bool theParentHidden, bool doScale)
		{
			Debug.ASSERT(!mDead);
			Debug.ASSERT(EffectSystem.gEffectSystem != null);
			List<TodParticleSystem> mParticleSystems = EffectSystem.gEffectSystem.mParticleHolder.mParticleSystems;
			List<Trail> mTrails = EffectSystem.gEffectSystem.mTrailHolder.mTrails;
			List<Reanimation> mReanimation = EffectSystem.gEffectSystem.mReanimationHolder.mReanimations;
			List<Attachment> mAttachment = EffectSystem.gEffectSystem.mAttachmentHolder.mAttachments;
			for (int i = 0; i < mNumEffects; i++)
			{
				AttachEffect attachEffect = mEffectArray[i];
				if (theParentHidden && attachEffect.mDontDrawIfParentHidden)
				{
					continue;
				}
				switch (attachEffect.mEffectType)
				{
				case EffectType.EFFECT_PARTICLE:
				{
					TodParticleSystem todParticleSystem = (TodParticleSystem)attachEffect.mEffectID;
					int num2 = mParticleSystems.IndexOf(todParticleSystem);
					if (num2 != -1)
					{
						TodParticleSystem todParticleSystem2 = todParticleSystem;
						if (todParticleSystem2 != null)
						{
							todParticleSystem2.Draw(g, doScale);
						}
					}
					break;
				}
				case EffectType.EFFECT_TRAIL:
				{
					Trail trail = null;
					int num = mTrails.IndexOf((Trail)attachEffect.mEffectID);
					if (num >= 0)
					{
						trail = mTrails[num];
					}
					if (trail != null)
					{
						trail.Draw(g);
					}
					break;
				}
				case EffectType.EFFECT_REANIM:
				{
					Reanimation reanimation = (Reanimation)attachEffect.mEffectID;
					if (reanimation != null && reanimation.mActive)
					{
						reanimation.Draw(g);
					}
					break;
				}
				case EffectType.EFFECT_ATTACHMENT:
				{
					Attachment attachment = (Attachment)attachEffect.mEffectID;
					if (attachment != null && attachment.mActive)
					{
						attachment.Draw(g, theParentHidden, true);
					}
					break;
				}
				}
			}
		}

		public void AttachmentDie()
		{
			Debug.ASSERT(EffectSystem.gEffectSystem != null);
			List<TodParticleSystem> mParticleSystems = EffectSystem.gEffectSystem.mParticleHolder.mParticleSystems;
			List<Trail> mTrails = EffectSystem.gEffectSystem.mTrailHolder.mTrails;
			List<Reanimation> mReanimations = EffectSystem.gEffectSystem.mReanimationHolder.mReanimations;
			List<Attachment> mAttachments = EffectSystem.gEffectSystem.mAttachmentHolder.mAttachments;
			for (int i = 0; i < mNumEffects; i++)
			{
				AttachEffect attachEffect = mEffectArray[i];
				switch (attachEffect.mEffectType)
				{
				case EffectType.EFFECT_PARTICLE:
				{
					TodParticleSystem todParticleSystem = attachEffect.mEffectID as TodParticleSystem;
					if (todParticleSystem != null && mParticleSystems.IndexOf(todParticleSystem) >= 0)
					{
						todParticleSystem.ParticleSystemDie();
					}
					break;
				}
				case EffectType.EFFECT_TRAIL:
				{
					Trail trail = attachEffect.mEffectID as Trail;
					if (trail != null && mTrails.IndexOf(trail) >= 0)
					{
						trail.mDead = true;
					}
					break;
				}
				case EffectType.EFFECT_REANIM:
				{
					Reanimation reanimation = attachEffect.mEffectID as Reanimation;
					if (reanimation != null && mReanimations.IndexOf(reanimation) >= 0)
					{
						reanimation.ReanimationDie();
					}
					break;
				}
				case EffectType.EFFECT_ATTACHMENT:
				{
					Attachment attachment = attachEffect.mEffectID as Attachment;
					if (attachment != null && mAttachments.IndexOf(attachment) >= 0)
					{
						attachment.AttachmentDie();
					}
					break;
				}
				}
				attachEffect.mEffectID = null;
			}
			mNumEffects = 0;
			mDead = true;
		}

		public void Detach()
		{
			Debug.ASSERT(EffectSystem.gEffectSystem != null);
			List<TodParticleSystem> mParticleSystems = EffectSystem.gEffectSystem.mParticleHolder.mParticleSystems;
			List<Trail> mTrails = EffectSystem.gEffectSystem.mTrailHolder.mTrails;
			List<Reanimation> mReanimations = EffectSystem.gEffectSystem.mReanimationHolder.mReanimations;
			List<Attachment> mAttachments = EffectSystem.gEffectSystem.mAttachmentHolder.mAttachments;
			for (int i = 0; i < mNumEffects; i++)
			{
				AttachEffect attachEffect = mEffectArray[i];
				switch (attachEffect.mEffectType)
				{
				case EffectType.EFFECT_PARTICLE:
				{
					TodParticleSystem todParticleSystem = null;
					TodParticleSystem todParticleSystem2 = attachEffect.mEffectID as TodParticleSystem;
					int num = mParticleSystems.IndexOf(todParticleSystem2);
					if (num != -1)
					{
						todParticleSystem = todParticleSystem2;
					}
					if (todParticleSystem != null)
					{
						Debug.ASSERT(todParticleSystem.mIsAttachment);
						todParticleSystem.mIsAttachment = false;
					}
					break;
				}
				case EffectType.EFFECT_TRAIL:
				{
					Trail trail = mTrails[mTrails.IndexOf((Trail)attachEffect.mEffectID)];
					if (trail != null)
					{
						Debug.ASSERT(trail.mIsAttachment);
						trail.mIsAttachment = false;
					}
					break;
				}
				case EffectType.EFFECT_REANIM:
				{
					Reanimation reanimation = mReanimations[mReanimations.IndexOf((Reanimation)attachEffect.mEffectID)];
					if (reanimation != null)
					{
						Debug.ASSERT(reanimation.mIsAttachment);
						reanimation.mIsAttachment = false;
					}
					break;
				}
				case EffectType.EFFECT_ATTACHMENT:
				{
					Attachment attachment = mAttachments[mAttachments.IndexOf((Attachment)attachEffect.mEffectID)];
					if (attachment != null)
					{
						attachment.Detach();
					}
					break;
				}
				}
				attachEffect.mEffectID = 0u;
			}
			mNumEffects = 0;
			mDead = true;
		}

		public void CrossFade(string theCrossFadeName)
		{
			Debug.ASSERT(EffectSystem.gEffectSystem != null);
			for (int i = 0; i < mNumEffects; i++)
			{
				AttachEffect attachEffect = mEffectArray[i];
				if (attachEffect.mEffectType == EffectType.EFFECT_PARTICLE)
				{
					List<TodParticleSystem> mParticleSystems = EffectSystem.gEffectSystem.mParticleHolder.mParticleSystems;
					int num = mParticleSystems.IndexOf((TodParticleSystem)attachEffect.mEffectID);
					TodParticleSystem todParticleSystem = null;
					if (num != -1)
					{
						todParticleSystem = mParticleSystems[num];
					}
					if (todParticleSystem != null)
					{
						todParticleSystem.CrossFade(theCrossFadeName);
					}
				}
			}
		}

		public void PropogateColor(SexyColor theColor, bool theEnableAdditiveColor, SexyColor theAdditiveColor, bool theEnableOverlayColor, SexyColor theOverlayColor)
		{
			Debug.ASSERT(EffectSystem.gEffectSystem != null);
			for (int i = 0; i < mNumEffects; i++)
			{
				AttachEffect attachEffect = mEffectArray[i];
				if (attachEffect.mDontPropogateColor)
				{
					continue;
				}
				switch (attachEffect.mEffectType)
				{
				case EffectType.EFFECT_PARTICLE:
				{
					List<TodParticleSystem> mParticleSystems = EffectSystem.gEffectSystem.mParticleHolder.mParticleSystems;
					if (attachEffect.mEffectID == null)
					{
						break;
					}
					int num = mParticleSystems.IndexOf((TodParticleSystem)attachEffect.mEffectID);
					if (num >= 0)
					{
						TodParticleSystem todParticleSystem = mParticleSystems[num];
						if (todParticleSystem != null)
						{
							todParticleSystem.OverrideColor(null, theColor);
							todParticleSystem.OverrideExtraAdditiveDraw(null, theEnableAdditiveColor);
						}
					}
					break;
				}
				case EffectType.EFFECT_REANIM:
				{
					Reanimation reanimation = (Reanimation)attachEffect.mEffectID;
					if (reanimation.mActive && reanimation != null)
					{
						reanimation.mColorOverride = theColor;
						reanimation.mExtraAdditiveColor = theAdditiveColor;
						reanimation.mEnableExtraAdditiveDraw = theEnableAdditiveColor;
						reanimation.mExtraOverlayColor = theOverlayColor;
						reanimation.mEnableExtraOverlayDraw = theEnableOverlayColor;
						reanimation.PropogateColorToAttachments();
					}
					break;
				}
				case EffectType.EFFECT_ATTACHMENT:
				{
					List<Attachment> mAttachment = EffectSystem.gEffectSystem.mAttachmentHolder.mAttachments;
					Attachment attachment = (Attachment)attachEffect.mEffectID;
					if (attachment != null)
					{
						attachment.PropogateColor(theColor, theEnableAdditiveColor, theAdditiveColor, theEnableOverlayColor, theOverlayColor);
					}
					break;
				}
				}
			}
		}

		public void SetPosition(SexyVector2 thePosition)
		{
			Debug.ASSERT(EffectSystem.gEffectSystem != null);
			for (int i = 0; i < mNumEffects; i++)
			{
				AttachEffect attachEffect = mEffectArray[i];
				SexyVector2 position = new SexyVector2(Vector2.Transform(thePosition.mVector, attachEffect.mOffset.mMatrix));
				switch (attachEffect.mEffectType)
				{
				case EffectType.EFFECT_PARTICLE:
				{
					List<TodParticleSystem> mParticleSystems = EffectSystem.gEffectSystem.mParticleHolder.mParticleSystems;
					int num = mParticleSystems.IndexOf((TodParticleSystem)attachEffect.mEffectID);
					if (num >= 0)
					{
						TodParticleSystem todParticleSystem = mParticleSystems[num];
						if (todParticleSystem != null)
						{
							todParticleSystem.SystemMove(position.x, position.y);
						}
					}
					break;
				}
				case EffectType.EFFECT_TRAIL:
				{
					List<Trail> mTrail = EffectSystem.gEffectSystem.mTrailHolder.mTrails;
					Trail trail = (Trail)attachEffect.mEffectID;
					if (trail != null)
					{
						trail.AddPoint(position.x, position.y);
					}
					break;
				}
				case EffectType.EFFECT_REANIM:
				{
					List<Reanimation> mReanimation = EffectSystem.gEffectSystem.mReanimationHolder.mReanimations;
					Reanimation reanimation = (Reanimation)attachEffect.mEffectID;
					if (reanimation != null)
					{
						reanimation.SetPosition(position.x * Constants.S, position.y * Constants.S);
					}
					break;
				}
				case EffectType.EFFECT_ATTACHMENT:
				{
					List<Attachment> mAttachment = EffectSystem.gEffectSystem.mAttachmentHolder.mAttachments;
					Attachment attachment = (Attachment)attachEffect.mEffectID;
					if (attachment != null)
					{
						attachment.SetPosition(position);
					}
					break;
				}
				}
			}
		}
	}
}
