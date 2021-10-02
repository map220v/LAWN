using Sexy;
using Sexy.TodLib;

namespace Lawn
{
	internal class SeedChooserScreen : Widget, StoreListener, AlmanacListener, LawnMessageBoxListener, SeedPacketsWidgetListener
	{
		public GameButton mStartButton;

		public GameButton mRandomButton;

		public GameButton mViewLawnButton;

		public GameButton mStoreButton;

		public GameButton mAlmanacButton;

		public GameButton mMenuButton;

		public GameButton mImitaterButton;

		public SeedPacketsWidget mSeedPacketsWidget;

		public ScrollWidget mScrollWidget;

		public ChosenSeed[] mChosenSeeds = new ChosenSeed[53];

		public LawnApp mApp;

		public Board mBoard;

		public int mNumSeedsToChoose;

		public int mSeedChooserAge;

		public int mSeedsInFlight;

		public int mSeedsInBank;

		public int mLastMouseX;

		public int mLastMouseY;

		public SeedChooserState mChooseState;

		public int mViewLawnTime;

		public bool mDoStartButton;

		public bool[] mPickWarningsWaved = new bool[20];

		public int mPendingWarningId;

		private static TodWeightedArray[] aSeedArray;

		static SeedChooserScreen()
		{
			aSeedArray = new TodWeightedArray[53];
			for (int i = 0; i < aSeedArray.Length; i++)
			{
				aSeedArray[i] = TodWeightedArray.GetNewTodWeightedArray();
			}
		}

