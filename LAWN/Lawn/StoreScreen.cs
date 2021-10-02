using Sexy;
using Sexy.TodLib;

namespace Lawn
{
	internal class StoreScreen : Dialog
	{
		private const int aNumpicks = 4;

		public LawnApp mApp;

		public NewLawnButton mBackButton;

		public NewLawnButton mPrevButton;

		public NewLawnButton mNextButton;

		public Widget mOverlayWidget;

		public int mStoreTime;

		public string mBubbleText = string.Empty;

		public int mBubbleCountDown;

		public bool mBubbleClickToContinue;

		public bool mBubbleAutoAdvance;

		public int mAmbientSpeechCountDown;

		public int mPreviousAmbientSpeechIndex;

		public StorePage mPage;

		public StoreItem mMouseOverItem;

		public int mHatchTimer;

		public bool mHatchOpen;

		public int mShakeX;

		public int mShakeY;

		public int mStartDialog;

		public bool mEasyBuyingCheat;

		public PottedPlant mPottedPlantSpecs = new PottedPlant();

		public static DataArray<Coin> mCoins;

		public bool mDrawnOnce;

		public bool mGoToTreeNow;

		public bool mPurchasedFullVersion;

		public bool mTrialLockedWhenStoreOpened;

		public StoreListener mListener;

		public StoreItem mPendingPurchaseItem;

		private static TodWeightedArray[] aPickArray;

		static StoreScreen()
		{
			mCoins = new DataArray<Coin>();
			aPickArray = new TodWeightedArray[4];
			for (int i = 0; i < aPickArray.Length; i++)
			{
				aPickArray[i] = TodWeightedArray.GetNewTodWeightedArray();
			}
		}

		public StoreScreen(LawnApp theApp, StoreListener theListener)
			: base(null, null, 4, true, "[STORE]", "", "", 0)
		{
			mListener = theListener;
			mApp = theApp;
			mClip = false;
			mStoreTime = 0;
			mBubbleCountDown = 0;
			mBubbleClickToContinue = false;
			mBubbleAutoAdvance = false;
			mAmbientSpeechCountDown = 200;
			mPreviousAmbientSpeechIndex = -1;
			mPage = StorePage.STORE_PAGE_SLOT_UPGRADES;
			mMouseOverItem = StoreItem.STORE_ITEM_INVALID;
			mHatchTimer = 0;
			mShakeX = 0;
			mShakeY = 0;
			mStartDialog = -1;
			mHatchOpen = true;
			mEasyBuyingCheat = false;
			mCoins.DataArrayInitialize(1024u, "coins");
			mApp.DelayLoadStoreResource("DelayLoad_Store");
			Resize(0, 0, Constants.BOARD_WIDTH, Constants.BOARD_HEIGHT);
			mPottedPlantSpecs.InitializePottedPlant(SeedType.SEED_MARIGOLD);
			mPottedPlantSpecs.mDrawVariation = (DrawVariation)TodCommon.RandRangeInt(2, 12);
			mBackButton = new NewLawnButton(null, 100, this);
			mBackButton.mDoFinger = true;
			mBackButton.mLabel = "";
			mBackButton.mButtonImage = AtlasResources.IMAGE_STORE_MAINMENUBUTTON;
			mBackButton.mDownImage = AtlasResources.IMAGE_STORE_MAINMENUBUTTONDOWN;
			mBackButton.mColors[0] = new SexyColor(98, 153, 235);
			mBackButton.mColors[1] = new SexyColor(167, 192, 235);
			mBackButton.Resize(Constants.StoreScreen_BackButton_X, Constants.StoreScreen_BackButton_Y, AtlasResources.IMAGE_STORE_MAINMENUBUTTON.mWidth, AtlasResources.IMAGE_STORE_MAINMENUBUTTON.mHeight);
			mBackButton.mTextOffsetX = -7;
			mBackButton.mTextOffsetY = 1;
			mBackButton.mTextDownOffsetX = 2;
			mBackButton.mTextDownOffsetY = 1;
			mPrevButton = new NewLawnButton(null, 101, this);
			mPrevButton.mDoFinger = true;
			mPrevButton.mLabel = "";
			mPrevButton.mButtonImage = AtlasResources.IMAGE_STORE_PREVBUTTON;
			mPrevButton.mOverImage = AtlasResources.IMAGE_STORE_PREVBUTTONHIGHLIGHT;
			mPrevButton.mDownImage = AtlasResources.IMAGE_STORE_PREVBUTTONHIGHLIGHT;
			mPrevButton.mColors[0] = new SexyColor(255, 240, 0);
			mPrevButton.mColors[1] = new SexyColor(200, 200, 255);
			mPrevButton.Resize(Constants.StoreScreen_PrevButton_X, Constants.StoreScreen_PrevButton_Y, AtlasResources.IMAGE_STORE_PREVBUTTON.mWidth, AtlasResources.IMAGE_STORE_PREVBUTTON.mHeight);
			mNextButton = new NewLawnButton(null, 102, this);
			mNextButton.mDoFinger = true;
			mNextButton.mLabel = "";
			mNextButton.mButtonImage = AtlasResources.IMAGE_STORE_NEXTBUTTON;
			mNextButton.mOverImage = AtlasResources.IMAGE_STORE_NEXTBUTTONHIGHLIGHT;
			mNextButton.mDownImage = AtlasResources.IMAGE_STORE_NEXTBUTTONHIGHLIGHT;
			mNextButton.mColors[0] = new SexyColor(255, 240, 0);
			mNextButton.mColors[1] = new SexyColor(200, 200, 255);
			mNextButton.Resize(Constants.StoreScreen_NextButton_X, Constants.StoreScreen_NextButton_Y, AtlasResources.IMAGE_STORE_NEXTBUTTON.mWidth, AtlasResources.IMAGE_STORE_NEXTBUTTON.mHeight);
			mOverlayWidget = new StoreScreenOverlay(this);
			mOverlayWidget.Resize(0, 0, Constants.BOARD_WIDTH, Constants.BOARD_HEIGHT);
			if (!IsPageShown(StorePage.STORE_PAGE_PLANT_UPGRADES))
			{
				mPrevButton.mDisabledImage = AtlasResources.IMAGE_STORE_PREVBUTTONDISABLED;
				mPrevButton.SetDisabled(true);
				mNextButton.mDisabledImage = AtlasResources.IMAGE_STORE_NEXTBUTTONDISABLED;
				mNextButton.SetDisabled(true);
			}
			mDrawnOnce = false;
			mGoToTreeNow = false;
			mPurchasedFullVersion = false;
			mTrialLockedWhenStoreOpened = mApp.IsTrialStageLocked();
		}

