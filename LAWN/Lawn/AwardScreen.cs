using Sexy;
using Sexy.TodLib;
using System.Collections.Generic;

namespace Lawn
{
	internal class AwardScreen : Widget, AlmanacListener, StoreListener, ButtonListener
	{
		internal class AchievementScreenItem
		{
			public AchievementId mId;

			public int mStartAnimTime;

			public int mEndAnimTime;

			public int mDestY;

			public int mStartY;

			public int mY;
		}

		public NewLawnButton mCreditsButton;

		public GameButton mStartButton;

		public GameButton mMenuButton;

		public GameButton mContinueButton;

		public bool mShowStartButtonAfterAchievements;

		public bool mShowMenuButtonAfterAchievements;

		public LawnApp mApp;

		public int mFadeInCounter;

		public AwardType mAwardType;

		public int mAchievementAnimTime;

		public List<AchievementScreenItem> mAchievementItems = new List<AchievementScreenItem>();

		public bool mShowingAchievements;

		public AwardScreen(LawnApp theApp, AwardType theAwardType, bool theShowAchievements)
		{
			theShowAchievements = false;
			mApp = theApp;
			mClip = false;
			mFadeInCounter = 180;
			mAwardType = theAwardType;
			mShowingAchievements = theShowAchievements;
			mAchievementAnimTime = 0;
			mApp.DelayLoadMainMenuResource(false);
			if (mShowingAchievements)
			{
				for (int i = 0; i < 18; i++)
				{
					if (mApp.mPlayerInfo.mEarnedAchievements[i] && !mApp.mPlayerInfo.mShownAchievements[i])
					{
						mApp.mPlayerInfo.mShownAchievements[i] = true;
						AchievementScreenItem achievementScreenItem = new AchievementScreenItem();
						achievementScreenItem.mId = (AchievementId)i;
						achievementScreenItem.mStartAnimTime = 100 * mAchievementItems.Count + 50;
						achievementScreenItem.mEndAnimTime = achievementScreenItem.mStartAnimTime + 100;
						achievementScreenItem.mStartY = Constants.BOARD_HEIGHT;
						achievementScreenItem.mY = achievementScreenItem.mStartY;
						mAchievementItems.Add(achievementScreenItem);
					}
				}
				if (mAchievementItems.Count == 0)
				{
					mShowingAchievements = false;
				}
				else
				{
					int num = 114 - mAchievementItems.Count * 76 / 2 + 20;
					for (int j = 0; j < mAchievementItems.Count; j++)
					{
						mAchievementItems[j].mDestY = num;
						num += 76;
					}
				}
				mApp.WriteCurrentUserConfig();
			}
			int level = mApp.mPlayerInfo.GetLevel();
			if (mAwardType == AwardType.AWARD_CREDITS_ZOMBIE_NOTE)
			{
				mApp.DelayLoadBackgroundResource("DelayLoad_Background6");
				mApp.DelayLoadZombieNotePaperResource("DelayLoad_ZombieNote");
				mApp.DelayLoadZombieNoteResource("DelayLoad_Credits");
			}
			else if (mAwardType == AwardType.AWARD_HELP_ZOMBIE_NOTE)
			{
				mApp.DelayLoadBackgroundResource("DelayLoad_Background1");
				mApp.DelayLoadZombieNotePaperResource("DelayLoad_ZombieNote");
				mApp.DelayLoadZombieNoteResource("DelayLoad_ZombieNoteHelp");
			}
			else if (mApp.IsAdventureMode())
			{
				switch (level)
				{
				case 10:
					mApp.DelayLoadBackgroundResource("DelayLoad_Background1");
					mApp.DelayLoadZombieNotePaperResource("DelayLoad_ZombieNote");
					mApp.DelayLoadZombieNoteResource("DelayLoad_ZombieNote1");
					break;
				case 20:
					mApp.DelayLoadBackgroundResource("DelayLoad_Background2");
					mApp.DelayLoadZombieNotePaperResource("DelayLoad_ZombieNote");
					mApp.DelayLoadZombieNoteResource("DelayLoad_ZombieNote2");
					break;
				case 30:
					mApp.DelayLoadBackgroundResource("DelayLoad_Background1");
					mApp.DelayLoadZombieNotePaperResource("DelayLoad_ZombieNote");
					mApp.DelayLoadZombieNoteResource("DelayLoad_ZombieNote3");
					break;
				case 40:
					mApp.DelayLoadBackgroundResource("DelayLoad_Background2");
					mApp.DelayLoadZombieNotePaperResource("DelayLoad_ZombieNote");
					mApp.DelayLoadZombieNoteResource("DelayLoad_ZombieNote4");
					break;
				case 50:
					mApp.DelayLoadBackgroundResource("DelayLoad_Background1");
					mApp.DelayLoadZombieNotePaperResource("DelayLoad_ZombieNote");
					mApp.DelayLoadZombieNoteResource("DelayLoad_ZombieFinalNote");
					break;
				}
			}
			mStartButton = new GameButton(100, this);
			mStartButton.mButtonImage = AtlasResources.IMAGE_SEEDCHOOSER_BUTTON;
			mStartButton.mOverImage = null;
			mStartButton.mDownImage = null;
			mStartButton.mDisabledImage = AtlasResources.IMAGE_SEEDCHOOSER_BUTTON_DISABLED;
			mStartButton.mOverOverlayImage = AtlasResources.IMAGE_SEEDCHOOSER_BUTTON_GLOW;
			mStartButton.SetFont(Resources.FONT_DWARVENTODCRAFT15);
			mStartButton.mColors[0] = new SexyColor(213, 159, 43);
			mStartButton.mColors[1] = new SexyColor(213, 159, 43);
			mContinueButton = new GameButton(100, this);
			mContinueButton.mButtonImage = AtlasResources.IMAGE_SEEDCHOOSER_BUTTON;
			mContinueButton.mOverImage = null;
			mContinueButton.mDownImage = null;
			mContinueButton.mDisabledImage = AtlasResources.IMAGE_SEEDCHOOSER_BUTTON_DISABLED;
			mContinueButton.mOverOverlayImage = AtlasResources.IMAGE_SEEDCHOOSER_BUTTON_GLOW;
			mContinueButton.SetFont(Resources.FONT_DWARVENTODCRAFT15);
			mContinueButton.mColors[0] = new SexyColor(213, 159, 43);
			mContinueButton.mColors[1] = new SexyColor(213, 159, 43);
			mContinueButton.mTextOffsetY = -1;
			mContinueButton.mParentWidget = this;
			mContinueButton.mBtnNoDraw = true;
			mContinueButton.mDisabled = true;
			mMenuButton = new GameButton(101, this);
			mMenuButton.SetLabel("[AWARD_MAIN_MENU_BUTTON]");
			mMenuButton.mButtonImage = AtlasResources.IMAGE_SEEDCHOOSER_BUTTON2;
			mMenuButton.mOverImage = AtlasResources.IMAGE_SEEDCHOOSER_BUTTON2_GLOW;
			mMenuButton.mDownImage = null;
			mMenuButton.SetFont(Resources.FONT_BRIANNETOD12);
			mMenuButton.mColors[0] = new SexyColor(42, 42, 90);
			mMenuButton.mColors[1] = new SexyColor(42, 42, 90);
			mMenuButton.mParentWidget = this;
			mMenuButton.mTextOffsetY = (int)Constants.InvertAndScale(2f);
			if (mApp.IsFirstTimeAdventureMode() && level <= 3)
			{
				mMenuButton.mBtnNoDraw = true;
				mMenuButton.mDisabled = true;
			}
			mCreditsButton = null;
			if (mAwardType == AwardType.AWARD_CREDITS_ZOMBIE_NOTE)
			{
				mCreditsButton = GameButton.MakeNewButton(100, this, "[ROLL_CREDITS]", Resources.FONT_HOUSEOFTERROR16, AtlasResources.IMAGE_CREDITS_PLAYBUTTON, null, null);
				mCreditsButton.mTextDownOffsetX = 1;
				mCreditsButton.mTextDownOffsetY = 1;
				mCreditsButton.mColors[0] = new SexyColor(255, 255, 255);
				mCreditsButton.mColors[1] = new SexyColor(213, 159, 43);
				mCreditsButton.Resize(Constants.AwardScreen_CreditsButton.mX, Constants.AwardScreen_CreditsButton.mY, Constants.AwardScreen_CreditsButton.mWidth, Constants.AwardScreen_CreditsButton.mHeight);
				mCreditsButton.mTextOffsetX = Constants.AwardScreen_CreditsButton_Offset.X;
				mCreditsButton.mTextOffsetY = Constants.AwardScreen_CreditsButton_Offset.Y;
				AddWidget(mCreditsButton);
				mStartButton.mBtnNoDraw = true;
				mStartButton.mDisabled = true;
			}
			else if (mAwardType == AwardType.AWARD_HELP_ZOMBIE_NOTE)
			{
				mStartButton.SetLabel("[MAIN_MENU_BUTTON]");
				mMenuButton.mBtnNoDraw = true;
				mMenuButton.mDisabled = true;
			}
			else if (!mApp.IsAdventureMode())
			{
				mStartButton.SetLabel("[MAIN_MENU_BUTTON]");
				mMenuButton.mBtnNoDraw = true;
				mMenuButton.mDisabled = true;
			}
			else if (level == 1 && mApp.HasFinishedAdventure())
			{
				mStartButton.SetLabel("[CONTINUE_BUTTON]");
				mMenuButton.mBtnNoDraw = true;
				mMenuButton.mDisabled = true;
			}
			else
			{
				switch (level)
				{
				case 15:
					mStartButton.SetLabel("[VIEW_ALMANAC_BUTTON]");
					break;
				case 25:
				case 35:
				case 45:
					mStartButton.SetLabel("[CONTINUE_BUTTON]");
					break;
				default:
					mStartButton.SetLabel("[NEXT_LEVEL_BUTTON]");
					break;
				}
			}
			if (mApp.IsFirstTimeAdventureMode() && level == 25 && mApp.IsTrialStageLocked() && !mApp.mPlayerInfo.mHasSeenUpsell)
			{
				mMenuButton.mBtnNoDraw = true;
				mMenuButton.mDisabled = true;
			}
			if (mShowingAchievements)
			{
				mShowStartButtonAfterAchievements = !mStartButton.mBtnNoDraw;
				mShowMenuButtonAfterAchievements = !mMenuButton.mBtnNoDraw;
				mStartButton.mBtnNoDraw = true;
				mStartButton.mDisabled = true;
				mMenuButton.mBtnNoDraw = true;
				mMenuButton.mDisabled = true;
				mContinueButton.SetLabel("[CONTINUE_BUTTON]");
			}
			if (IsPaperNote())
			{
				mApp.mMusic.StopAllMusic();
				mStartButton.mY += 20;
				mMenuButton.mY += 20;
				mApp.PlayFoley(FoleyType.FOLEY_PAPER);
			}
			else
			{
				mApp.mMusic.StopAllMusic();
			}
		}

