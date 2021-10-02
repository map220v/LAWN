using Sexy;
using Sexy.TodLib;
using System;

namespace Lawn
{
	internal class CutScene
	{
		internal static int TimePanRightStart;

		internal static int TimePanRightEnd;

		internal static int TimeEarlyDaveEnterStart;

		internal static int TimeEarlyDaveEnterEnd;

		internal static int TimeEarlyDaveLeaveStart;

		internal static int TimeEarlyDaveLeaveEnd;

		internal static int TimeSeedChoserSlideOnStart;

		internal static int TimeSeedChoserSlideOnEnd;

		internal static int TimeSeedChoserSlideOffStart;

		internal static int TimeSeedChoserSlideOffEnd;

		internal static int TimeSeedBankOnStart;

		internal static int TimeSeedBankOnEnd;

		internal static int TimePanLeftStart;

		internal static int TimePanLeftEnd;

		internal static int TimeSeedBankRightStart;

		internal static int TimeSeedBankRightEnd;

		internal static int TimeRollSodStart;

		internal static int TimeRollSodEnd;

		internal static int TimeGraveStoneStart;

		internal static int TimeGraveStoneEnd;

		internal static int[] TimeLawnMowerStart;

		internal static int TimeLawnMowerDuration;

		internal static int TimeReadySetPlantStart;

		internal static int TimeReadySetPlantEnd;

		internal static int TimeFogRollIn;

		internal static int TimeCrazyDaveEnterStart;

		internal static int TimeCrazyDaveEnterEnd;

		internal static int TimeCrazyDaveLeaveStart;

		internal static int TimeCrazyDaveLeaveEnd;

		internal static int TimeIntroEnd;

		internal static int LostTimePanRightStart;

		internal static int LostTimePanRightEnd;

		internal static int LostTimeBrainGraphicStart;

		internal static int LostTimeBrainGraphicShake;

		internal static int LostTimeBrainGraphicCancelShake;

		internal static int LostTimeBrainGraphicEnd;

		internal static int LostTimeEnd;

		internal static int TimeIntro_PresentsFadeIn;

		internal static int TimeIntro_LogoStart;

		internal static int TimeIntro_LogoEnd;

		internal static int TimeIntro_PanRightStart;

		internal static int TimeIntro_PanRightEnd;

		internal static int TimeIntro_FadeOut;

		internal static int TimeIntro_FadeOutEnd;

		internal static int TimeIntro_End;

		public LawnApp mApp;

		public Board mBoard;

		public int mCutsceneTime;

		public int mSodTime;

		public int mGraveStoneTime;

		public int mReadySetPlantTime;

		public int mFogTime;

		public int mBossTime;

		public int mCrazyDaveTime;

		public int mLawnMowerTime;

		public int mCrazyDaveDialogStart;

		public bool mSeedChoosing;

		public Reanimation mZombiesWonReanimID;

		public bool mPreloaded;

		public bool mPlacedZombies;

		public bool mPlacedLawnItems;

		public int mCrazyDaveCountDown;

		public int mCrazyDaveLastTalkIndex;

		public bool mUpsellHideBoard;

		private ChallengeScreen mUpsellChallengeScreen;

		public bool mPreUpdatingBoard;

		private static TodWeightedGridArray[] aPicks;

		static CutScene()
		{
			TimePanRightStart = 1500;
			TimePanRightEnd = TimePanRightStart + 2000;
			TimeEarlyDaveEnterStart = TimePanRightStart + 500;
			TimeEarlyDaveEnterEnd = TimeEarlyDaveEnterStart + 750;
			TimeEarlyDaveLeaveStart = TimeEarlyDaveEnterEnd + 500;
			TimeEarlyDaveLeaveEnd = TimeEarlyDaveLeaveStart + 750;
			TimeSeedChoserSlideOnStart = TimePanRightEnd + 500;
			TimeSeedChoserSlideOnEnd = TimeSeedChoserSlideOnStart + 250;
			TimeSeedChoserSlideOffStart = TimeSeedChoserSlideOnEnd + 250;
			TimeSeedChoserSlideOffEnd = TimeSeedChoserSlideOffStart + 250;
			TimeSeedBankOnStart = TimeSeedChoserSlideOnStart;
			TimeSeedBankOnEnd = TimeSeedChoserSlideOnEnd;
			TimePanLeftStart = TimeSeedChoserSlideOffStart;
			TimePanLeftEnd = TimePanLeftStart + 1500;
			TimeSeedBankRightStart = TimeSeedChoserSlideOffEnd;
			TimeSeedBankRightEnd = TimePanLeftEnd;
			TimeRollSodStart = TimePanLeftEnd;
			TimeRollSodEnd = TimeRollSodStart + 2000;
			TimeGraveStoneStart = TimePanLeftEnd;
			TimeGraveStoneEnd = TimeRollSodStart + 1000;
			TimeLawnMowerStart = new int[6]
			{
				TimePanLeftEnd + 300,
				TimePanLeftEnd + 250,
				TimePanLeftEnd + 200,
				TimePanLeftEnd + 150,
				TimePanLeftEnd + 100,
				TimePanLeftEnd + 50
			};
			TimeLawnMowerDuration = 250;
			TimeReadySetPlantStart = TimePanLeftEnd;
			TimeReadySetPlantEnd = TimeReadySetPlantStart + 1830;
			TimeFogRollIn = TimePanLeftEnd - 50;
			TimeCrazyDaveEnterStart = TimePanLeftEnd + 500;
			TimeCrazyDaveEnterEnd = TimeCrazyDaveEnterStart + 750;
			TimeCrazyDaveLeaveStart = TimeCrazyDaveEnterEnd + 500;
			TimeCrazyDaveLeaveEnd = TimeCrazyDaveLeaveStart + 750;
			TimeIntroEnd = TimeReadySetPlantStart;
			LostTimePanRightStart = 1500;
			LostTimePanRightEnd = TimePanRightStart + 2000;
			LostTimeBrainGraphicStart = LostTimePanRightEnd + 3500;
			LostTimeBrainGraphicShake = LostTimeBrainGraphicStart + 1000;
			LostTimeBrainGraphicCancelShake = LostTimeBrainGraphicShake + 1000;
			LostTimeBrainGraphicEnd = LostTimeBrainGraphicCancelShake + 3000;
			LostTimeEnd = LostTimeBrainGraphicEnd;
			TimeIntro_PresentsFadeIn = 1000;
			TimeIntro_LogoStart = TimeIntro_PresentsFadeIn + 4500;
			TimeIntro_LogoEnd = TimeIntro_LogoStart + 400;
			TimeIntro_PanRightStart = TimeIntro_PresentsFadeIn + 4890;
			TimeIntro_PanRightEnd = TimeIntro_PanRightStart + 6000;
			TimeIntro_FadeOut = TimeIntro_PanRightStart + 5000;
			TimeIntro_FadeOutEnd = TimeIntro_FadeOut + 1000;
			TimeIntro_End = TimeIntro_FadeOutEnd + 2000;
			aPicks = new TodWeightedGridArray[25];
			for (int i = 0; i < aPicks.Length; i++)
			{
				aPicks[i] = TodWeightedGridArray.GetNewTodWeightedGridArray();
			}
		}

		public CutScene()
		{
			mApp = (LawnApp)GlobalStaticVars.gSexyAppBase;
			mBoard = mApp.mBoard;
			mCutsceneTime = 0;
			mSodTime = 0;
			mFogTime = 0;
			mBossTime = 0;
			mCrazyDaveTime = 0;
			mGraveStoneTime = 0;
			mReadySetPlantTime = 0;
			mLawnMowerTime = 0;
			mCrazyDaveDialogStart = -1;
			mSeedChoosing = false;
			mZombiesWonReanimID = null;
			mPreloaded = false;
			mPlacedZombies = false;
			mPlacedLawnItems = false;
			mCrazyDaveCountDown = 0;
			mCrazyDaveLastTalkIndex = -1;
			mUpsellHideBoard = false;
			mUpsellChallengeScreen = null;
			mPreUpdatingBoard = false;
		}

		public void Dispose()
		{
			mApp.mMuteSoundsForCutscene = false;
		}

		public void StartLevelIntro()
		{
			mCutsceneTime = 0;
			mBoard.mSeedBank.Move(-mBoard.mSeedBank.mWidth, 0);
			mBoard.mMenuButton.mBtnNoDraw = true;
			mApp.mSeedChooserScreen.mMouseVisible = false;
			mApp.mSeedChooserScreen.Move(0, Constants.SEED_CHOOSER_OFFSETSCREEN_OFFSET);
			mApp.mSeedChooserScreen.mMenuButton.mBtnNoDraw = true;
			mBoard.mShowShovel = false;
			mBoard.mSeedBank.mCutSceneDarken = 255;
			mPlacedZombies = false;
			mPreloaded = false;
			mPlacedLawnItems = false;
			mApp.mWidgetManager.SetFocus(mBoard);
			bool flag = false;
			if (!mApp.IsFirstTimeAdventureMode())
			{
				flag = false;
			}
			else if (mBoard.mLevel == 1 || mBoard.mLevel == 2 || mBoard.mLevel == 4)
			{
				flag = true;
			}
			if (flag)
			{
				mSodTime = TimeRollSodEnd - TimeRollSodStart;
				mBoard.mSodPosition = 0;
			}
			else
			{
				mSodTime = 0;
				mBoard.mSodPosition = 1000;
			}
			mGraveStoneTime = 0;
			mBoard.mEnableGraveStones = false;
			if (mBoard.StageHasGraveStones())
			{
				if ((mApp.IsAdventureMode() || mApp.IsQuickPlayMode()) && mApp.IsWhackAZombieLevel())
				{
					mGraveStoneTime = 0;
				}
				else if (!IsSurvivalRepick())
				{
					mGraveStoneTime = TimeGraveStoneEnd - TimeGraveStoneStart;
				}
			}
			if (mApp.IsFirstTimeAdventureMode() && mBoard.mLevel <= 2)
			{
				mReadySetPlantTime = 0;
			}
			else if (mApp.IsShovelLevel() || mApp.IsSquirrelLevel() || mApp.IsWallnutBowlingLevel() || mApp.IsIZombieLevel() || mApp.IsWhackAZombieLevel() || mApp.IsScaryPotterLevel() || mApp.mGameMode == GameMode.GAMEMODE_CHALLENGE_LAST_STAND || mApp.mGameMode == GameMode.GAMEMODE_CHALLENGE_ZOMBIQUARIUM || mApp.mGameMode == GameMode.GAMEMODE_CHALLENGE_LAST_STAND || mApp.mGameMode == GameMode.GAMEMODE_TREE_OF_WISDOM)
			{
				mReadySetPlantTime = 0;
			}
			else
			{
				mReadySetPlantTime = TimeReadySetPlantEnd - TimeReadySetPlantStart;
			}
			mLawnMowerTime = 0;
			if (!IsSurvivalRepick())
			{
				mLawnMowerTime = 550;
			}
			bool flag2 = false;
			if (mBoard.mPrevBoardResult == BoardResult.BOARDRESULT_RESTART || mBoard.mPrevBoardResult == BoardResult.BOARDRESULT_LOST)
			{
				flag2 = true;
			}
			if (mApp.IsFirstTimeAdventureMode() && mBoard.mLevel == 11)
			{
				mCrazyDaveDialogStart = 201;
			}
			else if (mApp.IsFirstTimeAdventureMode() && mBoard.mLevel == 12)
			{
				mCrazyDaveDialogStart = 1401;
			}
			else if (mApp.IsFirstTimeAdventureMode() && mBoard.mLevel >= 13 && mBoard.mLevel <= 24 && mBoard.mLevel != 15 && mBoard.mLevel != 20 && mBoard.mLevel != 21 && CanGetPacketUpgrade())
			{
				mCrazyDaveDialogStart = 1501;
			}
			else if (mApp.IsFirstTimeAdventureMode() && mBoard.mLevel >= 16 && mBoard.mLevel <= 24 && mBoard.mLevel != 20 && mBoard.mLevel != 21 && CanGetSecondPacketUpgrade())
			{
				mCrazyDaveDialogStart = 1551;
			}
			else if (mApp.IsWallnutBowlingLevel() && mApp.IsAdventureMode())
			{
				if (mApp.IsFirstTimeAdventureMode())
				{
					mCrazyDaveDialogStart = 2400;
				}
				else
				{
					mCrazyDaveDialogStart = 2411;
					mBoard.mChallenge.mShowBowlingLine = true;
				}
				mBoard.mShowShovel = true;
			}
			else if (mApp.IsFirstTimeAdventureMode() && mBoard.mLevel == 21)
			{
				mCrazyDaveDialogStart = 501;
			}
			else if (mApp.IsWhackAZombieLevel() && mApp.IsAdventureMode())
			{
				mCrazyDaveDialogStart = 401;
			}
			else if (mApp.IsLittleTroubleLevel() && mApp.IsAdventureMode())
			{
				mCrazyDaveDialogStart = 701;
			}
			else if (mApp.IsFirstTimeAdventureMode() && mBoard.mLevel == 31)
			{
				mCrazyDaveDialogStart = 801;
			}
			else if (mApp.IsScaryPotterLevel() && mApp.IsAdventureMode())
			{
				mCrazyDaveDialogStart = 2500;
			}
			else if (mApp.IsStormyNightLevel() && mApp.IsAdventureMode())
			{
				mCrazyDaveDialogStart = 1101;
			}
			else if (mApp.IsFirstTimeAdventureMode() && mBoard.mLevel == 41)
			{
				mCrazyDaveDialogStart = 1201;
			}
			else if (mApp.IsBungeeBlitzLevel() && mApp.IsAdventureMode())
			{
				mCrazyDaveDialogStart = 1304;
			}
			else if (!mApp.IsFirstTimeAdventureMode() && !mApp.IsQuickPlayMode() && mBoard.mLevel == 1)
			{
				mCrazyDaveDialogStart = 1601;
			}
			else if (mApp.mGameMode == GameMode.GAMEMODE_PUZZLE_I_ZOMBIE_1)
			{
				mCrazyDaveDialogStart = 2200;
			}
			else if (mApp.mGameMode == GameMode.GAMEMODE_UPSELL)
			{
				mCrazyDaveDialogStart = 3300;
				mUpsellHideBoard = true;
				mBoard.mMenuButton.mBtnNoDraw = false;
			}
			else if (mApp.mGameMode == GameMode.GAMEMODE_SCARY_POTTER_1 && !mApp.HasBeatenChallenge(GameMode.GAMEMODE_SCARY_POTTER_1))
			{
				mCrazyDaveDialogStart = 3000;
			}
			else if (mApp.IsFinalBossLevel() && mApp.IsAdventureMode() && !flag2)
			{
				mCrazyDaveDialogStart = 2300;
			}
			if (mCrazyDaveDialogStart != -1)
			{
				mCrazyDaveTime = TimeEarlyDaveLeaveEnd - TimePanRightStart;
				if (mApp.IsFinalBossLevel() && mApp.IsAdventureMode())
				{
					mCrazyDaveTime += 4000;
				}
			}
			if (mBoard.StageHasFog())
			{
				mFogTime = TimeFogRollIn + 2000 - TimeReadySetPlantStart - mLawnMowerTime - mSodTime;
			}
			else
			{
				mFogTime = 0;
			}
			if (mApp.IsFinalBossLevel())
			{
				mBossTime = 4000;
			}
			else
			{
				mBossTime = 0;
			}
			if (IsScrolledLeftAtStart())
			{
				mBoard.Move((int)((float)Constants.BOARD_OFFSET * Constants.S), 0);
			}
			if (IsNonScrollingCutscene() && mCrazyDaveTime == 0)
			{
				CancelIntro();
				return;
			}
			if (mApp.IsFinalBossLevel() || mApp.IsScaryPotterLevel() || mApp.IsWallnutBowlingLevel())
			{
				PreloadResources();
				PlaceLawnItems();
			}
			string theText = string.Empty;
			if (mCrazyDaveTime <= 0 && mApp.mGameMode != GameMode.GAMEMODE_INTRO && (mApp.mGameMode == GameMode.GAMEMODE_ADVENTURE || mApp.IsQuickPlayMode()))
			{
				if (mBoard.mBackground == BackgroundType.BACKGROUND_1_DAY || mBoard.mBackground == BackgroundType.BACKGROUND_2_NIGHT)
				{
					theText = TodStringFile.TodStringTranslate("[PLAYERS_HOUSE]");
				}
				else if (mBoard.mBackground == BackgroundType.BACKGROUND_3_POOL || mBoard.mBackground == BackgroundType.BACKGROUND_4_FOG)
				{
					theText = TodStringFile.TodStringTranslate("[PLAYERS_BACKYARD]");
				}
				else if (mBoard.mBackground == BackgroundType.BACKGROUND_5_ROOF || mBoard.mBackground == BackgroundType.BACKGROUND_6_BOSS)
				{
					theText = TodStringFile.TodStringTranslate("[PLAYERS_ROOF]");
				}
				else
				{
					Debug.ASSERT(false);
				}
			}
			theText = TodCommon.TodReplaceString(theText, "{PLAYER}", mApp.mPlayerInfo.mName);
			if (!theText.empty())
			{
				mBoard.DisplayAdvice(theText, MessageStyle.MESSAGE_STYLE_HOUSE_NAME, AdviceType.ADVICE_NONE);
			}
			if (mApp.mGameMode == GameMode.GAMEMODE_UPSELL)
			{
				mApp.mMusic.StopAllMusic();
			}
			else if (mApp.mGameMode == GameMode.GAMEMODE_INTRO)
			{
				mApp.mMusic.MakeSureMusicIsPlaying(MusicTune.MUSIC_TUNE_POOL_WATERYGRAVES);
			}
			else if (mCrazyDaveTime > 0)
			{
				mApp.mMusic.MakeSureMusicIsPlaying(MusicTune.MUSIC_TUNE_TITLE_CRAZY_DAVE_MAIN_THEME);
			}
			else if (mApp.IsFinalBossLevel())
			{
				mApp.mMusic.StopAllMusic();
			}
			else
			{
				mApp.mMusic.MakeSureMusicIsPlaying(MusicTune.MUSIC_TUNE_CHOOSE_YOUR_SEEDS);
			}
		}