		public override void Dispose()
		{
			mCoins.DataArrayDispose();
			mBackButton.Dispose();
			mPrevButton.Dispose();
			mNextButton.Dispose();
			mOverlayWidget.Dispose();
			mApp.DelayLoadStoreResource(string.Empty);
		}

		public override void KeyDown(KeyCode theKey)
		{
		}

		public override void Update()
		{
			mApp.mMusic.MakeSureMusicIsPlaying(MusicTune.MUSIC_TUNE_TITLE_CRAZY_DAVE_MAIN_THEME);
			if (mApp.mGameMode != GameMode.GAMEMODE_CHALLENGE_ZEN_GARDEN || mApp.mBoard == null)
			{
				mApp.UpdateCrazyDave();
			}
			if (IsWaitingForDialog())
			{
				return;
			}
			if (mApp.mCrazyDaveState == CrazyDaveState.CRAZY_DAVE_OFF)
			{
				if (mDrawnOnce)
				{
					StorePreLoad();
				}
				return;
			}
			mStoreTime += 3;
			if (mApp.mCrazyDaveState != 0 && mApp.mCrazyDaveState != CrazyDaveState.CRAZY_DAVE_ENTERING)
			{
				if (mHatchTimer > 0)
				{
					mHatchTimer -= 3;
					mBackButton.mX -= mShakeX;
					mBackButton.mY -= mShakeY;
					mPrevButton.mX -= mShakeX;
					mPrevButton.mY -= mShakeY;
					mNextButton.mX -= mShakeX;
					mNextButton.mY -= mShakeY;
					if (mHatchTimer >= 0 && mHatchTimer < 3)
					{
						if (!mBubbleClickToContinue)
						{
							EnableButtons(true);
						}
						mShakeX = 0;
						mShakeY = 0;
					}
					else if (mHatchTimer > 35)
					{
						mShakeX = 0;
						mShakeY = RandomNumbers.NextNumber(3) - 1;
					}
					else
					{
						mShakeX = 0;
						mShakeY = 0;
					}
					mBackButton.mX += mShakeX;
					mBackButton.mY += mShakeY;
					mPrevButton.mX += mShakeX;
					mPrevButton.mY += mShakeY;
					mNextButton.mX += mShakeX;
					mNextButton.mY += mShakeY;
				}
				else if (mStartDialog != -1)
				{
					SetBubbleText(mStartDialog, 0, true);
					mStartDialog = -1;
				}
				else if (!mBubbleClickToContinue)
				{
					if (mBubbleCountDown > 0)
					{
						mBubbleCountDown -= 3;
						if (mBubbleCountDown >= 0 && mBubbleCountDown < 3)
						{
							if (mApp.mSoundSystem.IsFoleyPlaying(FoleyType.FOLEY_CRAZYDAVESHORT) || mApp.mSoundSystem.IsFoleyPlaying(FoleyType.FOLEY_CRAZYDAVELONG) || mApp.mSoundSystem.IsFoleyPlaying(FoleyType.FOLEY_CRAZYDAVEEXTRALONG))
							{
								mBubbleCountDown = 3;
							}
							else if (mBubbleAutoAdvance)
							{
								AdvanceCrazyDaveDialog();
							}
							else
							{
								mApp.CrazyDaveStopTalking();
							}
						}
					}
					else
					{
						mAmbientSpeechCountDown -= 3;
						if (mAmbientSpeechCountDown <= 0)
						{
							for (int i = 0; i < 4; i++)
							{
								aPickArray[i].Reset();
							}
							for (int j = 0; j < 4; j++)
							{
								aPickArray[j].mItem = 2015 + j;
								if (mPreviousAmbientSpeechIndex == (int)aPickArray[j].mItem)
								{
									aPickArray[j].mWeight = 0;
								}
								else if (j == 3)
								{
									if (!mApp.HasFinishedAdventure())
									{
										aPickArray[j].mWeight = 0;
									}
									else
									{
										aPickArray[j].mWeight = 20;
									}
								}
								else
								{
									aPickArray[j].mWeight = 100;
								}
							}
							SetBubbleText(mPreviousAmbientSpeechIndex = (int)TodCommon.TodPickFromWeightedArray(aPickArray, 4), 800, false);
							mAmbientSpeechCountDown = TodCommon.RandRangeInt(500, 1000);
						}
					}
				}
			}
			UpdateMouse();
			if (CanInteractWithButtons() && mTrialLockedWhenStoreOpened && !mApp.IsTrialStageLocked())
			{
				mPurchasedFullVersion = true;
				mResult = 1000;
			}
			else
			{
				base.Update();
				MarkDirty();
			}
		}

