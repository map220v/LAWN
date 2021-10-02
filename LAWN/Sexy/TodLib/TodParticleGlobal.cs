using Microsoft.Xna.Framework;

namespace Sexy.TodLib
{
	internal static class TodParticleGlobal
	{
		public static int gParticleDefCount;

		public static TodParticleDefinition[] gParticleDefArray;

		public static int gParticleParamArraySize;

		public static ParticleParams[] gParticleParamArray;

		public static void TodParticleLoadDefinitions(ref ParticleParams[] theParticleParamArray, int theParticleParamArraySize)
		{
			gParticleParamArraySize = theParticleParamArraySize;
			gParticleParamArray = theParticleParamArray;
			gParticleDefCount = theParticleParamArraySize;
			gParticleDefArray = new TodParticleDefinition[gParticleDefCount];
			for (int i = 0; i < gParticleDefArray.Length; i++)
			{
				gParticleDefArray[i] = new TodParticleDefinition();
			}
			for (int j = 0; j < gParticleParamArraySize; j++)
			{
				ParticleParams particleParams = theParticleParamArray[j];
				Debug.ASSERT(particleParams.mParticleEffect == (ParticleEffect)j);
				TodParticleDefinition theParticleDef = gParticleDefArray[j];
				if (!TodParticleLoadADef(ref theParticleDef, particleParams.mParticleFileName))
				{
					break;
				}
				gParticleDefArray[j] = theParticleDef;
				GlobalStaticVars.gSexyAppBase.mNumLoadingThreadTasks += 6;
			}
		}

		public static void TodParticleFreeDefinitions()
		{
			gParticleDefArray = null;
			gParticleDefCount = 0;
			gParticleParamArray = null;
			gParticleParamArraySize = 0;
		}

		public static bool TodParticleLoadADef(ref TodParticleDefinition theParticleDef, string aFilename)
		{
			if (!GlobalStaticVars.gSexyAppBase.mResourceManager.LoadParticle(aFilename, ref theParticleDef))
			{
				return false;
			}
			for (int i = 0; i < theParticleDef.mEmitterDefCount; i++)
			{
				TodEmitterDefinition todEmitterDefinition = theParticleDef.mEmitterDefs[i];
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mSystemDuration, 0f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mSpawnRate, 0f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mSpawnMinActive, -1f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mSpawnMaxActive, -1f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mSpawnMaxLaunched, -1f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mEmitterRadius, 0f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mEmitterOffsetX, 0f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mEmitterOffsetY, 0f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mEmitterBoxX, 0f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mEmitterBoxY, 0f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mEmitterSkewX, 0f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mEmitterSkewY, 0f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mParticleDuration, 100f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mLaunchSpeed, 0f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mSystemRed, 1f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mSystemGreen, 1f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mSystemBlue, 1f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mSystemAlpha, 1f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mSystemBrightness, 1f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mLaunchAngle, 0f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mCrossFadeDuration, 0f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mParticleRed, 1f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mParticleGreen, 1f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mParticleBlue, 1f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mParticleAlpha, 1f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mParticleBrightness, 1f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mParticleSpinAngle, 0f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mParticleSpinSpeed, 0f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mParticleScale, 1f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mParticleStretch, 1f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mCollisionReflect, 0f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mCollisionSpin, 0f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mClipTop, 0f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mClipBottom, 0f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mClipLeft, 0f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mClipRight, 0f);
				Definition.FloatTrackSetDefault(ref todEmitterDefinition.mAnimationRate, 0f);
			}
			return true;
		}

		public static float FloatTrackEvaluateFromLastTime(ref FloatParameterTrack theTrack, float theTimeValue, float theInterp)
		{
			if (theTimeValue < 0f)
			{
				return 0f;
			}
			return Definition.FloatTrackEvaluate(ref theTrack, theTimeValue, theInterp);
		}

		internal static float CrossFadeLerp(float theFrom, float theTo, bool theFromIsSet, bool theToIsSet, float theFraction)
		{
			if (!theFromIsSet)
			{
				return theTo;
			}
			if (!theToIsSet)
			{
				return theFrom;
			}
			return theFrom + (theTo - theFrom) * theFraction;
		}