		public void CancelIntro()
		{
			PreloadResources();
			PlaceStreetZombies();
			if (mCutsceneTime < TimePanRightEnd + mCrazyDaveTime)
			{
				mCutsceneTime = TimeSeedChoserSlideOnEnd + mCrazyDaveTime - 20;
				if (!IsNonScrollingCutscene())
				{
					mBoard.Move(Constants.BOARD_OFFSET - Constants.BACKGROUND_IMAGE_WIDTH + Constants.WIDE_BOARD_WIDTH, 0);
				}
				if (mBoard.mAdvice.mMessageStyle == MessageStyle.MESSAGE_STYLE_HOUSE_NAME)
				{
					mBoard.ClearAdvice(AdviceType.ADVICE_NONE);
				}
				if (mCrazyDaveDialogStart != -1)
				{
					if (mApp.mCrazyDaveState == CrazyDaveState.CRAZY_DAVE_OFF)
					{
						mApp.CrazyDaveEnter();
					}
					mApp.mCrazyDaveMessageIndex = mCrazyDaveDialogStart;
				}
				while (mApp.mCrazyDaveMessageIndex != -1)
				{
					AdvanceCrazyDaveDialog(true);
				}
				if (mBoard.mLevel == 5)
				{
					int count = mBoard.mPlants.Count;
					for (int i = 0; i < count; i++)
					{
						Plant plant = mBoard.mPlants[i];
						if (!plant.mDead)
						{
							plant.Die();
						}
					}
					mBoard.mChallenge.mShowBowlingLine = true;
				}
			}
			mApp.CrazyDaveDie();
			if (mCutsceneTime > TimePanLeftStart + mCrazyDaveTime || !mBoard.ChooseSeedsOnCurrentLevel())
			{
				mCutsceneTime = TimeIntroEnd + mLawnMowerTime + mSodTime + mGraveStoneTime + mCrazyDaveTime + mFogTime + mBossTime + mReadySetPlantTime - 20;
				PlaceLawnItems();
				if (mApp.IsStormyNightLevel())
				{
					mBoard.mChallenge.mChallengeStateCounter = 0;
				}
				if (mApp.IsFinalBossLevel())
				{
					mBoard.mChallenge.PlayBossEnter();
				}
				if (!mApp.IsChallengeWithoutSeedBank())
				{
					mBoard.mSeedBank.Move(0, 0);
				}
				mBoard.mEnableGraveStones = true;
				ShowShovel();
				if (mApp.IsFinalBossLevel())
				{
					mApp.mMusic.StartGameMusic();
				}
				if (mBoard.mFogBlownCountDown > 0)
				{
					mBoard.mFogBlownCountDown = 0;
					mBoard.mFogOffset = 0f;
				}
				if (mBoard.mTutorialState != TutorialState.TUTORIAL_ZEN_GARDEN_PICKUP_WATER)
				{
					mBoard.mMenuButton.mBtnNoDraw = false;
				}
				mApp.mSoundSystem.StopFoley(FoleyType.FOLEY_DIGGER);
			}
		}

		public void Update(bool updateDave)
		{
			if (mPreUpdatingBoard)
			{
				return;
			}
			if (IsShowingCrazyDave() && mApp.mGameScene == GameScenes.SCENE_LEVEL_INTRO && (!mBoard.mPaused || mApp.mGameMode != GameMode.GAMEMODE_UPSELL) && updateDave)
			{
				mApp.UpdateCrazyDave();
			}
			if (mBoard.mPaused)
			{
				return;
			}
			if (mApp.mGameScene == GameScenes.SCENE_ZOMBIES_WON)
			{
				mCutsceneTime += 10;
				UpdateZombiesWon();
			}
			else
			{
				if (mApp.mGameScene != GameScenes.SCENE_LEVEL_INTRO || mBoard.mDrawCount == 0)
				{
					return;
				}
				if (!mPreloaded)
				{
					PreloadResources();
				}
				if (!mPlacedZombies)
				{
					PlaceStreetZombies();
				}
				if (IsNonScrollingCutscene() || !mBoard.ChooseSeedsOnCurrentLevel())
				{
					PlaceLawnItems();
				}
				bool flag = false;
				if (mSeedChoosing)
				{
					flag = true;
				}
				else if (mApp.mCrazyDaveMessageIndex != -1)
				{
					flag = true;
				}
				else if (IsInShovelTutorial())
				{
					flag = true;
				}
				if (mApp.mGameMode == GameMode.GAMEMODE_UPSELL)
				{
					UpdateUpsell();
					if (mApp.mCrazyDaveState != 0 && mApp.mCrazyDaveState != CrazyDaveState.CRAZY_DAVE_ENTERING)
					{
						flag = true;
					}
				}
				if (mApp.mGameMode == GameMode.GAMEMODE_INTRO)
				{
					mCutsceneTime += 10;
					UpdateIntro();
					return;
				}
				if (!flag)
				{
					mCutsceneTime += 10;
					if (mCutsceneTime == TimeSeedChoserSlideOnEnd + mCrazyDaveTime && mBoard.ChooseSeedsOnCurrentLevel())
					{
						StartSeedChooser();
					}
				}
				int num = TimeIntroEnd + mLawnMowerTime + mSodTime + mGraveStoneTime + mCrazyDaveTime + mFogTime + mBossTime + mReadySetPlantTime;
				if (mCutsceneTime >= num)
				{
					mBoard.RemoveCutsceneZombies();
					if (mBoard.mTutorialState != TutorialState.TUTORIAL_ZEN_GARDEN_PICKUP_WATER && mBoard.mTutorialState != TutorialState.TUTORIAL_ZEN_GARDEN_KEEP_WATERING && mBoard.mTutorialState != TutorialState.TUTORIAL_ZEN_GARDEN_FERTILIZE_PLANTS && mBoard.mTutorialState != TutorialState.TUTORIAL_ZEN_GARDEN_VISIT_STORE && mBoard.mTutorialState != TutorialState.TUTORIAL_ZEN_GARDEN_WATER_PLANT && mBoard.mTutorialState != TutorialState.TUTORIAL_ZEN_GARDEN_COMPLETED)
					{
						mBoard.mMenuButton.mBtnNoDraw = false;
					}
					ShowShovel();
					mApp.StartPlaying();
				}
				else
				{
					AnimateBoard();
				}
			}
		}