		public override void Draw(Graphics g)
		{
			g.SetLinearBlend(true);
			mDrawnOnce = true;
			int theY = TodCommon.TodAnimateCurve(50, 110, mStoreTime, Constants.StoreScreen_StoreSign_Y_Min, Constants.StoreScreen_StoreSign_Y_Max, TodCurves.CURVE_EASE_IN_OUT);
			if (mApp.IsNight())
			{
				g.DrawImage(Resources.IMAGE_STORE_BACKGROUNDNIGHT, 0, 0);
			}
			else
			{
				g.DrawImage(Resources.IMAGE_STORE_BACKGROUND, 0, 0);
			}
			g.DrawImage(Resources.IMAGE_STORE_CAR, Constants.StoreScreen_Car_X + mShakeX, Constants.StoreScreen_Car_Y + mShakeY);
			if (mHatchTimer == 0 && mHatchOpen)
			{
				g.DrawImage(Resources.IMAGE_STORE_HATCHBACKOPEN, Constants.StoreScreen_HatchOpen_X + mShakeX, Constants.StoreScreen_HatchOpen_Y + mShakeY);
				if (mApp.IsNight())
				{
					g.DrawImage(Resources.IMAGE_STORE_CAR_NIGHT, Constants.StoreScreen_CarNight_X + mShakeX, Constants.StoreScreen_CarNight_Y + mShakeY);
				}
			}
			else
			{
				g.DrawImage(Resources.IMAGE_STORE_CARCLOSED, Constants.StoreScreen_HatchClosed_X + mShakeX, Constants.StoreScreen_HatchClosed_Y + mShakeY);
				if (mApp.IsNight())
				{
					g.DrawImage(Resources.IMAGE_STORE_CAR_NIGHT, Constants.StoreScreen_CarNight_X + mShakeX, Constants.StoreScreen_CarNight_Y + mShakeY);
					g.DrawImage(Resources.IMAGE_STORE_CARCLOSED_NIGHT, Constants.StoreScreen_HatchClosed_X + mShakeX, Constants.StoreScreen_HatchClosed_Y + mShakeY);
				}
			}
			g.DrawImage(AtlasResources.IMAGE_STORE_SIGN, Constants.StoreScreen_StoreSign_X, theY);
			Graphics @new = Graphics.GetNew(g);
			@new.mTransX += Constants.StoreScreen_RetardedDave_Offset_X;
			@new.mTransY = @new.mTransY;
			mApp.DrawCrazyDave(@new);
			@new.PrepareForReuse();
			if (mHatchTimer == 0 && mHatchOpen)
			{
				for (int i = 0; i < 8; i++)
				{
					StoreItem storeItemType = GetStoreItemType(i);
					if (storeItemType != StoreItem.STORE_ITEM_INVALID)
					{
						DrawItem(g, i, storeItemType);
					}
				}
			}
			int storeScreen_Coinbank_X = Constants.StoreScreen_Coinbank_X;
			int storeScreen_Coinbank_Y = Constants.StoreScreen_Coinbank_Y;
			g.DrawImage(AtlasResources.IMAGE_COINBANK, storeScreen_Coinbank_X, storeScreen_Coinbank_Y);
			g.SetColor(new SexyColor(180, 255, 90));
			g.SetFont(Resources.FONT_CONTINUUMBOLD14);
			string moneyString = LawnApp.GetMoneyString(mApp.mPlayerInfo.mCoins);
			int theX = storeScreen_Coinbank_X + Constants.StoreScreen_Coinbank_TextOffset.X - Resources.FONT_CONTINUUMBOLD14.StringWidth(moneyString);
			g.DrawString(moneyString, theX, storeScreen_Coinbank_Y + Constants.StoreScreen_Coinbank_TextOffset.Y);
		}

		public override void AddedToManager(WidgetManager theWidgetManager)
		{
			base.AddedToManager(theWidgetManager);
			AddWidget(mBackButton);
			AddWidget(mPrevButton);
			AddWidget(mNextButton);
			AddWidget(mOverlayWidget);
		}

		public override void RemovedFromManager(WidgetManager theWidgetManager)
		{
			base.RemovedFromManager(theWidgetManager);
			RemoveWidget(mBackButton);
			RemoveWidget(mPrevButton);
			RemoveWidget(mNextButton);
			RemoveWidget(mOverlayWidget);
			mApp.CrazyDaveDie();
		}

		public override void ButtonPress(int theId)
		{
			if (theId != 101 && theId != 102)
			{
				mApp.PlaySample(Resources.SOUND_BUTTONCLICK);
			}
		}

		public override bool BackButtonPress()
		{
			ButtonDepress(mBackButton.mId);
			return true;
		}

		public override void ButtonDepress(int theId)
		{
			switch (theId)
			{
			case 100:
				mResult = 1000;
				if (mListener != null)
				{
					mListener.BackFromStore();
				}
				return;
			case 101:
				mHatchTimer = 51;
				mApp.PlaySample(Resources.SOUND_HATCHBACK_CLOSE);
				mBubbleCountDown = 1;
				EnableButtons(false);
				do
				{
					mPage--;
					if (mPage < StorePage.STORE_PAGE_SLOT_UPGRADES)
					{
						mPage = StorePage.STORE_PAGE_ZEN2;
					}
				}
				while (!IsPageShown(mPage));
				break;
			}
			if (theId != 102)
			{
				return;
			}
			mHatchTimer = 51;
			mApp.PlaySample(Resources.SOUND_HATCHBACK_CLOSE);
			mBubbleCountDown = 1;
			EnableButtons(false);
			do
			{
				mPage++;
				if (mPage >= StorePage.NUM_STORE_PAGES)
				{
					mPage = StorePage.STORE_PAGE_SLOT_UPGRADES;
				}
			}
			while (!IsPageShown(mPage));
		}

		public virtual void KeyChar(char theChar)
		{
			if (mBubbleClickToContinue && (theChar == ' ' || theChar == '\r'))
			{
				AdvanceCrazyDaveDialog();
				return;
			}
			if (theChar == 'c' || theChar == 'C')
			{
				mEasyBuyingCheat = true;
				mNextButton.mMouseVisible = true;
				mNextButton.SetDisabled(false);
				mPrevButton.mMouseVisible = true;
				mPrevButton.SetDisabled(false);
			}
			if (theChar == '0')
			{
				mApp.mPlayerInfo.AddCoins(50000);
				mApp.PlaySample(Resources.SOUND_BUTTONCLICK);
			}
			if (theChar == '$')
			{
				mApp.mPlayerInfo.AddCoins(100);
				mApp.PlaySample(Resources.SOUND_BUTTONCLICK);
			}
			if (theChar == 'l')
			{
				mApp.mDebugTrialLocked = !mApp.mDebugTrialLocked;
				mApp.mTrialType = TrialType.TRIAL_STAGELOCKED;
			}
		}

