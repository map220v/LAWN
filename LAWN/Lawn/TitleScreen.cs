using Sexy;
using Sexy.TodLib;
using System;

namespace Lawn
{
	internal class TitleScreen : Widget, ButtonListener
	{
		private const int NUM_TRIGGERS = 5;

		public HyperlinkWidget mStartButton;

		public float mCurBarWidth;

		public float mTotalBarWidth;

		public float mBarVel;

		public float mBarStartProgress;

		public bool mRegisterClicked;

		public bool mLoadingThreadComplete;

		public int mTitleAge;

		public KeyCode mQuickLoadKey;

		public bool mNeedRegister;

		public bool mNeedShowRegisterBox;

		public bool mNeedToInit;

		public float mPrevLoadingPercent;

		public TitleState mTitleState;

		public int mTitleStateCounter;

		public int mTitleStateDuration;

		public bool mLoaderScreenIsLoaded;

		public bool mNeedToUnpackAtlas;

		public int mNextImageIndex;

		public LawnApp mApp;

		private static Image[] PreflightImages;

		private float[] aTriggerPoint = new float[5];

		public void ButtonPress(int theId, int theClickCount)
		{
		}

		public void ButtonDownTick(int theId)
		{
		}

		public void ButtonMouseEnter(int theId)
		{
		}

		public void ButtonMouseLeave(int theId)
		{
		}

		public void ButtonMouseMove(int theId, int theX, int theY)
		{
		}

		public TitleScreen(LawnApp theApp)
		{
			mApp = theApp;
			mCurBarWidth = 0f;
			mTotalBarWidth = Constants.InvertAndScale(314f);
			mBarVel = 0.2f;
			mBarStartProgress = 0f;
			mPrevLoadingPercent = 0f;
			mTitleAge = 0;
			mNeedRegister = false;
			mRegisterClicked = false;
			mNeedShowRegisterBox = false;
			mLoadingThreadComplete = false;
			mNeedToInit = true;
			mQuickLoadKey = KeyCode.KEYCODE_UNKNOWN;
			mTitleState = TitleState.TITLESTATE_WAITING_FOR_FIRST_DRAW;
			mTitleStateDuration = 0;
			mTitleStateCounter = 0;
			mLoaderScreenIsLoaded = false;
			mNeedToUnpackAtlas = true;
			mStartButton = new HyperlinkWidget(0, this);
			mStartButton.mColor = new SexyColor(218, 184, 33);
			mStartButton.mOverColor = new SexyColor(250, 90, 15);
			mStartButton.mUnderlineSize = 0;
			mStartButton.mDisabled = true;
			mStartButton.mVisible = false;
			mNextImageIndex = 0;
			PreflightImages = new Image[22]
			{
				Resources.IMAGE_PLANTSZOMBIES,
				Resources.IMAGE_CHARREDZOMBIES,
				Resources.IMAGE_ALMANACUI,
				Resources.IMAGE_SEEDATLAS,
				Resources.IMAGE_DAVE,
				Resources.IMAGE_CACHED,
				Resources.IMAGE_DIALOG,
				Resources.IMAGE_PARTICLES,
				Resources.IMAGE_CONVEYORBELT_BACKDROP,
				Resources.IMAGE_CONVEYORBELT_BELT,
				Resources.IMAGE_QUICKPLAY,
				Resources.IMAGE_SPEECHBUBBLE,
				Resources.IMAGE_GOODIES,
				Resources.IMAGE_ZOMBIE_NOTE_SMALL,
				Resources.IMAGE_SELECTORSCREEN_ACHIEVEMENTS_TOP_BACKGROUND,
				Resources.IMAGE_SELECTORSCREEN_MOREGAMES_BACKGROUND,
				Resources.IMAGE_SELECTORSCREEN_MAIN_BACKGROUND,
				Resources.IMAGE_SELECTORSCREEN_ACHIEVEMENTS_HOLE,
				Resources.IMAGE_SELECTORSCREEN_ACHIEVEMENTS_HOLE_CHINA,
				Resources.IMAGE_SELECTORSCREEN_QUICKPLAY_BACKGROUND,
				Resources.IMAGE_REANIM_ZOMBIESWON,
				Resources.IMAGE_SCARY_POT
			};
			aTriggerPoint[0] = 0.11f * mTotalBarWidth;
			aTriggerPoint[1] = 0.32f * mTotalBarWidth;
			aTriggerPoint[2] = 0.52f * mTotalBarWidth;
			aTriggerPoint[3] = 0.72f * mTotalBarWidth;
			aTriggerPoint[4] = 0.906f * mTotalBarWidth;
		}