		public void AnimateBoard()
		{
			int timeEarlyDaveEnterStart = TimeEarlyDaveEnterStart;
			int timeEarlyDaveEnterEnd = TimeEarlyDaveEnterEnd;
			int timeEarlyDaveLeaveEnd = TimeEarlyDaveLeaveEnd;
			int num = TimePanRightStart + mCrazyDaveTime;
			int num2 = TimePanRightEnd + mCrazyDaveTime;
			int num3 = TimePanLeftStart + mCrazyDaveTime;
			int num4 = TimePanLeftEnd + mCrazyDaveTime;
			if (mCrazyDaveTime > 0)
			{
				if (mCutsceneTime == timeEarlyDaveEnterStart)
				{
					mApp.CrazyDaveEnter();
					if (mApp.mGameMode == GameMode.GAMEMODE_UPSELL)
					{
						Reanimation reanimation = mApp.ReanimationTryToGet(mApp.mCrazyDaveReanimID);
						reanimation.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_anim_enterup, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 0, 12f);
						reanimation.SetPosition(100f * Constants.S, 70f * Constants.S);
					}
				}
				if (mCutsceneTime == timeEarlyDaveEnterEnd && mCrazyDaveDialogStart != -1)
				{
					mApp.CrazyDaveTalkIndex(mCrazyDaveDialogStart);
					mCrazyDaveDialogStart = -1;
				}
				if (mCutsceneTime == timeEarlyDaveLeaveEnd && IsNonScrollingCutscene())
				{
					mCutsceneTime = num4;
				}
			}
			int num5 = Constants.BOARD_OFFSET;
			if (!IsScrolledLeftAtStart())
			{
				num5 = (int)(Constants.IS * (float)Constants.Board_Offset_AspectRatio_Correction);
			}
			if (mCutsceneTime <= num)
			{
				mBoard.Move((int)((float)num5 * Constants.S), 0);
			}
			if (mCutsceneTime > num && mCutsceneTime <= num2)
			{
				int thePositionStart = -num5;
				int thePositionEnd = -Constants.BOARD_OFFSET + Constants.BACKGROUND_IMAGE_WIDTH - Constants.WIDE_BOARD_WIDTH + Constants.Board_Cutscene_ExtraScroll;
				int num6 = CalcPosition(num, num2, thePositionStart, thePositionEnd);
				mBoard.Move(-(int)((float)num6 * Constants.S), 0);
			}
			if (mBoard.ChooseSeedsOnCurrentLevel())
			{
				int num7 = TimeSeedChoserSlideOnStart + mCrazyDaveTime;
				int num8 = TimeSeedChoserSlideOnEnd + mCrazyDaveTime;
				if (mCutsceneTime > num7 && mCutsceneTime <= num8)
				{
					int sEED_CHOOSER_OFFSETSCREEN_OFFSET = Constants.SEED_CHOOSER_OFFSETSCREEN_OFFSET;
					int thePositionEnd2 = 0;
					int theNewY = CalcPosition(num7, num8, sEED_CHOOSER_OFFSETSCREEN_OFFSET, thePositionEnd2);
					mApp.mSeedChooserScreen.Move(0, theNewY);
					int mY = CalcPosition(num7, num8, (int)Constants.InvertAndScale(-50f), Constants.UIMenuButtonPosition.Y);
					mApp.mSeedChooserScreen.mMenuButton.mY = mY;
					mApp.mSeedChooserScreen.mMenuButton.mBtnNoDraw = false;
				}
				int num9 = TimeSeedChoserSlideOffStart + mCrazyDaveTime;
				int num10 = TimeSeedChoserSlideOffEnd + mCrazyDaveTime;
				if (mCutsceneTime > num9 && mCutsceneTime <= num10)
				{
					int thePositionStart2 = 0;
					int sEED_CHOOSER_OFFSETSCREEN_OFFSET2 = Constants.SEED_CHOOSER_OFFSETSCREEN_OFFSET;
					int theNewY2 = CalcPosition(num9, num10, thePositionStart2, sEED_CHOOSER_OFFSETSCREEN_OFFSET2);
					mApp.mSeedChooserScreen.Move(0, theNewY2);
					mApp.mSeedChooserScreen.mMenuButton.mDisabled = true;
				}
			}
			if (mCutsceneTime > num3)
			{
				int thePositionStart3 = Constants.BACKGROUND_IMAGE_WIDTH - Constants.WIDE_BOARD_WIDTH - Constants.BOARD_OFFSET + Constants.Board_Cutscene_ExtraScroll + (int)(Constants.IS * (float)Constants.Board_Offset_AspectRatio_Correction);
				int thePositionEnd3 = 0;
				int num11 = CalcPosition(num3, num4, thePositionStart3, thePositionEnd3);
				mBoard.Move(-(int)((float)num11 * Constants.S) + Constants.Board_Offset_AspectRatio_Correction, 0);
			}
			int num12 = 0;
			if (!mBoard.ChooseSeedsOnCurrentLevel())
			{
				num12 = TimePanLeftEnd - TimeSeedChoserSlideOnStart + mSodTime + mGraveStoneTime + mFogTime + mBossTime;
			}
			int num13 = TimeSeedBankOnStart + mCrazyDaveTime + num12;
			int num14 = TimeSeedBankOnEnd + mCrazyDaveTime + num12;
			if (!mApp.IsChallengeWithoutSeedBank() && mCutsceneTime > num13 && mCutsceneTime <= num14)
			{
				int x = CalcPosition(num13, num14, -mBoard.mSeedBank.mWidth, 0);
				mBoard.mSeedBank.Move(x, 0);
			}
			int num15 = TimeSeedBankRightStart + mCrazyDaveTime;
			int theTimeEnd = TimeSeedBankRightEnd + mCrazyDaveTime;
			if (mCutsceneTime > num15)
			{
				mBoard.mSeedBank.mCutSceneDarken = TodCommon.TodAnimateCurve(num15, theTimeEnd, mCutsceneTime, 255, 128, TodCurves.CURVE_EASE_OUT);
			}
			if (mSodTime > 0)
			{
				int num16 = TimeRollSodStart + mCrazyDaveTime;
				int num17 = TimeRollSodEnd + mCrazyDaveTime;
				int mSodPosition = TodCommon.TodAnimateCurve(num16, num17, mCutsceneTime, 0, 1000, TodCurves.CURVE_LINEAR);
				mBoard.mSodPosition = mSodPosition;
				if (mCutsceneTime == num16)
				{
					mApp.PlayFoley(FoleyType.FOLEY_DIGGER);
					if (mBoard.mLevel == 1)
					{
						mApp.AddReanimation(Constants.BOARD_EDGE, 0f, 400000, ReanimationType.REANIM_SODROLL, false);
						mApp.AddTodParticle(Constants.CutScene_ExtraRoom_1_Particle_Pos.X + Constants.BOARD_EXTRA_ROOM, Constants.CutScene_ExtraRoom_1_Particle_Pos.Y, 400001, ParticleEffect.PARTICLE_SOD_ROLL);
					}
					else if (mBoard.mLevel == 2)
					{
						mApp.AddReanimation((float)Constants.BOARD_EDGE - 10f, (float)Constants.CutScene_SodRoll_1_Pos * Constants.S, 400000, ReanimationType.REANIM_SODROLL, false);
						mApp.AddReanimation((float)Constants.BOARD_EDGE - 10f, (float)Constants.CutScene_SodRoll_2_Pos * Constants.S, 400000, ReanimationType.REANIM_SODROLL, false);
						mApp.AddTodParticle(Constants.CutScene_ExtraRoom_2_Particle_Pos.X + Constants.BOARD_EXTRA_ROOM, Constants.CutScene_ExtraRoom_2_Particle_Pos.Y, 400001, ParticleEffect.PARTICLE_SOD_ROLL);
						mApp.AddTodParticle(Constants.CutScene_ExtraRoom_3_Particle_Pos.X + Constants.BOARD_EXTRA_ROOM, Constants.CutScene_ExtraRoom_3_Particle_Pos.Y, 400001, ParticleEffect.PARTICLE_SOD_ROLL);
					}
					else if (mBoard.mLevel == 4)
					{
						mApp.AddReanimation((float)(Constants.CutScene_SodRoll_3_Pos.X + Constants.BOARD_EDGE) + 10f, (float)Constants.CutScene_SodRoll_3_Pos.Y * Constants.S, 400000, ReanimationType.REANIM_SODROLL, false);
						mApp.AddReanimation((float)(Constants.CutScene_SodRoll_4_Pos.X + Constants.BOARD_EDGE) + 10f, (float)Constants.CutScene_SodRoll_4_Pos.Y * Constants.S, 400000, ReanimationType.REANIM_SODROLL, false);
						mApp.AddTodParticle(Constants.CutScene_ExtraRoom_4_Particle_Pos.X + Constants.BOARD_EXTRA_ROOM, Constants.CutScene_ExtraRoom_4_Particle_Pos.Y, 400001, ParticleEffect.PARTICLE_SOD_ROLL);
						mApp.AddTodParticle(Constants.CutScene_ExtraRoom_5_Particle_Pos.X + Constants.BOARD_EXTRA_ROOM, Constants.CutScene_ExtraRoom_5_Particle_Pos.Y, 400001, ParticleEffect.PARTICLE_SOD_ROLL);
					}
				}
				if (mCutsceneTime == num17)
				{
					mApp.mSoundSystem.StopFoley(FoleyType.FOLEY_DIGGER);
				}
			}
			if (mGraveStoneTime > 0)
			{
				int num18 = TimeGraveStoneStart + mSodTime + mCrazyDaveTime;
				if (mCutsceneTime == num18)
				{
					mBoard.mEnableGraveStones = true;
					AddGraveStoneParticles();
				}
			}
			if (mCutsceneTime == num3)
			{
				PlaceLawnItems();
			}
			if (!IsSurvivalRepick())
			{
				for (int i = 0; i < Constants.MAX_GRIDSIZEY; i++)
				{
					int num19 = TimeLawnMowerStart[i] + mSodTime + mGraveStoneTime + mCrazyDaveTime;
					int theTimeEnd2 = num19 + TimeLawnMowerDuration;
					if (mCutsceneTime > num19)
					{
						LawnMower lawnMower = mBoard.FindLawnMowerInRow(i);
						if (lawnMower != null)
						{
							lawnMower.mVisible = true;
							int num20 = CalcPosition(num19, theTimeEnd2, -80, -21) + Constants.BOARD_EXTRA_ROOM;
							lawnMower.mPosX = num20;
						}
					}
				}
			}
			int num21 = TimeFogRollIn + mSodTime + mGraveStoneTime + mCrazyDaveTime;
			if (mBoard.mFogBlownCountDown > 0 && mCutsceneTime > num21)
			{
				if (mBoard.mFogBlownCountDown > 200)
				{
					mBoard.mFogBlownCountDown = 200;
				}
				mBoard.mFogBlownCountDown--;
			}
			if (mApp.IsStormyNightLevel())
			{
				if (mCutsceneTime == num2 - 1000)
				{
					mBoard.mChallenge.mChallengeState = ChallengeState.STATECHALLENGE_STORM_FLASH_2;
					mBoard.mChallenge.mChallengeStateCounter = 310;
				}
				else if (mCutsceneTime == num4)
				{
					mBoard.mChallenge.mChallengeState = ChallengeState.STATECHALLENGE_STORM_FLASH_2;
					mBoard.mChallenge.mChallengeStateCounter = 310;
				}
			}
			int num22 = TimeReadySetPlantStart + mLawnMowerTime + mCrazyDaveTime;
			if (mBossTime > 0 && mCutsceneTime == num22)
			{
				mBoard.mChallenge.PlayBossEnter();
			}
			if (mApp.IsFinalBossLevel() && mCutsceneTime == num13)
			{
				mApp.mMusic.StartGameMusic();
			}
			int num23 = TimeReadySetPlantStart + mLawnMowerTime + mSodTime + mGraveStoneTime + mCrazyDaveTime + mFogTime + mBossTime;
			if (mReadySetPlantTime > 0 && mCutsceneTime == num23)
			{
				int x2 = Constants.CutScene_ReadySetPlant_Pos.X;
				int y = Constants.CutScene_ReadySetPlant_Pos.Y;
				mApp.AddReanimation((float)x2 * Constants.IS, (float)y * Constants.IS, 900000, ReanimationType.REANIM_READYSETPLANT);
				mApp.PlaySample(Resources.SOUND_READYSETPLANT);
				mApp.IsFinalBossLevel();
			}
			if (mReadySetPlantTime == 0 && mCutsceneTime == num23 - 2000)
			{
				mApp.IsFinalBossLevel();
			}
		}

		public void StartSeedChooser()
		{
			mApp.mSeedChooserScreen.mMouseVisible = true;
			mSeedChoosing = true;
			mApp.mWidgetManager.SetFocus(mApp.mSeedChooserScreen);
		}

		public void EndSeedChooser()
		{
			mApp.mSeedChooserScreen.mMouseVisible = false;
			mSeedChoosing = false;
			mCutsceneTime = TimeSeedChoserSlideOnEnd + mCrazyDaveTime + 10;
			mApp.mWidgetManager.SetFocus(mBoard);
		}

		public int CalcPosition(int theTimeStart, int theTimeEnd, int thePositionStart, int thePositionEnd)
		{
			return TodCommon.TodAnimateCurve(theTimeStart, theTimeEnd, mCutsceneTime, thePositionStart, thePositionEnd, TodCurves.CURVE_EASE_IN_OUT);
		}

		public void PlaceStreetZombies()
		{
			if (mPlacedZombies)
			{
				return;
			}
			mPlacedZombies = true;
			if (mApp.IsFinalBossLevel())
			{
				return;
			}
			int num = 0;
			int[] array = new int[33];
			int num2 = 0;
			for (int i = 0; i < 33; i++)
			{
				array[i] = 0;
			}
			Debug.ASSERT(mBoard.mNumWaves <= 100);
			for (int j = 0; j < mBoard.mNumWaves; j++)
			{
				for (int k = 0; k < 50; k++)
				{
					ZombieType zombieType = mBoard.mZombiesInWave[j, k];
					if (zombieType == ZombieType.ZOMBIE_INVALID)
					{
						break;
					}
					num2 += Zombie.GetZombieDefinition(zombieType).mZombieValue;
					if (zombieType != ZombieType.ZOMBIE_FLAG && (zombieType != ZombieType.ZOMBIE_YETI || (!mApp.IsQuickPlayMode() && mApp.IsStormyNightLevel())) && (zombieType != ZombieType.ZOMBIE_BOBSLED || mApp.mGameMode == GameMode.GAMEMODE_CHALLENGE_BOBSLED_BONANZA))
					{
						Debug.ASSERT(zombieType >= ZombieType.ZOMBIE_NORMAL && zombieType < ZombieType.NUM_ZOMBIE_TYPES);
						array[(int)zombieType]++;
						num++;
						if (zombieType == ZombieType.ZOMBIE_BUNGEE || zombieType == ZombieType.ZOMBIE_BOBSLED)
						{
							array[(int)zombieType] = 1;
						}
					}
				}
			}
			if (mApp.mGameMode == GameMode.GAMEMODE_CHALLENGE_LAST_STAND)
			{
				for (int l = 0; l < 33; l++)
				{
					if (l != 19 && mBoard.mZombieAllowed[l])
					{
						array[l] = Math.Max(array[l], 1);
					}
				}
			}
			if (mBoard.StageHasPool())
			{
				array[10] = 1;
			}
			bool[,] array2 = new bool[5, 5];
			for (int m = 0; m < 5; m++)
			{
				for (int n = 0; n < 5; n++)
				{
					array2[m, n] = false;
				}
			}
			int num3 = 10;
			if (mApp.IsLittleTroubleLevel())
			{
				num3 = 15;
			}
			else if (mApp.IsStormyNightLevel() && (mApp.IsAdventureMode() || mApp.IsQuickPlayMode()))
			{
				num3 = 18;
			}
			else if (mApp.IsMiniBossLevel())
			{
				num3 = 18;
			}
			Debug.ASSERT(num3 <= 18);
			for (int num4 = 0; num4 < 33; num4++)
			{
				if (array[num4] != 0 && (Is2x2Zombie((ZombieType)num4) || num4 == 12))
				{
					FindAndPlaceZombie((ZombieType)num4, array2);
				}
			}
			for (int num5 = 0; num5 < 33; num5++)
			{
				if (array[num5] != 0 && !Is2x2Zombie((ZombieType)num5) && num5 != 12)
				{
					int num6 = array[num5] * num3 / num;
					num6 = TodCommon.ClampInt(num6, 1, array[num5]);
					for (int num7 = 0; num7 < num6; num7++)
					{
						FindAndPlaceZombie((ZombieType)num5, array2);
					}
				}
			}
		}