		public override void MouseDown(int x, int y, int theClickCount)
		{
			if (mBubbleClickToContinue)
			{
				AdvanceCrazyDaveDialog();
				return;
			}
			UpdateMouse();
			if (!CanInteractWithButtons())
			{
				return;
			}
			for (int i = 0; i < 8; i++)
			{
				StoreItem storeItemType = GetStoreItemType(i);
				if (storeItemType == StoreItem.STORE_ITEM_INVALID)
				{
					continue;
				}
				int thePosX = 0;
				int thePosY = 0;
				GetStorePosition(i, ref thePosX, ref thePosY);
				if (new TRect(thePosX, thePosY, Constants.StoreScreen_MouseRegion.X, Constants.StoreScreen_MouseRegion.Y).Contains(x, y))
				{
					if (IsItemSoldOut(storeItemType) || IsItemUnavailable(storeItemType) || IsComingSoon(storeItemType))
					{
						break;
					}
					PurchaseItem(storeItemType);
				}
			}
		}

		public void SetBubbleText(int theCrazyDaveMessage, int theTime, bool theClickToContinue)
		{
			mApp.CrazyDaveTalkIndex(theCrazyDaveMessage);
			mBubbleCountDown = theTime;
			mBubbleClickToContinue = theClickToContinue;
		}

		public void GetStorePosition(int theSpotIndex, ref int thePosX, ref int thePosY)
		{
			if (theSpotIndex <= 3)
			{
				thePosX = Constants.StoreScreen_ItemOffset_1_X + Constants.StoreScreen_ItemSize * theSpotIndex;
				thePosY = Constants.StoreScreen_ItemOffset_1_Y;
			}
			else
			{
				thePosX = Constants.StoreScreen_ItemOffset_2_X + Constants.StoreScreen_ItemSize * (theSpotIndex - Constants.StoreScreen_ItemSize_Offset);
				thePosY = Constants.StoreScreen_ItemOffset_2_Y;
			}
		}

		public StoreItem GetStoreItemType(int theSpotIndex)
		{
			if (mPage == StorePage.STORE_PAGE_SLOT_UPGRADES)
			{
				switch (theSpotIndex)
				{
				case 0:
					return StoreItem.STORE_ITEM_PACKET_UPGRADE;
				case 1:
					return StoreItem.STORE_ITEM_POOL_CLEANER;
				case 2:
					return StoreItem.STORE_ITEM_RAKE;
				case 3:
					return StoreItem.STORE_ITEM_ROOF_CLEANER;
				case 4:
					return StoreItem.STORE_ITEM_PLANT_GATLINGPEA;
				case 5:
					return StoreItem.STORE_ITEM_PLANT_TWINSUNFLOWER;
				case 6:
					return StoreItem.STORE_ITEM_PLANT_GLOOMSHROOM;
				case 7:
					return StoreItem.STORE_ITEM_PLANT_CATTAIL;
				default:
					return StoreItem.STORE_ITEM_INVALID;
				}
			}
			if (mPage == StorePage.STORE_PAGE_PLANT_UPGRADES)
			{
				switch (theSpotIndex)
				{
				case 0:
					return StoreItem.STORE_ITEM_PLANT_SPIKEROCK;
				case 1:
					return StoreItem.STORE_ITEM_PLANT_GOLD_MAGNET;
				case 2:
					return StoreItem.STORE_ITEM_PLANT_WINTERMELON;
				case 3:
					return StoreItem.STORE_ITEM_PLANT_COBCANNON;
				case 4:
					return StoreItem.STORE_ITEM_PLANT_IMITATER;
				case 5:
					return StoreItem.STORE_ITEM_FIRSTAID;
				default:
					return StoreItem.STORE_ITEM_INVALID;
				}
			}
			if (mPage == StorePage.STORE_PAGE_ZEN1)
			{
				switch (theSpotIndex)
				{
				case 0:
					return StoreItem.STORE_ITEM_POTTED_MARIGOLD_1;
				case 1:
					return StoreItem.STORE_ITEM_POTTED_MARIGOLD_2;
				case 2:
					return StoreItem.STORE_ITEM_POTTED_MARIGOLD_3;
				case 3:
					return StoreItem.STORE_ITEM_GOLD_WATERINGCAN;
				case 4:
					return StoreItem.STORE_ITEM_FERTILIZER;
				case 5:
					return StoreItem.STORE_ITEM_BUG_SPRAY;
				case 6:
					return StoreItem.STORE_ITEM_PHONOGRAPH;
				case 7:
					return StoreItem.STORE_ITEM_GARDENING_GLOVE;
				default:
					return StoreItem.STORE_ITEM_INVALID;
				}
			}
			if (mPage == StorePage.STORE_PAGE_ZEN2)
			{
				switch (theSpotIndex)
				{
				case 0:
					return StoreItem.STORE_ITEM_MUSHROOM_GARDEN;
				case 1:
					return StoreItem.STORE_ITEM_AQUARIUM_GARDEN;
				case 2:
					return StoreItem.STORE_ITEM_WHEEL_BARROW;
				case 3:
					return StoreItem.STORE_ITEM_STINKY_THE_SNAIL;
				default:
					return StoreItem.STORE_ITEM_INVALID;
				}
			}
			Debug.ASSERT(false);
			return StoreItem.STORE_ITEM_INVALID;
		}

		public void UpdateMouse()
		{
		}