		public override void Dispose()
		{
			if (Main.LOW_MEMORY_DEVICE)
			{
				mApp.mResourceManager.DeleteResources("Init");
				mApp.mResourceManager.DeleteResources("LoaderBar");
			}
			mStartButton.Dispose();
		}

		public override void Update()
		{
			base.Update();
			if (mApp.mShutdown)
			{
				return;
			}
			MarkDirty();
			if (mTitleState == TitleState.TITLESTATE_WAITING_FOR_FIRST_DRAW)
			{
				mApp.mMusic.MusicTitleScreenInit();
				mApp.StartLoadingThread();
				mTitleState = TitleState.TITLESTATE_POPCAP_LOGO;
				mTitleStateDuration = 200;
				mTitleStateCounter = mTitleStateDuration;
			}
			if (mQuickLoadKey != 0 && mTitleState != TitleState.TITLESTATE_SCREEN)
			{
				mTitleState = TitleState.TITLESTATE_SCREEN;
				mTitleStateDuration = 0;
				mTitleStateCounter = 100;
			}
			mTitleAge += 3;
			if (mTitleStateCounter > 0)
			{
				mTitleStateCounter -= 3;
			}
			if (mTitleState == TitleState.TITLESTATE_POPCAP_LOGO)
			{
				if (mTitleStateCounter <= 0)
				{
					mTitleState = TitleState.TITLESTATE_SCREEN;
					mTitleStateDuration = 100;
					mTitleStateCounter = mTitleStateDuration;
				}
			}
			else
			{
				if (!mLoaderScreenIsLoaded)
				{
					return;
				}
				if (mNeedToUnpackAtlas)
				{
					AtlasResources.mAtlasResources.UnpackLoadingAtlasImages();
					mNeedToUnpackAtlas = false;
				}
				float num = (float)mApp.GetLoadingThreadProgress();
				if (mNeedToInit)
				{
					mNeedToInit = false;
					mStartButton.mLabel = TodStringFile.TodStringTranslate("[LOADING]");
					mStartButton.SetFont(Resources.FONT_BRIANNETOD16);
					mStartButton.Resize(mWidth / 2 - AtlasResources.IMAGE_LOADBAR_DIRT.mWidth / 2, (int)(Constants.S * 650f), (int)mTotalBarWidth, (int)Constants.InvertAndScale(18f));
					mStartButton.mVisible = true;
					float num2 = (!(num > 1E-06f)) ? 3000f : ((float)mTitleAge / num);
					float num3 = num2 * (1f - num);
					num3 = TodCommon.ClampFloat(num3, 100f, 3000f);
					mBarVel = mTotalBarWidth / num3;
					mBarStartProgress = Math.Min(num, 0.9f);
				}
				float num4 = (num - mBarStartProgress) / (1f - mBarStartProgress);
				int theY = (mTitleStateCounter <= 10) ? TodCommon.TodAnimateCurve(10, 0, mTitleStateCounter, (int)(Constants.S * 532f), (int)(Constants.S * 523f), TodCurves.CURVE_BOUNCE) : TodCommon.TodAnimateCurve(60, 10, mTitleStateCounter, (int)(Constants.S * 731f), (int)(Constants.S * 532f), TodCurves.CURVE_EASE_IN);
				mStartButton.Resize(mStartButton.mX, theY, (int)mTotalBarWidth, mStartButton.mHeight);
				if (mTitleStateCounter > 0)
				{
					return;
				}
				mApp.mEffectSystem.Update();
				float num5 = mCurBarWidth;
				mCurBarWidth += mBarVel;
				if (!mLoadingThreadComplete)
				{
					if (mCurBarWidth > mTotalBarWidth * 0.99f)
					{
						mCurBarWidth = mTotalBarWidth * 0.99f;
					}
				}
				else if (mCurBarWidth > mTotalBarWidth)
				{
					if (mApp.mRestoreLocation == RestoreLocation.RESTORE_BOARD)
					{
						mApp.LoadingCompleted();
					}
					else
					{
						mStartButton.mLabel = TodStringFile.TodStringTranslate("[CLICK_TO_START]");
						mCurBarWidth = mTotalBarWidth;
					}
				}
				if (num4 > mPrevLoadingPercent + 0.01f || mLoadingThreadComplete)
				{
					float num6 = TodCommon.TodAnimateCurveFloatTime(0f, 1f, num4, 0f, mTotalBarWidth, TodCurves.CURVE_EASE_IN);
					float num7 = num6 - mCurBarWidth;
					float num8 = TodCommon.TodAnimateCurveFloatTime(0f, 1f, num4, 0.0001f, 1E-05f, TodCurves.CURVE_LINEAR);
					if (mLoadingThreadComplete)
					{
						num8 = 0.0001f;
					}
					mBarVel += num7 * Math.Abs(num7) * num8;
					float num9 = TodCommon.TodAnimateCurveFloatTime(0f, 1f, num4, 0.2f, 0.01f, TodCurves.CURVE_LINEAR);
					float num10 = 2f;
					if (mApp.mTodCheatKeys)
					{
						num9 = 0f;
						num10 = 100f;
					}
					if (mBarVel < num9)
					{
						mBarVel = num9;
					}
					else if (mBarVel > num10)
					{
						mBarVel = num10;
					}
					mPrevLoadingPercent = num4;
				}
				if (GameConstants.TESTING_LOAD_BAR && mBarVel > 0.5f)
				{
					mBarVel = 0.5f;
				}
				if (!mLoadingThreadComplete && mApp.mLoadingThreadCompleted)
				{
					LawnApp.PreallocateMemory();
					mLoadingThreadComplete = true;
					mStartButton.SetDisabled(false);
					mStartButton.SetVisible(true);
				}
				for (int i = 0; i < 5; i++)
				{
					if (!(aTriggerPoint[i] <= num5) && !(aTriggerPoint[i] > mCurBarWidth))
					{
						ReanimationType theReanimationType = ReanimationType.REANIM_LOADBAR_SPROUT;
						if (i == 4)
						{
							theReanimationType = ReanimationType.REANIM_LOADBAR_ZOMBIEHEAD;
						}
						float num11 = (float)Constants.TitleScreen_ReanimStart_X + aTriggerPoint[i];
						float num12 = (float)mStartButton.mY - Constants.InvertAndScale(42f);
						Reanimation reanimation = mApp.AddReanimation(num11, num12, 0, theReanimationType, false);
						reanimation.mLoopType = ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD;
						reanimation.mAnimRate = 18f;
						switch (i)
						{
						case 1:
						case 3:
							reanimation.OverrideScale(-1f, 1f);
							break;
						case 2:
							reanimation.SetPosition(num11, num12 - 5f);
							reanimation.OverrideScale(1.1f, 1.2f);
							break;
						case 4:
							reanimation.SetPosition(num11 - Constants.InvertAndScale(20f), num12);
							break;
						}
						if (i == 4)
						{
							mApp.PlaySample(Resources.SOUND_LOADINGBAR_FLOWER);
							mApp.PlaySample(Resources.SOUND_LOADINGBAR_ZOMBIE);
						}
						else
						{
							mApp.PlaySample(Resources.SOUND_LOADINGBAR_FLOWER);
						}
					}
				}
			}
		}

