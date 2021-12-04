using Microsoft.Xna.Framework;
using Sexy;
using Sexy.TodLib;
using System;

namespace Lawn
{
	internal class GameSelector : Widget, ButtonListener, StoreListener, AlmanacListener, MiniGamesWidgetListener, QuickPlayWidgetListener
	{
		private enum GameSelectorScreenState
		{
			Main,
			MoreGames,
			Achievements,
			QuickPlay
		}

		private GameSelectorScreenState state;

		public LawnApp mApp;

		public NewLawnButton mAdventureButton;

		public NewLawnButton mMoreWaysToPlayButton;

		public NewLawnButton mZenGardenButton;

		public NewLawnButton mOptionsButton;

		public DialogButton mStoreButton;

		public DialogButton mAlmanacButton;

		public DialogButton mUnlockGameButton;

		public DialogButton mAchievementsButton;

		public DialogButton mMoreGamesButton;

		public DialogButton mMoreGamesBackButton;

		public DialogButton mLeaderboardButton;

		public DialogButton mMiniGamesButton;

		public DialogButton mVaseBreakerButton;

		public DialogButton mIZombieButton;

		public DialogButton mMoreWaysBackButton;

		public int mSelectedQuickplayButtonId;

		public MoreGamesListWidget mMoreGamesListWidget;

		public ScrollWidget mMoreGamesScrollWidget;

		public string mLexText = string.Empty;

		public AchievementsWidget mAchievementsWidget;

		public ScrollWidget mAchievementsScrollWidget;

		public QuickPlayWidget mQuickplayWidget;

		public ScrollWidget mQuickplayScrollWidget;

		public MiniGamesWidget mMiniGamesWidget;

		public ScrollWidget mMiniGamesScrollWidget;

		public bool mQuickplayLocked;

		public bool mShowStartButton;

		public TodParticleSystem mTrophyParticleID;

		public Reanimation mWoodSignReanimID;

		public Reanimation[] mCloudReanimID = new Reanimation[6];

		public int[] mCloudCounter = new int[6];

		public Reanimation[] mFlowerReanimID = new Reanimation[3];

		public int mLeafCounter;

		public SelectorSignState mSignState;

		public bool mInUserDialog;

		public int mLevel;

		public bool mLoading;

		public bool mHasTrophy;

		public bool mUnlockSelectorCheat;

		public int mSlideCounter;

		public int mStartX;

		public int mStartY;

		public int mDestX;

		public int mDestY;

		public bool mNeedToPlayRollIn;

		public int mFadeInCounter;

		private static string cachedWelcomeString;

		private static string cachedWelcomeStringName;

		private string cachedTrophyString = string.Empty;

		private int cachedTrophyStringCount = -1;

		private bool wantToExitFromAchievementsViaBackButton;

		private float woodSignY;

		public bool mDoNewGameAfterStore;

		public int mQuickplaySlideCounter;

		public bool mRetractingQuickplay;