		public bool IsItemUnavailable(StoreItem theStoreItem)
		{
			if (mEasyBuyingCheat)
			{
				return false;
			}
			if (theStoreItem == StoreItem.STORE_ITEM_ROOF_CLEANER)
			{
				if (mApp.IsTrialStageLocked())
				{
					return true;
				}
				if (!mApp.HasFinishedAdventure() && mApp.mPlayerInfo.mLevel < 42)
				{
					return true;
				}
			}
			if (theStoreItem == StoreItem.STORE_ITEM_PLANT_GLOOMSHROOM)
			{
				if (mApp.IsTrialStageLocked())
				{
					return true;
				}
				if (!mApp.HasFinishedAdventure() && mApp.mPlayerInfo.mLevel < 35)
				{
					return true;
				}
			}
			if (theStoreItem == StoreItem.STORE_ITEM_PLANT_CATTAIL)
			{
				if (mApp.IsTrialStageLocked())
				{
					return true;
				}
				if (!mApp.HasFinishedAdventure() && mApp.mPlayerInfo.mLevel < 35)
				{
					return true;
				}
			}
			if (theStoreItem == StoreItem.STORE_ITEM_PLANT_SPIKEROCK && !mApp.HasFinishedAdventure() && mApp.mPlayerInfo.mLevel < 41)
			{
				return true;
			}
			if (theStoreItem == StoreItem.STORE_ITEM_PLANT_GOLD_MAGNET && !mApp.HasFinishedAdventure() && mApp.mPlayerInfo.mLevel < 41)
			{
				return true;
			}
			if ((theStoreItem == StoreItem.STORE_ITEM_PLANT_WINTERMELON || theStoreItem == StoreItem.STORE_ITEM_PLANT_COBCANNON || theStoreItem == StoreItem.STORE_ITEM_PLANT_IMITATER || theStoreItem == StoreItem.STORE_ITEM_FIRSTAID) && !mApp.HasFinishedAdventure())
			{
				return true;
			}
			return false;
		}

		public bool IsItemSoldOut(StoreItem theStoreItem)
		{
			switch (theStoreItem)
			{
			case StoreItem.STORE_ITEM_INVALID:
				return false;
			case StoreItem.STORE_ITEM_PACKET_UPGRADE:
				if (mApp.mPlayerInfo.mPurchases[21] >= 3)
				{
					return true;
				}
				return false;
			case StoreItem.STORE_ITEM_FERTILIZER:
			case StoreItem.STORE_ITEM_BUG_SPRAY:
				if (mApp.mPlayerInfo.mPurchases[(int)theStoreItem] - 1000 > 15)
				{
					return true;
				}
				return false;
			case StoreItem.STORE_ITEM_BONUS_LAWN_MOWER:
				if (mApp.mPlayerInfo.mPurchases[9] >= 2)
				{
					return true;
				}
				return false;
			default:
				if (IsPottedPlant(theStoreItem))
				{
					int currentDaysSince = LawnCommon.GetCurrentDaysSince2000();
					if (mApp.mZenGarden.IsZenGardenFull(true))
					{
						return true;
					}
					if (mApp.mPlayerInfo.mPurchases[(int)theStoreItem] == currentDaysSince)
					{
						return true;
					}
					return false;
				}
				Debug.ASSERT(theStoreItem >= StoreItem.STORE_ITEM_PLANT_GATLINGPEA && theStoreItem < (StoreItem)80);
				if (mApp.mPlayerInfo.mPurchases[(int)theStoreItem] != 0)
				{
					return true;
				}
				return false;
			}
		}

		public static int GetItemCost(StoreItem theStoreItem)
		{
			LawnApp lawnApp = (LawnApp)GlobalStaticVars.gSexyAppBase;
			switch (theStoreItem)
			{
			case StoreItem.STORE_ITEM_BONUS_LAWN_MOWER:
				if (lawnApp.mPlayerInfo.mPurchases[9] == 0)
				{
					return 200;
				}
				return 500;
			case StoreItem.STORE_ITEM_PLANT_GATLINGPEA:
				return 500;
			case StoreItem.STORE_ITEM_PLANT_TWINSUNFLOWER:
				return 500;
			case StoreItem.STORE_ITEM_PLANT_GLOOMSHROOM:
				return 750;
			case StoreItem.STORE_ITEM_PLANT_CATTAIL:
				return 1000;
			case StoreItem.STORE_ITEM_PLANT_WINTERMELON:
				return 1000;
			case StoreItem.STORE_ITEM_PLANT_GOLD_MAGNET:
				return 300;
			case StoreItem.STORE_ITEM_PLANT_SPIKEROCK:
				return 750;
			case StoreItem.STORE_ITEM_PLANT_COBCANNON:
				return 2000;
			case StoreItem.STORE_ITEM_PLANT_IMITATER:
				return 3000;
			case StoreItem.STORE_ITEM_POTTED_MARIGOLD_1:
				return 250;
			case StoreItem.STORE_ITEM_POTTED_MARIGOLD_2:
				return 250;
			case StoreItem.STORE_ITEM_POTTED_MARIGOLD_3:
				return 250;
			case StoreItem.STORE_ITEM_MUSHROOM_GARDEN:
				return 3000;
			case StoreItem.STORE_ITEM_AQUARIUM_GARDEN:
				return 3000;
			case StoreItem.STORE_ITEM_GOLD_WATERINGCAN:
				return 1000;
			case StoreItem.STORE_ITEM_FERTILIZER:
				return 75;
			case StoreItem.STORE_ITEM_PHONOGRAPH:
				return 1500;
			case StoreItem.STORE_ITEM_BUG_SPRAY:
				return 100;
			case StoreItem.STORE_ITEM_GARDENING_GLOVE:
				return 100;
			case StoreItem.STORE_ITEM_WHEEL_BARROW:
				return 20;
			case StoreItem.STORE_ITEM_STINKY_THE_SNAIL:
				return 300;
			case StoreItem.STORE_ITEM_POOL_CLEANER:
				return 100;
			case StoreItem.STORE_ITEM_ROOF_CLEANER:
				return 300;
			case StoreItem.STORE_ITEM_RAKE:
				return 20;
			case StoreItem.STORE_ITEM_FIRSTAID:
				return 200;
			case StoreItem.STORE_ITEM_PACKET_UPGRADE:
				if (lawnApp.mPlayerInfo.mPurchases[21] + 1 == 1)
				{
					return 75;
				}
				if (lawnApp.mPlayerInfo.mPurchases[21] + 1 == 2)
				{
					return 500;
				}
				if (lawnApp.mPlayerInfo.mPurchases[21] + 1 == 3)
				{
					return 2000;
				}
				return 2000;
			default:
				Debug.ASSERT(false);
				return 0;
			}
		}