		public SeedChooserScreen()
		{
			mApp = (LawnApp)GlobalStaticVars.gSexyAppBase;
			mBoard = mApp.mBoard;
			mClip = false;
			mSeedsInFlight = 0;
			mSeedsInBank = 0;
			mLastMouseX = -1;
			mLastMouseY = -1;
			mChooseState = SeedChooserState.CHOOSE_NORMAL;
			mViewLawnTime = 0;
			mDoStartButton = false;
			mSeedPacketsWidget = new SeedPacketsWidget(mApp, Has12Rows() ? 12 : 11, false, this);
			mScrollWidget = new ScrollWidget();
			mScrollWidget.Resize(Constants.SCROLL_AREA_OFFSET_X, Constants.SCROLL_AREA_OFFSET_Y, mSeedPacketsWidget.mWidth + (int)Constants.InvertAndScale(10f), (int)Constants.InvertAndScale(227f));
			mScrollWidget.AddWidget(mSeedPacketsWidget);
			mScrollWidget.EnableIndicators(AtlasResources.IMAGE_SCROLL_INDICATOR);
			mSeedPacketsWidget.Move(0, 0);
			AddWidget(mScrollWidget);
			mStartButton = new GameButton(100, this);
			mStartButton.SetLabel("[PLAY_BUTTON]");
			mStartButton.mButtonImage = AtlasResources.IMAGE_SEEDCHOOSER_SMALL_BUTTON;
			mStartButton.mOverImage = null;
			mStartButton.mDownImage = AtlasResources.IMAGE_SEEDCHOOSER_SMALL_BUTTON_PRESSED;
			mStartButton.mDisabledImage = AtlasResources.IMAGE_SEEDCHOOSER_SMALL_BUTTON_DISABLED;
			mStartButton.mOverOverlayImage = null;
			mStartButton.SetFont(Resources.FONT_DWARVENTODCRAFT15);
			mStartButton.mColors[1] = new SexyColor(255, 231, 26);
			mStartButton.mColors[0] = new SexyColor(255, 231, 26);
			mStartButton.Resize((int)Constants.InvertAndScale(147f), (int)Constants.InvertAndScale(280f), (int)Constants.InvertAndScale(67f), mStartButton.mButtonImage.GetHeight());
			mStartButton.mTextOffsetX = 0;
			mStartButton.mTextOffsetX = 0;
			mStartButton.mTextPushOffsetX = 1;
			mStartButton.mTextPushOffsetY = 3;
			EnableStartButton(false);
			mMenuButton = new GameButton(104, this);
			mMenuButton.SetLabel("[MENU_BUTTON]");
			mMenuButton.Resize(Constants.UIMenuButtonPosition.X + Constants.Board_Offset_AspectRatio_Correction, Constants.UIMenuButtonPosition.Y, Constants.UIMenuButtonWidth, AtlasResources.IMAGE_BUTTON_LEFT.mHeight);
			mMenuButton.mDrawStoneButton = true;
			mRandomButton = new GameButton(101, this);
			mRandomButton.SetLabel("(Debug Play)");
			mRandomButton.mButtonImage = AtlasResources.IMAGE_BLANK;
			mRandomButton.mOverImage = AtlasResources.IMAGE_BLANK;
			mRandomButton.mDownImage = AtlasResources.IMAGE_BLANK;
			mRandomButton.SetFont(Resources.FONT_BRIANNETOD12);
			mRandomButton.mColors[0] = new SexyColor(255, 240, 0);
			mRandomButton.mColors[1] = new SexyColor(200, 200, 255);
			mRandomButton.Resize((int)Constants.InvertAndScale(332f), (int)Constants.InvertAndScale(546f), (int)Constants.InvertAndScale(100f), (int)Constants.InvertAndScale(30f));
			if (!mApp.mTodCheatKeys)
			{
				mRandomButton.mBtnNoDraw = true;
				mRandomButton.mDisabled = true;
			}
			mViewLawnButton = new GameButton(102, this);
			mViewLawnButton.SetLabel("[VIEW_LAWN]");
			mViewLawnButton.mButtonImage = AtlasResources.IMAGE_SEEDCHOOSER_BUTTON2;
			mViewLawnButton.mOverImage = AtlasResources.IMAGE_SEEDCHOOSER_BUTTON2_GLOW;
			mViewLawnButton.mDownImage = null;
			mViewLawnButton.SetFont(Resources.FONT_BRIANNETOD12);
			mViewLawnButton.mColors[0] = new SexyColor(42, 42, 90);
			mViewLawnButton.mColors[1] = new SexyColor(42, 42, 90);
			mViewLawnButton.Resize((int)Constants.InvertAndScale(22f), (int)Constants.InvertAndScale(300f), (int)Constants.InvertAndScale(111f), (int)Constants.InvertAndScale(26f));
			mViewLawnButton.mParentWidget = this;
			mViewLawnButton.mTextOffsetY = 1;
			if (!mBoard.mCutScene.IsSurvivalRepick())
			{
				mViewLawnButton.mBtnNoDraw = true;
				mViewLawnButton.mDisabled = true;
			}
			mAlmanacButton = new GameButton(103, this);
			mAlmanacButton.SetLabel("[ALMANAC_BUTTON]");
			mAlmanacButton.mButtonImage = AtlasResources.IMAGE_SEEDCHOOSER_BUTTON2;
			mAlmanacButton.mOverImage = AtlasResources.IMAGE_SEEDCHOOSER_BUTTON2_GLOW;
			mAlmanacButton.mDownImage = null;
			mAlmanacButton.SetFont(Resources.FONT_BRIANNETOD12);
			mAlmanacButton.mColors[0] = new SexyColor(42, 42, 90);
			mAlmanacButton.mColors[1] = new SexyColor(42, 42, 90);
			mAlmanacButton.Resize((int)Constants.InvertAndScale(63f), (int)Constants.InvertAndScale(286f), AtlasResources.IMAGE_SEEDCHOOSER_BUTTON2.mWidth, (int)Constants.InvertAndScale(26f));
			mAlmanacButton.mParentWidget = this;
			mAlmanacButton.mTextOffsetY = (int)Constants.InvertAndScale(2f);
			mStoreButton = new GameButton(105, this);
			mStoreButton.SetLabel("[SHOP_BUTTON]");
			mStoreButton.mButtonImage = AtlasResources.IMAGE_SEEDCHOOSER_BUTTON2;
			mStoreButton.mOverImage = AtlasResources.IMAGE_SEEDCHOOSER_BUTTON2_GLOW;
			mStoreButton.mDownImage = null;
			mStoreButton.SetFont(Resources.FONT_BRIANNETOD12);
			mStoreButton.mColors[0] = new SexyColor(42, 42, 90);
			mStoreButton.mColors[1] = new SexyColor(42, 42, 90);
			mStoreButton.Resize((int)Constants.InvertAndScale(218f), (int)Constants.InvertAndScale(286f), AtlasResources.IMAGE_SEEDCHOOSER_BUTTON2.mWidth, (int)Constants.InvertAndScale(26f));
			mStoreButton.mParentWidget = this;
			mStoreButton.mTextOffsetY = (int)Constants.InvertAndScale(2f);
			mImitaterButton = new GameButton(106, this);
			mImitaterButton.mButtonImage = null;
			mImitaterButton.mOverImage = null;
			mImitaterButton.mDownImage = null;
			mImitaterButton.mDisabledImage = null;
			mImitaterButton.Resize((int)Constants.InvertAndScale(310f), (int)Constants.InvertAndScale(27f), Constants.SMALL_SEEDPACKET_WIDTH, Constants.SMALL_SEEDPACKET_HEIGHT);
			mImitaterButton.mParentWidget = this;
			if (!mApp.CanShowAlmanac())
			{
				mAlmanacButton.mBtnNoDraw = true;
				mAlmanacButton.mDisabled = true;
			}
			if (!mApp.CanShowStore())
			{
				mStoreButton.mBtnNoDraw = true;
				mStoreButton.mDisabled = true;
			}
			for (int i = 0; i < 49; i++)
			{
				SeedType mSeedType = (SeedType)i;
				ChosenSeed chosenSeed = new ChosenSeed
				{
					mSeedType = mSeedType
				};
				GetSeedPositionInChooser(i, ref chosenSeed.mX, ref chosenSeed.mY);
				chosenSeed.mTimeStartMotion = 0;
				chosenSeed.mTimeEndMotion = 0;
				chosenSeed.mStartX = chosenSeed.mX;
				chosenSeed.mStartY = chosenSeed.mY;
				chosenSeed.mEndX = chosenSeed.mX;
				chosenSeed.mEndY = chosenSeed.mY;
				chosenSeed.mSeedState = ChosenSeedState.SEED_IN_CHOOSER;
				chosenSeed.mSeedIndexInBank = 0;
				chosenSeed.mRefreshCounter = 0;
				chosenSeed.mRefreshing = false;
				chosenSeed.mImitaterType = SeedType.SEED_NONE;
				chosenSeed.mCrazyDavePicked = false;
				if (i == 48)
				{
					chosenSeed.mSeedState = ChosenSeedState.SEED_PACKET_HIDDEN;
				}
				mChosenSeeds[i] = chosenSeed;
			}
			if (mBoard.mCutScene.IsSurvivalRepick())
			{
				for (int j = 0; j < mBoard.mSeedBank.mNumPackets; j++)
				{
					SeedPacket seedPacket = mBoard.mSeedBank.mSeedPackets[j];
					SeedType mPacketType = seedPacket.mPacketType;
					ChosenSeed chosenSeed2 = mChosenSeeds[(int)mPacketType];
					chosenSeed2.mRefreshing = seedPacket.mRefreshing;
					chosenSeed2.mRefreshCounter = seedPacket.mRefreshCounter;
				}
				mBoard.mSeedBank.mNumPackets = 0;
			}
			if (mApp.mGameMode == GameMode.GAMEMODE_CHALLENGE_SEEING_STARS)
			{
				ChosenSeed chosenSeed3 = mChosenSeeds[29];
				chosenSeed3.mY = mBoard.GetSeedPacketPositionY(0);
				chosenSeed3.mX = 0;
				chosenSeed3.mEndX = chosenSeed3.mX;
				chosenSeed3.mEndY = chosenSeed3.mY;
				chosenSeed3.mStartX = chosenSeed3.mX;
				chosenSeed3.mStartY = chosenSeed3.mY;
				chosenSeed3.mSeedState = ChosenSeedState.SEED_IN_BANK;
				chosenSeed3.mSeedIndexInBank = 0;
				mSeedsInBank++;
			}
			if (mApp.IsAdventureMode() && !mApp.IsFirstTimeAdventureMode())
			{
				CrazyDavePickSeeds();
			}
			UpdateImitaterButton();
			for (int k = 0; k < mPickWarningsWaved.Length; k++)
			{
				mPickWarningsWaved[k] = false;
			}
		}

		public override void Dispose()
		{
			mStartButton.Dispose();
			mRandomButton.Dispose();
			mViewLawnButton.Dispose();
			mAlmanacButton.Dispose();
			mImitaterButton.Dispose();
			mStoreButton.Dispose();
			mMenuButton.Dispose();
			RemoveAllWidgets(true);
		}