		public override void Resize(int theX, int theY, int theWidth, int theHeight)
		{
			base.Resize(theX, theY, theWidth, theHeight);
			int width = mStartButton.mButtonImage.GetWidth();
			int height = mStartButton.mButtonImage.GetHeight();
			mStartButton.Resize(mWidth / 2 - width / 2, mHeight - height - (int)(Constants.S * (float)Constants.AwardScreen_ContinueButton_Offset), width, height);
			mStartButton.mTextOffsetY = (int)Constants.InvertAndScale(1f);
			mStartButton.mParentWidget = this;
			width = mContinueButton.mButtonImage.GetWidth();
			height = mContinueButton.mButtonImage.GetHeight();
			mContinueButton.Resize(mWidth / 2 - width / 2, mHeight - height - (int)(Constants.S * (float)Constants.AwardScreen_ContinueButton_Offset), width, height);
			mMenuButton.Resize(Constants.AwardScreen_MenuButton.X, Constants.AwardScreen_MenuButton.Y, AtlasResources.IMAGE_SEEDCHOOSER_BUTTON2.mWidth, AtlasResources.IMAGE_SEEDCHOOSER_BUTTON2.mHeight);
		}

		public override void Dispose()
		{
			mApp.DelayLoadBackgroundResource(string.Empty);
			mApp.DelayLoadZombieNotePaperResource(string.Empty);
			mApp.DelayLoadZombieNoteResource(string.Empty);
			mStartButton.Dispose();
			mMenuButton.Dispose();
			RemoveAllWidgets();
		}