		public bool CanAffordItem(StoreItem theStoreItem)
		{
			int itemCost = GetItemCost(theStoreItem);
			if (mApp.mPlayerInfo.mCoins >= itemCost)
			{
				return true;
			}
			return false;
		}

		public void PurchaseItem(StoreItem theItemType)
		{
			mBubbleCountDown = 0;
			mApp.CrazyDaveStopTalking();
			if (!CanAffordItem(theItemType))
			{
				mApp.DoDialog(25, true, "[NOT_ENOUGH_MONEY]", "[CANNOT_AFFORD_ITEM]", "[DIALOG_BUTTON_OK]", 3);
				return;
			}
			mPendingPurchaseItem = theItemType;
			int theMessageIndex = 0;
			switch (theItemType)
			{
			case StoreItem.STORE_ITEM_PLANT_GATLINGPEA:
				theMessageIndex = 2000;
				break;
			case StoreItem.STORE_ITEM_PLANT_TWINSUNFLOWER:
				theMessageIndex = 2001;
				break;
			case StoreItem.STORE_ITEM_PLANT_GLOOMSHROOM:
				theMessageIndex = 2002;
				break;
			case StoreItem.STORE_ITEM_PLANT_CATTAIL:
				theMessageIndex = 2003;
				break;
			case StoreItem.STORE_ITEM_PLANT_WINTERMELON:
				theMessageIndex = 2004;
				break;
			case StoreItem.STORE_ITEM_PLANT_GOLD_MAGNET:
				theMessageIndex = 2005;
				break;
			case StoreItem.STORE_ITEM_PLANT_SPIKEROCK:
				theMessageIndex = 2006;
				break;
			case StoreItem.STORE_ITEM_PLANT_COBCANNON:
				theMessageIndex = 2007;
				break;
			case StoreItem.STORE_ITEM_PLANT_IMITATER:
				theMessageIndex = 2008;
				break;
			case StoreItem.STORE_ITEM_BONUS_LAWN_MOWER:
				theMessageIndex = 2009;
				break;
			case StoreItem.STORE_ITEM_POTTED_MARIGOLD_1:
				theMessageIndex = 2010;
				break;
			case StoreItem.STORE_ITEM_POTTED_MARIGOLD_2:
				theMessageIndex = 2010;
				break;
			case StoreItem.STORE_ITEM_POTTED_MARIGOLD_3:
				theMessageIndex = 2010;
				break;
			case StoreItem.STORE_ITEM_POOL_CLEANER:
				theMessageIndex = 2026;
				break;
			case StoreItem.STORE_ITEM_ROOF_CLEANER:
				theMessageIndex = 2027;
				break;
			case StoreItem.STORE_ITEM_RAKE:
				theMessageIndex = 2028;
				break;
			case StoreItem.STORE_ITEM_FIRSTAID:
				theMessageIndex = 2033;
				break;
			case StoreItem.STORE_ITEM_PACKET_UPGRADE:
				theMessageIndex = ((mApp.mPlayerInfo.mPurchases[21] + 1 != 1) ? ((mApp.mPlayerInfo.mPurchases[21] + 1 != 2) ? ((mApp.mPlayerInfo.mPurchases[21] + 1 != 3) ? 2014 : 2013) : 2012) : 2011);
				break;
			case StoreItem.STORE_ITEM_GOLD_WATERINGCAN:
				theMessageIndex = 2019;
				break;
			case StoreItem.STORE_ITEM_FERTILIZER:
				theMessageIndex = 2020;
				break;
			case StoreItem.STORE_ITEM_BUG_SPRAY:
				theMessageIndex = 2022;
				break;
			case StoreItem.STORE_ITEM_PHONOGRAPH:
				theMessageIndex = 2021;
				break;
			case StoreItem.STORE_ITEM_GARDENING_GLOVE:
				theMessageIndex = 2023;
				break;
			case StoreItem.STORE_ITEM_MUSHROOM_GARDEN:
				theMessageIndex = 2032;
				break;
			case StoreItem.STORE_ITEM_WHEEL_BARROW:
				theMessageIndex = 2024;
				break;
			case StoreItem.STORE_ITEM_STINKY_THE_SNAIL:
				theMessageIndex = 2025;
				break;
			case StoreItem.STORE_ITEM_AQUARIUM_GARDEN:
				theMessageIndex = 2029;
				break;
			default:
				Debug.ASSERT(false);
				break;
			}
			string crazyDaveText = mApp.GetCrazyDaveText(theMessageIndex);
			int aQuantity = mApp.mPlayerInfo.mPurchases[21] + 7;
			ConfirmPurchaseDialog confirmPurchaseDialog = new ConfirmPurchaseDialog(mApp, theItemType, aQuantity, GetItemCost(theItemType), crazyDaveText);
			confirmPurchaseDialog.Resize(Constants.StoreScreen_Dialog.X, Constants.StoreScreen_Dialog.Y, Constants.StoreScreen_Dialog.Width, Constants.StoreScreen_Dialog.Height);
			mApp.AddDialog(46, confirmPurchaseDialog);
		}