		public GameSelector(LawnApp theApp)
		{
			mApp = theApp;
			mLevel = 1;
			mLoading = false;
			mHasTrophy = false;
			mDoNewGameAfterStore = false;
			mInUserDialog = false;
			mAdventureButton = GameButton.MakeNewButton(100, this, "", null, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_ADVENTURE_BUTTON, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_ADVENTURE_HIGHLIGHT, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_ADVENTURE_HIGHLIGHT);
			mAdventureButton.Resize(Constants.MAIN_MENU_ORIGIN_X + Constants.GameSelector_AdventureButton_X, 10, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_ADVENTURE_BUTTON.mWidth, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_ADVENTURE_BUTTON.mHeight);
			mAdventureButton.mClip = false;
			mFadeInCounter = 0;
			mMoreWaysToPlayButton = GameButton.MakeNewButton(101, this, "", null, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_MOREWAYS, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_MOREWAYS_HIGHLIGHT, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_MOREWAYS_HIGHLIGHT);
			mMoreWaysToPlayButton.Resize(Constants.MAIN_MENU_ORIGIN_X + Constants.GameSelector_MiniGameButton_X, Constants.GameSelector_MiniGameButton_Y, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_MOREWAYS.mWidth, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_MOREWAYS.mHeight);
			mMoreWaysToPlayButton.mClip = false;
			mLeaderboardButton = GameButton.MakeNewButton(120, this, "", null, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_LEADERBOARD, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_LEADERBOARD_HIGHLIGHT, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_LEADERBOARD_HIGHLIGHT);
			mLeaderboardButton.Resize(Constants.MAIN_MENU_ORIGIN_X + Constants.GameSelector_LeaderboardButton_X, Constants.GameSelector_LeaderboardButton_Y, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_LEADERBOARD.mWidth, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_LEADERBOARD.mHeight);
			mLeaderboardButton.mClip = false;
			mZenGardenButton = GameButton.MakeNewButton(124, this, "", null, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_ZENGARDEN, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_ZENGARDEN_HIGHLIGHT, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_ZENGARDEN_HIGHLIGHT);
			mZenGardenButton.Resize(Constants.MAIN_MENU_ORIGIN_X + Constants.GameSelector_ZenGardenButton_X, Constants.GameSelector_ZenGardenButton_Y, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_ZENGARDEN.mWidth, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_ZENGARDEN.mHeight);
			mZenGardenButton.mClip = false;
			mOptionsButton = GameButton.MakeNewButton(109, this, "", null, AtlasResources.IMAGE_SELECTORSCREEN_OPTIONS1, AtlasResources.IMAGE_SELECTORSCREEN_OPTIONS2, AtlasResources.IMAGE_SELECTORSCREEN_OPTIONS2);
			mOptionsButton.Resize(Constants.MAIN_MENU_ORIGIN_X + Constants.GameSelector_OptionsButton_X, Constants.GameSelector_OptionsButton_Y, AtlasResources.IMAGE_SELECTORSCREEN_OPTIONS1.mWidth, AtlasResources.IMAGE_SELECTORSCREEN_OPTIONS1.mHeight + 25);
			mUnlockGameButton = GameButton.MakeNewButton(113, this, string.Empty, null, AtlasResources.IMAGE_BLANK, AtlasResources.IMAGE_BLANK, AtlasResources.IMAGE_BLANK);
			mUnlockGameButton.Resize(Constants.MAIN_MENU_ORIGIN_X, (int)Constants.InvertAndScale(100f), (int)Constants.InvertAndScale(210f), (int)Constants.InvertAndScale(30f));
			mStoreButton = GameButton.MakeNewButton(112, this, "", null, AtlasResources.IMAGE_SELECTORSCREEN_STORE, AtlasResources.IMAGE_SELECTORSCREEN_STOREHIGHLIGHT, AtlasResources.IMAGE_SELECTORSCREEN_STOREHIGHLIGHT);
			mStoreButton.Resize(Constants.MAIN_MENU_ORIGIN_X + Constants.GameSelector_StoreButton_X, Constants.GameSelector_StoreButton_Y, AtlasResources.IMAGE_SELECTORSCREEN_STORE.mWidth, AtlasResources.IMAGE_SELECTORSCREEN_STORE.mHeight);
			mAlmanacButton = GameButton.MakeNewButton(114, this, "", null, AtlasResources.IMAGE_SELECTORSCREEN_ALMANAC, AtlasResources.IMAGE_SELECTORSCREEN_ALMANACHIGHLIGHT, AtlasResources.IMAGE_SELECTORSCREEN_ALMANACHIGHLIGHT);
			mAlmanacButton.Resize(Constants.MAIN_MENU_ORIGIN_X + Constants.GameSelector_AlmanacButton_X, Constants.GameSelector_AlmanacButton_Y, AtlasResources.IMAGE_SELECTORSCREEN_ALMANAC.mWidth, AtlasResources.IMAGE_SELECTORSCREEN_ALMANAC.mHeight);
			mMoreGamesButton = GameButton.MakeNewButton(115, this, "", null, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_MOREGAMES_BUTTON, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_MOREGAMES_HIGHLIGHT, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_MOREGAMES_HIGHLIGHT);
			mMoreGamesButton.Resize(Constants.MAIN_MENU_ORIGIN_X + (int)Constants.InvertAndScale(10f), (int)Constants.InvertAndScale(170f), AtlasResources.IMAGE_REANIM_SELECTORSCREEN_MOREGAMES_BUTTON.mWidth, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_MOREGAMES_BUTTON.mHeight);
			mMoreGamesButton.mTranslateX = 0;
			mMoreGamesButton.mTranslateY = 0;
			mMoreGamesBackButton = GameButton.MakeNewButton(116, this, "", null, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_MOREGAMES_BACK_BUTTON, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_MOREGAMES_BACK_HIGHLIGHT, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_MOREGAMES_BACK_HIGHLIGHT);
			mMoreGamesBackButton.Resize(Constants.MAIN_MENU_ORIGIN_X + (int)Constants.InvertAndScale(10f), (int)Constants.InvertAndScale(170f), AtlasResources.IMAGE_REANIM_SELECTORSCREEN_MOREGAMES_BACK_BUTTON.mWidth, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_MOREGAMES_BACK_BUTTON.mHeight);
			mMoreGamesBackButton.mVisible = false;
			mMoreGamesBackButton.mDisabled = true;
			mMoreGamesBackButton.mTranslateX = 0;
			mMoreGamesBackButton.mTranslateY = 0;
			mAchievementsButton = GameButton.MakeNewButton(117, this, "", null, AtlasResources.IMAGE_BLANK, AtlasResources.IMAGE_BLANK, AtlasResources.IMAGE_BLANK);
			mAchievementsButton.Resize(Constants.MAIN_MENU_ORIGIN_X + Constants.GameSelector_AchievementsStatue_X, Constants.GameSelector_AchievementsStatue_Y, Resources.IMAGE_ACHIEVEMENT_GNOME.mWidth, Resources.IMAGE_ACHIEVEMENT_GNOME.mHeight);
			mMiniGamesButton = GameButton.MakeNewButton(121, this, "", null, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_MINIGAMES, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_MINIGAMES_HIGHLIGHT, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_MINIGAMES_HIGHLIGHT);
			mVaseBreakerButton = GameButton.MakeNewButton(122, this, "", null, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_VASEBREAKER, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_VASEBREAKER_HIGHLIGHT, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_VASEBREAKER_HIGHLIGHT);
			mIZombieButton = GameButton.MakeNewButton(123, this, "", null, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_IZOMBIE, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_IZOMBIE_HIGHLIGHT, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_IZOMBIE_HIGHLIGHT);
			mMoreWaysBackButton = GameButton.MakeNewButton(108, this, "", null, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_MOREWAYS_BACK, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_MOREWAYS_BACK_HIGHLIGHT, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_MOREWAYS_BACK_HIGHLIGHT);
			mMiniGamesButton.Resize(Constants.QUICKPLAY_ORIGIN_X + Constants.GameSelector_MoreWaysToPlay_MiniGames_X, Constants.GameSelector_MoreWaysToPlay_MiniGames_Y, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_MINIGAMES.mWidth + 2, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_MINIGAMES.mHeight + 2);
			mVaseBreakerButton.Resize(Constants.QUICKPLAY_ORIGIN_X + Constants.GameSelector_MoreWaysToPlay_VaseBreaker_X, Constants.GameSelector_MoreWaysToPlay_VaseBreaker_Y, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_VASEBREAKER.mWidth + 2, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_VASEBREAKER.mHeight + 2);
			mIZombieButton.Resize(Constants.QUICKPLAY_ORIGIN_X + Constants.GameSelector_MoreWaysToPlay_IZombie_X, Constants.GameSelector_MoreWaysToPlay_IZombie_Y, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_IZOMBIE.mWidth + 2, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_IZOMBIE.mHeight + 2);
			mMoreWaysBackButton.Resize(Constants.QUICKPLAY_ORIGIN_X + Constants.GameSelector_MoreWaysToPlay_Back_X, Constants.GameSelector_MoreWaysToPlay_Back_Y, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_MOREWAYS_BACK.mWidth + 2, AtlasResources.IMAGE_REANIM_SELECTORSCREEN_MOREWAYS_BACK.mHeight + 2);
			mMiniGamesButton.mTranslateX = -1;
			mMoreWaysBackButton.mTranslateX = (mMoreWaysBackButton.mTranslateY = 0);
			mSelectedQuickplayButtonId = 121;
			ToggleGameButton(mSelectedQuickplayButtonId);
			mMoreGamesListWidget = new MoreGamesListWidget(mApp);
			LoadGames();
			mMoreGamesListWidget.Resize(0, 0, mMoreGamesListWidget.mWidth, mMoreGamesListWidget.GetPreferredHeight() + (int)Constants.InvertAndScale(20f));
			mMoreGamesScrollWidget = new ScrollWidget();
			mMoreGamesScrollWidget.Resize(0, 0, mMoreGamesListWidget.mWidth, Constants.BOARD_HEIGHT);
			mMoreGamesScrollWidget.AddWidget(mMoreGamesListWidget);
			mMoreGamesScrollWidget.EnableIndicators(AtlasResources.IMAGE_SCROLL_INDICATOR);
			mAchievementsWidget = new AchievementsWidget(mApp);
			mAchievementsScrollWidget = new ScrollWidget();
			mAchievementsScrollWidget.EnableBounce(false);
			mAchievementsScrollWidget.Resize(Constants.ACHIEVEMENTS_ORIGIN_X, Constants.BOARD_HEIGHT, mAchievementsWidget.mWidth, Constants.BOARD_HEIGHT);
			mAchievementsScrollWidget.AddWidget(mAchievementsWidget);
			mQuickplayWidget = new QuickPlayWidget(mApp, this);
			mQuickplayScrollWidget = new ScrollWidget();
			mQuickplayScrollWidget.SetScrollMode(ScrollWidget.ScrollMode.SCROLL_HORIZONTAL);
			mQuickplayWidget.SizeToFit();
			mQuickplayScrollWidget.Resize(Constants.QUICKPLAY_ORIGIN_X + 30, 0, Constants.BOARD_WIDTH, mQuickplayWidget.mHeight);
			mQuickplayScrollWidget.AddWidget(mQuickplayWidget);
			mQuickplayScrollWidget.EnableIndicators(AtlasResources.IMAGE_SCROLL_INDICATOR);
			mMiniGamesWidget = new MiniGamesWidget(mApp, this);
			mMiniGamesScrollWidget = new ScrollWidget();
			mMiniGamesScrollWidget.SetScrollMode(ScrollWidget.ScrollMode.SCROLL_HORIZONTAL);
			mMiniGamesWidget.SizeToFit();
			mMiniGamesScrollWidget.Resize(Constants.QUICKPLAY_ORIGIN_X + 30, 10, Constants.BOARD_WIDTH, mMiniGamesWidget.mHeight);
			mMiniGamesScrollWidget.AddWidget(mMiniGamesWidget);
			mMiniGamesScrollWidget.EnableIndicators(AtlasResources.IMAGE_SCROLL_INDICATOR);
			mRetractingQuickplay = false;
			mQuickplaySlideCounter = 0;
			mNeedToPlayRollIn = false;
			AddWidget(mAdventureButton);
			AddWidget(mMoreWaysToPlayButton);
			AddWidget(mOptionsButton);
			AddWidget(mStoreButton);
			AddWidget(mAlmanacButton);
			AddWidget(mZenGardenButton);
			AddWidget(mAchievementsButton);
			if (mApp.mPlayerInfo != null && mApp.mPlayerInfo.mHasUnlockedPuzzleMode)
			{
				AddWidget(mVaseBreakerButton);
				AddWidget(mIZombieButton);
			}
			AddWidget(mMiniGamesButton);
			AddWidget(mMoreWaysBackButton);
			AddWidget(mAchievementsScrollWidget);
			AddWidget(mLeaderboardButton);
			AddWidget(mMiniGamesScrollWidget);
			mApp.mMusic.MakeSureMusicIsPlaying(MusicTune.MUSIC_TUNE_TITLE_CRAZY_DAVE_MAIN_THEME);
			mQuickplayLocked = false;
			mUnlockSelectorCheat = false;
			mTrophyParticleID = null;
			mShowStartButton = false;
			float theX = 0f;
			float theY = 0f;
			Reanimation theReanimation = mApp.AddReanimation(theX, theY, 400000, ReanimationType.REANIM_WOODSIGN, false);
			mWoodSignReanimID = mApp.ReanimationGetID(theReanimation);
			SetupUnlockFullGameReanim();
			if (SexyAppBase.IsInTrialMode)
			{
				AddWidget(mUnlockGameButton);
			}
			mLeafCounter = 50;
			mSignState = SelectorSignState.SIGN_UP;
			SyncProfile(false);
			LowerSign();
			mSlideCounter = 0;
			mStartX = (mStartY = (mDestX = (mDestY = 0)));
		}