		public override bool BackButtonPress()
		{
			if (mAwardType == AwardType.AWARD_HELP_ZOMBIE_NOTE)
			{
				mApp.KillAwardScreen();
				mApp.ShowGameSelectorWithOptions();
				return true;
			}
			if (mAwardType == AwardType.AWARD_FOR_LEVEL || mAwardType == AwardType.AWARD_PRE_CREDITS_ZOMBIE_NOTE || mAwardType == AwardType.AWARD_CREDITS_ZOMBIE_NOTE)
			{
				mApp.KillAwardScreen();
				mApp.ShowGameSelector();
				return true;
			}
			ButtonPress(mStartButton.mId);
			ButtonDepress(mStartButton.mId);
			return true;
		}

		public override void Update()
		{
			base.Update();
			if (mShowingAchievements)
			{
				mAchievementAnimTime++;
				for (int i = 0; i < mAchievementItems.Count; i++)
				{
					if (mAchievementAnimTime >= mAchievementItems[i].mStartAnimTime)
					{
						bool flag = mAchievementItems[i].mY == mAchievementItems[i].mDestY;
						mAchievementItems[i].mY = TodCommon.TodAnimateCurve(mAchievementItems[i].mStartAnimTime, mAchievementItems[i].mEndAnimTime, mAchievementAnimTime, mAchievementItems[i].mStartY, mAchievementItems[i].mDestY, TodCurves.CURVE_EASE_IN_OUT);
						if (!flag && mAchievementItems[i].mY == mAchievementItems[i].mDestY)
						{
							mApp.PlaySample(Resources.SOUND_ACHIEVEMENT);
						}
					}
				}
				if (mAchievementItems[mAchievementItems.Count - 1].mY == mAchievementItems[mAchievementItems.Count - 1].mDestY)
				{
					mContinueButton.mBtnNoDraw = false;
					mContinueButton.mDisabled = false;
				}
			}
			if (mApp.GetDialogCount() > 0)
			{
				return;
			}
			mStartButton.Update();
			mMenuButton.Update();
			mContinueButton.Update();
			if (mFadeInCounter > 0)
			{
				mFadeInCounter -= 3;
				if (mFadeInCounter < 0)
				{
					mFadeInCounter = 0;
				}
			}
		}