		public void PurchasePendingItem()
		{
			mApp.KillDialog(46);
			StoreItem storeItem = mPendingPurchaseItem;
			int itemCost = GetItemCost(storeItem);
			mApp.mPlayerInfo.AddCoins(-itemCost);
			mApp.mPlayerInfo.mMoneySpent += itemCost;
			if (mApp.mPlayerInfo.mMoneySpent >= 2500)
			{
				ReportAchievement.GiveAchievement(AchievementId.Shopaholic);
			}
			switch (storeItem)
			{
			case StoreItem.STORE_ITEM_PACKET_UPGRADE:
			{
				mApp.mPlayerInfo.mPurchases[21]++;
				string theDialogLines = Common.StrFormat_(TodStringFile.TodStringTranslate("[NOW_YOU_CAN_CHOOSE_X_SEEDS]"), 6 + mApp.mPlayerInfo.mPurchases[21]);
				LawnDialog lawnDialog = mApp.DoDialog(26, true, "[MORE_SLOTS]", theDialogLines, "[DIALOG_BUTTON_OK]", 3);
				lawnDialog.CalcSize(0, 0);
				if (mApp.mBoard != null)
				{
					mApp.mBoard.mSeedBank.UpdateHeight();
				}
				break;
			}
			case StoreItem.STORE_ITEM_BONUS_LAWN_MOWER:
				mApp.mPlayerInfo.mPurchases[9]++;
				break;
			case StoreItem.STORE_ITEM_RAKE:
				mApp.mPlayerInfo.mPurchases[24] = 3;
				break;
			case StoreItem.STORE_ITEM_FERTILIZER:
			case StoreItem.STORE_ITEM_BUG_SPRAY:
				if (mApp.mPlayerInfo.mPurchases[(int)storeItem] < 1000)
				{
					mApp.mPlayerInfo.mPurchases[(int)storeItem] = 1000;
				}
				mApp.mPlayerInfo.mPurchases[(int)storeItem] += 5;
				break;
			default:
				if (IsPottedPlant(storeItem))
				{
					mApp.mZenGarden.AddPottedPlant(mPottedPlantSpecs);
					mPottedPlantSpecs.InitializePottedPlant(SeedType.SEED_MARIGOLD);
					mPottedPlantSpecs.mDrawVariation = (DrawVariation)TodCommon.RandRangeInt(2, 12);
					mApp.mPlayerInfo.mPurchases[(int)storeItem] = LawnCommon.GetCurrentDaysSince2000();
				}
				else
				{
					Debug.ASSERT(storeItem >= StoreItem.STORE_ITEM_PLANT_GATLINGPEA && storeItem < (StoreItem)80);
					mApp.mPlayerInfo.mPurchases[(int)storeItem] = 1;
				}
				break;
			}
			if (storeItem == StoreItem.STORE_ITEM_FIRSTAID)
			{
				SetBubbleText(3400, 800, false);
			}
			if (mApp.mSeedChooserScreen != null)
			{
				mApp.mSeedChooserScreen.UpdateAfterPurchase();
			}
			int i;
			for (i = 0; i < 49; i++)
			{
				if (!mApp.HasSeedType((SeedType)i))
				{
					i = -1;
					break;
				}
			}
			if (i == 49 && !mApp.mPlayerInfo.mShownAchievements[1])
			{
				ReportAchievement.GiveAchievement(AchievementId.ACHIEVEMENT_MORTICULTURALIST);
				mApp.mPlayerInfo.mShownAchievements[1] = true;
				SetBubbleText(4000, 150, false);
				mBubbleAutoAdvance = true;
			}
			mApp.WriteCurrentUserConfig();
		}

		public void FinishTreeOfWisdomDialog(bool isYes)
		{
			mApp.KillDialog(47);
			if (mApp.mSeedChooserScreen != null)
			{
				mApp.mSeedChooserScreen.UpdateAfterPurchase();
			}
			mApp.WriteCurrentUserConfig();
			if (isYes)
			{
				mGoToTreeNow = true;
				mResult = 1000;
				if (mListener != null)
				{
					mListener.BackFromStore();
				}
			}
		}

		public void DrawItem(Graphics g, int theItemPosition, StoreItem theItemType)
		{
			if (!IsItemUnavailable(theItemType))
			{
				int thePosX = 0;
				int thePosY = 0;
				GetStorePosition(theItemPosition, ref thePosX, ref thePosY);
				GlobalMembersStoreScreen.DrawStoreItem(g, thePosX, thePosY, theItemType, IsComingSoon(theItemType), IsItemSoldOut(theItemType), mApp.mPlayerInfo.mPurchases[21] + 7, GetItemCost(theItemType));
			}
		}

		public void EnableButtons(bool theEnable)
		{
			bool flag = true;
			if (!mEasyBuyingCheat && !IsPageShown(StorePage.STORE_PAGE_PLANT_UPGRADES))
			{
				flag = false;
			}
			if (flag || !theEnable)
			{
				mNextButton.mMouseVisible = theEnable;
				mNextButton.SetDisabled(!theEnable);
				mPrevButton.mMouseVisible = theEnable;
				mPrevButton.SetDisabled(!theEnable);
			}
			mBackButton.mMouseVisible = theEnable;
			mBackButton.SetDisabled(!theEnable);
		}

		public void SetupForIntro(int theDialogIndex)
		{
			mStartDialog = theDialogIndex;
			mHatchOpen = false;
			SetupBackButtonForZenGarden();
			EnableButtons(false);
		}

		public void SetupBackButtonForZenGarden()
		{
			mBackButton.mButtonImage = AtlasResources.IMAGE_STORE_CONTINUEBUTTON;
			mBackButton.mDownImage = AtlasResources.IMAGE_STORE_CONTINUEBUTTONDOWN;
		}

		public bool CanInteractWithButtons()
		{
			if (mStoreTime < 120)
			{
				return false;
			}
			if (mBubbleClickToContinue || mBubbleAutoAdvance)
			{
				return false;
			}
			if (mHatchTimer > 0)
			{
				return false;
			}
			if (IsWaitingForDialog())
			{
				return false;
			}
			return true;
		}

		public static bool IsPottedPlant(StoreItem theStoreItem)
		{
			if (theStoreItem == StoreItem.STORE_ITEM_POTTED_MARIGOLD_1 || theStoreItem == StoreItem.STORE_ITEM_POTTED_MARIGOLD_2 || theStoreItem == StoreItem.STORE_ITEM_POTTED_MARIGOLD_3)
			{
				return true;
			}
			return false;
		}