		public override void Update()
		{
			base.Update();
			mLastMouseX = mApp.mWidgetManager.mLastMouseX;
			mLastMouseY = mApp.mWidgetManager.mLastMouseY;
			mSeedChooserAge += 3;
			for (int i = 0; i < 49; i++)
			{
				SeedType theSeedType = (SeedType)i;
				if (mApp.HasSeedType(theSeedType))
				{
					ChosenSeed theChosenSeed = mChosenSeeds[i];
					if (theChosenSeed.mSeedState == ChosenSeedState.SEED_FLYING_TO_BANK || theChosenSeed.mSeedState == ChosenSeedState.SEED_FLYING_TO_CHOOSER)
					{
						theChosenSeed.mX = TodCommon.TodAnimateCurve(theChosenSeed.mTimeStartMotion, theChosenSeed.mTimeEndMotion, mSeedChooserAge, theChosenSeed.mStartX, theChosenSeed.mEndX, TodCurves.CURVE_EASE_IN_OUT);
						theChosenSeed.mY = TodCommon.TodAnimateCurve(theChosenSeed.mTimeStartMotion, theChosenSeed.mTimeEndMotion, mSeedChooserAge, theChosenSeed.mStartY, theChosenSeed.mEndY, TodCurves.CURVE_EASE_IN_OUT);
					}
					if (theChosenSeed.mSeedState == ChosenSeedState.SEED_FLYING_TO_BANK && mSeedChooserAge >= theChosenSeed.mTimeEndMotion)
					{
						LandFlyingSeed(ref theChosenSeed);
					}
					if (theChosenSeed.mSeedState == ChosenSeedState.SEED_FLYING_TO_CHOOSER && mSeedChooserAge >= theChosenSeed.mTimeEndMotion)
					{
						LandFlyingSeed(ref theChosenSeed);
					}
				}
			}
			mStartButton.Update();
			mRandomButton.Update();
			mViewLawnButton.Update();
			mAlmanacButton.Update();
			mImitaterButton.Update();
			mStoreButton.Update();
			mMenuButton.Update();
			UpdateViewLawn();
			if (mDoStartButton)
			{
				mDoStartButton = false;
				OnStartButton();
			}
			MarkDirty();
		}

		public override void Draw(Graphics g)
		{
			if (mApp.GetDialog(Dialogs.DIALOG_STORE) != null || mApp.GetDialog(Dialogs.DIALOG_ALMANAC) != null)
			{
				return;
			}
			g.SetLinearBlend(true);
			if (!mBoard.ChooseSeedsOnCurrentLevel() || (mBoard.mCutScene != null && mBoard.mCutScene.IsBeforePreloading()))
			{
				return;
			}
			g.DrawImage(AtlasResources.IMAGE_SEEDCHOOSER_BACKGROUND_TOP, Constants.SeedChooserScreen_Background_Top.X, Constants.SeedChooserScreen_Background_Top.Y);
			g.DrawImage(AtlasResources.IMAGE_SEEDCHOOSER_BACKGROUND_MIDDLE, Constants.SeedChooserScreen_Background_Middle.X, Constants.SeedChooserScreen_Background_Middle.Y, AtlasResources.IMAGE_SEEDCHOOSER_BACKGROUND_MIDDLE.mWidth, Constants.SeedChooserScreen_Background_Middle_Height);
			g.DrawImage(AtlasResources.IMAGE_SEEDCHOOSER_BACKGROUND_BOTTOM, Constants.SeedChooserScreen_Background_Bottom.X, Constants.SeedChooserScreen_Background_Bottom.Y);
			if (mApp.HasSeedType(SeedType.SEED_IMITATER))
			{
				g.DrawImage(AtlasResources.IMAGE_SEEDCHOOSER_IMITATERADDON, (int)Constants.InvertAndScale(304f), (int)Constants.InvertAndScale(18f));
				if (!mImitaterButton.mDisabled)
				{
					g.DrawImageCel(AtlasResources.IMAGE_SEEDPACKETS, mImitaterButton.mX, mImitaterButton.mY, 48);
				}
			}
			int mNumPackets = mBoard.mSeedBank.mNumPackets;
			for (int i = 0; i < mNumPackets; i++)
			{
				SeedType seedType = FindSeedInBank(i);
				int x = 0;
				int y = 0;
				GetSeedPositionInBank(i, ref x, ref y);
				if (seedType != SeedType.SEED_NONE)
				{
					ChosenSeed chosenSeed = mChosenSeeds[(int)seedType];
					SeedPacket.DrawSmallSeedPacket(g, x, y, seedType, chosenSeed.mImitaterType, 0f, 255, true, false, true, true);
					if (chosenSeed.mCrazyDavePicked)
					{
						g.DrawImage(AtlasResources.IMAGE_LOCK, x, y + (int)Constants.InvertAndScale(13f));
					}
				}
				else
				{
					g.DrawImage(AtlasResources.IMAGE_SEEDPACKETSILHOUETTE, x, y);
				}
			}
			DeferOverlay();
			mStartButton.Draw(g);
			mRandomButton.Draw(g);
			mViewLawnButton.Draw(g);
			mAlmanacButton.Draw(g);
			mStoreButton.Draw(g);
			Graphics @new = Graphics.GetNew(g);
			@new.mTransX -= mX;
			@new.mTransY -= mY;
			mMenuButton.Draw(@new);
			@new.PrepareForReuse();
		}

		public override void DrawOverlay(Graphics g)
		{
			g.SetColor(Constants.SeedChooserScreen_BackColour);
			g.SetColorizeImages(true);
			if (mSeedPacketsWidget.mY < 0)
			{
				g.DrawImage(AtlasResources.IMAGE_ALMANAC_PLANTS_TOPGRADIENT, mScrollWidget.mX + Constants.SeedChooserScreen_Gradient_Top.X, Constants.SeedChooserScreen_Gradient_Top.Y, Constants.SeedChooserScreen_Gradient_Top.Width, Constants.SeedChooserScreen_Gradient_Top.Height);
			}
			if ((float)(mSeedPacketsWidget.mY + mSeedPacketsWidget.mHeight) > Constants.InvertAndScale(227f))
			{
				g.DrawImage(AtlasResources.IMAGE_ALMANAC_PLANTS_BOTTOMGRADIENT, mScrollWidget.mX + Constants.SeedChooserScreen_Gradient_Bottom.X, Constants.SeedChooserScreen_Gradient_Bottom.Y, Constants.SeedChooserScreen_Gradient_Bottom.Width, Constants.SeedChooserScreen_Gradient_Bottom.Height);
			}
			g.SetColorizeImages(false);
			bool flag = false;
			for (int i = 0; i < 49; i++)
			{
				SeedType theSeedType = (SeedType)i;
				if (!mApp.HasSeedType(theSeedType))
				{
					continue;
				}
				ChosenSeed chosenSeed = mChosenSeeds[i];
				if (chosenSeed.mSeedState == ChosenSeedState.SEED_FLYING_TO_BANK)
				{
					g.SetClipRect(0, 0, mWidth, mHeight);
				}
				else
				{
					if (chosenSeed.mSeedState != ChosenSeedState.SEED_FLYING_TO_CHOOSER)
					{
						continue;
					}
					g.SetClipRect(0, 0, mWidth, mScrollWidget.mY + mScrollWidget.mHeight);
					g.HardwareClip();
					flag = true;
				}
				SeedPacket.DrawSmallSeedPacket(g, chosenSeed.mX, chosenSeed.mY, chosenSeed.mSeedType, chosenSeed.mImitaterType, 0f, 255, true, false, true, true);
			}
			if (flag)
			{
				g.EndHardwareClip();
			}
			g.SetColorizeImages(false);
		}

		public virtual void ButtonPress(int theId)
		{
		}