		private void SetupUnlockFullGameReanim()
		{
			if (SexyAppBase.IsInTrialMode)
			{
				mWoodSignReanimID.AssignRenderGroupToPrefix("short rope1", -1);
				mWoodSignReanimID.AssignRenderGroupToPrefix("short rope2", -1);
				return;
			}
			mWoodSignReanimID.AssignRenderGroupToPrefix("long rope1", -1);
			mWoodSignReanimID.AssignRenderGroupToPrefix("long rope2", -1);
			mWoodSignReanimID.AssignRenderGroupToPrefix("click here", -1);
			mWoodSignReanimID.AssignRenderGroupToPrefix("broken", -1);
		}

		public override void Dispose()
		{
			RemoveAllWidgets(true, true);
		}

		public void SyncProfile(bool theShowLoading)
		{
			if (theShowLoading)
			{
				mLoading = true;
				mApp.PreloadForUser();
				mLoading = false;
			}
			TodParticleSystem todParticleSystem = mApp.ParticleTryToGet(mTrophyParticleID);
			if (todParticleSystem != null)
			{
				todParticleSystem.ParticleSystemDie();
				mTrophyParticleID = null;
			}
			mLevel = 1;
			if (mApp.mPlayerInfo != null)
			{
				mLevel = mApp.mPlayerInfo.mLevel;
			}
			mShowStartButton = true;
			mQuickplayLocked = true;
			if (mApp.mPlayerInfo != null && !mApp.IsIceDemo())
			{
				if (mLevel >= 2)
				{
					mShowStartButton = false;
				}
				if (mApp.mPlayerInfo.mHasUnlockedMinigames)
				{
					mQuickplayLocked = false;
				}
			}
			if (mApp.HasFinishedAdventure() && !mApp.IsTrialStageLocked())
			{
				mHasTrophy = true;
			}
			else
			{
				mHasTrophy = false;
			}
			if (mHasTrophy)
			{
				AddTrophySparkle();
			}
			SyncButtons();
			AlmanacDialog.AlmanacInitForPlayer();
			Board.BoardInitForPlayer();
		}

		public override void Draw(Graphics g)
		{
			if (mApp.GetDialog(Dialogs.DIALOG_STORE) == null && mApp.GetDialog(Dialogs.DIALOG_ALMANAC) == null)
			{
				DrawMoreGamesArea(g);
				DrawMainMenuArea(g);
				DrawQuickplayArea(g);
				DeferOverlay();
			}
		}