		public override void Draw(Graphics g)
		{
			g.SetLinearBlend(true);
			base.Draw(g);
			if (mTitleState == TitleState.TITLESTATE_POPCAP_LOGO)
			{
				g.SetColor(SexyColor.Black);
				g.FillRect(0, 0, mWidth, mHeight);
				int num = 50;
				int theAlpha = 255;
				if (mTitleStateCounter < mTitleStateDuration - num)
				{
					theAlpha = TodCommon.TodAnimateCurve(num, 0, mTitleStateCounter, 255, 0, TodCurves.CURVE_LINEAR);
				}
				g.SetColorizeImages(true);
				g.SetColor(new SexyColor(255, 255, 255, theAlpha));
				if (Constants.Language != Constants.LanguageIndex.de)
				{
					g.DrawImage(Resources.IMAGE_POPCAP_LOGO, (mWidth - Resources.IMAGE_POPCAP_LOGO.mWidth) / 2, (mHeight - Resources.IMAGE_POPCAP_LOGO.mHeight) / 2);
				}
				else
				{
					g.DrawImage(Resources.IMAGE_POPCAP_LOGO_REGISTERED, (mWidth - Resources.IMAGE_POPCAP_LOGO_REGISTERED.mWidth) / 2, (mHeight - Resources.IMAGE_POPCAP_LOGO_REGISTERED.mHeight) / 2);
				}
				g.SetColorizeImages(false);
				return;
			}
			if (!mLoaderScreenIsLoaded)
			{
				g.SetColor(SexyColor.Black);
				g.FillRect(0, 0, mWidth, mHeight);
				return;
			}
			PreflightNextImage(g);
			g.DrawImage(Resources.IMAGE_TITLESCREEN, 0, 0);
			if (mNeedToInit)
			{
				return;
			}
			g.DrawImage(theY: (mTitleStateCounter <= 60) ? TodCommon.TodAnimateCurve(60, 50, mTitleStateCounter, (int)Constants.InvertAndScale(10f), (int)Constants.InvertAndScale(15f), TodCurves.CURVE_BOUNCE) : TodCommon.TodAnimateCurve(100, 60, mTitleStateCounter, (int)Constants.InvertAndScale(-150f), (int)Constants.InvertAndScale(10f), TodCurves.CURVE_EASE_IN), theImage: Resources.IMAGE_PVZ_LOGO, theX: mWidth / 2 - Resources.IMAGE_PVZ_LOGO.mWidth / 2);
			int mX = mStartButton.mX;
			int num2 = mStartButton.mY - (int)Constants.InvertAndScale(34f);
			g.DrawImage(AtlasResources.IMAGE_LOADBAR_DIRT, mX, (float)num2 + Constants.InvertAndScale(18f));
			if (mCurBarWidth >= mTotalBarWidth)
			{
				g.DrawImage(AtlasResources.IMAGE_LOADBAR_GRASS, mX, num2);
				if (mLoadingThreadComplete)
				{
					DrawToPreload(g);
				}
			}
			else
			{
				Graphics @new = Graphics.GetNew(g);
				@new.ClipRect(mX, num2, (int)mCurBarWidth, AtlasResources.IMAGE_LOADBAR_GRASS.mHeight);
				@new.DrawImage(AtlasResources.IMAGE_LOADBAR_GRASS, mX, num2);
				float num3 = mCurBarWidth * 0.94f;
				float rad = (0f - num3) / 180f * (float)Math.PI * 2f;
				float num4 = TodCommon.TodAnimateCurveFloatTime(0f, mTotalBarWidth, mCurBarWidth, 1f, 0.5f, TodCurves.CURVE_LINEAR);
				SexyTransform2D sexyTransform2D = default(SexyTransform2D);
				TodCommon.TodScaleRotateTransformMatrix(ref sexyTransform2D.mMatrix, (float)mX + Constants.InvertAndScale(11f) + num3, (float)num2 - Constants.InvertAndScale(3f) - Constants.InvertAndScale(35f) * num4 + Constants.InvertAndScale(35f), rad, num4, num4);
				TodCommon.TodBltMatrix(theSrcRect: new TRect(0, 0, AtlasResources.IMAGE_REANIM_LOAD_SODROLLCAP.mWidth, AtlasResources.IMAGE_REANIM_LOAD_SODROLLCAP.mHeight), g: g, theImage: AtlasResources.IMAGE_REANIM_LOAD_SODROLLCAP, theTransform: sexyTransform2D.mMatrix, theClipRect: ref g.mClipRect, theColor: SexyColor.White, theDrawMode: g.mDrawMode);
				@new.PrepareForReuse();
			}
			foreach (Reanimation mReanimation in mApp.mEffectSystem.mReanimationHolder.mReanimations)
			{
				mReanimation.Draw(g);
			}
		}

