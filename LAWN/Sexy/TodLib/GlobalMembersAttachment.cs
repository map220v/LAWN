using System.Collections.Generic;

namespace Sexy.TodLib
{
	internal static class GlobalMembersAttachment
	{
		public static void AttachmentUpdateAndMove(ref Attachment theAttachmentID, float theX, float theY)
		{
			if (theAttachmentID == null)
			{
				return;
			}
			Debug.ASSERT(EffectSystem.gEffectSystem != null);
			List<Attachment> mAttachment = EffectSystem.gEffectSystem.mAttachmentHolder.mAttachments;
			Attachment attachment = null;
			if (theAttachmentID != null && theAttachmentID.mActive)
			{
				attachment = theAttachmentID;
			}
			if (attachment != null)
			{
				attachment.Update();
				attachment.SetPosition(new SexyVector2(theX, theY));
				if (attachment.mDead)
				{
					theAttachmentID = null;
				}
			}
			else
			{
				theAttachmentID = null;
			}
		}

		public static void AttachmentUpdateAndSetMatrix(ref Attachment theAttachmentID, ref SexyTransform2D theMatrix)
		{
			if (theAttachmentID == null)
			{
				return;
			}
			Debug.ASSERT(EffectSystem.gEffectSystem != null);
			List<Attachment> mAttachment = EffectSystem.gEffectSystem.mAttachmentHolder.mAttachments;
			Attachment attachment = null;
			if (theAttachmentID != null && theAttachmentID.mActive)
			{
				attachment = theAttachmentID;
			}
			if (attachment != null)
			{
				attachment.Update();
				attachment.SetMatrix(ref theMatrix);
				if (attachment != null && attachment.mDead)
				{
					theAttachmentID = null;
				}
			}
			else
			{
				theAttachmentID = null;
			}
		}

		public static void AttachmentOverrideColor(Attachment theAttachmentID, SexyColor theColor)
		{
			if (theAttachmentID != null)
			{
				Debug.ASSERT(EffectSystem.gEffectSystem != null);
				List<Attachment> mAttachment = EffectSystem.gEffectSystem.mAttachmentHolder.mAttachments;
				Attachment attachment = null;
				if (theAttachmentID != null && theAttachmentID.mActive)
				{
					attachment = theAttachmentID;
				}
				if (attachment != null)
				{
					attachment.OverrideColor(theColor);
				}
			}
		}

		public static void AttachmentOverrideScale(Attachment theAttachmentID, float theScale)
		{
			if (theAttachmentID != null)
			{
				Debug.ASSERT(EffectSystem.gEffectSystem != null);
				List<Attachment> mAttachment = EffectSystem.gEffectSystem.mAttachmentHolder.mAttachments;
				Attachment attachment = null;
				if (theAttachmentID != null && theAttachmentID.mActive)
				{
					attachment = theAttachmentID;
				}
				if (attachment != null)
				{
					attachment.OverrideScale(theScale);
				}
			}
		}

		public static void AttachmentCrossFade(Attachment theAttachmentID, string theCrossFadeName)
		{
			if (theAttachmentID != null)
			{
				Debug.ASSERT(EffectSystem.gEffectSystem != null);
				List<Attachment> mAttachments = EffectSystem.gEffectSystem.mAttachmentHolder.mAttachments;
				Attachment attachment = null;
				if (mAttachments.Contains(theAttachmentID))
				{
					attachment = mAttachments[mAttachments.IndexOf(theAttachmentID)];
				}
				if (attachment != null)
				{
					attachment.CrossFade(theCrossFadeName);
				}
			}
		}

		public static void AttachmentDraw(Attachment theAttachmentID, Graphics g, bool theParentHidden, bool doScale)
		{
			if (theAttachmentID != null)
			{
				if (theAttachmentID.mUsesClipping)
				{
					g.HardwareClip();
				}
				Debug.ASSERT(EffectSystem.gEffectSystem != null);
				List<Attachment> mAttachment = EffectSystem.gEffectSystem.mAttachmentHolder.mAttachments;
				Attachment attachment = null;
				if (theAttachmentID != null && theAttachmentID.mActive)
				{
					attachment = theAttachmentID;
				}
				if (attachment != null)
				{
					attachment.Draw(g, theParentHidden, doScale);
				}
				if (theAttachmentID.mUsesClipping)
				{
					g.EndHardwareClip();
				}
			}
		}

		public static void AttachmentDie(ref Attachment theAttachmentID)
		{
			if (theAttachmentID != null)
			{
				Debug.ASSERT(EffectSystem.gEffectSystem != null);
				List<Attachment> mAttachments = EffectSystem.gEffectSystem.mAttachmentHolder.mAttachments;
				Attachment attachment = null;
				if (mAttachments.Contains(theAttachmentID))
				{
					attachment = mAttachments[mAttachments.IndexOf(theAttachmentID)];
				}
				theAttachmentID = null;
				if (attachment != null)
				{
					attachment.AttachmentDie();
				}
			}
		}