		public override bool BackButtonPress()
		{
			int mId = mMenuButton.mId;
			ButtonPress(mId);
			ButtonDepress(mId);
			return true;
		}

		public virtual void ButtonDepress(int theId)
		{
			if (mSeedsInFlight > 0 || mChooseState == SeedChooserState.CHOOSE_VIEW_LAWN || !mMouseVisible)
			{
				return;
			}
			switch (theId)
			{
			case 102:
				mChooseState = SeedChooserState.CHOOSE_VIEW_LAWN;
				mMenuButton.mDisabled = true;
				mViewLawnTime = 0;
				break;
			case 103:
				mApp.DoAlmanacDialog(SeedType.SEED_NONE, ZombieType.ZOMBIE_INVALID, this);
				break;
			case 105:
			{
				StoreScreen storeScreen = mApp.ShowStoreScreen(this);
				storeScreen.mBackButton.mButtonImage = AtlasResources.IMAGE_STORE_CONTINUEBUTTON;
				storeScreen.mBackButton.mDownImage = AtlasResources.IMAGE_STORE_CONTINUEBUTTONDOWN;
				return;
			}
			case 104:
				mMenuButton.mIsOver = false;
				mMenuButton.mIsDown = false;
				mApp.DoNewOptions(false);
				break;
			}
			if (mApp.GetSeedsAvailable() >= mBoard.mSeedBank.mNumPackets)
			{
				switch (theId)
				{
				case 100:
					OnStartButton();
					break;
				case 101:
					PickRandomSeeds();
					break;
				}
			}
		}