		public override void DrawOverlay(Graphics g)
		{
			if (mApp.GetDialog(Dialogs.DIALOG_STORE) != null || mApp.GetDialog(Dialogs.DIALOG_ALMANAC) != null)
			{
				return;
			}
			g.SetLinearBlend(true);
			g.DrawImage(AtlasResources.IMAGE_REANIM_SELECTORSCREEN_LEAVES, Constants.MAIN_MENU_ORIGIN_X, Constants.InvertAndScale(287f));
			if (mApp.mPlayerInfo == null)
			{
				return;
			}
			if (!mApp.IsIceDemo() && !mShowStartButton)
			{
				float num = 0f;
				float num2 = 0f;
				float num3 = 0f;
				float num4 = 0f;
				int num5 = TodCommon.ClampInt((mLevel - 1) / 10 + 1, 1, 6);
				int num6 = mLevel - (num5 - 1) * 10;
				if (num5 == 1)
				{
					num2 += 1f;
				}
				if (num5 == 4)
				{
					num -= 1f;
				}
				if (num6 == 3)
				{
					num3 -= 1f;
				}
				if (mAdventureButton.mIsDown)
				{
					num += (float)Constants.GameSelector_LevelNumber_ButtonDown_Offset;
					num3 += (float)Constants.GameSelector_LevelNumber_ButtonDown_Offset;
					num2 += (float)Constants.GameSelector_LevelNumber_ButtonDown_Offset;
					num4 += (float)Constants.GameSelector_LevelNumber_ButtonDown_Offset;
				}
				g.SetColorizeImages(true);
				g.SetColor(mAdventureButton.mColors[5]);
				TodCommon.TodDrawImageCelF(g, AtlasResources.IMAGE_SELECTORSCREEN_LEVELNUMBERS, (float)(Constants.MAIN_MENU_ORIGIN_X + Constants.GameSelector_LevelNumber_1_Pos.X) + num, (float)Constants.GameSelector_LevelNumber_1_Pos.Y + num2, num5, 0);
				if (num6 < 10)
				{
					TodCommon.TodDrawImageCelF(g, AtlasResources.IMAGE_SELECTORSCREEN_LEVELNUMBERS, (float)(Constants.MAIN_MENU_ORIGIN_X + Constants.GameSelector_LevelNumber_2_Pos.X) + num3, (float)Constants.GameSelector_LevelNumber_2_Pos.Y + num4, num6, 0);
				}
				else if (num6 == 10)
				{
					TodCommon.TodDrawImageCelF(g, AtlasResources.IMAGE_SELECTORSCREEN_LEVELNUMBERS, (float)(Constants.MAIN_MENU_ORIGIN_X + Constants.GameSelector_LevelNumber_2_Pos.X) + num3, (float)Constants.GameSelector_LevelNumber_2_Pos.Y + num4, 1, 0);
					TodCommon.TodDrawImageCelF(g, AtlasResources.IMAGE_SELECTORSCREEN_LEVELNUMBERS, (float)(Constants.MAIN_MENU_ORIGIN_X + Constants.GameSelector_LevelNumber_3_Pos.X) + num3, (float)Constants.GameSelector_LevelNumber_3_Pos.Y + num4, 0, 0);
				}
				g.SetColor(new SexyColor(255, 255, 255));
				g.FillRect(Constants.MAIN_MENU_ORIGIN_X + Constants.GameSelector_LevelNumber_Bar.X + (int)num, Constants.GameSelector_LevelNumber_Bar.Y + (int)num2, Constants.GameSelector_LevelNumber_Bar.Width, Constants.GameSelector_LevelNumber_Bar.Height);
				g.SetColorizeImages(false);
			}
			Reanimation reanimation = mApp.ReanimationGet(mWoodSignReanimID);
			g.Translate(Constants.MAIN_MENU_ORIGIN_X, 0);
			reanimation.Draw(g);
			g.Translate(-Constants.MAIN_MENU_ORIGIN_X, 0);
			if (mApp.mPlayerInfo != null && !string.IsNullOrEmpty(mApp.mPlayerInfo.mName) && (mSignState == SelectorSignState.SIGN_DOWN || mSignState == SelectorSignState.SIGN_MOVING_DOWN))
			{
				string welcomeString = GetWelcomeString();
				int theTrackIndex = reanimation.FindTrackIndex(GlobalMembersReanimIds.ReanimTrackId_welcome);
				SexyTransform2D theOverlayMatrix = default(SexyTransform2D);
				reanimation.GetAttachmentOverlayMatrix(theTrackIndex, out theOverlayMatrix);
				float num7 = Resources.FONT_BRIANNETOD16.StringWidth(welcomeString);
				SexyTransform2D sexyTransform2D = default(SexyTransform2D);
				sexyTransform2D.Translate(Constants.GameSelector_PlayerName_Pos.X - (int)(num7 * 0.5f) + Constants.MAIN_MENU_ORIGIN_X + mX, Constants.GameSelector_PlayerName_Pos.Y + mY);
				sexyTransform2D.Translate(-g.mTransX, -g.mTransY);
				TodCommon.TodDrawStringMatrix(g, Resources.FONT_BRIANNETOD16, theOverlayMatrix.mMatrix * sexyTransform2D.mMatrix, welcomeString, new SexyColor(255, 245, 200));
			}
			if (mApp.mPlayerInfo != null)
			{
				bool flag = mApp.mPlayerInfo.mLastSeenMoreGames < mApp.mProfileMgr.mLastMoreGamesUpdate;
			}
			g.SetColor(new SexyColor(115, 101, 66));
			g.SetColorizeImages(true);
			int num8 = mMoreGamesListWidget.mY + mMoreGamesListWidget.mHeight;
			int num9 = -mMoreGamesListWidget.mY + mMoreGamesScrollWidget.mHeight - 52;
			if (mMoreGamesListWidget.mY < -48)
			{
				int num10 = Constants.MORE_GAMES_PLANK_HEIGHT + Constants.MORE_GAMES_ITEM_GAP;
				int num11 = (num9 + num10 / 2) / num10 * num10;
				int theTimeAge = Math.Abs(num11 - num9);
				int theAlpha = TodCommon.TodAnimateCurve(0, 35, theTimeAge, 0, 220, TodCurves.CURVE_LINEAR);
				g.SetColor(new SexyColor(115, 101, 66, theAlpha));
			}
			if (num8 > 340)
			{
				int num12 = Constants.MORE_GAMES_PLANK_HEIGHT + Constants.MORE_GAMES_ITEM_GAP;
				int num13 = (num9 + num12 / 2) / num12 * num12;
				int theTimeAge2 = Math.Abs(num13 - num9);
				int theAlpha = TodCommon.TodAnimateCurve(0, 35, theTimeAge2, 0, 220, TodCurves.CURVE_LINEAR);
				g.SetColor(new SexyColor(115, 101, 66, theAlpha));
			}
			g.SetColorizeImages(false);
			g.Translate(-(Constants.QUICKPLAY_ORIGIN_X + mX), 0);
		}

		private string GetWelcomeString()
		{
			if (cachedWelcomeString == null || cachedWelcomeStringName != mApp.mPlayerInfo.mName)
			{
				cachedWelcomeString = Common.StrFormat_(TodStringFile.TodStringTranslate("[WELCOME_USER_NAME]"), mApp.mPlayerInfo.mName);
				cachedWelcomeStringName = mApp.mPlayerInfo.mName;
			}
			return cachedWelcomeString;
		}