		public static void AttachmentDetach(ref Attachment theAttachmentID)
		{
			if (theAttachmentID != null)
			{
				Debug.ASSERT(EffectSystem.gEffectSystem != null);
				List<Attachment> mAttachments = EffectSystem.gEffectSystem.mAttachmentHolder.mAttachments;
				Attachment attachment = null;
				if (mAttachments.Contains(theAttachmentID))
				{
					attachment = mAttachments[mAttachments.IndexOf(theAttachmentID)];
				}
				theAttachmentID = null;
				if (attachment != null)
				{
					attachment.Detach();
				}
			}
		}

		public static AttachEffect AttachReanim(ref Attachment theAttachmentID, Reanimation theReanimation, float theOffsetX, float theOffsetY)
		{
			AttachEffect result = CreateEffectAttachment(ref theAttachmentID, EffectType.EFFECT_REANIM, theReanimation, theOffsetX, theOffsetY);
			Debug.ASSERT(!theReanimation.mIsAttachment);
			theReanimation.mIsAttachment = true;
			return result;
		}

		public static AttachEffect AttachParticle(ref Attachment theAttachmentID, TodParticleSystem theParticleSystem, float theOffsetX, float theOffsetY)
		{
			if (theParticleSystem == null || theParticleSystem.mDead)
			{
				return null;
			}
			AttachEffect result = CreateEffectAttachment(ref theAttachmentID, EffectType.EFFECT_PARTICLE, theParticleSystem, theOffsetX, theOffsetY);
			Debug.ASSERT(!theParticleSystem.mIsAttachment);
			theParticleSystem.mIsAttachment = true;
			return result;
		}

		public static AttachEffect AttachTrail(ref Attachment theAttachmentID, Trail theTrail, float theOffsetX, float theOffsetY)
		{
			Trail theDataID = EffectSystem.gEffectSystem.mTrailHolder.mTrails[EffectSystem.gEffectSystem.mTrailHolder.mTrails.IndexOf(theTrail)];
			AttachEffect result = CreateEffectAttachment(ref theAttachmentID, EffectType.EFFECT_TRAIL, theDataID, theOffsetX, theOffsetY);
			Debug.ASSERT(!theTrail.mIsAttachment);
			theTrail.mIsAttachment = true;
			return result;
		}

		public static void AttachmentDetachCrossFadeParticleType(ref Attachment theAttachmentID, ParticleEffect theParticleEffect, string theCrossFadeName)
		{
			List<Attachment> mAttachments = EffectSystem.gEffectSystem.mAttachmentHolder.mAttachments;
			Attachment attachment = null;
			if (mAttachments.IndexOf(theAttachmentID) >= 0)
			{
				attachment = theAttachmentID;
			}
			if (attachment == null)
			{
				return;
			}
			Debug.ASSERT(theParticleEffect >= ParticleEffect.PARTICLE_MELONSPLASH && (int)theParticleEffect < TodParticleGlobal.gParticleDefCount);
			TodParticleDefinition todParticleDefinition = TodParticleGlobal.gParticleDefArray[(int)theParticleEffect];
			List<TodParticleSystem> mParticleSystems = EffectSystem.gEffectSystem.mParticleHolder.mParticleSystems;
			for (int i = 0; i < attachment.mNumEffects; i++)
			{
				AttachEffect attachEffect = attachment.mEffectArray[i];
				if (attachEffect.mEffectType != 0)
				{
					continue;
				}
				TodParticleSystem todParticleSystem = (TodParticleSystem)attachEffect.mEffectID;
				if (!mParticleSystems.Contains(todParticleSystem) || todParticleSystem.mParticleDef != todParticleDefinition)
				{
					continue;
				}
				if (theCrossFadeName != null)
				{
					if (todParticleSystem.mIsAttachment)
					{
						todParticleSystem.mIsAttachment = false;
					}
					todParticleSystem.CrossFade(theCrossFadeName);
				}
				else
				{
					todParticleSystem.ParticleSystemDie();
				}
				attachEffect.mEffectID = null;
			}
		}

		public static void AttachmentPropogateColor(Attachment theAttachment, SexyColor theColor, bool theEnableAdditiveColor, SexyColor theAdditiveColor, bool theEnableOverlayColor, SexyColor theOverlayColor)
		{
			if (theAttachment != null)
			{
				Debug.ASSERT(EffectSystem.gEffectSystem != null);
				List<Attachment> mAttachment = EffectSystem.gEffectSystem.mAttachmentHolder.mAttachments;
				Attachment attachment = null;
				if (theAttachment.mActive)
				{
					attachment = theAttachment;
				}
				if (attachment != null)
				{
					attachment.PropogateColor(theColor, theEnableAdditiveColor, theAdditiveColor, theEnableOverlayColor, theOverlayColor);
				}
			}
		}