		public override void Draw(Graphics g)
		{
			if (mApp.GetDialog(Dialogs.DIALOG_STORE) != null)
			{
				return;
			}
			int level = mApp.mPlayerInfo.GetLevel();
			g.SetLinearBlend(true);
			if (mShowingAchievements)
			{
				DrawAchievements(g);
			}
			else if (mAwardType == AwardType.AWARD_CREDITS_ZOMBIE_NOTE)
			{
				g.SetColor(new SexyColor(125, 200, 255, 255));
				g.SetColorizeImages(true);
				g.DrawImage(Resources.IMAGE_BACKGROUND6BOSS, Constants.AwardScreen_Note_Credits_Background.X, Constants.AwardScreen_Note_Credits_Background.Y, Constants.AwardScreen_Note_Credits_Background.Width, Constants.AwardScreen_Note_Credits_Background.Height);
				g.SetColorizeImages(false);
				g.DrawImage(Resources.IMAGE_ZOMBIE_NOTE, Constants.AwardScreen_Note_Credits_Paper.X, Constants.AwardScreen_Note_Credits_Paper.Y);
				g.DrawImage(Resources.IMAGE_CREDITS_ZOMBIENOTE, Constants.AwardScreen_Note_Credits_Text.X, Constants.AwardScreen_Note_Credits_Text.Y, Constants.AwardScreen_Note_Credits_Text.Width, Constants.AwardScreen_Note_Credits_Text.Height);
			}
			else if (mAwardType == AwardType.AWARD_HELP_ZOMBIE_NOTE)
			{
				g.DrawImage(Resources.IMAGE_BACKGROUND1, Constants.AwardScreen_Note_Help_Background.X, Constants.AwardScreen_Note_Help_Background.Y, Constants.AwardScreen_Note_Help_Background.Width, Constants.AwardScreen_Note_Help_Background.Height);
				g.DrawImage(Resources.IMAGE_ZOMBIE_NOTE, Constants.AwardScreen_Note_Help_Paper.X, Constants.AwardScreen_Note_Help_Paper.Y);
				g.DrawImage(Resources.IMAGE_ZOMBIE_NOTE_HELP, Constants.AwardScreen_Note_Help_Text.X, Constants.AwardScreen_Note_Help_Text.Y);
			}
			else if (mAwardType != AwardType.AWARD_ACHIEVEMENT_ONLY)
			{
				if (!mApp.IsAdventureMode())
				{
					if (mApp.EarnedGoldTrophy())
					{
						DrawBottom(g, "[BEAT_GAME_MESSAGE1]", "[GOLD_SUNFLOWER_TROPHY]", "[BEAT_GAME_MESSAGE2]");
					}
					else if (mApp.IsSurvivalMode())
					{
						int numTrophies = mApp.GetNumTrophies(ChallengePage.CHALLENGE_PAGE_SURVIVAL);
						if (numTrophies <= 7)
						{
							DrawBottom(g, "[GOT_TROPHY]", "[TROPHY]", "[YOU_UNLOCKED_A_SURVIVAL]");
							g.DrawImage(AtlasResources.IMAGE_TROPHY_HI_RES, Constants.AwardScreen_Trophy.X - AtlasResources.IMAGE_TROPHY_HI_RES.mWidth / 2, Constants.AwardScreen_Trophy.Y);
						}
						else if (numTrophies == 10)
						{
							DrawBottom(g, "[GOT_TROPHY]", "[TROPHY]", "[YOU_UNLOCKED_ENDLESS_SURVIVAL]");
							g.DrawImage(AtlasResources.IMAGE_TROPHY_HI_RES, Constants.AwardScreen_Trophy.X - AtlasResources.IMAGE_TROPHY_HI_RES.mWidth / 2, Constants.AwardScreen_Trophy.Y);
						}
						else
						{
							DrawBottom(g, "[GOT_TROPHY]", "[TROPHY]", "[EARN_MORE_TROPHIES_FOR_ENDLESS_SURVIVAL]");
							g.DrawImage(AtlasResources.IMAGE_TROPHY_HI_RES, Constants.AwardScreen_Trophy.X - AtlasResources.IMAGE_TROPHY_HI_RES.mWidth / 2, Constants.AwardScreen_Trophy.Y);
						}
					}
					else if (mApp.IsScaryPotterLevel())
					{
						DrawBottom(g, "[GOT_TROPHY]", "[TROPHY]", "[UNLOCKED_VASEBREAKER_LEVEL]");
						g.DrawImage(AtlasResources.IMAGE_TROPHY_HI_RES, Constants.AwardScreen_Trophy.X - AtlasResources.IMAGE_TROPHY_HI_RES.mWidth / 2, Constants.AwardScreen_Trophy.Y);
					}
					else if (mApp.IsPuzzleMode())
					{
						DrawBottom(g, "[GOT_TROPHY]", "[TROPHY]", "[UNLOCKED_I_ZOMBIE_LEVEL]");
						g.DrawImage(AtlasResources.IMAGE_TROPHY_HI_RES, Constants.AwardScreen_Trophy.X - AtlasResources.IMAGE_TROPHY_HI_RES.mWidth / 2, Constants.AwardScreen_Trophy.Y);
					}
					else
					{
						int numTrophies2 = mApp.GetNumTrophies(ChallengePage.CHALLENGE_PAGE_CHALLENGE);
						if (numTrophies2 <= 17)
						{
							DrawBottom(g, "[GOT_TROPHY]", "[TROPHY]", "[CHALLENGE_UNLOCKED]");
							g.DrawImage(AtlasResources.IMAGE_TROPHY_HI_RES, Constants.AwardScreen_Trophy.X - AtlasResources.IMAGE_TROPHY_HI_RES.mWidth / 2, Constants.AwardScreen_Trophy.Y);
						}
						else
						{
							DrawBottom(g, "[GOT_TROPHY]", "[TROPHY]", "[GET_MORE_TROPHIES]");
							g.DrawImage(AtlasResources.IMAGE_TROPHY_HI_RES, Constants.AwardScreen_Trophy.X - AtlasResources.IMAGE_TROPHY_HI_RES.mWidth / 2, Constants.AwardScreen_Trophy.Y);
						}
					}
				}
				else if (level == 5)
				{
					DrawBottom(g, "[GOT_SHOVEL]", "[SHOVEL]", "[SHOVEL_DESCRIPTION]");
					g.DrawImage(AtlasResources.IMAGE_SHOVEL_HI_RES, Constants.AwardScreen_Shovel.X - AtlasResources.IMAGE_SHOVEL_HI_RES.mWidth / 2, Constants.AwardScreen_Shovel.Y);
				}
				else if (level == 10)
				{
					g.DrawImage(Resources.IMAGE_BACKGROUND1, Constants.AwardScreen_Note_1_Background.X, Constants.AwardScreen_Note_1_Background.Y, Constants.AwardScreen_Note_1_Background.Width, Constants.AwardScreen_Note_1_Background.Height);
					g.DrawImage(Resources.IMAGE_ZOMBIE_NOTE, Constants.AwardScreen_Note_1_Paper.X, Constants.AwardScreen_Note_1_Paper.Y);
					g.DrawImage(Resources.IMAGE_ZOMBIE_NOTE1, Constants.AwardScreen_Note_1_Text.X, Constants.AwardScreen_Note_1_Text.Y);
					TodCommon.TodDrawString(g, "[FOUND_NOTE]", Constants.AwardScreen_Note_Message.X, Constants.AwardScreen_Note_Message.Y, Resources.FONT_DWARVENTODCRAFT15, new SexyColor(255, 200, 0, 255), DrawStringJustification.DS_ALIGN_CENTER);
				}
				else if (level == 15)
				{
					DrawBottom(g, "[FOUND_SUBURBAN_ALMANAC]", "[SUBURBAN_ALMANAC]", "[SUBURBAN_ALMANAC_DESCRIPTION]");
					g.DrawImage(AtlasResources.IMAGE_ALMANAC, Constants.AwardScreen_Almanac.X - AtlasResources.IMAGE_ALMANAC.mWidth / 2, Constants.AwardScreen_Almanac.Y);
				}
				else if (level == 20)
				{
					g.DrawImage(Resources.IMAGE_BACKGROUND2, Constants.AwardScreen_Note_2_Background.X, Constants.AwardScreen_Note_2_Background.Y, Constants.AwardScreen_Note_2_Background.Width, Constants.AwardScreen_Note_2_Background.Height);
					g.DrawImage(Resources.IMAGE_ZOMBIE_NOTE, Constants.AwardScreen_Note_2_Paper.X, Constants.AwardScreen_Note_2_Paper.Y);
					g.DrawImage(Resources.IMAGE_ZOMBIE_NOTE2, Constants.AwardScreen_Note_2_Text.X, Constants.AwardScreen_Note_2_Text.Y);
					TodCommon.TodDrawString(g, "[FOUND_NOTE]", Constants.AwardScreen_Note_Message.X, Constants.AwardScreen_Note_Message.Y, Resources.FONT_DWARVENTODCRAFT15, new SexyColor(255, 200, 0, 255), DrawStringJustification.DS_ALIGN_CENTER);
				}
				else if (level == 25)
				{
					DrawBottom(g, "[FOUND_KEYS]", "[KEYS]", "[KEYS_DESCRIPTION]");
					g.DrawImage(AtlasResources.IMAGE_CARKEYS, Constants.AwardScreen_CarKeys.X - AtlasResources.IMAGE_CARKEYS.mWidth / 2, Constants.AwardScreen_CarKeys.Y);
				}
				else if (level == 30)
				{
					g.DrawImage(Resources.IMAGE_BACKGROUND1, Constants.AwardScreen_Note_3_Background.X, Constants.AwardScreen_Note_3_Background.Y, Constants.AwardScreen_Note_3_Background.Width, Constants.AwardScreen_Note_3_Background.Height);
					g.DrawImage(Resources.IMAGE_ZOMBIE_NOTE, Constants.AwardScreen_Note_3_Paper.X, Constants.AwardScreen_Note_3_Paper.Y);
					g.DrawImage(Resources.IMAGE_ZOMBIE_NOTE3, Constants.AwardScreen_Note_3_Text.X, Constants.AwardScreen_Note_3_Text.Y);
					TodCommon.TodDrawString(g, "[FOUND_NOTE]", Constants.AwardScreen_Note_Message.X, Constants.AwardScreen_Note_Message.Y, Resources.FONT_DWARVENTODCRAFT15, new SexyColor(255, 200, 0, 255), DrawStringJustification.DS_ALIGN_CENTER);
				}
				else if (level == 35)
				{
					DrawBottom(g, "[FOUND_TACO]", "[TACO]", "[TACO_DESCRIPTION]");
					g.DrawImage(AtlasResources.IMAGE_TACO, Constants.AwardScreen_Taco.X - AtlasResources.IMAGE_TACO.mWidth / 2, Constants.AwardScreen_Taco.Y);
				}
				else if (level == 40)
				{
					g.DrawImage(Resources.IMAGE_BACKGROUND2, Constants.AwardScreen_Note_4_Background.X, Constants.AwardScreen_Note_4_Background.Y, Constants.AwardScreen_Note_4_Background.Width, Constants.AwardScreen_Note_4_Background.Height);
					g.DrawImage(Resources.IMAGE_ZOMBIE_NOTE, Constants.AwardScreen_Note_4_Paper.X, Constants.AwardScreen_Note_4_Paper.Y);
					g.DrawImage(Resources.IMAGE_ZOMBIE_NOTE4, Constants.AwardScreen_Note_4_Text.X, Constants.AwardScreen_Note_4_Text.Y);
					TodCommon.TodDrawString(g, "[FOUND_NOTE]", Constants.AwardScreen_Note_Message.X, Constants.AwardScreen_Note_Message.Y, Resources.FONT_DWARVENTODCRAFT15, new SexyColor(255, 200, 0, 255), DrawStringJustification.DS_ALIGN_CENTER);
				}
				else if (level == 45)
				{
					DrawBottom(g, "[FOUND_WATERING_CAN]", "[WATERING_CAN]", "[WATERING_CAN_DESCRIPTION]");
					g.DrawImage(AtlasResources.IMAGE_REANIM_ZENGARDEN_WATERINGCAN1, Constants.AwardScreen_WateringCan.X - AtlasResources.IMAGE_REANIM_ZENGARDEN_WATERINGCAN1.mWidth / 2, Constants.AwardScreen_WateringCan.Y);
				}
				else if (level == 50)
				{
					g.DrawImage(Resources.IMAGE_BACKGROUND1, Constants.AwardScreen_Note_Final_Background.X, Constants.AwardScreen_Note_Final_Background.Y, Constants.AwardScreen_Note_Final_Background.Width, Constants.AwardScreen_Note_Final_Background.Height);
					g.DrawImage(Resources.IMAGE_ZOMBIE_NOTE, Constants.AwardScreen_Note_Final_Paper.X, Constants.AwardScreen_Note_Final_Paper.Y);
					g.DrawImage(Resources.IMAGE_ZOMBIE_FINAL_NOTE, Constants.AwardScreen_Note_Final_Text.X, Constants.AwardScreen_Note_Final_Text.Y);
					TodCommon.TodDrawString(g, "[FOUND_NOTE]", Constants.AwardScreen_Note_Message.X, Constants.AwardScreen_Note_Message.Y, Resources.FONT_DWARVENTODCRAFT15, new SexyColor(255, 200, 0, 255), DrawStringJustification.DS_ALIGN_CENTER);
				}
				else if ((level == 1 && mApp.HasFinishedAdventure()) || mAwardType == AwardType.AWARD_PRE_CREDITS_ZOMBIE_NOTE)
				{
					DrawBottom(g, "[WIN_MESSAGE1]", "", "[WIN_MESSAGE2]");
				}
				else
				{
					DrawAwardSeed(g);
				}
			}
			mStartButton.Draw(g);
			mMenuButton.Draw(g);
			mContinueButton.Draw(g);
			DeferOverlay();
		}