		public void AddGraveStoneParticles()
		{
			int index = -1;
			GridItem theGridItem = null;
			while (mBoard.IterateGridItems(ref theGridItem, ref index))
			{
				if (theGridItem.mGridItemType == GridItemType.GRIDITEM_GRAVESTONE)
				{
					theGridItem.AddGraveStoneParticles();
				}
			}
		}

		public void PlaceAZombie(ZombieType theZombieType, int theGridX, int theGridY)
		{
			bool flag = false;
			if (theZombieType == ZombieType.ZOMBIE_DUCKY_TUBE && mApp.mGameMode == GameMode.GAMEMODE_CHALLENGE_WAR_AND_PEAS_2)
			{
				theZombieType = ZombieType.ZOMBIE_PEA_HEAD;
				flag = true;
			}
			Zombie zombie = mBoard.AddZombieInRow(theZombieType, 0, GameConstants.ZOMBIE_WAVE_CUTSCENE);
			Debug.ASSERT(zombie != null);
			zombie.mPosX = 830 + 56 * theGridX + 110;
			zombie.mPosY = 70 + 90 * theGridY;
			if (theGridX % 2 == 1)
			{
				zombie.mPosY += 30f;
			}
			if (flag)
			{
				Reanimation reanimation = mApp.ReanimationGet(zombie.mBodyReanimID);
				reanimation.AssignRenderGroupToPrefix("Zombie_duckytube", 0);
			}
			if (mBoard.StageHasRoof())
			{
				zombie.mPosY -= 7 * (5 - theGridX) - 2 * (5 - theGridY) + 5;
				zombie.mPosX -= 5f;
			}
			if (theZombieType == ZombieType.ZOMBIE_ZAMBONI)
			{
				zombie.mPosY -= 10f;
				zombie.mPosX -= 30f;
			}
			else if (mApp.IsLittleTroubleLevel())
			{
				zombie.mPosY += RandomNumbers.NextNumber(50) - 25;
				zombie.mPosX += RandomNumbers.NextNumber(50) - 25;
			}
			else if (Is2x2Zombie(theZombieType))
			{
				zombie.mPosX += -20 + RandomNumbers.NextNumber(15);
			}
			else if (theGridY == 4 && (mApp.CanShowAlmanac() || mApp.CanShowStore()))
			{
				zombie.mPosX += RandomNumbers.NextNumber(15);
			}
			else
			{
				zombie.mPosY += RandomNumbers.NextNumber(15);
				zombie.mPosX += RandomNumbers.NextNumber(15);
			}
			int num = theGridY * 2 + theGridX % 2;
			zombie.mRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_LAWN, 0, num * 2);
			if (theZombieType == ZombieType.ZOMBIE_BUNGEE)
			{
				zombie.mRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_GROUND, 0, 0);
				zombie.mPosX = 950f + (float)theGridX * 50f;
				zombie.mPosY = 50f;
				zombie.mRow = 0;
			}
			if (theZombieType == ZombieType.ZOMBIE_BOBSLED)
			{
				zombie.mPosX = 1105f;
				zombie.mPosY = 480f;
				zombie.mRow = 0;
				zombie.mRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_LAWN, 0, 1000);
			}
		}

		public bool CanZombieGoInGridSpot(ZombieType theZombieType, int theGridX, int theGridY, bool[,] theZombieGrid)
		{
			if (theZombieGrid[theGridX, theGridY])
			{
				return false;
			}
			if (Is2x2Zombie(theZombieType))
			{
				if (theGridX == 0 || theGridY == 0)
				{
					return false;
				}
				if (theZombieGrid[theGridX - 1, theGridY] || theZombieGrid[theGridX, theGridY - 1] || theZombieGrid[theGridX - 1, theGridY - 1])
				{
					return false;
				}
			}
			if (theGridX == 4 && theGridY == 0)
			{
				return false;
			}
			if (theGridX != 4 && theZombieType == ZombieType.ZOMBIE_ZAMBONI)
			{
				return false;
			}
			if (theGridX == 0 && mBoard.StageHasPool())
			{
				return false;
			}
			if (mBoard.StageHasRoof() && theGridX == 0 && theGridY == 0)
			{
				return false;
			}
			if (theGridX == 4 && mBoard.StageHasFog() && theZombieType == ZombieType.ZOMBIE_BALLOON)
			{
				return false;
			}
			if (theZombieType == ZombieType.ZOMBIE_GARGANTUAR || theZombieType == ZombieType.ZOMBIE_REDEYE_GARGANTUAR || theZombieType == ZombieType.ZOMBIE_ZAMBONI || theZombieType == ZombieType.ZOMBIE_BOBSLED || theZombieType == ZombieType.ZOMBIE_POLEVAULTER)
			{
				switch (theGridX)
				{
				case 0:
					return false;
				case 1:
					if (mBoard.StageHasPool())
					{
						return false;
					}
					break;
				}
				if (theGridX == 1 && theGridY == 0)
				{
					return false;
				}
			}
			return true;
		}

		public bool IsSurvivalRepick()
		{
			if (mApp.IsSurvivalMode() && mBoard.mChallenge.mSurvivalStage > 0 && mApp.mGameScene == GameScenes.SCENE_LEVEL_INTRO)
			{
				return true;
			}
			return false;
		}

		public bool IsAfterSeedChooser()
		{
			return mCutsceneTime > TimeSeedChoserSlideOffStart + mCrazyDaveTime;
		}

		public void AddFlowerPots()
		{
			int num = 0;
			if (mBoard.mLevel == 41)
			{
				num = 5;
			}
			else if (mBoard.mLevel == 42)
			{
				num = 4;
			}
			else if (mBoard.mLevel >= 43 && mBoard.mLevel <= 50)
			{
				num = 3;
			}
			else if (mApp.mGameMode == GameMode.GAMEMODE_CHALLENGE_COLUMN)
			{
				num = 8;
			}
			else if (mBoard.StageHasRoof())
			{
				num = 3;
			}
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < Constants.MAX_GRIDSIZEY; j++)
				{
					if (mBoard.CanPlantAt(i, j, SeedType.SEED_FLOWERPOT) == PlantingReason.PLANTING_OK)
					{
						Plant newPlant = Plant.GetNewPlant();
						newPlant.mIsOnBoard = true;
						newPlant.PlantInitialize(i, j, SeedType.SEED_FLOWERPOT, SeedType.SEED_NONE);
						mBoard.mPlants.Add(newPlant);
					}
				}
			}
		}

		public void UpdateZombiesWon()
		{
			if (mCutsceneTime > LostTimePanRightStart && mCutsceneTime <= LostTimePanRightEnd)
			{
				int num = CalcPosition(TimePanRightStart, TimePanRightEnd, (int)((float)Constants.Board_Offset_AspectRatio_Correction * Constants.IS), Constants.BOARD_OFFSET);
				mBoard.Move((int)((float)num * Constants.S), 0);
			}
			if (mCutsceneTime == LostTimeBrainGraphicStart - 400 || mCutsceneTime == LostTimeBrainGraphicStart - 900)
			{
				mApp.PlayFoley(FoleyType.FOLEY_CHOMP);
			}
			if (mCutsceneTime == LostTimeBrainGraphicStart)
			{
				ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_ZOMBIES_WON, true);
				int num2 = Constants.BOARD_EXTRA_ROOM / 2;
				Reanimation reanimation = mApp.AddReanimation(-Constants.BOARD_OFFSET + num2 + Constants.Board_Offset_AspectRatio_Correction, 0f, 900000, ReanimationType.REANIM_ZOMBIES_WON);
				reanimation.mAnimRate = 12f;
				reanimation.mLoopType = ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD;
				ReanimatorTrackInstance trackInstanceByName = reanimation.GetTrackInstanceByName(Reanimation.ReanimTrackId_fullscreen);
				trackInstanceByName.mTrackColor = SexyColor.Black;
				mZombiesWonReanimID = mApp.ReanimationGetID(reanimation);
				reanimation.SetFramesForLayer(GlobalMembersReanimIds.ReanimTrackId_zombieswon);
				mApp.PlayFoley(FoleyType.FOLEY_SCREAM);
			}
			if (mCutsceneTime == LostTimeBrainGraphicShake)
			{
				Reanimation reanimation2 = mApp.ReanimationGet(mZombiesWonReanimID);
				reanimation2.SetShakeOverride(GlobalMembersReanimIds.ReanimTrackId_zombieswon, 1f);
			}
			if (mCutsceneTime == LostTimeBrainGraphicCancelShake)
			{
				Reanimation reanimation3 = mApp.ReanimationGet(mZombiesWonReanimID);
				reanimation3.SetShakeOverride(GlobalMembersReanimIds.ReanimTrackId_zombieswon, 0f);
			}
			if (mCutsceneTime == LostTimeBrainGraphicEnd)
			{
				Reanimation reanimation4 = mApp.ReanimationGet(mZombiesWonReanimID);
				reanimation4.SetFramesForLayer(GlobalMembersReanimIds.ReanimTrackId_anim_screen);
			}
			if (mCutsceneTime == LostTimeEnd)
			{
				if (mApp.IsSurvivalMode())
				{
					int survivalFlagsCompleted = mBoard.GetSurvivalFlagsCompleted();
					string theStringToSubstitute = mApp.Pluralize(survivalFlagsCompleted, "[ONE_FLAG]", "[COUNT_FLAGS]");
					string theMessage = TodCommon.TodReplaceString("[SURVIVAL_DEATH_MESSAGE]", "{FLAGS}", theStringToSubstitute);
					GameOverDialog theDialog = new GameOverDialog(theMessage, true);
					mApp.AddDialog(17, theDialog);
				}
				else
				{
					GameOverDialog theDialog2 = new GameOverDialog("", false);
					mApp.AddDialog(17, theDialog2);
				}
			}
		}

		public void StartZombiesWon()
		{
			mCutsceneTime = 0;
			mBoard.mMenuButton.mBtnNoDraw = true;
			mBoard.mShowShovel = false;
			mApp.mMusic.StopAllMusic();
			mBoard.StopAllZombieSounds();
			mApp.PlaySample(Resources.SOUND_LOSEMUSIC);
		}

		public bool ShowZombieWalking()
		{
			return true;
		}

		public bool IsCutSceneOver()
		{
			Debug.ASSERT(mApp.mGameScene == GameScenes.SCENE_ZOMBIES_WON);
			if (mCutsceneTime >= LostTimeEnd)
			{
				return true;
			}
			return false;
		}

		public void ZombieWonClick()
		{
			if (IsCutSceneOver() || mApp.mTodCheatKeys)
			{
				mApp.EndLevel();
			}
		}

		public void MouseDown(int x, int y)
		{
			if (mApp.mTodCheatKeys && mApp.mGameMode == GameMode.GAMEMODE_UPSELL)
			{
				if (mCrazyDaveCountDown > 1)
				{
					mCrazyDaveCountDown = 1;
				}
			}
			else if (IsShowingCrazyDave())
			{
				AdvanceCrazyDaveDialog(false);
			}
			else if (mApp.mTodCheatKeys)
			{
				CancelIntro();
			}
		}

		public void AdvanceCrazyDaveDialog(bool theJustSkipping)
		{
			if (mApp.mGameMode == GameMode.GAMEMODE_UPSELL || mApp.mCrazyDaveMessageIndex == -1)
			{
				return;
			}
			if (mApp.mCrazyDaveMessageIndex == 2406 && !theJustSkipping)
			{
				mBoard.SetTutorialState(TutorialState.TUTORIAL_SHOVEL_PICKUP);
				mApp.CrazyDaveLeave();
				return;
			}
			if (!mApp.AdvanceCrazyDaveText())
			{
				mApp.CrazyDaveLeave();
				if (mApp.IsFinalBossLevel() && mApp.IsAdventureMode())
				{
					Reanimation reanimation = mApp.ReanimationGet(mApp.mCrazyDaveReanimID);
					reanimation.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_anim_grab, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 0, 18f);
					mApp.mMusic.FadeOut(50);
					if (!theJustSkipping)
					{
						mApp.PlaySample(Resources.SOUND_BUNGEE_SCREAM);
					}
				}
				else if (mBoard.ChooseSeedsOnCurrentLevel())
				{
					mApp.mMusic.MakeSureMusicIsPlaying(MusicTune.MUSIC_TUNE_CHOOSE_YOUR_SEEDS);
				}
				else if (IsNonScrollingCutscene())
				{
					mApp.mMusic.FadeOut(50);
				}
				return;
			}
			if (mApp.mCrazyDaveMessageIndex == 107 || mApp.mCrazyDaveMessageIndex == 2407)
			{
				mBoard.mChallenge.ShovelAddWallnuts();
			}
			if (mApp.mCrazyDaveMessageIndex == 405 || mApp.mCrazyDaveMessageIndex == 2411)
			{
				mBoard.mChallenge.mShowBowlingLine = true;
			}
			if ((mApp.mCrazyDaveMessageIndex == 1503 || mApp.mCrazyDaveMessageIndex == 1553) && !theJustSkipping)
			{
				int itemCost = StoreScreen.GetItemCost(StoreItem.STORE_ITEM_PACKET_UPGRADE);
				int num = mApp.mPlayerInfo.mPurchases[21] + 6;
				string theDialogLines = TodCommon.TodReplaceNumberString("[UPGRADE_DIALOG_BODY]", "{SLOTS}", num + 1);
				string moneyString = LawnApp.GetMoneyString(itemCost);
				LawnDialog lawnDialog = mApp.DoDialog(51, true, moneyString, theDialogLines, string.Empty, 1);
				lawnDialog.Resize((int)Constants.InvertAndScale(300f), (int)Constants.InvertAndScale(100f), (int)Constants.InvertAndScale(370f), (int)Constants.InvertAndScale(200f));
				mBoard.mCoinBankFadeCount = 100;
			}
			else if (mApp.mCrazyDaveMessageIndex == 406)
			{
				mBoard.mEnableGraveStones = true;
				AddGraveStoneParticles();
			}
		}

		public void ShowShovel()
		{
			if (!mApp.IsWhackAZombieLevel() && !mApp.IsWallnutBowlingLevel() && mApp.mGameMode != GameMode.GAMEMODE_CHALLENGE_BEGHOULED && mApp.mGameMode != GameMode.GAMEMODE_CHALLENGE_BEGHOULED_TWIST && !mApp.IsIZombieLevel() && mApp.mGameMode != GameMode.GAMEMODE_CHALLENGE_ZEN_GARDEN && mApp.mGameMode != GameMode.GAMEMODE_TREE_OF_WISDOM && mApp.mGameMode != GameMode.GAMEMODE_TREE_OF_WISDOM && (!mApp.IsFirstTimeAdventureMode() || mBoard.mLevel > 4))
			{
				mBoard.mShowShovel = true;
			}
		}

		public bool CanGetPacketUpgrade()
		{
			int itemCost = StoreScreen.GetItemCost(StoreItem.STORE_ITEM_PACKET_UPGRADE);
			if (mApp.mPlayerInfo.mPurchases[21] == 0 && mApp.mPlayerInfo.mCoins >= itemCost && mApp.mPlayerInfo.mDidntPurchasePacketUpgrade < 2)
			{
				return true;
			}
			return false;
		}

		public void FindPlaceForStreetZombies(ZombieType theZombieType, bool[,] theZombieGrid, ref int thePosX, ref int thePosY)
		{
			if (theZombieType == ZombieType.ZOMBIE_BUNGEE)
			{
				thePosX = 0;
				thePosY = 0;
				return;
			}
			for (int i = 0; i < aPicks.Length; i++)
			{
				aPicks[i].Reset();
			}
			int num = 0;
			for (int j = 0; j < 5; j++)
			{
				for (int k = 0; k < 5; k++)
				{
					if (CanZombieGoInGridSpot(theZombieType, j, k, theZombieGrid))
					{
						aPicks[num].mX = j;
						aPicks[num].mY = k;
						aPicks[num].mWeight = 1;
						num++;
					}
				}
			}
			if (num == 0)
			{
				thePosX = 2;
				thePosY = 2;
			}
			else
			{
				TodWeightedGridArray todWeightedGridArray = TodCommon.TodPickFromWeightedGridArray(aPicks, num);
				thePosX = todWeightedGridArray.mX;
				thePosY = todWeightedGridArray.mY;
			}
		}

		public void FindAndPlaceZombie(ZombieType theZombieType, bool[,] theZombieGrid)
		{
			int thePosX = 0;
			int thePosY = 0;
			FindPlaceForStreetZombies(theZombieType, theZombieGrid, ref thePosX, ref thePosY);
			if (theZombieType != ZombieType.ZOMBIE_BUNGEE)
			{
				theZombieGrid[thePosX, thePosY] = true;
			}
			if (Is2x2Zombie(theZombieType))
			{
				Debug.ASSERT(thePosX > 0 && thePosY > 0);
				theZombieGrid[thePosX - 1, thePosY] = true;
				theZombieGrid[thePosX, thePosY - 1] = true;
				theZombieGrid[thePosX - 1, thePosY - 1] = true;
			}
			PlaceAZombie(theZombieType, thePosX, thePosY);
			if (theZombieType == ZombieType.ZOMBIE_BUNGEE && mApp.IsBungeeBlitzLevel())
			{
				PlaceAZombie(ZombieType.ZOMBIE_BUNGEE, 1, thePosY);
				PlaceAZombie(ZombieType.ZOMBIE_BUNGEE, 2, thePosY);
			}
		}

		public bool Is2x2Zombie(ZombieType theZombieType)
		{
			if (theZombieType == ZombieType.ZOMBIE_GARGANTUAR || theZombieType == ZombieType.ZOMBIE_REDEYE_GARGANTUAR)
			{
				return true;
			}
			return false;
		}

		public void PreloadResources()
		{
			if (mPreloaded)
			{
				return;
			}
			mPreloaded = true;
			PerfTimer perfTimer = default(PerfTimer);
			perfTimer.Start();
			for (int i = 0; i < mBoard.mNumWaves; i++)
			{
				for (int j = 0; j < 50; j++)
				{
					ZombieType zombieType = mBoard.mZombiesInWave[i, j];
					if (zombieType == ZombieType.ZOMBIE_INVALID)
					{
						break;
					}
					Zombie.PreloadZombieResources(zombieType);
				}
			}
			for (int k = 0; k < 53; k++)
			{
				SeedType theSeedType = (SeedType)k;
				if (mApp.HasSeedType(theSeedType))
				{
					Plant.PreloadPlantResources(theSeedType);
				}
			}
			if (mApp.IsFirstTimeAdventureMode() && mBoard.mLevel <= 50)
			{
				SeedType awardSeedForLevel = mApp.GetAwardSeedForLevel(mBoard.mLevel);
				Plant.PreloadPlantResources(awardSeedForLevel);
			}
			if (mCrazyDaveDialogStart != -1)
			{
				ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_CRAZY_DAVE, true);
			}
			if (mApp.mPlayerInfo.mPurchases[24] != 0)
			{
				ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_RAKE, true);
			}
			if (mApp.mGameMode == GameMode.GAMEMODE_CHALLENGE_ZEN_GARDEN)
			{
				Plant.PreloadPlantResources(SeedType.SEED_SPROUT);
				Plant.PreloadPlantResources(SeedType.SEED_MARIGOLD);
			}
			if (mBoard.StageHasRoof())
			{
				ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_ROOF_CLEANER, true);
			}
			else
			{
				ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_LAWNMOWER, true);
			}
			if (mBoard.StageHasPool())
			{
				ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_SPLASH, true);
				ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_POOL_CLEANER, true);
			}
			if (mBoard.CanDropLoot())
			{
				ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_COIN_SILVER, true);
				ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_COIN_GOLD, true);
				ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_DIAMOND, true);
			}
			if (mSodTime > 0)
			{
				ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_SODROLL, true);
			}
			if (mApp.mGameMode == GameMode.GAMEMODE_CHALLENGE_PORTAL_COMBAT)
			{
				ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_PORTAL_CIRCLE, true);
				ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_PORTAL_SQUARE, true);
			}
			if (mApp.IsWhackAZombieLevel() || mApp.IsScaryPotterLevel())
			{
				ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_HAMMER, true);
			}
			if (mApp.IsStormyNightLevel() || mApp.mGameMode == GameMode.GAMEMODE_CHALLENGE_RAINING_SEEDS)
			{
				ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_RAIN_CIRCLE, true);
				ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_RAIN_SPLASH, true);
			}
			if (mApp.mGameMode == GameMode.GAMEMODE_INTRO)
			{
				mApp.DelayLoadBackgroundResource("DelayLoad_Background3");
				Zombie.PreloadZombieResources(ZombieType.ZOMBIE_NORMAL);
				Zombie.PreloadZombieResources(ZombieType.ZOMBIE_TRAFFIC_CONE);
				Zombie.PreloadZombieResources(ZombieType.ZOMBIE_PAIL);
				Zombie.PreloadZombieResources(ZombieType.ZOMBIE_ZAMBONI);
				Plant.PreloadPlantResources(SeedType.SEED_SUNFLOWER);
				Plant.PreloadPlantResources(SeedType.SEED_PEASHOOTER);
				Plant.PreloadPlantResources(SeedType.SEED_SQUASH);
				Plant.PreloadPlantResources(SeedType.SEED_THREEPEATER);
				Plant.PreloadPlantResources(SeedType.SEED_LILYPAD);
				Plant.PreloadPlantResources(SeedType.SEED_TORCHWOOD);
				Plant.PreloadPlantResources(SeedType.SEED_SPIKEWEED);
				Plant.PreloadPlantResources(SeedType.SEED_TANGLEKELP);
			}
			PlaceStreetZombies();
			mBoard.mPreloadTime = Math.Max((int)perfTimer.GetDuration(), 0);
		}

		public bool IsBeforePreloading()
		{
			if (mApp.mGameScene != GameScenes.SCENE_LEVEL_INTRO)
			{
				return false;
			}
			return !mPreloaded;
		}

		public bool IsShowingCrazyDave()
		{
			if (mApp.mGameScene != GameScenes.SCENE_LEVEL_INTRO)
			{
				return false;
			}
			if (mCrazyDaveTime > 0 && mCutsceneTime < TimePanRightEnd + mCrazyDaveTime)
			{
				return true;
			}
			return false;
		}

		public bool IsNonScrollingCutscene()
		{
			if (mApp.mGameMode == GameMode.GAMEMODE_CHALLENGE_ICE || mApp.mGameMode == GameMode.GAMEMODE_UPSELL || mApp.IsScaryPotterLevel() || mApp.IsIZombieLevel() || mApp.IsWhackAZombieLevel() || mApp.IsShovelLevel() || mApp.IsSquirrelLevel() || mApp.IsWallnutBowlingLevel() || mApp.mGameMode == GameMode.GAMEMODE_CHALLENGE_ZEN_GARDEN || mApp.mGameMode == GameMode.GAMEMODE_TREE_OF_WISDOM || mApp.mGameMode == GameMode.GAMEMODE_CHALLENGE_ZOMBIQUARIUM)
			{
				return true;
			}
			return false;
		}

		public bool IsScrolledLeftAtStart()
		{
			if (mBoard.mChallenge.mSurvivalStage > 0 && mApp.IsSurvivalMode())
			{
				return false;
			}
			if (mApp.IsShovelLevel() || mApp.IsSquirrelLevel() || mApp.IsWallnutBowlingLevel() || IsNonScrollingCutscene())
			{
				return false;
			}
			return true;
		}

		public bool IsInShovelTutorial()
		{
			if (mBoard.mTutorialState == TutorialState.TUTORIAL_SHOVEL_PICKUP || mBoard.mTutorialState == TutorialState.TUTORIAL_SHOVEL_DIG || mBoard.mTutorialState == TutorialState.TUTORIAL_SHOVEL_KEEP_DIGGING)
			{
				return true;
			}
			return false;
		}

		public void PlaceLawnItems()
		{
			if (!mPlacedLawnItems)
			{
				mPlacedLawnItems = true;
				if (!IsSurvivalRepick())
				{
					mBoard.InitLawnMowers();
					AddFlowerPots();
				}
				if (!IsSurvivalRepick())
				{
					mBoard.PlaceRake();
				}
			}
		}

		public bool CanGetSecondPacketUpgrade()
		{
			int itemCost = StoreScreen.GetItemCost(StoreItem.STORE_ITEM_PACKET_UPGRADE);
			if (mApp.mPlayerInfo.mPurchases[21] == 1 && mApp.mPlayerInfo.mCoins >= itemCost && mApp.mPlayerInfo.mDidntPurchasePacketUpgrade < 2)
			{
				return true;
			}
			return false;
		}

		public int ParseTalkTimeFromMessage()
		{
			string crazyDaveText = mApp.GetCrazyDaveText(mCrazyDaveLastTalkIndex);
			int num = crazyDaveText.IndexOf("{TIME_");
			if (num != -1)
			{
				int num2 = crazyDaveText.IndexOf("}", num);
				string s = crazyDaveText.Substring(num + 6, num2 - num - 6);
				mCrazyDaveCountDown = int.Parse(s);
				return mCrazyDaveCountDown;
			}
			return 100;
		}

		public int ParseDelayTimeFromMessage()
		{
			string crazyDaveText = mApp.GetCrazyDaveText(mCrazyDaveLastTalkIndex);
			int num = crazyDaveText.IndexOf("{DELAY_");
			if (num != -1)
			{
				int num2 = crazyDaveText.IndexOf("}", num);
				string s = crazyDaveText.Substring(num + 7, num2 - num - 7);
				mCrazyDaveCountDown = int.Parse(s);
				return mCrazyDaveCountDown;
			}
			return 100;
		}

		public void UpdateUpsell()
		{
			if (!mBoard.mMenuButton.mIsOver)
			{
				bool mIsOver = mBoard.mStoreButton.mIsOver;
			}
			if (mApp.mCrazyDaveState == CrazyDaveState.CRAZY_DAVE_OFF || mApp.mCrazyDaveState == CrazyDaveState.CRAZY_DAVE_ENTERING)
			{
				return;
			}
			if (mCrazyDaveLastTalkIndex == -1)
			{
				mApp.CrazyDaveTalkIndex(mCrazyDaveDialogStart);
				mCrazyDaveLastTalkIndex = mCrazyDaveDialogStart;
				mCrazyDaveCountDown = ParseTalkTimeFromMessage();
				return;
			}
			if (mCrazyDaveCountDown > 0)
			{
				mCrazyDaveCountDown--;
			}
			if (mCrazyDaveLastTalkIndex == 3317)
			{
				if (mCrazyDaveCountDown == 0)
				{
					mBoard.mStoreButton.Resize(450, 420, 260, 46);
					mBoard.mMenuButton.mBtnNoDraw = false;
					mBoard.mStoreButton.mBtnNoDraw = false;
				}
				return;
			}
			if (mCrazyDaveLastTalkIndex == 3311 && mCrazyDaveCountDown == 90)
			{
				mApp.mMusic.MakeSureMusicIsPlaying(MusicTune.MUSIC_TUNE_MINIGAME_LOONBOON);
			}
			if (mCrazyDaveCountDown != 0)
			{
				return;
			}
			if (mApp.mCrazyDaveMessageIndex != -1)
			{
				mCrazyDaveCountDown = ParseDelayTimeFromMessage();
				mApp.CrazyDaveStopTalking();
				return;
			}
			int theMessageIndex = mCrazyDaveLastTalkIndex + 1;
			mApp.CrazyDaveTalkIndex(theMessageIndex);
			mCrazyDaveLastTalkIndex = theMessageIndex;
			mCrazyDaveCountDown = ParseTalkTimeFromMessage();
			if (mCrazyDaveLastTalkIndex == 3305)
			{
				Reanimation reanimation = mApp.ReanimationGet(mApp.mCrazyDaveReanimID);
				Reanimation reanimation2 = mApp.AddReanimation(0f, 0f, 0, ReanimationType.REANIM_SQUASH);
				reanimation2.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_anim_idle, ReanimLoopType.REANIM_LOOP, 0, 15f);
				ReanimatorTrackInstance trackInstanceByName = reanimation.GetTrackInstanceByName(GlobalMembersReanimIds.ReanimTrackId_dave_handinghand);
				AttachEffect attachEffect = GlobalMembersAttachment.AttachReanim(ref trackInstanceByName.mAttachmentID, reanimation2, Constants.S * 92f, Constants.S * 387f);
				attachEffect.mOffset.mMatrix.M11 = 1.2f;
				attachEffect.mOffset.mMatrix.M22 = 1.2f;
				reanimation.Update();
			}
			if (mCrazyDaveLastTalkIndex == 3306)
			{
				Reanimation reanimation3 = mApp.ReanimationGet(mApp.mCrazyDaveReanimID);
				Reanimation theAttachReanim = mApp.AddReanimation(0f, 0f, 0, ReanimationType.REANIM_THREEPEATER);
				theAttachReanim.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_anim_idle, ReanimLoopType.REANIM_LOOP, 0, 15f);
				Reanimation reanimation4 = mApp.AddReanimation(0f, 0f, 0, ReanimationType.REANIM_THREEPEATER);
				reanimation4.mLoopType = ReanimLoopType.REANIM_LOOP;
				reanimation4.mAnimRate = theAttachReanim.mAnimRate;
				reanimation4.SetFramesForLayer("anim_head_idle1");
				reanimation4.AttachToAnotherReanimation(ref theAttachReanim, "anim_head1");
				Reanimation reanimation5 = mApp.AddReanimation(0f, 0f, 0, ReanimationType.REANIM_THREEPEATER);
				reanimation5.mLoopType = ReanimLoopType.REANIM_LOOP;
				reanimation5.mAnimRate = theAttachReanim.mAnimRate;
				reanimation5.SetFramesForLayer("anim_head_idle2");
				reanimation5.AttachToAnotherReanimation(ref theAttachReanim, "anim_head2");
				Reanimation reanimation6 = mApp.AddReanimation(0f, 0f, 0, ReanimationType.REANIM_THREEPEATER);
				reanimation6.mLoopType = ReanimLoopType.REANIM_LOOP;
				reanimation6.mAnimRate = theAttachReanim.mAnimRate;
				reanimation6.SetFramesForLayer("anim_head_idle3");
				reanimation6.AttachToAnotherReanimation(ref theAttachReanim, "anim_head3");
				ReanimatorTrackInstance trackInstanceByName2 = reanimation3.GetTrackInstanceByName("Dave_body1");
				AttachEffect attachEffect2 = GlobalMembersAttachment.AttachReanim(ref trackInstanceByName2.mAttachmentID, theAttachReanim, 0f, 0f);
				TodCommon.TodScaleRotateTransformMatrix(ref attachEffect2.mOffset.mMatrix, -50f, 230f, 0.5f, 1.2f, 1.2f);
				reanimation3.Update();
				theAttachReanim.Update();
			}
			if (mCrazyDaveLastTalkIndex == 3307)
			{
				Reanimation reanimation7 = mApp.ReanimationGet(mApp.mCrazyDaveReanimID);
				Reanimation reanimation8 = mApp.AddReanimation(0f, 0f, 0, ReanimationType.REANIM_MAGNETSHROOM);
				reanimation8.PlayReanim("anim_idle", ReanimLoopType.REANIM_LOOP, 0, 15f);
				TodCommon.TodScaleRotateTransformMatrix(ref reanimation8.mOverlayMatrix.mMatrix, 0f, 0f, 0.3f, 1f, 1f);
				ReanimatorTrackInstance trackInstanceByName3 = reanimation7.GetTrackInstanceByName("Dave_pot");
				AttachEffect attachEffect3 = GlobalMembersAttachment.AttachReanim(ref trackInstanceByName3.mAttachmentID, reanimation8, Constants.S * 25f, Constants.S * 49f);
				attachEffect3.mOffset.mMatrix.M11 = 1.2f;
				attachEffect3.mOffset.mMatrix.M22 = 1.2f;
				reanimation7.Update();
			}
			if (mCrazyDaveLastTalkIndex == 3309)
			{
				Reanimation reanimation9 = mApp.ReanimationGet(mApp.mCrazyDaveReanimID);
				Reanimation reanimation10 = reanimation9.FindSubReanim(ReanimationType.REANIM_THREEPEATER);
				reanimation10.ReanimationDie();
				Reanimation reanimation11 = reanimation9.FindSubReanim(ReanimationType.REANIM_MAGNETSHROOM);
				reanimation11.ReanimationDie();
			}
			if (mCrazyDaveLastTalkIndex == 3312)
			{
				mApp.mMusic.MakeSureMusicIsPlaying(MusicTune.MUSIC_TUNE_MINIGAME_LOONBOON);
				LoadUpsellBoardPool();
				mApp.PlaySample(Resources.SOUND_FINALWAVE);
				mUpsellHideBoard = false;
			}
			if (mCrazyDaveLastTalkIndex == 3313)
			{
				LoadUpsellBoardFog();
				mApp.PlaySample(Resources.SOUND_HUGE_WAVE);
				mUpsellHideBoard = false;
			}
			if (mCrazyDaveLastTalkIndex == 3314)
			{
				LoadUpsellChallengeScreen();
				mApp.PlaySample(Resources.SOUND_FINALWAVE);
				mUpsellHideBoard = false;
			}
			if (mCrazyDaveLastTalkIndex == 3315)
			{
				ClearUpsellBoard();
				mApp.PlaySample(Resources.SOUND_FINALWAVE);
				mUpsellHideBoard = true;
				mApp.AddTodParticle(Constants.CutScene_Upsell_TerraCotta_Arrow.X, Constants.CutScene_Upsell_TerraCotta_Arrow.Y, 900000, ParticleEffect.PARTICLE_UPSELL_ARROW);
			}
			if (mCrazyDaveLastTalkIndex == 3316)
			{
				LoadUpsellBoardRoof();
				mApp.PlaySample(Resources.SOUND_HUGE_WAVE);
				mUpsellHideBoard = false;
			}
			if (mCrazyDaveLastTalkIndex == 3317)
			{
				ClearUpsellBoard();
				mBoard.mMenuButton.mBtnNoDraw = true;
				mUpsellHideBoard = true;
			}
		}

		public void ClearUpsellBoard()
		{
			for (int i = 0; i < Constants.MAX_GRIDSIZEY; i++)
			{
				mBoard.mIceTimer[i] = 0;
				mBoard.mIceMinX[i] = Constants.BOARD_WIDTH;
			}
			mBoard.mZombiesRow1.Clear();
			mBoard.mZombiesRow2.Clear();
			mBoard.mZombiesRow3.Clear();
			mBoard.mZombiesRow4.Clear();
			mBoard.mZombiesRow5.Clear();
			mBoard.mZombiesRow6.Clear();
			for (int j = 0; j < mBoard.mZombies.Count; j++)
			{
				mBoard.mZombies[j].PrepareForReuse();
			}
			mBoard.mZombies.Clear();
			for (int k = 0; k < mBoard.mPlants.Count; k++)
			{
				mBoard.mPlants[k].PrepareForReuse();
			}
			mBoard.mPlants.Clear();
			for (int l = 0; l < mBoard.mCoins.Count; l++)
			{
				mBoard.mCoins[l].PrepareForReuse();
			}
			mBoard.mCoins.Clear();
			for (int m = 0; m < mBoard.mProjectiles.Count; m++)
			{
				mBoard.mProjectiles[m].PrepareForReuse();
			}
			mBoard.mProjectiles.Clear();
			for (int n = 0; n < mBoard.mGridItems.Count; n++)
			{
				mBoard.mGridItems[n].PrepareForReuse();
			}
			mBoard.mGridItems.Clear();
			for (int num = 0; num < mBoard.mLawnMowers.Count; num++)
			{
				mBoard.mLawnMowers[num].PrepareForReuse();
			}
			mBoard.mLawnMowers.Clear();
			int index = -1;
			TodParticleSystem theParticle = null;
			while (mBoard.IterateParticles(ref theParticle, ref index))
			{
				theParticle.ParticleSystemDie();
			}
			int index2 = -1;
			Reanimation theReanimation = null;
			while (mBoard.IterateReanimations(ref theReanimation, ref index2))
			{
				if (theReanimation.mReanimationType != ReanimationType.REANIM_CRAZY_DAVE)
				{
					theReanimation.ReanimationDie();
				}
			}
			mBoard.mPoolSparklyParticleID = null;
			if (mUpsellChallengeScreen != null)
			{
				mUpsellChallengeScreen.Dispose();
				mUpsellChallengeScreen = null;
			}
		}

		public void AddUpsellZombie(ZombieType theZombieType, int thePixelX, int theGridY)
		{
			Zombie zombie = mBoard.AddZombieInRow(theZombieType, theGridY, 0);
			zombie.mPosX = thePixelX;
			zombie.mPosY = zombie.GetPosYBasedOnRow(theGridY);
			zombie.SetRow(theGridY);
			zombie.mX = (int)zombie.mPosX;
			zombie.mY = (int)zombie.mPosY;
			if (mBoard.StageHasPool() && (theGridY == 2 || theGridY == 3))
			{
				zombie.mUsesClipping = true;
			}
		}

		public void LoadIntroBoard()
		{
			ClearUpsellBoard();
			mApp.mMuteSoundsForCutscene = true;
			mBoard.NewPlant(0, 1, SeedType.SEED_THREEPEATER, SeedType.SEED_NONE);
			mBoard.NewPlant(0, 2, SeedType.SEED_LILYPAD, SeedType.SEED_NONE);
			mBoard.NewPlant(0, 2, SeedType.SEED_PEASHOOTER, SeedType.SEED_NONE);
			mBoard.NewPlant(0, 3, SeedType.SEED_LILYPAD, SeedType.SEED_NONE);
			mBoard.NewPlant(0, 3, SeedType.SEED_PEASHOOTER, SeedType.SEED_NONE);
			mBoard.NewPlant(0, 4, SeedType.SEED_SUNFLOWER, SeedType.SEED_NONE);
			mBoard.NewPlant(1, 0, SeedType.SEED_THREEPEATER, SeedType.SEED_NONE);
			mBoard.NewPlant(1, 1, SeedType.SEED_SUNFLOWER, SeedType.SEED_NONE);
			mBoard.NewPlant(1, 2, SeedType.SEED_LILYPAD, SeedType.SEED_NONE);
			mBoard.NewPlant(1, 2, SeedType.SEED_SUNFLOWER, SeedType.SEED_NONE);
			mBoard.NewPlant(1, 4, SeedType.SEED_THREEPEATER, SeedType.SEED_NONE);
			mBoard.NewPlant(1, 5, SeedType.SEED_THREEPEATER, SeedType.SEED_NONE);
			mBoard.NewPlant(2, 0, SeedType.SEED_SUNFLOWER, SeedType.SEED_NONE);
			mBoard.NewPlant(2, 1, SeedType.SEED_PEASHOOTER, SeedType.SEED_NONE);
			mBoard.NewPlant(2, 3, SeedType.SEED_LILYPAD, SeedType.SEED_NONE);
			mBoard.NewPlant(2, 3, SeedType.SEED_PEASHOOTER, SeedType.SEED_NONE);
			mBoard.NewPlant(2, 4, SeedType.SEED_SUNFLOWER, SeedType.SEED_NONE);
			mBoard.NewPlant(2, 5, SeedType.SEED_SUNFLOWER, SeedType.SEED_NONE);
			mBoard.NewPlant(3, 0, SeedType.SEED_TORCHWOOD, SeedType.SEED_NONE);
			mBoard.NewPlant(3, 4, SeedType.SEED_THREEPEATER, SeedType.SEED_NONE);
			mBoard.NewPlant(4, 2, SeedType.SEED_LILYPAD, SeedType.SEED_NONE);
			mBoard.NewPlant(4, 2, SeedType.SEED_TORCHWOOD, SeedType.SEED_NONE);
			mBoard.NewPlant(5, 1, SeedType.SEED_TORCHWOOD, SeedType.SEED_NONE);
			mBoard.NewPlant(5, 4, SeedType.SEED_TORCHWOOD, SeedType.SEED_NONE);
			mBoard.NewPlant(5, 5, SeedType.SEED_TORCHWOOD, SeedType.SEED_NONE);
			mBoard.NewPlant(6, 0, SeedType.SEED_SPIKEWEED, SeedType.SEED_NONE);
			mBoard.NewPlant(6, 4, SeedType.SEED_SPIKEWEED, SeedType.SEED_NONE);
			mBoard.NewPlant(7, 1, SeedType.SEED_SPIKEWEED, SeedType.SEED_NONE);
			AddUpsellZombie(ZombieType.ZOMBIE_NORMAL, 460, 0);
			AddUpsellZombie(ZombieType.ZOMBIE_FOOTBALL, 680, 0);
			AddUpsellZombie(ZombieType.ZOMBIE_TRAFFIC_CONE, 730, 0);
			AddUpsellZombie(ZombieType.ZOMBIE_NORMAL, 810, 0);
			AddUpsellZombie(ZombieType.ZOMBIE_TRAFFIC_CONE, 670, 1);
			AddUpsellZombie(ZombieType.ZOMBIE_NORMAL, 740, 1);
			AddUpsellZombie(ZombieType.ZOMBIE_NORMAL, 880, 1);
			AddUpsellZombie(ZombieType.ZOMBIE_NORMAL, 500, 2);
			AddUpsellZombie(ZombieType.ZOMBIE_TRAFFIC_CONE, 680, 2);
			AddUpsellZombie(ZombieType.ZOMBIE_PAIL, 604, 3);
			AddUpsellZombie(ZombieType.ZOMBIE_SNORKEL, 880, 3);
			AddUpsellZombie(ZombieType.ZOMBIE_NORMAL, 600, 4);
			AddUpsellZombie(ZombieType.ZOMBIE_PAIL, 690, 4);
			AddUpsellZombie(ZombieType.ZOMBIE_NORMAL, 780, 4);
			AddUpsellZombie(ZombieType.ZOMBIE_CATAPULT, 730, 5);
			AddUpsellZombie(ZombieType.ZOMBIE_NORMAL, 590, 5);
			mPreUpdatingBoard = true;
			for (int i = 0; i < 100; i++)
			{
				mBoard.Update();
			}
			mPreUpdatingBoard = false;
		}

		public void LoadUpsellBoardPool()
		{
			ClearUpsellBoard();
			mApp.mMuteSoundsForCutscene = true;
			mBoard.NewPlant(0, 1, SeedType.SEED_THREEPEATER, SeedType.SEED_NONE);
			mBoard.NewPlant(0, 2, SeedType.SEED_LILYPAD, SeedType.SEED_NONE);
			mBoard.NewPlant(0, 2, SeedType.SEED_PEASHOOTER, SeedType.SEED_NONE);
			mBoard.NewPlant(0, 3, SeedType.SEED_LILYPAD, SeedType.SEED_NONE);
			mBoard.NewPlant(0, 3, SeedType.SEED_PEASHOOTER, SeedType.SEED_NONE);
			mBoard.NewPlant(0, 4, SeedType.SEED_SUNFLOWER, SeedType.SEED_NONE);
			mBoard.NewPlant(1, 0, SeedType.SEED_THREEPEATER, SeedType.SEED_NONE);
			mBoard.NewPlant(1, 1, SeedType.SEED_SUNFLOWER, SeedType.SEED_NONE);
			mBoard.NewPlant(1, 2, SeedType.SEED_LILYPAD, SeedType.SEED_NONE);
			mBoard.NewPlant(1, 2, SeedType.SEED_SUNFLOWER, SeedType.SEED_NONE);
			mBoard.NewPlant(1, 4, SeedType.SEED_THREEPEATER, SeedType.SEED_NONE);
			mBoard.NewPlant(1, 5, SeedType.SEED_THREEPEATER, SeedType.SEED_NONE);
			mBoard.NewPlant(2, 0, SeedType.SEED_SUNFLOWER, SeedType.SEED_NONE);
			mBoard.NewPlant(2, 1, SeedType.SEED_PEASHOOTER, SeedType.SEED_NONE);
			mBoard.NewPlant(2, 3, SeedType.SEED_LILYPAD, SeedType.SEED_NONE);
			mBoard.NewPlant(2, 3, SeedType.SEED_PEASHOOTER, SeedType.SEED_NONE);
			mBoard.NewPlant(2, 4, SeedType.SEED_SUNFLOWER, SeedType.SEED_NONE);
			mBoard.NewPlant(2, 5, SeedType.SEED_SUNFLOWER, SeedType.SEED_NONE);
			mBoard.NewPlant(3, 4, SeedType.SEED_THREEPEATER, SeedType.SEED_NONE);
			mBoard.NewPlant(4, 0, SeedType.SEED_TORCHWOOD, SeedType.SEED_NONE);
			mBoard.NewPlant(4, 2, SeedType.SEED_LILYPAD, SeedType.SEED_NONE);
			mBoard.NewPlant(4, 2, SeedType.SEED_TORCHWOOD, SeedType.SEED_NONE);
			mBoard.NewPlant(5, 1, SeedType.SEED_TORCHWOOD, SeedType.SEED_NONE);
			mBoard.NewPlant(5, 4, SeedType.SEED_TORCHWOOD, SeedType.SEED_NONE);
			mBoard.NewPlant(5, 5, SeedType.SEED_TORCHWOOD, SeedType.SEED_NONE);
			mBoard.NewPlant(6, 0, SeedType.SEED_SPIKEWEED, SeedType.SEED_NONE);
			mBoard.NewPlant(6, 3, SeedType.SEED_TANGLEKELP, SeedType.SEED_NONE);
			mBoard.NewPlant(6, 4, SeedType.SEED_SPIKEWEED, SeedType.SEED_NONE);
			mBoard.NewPlant(6, 5, SeedType.SEED_SQUASH, SeedType.SEED_NONE);
			mBoard.NewPlant(7, 1, SeedType.SEED_SPIKEWEED, SeedType.SEED_NONE);
			AddUpsellZombie(ZombieType.ZOMBIE_NORMAL, 460, 0);
			AddUpsellZombie(ZombieType.ZOMBIE_ZAMBONI, 680, 0);
			AddUpsellZombie(ZombieType.ZOMBIE_TRAFFIC_CONE, 670, 1);
			AddUpsellZombie(ZombieType.ZOMBIE_NORMAL, 740, 1);
			AddUpsellZombie(ZombieType.ZOMBIE_NORMAL, 500, 2);
			AddUpsellZombie(ZombieType.ZOMBIE_TRAFFIC_CONE, 680, 2);
			AddUpsellZombie(ZombieType.ZOMBIE_NORMAL, 604, 3);
			AddUpsellZombie(ZombieType.ZOMBIE_NORMAL, 690, 4);
			AddUpsellZombie(ZombieType.ZOMBIE_NORMAL, 740, 4);
			AddUpsellZombie(ZombieType.ZOMBIE_PAIL, 730, 5);
			AddUpsellZombie(ZombieType.ZOMBIE_NORMAL, 590, 5);
			mPreUpdatingBoard = true;
			for (int i = 0; i < 100; i++)
			{
				mBoard.Update();
			}
			mPreUpdatingBoard = false;
			mApp.mMuteSoundsForCutscene = false;
		}

		public void LoadUpsellBoardFog()
		{
			ClearUpsellBoard();
			mApp.mMuteSoundsForCutscene = true;
			mBoard.mBackground = BackgroundType.BACKGROUND_4_FOG;
			mBoard.LoadBackgroundImages();
			mBoard.NewPlant(0, 1, SeedType.SEED_SUNSHROOM, SeedType.SEED_NONE);
			mBoard.NewPlant(0, 4, SeedType.SEED_SUNSHROOM, SeedType.SEED_NONE);
			mBoard.NewPlant(1, 0, SeedType.SEED_SUNSHROOM, SeedType.SEED_NONE);
			mBoard.NewPlant(1, 1, SeedType.SEED_SUNSHROOM, SeedType.SEED_NONE);
			mBoard.NewPlant(1, 2, SeedType.SEED_LILYPAD, SeedType.SEED_NONE);
			mBoard.NewPlant(1, 2, SeedType.SEED_CACTUS, SeedType.SEED_NONE);
			mBoard.NewPlant(1, 4, SeedType.SEED_SUNSHROOM, SeedType.SEED_NONE);
			mBoard.NewPlant(1, 5, SeedType.SEED_SUNSHROOM, SeedType.SEED_NONE);
			mBoard.NewPlant(2, 0, SeedType.SEED_CACTUS, SeedType.SEED_NONE);
			mBoard.NewPlant(2, 4, SeedType.SEED_CACTUS, SeedType.SEED_NONE);
			mBoard.NewPlant(2, 5, SeedType.SEED_FUMESHROOM, SeedType.SEED_NONE);
			mBoard.NewPlant(3, 1, SeedType.SEED_FUMESHROOM, SeedType.SEED_NONE);
			mBoard.NewPlant(3, 2, SeedType.SEED_LILYPAD, SeedType.SEED_NONE);
			mBoard.NewPlant(3, 3, SeedType.SEED_LILYPAD, SeedType.SEED_NONE);
			mBoard.NewPlant(3, 3, SeedType.SEED_CACTUS, SeedType.SEED_NONE);
			mBoard.NewPlant(3, 5, SeedType.SEED_PUFFSHROOM, SeedType.SEED_NONE);
			mBoard.NewPlant(4, 0, SeedType.SEED_PUFFSHROOM, SeedType.SEED_NONE);
			mBoard.NewPlant(4, 1, SeedType.SEED_MAGNETSHROOM, SeedType.SEED_NONE);
			mBoard.NewPlant(4, 2, SeedType.SEED_SEASHROOM, SeedType.SEED_NONE);
			mBoard.NewPlant(4, 5, SeedType.SEED_PUFFSHROOM, SeedType.SEED_NONE);
			mBoard.NewPlant(5, 1, SeedType.SEED_PUFFSHROOM, SeedType.SEED_NONE);
			mBoard.NewPlant(5, 2, SeedType.SEED_LILYPAD, SeedType.SEED_NONE);
			mBoard.NewPlant(5, 2, SeedType.SEED_PLANTERN, SeedType.SEED_NONE);
			mBoard.NewPlant(5, 3, SeedType.SEED_SEASHROOM, SeedType.SEED_NONE);
			mBoard.NewPlant(6, 2, SeedType.SEED_SEASHROOM, SeedType.SEED_NONE);
			mBoard.NewPlant(6, 3, SeedType.SEED_SEASHROOM, SeedType.SEED_NONE);
			AddUpsellZombie(ZombieType.ZOMBIE_NORMAL, 460, 0);
			AddUpsellZombie(ZombieType.ZOMBIE_NORMAL, 680, 0);
			AddUpsellZombie(ZombieType.ZOMBIE_BALLOON, 780, 0);
			AddUpsellZombie(ZombieType.ZOMBIE_TRAFFIC_CONE, 670, 1);
			AddUpsellZombie(ZombieType.ZOMBIE_BALLOON, 640, 1);
			AddUpsellZombie(ZombieType.ZOMBIE_PAIL, 640, 2);
			AddUpsellZombie(ZombieType.ZOMBIE_TRAFFIC_CONE, 780, 3);
			AddUpsellZombie(ZombieType.ZOMBIE_BALLOON, 704, 4);
			AddUpsellZombie(ZombieType.ZOMBIE_NORMAL, 690, 4);
			AddUpsellZombie(ZombieType.ZOMBIE_PAIL, 590, 5);
			AddUpsellZombie(ZombieType.ZOMBIE_NORMAL, 740, 5);
			mPreUpdatingBoard = true;
			for (int i = 0; i < 100; i++)
			{
				mBoard.Update();
			}
			mPreUpdatingBoard = false;
			mApp.mMuteSoundsForCutscene = false;
		}

		public void LoadUpsellChallengeScreen()
		{
			ClearUpsellBoard();
		}

		public void LoadUpsellBoardRoof()
		{
			ClearUpsellBoard();
			mApp.mMuteSoundsForCutscene = true;
			mBoard.mBackground = BackgroundType.BACKGROUND_5_ROOF;
			mBoard.LoadBackgroundImages();
			mBoard.mPlantRow[0] = PlantRowType.PLANTROW_NORMAL;
			mBoard.mPlantRow[1] = PlantRowType.PLANTROW_NORMAL;
			mBoard.mPlantRow[2] = PlantRowType.PLANTROW_NORMAL;
			mBoard.mPlantRow[3] = PlantRowType.PLANTROW_NORMAL;
			mBoard.mPlantRow[4] = PlantRowType.PLANTROW_NORMAL;
			mBoard.mPlantRow[5] = PlantRowType.PLANTROW_DIRT;
			for (int i = 0; i < Constants.GRIDSIZEX; i++)
			{
				for (int j = 0; j < Constants.MAX_GRIDSIZEY; j++)
				{
					if (mBoard.mPlantRow[j] == PlantRowType.PLANTROW_DIRT)
					{
						mBoard.mGridSquareType[i, j] = GridSquareType.GRIDSQUARE_DIRT;
					}
					else
					{
						mBoard.mGridSquareType[i, j] = GridSquareType.GRIDSQUARE_GRASS;
					}
				}
			}
			mBoard.NewPlant(0, 0, SeedType.SEED_FLOWERPOT, SeedType.SEED_NONE);
			mBoard.NewPlant(0, 0, SeedType.SEED_CABBAGEPULT, SeedType.SEED_NONE);
			mBoard.NewPlant(0, 1, SeedType.SEED_FLOWERPOT, SeedType.SEED_NONE);
			mBoard.NewPlant(0, 1, SeedType.SEED_CABBAGEPULT, SeedType.SEED_NONE);
			mBoard.NewPlant(0, 2, SeedType.SEED_FLOWERPOT, SeedType.SEED_NONE);
			mBoard.NewPlant(0, 2, SeedType.SEED_SUNFLOWER, SeedType.SEED_NONE);
			mBoard.NewPlant(0, 3, SeedType.SEED_FLOWERPOT, SeedType.SEED_NONE);
			mBoard.NewPlant(0, 3, SeedType.SEED_SUNFLOWER, SeedType.SEED_NONE);
			mBoard.NewPlant(0, 4, SeedType.SEED_FLOWERPOT, SeedType.SEED_NONE);
			mBoard.NewPlant(0, 4, SeedType.SEED_CABBAGEPULT, SeedType.SEED_NONE);
			mBoard.NewPlant(1, 0, SeedType.SEED_FLOWERPOT, SeedType.SEED_NONE);
			mBoard.NewPlant(1, 0, SeedType.SEED_CABBAGEPULT, SeedType.SEED_NONE);
			mBoard.NewPlant(1, 1, SeedType.SEED_FLOWERPOT, SeedType.SEED_NONE);
			mBoard.NewPlant(1, 1, SeedType.SEED_SUNFLOWER, SeedType.SEED_NONE);
			mBoard.NewPlant(1, 2, SeedType.SEED_FLOWERPOT, SeedType.SEED_NONE);
			mBoard.NewPlant(1, 2, SeedType.SEED_CABBAGEPULT, SeedType.SEED_NONE);
			mBoard.NewPlant(1, 3, SeedType.SEED_FLOWERPOT, SeedType.SEED_NONE);
			mBoard.NewPlant(1, 3, SeedType.SEED_CABBAGEPULT, SeedType.SEED_NONE);
			mBoard.NewPlant(1, 4, SeedType.SEED_FLOWERPOT, SeedType.SEED_NONE);
			mBoard.NewPlant(1, 4, SeedType.SEED_SUNFLOWER, SeedType.SEED_NONE);
			mBoard.NewPlant(2, 0, SeedType.SEED_FLOWERPOT, SeedType.SEED_NONE);
			mBoard.NewPlant(2, 0, SeedType.SEED_CABBAGEPULT, SeedType.SEED_NONE);
			mBoard.NewPlant(2, 1, SeedType.SEED_FLOWERPOT, SeedType.SEED_NONE);
			mBoard.NewPlant(2, 1, SeedType.SEED_CABBAGEPULT, SeedType.SEED_NONE);
			mBoard.NewPlant(2, 2, SeedType.SEED_FLOWERPOT, SeedType.SEED_NONE);
			mBoard.NewPlant(2, 2, SeedType.SEED_CABBAGEPULT, SeedType.SEED_NONE);
			mBoard.NewPlant(2, 3, SeedType.SEED_FLOWERPOT, SeedType.SEED_NONE);
			mBoard.NewPlant(2, 3, SeedType.SEED_SUNFLOWER, SeedType.SEED_NONE);
			mBoard.NewPlant(2, 4, SeedType.SEED_FLOWERPOT, SeedType.SEED_NONE);
			mBoard.NewPlant(2, 4, SeedType.SEED_CABBAGEPULT, SeedType.SEED_NONE);
			mBoard.NewPlant(3, 1, SeedType.SEED_FLOWERPOT, SeedType.SEED_NONE);
			mBoard.NewPlant(3, 1, SeedType.SEED_CABBAGEPULT, SeedType.SEED_NONE);
			mBoard.NewPlant(3, 2, SeedType.SEED_FLOWERPOT, SeedType.SEED_NONE);
			mBoard.NewPlant(3, 2, SeedType.SEED_CABBAGEPULT, SeedType.SEED_NONE);
			mBoard.NewPlant(3, 3, SeedType.SEED_FLOWERPOT, SeedType.SEED_NONE);
			mBoard.NewPlant(3, 3, SeedType.SEED_SUNFLOWER, SeedType.SEED_NONE);
			mBoard.NewPlant(3, 4, SeedType.SEED_FLOWERPOT, SeedType.SEED_NONE);
			mBoard.NewPlant(3, 4, SeedType.SEED_CABBAGEPULT, SeedType.SEED_NONE);
			mBoard.NewPlant(4, 0, SeedType.SEED_FLOWERPOT, SeedType.SEED_NONE);
			mBoard.NewPlant(4, 0, SeedType.SEED_CHOMPER, SeedType.SEED_NONE);
			mBoard.NewPlant(4, 1, SeedType.SEED_FLOWERPOT, SeedType.SEED_NONE);
			mBoard.NewPlant(4, 1, SeedType.SEED_CHOMPER, SeedType.SEED_NONE);
			mBoard.NewPlant(4, 2, SeedType.SEED_FLOWERPOT, SeedType.SEED_NONE);
			mBoard.NewPlant(4, 2, SeedType.SEED_REPEATER, SeedType.SEED_NONE);
			mBoard.NewPlant(4, 3, SeedType.SEED_FLOWERPOT, SeedType.SEED_NONE);
			mBoard.NewPlant(5, 2, SeedType.SEED_FLOWERPOT, SeedType.SEED_NONE);
			mBoard.NewPlant(5, 2, SeedType.SEED_WALLNUT, SeedType.SEED_NONE);
			mBoard.NewPlant(5, 3, SeedType.SEED_FLOWERPOT, SeedType.SEED_NONE);
			mBoard.NewPlant(5, 3, SeedType.SEED_THREEPEATER, SeedType.SEED_NONE);
			mBoard.NewPlant(5, 4, SeedType.SEED_FLOWERPOT, SeedType.SEED_NONE);
			mBoard.NewPlant(5, 4, SeedType.SEED_WALLNUT, SeedType.SEED_NONE);
			AddUpsellZombie(ZombieType.ZOMBIE_NORMAL, 460, 0);
			AddUpsellZombie(ZombieType.ZOMBIE_NORMAL, 680, 0);
			AddUpsellZombie(ZombieType.ZOMBIE_CATAPULT, 780, 1);
			AddUpsellZombie(ZombieType.ZOMBIE_TRAFFIC_CONE, 670, 1);
			AddUpsellZombie(ZombieType.ZOMBIE_NORMAL, 580, 0);
			AddUpsellZombie(ZombieType.ZOMBIE_NORMAL, 540, 1);
			AddUpsellZombie(ZombieType.ZOMBIE_PAIL, 500, 1);
			AddUpsellZombie(ZombieType.ZOMBIE_PAIL, 640, 2);
			AddUpsellZombie(ZombieType.ZOMBIE_TRAFFIC_CONE, 780, 3);
			AddUpsellZombie(ZombieType.ZOMBIE_NORMAL, 380, 3);
			AddUpsellZombie(ZombieType.ZOMBIE_CATAPULT, 704, 4);
			AddUpsellZombie(ZombieType.ZOMBIE_NORMAL, 690, 4);
			AddUpsellZombie(ZombieType.ZOMBIE_NORMAL, 590, 4);
			mPreUpdatingBoard = true;
			for (int k = 0; k < 100; k++)
			{
				mBoard.Update();
			}
			mPreUpdatingBoard = false;
			mApp.mMuteSoundsForCutscene = false;
		}

		public bool ShouldRunUpsellBoard()
		{
			if (mApp.mGameMode != GameMode.GAMEMODE_UPSELL && mApp.mGameMode != GameMode.GAMEMODE_INTRO)
			{
				return false;
			}
			return !mUpsellHideBoard;
		}

		public void DrawUpsell(Graphics g)
		{
			if (mCrazyDaveLastTalkIndex == 3315)
			{
				Reanimation newReanimation = Reanimation.GetNewReanimation();
				newReanimation.ReanimationInitializeType(Constants.CutScene_Upsell_TerraCotta_Pot.X, Constants.CutScene_Upsell_TerraCotta_Pot.Y, ReanimationType.REANIM_FLOWER_POT);
				newReanimation.SetFramesForLayer("anim_zengarden");
				newReanimation.OverrideScale(1.3f, 1.3f);
				newReanimation.Draw(g);
				mBoard.mMenuButton.Draw(g);
				newReanimation.PrepareForReuse();
			}
			if (mUpsellChallengeScreen != null)
			{
				mUpsellChallengeScreen.Draw(g);
				mBoard.mMenuButton.Draw(g);
			}
		}

		public void DrawIntro(Graphics g)
		{
			if (mCutsceneTime <= TimeIntro_PanRightStart)
			{
				g.SetColorizeImages(true);
				g.SetColor(SexyColor.Black);
				g.FillRect(-mBoard.mX, -mBoard.mY, Constants.BOARD_WIDTH, Constants.BOARD_HEIGHT);
				g.SetColorizeImages(false);
			}
			int num = TimeIntro_PanRightStart - 1000;
			if (mCutsceneTime > TimeIntro_PresentsFadeIn && mCutsceneTime <= num)
			{
				int theAlpha = (mCutsceneTime >= num - 600) ? TodCommon.TodAnimateCurve(num - 600, num - 300, mCutsceneTime, 255, 0, TodCurves.CURVE_LINEAR) : TodCommon.TodAnimateCurve(TimeIntro_PresentsFadeIn, TimeIntro_PresentsFadeIn + 300, mCutsceneTime, 0, Constants.CutScene_LogoEndPos, TodCurves.CURVE_LINEAR);
				TodCommon.TodDrawString(theColor: new SexyColor(255, 255, 255, theAlpha), g: g, theText: "[INTRO_PRESENTS]", thePosX: Constants.BOARD_WIDTH / 2 - mBoard.mX, thePosY: (int)(310f * Constants.S) - mBoard.mY - 40, theFont: Resources.FONT_BRIANNETOD16, theJustification: DrawStringJustification.DS_ALIGN_CENTER);
			}
			if (mCutsceneTime > TimeIntro_LogoStart && mCutsceneTime <= TimeIntro_PanRightEnd)
			{
				float num2 = TodCommon.TodAnimateCurveFloat(TimeIntro_LogoStart, TimeIntro_LogoEnd, mCutsceneTime, 5f, 1f, TodCurves.CURVE_EASE_OUT);
				TRect theRect = new TRect(Constants.BOARD_WIDTH / 2 - mBoard.mX - (int)((float)Constants.BOARD_WIDTH * 0.5f * num2), Constants.BOARD_HEIGHT / 2 - mBoard.mY - (int)(75f * num2), (int)((float)Constants.BOARD_WIDTH * num2), (int)((float)Constants.CutScene_LogoBackRect_Height * num2));
				g.SetColor(new SexyColor(0, 0, 0, 128));
				g.SetColorizeImages(true);
				g.FillRect(theRect);
				g.SetColorizeImages(false);
				TodCommon.TodDrawImageScaledF(g, Resources.IMAGE_PVZ_LOGO, (float)(Constants.BOARD_WIDTH / 2 - mBoard.mX) - (float)Resources.IMAGE_PVZ_LOGO.mWidth * 0.5f * num2, (float)(Constants.BOARD_HEIGHT / 2 - mBoard.mY) - (float)Resources.IMAGE_PVZ_LOGO.mHeight * 0.5f * num2, num2, num2);
			}
			if (mCutsceneTime > TimeIntro_FadeOut && mCutsceneTime <= TimeIntro_FadeOutEnd)
			{
				int theAlpha2 = TodCommon.TodAnimateCurve(TimeIntro_FadeOut, TimeIntro_FadeOutEnd, mCutsceneTime, 0, 255, TodCurves.CURVE_LINEAR);
				g.SetColor(new SexyColor(0, 0, 0, theAlpha2));
				g.SetColorizeImages(true);
				g.FillRect(-mBoard.mX, -mBoard.mY, Constants.BOARD_WIDTH, Constants.BOARD_HEIGHT);
			}
			if (mCutsceneTime > TimeIntro_FadeOutEnd)
			{
				g.SetColor(SexyColor.Black);
				g.SetColorizeImages(true);
				g.FillRect(-mBoard.mX, -mBoard.mY, Constants.BOARD_WIDTH, Constants.BOARD_HEIGHT);
			}
		}

		public void UpdateIntro()
		{
			int num = TodCommon.TodAnimateCurve(TimeIntro_PanRightStart, TimeIntro_PanRightEnd, mCutsceneTime, -100, 100, TodCurves.CURVE_LINEAR);
			mBoard.Move((int)((float)(-num) * Constants.S), 0);
			if (mCutsceneTime == 10)
			{
				LoadIntroBoard();
			}
			if (mCutsceneTime == TimeIntro_FadeOut)
			{
				mApp.mMusic.FadeOut(250);
			}
			if (mCutsceneTime == TimeIntro_LogoEnd)
			{
				mApp.AddTodParticle(Constants.CutScene_LogoEnd_Particle_Pos.X, Constants.CutScene_LogoEnd_Particle_Pos.Y, 400000, ParticleEffect.PARTICLE_SCREEN_FLASH);
				mApp.mMuteSoundsForCutscene = false;
				mApp.PlaySample(Resources.SOUND_HUGE_WAVE);
				mApp.mMuteSoundsForCutscene = true;
			}
			if (mCutsceneTime == TimeIntro_FadeOut - 200)
			{
				mApp.mMuteSoundsForCutscene = false;
				mApp.PlaySample(Resources.SOUND_SIREN);
				mApp.mMuteSoundsForCutscene = true;
			}
			if (mCutsceneTime == TimeIntro_End)
			{
				mApp.PreNewGame(GameMode.GAMEMODE_ADVENTURE, false);
			}
		}
	}
}