		public void AdvanceCrazyDaveDialog()
		{
			if (!mBubbleClickToContinue && !mBubbleAutoAdvance)
			{
				return;
			}
			if (mApp.mCrazyDaveMessageIndex == 3100)
			{
				mHatchTimer = 150;
				mHatchOpen = true;
				mApp.PlaySample(Resources.SOUND_HATCHBACK_OPEN);
			}
			if (!mApp.AdvanceCrazyDaveText())
			{
				mApp.CrazyDaveStopTalking();
				mBubbleClickToContinue = false;
				mBubbleAutoAdvance = false;
				mAmbientSpeechCountDown = 500;
				if (mHatchTimer == 0)
				{
					EnableButtons(true);
				}
			}
			else
			{
				int theTime = 0;
				if (mApp.mCrazyDaveMessageIndex == 4001)
				{
					theTime = 200;
				}
				else if (mApp.mCrazyDaveMessageIndex == 4002)
				{
					theTime = 400;
				}
				else if (mApp.mCrazyDaveMessageIndex == 4003)
				{
					theTime = 150;
				}
				else if (mApp.mCrazyDaveMessageIndex == 4004)
				{
					theTime = 500;
				}
				SetBubbleText(mApp.mCrazyDaveMessageIndex, theTime, mBubbleClickToContinue);
			}
			if (mApp.mCrazyDaveMessageIndex == 303 || mApp.mCrazyDaveMessageIndex == 606 || mApp.mCrazyDaveMessageIndex == 2105 || mApp.mCrazyDaveMessageIndex == 2601)
			{
				mHatchTimer = 150;
				mHatchOpen = true;
				mApp.PlaySample(Resources.SOUND_HATCHBACK_OPEN);
			}
			if (mApp.mCrazyDaveMessageIndex == 603)
			{
				mApp.mPlayerInfo.AddCoins(100);
				mApp.mPlayerInfo.mNeedsMagicTacoReward = false;
				mApp.WriteCurrentUserConfig();
			}
			if (mApp.mCrazyDaveMessageIndex == 2103)
			{
				mApp.mPlayerInfo.mNeedsMagicBaconReward = false;
				mApp.WriteCurrentUserConfig();
				mApp.PlaySample(Resources.SOUND_DIAMOND);
				Coin coin = mCoins.DataArrayAlloc();
				coin.CoinInitialize(80, 520, CoinType.COIN_DIAMOND, CoinMotion.COIN_MOTION_FROM_PRESENT);
				coin.mVelX = 0f;
				coin.mVelY = -5f;
			}
			if (mApp.mCrazyDaveMessageIndex == 902)
			{
				mApp.mPlayerInfo.AddCoins(100);
			}
			if (mApp.mCrazyDaveMessageIndex == 1002)
			{
				mApp.mPlayerInfo.AddCoins(100);
			}
		}

		public bool IsComingSoon(StoreItem theStoreItem)
		{
			if (IsFullVersionOnly(theStoreItem))
			{
				return true;
			}
			if (IsPottedPlant(theStoreItem) && !mApp.HasFinishedAdventure())
			{
				return true;
			}
			return false;
		}

		public void StorePreLoad()
		{
			ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_CRAZY_DAVE, true);
			mApp.CrazyDaveEnter();
			Plant.PreloadPlantResources(SeedType.SEED_GATLINGPEA);
			Plant.PreloadPlantResources(SeedType.SEED_TWINSUNFLOWER);
			if (mApp.HasFinishedAdventure())
			{
				Plant.PreloadPlantResources(SeedType.SEED_TWINSUNFLOWER);
				Plant.PreloadPlantResources(SeedType.SEED_GLOOMSHROOM);
				Plant.PreloadPlantResources(SeedType.SEED_CATTAIL);
				Plant.PreloadPlantResources(SeedType.SEED_WINTERMELON);
				Plant.PreloadPlantResources(SeedType.SEED_GOLD_MAGNET);
				Plant.PreloadPlantResources(SeedType.SEED_SPIKEROCK);
				Plant.PreloadPlantResources(SeedType.SEED_COBCANNON);
				Plant.PreloadPlantResources(SeedType.SEED_IMITATER);
			}
		}

		public bool IsPageShown(StorePage thePage)
		{
			if (mApp.IsTrialStageLocked())
			{
				if (thePage == StorePage.STORE_PAGE_SLOT_UPGRADES)
				{
					return true;
				}
				return false;
			}
			if (thePage == StorePage.STORE_PAGE_ZEN1 && !mApp.mPlayerInfo.mZenGardenTutorialComplete && !mApp.mZenGarden.mIsTutorial)
			{
				return false;
			}
			if (thePage == StorePage.STORE_PAGE_ZEN2 && !mApp.mPlayerInfo.mZenGardenTutorialComplete && !mApp.HasFinishedAdventure())
			{
				return false;
			}
			if (mApp.HasFinishedAdventure())
			{
				return true;
			}
			if (thePage == StorePage.STORE_PAGE_PLANT_UPGRADES && mApp.mPlayerInfo.mLevel < 42)
			{
				return false;
			}
			return true;
		}

		public override void DrawOverlay(Graphics g)
		{
		}

		public bool IsFullVersionOnly(StoreItem theStoreItem)
		{
			if (mApp.IsTrialStageLocked())
			{
				if (theStoreItem == StoreItem.STORE_ITEM_PACKET_UPGRADE && mApp.mPlayerInfo.mPurchases[21] >= 2)
				{
					return true;
				}
				if (theStoreItem == StoreItem.STORE_ITEM_PLANT_TWINSUNFLOWER)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsWaitingForDialog()
		{
			if (mApp.GetDialog(Dialogs.DIALOG_STORE_PURCHASE) == null && mApp.GetDialog(Dialogs.DIALOG_NOT_ENOUGH_MONEY) == null && mApp.GetDialog(Dialogs.DIALOG_VISIT_TREE_OF_WISDOM) == null)
			{
				return mApp.GetDialog(Dialogs.DIALOG_UPGRADED) != null;
			}
			return true;
		}
	}
}