		public static Reanimation FindReanimAttachment(Attachment theAttachmentID)
		{
			Debug.ASSERT(EffectSystem.gEffectSystem != null);
			List<Attachment> mAttachments = EffectSystem.gEffectSystem.mAttachmentHolder.mAttachments;
			Attachment attachment = null;
			if (mAttachments.Contains(theAttachmentID))
			{
				attachment = mAttachments[mAttachments.IndexOf(theAttachmentID)];
			}
			if (attachment == null)
			{
				return null;
			}
			for (int i = 0; i < attachment.mNumEffects; i++)
			{
				AttachEffect attachEffect = attachment.mEffectArray[i];
				if (attachEffect.mEffectType == EffectType.EFFECT_REANIM)
				{
					List<Reanimation> mReanimation = EffectSystem.gEffectSystem.mReanimationHolder.mReanimations;
					Reanimation reanimation = attachEffect.mEffectID as Reanimation;
					if (reanimation != null && !reanimation.mDead)
					{
						return reanimation;
					}
				}
			}
			return null;
		}

		public static AttachEffect FindFirstAttachment(Attachment theAttachmentID)
		{
			Debug.ASSERT(EffectSystem.gEffectSystem != null);
			List<Attachment> mAttachments = EffectSystem.gEffectSystem.mAttachmentHolder.mAttachments;
			Attachment attachment = null;
			if (mAttachments.Contains(theAttachmentID))
			{
				attachment = mAttachments[mAttachments.IndexOf(theAttachmentID)];
			}
			if (attachment == null)
			{
				return null;
			}
			if (attachment.mNumEffects == 0)
			{
				return null;
			}
			return attachment.mEffectArray[0];
		}

		public static void AttachmentReanimTypeDie(ref Attachment theAttachmentID, ReanimationType theReanimType)
		{
			List<Attachment> mAttachments = EffectSystem.gEffectSystem.mAttachmentHolder.mAttachments;
			Attachment attachment = null;
			if (mAttachments.Contains(theAttachmentID))
			{
				attachment = mAttachments[mAttachments.IndexOf(theAttachmentID)];
			}
			if (attachment == null)
			{
				return;
			}
			List<Reanimation> mReanimations = EffectSystem.gEffectSystem.mReanimationHolder.mReanimations;
			for (int i = 0; i < attachment.mNumEffects; i++)
			{
				AttachEffect attachEffect = attachment.mEffectArray[i];
				if (attachEffect.mEffectType == EffectType.EFFECT_REANIM)
				{
					Reanimation reanimation = mReanimations[mReanimations.IndexOf((Reanimation)attachEffect.mEffectID)];
					if (reanimation != null && reanimation.mReanimationType == theReanimType)
					{
						reanimation.ReanimationDie();
					}
				}
			}
		}

		public static bool IsFullOfAttachments(ref Attachment theAttachmentID)
		{
			Debug.ASSERT(EffectSystem.gEffectSystem != null);
			List<Attachment> mAttachment = EffectSystem.gEffectSystem.mAttachmentHolder.mAttachments;
			Attachment attachment = null;
			if (theAttachmentID != null && theAttachmentID.mActive)
			{
				attachment = theAttachmentID;
			}
			if (attachment == null)
			{
				return false;
			}
			if (attachment.mNumEffects >= 16)
			{
				return true;
			}
			return false;
		}

		public static AttachEffect CreateEffectAttachment(ref Attachment theAttachmentID, EffectType theEffectType, object theDataID, float theOffsetX, float theOffsetY)
		{
			Debug.ASSERT(EffectSystem.gEffectSystem != null);
			List<Attachment> mAttachments = EffectSystem.gEffectSystem.mAttachmentHolder.mAttachments;
			Attachment attachment = null;
			int num = mAttachments.IndexOf(theAttachmentID);
			if (num >= 0)
			{
				attachment = theAttachmentID;
			}
			if (attachment == null || attachment.mDead)
			{
				attachment = (theAttachmentID = EffectSystem.gEffectSystem.mAttachmentHolder.AllocAttachment());
			}
			Debug.ASSERT(attachment.mNumEffects < 16);
			Debug.ASSERT(!attachment.mDead);
			if (attachment.mNumEffects >= attachment.mEffectArray.Length)
			{
				return null;
			}
			switch (theEffectType)
			{
			case EffectType.EFFECT_PARTICLE:
				if ((theDataID as TodParticleSystem).mDead)
				{
					return null;
				}
				break;
			case EffectType.EFFECT_REANIM:
				if ((theDataID as Reanimation).mDead)
				{
					return null;
				}
				break;
			case EffectType.EFFECT_TRAIL:
				if ((theDataID as Trail).mDead)
				{
					return null;
				}
				break;
			case EffectType.EFFECT_ATTACHMENT:
				if ((theDataID as Attachment).mDead)
				{
					return null;
				}
				break;
			}
			AttachEffect attachEffect = attachment.mEffectArray[attachment.mNumEffects];
			attachEffect.mEffectType = theEffectType;
			attachEffect.mEffectID = theDataID;
			attachEffect.mDontDrawIfParentHidden = false;
			attachEffect.mOffset = new SexyTransform2D(true);
			attachEffect.mOffset.mMatrix.M41 = theOffsetX;
			attachEffect.mOffset.mMatrix.M42 = theOffsetY;
			attachment.mNumEffects++;
			return attachEffect;
		}
	}
}