		public void DrawMainMenuArea(Graphics g)
		{
			g.SetLinearBlend(true);
			g.DrawImage(Resources.IMAGE_SELECTORSCREEN_MAIN_BACKGROUND, Constants.MAIN_MENU_ORIGIN_X, 0);
			int num = (mAchievementsButton.mIsDown && mAchievementsButton.mIsOver) ? 1 : 0;
			g.DrawImage(Resources.IMAGE_ACHIEVEMENT_GNOME, Constants.MAIN_MENU_ORIGIN_X + Constants.GameSelector_AchievementsStatue_X + num, Constants.GameSelector_AchievementsStatue_Y + num);
			if (mAchievementsButton.mIsDown)
			{
				g.DrawImage(AtlasResources.IMAGE_REANIM_SELECTORSCREEN_ACHIEVEMENTS_HIGHLIGHT, Constants.MAIN_MENU_ORIGIN_X + Constants.GameSelector_AchievementsButton_X + num, Constants.GameSelector_AchievementsButton_Y + num);
			}
			else
			{
				g.DrawImage(AtlasResources.IMAGE_REANIM_SELECTORSCREEN_ACHIEVEMENTS_BUTTON, Constants.MAIN_MENU_ORIGIN_X + Constants.GameSelector_AchievementsButton_X + num, Constants.GameSelector_AchievementsButton_Y + num);
			}
			g.SetColor(SexyColor.White);
			g.SetFont(Resources.FONT_PICO129);
		}

		public void DrawQuickplayArea(Graphics g)
		{
			g.DrawImage(Resources.IMAGE_SELECTORSCREEN_QUICKPLAY_BACKGROUND, Constants.QUICKPLAY_ORIGIN_X, 0);
			g.DrawImage(AtlasResources.IMAGE_TROPHY, Constants.QUICKPLAY_ORIGIN_X + Constants.BOARD_WIDTH - 155, Constants.BOARD_HEIGHT - 50);
			int num = mApp.GetNumTrophies(ChallengePage.CHALLENGE_PAGE_CHALLENGE) + mApp.GetNumTrophies(ChallengePage.CHALLENGE_PAGE_PUZZLE);
			int num2 = 37;
			if (cachedTrophyStringCount != num)
			{
				cachedTrophyString = num.ToString() + "/" + num2.ToString();
				cachedTrophyStringCount = num;
			}
			g.SetFont(Resources.FONT_DWARVENTODCRAFT18);
			g.SetColor(new Color(224, 187, 98));
			g.DrawString(cachedTrophyString, Constants.QUICKPLAY_ORIGIN_X + Constants.BOARD_WIDTH - 90, Constants.BOARD_HEIGHT - 50);
		}

		public void DrawMoreGamesArea(Graphics g)
		{
		}

		public void RaiseSign()
		{
			Reanimation reanimation = mApp.ReanimationGet(mWoodSignReanimID);
			reanimation.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_anim_lift, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 0, 10f);
			mSignState = SelectorSignState.SIGN_MOVING_UP;
			mNeedToPlayRollIn = false;
		}