		public override void DrawOverlay(Graphics g)
		{
			base.DrawOverlay(g);
			int theAlpha = TodCommon.TodAnimateCurve(180, 0, mFadeInCounter, 255, 0, TodCurves.CURVE_LINEAR);
			if (IsPaperNote())
			{
				g.SetColor(new SexyColor(0, 0, 0, theAlpha, false));
			}
			else
			{
				g.SetColor(new SexyColor(255, 255, 255, theAlpha, false));
			}
			g.SetColorizeImages(true);
			g.FillRect(0, 0, Constants.BOARD_WIDTH, Constants.BOARD_HEIGHT);
		}

		public virtual void KeyChar(char theChar)
		{
			if (theChar == ' ' || theChar == '\r' || theChar == '\u001b')
			{
				StartButtonPressed();
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
		}

		public override void MouseUp(int x, int y, int theClickCount)
		{
			if (theClickCount == 1)
			{
				if (mStartButton.IsMouseOver())
				{
					StartButtonPressed();
				}
				if (mContinueButton.IsMouseOver())
				{
					AchievementsContinuePressed();
				}
				if (mMenuButton.IsMouseOver())
				{
					mApp.KillAwardScreen();
					mApp.ShowGameSelector();
				}
			}
		}

		public override void MouseDown(int x, int y, int theClickCount)
		{
			if (theClickCount == 1)
			{
				mStartButton.Update();
				mMenuButton.Update();
				mContinueButton.Update();
				if (mStartButton.IsMouseOver())
				{
					mApp.PlaySample(Resources.SOUND_TAP);
				}
				if (mMenuButton.IsMouseOver())
				{
					mApp.PlaySample(Resources.SOUND_TAP);
				}
				if (mContinueButton.IsMouseOver())
				{
					mApp.PlaySample(Resources.SOUND_TAP);
				}
			}
		}

		public void DrawAwardSeed(Graphics g)
		{
			int level = mApp.mPlayerInfo.GetLevel();
			SeedType awardSeedForLevel = mApp.GetAwardSeedForLevel(level - 1);
			string nameString = Plant.GetNameString(awardSeedForLevel, SeedType.SEED_NONE);
			string empty = string.Empty;
			empty = ((!mApp.IsTrialStageLocked() || awardSeedForLevel < SeedType.SEED_SQUASH || awardSeedForLevel == SeedType.SEED_TANGLEKELP) ? Plant.GetToolTip(awardSeedForLevel) : "[AVAILABLE_IN_FULL_VERSION]");
			DrawBottom(g, "[NEW_PLANT]", nameString, empty);
			int x = Constants.AwardScreen_Seed_Pos.X;
			int y = Constants.AwardScreen_Seed_Pos.Y;
			SeedPacket.DrawSmallSeedPacket(g, x, y, awardSeedForLevel, SeedType.SEED_NONE, 0f, 255, true, false, true, true);
		}

		public void DrawAchievements(Graphics g)
		{
			LawnCommon.DrawImageBox(g, new TRect(0, 0, mWidth, mHeight), AtlasResources.IMAGE_ALMANAC_ROUNDED_OUTLINE);
			for (int i = 0; i < mAchievementItems.Count; i++)
			{
				AchievementItem achievementItem = Achievements.GetAchievementItem(mAchievementItems[i].mId);
				g.DrawImage(achievementItem.AchievementImage, Constants.AwardScreen_AchievementImage_Pos.X, mAchievementItems[i].mY + Constants.AwardScreen_AchievementImage_Pos.Y);
				g.SetFont(Resources.FONT_DWARVENTODCRAFT15);
				g.SetColor(new SexyColor(224, 187, 98));
				string theString = TodCommon.TodReplaceString("[ACHIEVEMENT_EARNED]", "{ACHIEVEMENT}", mApp.GetAchievementName(mAchievementItems[i].mId));
				g.DrawString(theString, Constants.AwardScreen_AchievementName_Pos.X, mAchievementItems[i].mY + Constants.AwardScreen_AchievementName_Pos.Y);
				g.SetFont(Resources.FONT_DWARVENTODCRAFT12);
				g.SetColor(new SexyColor(255, 255, 255));
				TRect theRect = new TRect(Constants.AwardScreen_AchievementDescription_Rect.X, mAchievementItems[i].mY + Constants.AwardScreen_AchievementDescription_Rect.Y, Constants.AwardScreen_AchievementDescription_Rect.Width, Constants.AwardScreen_AchievementDescription_Rect.Height);
				g.WriteWordWrapped(theRect, mApp.GetAchievementDescription(mAchievementItems[i].mId), 0, -1, true);
			}
		}

		public void DrawBottom(Graphics g, string theTitle, string theAward, string theMessage)
		{
			LawnCommon.DrawImageBox(g, new TRect(0, 0, mWidth, mHeight), AtlasResources.IMAGE_ALMANAC_ROUNDED_OUTLINE);
			LawnCommon.DrawImageBox(g, Constants.AwardScreen_ClayTablet, AtlasResources.IMAGE_ALMANAC_CLAY_TABLET);
			TodCommon.TodDrawString(g, theTitle, Constants.AwardScreen_TitlePos.X, Constants.AwardScreen_TitlePos.Y, Resources.FONT_DWARVENTODCRAFT18, Constants.GreenFontColour, Constants.BOARD_WIDTH - 60, DrawStringJustification.DS_ALIGN_CENTER);
			TRect tRect = default(TRect);
			if (!theAward.empty())
			{
				g.DrawImage(AtlasResources.IMAGE_ALMANAC_GROUNDDAY, Constants.AwardScreen_GroundDay_Pos.X, Constants.AwardScreen_GroundDay_Pos.Y);
				LawnCommon.DrawImageBox(g, Constants.AwardScreen_BrownRect, AtlasResources.IMAGE_ALMANAC_BROWN_RECT, false);
				TodCommon.TodDrawString(g, theAward, Constants.AwardScreen_BottomMessage_Pos.X, Constants.AwardScreen_BottomMessage_Pos.Y, Resources.FONT_DWARVENTODCRAFT18, Constants.YellowFontColour, DrawStringJustification.DS_ALIGN_CENTER);
				tRect = Constants.AwardScreen_BottomText_Rect_Text;
			}
			else
			{
				tRect = Constants.AwardScreen_BottomText_Rect_NoText;
			}
			g.SetColorizeImages(true);
			g.SetColor(new SexyColor(253, 186, 117));
			LawnCommon.DrawImageBox(g, tRect, AtlasResources.IMAGE_ALMANAC_PAPER);
			g.SetColorizeImages(false);
			tRect.Inflate((int)Constants.InvertAndScale(-6f), (int)Constants.InvertAndScale(-6f));
			TodStringFile.TodDrawStringWrapped(g, theMessage, tRect, Resources.FONT_BRIANNETOD16, new SexyColor(40, 50, 90), DrawStringJustification.DS_ALIGN_CENTER_VERTICAL_MIDDLE, true);
		}

		public void StartButtonPressed()
		{
			if (mApp.GetDialog(4) != null)
			{
				return;
			}
			if (mAwardType == AwardType.AWARD_PRE_CREDITS_ZOMBIE_NOTE)
			{
				mApp.KillAwardScreen();
				mApp.ShowAwardScreen(AwardType.AWARD_CREDITS_ZOMBIE_NOTE, false);
				return;
			}
			if (mAwardType == AwardType.AWARD_CREDITS_ZOMBIE_NOTE)
			{
				mApp.KillAwardScreen();
				mApp.ShowCreditScreen();
				return;
			}
			if (mAwardType == AwardType.AWARD_HELP_ZOMBIE_NOTE)
			{
				mApp.KillAwardScreen();
				mApp.ShowGameSelector();
				return;
			}
			if (mApp.IsSurvivalMode())
			{
				mApp.KillAwardScreen();
				mApp.ShowChallengeScreen(ChallengePage.CHALLENGE_PAGE_SURVIVAL);
				return;
			}
			if (mApp.IsPuzzleMode())
			{
				mApp.KillAwardScreen();
				mApp.ShowChallengeScreen(ChallengePage.CHALLENGE_PAGE_PUZZLE);
				return;
			}
			if (mApp.IsChallengeMode())
			{
				mApp.KillAwardScreen();
				mApp.ShowChallengeScreen(ChallengePage.CHALLENGE_PAGE_CHALLENGE);
				return;
			}
			switch (mApp.mPlayerInfo.GetLevel())
			{
			case 15:
				mApp.DoAlmanacDialog(SeedType.SEED_NONE, ZombieType.ZOMBIE_INVALID, this);
				break;
			case 25:
			{
				StoreScreen storeScreen3 = mApp.ShowStoreScreen(this);
				storeScreen3.SetupForIntro(301);
				break;
			}
			case 35:
			{
				StoreScreen storeScreen2 = mApp.ShowStoreScreen(this);
				storeScreen2.SetupForIntro(601);
				break;
			}
			case 42:
			{
				StoreScreen storeScreen = mApp.ShowStoreScreen(this);
				storeScreen.SetupForIntro(3100);
				break;
			}
			case 45:
				mApp.KillAwardScreen();
				mApp.WriteCurrentUserConfig();
				mApp.PreNewGame(GameMode.GAMEMODE_CHALLENGE_ZEN_GARDEN, false);
				mApp.mZenGarden.SetupForZenTutorial();
				break;
			default:
				mApp.KillAwardScreen();
				mApp.PreNewGame(GameMode.GAMEMODE_ADVENTURE, false);
				break;
			}
		}

		public void AchievementsContinuePressed()
		{
			if (mAwardType == AwardType.AWARD_ACHIEVEMENT_ONLY)
			{
				mApp.KillAwardScreen();
				if (mApp.IsQuickPlayMode())
				{
					mApp.ShowGameSelectorQuickPlay(false);
				}
				else
				{
					mApp.PreNewGame(GameMode.GAMEMODE_ADVENTURE, false);
				}
				return;
			}
			mStartButton.mBtnNoDraw = !mShowStartButtonAfterAchievements;
			mStartButton.mDisabled = !mShowStartButtonAfterAchievements;
			mMenuButton.mBtnNoDraw = !mShowMenuButtonAfterAchievements;
			mMenuButton.mDisabled = !mShowMenuButtonAfterAchievements;
			mContinueButton.mDisabled = true;
			mContinueButton.mBtnNoDraw = true;
			mShowingAchievements = false;
			int level = mApp.mPlayerInfo.GetLevel();
			if (level == 1 && mApp.HasFinishedAdventure())
			{
				mApp.KillAwardScreen();
				mApp.ShowAwardScreen(AwardType.AWARD_CREDITS_ZOMBIE_NOTE, false);
			}
		}

		public bool IsPaperNote()
		{
			if (mAwardType == AwardType.AWARD_CREDITS_ZOMBIE_NOTE || mAwardType == AwardType.AWARD_HELP_ZOMBIE_NOTE)
			{
				return true;
			}
			if (mApp.IsAdventureMode())
			{
				int level = mApp.mPlayerInfo.GetLevel();
				if (level == 10 || level == 20 || level == 30 || level == 40 || level == 50)
				{
					return true;
				}
			}
			return false;
		}

		public virtual void ButtonPress(int theId)
		{
			mApp.PlaySample(Resources.SOUND_TAP);
		}

		public virtual void ButtonDepress(int theId)
		{
			StartButtonPressed();
		}

		public void BackFromAlmanac()
		{
			mApp.KillAwardScreen();
			mApp.PreNewGame(GameMode.GAMEMODE_ADVENTURE, false);
		}

		public void BackFromStore()
		{
			mApp.KillDialog(4);
			mApp.KillAwardScreen();
			mApp.PreNewGame(GameMode.GAMEMODE_ADVENTURE, false);
		}

		public void ButtonMouseMove(int id, int x, int y)
		{
		}

		public void ButtonMouseLeave(int id)
		{
		}

		public void ButtonMouseEnter(int id)
		{
		}

		public void ButtonMouseTick(int id)
		{
		}

		public void ButtonPress(int id, int id2)
		{
		}

		public void ButtonDownTick(int id)
		{
		}
	}
}