		internal static void RenderParticle(Graphics g, TodParticle theParticle, SexyColor theColor, ref ParticleRenderParams theParams)
		{
			TodParticleEmitter mParticleEmitter = theParticle.mParticleEmitter;
			TodEmitterDefinition mEmitterDef = mParticleEmitter.mEmitterDef;
			Image image;
			if (mParticleEmitter.mImageOverride != null)
			{
				image = mParticleEmitter.mImageOverride;
			}
			else
			{
				if (mEmitterDef.mImage == null)
				{
					return;
				}
				image = mEmitterDef.mImage;
			}
			int celWidth = image.GetCelWidth();
			int celHeight = image.GetCelHeight();
			int num;
			if (mParticleEmitter.mFrameOverride != -1)
			{
				num = mParticleEmitter.mFrameOverride;
			}
			else if (Definition.FloatTrackIsSet(ref mEmitterDef.mAnimationRate))
			{
				num = (int)(theParticle.mAnimationTimeValue * (float)mEmitterDef.mImageFrames);
				num = TodCommon.ClampInt(num, 0, mEmitterDef.mImageFrames - 1);
			}
			else if (mEmitterDef.mAnimated != 0)
			{
				num = (int)(theParticle.mParticleTimeValue * (float)mEmitterDef.mImageFrames);
				num = TodCommon.ClampInt(num, 0, mEmitterDef.mImageFrames - 1);
			}
			else
			{
				num = theParticle.mImageFrame;
			}
			num += mEmitterDef.mImageCol;
			if (num >= image.mNumCols)
			{
				num = image.mNumCols - 1;
			}
			int num2 = mEmitterDef.mImageRow;
			if (num2 >= image.mNumRows)
			{
				num2 = image.mNumRows - 1;
			}
			TRect tRect = new TRect(celWidth * num, celHeight * num2, celWidth, celHeight);
			float num3 = Definition.FloatTrackEvaluate(ref mEmitterDef.mClipTop, theParticle.mParticleTimeValue, theParticle.mParticleInterp[11]);
			float num4 = Definition.FloatTrackEvaluate(ref mEmitterDef.mClipBottom, theParticle.mParticleTimeValue, theParticle.mParticleInterp[12]);
			float num5 = Definition.FloatTrackEvaluate(ref mEmitterDef.mClipLeft, theParticle.mParticleTimeValue, theParticle.mParticleInterp[13]);
			float num6 = Definition.FloatTrackEvaluate(ref mEmitterDef.mClipRight, theParticle.mParticleTimeValue, theParticle.mParticleInterp[14]);
			theParams.mPosX += num5 * (float)celWidth;
			theParams.mPosY += num3 * (float)celHeight;
			tRect.mX += TodCommon.FloatRoundToInt(num5 * (float)celWidth);
			tRect.mY += TodCommon.FloatRoundToInt(num3 * (float)celHeight);
			tRect.mWidth -= TodCommon.FloatRoundToInt((num5 + num6) * (float)celWidth);
			tRect.mHeight -= TodCommon.FloatRoundToInt((num3 + num4) * (float)celHeight);
			if (TodCommon.TestBit((uint)mEmitterDef.mParticleFlags, 2))
			{
				theParams.mPosX = TodCommon.FloatRoundToInt(theParams.mPosX);
				theParams.mPosY = TodCommon.FloatRoundToInt(theParams.mPosY);
			}
			Graphics.DrawMode drawMode = g.mDrawMode;
			if (TodCommon.TestBit((uint)mEmitterDef.mParticleFlags, 8))
			{
				drawMode = Graphics.DrawMode.DRAWMODE_ADDITIVE;
			}
			if (TodCommon.TestBit((uint)mEmitterDef.mParticleFlags, 9))
			{
				SexyColor aColor = g.GetColor();
				Graphics.DrawMode mDrawMode = g.mDrawMode;
				g.SetColor(theColor);
				g.SetDrawMode(drawMode);
				g.FillRect(-g.mTransX, -g.mTransY, 480, 320);
				g.SetColor(aColor);
				g.SetDrawMode(mDrawMode);
				return;
			}
			tRect.mX += image.mS;
			tRect.mY += image.mT;
			int mWidth = tRect.mWidth;
			int mHeight = tRect.mHeight;
			g.SetDrawMode(drawMode);
			g.DrawImageRotatedScaled(image, new TRect((int)theParams.mPosX, (int)theParams.mPosY, tRect.mWidth, tRect.mHeight), new TRect(tRect.mX, tRect.mY, tRect.mWidth, tRect.mHeight), mParticleEmitter.mExtraAdditiveDrawOverride ? SexyColor.White : theColor, theParams.mSpinPosition, new Vector2(theParams.mParticleScale, theParams.mParticleScale * theParams.mParticleStretch), new Vector2(tRect.mWidth / 2, tRect.mHeight / 2));
			if (mParticleEmitter.mExtraAdditiveDrawOverride)
			{
				g.SetDrawMode(Graphics.DrawMode.DRAWMODE_ADDITIVE);
				g.DrawImageRotatedScaled(image, new TRect((int)theParams.mPosX, (int)theParams.mPosY, tRect.mWidth, tRect.mHeight), new TRect(tRect.mX, tRect.mY, tRect.mWidth, tRect.mHeight), theColor, theParams.mSpinPosition, new Vector2(theParams.mParticleScale, theParams.mParticleScale * theParams.mParticleStretch), new Vector2(tRect.mWidth / 2, tRect.mHeight / 2));
			}
		}
	}
}