		public override void Resize(int theX, int theY, int theWidth, int theHeight)
		{
			base.Resize(theX, theY, theWidth, theHeight);
		}

		public override void AddedToManager(WidgetManager theWidgetManager)
		{
			base.AddedToManager(theWidgetManager);
			theWidgetManager.AddWidget(mStartButton);
		}

		public override void RemovedFromManager(WidgetManager theWidgetManager)
		{
			base.RemovedFromManager(theWidgetManager);
			theWidgetManager.RemoveWidget(mStartButton);
		}

		public virtual void ButtonPress(int theId)
		{
			mApp.PlaySample(Resources.SOUND_BUTTONCLICK);
		}

		public override bool BackButtonPress()
		{
			mApp.AppExit();
			return true;
		}

		public virtual void ButtonDepress(int theId)
		{
			switch (theId)
			{
			case 0:
				mApp.LoadingCompleted();
				break;
			case 1:
				mRegisterClicked = true;
				break;
			}
		}

		public override void MouseDown(int x, int y, int theClickCount)
		{
			if (mLoadingThreadComplete)
			{
				mApp.PlaySample(Resources.SOUND_BUTTONCLICK);
				mApp.LoadingCompleted();
			}
		}

		public override void KeyDown(KeyCode theKey)
		{
			if (mLoadingThreadComplete)
			{
				mApp.PlaySample(Resources.SOUND_BUTTONCLICK);
				mApp.LoadingCompleted();
			}
			else if (mApp.mTodCheatKeys && mApp.mPlayerInfo != null)
			{
				mQuickLoadKey = theKey;
			}
		}

		public void SetRegistered()
		{
		}

		public void DrawToPreload(Graphics g)
		{
		}

		public void PreflightNextImage(Graphics g)
		{
			if (mNextImageIndex < PreflightImages.Length && PreflightImages[mNextImageIndex] != null)
			{
				g.DrawImage(PreflightImages[mNextImageIndex], 0, 0, new TRect(0, 0, 1, 1));
				mNextImageIndex++;
			}
		}
	}
}