		public void LowerSign()
		{
			Reanimation reanimation = mApp.ReanimationGet(mWoodSignReanimID);
			reanimation.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_anim_drop, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 0, 10f);
			mSignState = SelectorSignState.SIGN_MOVING_DOWN;
			mNeedToPlayRollIn = true;
			woodSignY = -30f;
		}

		public override bool BackButtonPress()
		{
			switch (state)
			{
			case GameSelectorScreenState.Achievements:
				mAchievementsScrollWidget.ScrollToMin(true);
				wantToExitFromAchievementsViaBackButton = true;
				mAchievementsScrollWidget.mSpringOverride = 0.6f;
				break;
			case GameSelectorScreenState.MoreGames:
				ButtonPress(mMoreGamesBackButton.mId);
				ButtonDepress(mMoreGamesBackButton.mId);
				break;
			case GameSelectorScreenState.QuickPlay:
				ButtonPress(mMoreWaysBackButton.mId);
				ButtonDepress(mMoreWaysBackButton.mId);
				break;
			case GameSelectorScreenState.Main:
				mApp.WantsToExit = true;
				break;
			}
			return true;
		}

		public override void Update()
		{
			base.Update();
			if (wantToExitFromAchievementsViaBackButton && mAchievementsScrollWidget.GetScrollOffset().y > -5f)
			{
				mAchievementsScrollWidget.SetScrollOffset(0f, 0f);
				mAchievementsScrollWidget.mSpringOverride = 0f;
				ButtonPress(118);
				ButtonDepress(118);
				wantToExitFromAchievementsViaBackButton = false;
			}
			if (mApp.GetDialog(Dialogs.DIALOG_STORE) != null || mApp.GetDialog(Dialogs.DIALOG_ALMANAC) != null)
			{
				return;
			}
			MarkDirty();
			if (mSlideCounter > 0)
			{
				int theNewX = TodCommon.TodAnimateCurve(75, 0, mSlideCounter, mStartX, mDestX, TodCurves.CURVE_EASE_IN_OUT);
				int theNewY = TodCommon.TodAnimateCurve(75, 0, mSlideCounter, mStartY, mDestY, TodCurves.CURVE_EASE_IN_OUT);
				Move(theNewX, theNewY);
				mSlideCounter -= 3;
				if (mSlideCounter >= 0 && mSlideCounter < 3)
				{
					if (mX == -Constants.MAIN_MENU_ORIGIN_X && mY == 0 && mSignState == SelectorSignState.SIGN_UP)
					{
						LowerSign();
					}
				}
				else if (mSignState == SelectorSignState.SIGN_DOWN && mStartX == -Constants.MAIN_MENU_ORIGIN_X && mDestX == 0)
				{
					RaiseSign();
				}
			}
			if (mQuickplaySlideCounter > 0)
			{
				int num = 150;
				int theNewY2 = (!mRetractingQuickplay) ? TodCommon.TodAnimateCurve(30, 0, mQuickplaySlideCounter, -mMiniGamesScrollWidget.mHeight - num, 10, TodCurves.CURVE_EASE_IN_OUT) : TodCommon.TodAnimateCurve(30, 0, mQuickplaySlideCounter, 10, -mMiniGamesScrollWidget.mHeight - num, TodCurves.CURVE_EASE_IN_OUT);
				mQuickplayScrollWidget.Move(mQuickplayScrollWidget.mX, theNewY2);
				mMiniGamesScrollWidget.Move(mMiniGamesScrollWidget.mX, theNewY2);
				mQuickplaySlideCounter -= 3;
				if (mQuickplaySlideCounter >= 0 && mQuickplaySlideCounter < 3 && mRetractingQuickplay && (mSlideCounter == 0 || mDestX == -Constants.QUICKPLAY_ORIGIN_X))
				{
					PopulateQuickPlayWidget();
					SlideOutQuickPlayWidget();
				}
			}
			if (mApp.mZenGarden != null)
			{
				mApp.mZenGarden.UpdatePlantNeeds();
			}
			TodParticleSystem todParticleSystem = mApp.ParticleTryToGet(mTrophyParticleID);
			if (todParticleSystem != null)
			{
				todParticleSystem.Update();
			}
			Reanimation reanimation = mApp.ReanimationGet(mWoodSignReanimID);
			if (mSignState == SelectorSignState.SIGN_DOWN)
			{
				if (reanimation.mLoopCount > 0)
				{
					if (mApp.mPlayerInfo == null)
					{
						mInUserDialog = true;
						mApp.DoCreateUserDialog(true);
						RaiseSign();
					}
					if (mHasTrophy)
					{
						AddTrophySparkle();
					}
					if (mApp.mPlayerInfo != null && mApp.mPlayerInfo.mNeedsMessageOnGameSelector)
					{
						mApp.mPlayerInfo.mNeedsMessageOnGameSelector = false;
						mApp.WriteCurrentUserConfig();
						mApp.LawnMessageBox(49, "[ADVENTURE_COMPLETE_HEADER]", "[ADVENTURE_COMPLETE_BODY]", "[DIALOG_BUTTON_OK]", "", 3, null);
					}
				}
			}
			else if (mSignState == SelectorSignState.SIGN_MOVING_UP)
			{
				if (reanimation.mLoopCount > 0)
				{
					mSignState = SelectorSignState.SIGN_UP;
				}
			}
			else if (mSignState == SelectorSignState.SIGN_MOVING_DOWN && reanimation.mLoopCount > 0)
			{
				mSignState = SelectorSignState.SIGN_DOWN;
			}
			if (mInUserDialog && mApp.GetDialog(30) == null)
			{
				mInUserDialog = false;
				LowerSign();
			}
			if (mNeedToPlayRollIn)
			{
				mApp.PlaySample(Resources.SOUND_ROLL_IN);
				mNeedToPlayRollIn = false;
			}
			if (woodSignY != 0f)
			{
				woodSignY += 1f;
				if (woodSignY > 0f)
				{
					woodSignY = 0f;
				}
				reanimation.SetPosition(reanimation.mOverlayMatrix.mMatrix.Translation.X, woodSignY);
			}
			reanimation.Update();
			if (mFadeInCounter > 0)
			{
				mFadeInCounter -= 3;
			}
		}

		public virtual void ButtonMouseEnter(int theId)
		{
			if (!mQuickplayLocked || theId != 101)
			{
				mApp.PlayFoley(FoleyType.FOLEY_BLEEP);
			}
		}

		public virtual void ButtonPress(int theId, int theClickCount)
		{
			if (mSlideCounter == 0)
			{
				if (theId == 100 || theId == 101)
				{
					mApp.PlaySample(Resources.SOUND_GRAVEBUTTON);
				}
				else
				{
					mApp.PlaySample(Resources.SOUND_TAP);
				}
			}
		}

		public virtual void ButtonDepress(int theId)
		{
			if (mApp.mPlayerInfo == null || mSlideCounter != 0)
			{
				return;
			}
			if (theId == 101 && mQuickplayLocked)
			{
				mApp.LawnMessageBox(49, "[MODE_LOCKED]", "[QUICKPLAY_LOCKED_MESSAGE]", "[DIALOG_BUTTON_OK]", "", 3, null);
				return;
			}
			state = GameSelectorScreenState.Main;
			switch (theId)
			{
			case 119:
				break;
			case 124:
				mApp.KillGameSelector();
				mApp.PreNewGame(GameMode.GAMEMODE_CHALLENGE_ZEN_GARDEN, false);
				break;
			case 120:
				mApp.KillGameSelector();
				mApp.ShowLeaderboardScreen();
				break;
			case 100:
				mApp.KillGameSelector();
				ClickedAdventure();
				break;
			case 101:
				mQuickplayWidget.Clear();
				mQuickplayWidget.SizeToFit();
				SlideTo(-Constants.QUICKPLAY_ORIGIN_X - 30, 0);
				SlideOutQuickPlayWidget();
				state = GameSelectorScreenState.QuickPlay;
				break;
			case 111:
				mApp.ConfirmQuit();
				break;
			case 110:
				mApp.KillGameSelector();
				mApp.ShowAwardScreen(AwardType.AWARD_HELP_ZOMBIE_NOTE, false);
				break;
			case 109:
				mApp.DoNewOptions(true);
				break;
			case 113:
				mApp.BuyGame();
				break;
			case 112:
				mDoNewGameAfterStore = false;
				mApp.ShowStoreScreen(this);
				break;
			case 114:
				mApp.DoAlmanacDialog(SeedType.SEED_NONE, ZombieType.ZOMBIE_INVALID, this);
				break;
			case 115:
				mMoreGamesButton.mVisible = false;
				mMoreGamesButton.mDisabled = true;
				mMoreGamesBackButton.mVisible = true;
				mMoreGamesBackButton.mDisabled = false;
				mMoreGamesScrollWidget.ScrollToMin(false);
				if (mApp.mPlayerInfo != null)
				{
					mApp.mPlayerInfo.mLastSeenMoreGames = DateTime.UtcNow;
					mApp.WriteCurrentUserConfig();
				}
				SlideTo(0, 0);
				state = GameSelectorScreenState.MoreGames;
				break;
			case 116:
				mMoreGamesBackButton.mVisible = false;
				mMoreGamesBackButton.mDisabled = true;
				mMoreGamesButton.mVisible = true;
				mMoreGamesButton.mDisabled = false;
				SlideTo(-Constants.MAIN_MENU_ORIGIN_X, 0);
				state = GameSelectorScreenState.Main;
				break;
			case 108:
				RetractQuickPlayWidget();
				SlideTo(-Constants.MAIN_MENU_ORIGIN_X, 0);
				state = GameSelectorScreenState.Main;
				break;
			case 117:
#if DEBUG
				mApp.KillGameSelector();
				mApp.ShowChallengeScreen(ChallengePage.CHALLENGE_PAGE_LIMBO);
#else
				mAchievementsScrollWidget.ScrollToMin(false);
				SlideTo(-Constants.ACHIEVEMENTS_ORIGIN_X, -Constants.BOARD_HEIGHT);
				mAchievementsScrollWidget.SetDisabled(false);
				state = GameSelectorScreenState.Achievements;
#endif
					break;
			case 118:
				if (mAchievementsScrollWidget.GetScrollOffset().y != 0f)
				{
					mAchievementsScrollWidget.ScrollToMin(true);
					wantToExitFromAchievementsViaBackButton = true;
				}
				else
				{
					SlideTo(-Constants.MAIN_MENU_ORIGIN_X, 0);
					state = GameSelectorScreenState.Main;
					mAchievementsScrollWidget.SetDisabled(true);
				}
				break;
			case 102:
			case 103:
			case 104:
			case 105:
			case 106:
			case 107:
				if (theId != mSelectedQuickplayButtonId)
				{
					ToggleGameButton(mSelectedQuickplayButtonId);
					ToggleGameButton(theId);
					mSelectedQuickplayButtonId = theId;
					RetractQuickPlayWidget();
				}
				break;
			case 121:
			case 122:
			case 123:
				if (theId != mSelectedQuickplayButtonId)
				{
					ToggleGameButton(mSelectedQuickplayButtonId);
					ToggleGameButton(theId);
					mSelectedQuickplayButtonId = theId;
					RetractQuickPlayWidget();
				}
				state = GameSelectorScreenState.MoreGames;
				break;
			}
		}

		public override void KeyDown(KeyCode theKey)
		{
			if (mApp.mKonamiCheck.Check(theKey))
			{
				mApp.PlayFoley(FoleyType.FOLEY_DROP);
			}
			else if (mApp.mMustacheCheck.Check(theKey) || mApp.mMoustacheCheck.Check(theKey))
			{
				mApp.PlayFoley(FoleyType.FOLEY_POLEVAULT);
				mApp.mMustacheMode = !mApp.mMustacheMode;
			}
			else if (mApp.mSuperMowerCheck.Check(theKey) || mApp.mSuperMowerCheck2.Check(theKey))
			{
				mApp.PlayFoley(FoleyType.FOLEY_ZAMBONI);
				mApp.mSuperMowerMode = !mApp.mSuperMowerMode;
			}
			else if (mApp.mFutureCheck.Check(theKey))
			{
				mApp.PlaySample(Resources.SOUND_BOING);
				mApp.mFutureMode = !mApp.mFutureMode;
			}
			else if (mApp.mPinataCheck.Check(theKey))
			{
				if (mApp.CanDoPinataMode())
				{
					mApp.PlayFoley(FoleyType.FOLEY_JUICY);
					mApp.mPinataMode = !mApp.mPinataMode;
				}
				else
				{
					mApp.PlaySample(Resources.SOUND_BUZZER);
				}
			}
			else if (mApp.mDanceCheck.Check(theKey))
			{
				if (mApp.CanDoDanceMode())
				{
					mApp.PlayFoley(FoleyType.FOLEY_DANCER);
					mApp.mDanceMode = !mApp.mDanceMode;
				}
				else
				{
					mApp.PlaySample(Resources.SOUND_BUZZER);
				}
			}
			else if (mApp.mDaisyCheck.Check(theKey))
			{
				if (mApp.CanDoDaisyMode())
				{
					mApp.PlaySample(Resources.SOUND_LOADINGBAR_FLOWER);
					mApp.mDaisyMode = !mApp.mDaisyMode;
				}
				else
				{
					mApp.PlaySample(Resources.SOUND_BUZZER);
				}
			}
		}

		public override void KeyChar(SexyChar theChar)
		{
			if (theChar.value_type != 'u' || mApp.mPlayerInfo == null)
			{
				return;
			}
			mApp.mPlayerInfo.mFinishedAdventure = 2;
			mApp.mPlayerInfo.AddCoins(50000);
			mApp.mPlayerInfo.mHasUsedCheatKeys = true;
			mApp.mPlayerInfo.mHasUnlockedMinigames = true;
			mApp.mPlayerInfo.mHasUnlockedPuzzleMode = true;
			mApp.mPlayerInfo.mHasUnlockedSurvivalMode = true;
			for (int i = 0; i < 200; i++)
			{
				GameMode gameMode = (GameMode)(i + 1);
				if (gameMode != GameMode.GAMEMODE_SCARY_POTTER_ENDLESS && gameMode != GameMode.GAMEMODE_PUZZLE_I_ZOMBIE_ENDLESS && gameMode != GameMode.GAMEMODE_SURVIVAL_ENDLESS_STAGE_3)
				{
					mApp.mPlayerInfo.mChallengeRecords[i] = 20;
				}
			}
			for (int j = 0; j < 80; j++)
			{
				mApp.mPlayerInfo.mPurchases[j] = 1;
			}
			mApp.mPlayerInfo.mPurchases[7] = 0;
			SyncProfile(true);
			string savedGameName = LawnCommon.GetSavedGameName(GameMode.GAMEMODE_ADVENTURE, (int)mApp.mPlayerInfo.mId);
			mApp.EraseFile(savedGameName);
		}

		public override void MouseDown(int x, int y, int theClickCount)
		{
			if (y > Constants.BOARD_HEIGHT)
			{
				mApp.PlaySample(Resources.SOUND_TAP);
				ButtonDepress(118);
			}
		}

		public override void AddedToManager(WidgetManager theWidgetManager)
		{
			base.AddedToManager(theWidgetManager);
			mX = -Constants.MAIN_MENU_ORIGIN_X;
		}

		public void SyncButtons()
		{
			bool flag = mApp.CanShowAlmanac() || mUnlockSelectorCheat;
			bool flag2 = mApp.CanShowStore() || mUnlockSelectorCheat;
			bool flag3 = mApp.CanShowZenGarden() || mUnlockSelectorCheat;
			mAlmanacButton.mDisabled = !flag;
			mAlmanacButton.mVisible = flag;
			mStoreButton.mDisabled = !flag2;
			mStoreButton.mVisible = flag2;
			mZenGardenButton.mDisabled = !flag3;
			mZenGardenButton.mVisible = flag3;
			SetupUnlockFullGameReanim();
			if (mQuickplayLocked)
			{
				mMoreWaysToPlayButton.mOverImage = AtlasResources.IMAGE_REANIM_SELECTORSCREEN_MOREWAYS;
				mMoreWaysToPlayButton.mDownImage = AtlasResources.IMAGE_REANIM_SELECTORSCREEN_MOREWAYS;
				mMoreWaysToPlayButton.SetColor(5, new SexyColor(128, 128, 128));
			}
			else
			{
				mMoreWaysToPlayButton.mOverImage = AtlasResources.IMAGE_REANIM_SELECTORSCREEN_MOREWAYS_HIGHLIGHT;
				mMoreWaysToPlayButton.mDownImage = AtlasResources.IMAGE_REANIM_SELECTORSCREEN_MOREWAYS_HIGHLIGHT;
				mMoreWaysToPlayButton.SetColor(5, SexyColor.White);
			}
		}

		public void AddTrophySparkle()
		{
		}

		public void ClickedAdventure()
		{
			mApp.mMusic.StopAllMusic();
			mAdventureButton.SetDisabled(true);
			mMoreWaysToPlayButton.SetDisabled(true);
			mZenGardenButton.SetDisabled(true);
			mOptionsButton.SetDisabled(true);
			mUnlockGameButton.SetDisabled(true);
			mStoreButton.SetDisabled(true);
			mAlmanacButton.SetDisabled(true);
			mMoreGamesButton.SetDisabled(true);
			mAchievementsButton.SetDisabled(true);
			mApp.KillGameSelector();
			if (mApp.IsIceDemo())
			{
				mApp.PreNewGame(GameMode.GAMEMODE_CHALLENGE_ICE, false);
			}
			else if (mApp.IsFirstTimeAdventureMode() && mLevel == 1 && !mApp.SaveFileExists())
			{
				mApp.PreNewGame(GameMode.GAMEMODE_INTRO, false);
			}
			else if (mApp.mPlayerInfo.mNeedsMagicTacoReward && mLevel == 35)
			{
				StoreScreen storeScreen = mApp.ShowStoreScreen(this);
				storeScreen.SetupForIntro(601);
				mDoNewGameAfterStore = true;
			}
			else if (ShouldDoZenTuturialBeforeAdventure())
			{
				mApp.PreNewGame(GameMode.GAMEMODE_CHALLENGE_ZEN_GARDEN, false);
				mApp.mZenGarden.SetupForZenTutorial();
			}
			else
			{
				mApp.PreNewGame(GameMode.GAMEMODE_ADVENTURE, true);
			}
		}

		public bool ShouldDoZenTuturialBeforeAdventure()
		{
			if (!mApp.HasFinishedAdventure() && mApp.mPlayerInfo.mLevel == 45 && mApp.mPlayerInfo.mNumPottedPlants == 0)
			{
				return true;
			}
			return false;
		}

		public void BackFromStore()
		{
			StoreScreen storeScreen = (StoreScreen)mApp.GetDialog(Dialogs.DIALOG_STORE);
			mApp.KillDialog(4);
			if (mDoNewGameAfterStore)
			{
				mApp.PreNewGame(GameMode.GAMEMODE_ADVENTURE, false);
			}
			else
			{
				mApp.mMusic.MakeSureMusicIsPlaying(MusicTune.MUSIC_TUNE_TITLE_CRAZY_DAVE_MAIN_THEME);
			}
		}

		public void QuickPlayStageSelected(int theLevel)
		{
			mApp.PlaySample(Resources.SOUND_TAP);
			mApp.KillGameSelector();
			GameMode theGameMode = (GameMode)(theLevel - 1 + 72);
			mApp.PreNewGame(theGameMode, true);
		}

		public void MiniGamesStageSelected(int theLevel)
		{
			mApp.PlaySample(Resources.SOUND_TAP);
			GameMode theGameMode = (GameMode)(theLevel + 1);
			mApp.KillGameSelector();
			mApp.PreNewGame(theGameMode, true);
		}

		public void BackFromAlmanac()
		{
			mApp.mMusic.MakeSureMusicIsPlaying(MusicTune.MUSIC_TUNE_TITLE_CRAZY_DAVE_MAIN_THEME);
		}

		public void LoadGames()
		{
		}

		public void SlideTo(int theX, int theY)
		{
			mSlideCounter = 75;
			mDestX = theX;
			mDestY = theY;
			mStartX = mX;
			mStartY = mY;
		}

		public void MoveToQuickplay(bool theDoFadeIn, GameSelectorButtons theButton)
		{
			state = GameSelectorScreenState.QuickPlay;
			if (theDoFadeIn)
			{
				mFadeInCounter = 180;
			}
			mQuickplayWidget.Clear();
			mQuickplayWidget.SizeToFit();
			Move(-Constants.QUICKPLAY_ORIGIN_X - 30, 0);
			RetractQuickPlayWidget();
			mQuickplaySlideCounter = 0;
			if (theButton != (GameSelectorButtons)mSelectedQuickplayButtonId)
			{
				ToggleGameButton(mSelectedQuickplayButtonId);
				ToggleGameButton((int)theButton);
				mSelectedQuickplayButtonId = (int)theButton;
				PopulateQuickPlayWidget();
				mMiniGamesScrollWidget.ScrollToMin(false);
				mMiniGamesScrollWidget.Move(mMiniGamesScrollWidget.mX, 10);
			}
		}

		public void SlideOutQuickPlayWidget()
		{
			mQuickplayScrollWidget.ScrollToMin(false);
			mMiniGamesScrollWidget.ScrollToMin(false);
			mRetractingQuickplay = false;
			mQuickplaySlideCounter = 30;
		}

		public void RetractQuickPlayWidget()
		{
			mQuickplayScrollWidget.ScrollToMin(false);
			mRetractingQuickplay = true;
			mQuickplaySlideCounter = 30;
		}

		public void PopulateQuickPlayWidget()
		{
			MiniGameMode mode = MiniGameMode.MINI_GAME_MODE_GAMES;
			switch (mSelectedQuickplayButtonId)
			{
			case 121:
				mode = MiniGameMode.MINI_GAME_MODE_GAMES;
				break;
			case 123:
				mode = MiniGameMode.MINI_GAME_MODE_I_ZOMBIE;
				break;
			case 122:
				mode = MiniGameMode.MINI_GAME_MODE_VASEBREAKER;
				break;
			}
			mMiniGamesWidget.SwitchMode(mode);
		}

		public void ToggleGameButton(int theId)
		{
			DialogButton dialogButton;
			switch (theId)
			{
			default:
				return;
			case 121:
				dialogButton = mMiniGamesButton;
				break;
			case 122:
				dialogButton = mVaseBreakerButton;
				break;
			case 123:
				dialogButton = mIZombieButton;
				break;
			}
			Image mButtonImage = dialogButton.mButtonImage;
			dialogButton.mButtonImage = dialogButton.mDownImage;
			dialogButton.mDownImage = (dialogButton.mOverImage = mButtonImage);
		}

		public void ButtonMouseMove(int id, int x, int y)
		{
		}

		public void ButtonMouseLeave(int id)
		{
		}

		public void ButtonMouseTick(int id)
		{
		}

		public void ButtonDownTick(int id)
		{
		}

		public void ButtonPress(int id)
		{
		}
	}
}