		public override void MouseDown(int x, int y, int theClickCount)
		{
			base.MouseDown(x, y, theClickCount);
			if (mSeedsInFlight > 0)
			{
				for (int i = 0; i < 49; i++)
				{
					ChosenSeed theChosenSeed = mChosenSeeds[i];
					LandFlyingSeed(ref theChosenSeed);
				}
			}
			if (mChooseState == SeedChooserState.CHOOSE_VIEW_LAWN)
			{
				CancelLawnView();
				return;
			}
			mStartButton.Update();
			mRandomButton.Update();
			mViewLawnButton.Update();
			mAlmanacButton.Update();
			mImitaterButton.Update();
			mStoreButton.Update();
			mMenuButton.Update();
			if (mRandomButton.IsMouseOver())
			{
				mApp.PlaySample(Resources.SOUND_TAP);
				ButtonDepress(101);
				return;
			}
			if (mViewLawnButton.IsMouseOver())
			{
				mApp.PlaySample(Resources.SOUND_TAP);
				ButtonDepress(102);
				return;
			}
			if (mMenuButton.IsMouseOver())
			{
				mApp.PlaySample(Resources.SOUND_GRAVEBUTTON);
			}
			if (mStartButton.IsMouseOver())
			{
				mApp.PlaySample(Resources.SOUND_TAP);
			}
			if (mAlmanacButton.IsMouseOver())
			{
				mApp.PlaySample(Resources.SOUND_TAP);
			}
			if (mStoreButton.IsMouseOver())
			{
				mApp.PlaySample(Resources.SOUND_TAP);
			}
			if (mImitaterButton.IsMouseOver())
			{
				if (mSeedsInBank != mBoard.mSeedBank.mNumPackets)
				{
					mApp.PlaySample(Resources.SOUND_TAP);
					ImitaterDialog imitaterDialog = new ImitaterDialog();
					mApp.AddDialog(imitaterDialog.mId, imitaterDialog);
					imitaterDialog.Resize((mWidth - imitaterDialog.mWidth) / 2, (mHeight - imitaterDialog.mHeight) / 2, imitaterDialog.mWidth, imitaterDialog.mHeight);
					mApp.mWidgetManager.SetFocus(imitaterDialog);
				}
				return;
			}
			int num = 0;
			ChosenSeed theChosenSeed2;
			while (true)
			{
				if (num < 49)
				{
					theChosenSeed2 = mApp.mSeedChooserScreen.mChosenSeeds[num];
					if (theChosenSeed2.mSeedState == ChosenSeedState.SEED_IN_BANK && x >= theChosenSeed2.mX && y >= theChosenSeed2.mY && x < theChosenSeed2.mX + Constants.SMALL_SEEDPACKET_WIDTH && y < theChosenSeed2.mY + Constants.SMALL_SEEDPACKET_HEIGHT)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			ClickedSeedInBank(ref theChosenSeed2);
		}

		public override void MouseUp(int x, int y, int theClickCount)
		{
			if (theClickCount == 1)
			{
				if (mMenuButton.IsMouseOver())
				{
					ButtonDepress(104);
				}
				else if (mStartButton.IsMouseOver())
				{
					ButtonDepress(100);
				}
				else if (mAlmanacButton.IsMouseOver())
				{
					ButtonDepress(103);
				}
				else if (mStoreButton.IsMouseOver())
				{
					ButtonDepress(105);
				}
				else if (mImitaterButton.IsMouseOver())
				{
					ButtonDepress(106);
				}
			}
		}

		public override void KeyChar(SexyChar theChar)
		{
			if (mChooseState == SeedChooserState.CHOOSE_VIEW_LAWN && (theChar.value_type == ' ' || theChar.value_type == '\r' || theChar.value_type == '\u001b'))
			{
				CancelLawnView();
			}
			else if (mApp.mTodCheatKeys && theChar == KeyCode.KEYCODE_ESCAPE)
			{
				PickRandomSeeds();
			}
			else
			{
				mBoard.KeyChar(theChar);
			}
		}

		public override void KeyDown(KeyCode theKey)
		{
			mBoard.DoTypingCheck(theKey);
		}

		public void GetSeedPositionInChooser(int theIndex, ref int x, ref int y)
		{
			if (theIndex == 48)
			{
				x = mImitaterButton.mX;
				y = mImitaterButton.mY;
			}
			else
			{
				mSeedPacketsWidget.GetSeedPosition((SeedType)theIndex, ref x, ref y);
				x += Constants.SCROLL_AREA_OFFSET_X;
				y += Constants.SCROLL_AREA_OFFSET_Y;
			}
		}

		public void GetSeedPositionInBank(int theIndex, ref int x, ref int y)
		{
			y = mBoard.mSeedBank.mY + mBoard.GetSeedPacketPositionY(theIndex) - mY;
			x = mBoard.mSeedBank.mX;
		}

		public void ClickedSeedInChooser(ref ChosenSeed theChosenSeed)
		{
			if (mSeedsInBank == mBoard.mSeedBank.mNumPackets || !mApp.HasSeedType(theChosenSeed.mSeedType))
			{
				return;
			}
			if (mApp.mPlayerInfo.mNeedsGrayedPlantWarning && SeedNotRecommendedToPick(theChosenSeed.mSeedType) != 0)
			{
				mApp.mPlayerInfo.mNeedsGrayedPlantWarning = false;
				mApp.DoDialog(16, true, "[DIALOG_WARNING]", "[NOT_RECOMMENDED_FOR_LEVEL]", "[DIALOG_BUTTON_OK]", 3);
				return;
			}
			theChosenSeed.mTimeStartMotion = mSeedChooserAge;
			theChosenSeed.mTimeEndMotion = mSeedChooserAge + 25;
			GetSeedPositionInChooser((int)theChosenSeed.mSeedType, ref theChosenSeed.mStartX, ref theChosenSeed.mStartY);
			if (theChosenSeed.mSeedType != SeedType.SEED_IMITATER)
			{
				theChosenSeed.mStartY += mSeedPacketsWidget.mY;
			}
			GetSeedPositionInBank(mSeedsInBank, ref theChosenSeed.mEndX, ref theChosenSeed.mEndY);
			theChosenSeed.mSeedState = ChosenSeedState.SEED_FLYING_TO_BANK;
			theChosenSeed.mSeedIndexInBank = mSeedsInBank;
			mSeedsInFlight++;
			mSeedsInBank++;
			mApp.PlaySample(Resources.SOUND_TAP);
			if (mSeedsInBank == mBoard.mSeedBank.mNumPackets)
			{
				EnableStartButton(true);
			}
		}

		public void ClickedSeedInBank(ref ChosenSeed theChosenSeed)
		{
			if (theChosenSeed.mSeedState == ChosenSeedState.SEED_IN_BANK && theChosenSeed.mCrazyDavePicked)
			{
				mApp.PlaySample(Resources.SOUND_BUZZER);
				return;
			}
			for (int i = theChosenSeed.mSeedIndexInBank + 1; i < mBoard.mSeedBank.mNumPackets; i++)
			{
				SeedType seedType = FindSeedInBank(i);
				if (seedType != SeedType.SEED_NONE)
				{
					ChosenSeed chosenSeed = mChosenSeeds[(int)seedType];
					chosenSeed.mTimeStartMotion = mSeedChooserAge;
					chosenSeed.mTimeEndMotion = mSeedChooserAge + 15;
					chosenSeed.mStartX = chosenSeed.mX;
					chosenSeed.mStartY = chosenSeed.mY;
					GetSeedPositionInBank(i - 1, ref chosenSeed.mEndX, ref chosenSeed.mEndY);
					chosenSeed.mSeedState = ChosenSeedState.SEED_FLYING_TO_BANK;
					chosenSeed.mSeedIndexInBank = i - 1;
					mSeedsInFlight++;
				}
			}
			theChosenSeed.mTimeStartMotion = mSeedChooserAge;
			theChosenSeed.mTimeEndMotion = mSeedChooserAge + 25;
			theChosenSeed.mStartX = theChosenSeed.mX;
			theChosenSeed.mStartY = theChosenSeed.mY;
			GetSeedPositionInChooser((int)theChosenSeed.mSeedType, ref theChosenSeed.mEndX, ref theChosenSeed.mEndY);
			if (theChosenSeed.mSeedType != SeedType.SEED_IMITATER)
			{
				theChosenSeed.mEndY += mSeedPacketsWidget.mY;
			}
			theChosenSeed.mSeedState = ChosenSeedState.SEED_FLYING_TO_CHOOSER;
			theChosenSeed.mSeedIndexInBank = 0;
			mSeedsInFlight++;
			mSeedsInBank--;
			EnableStartButton(false);
			mApp.PlaySample(Resources.SOUND_TAP);
		}

		public SeedType FindSeedInBank(int theIndexInBank)
		{
			for (int i = 0; i < 49; i++)
			{
				SeedType seedType = (SeedType)i;
				if (mApp.HasSeedType(seedType))
				{
					ChosenSeed chosenSeed = mChosenSeeds[i];
					if (chosenSeed.mSeedState == ChosenSeedState.SEED_IN_BANK && chosenSeed.mSeedIndexInBank == theIndexInBank)
					{
						return seedType;
					}
				}
			}
			return SeedType.SEED_NONE;
		}

		public void EnableStartButton(bool theEnabled)
		{
			mStartButton.SetDisabled(!theEnabled);
			if (theEnabled)
			{
				mStartButton.mColors[0] = new SexyColor(255, 231, 26);
			}
			else
			{
				mStartButton.mColors[0] = new SexyColor(64, 64, 64);
			}
		}

		public uint SeedNotRecommendedToPick(SeedType theSeedType)
		{
			uint theNumber = mBoard.SeedNotRecommendedForLevel(theSeedType);
			if (TodCommon.TestBit(theNumber, 0) && PickedPlantType(SeedType.SEED_INSTANT_COFFEE))
			{
				TodCommon.SetBit(ref theNumber, 0, 0);
			}
			return theNumber;
		}

		public bool SeedNotAllowedToPick(SeedType theSeedType)
		{
			if (mApp.mGameMode == GameMode.GAMEMODE_CHALLENGE_LAST_STAND && (theSeedType == SeedType.SEED_SUNFLOWER || theSeedType == SeedType.SEED_SUNSHROOM || theSeedType == SeedType.SEED_TWINSUNFLOWER || theSeedType == SeedType.SEED_SEASHROOM || theSeedType == SeedType.SEED_PUFFSHROOM))
			{
				return true;
			}
			return false;
		}

		public void CloseSeedChooser()
		{
			Debug.ASSERT(mBoard.mSeedBank.mNumPackets == mBoard.GetNumSeedsInBank());
			for (int i = 0; i < mBoard.mSeedBank.mNumPackets; i++)
			{
				SeedType seedType = FindSeedInBank(i);
				ChosenSeed chosenSeed = mChosenSeeds[(int)seedType];
				SeedPacket seedPacket = mBoard.mSeedBank.mSeedPackets[i];
				seedPacket.SetPacketType(seedType, chosenSeed.mImitaterType);
				if (chosenSeed.mRefreshing)
				{
					seedPacket.mRefreshCounter = mChosenSeeds[(int)seedType].mRefreshCounter;
					seedPacket.mRefreshTime = Plant.GetRefreshTime(seedPacket.mPacketType, seedPacket.mImitaterType);
					seedPacket.mRefreshing = true;
					seedPacket.mActive = false;
				}
			}
			mBoard.mCutScene.EndSeedChooser();
		}

		public bool PickedPlantType(SeedType theSeedType)
		{
			for (int i = 0; i < 49; i++)
			{
				ChosenSeed chosenSeed = mChosenSeeds[i];
				if (chosenSeed.mSeedState == ChosenSeedState.SEED_IN_BANK)
				{
					if (chosenSeed.mSeedType == theSeedType)
					{
						return true;
					}
					if (chosenSeed.mSeedType == SeedType.SEED_IMITATER && chosenSeed.mImitaterType == theSeedType)
					{
						return true;
					}
				}
			}
			return false;
		}

		public void OnStartButton()
		{
			if ((mApp.mGameMode == GameMode.GAMEMODE_CHALLENGE_SEEING_STARS && !PickedPlantType(SeedType.SEED_STARFRUIT) && !DisplayRepickWarningDialog(0, "[SEED_CHOOSER_SEEING_STARS_WARNING]")) || (mApp.IsFirstTimeAdventureMode() && mBoard.mLevel == 11 && !PickedPlantType(SeedType.SEED_PUFFSHROOM) && !DisplayRepickWarningDialog(1, "[SEED_CHOOSER_PUFFSHROOM_WARNING]")))
			{
				return;
			}
			if (!PickedPlantType(SeedType.SEED_SUNFLOWER) && !PickedPlantType(SeedType.SEED_TWINSUNFLOWER) && !PickedPlantType(SeedType.SEED_SUNSHROOM) && !mBoard.mCutScene.IsSurvivalRepick() && mApp.mGameMode != GameMode.GAMEMODE_CHALLENGE_LAST_STAND)
			{
				if (mApp.IsFirstTimeAdventureMode() && mBoard.mLevel == 11)
				{
					if (!DisplayRepickWarningDialog(2, "[SEED_CHOOSER_NIGHT_SUN_WARNING]"))
					{
						return;
					}
				}
				else if (!DisplayRepickWarningDialog(3, "[SEED_CHOOSER_SUN_WARNING]"))
				{
					return;
				}
			}
			if (mBoard.StageHasPool() && !PickedPlantType(SeedType.SEED_LILYPAD) && !PickedPlantType(SeedType.SEED_SEASHROOM) && !PickedPlantType(SeedType.SEED_TANGLEKELP) && !mBoard.mCutScene.IsSurvivalRepick())
			{
				if (mApp.IsFirstTimeAdventureMode() && mBoard.mLevel == 21)
				{
					if (!DisplayRepickWarningDialog(4, "[SEED_CHOOSER_LILY_WARNING]"))
					{
						return;
					}
				}
				else if (!DisplayRepickWarningDialog(5, "[SEED_CHOOSER_POOL_WARNING]"))
				{
					return;
				}
			}
			if ((!mBoard.StageHasRoof() || PickedPlantType(SeedType.SEED_FLOWERPOT) || !mApp.HasSeedType(SeedType.SEED_FLOWERPOT) || DisplayRepickWarningDialog(6, "[SEED_CHOOSER_ROOF_WARNING]")) && (mApp.mGameMode != GameMode.GAMEMODE_CHALLENGE_ART_CHALLENGE_1 || PickedPlantType(SeedType.SEED_WALLNUT) || DisplayRepickWarningDialog(7, "[SEED_CHOOSER_ART_WALLNUT_WARNING]")) && (mApp.mGameMode != GameMode.GAMEMODE_CHALLENGE_ART_CHALLENGE_2 || (PickedPlantType(SeedType.SEED_STARFRUIT) && PickedPlantType(SeedType.SEED_UMBRELLA) && PickedPlantType(SeedType.SEED_WALLNUT)) || DisplayRepickWarningDialog(8, "[SEED_CHOOSER_ART_2_WARNING]")) && (!FlyersAreComing() || FlyProtectionCurrentlyPlanted() || PickedPlantType(SeedType.SEED_CATTAIL) || PickedPlantType(SeedType.SEED_CACTUS) || PickedPlantType(SeedType.SEED_BLOVER) || DisplayRepickWarningDialog(9, "[SEED_CHOOSER_FLYER_WARNING]")) && CheckSeedUpgrade(10, SeedType.SEED_GATLINGPEA, SeedType.SEED_REPEATER) && CheckSeedUpgrade(11, SeedType.SEED_WINTERMELON, SeedType.SEED_MELONPULT) && CheckSeedUpgrade(12, SeedType.SEED_TWINSUNFLOWER, SeedType.SEED_SUNFLOWER) && CheckSeedUpgrade(13, SeedType.SEED_SPIKEROCK, SeedType.SEED_SPIKEWEED) && CheckSeedUpgrade(14, SeedType.SEED_COBCANNON, SeedType.SEED_KERNELPULT) && CheckSeedUpgrade(15, SeedType.SEED_GOLD_MAGNET, SeedType.SEED_MAGNETSHROOM) && CheckSeedUpgrade(16, SeedType.SEED_GLOOMSHROOM, SeedType.SEED_FUMESHROOM) && CheckSeedUpgrade(17, SeedType.SEED_CATTAIL, SeedType.SEED_LILYPAD))
			{
				CloseSeedChooser();
			}
		}

		public bool DisplayRepickWarningDialog(int theWarningId, string theMessage)
		{
			if (mPickWarningsWaved[theWarningId])
			{
				return true;
			}
			mPendingWarningId = theWarningId;
			mApp.LawnMessageBox(28, "[DIALOG_WARNING]", theMessage, "[DIALOG_BUTTON_YES]", "[REPICK_BUTTON]", 1, this);
			return false;
		}

		public void UpdateViewLawn()
		{
			if (mChooseState == SeedChooserState.CHOOSE_VIEW_LAWN)
			{
				mViewLawnTime++;
				if (mViewLawnTime == 100)
				{
					mBoard.DisplayAdviceAgain("[CLICK_TO_CONTINUE]", MessageStyle.MESSAGE_STYLE_HINT_STAY, AdviceType.ADVICE_CLICK_TO_CONTINUE);
				}
				else if (mViewLawnTime == 251)
				{
					mViewLawnTime = 250;
				}
				int num = 800;
				if (mViewLawnTime <= 100)
				{
					int thePositionStart = -Constants.BOARD_OFFSET + Constants.BACKGROUND_IMAGE_WIDTH - num;
					int thePositionEnd = 0;
					int num2 = TodCommon.TodAnimateCurve(0, 100, mViewLawnTime, thePositionStart, thePositionEnd, TodCurves.CURVE_EASE_IN_OUT);
					mBoard.Move((int)((float)(-num2) * Constants.S), 0);
					int thePositionStart2 = Constants.SEED_CHOOSER_OFFSETSCREEN_OFFSET - 265;
					int sEED_CHOOSER_OFFSETSCREEN_OFFSET = Constants.SEED_CHOOSER_OFFSETSCREEN_OFFSET;
					int num3 = TodCommon.TodAnimateCurve(0, 40, mViewLawnTime, thePositionStart2, sEED_CHOOSER_OFFSETSCREEN_OFFSET, TodCurves.CURVE_EASE_IN_OUT);
					Move(0, (int)((float)num3 * Constants.S));
				}
				else if (mViewLawnTime > 100 && mViewLawnTime <= 250)
				{
					mBoard.Move(0, 0);
					Move(0, Constants.SEED_CHOOSER_OFFSETSCREEN_OFFSET);
				}
				else if (mViewLawnTime > 250 && mViewLawnTime <= 350)
				{
					mBoard.ClearAdvice(AdviceType.ADVICE_CLICK_TO_CONTINUE);
					int thePositionStart3 = 0;
					int thePositionEnd2 = -Constants.BOARD_OFFSET + Constants.BACKGROUND_IMAGE_WIDTH - num;
					int num4 = TodCommon.TodAnimateCurve(250, 350, mViewLawnTime, thePositionStart3, thePositionEnd2, TodCurves.CURVE_EASE_IN_OUT);
					mBoard.Move((int)((float)(-num4) * Constants.S), 0);
					int sEED_CHOOSER_OFFSETSCREEN_OFFSET2 = Constants.SEED_CHOOSER_OFFSETSCREEN_OFFSET;
					int thePositionEnd3 = Constants.SEED_CHOOSER_OFFSETSCREEN_OFFSET - 265;
					int num5 = TodCommon.TodAnimateCurve(310, 350, mViewLawnTime, sEED_CHOOSER_OFFSETSCREEN_OFFSET2, thePositionEnd3, TodCurves.CURVE_EASE_IN_OUT);
					Move(0, (int)((float)num5 * Constants.S));
				}
				else
				{
					mChooseState = SeedChooserState.CHOOSE_NORMAL;
					mViewLawnTime = 0;
					mMenuButton.mDisabled = false;
				}
			}
		}

		public void CancelLawnView()
		{
			if (mChooseState == SeedChooserState.CHOOSE_VIEW_LAWN && mViewLawnTime > 100 && mViewLawnTime <= 250)
			{
				mViewLawnTime = 251;
			}
		}

		public void LandFlyingSeed(ref ChosenSeed theChosenSeed)
		{
			if (theChosenSeed.mSeedState == ChosenSeedState.SEED_FLYING_TO_BANK)
			{
				theChosenSeed.mTimeStartMotion = 0;
				theChosenSeed.mTimeEndMotion = 0;
				theChosenSeed.mSeedState = ChosenSeedState.SEED_IN_BANK;
				theChosenSeed.mX = theChosenSeed.mEndX;
				theChosenSeed.mY = theChosenSeed.mEndY;
				mSeedsInFlight--;
			}
			else if (theChosenSeed.mSeedState == ChosenSeedState.SEED_FLYING_TO_CHOOSER)
			{
				theChosenSeed.mTimeStartMotion = 0;
				theChosenSeed.mTimeEndMotion = 0;
				theChosenSeed.mSeedState = ChosenSeedState.SEED_IN_CHOOSER;
				theChosenSeed.mX = theChosenSeed.mEndX;
				theChosenSeed.mY = theChosenSeed.mEndY;
				mSeedsInFlight--;
				if (theChosenSeed.mSeedType == SeedType.SEED_IMITATER)
				{
					theChosenSeed.mSeedState = ChosenSeedState.SEED_PACKET_HIDDEN;
					theChosenSeed.mImitaterType = SeedType.SEED_NONE;
					UpdateImitaterButton();
				}
			}
		}

		public bool FlyProtectionCurrentlyPlanted()
		{
			int count = mBoard.mPlants.Count;
			for (int i = 0; i < count; i++)
			{
				Plant plant = mBoard.mPlants[i];
				if (!plant.mDead && (plant.mSeedType == SeedType.SEED_CATTAIL || plant.mSeedType == SeedType.SEED_CACTUS))
				{
					return true;
				}
			}
			return false;
		}

		public bool FlyersAreComing()
		{
			for (int i = 0; i < mBoard.mNumWaves; i++)
			{
				for (int j = 0; j < 50; j++)
				{
					switch (mBoard.mZombiesInWave[i, j])
					{
					case ZombieType.ZOMBIE_BALLOON:
						return true;
					default:
						continue;
					case ZombieType.ZOMBIE_INVALID:
						break;
					}
					break;
				}
			}
			return false;
		}

		public void UpdateAfterPurchase()
		{
			for (int i = 0; i < 49; i++)
			{
				ChosenSeed chosenSeed = mChosenSeeds[i];
				if (chosenSeed.mSeedState == ChosenSeedState.SEED_IN_BANK)
				{
					GetSeedPositionInBank(chosenSeed.mSeedIndexInBank, ref chosenSeed.mX, ref chosenSeed.mY);
					chosenSeed.mStartX = chosenSeed.mX;
					chosenSeed.mStartY = chosenSeed.mY;
					chosenSeed.mEndX = chosenSeed.mX;
					chosenSeed.mEndY = chosenSeed.mY;
				}
				else if (chosenSeed.mSeedState == ChosenSeedState.SEED_IN_CHOOSER)
				{
					GetSeedPositionInChooser(i, ref chosenSeed.mX, ref chosenSeed.mY);
					chosenSeed.mStartX = chosenSeed.mX;
					chosenSeed.mStartY = chosenSeed.mY;
					chosenSeed.mEndX = chosenSeed.mX;
					chosenSeed.mEndY = chosenSeed.mY;
				}
			}
			if (mSeedsInBank == mBoard.mSeedBank.mNumPackets)
			{
				EnableStartButton(true);
			}
			else
			{
				EnableStartButton(false);
			}
			UpdateImitaterButton();
		}

		public void UpdateImitaterButton()
		{
			if (!mApp.HasSeedType(SeedType.SEED_IMITATER))
			{
				mImitaterButton.mBtnNoDraw = true;
				mImitaterButton.mDisabled = true;
				return;
			}
			mImitaterButton.mBtnNoDraw = false;
			bool disabled = false;
			ChosenSeed chosenSeed = mChosenSeeds[48];
			if (chosenSeed.mSeedState != ChosenSeedState.SEED_PACKET_HIDDEN)
			{
				disabled = true;
			}
			mImitaterButton.SetDisabled(disabled);
		}

		public void CrazyDavePickSeeds()
		{
			for (int i = 0; i < aSeedArray.Length; i++)
			{
				aSeedArray[i].Reset();
			}
			for (int j = 0; j < 49; j++)
			{
				SeedType seedType = (SeedType)j;
				aSeedArray[j].mItem = (int)seedType;
				uint num = SeedNotRecommendedToPick(seedType);
				if (!mApp.HasSeedType(seedType) || num != 0 || SeedNotAllowedToPick(seedType) || Plant.IsUpgrade(seedType) || seedType == SeedType.SEED_IMITATER || seedType == SeedType.SEED_UMBRELLA || seedType == SeedType.SEED_BLOVER)
				{
					aSeedArray[j].mWeight = 0;
				}
				else
				{
					aSeedArray[j].mWeight = 1;
				}
			}
			if (mBoard.mZombieAllowed[22] || mBoard.mZombieAllowed[20])
			{
				aSeedArray[37].mWeight = 1;
			}
			if (mBoard.mZombieAllowed[16] || mBoard.StageHasFog())
			{
				aSeedArray[27].mWeight = 1;
			}
			if (mBoard.StageHasRoof())
			{
				aSeedArray[22].mWeight = 0;
			}
			int levelRandSeed = mBoard.GetLevelRandSeed();
			RandomNumbers.Seed(levelRandSeed);
			for (int k = 0; k < 3; k++)
			{
				SeedType seedType2 = (SeedType)PickFromWeightedArrayUsingSpecialRandSeed(aSeedArray, 49);
				aSeedArray[(int)seedType2].mWeight = 0;
				ChosenSeed chosenSeed = mChosenSeeds[(int)seedType2];
				chosenSeed.mY = mBoard.GetSeedPacketPositionY(k);
				chosenSeed.mX = 0;
				chosenSeed.mEndX = chosenSeed.mX;
				chosenSeed.mEndY = chosenSeed.mY;
				chosenSeed.mStartX = chosenSeed.mX;
				chosenSeed.mStartY = chosenSeed.mY;
				chosenSeed.mSeedState = ChosenSeedState.SEED_IN_BANK;
				chosenSeed.mSeedIndexInBank = k;
				chosenSeed.mCrazyDavePicked = true;
				mSeedsInBank++;
			}
		}

		public int PickFromWeightedArrayUsingSpecialRandSeed(TodWeightedArray[] theArray, int theCount)
		{
			int num = 0;
			for (int i = 0; i < theCount; i++)
			{
				num += theArray[i].mWeight;
			}
			Debug.ASSERT(num > 0);
			int num2 = (int)RandomNumbers.NextNumber((float)(double)(uint)num);
			int num3 = 0;
			for (int j = 0; j < theCount; j++)
			{
				num3 += theArray[j].mWeight;
				if (num2 < num3)
				{
					return (int)theArray[j].mItem;
				}
			}
			Debug.ASSERT(false);
			return -666;
		}

		public bool CheckSeedUpgrade(int theWarningId, SeedType theSeedTypeTo, SeedType theSeedTypeFrom)
		{
			if (mApp.IsSurvivalMode() || mPickWarningsWaved[theWarningId])
			{
				return true;
			}
			if (PickedPlantType(theSeedTypeTo) && !PickedPlantType(theSeedTypeFrom))
			{
				string theText = TodStringFile.TodStringTranslate("[SEED_CHOOSER_UPGRADE_WARNING]");
				string nameString = Plant.GetNameString(theSeedTypeTo, SeedType.SEED_NONE);
				string nameString2 = Plant.GetNameString(theSeedTypeFrom, SeedType.SEED_NONE);
				theText = TodCommon.TodReplaceString(theText, "{UPGRADE_TO}", nameString);
				theText = TodCommon.TodReplaceString(theText, "{UPGRADE_FROM}", nameString2);
				if (!DisplayRepickWarningDialog(theWarningId, theText))
				{
					return false;
				}
			}
			return true;
		}

		public void PickRandomSeeds()
		{
			for (int i = mSeedsInBank; i < mBoard.mSeedBank.mNumPackets; i++)
			{
				int num;
				SeedType seedType;
				do
				{
					num = RandomNumbers.NextNumber(mApp.GetSeedsAvailable());
					seedType = (SeedType)num;
				}
				while (!mApp.HasSeedType(seedType) || seedType == SeedType.SEED_IMITATER || mChosenSeeds[num].mSeedState != ChosenSeedState.SEED_IN_CHOOSER);
				ChosenSeed chosenSeed = mChosenSeeds[num];
				chosenSeed.mTimeStartMotion = 0;
				chosenSeed.mTimeEndMotion = 0;
				chosenSeed.mStartX = chosenSeed.mX;
				chosenSeed.mStartY = chosenSeed.mY;
				GetSeedPositionInBank(i, ref chosenSeed.mEndX, ref chosenSeed.mEndY);
				chosenSeed.mSeedState = ChosenSeedState.SEED_IN_BANK;
				chosenSeed.mSeedIndexInBank = i;
				mSeedsInBank++;
			}
			for (int j = 0; j < 49; j++)
			{
				ChosenSeed theChosenSeed = mChosenSeeds[j];
				LandFlyingSeed(ref theChosenSeed);
			}
			CloseSeedChooser();
		}

		public bool Has12Rows()
		{
			if (mApp.HasFinishedAdventure())
			{
				return true;
			}
			if (mApp.HasSeedType(SeedType.SEED_GATLINGPEA) || mApp.HasSeedType(SeedType.SEED_TWINSUNFLOWER) || mApp.HasSeedType(SeedType.SEED_GLOOMSHROOM) || mApp.HasSeedType(SeedType.SEED_CATTAIL) || mApp.HasSeedType(SeedType.SEED_WINTERMELON) || mApp.HasSeedType(SeedType.SEED_GATLINGPEA) || mApp.HasSeedType(SeedType.SEED_GOLD_MAGNET) || mApp.HasSeedType(SeedType.SEED_COBCANNON))
			{
				return true;
			}
			return false;
		}

		public bool SeedNotAllowedDuringTrial(SeedType theSeedType)
		{
			if (mApp.IsTrialStageLocked() && (theSeedType == SeedType.SEED_SQUASH || theSeedType == SeedType.SEED_THREEPEATER))
			{
				return true;
			}
			return false;
		}

		public void BackFromStore()
		{
			StoreScreen storeScreen = (StoreScreen)mApp.GetDialog(Dialogs.DIALOG_STORE);
			mApp.KillDialog(4);
			mApp.mMusic.MakeSureMusicIsPlaying(MusicTune.MUSIC_TUNE_CHOOSE_YOUR_SEEDS);
		}

		public void BackFromAlmanac()
		{
			mApp.mMusic.MakeSureMusicIsPlaying(MusicTune.MUSIC_TUNE_CHOOSE_YOUR_SEEDS);
		}

		public void LawnMessageBoxDone(int theResult)
		{
			if (theResult == 1000)
			{
				mPickWarningsWaved[mPendingWarningId] = true;
				mDoStartButton = true;
			}
		}

		public void SeedSelected(SeedType theSeedType)
		{
			if (theSeedType == SeedType.SEED_NONE)
			{
				return;
			}
			if (SeedNotAllowedToPick(theSeedType))
			{
				mApp.DoDialog(16, true, string.Empty, "[NOT_ALLOWED_ON_THIS_LEVEL]", "[DIALOG_BUTTON_OK]", 3);
			}
			else if (theSeedType >= SeedType.SEED_PEASHOOTER && (int)theSeedType < mChosenSeeds.Length)
			{
				ChosenSeed theChosenSeed = mChosenSeeds[(int)theSeedType];
				if (theChosenSeed.mSeedState == ChosenSeedState.SEED_IN_CHOOSER)
				{
					ClickedSeedInChooser(ref theChosenSeed);
				}
			}
		}
	}
}
