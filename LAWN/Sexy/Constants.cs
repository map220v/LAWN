using Lawn;
using Microsoft.Xna.Framework;

namespace Sexy
{
	internal class Constants : FrameworkConstants
	{
		public enum LanguageIndex
		{
			en = 1,
			fr,
			de,
			es,
			it
		}

		public const int PC_BOARD_WIDTH = 800;

		public const int PC_BOARD_HEIGHT = 600;

		public static bool Loaded;

		public static string ImageSubPath;

		public static DisplayOrientation SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;

		private static LanguageIndex mLanguage;

		public static string LanguageSubDir;

		public static SexyColor YellowFontColour = new SexyColor(227, 173, 57);

		public static SexyColor GreenFontColour = new SexyColor(0, 203, 0);

		public static SexyColor Almanac_Paper_Colour = new SexyColor(242, 182, 123);

		public static Point BackBufferSize;

		public static int BOARD_WIDTH;

		public static int BOARD_HEIGHT;

		public static int BOARD_EDGE;

		public static int BOARD_OFFSET;

		public static int BOARD_EXTRA_ROOM;

		public static int HIGH_GROUND_HEIGHT;

		public static int GRIDSIZEX;

		public static int MAX_GRIDSIZEY;

		public static int MORE_GAMES_ORIGIN_X;

		public static int MAIN_MENU_ORIGIN_X;

		public static int ACHIEVEMENTS_ORIGIN_X;

		public static int QUICKPLAY_ORIGIN_X;

		public static int MORE_GAMES_PLANK_HEIGHT;

		public static int MORE_GAMES_PLANK_WIDTH;

		public static int MORE_GAMES_ITEM_GAP;

		public static int SEED_CHOOSER_OFFSETSCREEN_OFFSET;

		public static int SMALL_SEEDPACKET_WIDTH;

		public static int SMALL_SEEDPACKET_HEIGHT;

		public static int DIALOG_HEADER_OFFSET;

		public static int WIDE_BOARD_WIDTH;

		public static int BACKGROUND_IMAGE_WIDTH;

		public static int LAWN_XMIN;

		public static int LAWN_YMIN;

		public static int SCROLL_AREA_OFFSET_X = 71;

		public static int SCROLL_AREA_OFFSET_Y = 28;

		public static int SEED_PACKET_HORIZ_GAP = 2;

		public static int SEED_PACKET_VERT_GAP = 1;

		public static Point SeedPacket_Selector_Pos;

		public static float S;

		public static float IS;

		public static int ReanimTextCenterOffsetX;

		public static int TitleScreen_ReanimStart_X;

		public static int GameSelector_Width;

		public static int GameSelector_Height;

		public static int GameSelector_AdventureButton_X;

		public static int GameSelector_MiniGameButton_X;

		public static int GameSelector_MiniGameButton_Y;

		public static int GameSelector_PuzzleButton_X;

		public static int GameSelector_PuzzleButton_Y;

		public static int GameSelector_OptionsButton_X;

		public static int GameSelector_OptionsButton_Y;

		public static int GameSelector_ZenGardenButton_X;

		public static int GameSelector_ZenGardenButton_Y;

		public static int GameSelector_AlmanacButton_X;

		public static int GameSelector_AlmanacButton_Y;

		public static int GameSelector_StoreButton_X;

		public static int GameSelector_StoreButton_Y;

		public static int GameSelector_AchievementsButton_X;

		public static int GameSelector_AchievementsButton_Y;

		public static int GameSelector_AchievementsStatue_X;

		public static int GameSelector_AchievementsStatue_Y;

		public static int GameSelector_MoreWaysToPlay_MiniGames_X;

		public static int GameSelector_MoreWaysToPlay_MiniGames_Y;

		public static int GameSelector_MoreWaysToPlay_VaseBreaker_X;

		public static int GameSelector_MoreWaysToPlay_VaseBreaker_Y;

		public static int GameSelector_MoreWaysToPlay_IZombie_X;

		public static int GameSelector_MoreWaysToPlay_IZombie_Y;

		public static int GameSelector_MoreWaysToPlay_Back_X;

		public static int GameSelector_MoreWaysToPlay_Back_Y;

		public static int GameSelector_LeaderboardButton_X;

		public static int GameSelector_LeaderboardButton_Y;

		public static Point GameSelector_PlayerName_Pos;

		public static Point GameSelector_LevelNumber_1_Pos;

		public static Point GameSelector_LevelNumber_2_Pos;

		public static Point GameSelector_LevelNumber_3_Pos;

		public static Rectangle GameSelector_LevelNumber_Bar;

		public static int GameSelector_LevelNumber_ButtonDown_Offset;

		public static int GameSelector_Update_Offset;

		public static int LeaderboardScreen_Vasebreaker_Button_X;

		public static int LeaderboardScreen_Vasebreaker_Button_Y;

		public static int LeaderboardScreen_IZombie_Button_X;

		public static int LeaderboardScreen_IZombie_Button_Y;

		public static int LeaderboardScreen_Killed_Button_X;

		public static int LeaderboardScreen_Killed_Button_Y;

		public static int LeaderboardScreen_PileBase_X;

		public static int LeaderboardScreen_PileBase_Y;

		public static int Leaderboard_IZombie_Score_X;

		public static int Leaderboard_IZombie_Score_Y;

		public static int Leaderboard_Vasebreaker_Score_X;

		public static int Leaderboard_Vasebreaker_Score_Y;

		public static int Leaderboard_Pile_1_X;

		public static TRect LeaderboardDialog_CannotConnect_Rect;

		public static int LeaderboardScreen_EdgeOfSpace_Overlay_Offset;

		public static int AchievementWidget_ROW_HEIGHT;

		public static int AchievementWidget_ROW_START;

		public static Point AchievementWidget_GAMERSCORE_POS;

		public static int AchievementWidget_HOLE_DEPTH;

		public static int AchievementWidget_BackButton_X;

		public static int AchievementWidget_BackButton_Y;

		public static TRect AchievementWidget_BackButton_Rect;

		public static int AchievementWidget_Background_Offset_Y;

		public static Rectangle AchievementWidget_Description_Box;

		public static Point AchievementWidget_Pipe_Offset;

		public static Point AchievementWidget_Worm_Offset;

		public static Point AchievementWidget_ZombieWorm_Offset;

		public static Point AchievementWidget_GemLeft_Offset;

		public static Point AchievementWidget_GemRight_Offset;

		public static Point AchievementWidget_Fossile_Offset;

		public static Point AchievementWidget_Image_Pos;

		public static int AchievementWidget_Image_Size;

		public static Point AchievementWidget_Name_Pos;

		public static int AchievementWidget_Name_MaxWidth;

		public static int QuickPlayWidget_Thumb_X;

		public static int QuickPlayWidget_Thumb_Y;

		public static int QuickPlayWidget_Bungee_Y;

		public static int SeedChooserScreen_MenuButton_X;

		public static Point SeedChooserScreen_Background_Top;

		public static Point SeedChooserScreen_Background_Middle;

		public static int SeedChooserScreen_Background_Middle_Height;

		public static Point SeedChooserScreen_Background_Bottom;

		public static Rectangle SeedChooserScreen_Gradient_Top;

		public static Rectangle SeedChooserScreen_Gradient_Bottom;

		public static Color SeedChooserScreen_BackColour = new Color(66, 32, 0);

		public static Point SeedPacket_Cost;

		public static Point SeedPacket_Cost_IZombie;

		public static Point SeedPacket_CostText_Pos;

		public static Point SeedPacket_CostText_IZombie_Pos;

		public static Point ImitaterDialog_Size;

		public static int ImitaterDialog_ScrollWidget_Offset_X;

		public static int ImitaterDialog_ScrollWidget_Y;

		public static int ImitaterDialog_ScrollWidget_ExtraWidth;

		public static int ImitaterDialog_Height;

		public static int ImitaterDialog_BottomGradient_Y;

		public static TRect ConveyorBeltClipRect;

		public static Point CutScene_ReadySetPlant_Pos;

		public static int CutScene_LogoEndPos;

		public static int CutScene_LogoBackRect_Height;

		public static Point CutScene_LogoEnd_Particle_Pos;

		public static Point CutScene_ExtraRoom_1_Particle_Pos;

		public static Point CutScene_ExtraRoom_2_Particle_Pos;

		public static Point CutScene_ExtraRoom_3_Particle_Pos;

		public static Point CutScene_ExtraRoom_4_Particle_Pos;

		public static Point CutScene_ExtraRoom_5_Particle_Pos;

		public static int CutScene_SodRoll_1_Pos;

		public static int CutScene_SodRoll_2_Pos;

		public static Point CutScene_SodRoll_3_Pos;

		public static Point CutScene_SodRoll_4_Pos;

		public static Point CutScene_Upsell_TerraCotta_Arrow;

		public static Point CutScene_Upsell_TerraCotta_Pot;

		public static int StoreScreen_BackButton_X;

		public static int StoreScreen_BackButton_Y;

		public static int StoreScreen_Car_X;

		public static int StoreScreen_Car_Y;

		public static int StoreScreen_PrevButton_X;

		public static int StoreScreen_PrevButton_Y;

		public static int StoreScreen_NextButton_X;

		public static int StoreScreen_NextButton_Y;

		public static int StoreScreen_HatchOpen_X;

		public static int StoreScreen_HatchOpen_Y;

		public static int StoreScreen_HatchClosed_X;

		public static int StoreScreen_HatchClosed_Y;

		public static int StoreScreen_CarNight_X;

		public static int StoreScreen_CarNight_Y;

		public static int StoreScreen_StoreSign_X;

		public static int StoreScreen_StoreSign_Y_Min;

		public static int StoreScreen_StoreSign_Y_Max;

		public static int StoreScreen_Coinbank_X;

		public static int StoreScreen_Coinbank_Y;

		public static Point StoreScreen_Coinbank_TextOffset;

		public static int StoreScreen_ItemOffset_1_X;

		public static int StoreScreen_ItemOffset_1_Y;

		public static int StoreScreen_ItemOffset_2_X;

		public static int StoreScreen_ItemOffset_2_Y;

		public static int StoreScreen_ItemSize;

		public static int StoreScreen_ItemSize_Offset;

		public static int StoreScreen_PriceTag_X;

		public static int StoreScreen_PriceTag_Y;

		public static int StoreScreen_PriceTag_Text_Offset_X;

		public static int StoreScreen_PriceTag_Text_Offset_Y;

		public static int StoreScreen_ComingSoon_X;

		public static int StoreScreen_ComingSoon_Y;

		public static int StoreScreen_SoldOut_Width;

		public static int StoreScreen_SoldOut_Y;

		public static int StoreScreen_SoldOut_Height;

		public static int StoreScreen_PacketUpgrade_X;

		public static int StoreScreen_PacketUpgrade_Y;

		public static Rectangle StoreScreen_PacketUpgrade_Text_Size;

		public static int StoreScreen_RetardedDave_Offset_X;

		public static int StoreScreen_FirstAidNut_Offset_Y;

		public static int StoreScreen_PoolCleaner_Offset_X;

		public static int StoreScreen_PoolCleaner_Offset_Y;

		public static int StoreScreen_Rake_Offset_X;

		public static int StoreScreen_Rake_Offset_Y;

		public static int StoreScreen_RoofCleaner_Offset_X;

		public static int StoreScreen_RoofCleaner_Offset_Y;

		public static int StoreScreen_Imitater_Offset_X;

		public static int StoreScreen_Imitater_Offset_Y;

		public static int StoreScreen_Default_Offset_Y;

		public static Point StoreScreen_MouseRegion;

		public static Rectangle StoreScreen_Dialog;

		public static Point StoreScreen_PotPlant_Offset;

		public static int StoreScreenMushroomGardenOffsetX;

		public static int StoreScreenAquariumGardenOffsetX;

		public static int StoreScreenGloveOffsetY;

		public static int StoreScreenWheelbarrowOffsetY;

		public static int StoreScreenBugSprayOffsetX;

		public static int StoreScreenBugSprayOffsetY;

		public static int StoreScreenPhonographOffsetY;

		public static int StoreScreenPlantFoodOffsetX;

		public static int StoreScreenPlantFoodOffsetY;

		public static int StoreScreenWateringCanOffsetX;

		public static int StoreScreenWateringCanOffsetY;

		public static int NewOptionsDialog_FXLabel_X;

		public static int NewOptionsDialog_FXLabel_Y;

		public static int NewOptionsDialog_MusicLabel_X;

		public static int NewOptionsDialog_MusicLabel_On_Y;

		public static int NewOptionsDialog_MusicLabel_Off_Y;

		public static int NewOptionsDialog_VibrationLabel_X;

		public static int NewOptionsDialog_VibrationLabel_Y;

		public static int NewOptionsDialog_LockedLabel_Y;

		public static int NewOptionsDialog_VibrationLabel_MaxWidth;

		public static int NewOptionsDialog_FullScreenOffset;

		public static int NewOptionsDialog_FX_Offset;

		public static int NewOptionsDialog_Music_Offset;

		public static int NewOptionsDialog_Version_Low_Y;

		public static int NewOptionsDialog_Version_High_Y;

		public static int RetardedDave_Bubble_Tip_Offset;

		public static Point RetardedDave_Bubble_Offset_Shop;

		public static Point RetardedDave_Bubble_Offset_Board;

		public static int RetardedDave_Bubble_Size;

		public static TRect RetardedDave_Bubble_Rect;

		public static int RetardedDave_Bubble_TapToContinue_Y;

		public static int AlmanacHeaderY;

		public static TRect Almanac_PlantsButtonRect;

		public static TRect Almanac_ZombiesButtonRect;

		public static TRect Almanac_CloseButtonRect;

		public static TRect Almanac_IndexButtonRect;

		public static Point Almanac_IndexPlantPos;

		public static Point Almanac_IndexZombiePos;

		public static Point Almanac_IndexPlantTextPos;

		public static Point Almanac_IndexZombieTextPos;

		public static TRect Almanac_PlantScrollRect;

		public static TRect Almanac_ZombieScrollRect;

		public static TRect Almanac_DescriptionScrollRect;

		public static int Almanac_PlantTopGradientY;

		public static int Almanac_ZombieTopGradientY;

		public static int Almanac_BottomGradientY;

		public static int Almanac_PlantGradientWidth;

		public static int Almanac_ZombieGradientWidth;

		public static TRect Almanac_ZombieStoneRect;

		public static Point Almanac_BackgroundPosition;

		public static TRect Almanac_ZombieClipRect;

		public static TRect Almanac_NavyRect;

		public static Point Almanac_NamePosition;

		public static TRect Almanac_ClayRect;

		public static TRect Almanac_BrownRect;

		public static Point Almanac_ZombieSpace;

		public static Point Almanac_ZombieOffset;

		public static Point Almanac_BossPosition;

		public static Point Almanac_ImpPosition;

		public static Point Almanac_ImitatorPosition;

		public static Point Almanac_SeedSpace;

		public static Point Almanac_SeedOffset;

		public static Point Almanac_ZombiePosition;

		public static Point Almanac_PlantPosition;

		public static float Almanac_Text_Scale;

		public static Point Almanac_PlantsHeader_Pos;

		public static Point Almanac_ZombieHeader_Pos;

		public static int Almanac_ItemName_MaxWidth;

		public static Point PlantGallerySize;

		public static Point ZombieGallerySize;

		public static Point ZombieGalleryWidget_Window_Offset;

		public static Rectangle ZombieGalleryWidget_Window_Clip;

		public static float Zombie_StartOffset;

		public static float Zombie_StartRandom_Offset;

		public static Point Zombie_Bungee_Offset;

		public static Point Zombie_Bungee_Target_Offset;

		public static int Zombie_Dancer_Dance_Limit_X;

		public static float Zombie_Dancer_Spotlight_Scale;

		public static Point Zombie_Dancer_Spotlight_Offset;

		public static int ZOMBIE_BACKUP_DANCER_RISE_HEIGHT;

		public static Point Zombie_Dancer_Spotlight_Pos;

		public static int Zombie_ClipOffset_Default;

		public static int Zombie_ClipOffset_Snorkel;

		public static int Zombie_ClipOffset_Snorkel_intoPool_Small;

		public static int Zombie_ClipOffset_Snorkel_Dying;

		public static int Zombie_ClipOffset_Snorkel_Dying_Small;

		public static int Zombie_ClipOffset_Pail;

		public static int Zombie_ClipOffset_Normal;

		public static int Zombie_ClipOffset_Digger;

		public static int Zombie_ClipOffset_Dolphin_Into_Pool;

		public static int Zombie_ClipOffset_Snorkel_Grabbed;

		public static int Zombie_ClipOffset_PeaHead_InPool;

		public static int Zombie_ClipOffset_RisingFromGrave;

		public static int Zombie_ClipOffset_RisingFromGrave_Small;

		public static int Zombie_ClipOffset_Snorkel_Into_Pool;

		public static int Zombie_ClipOffset_Normal_In_Pool;

		public static int Zombie_ClipOffset_Flag_In_Pool;

		public static int Zombie_ClipOffset_TrafficCone_In_Pool_SMALL;

		public static int Zombie_ClipOffset_Normal_In_Pool_SMALL;

		public static int Zombie_ClipOffset_Ducky_Dying_In_Pool;

		public static int Zombie_GameOver_ClipOffset_1;

		public static int Zombie_GameOver_ClipOffset_2;

		public static int Zombie_GameOver_ClipOffset_3;

		public static Rectangle AwardScreen_Note_Credits_Background;

		public static Point AwardScreen_Note_Credits_Paper;

		public static Rectangle AwardScreen_Note_Credits_Text;

		public static Rectangle AwardScreen_Note_Help_Background;

		public static Point AwardScreen_Note_Help_Paper;

		public static Point AwardScreen_Note_Help_Text;

		public static Rectangle AwardScreen_Note_1_Background;

		public static Point AwardScreen_Note_1_Paper;

		public static Point AwardScreen_Note_1_Text;

		public static Point AwardScreen_Note_Message;

		public static Rectangle AwardScreen_Note_2_Background;

		public static Point AwardScreen_Note_2_Paper;

		public static Point AwardScreen_Note_2_Text;

		public static Rectangle AwardScreen_Note_3_Background;

		public static Point AwardScreen_Note_3_Paper;

		public static Point AwardScreen_Note_3_Text;

		public static Rectangle AwardScreen_Note_4_Background;

		public static Point AwardScreen_Note_4_Paper;

		public static Point AwardScreen_Note_4_Text;

		public static Rectangle AwardScreen_Note_Final_Background;

		public static Point AwardScreen_Note_Final_Paper;

		public static Point AwardScreen_Note_Final_Text;

		public static Point AwardScreen_Bacon;

		public static Point AwardScreen_WateringCan;

		public static Point AwardScreen_Taco;

		public static Point AwardScreen_CarKeys;

		public static Point AwardScreen_Almanac;

		public static Point AwardScreen_Trophy;

		public static Point AwardScreen_Shovel;

		public static Point AwardScreen_MenuButton;

		public static TRect AwardScreen_ClayTablet;

		public static Point AwardScreen_TitlePos;

		public static Point AwardScreen_GroundDay_Pos;

		public static TRect AwardScreen_BrownRect;

		public static Point AwardScreen_BottomMessage_Pos;

		public static TRect AwardScreen_BottomText_Rect_Text;

		public static TRect AwardScreen_BottomText_Rect_NoText;

		public static TRect AwardScreen_CreditsButton;

		public static Point AwardScreen_CreditsButton_Offset;

		public static Point AwardScreen_Seed_Pos;

		public static int AwardScreen_ContinueButton_Offset;

		public static Point AwardScreen_AchievementImage_Pos;

		public static Point AwardScreen_AchievementName_Pos;

		public static Rectangle AwardScreen_AchievementDescription_Rect;

		public static Rectangle CreditScreen_ReplayButton;

		public static Point CreditScreen_ReplayButton_TextOffset;

		public static Rectangle CreditScreen_MainMenu;

		public static int CreditScreen_TextStart;

		public static int CreditScreen_TextEnd;

		public static int CreditScreen_TextClip;

		public static int CreditScreen_LeftText_X;

		public static int CreditScreen_RightText_X;

		public static int CreditScreen_LeftRight_Text_Width;

		public static TRect ConfirmPurchaseDialog_Background;

		public static Point ConfirmPurchaseDialog_Item_Pos;

		public static TRect ConfirmPurchaseDialog_Text;

		public static Insets LawnDialog_Insets;

		public static Point UILevelPosition;

		public static Point UIMenuButtonPosition;

		public static int UIMenuButtonWidth;

		public static int UISunBankPositionX;

		public static Point UISunBankTextOffset;

		public static Point UIShovelButtonPosition;

		public static Point UIProgressMeterPosition;

		public static int UIProgressMeterHeadEnd;

		public static int UIProgressMeterBarEnd;

		public static Point UICoinBankPosition;

		public static int Board_Cutscene_ExtraScroll;

		public static int Board_SunCoinRange;

		public static Point Board_SunCoin_CollectTarget;

		public static int[] Board_Cel_Y_Values_Normal;

		public static int[] Board_Cel_Y_Values_Pool;

		public static int[] Board_Cel_Y_Values_ZenGarden;

		public static Point Board_GameOver_Interior_Overlay_1;

		public static Point Board_GameOver_Interior_Overlay_2;

		public static Point Board_GameOver_Interior_Overlay_3;

		public static Point Board_GameOver_Interior_Overlay_4;

		public static Point Board_GameOver_Exterior_Overlay_1;

		public static Point Board_GameOver_Exterior_Overlay_2;

		public static Point Board_GameOver_Exterior_Overlay_3;

		public static Point Board_GameOver_Exterior_Overlay_4;

		public static Point Board_GameOver_Exterior_Overlay_5;

		public static Point Board_GameOver_Exterior_Overlay_6;

		public static int Board_Offset_AspectRatio_Correction;

		public static int Board_Ice_Start;

		public static int Board_ProgressBarText_Pos;

		public static int MessageWidget_SlotMachine_Y;

		public static Point LawnMower_Coin_Offset;

		public static int DescriptionWidget_ScrollBar_Padding;

		public static Point Coin_AwardSeedpacket_Pos;

		public static Point Coin_Glow_Offset;

		public static float Coin_Silver_Offset;

		public static Point Coin_MoneyBag_Offset;

		public static Point Coin_Shovel_Offset;

		public static Point Coin_Silver_Award_Offset;

		public static Point Coin_Almanac_Offset;

		public static Point Coin_Note_Offset;

		public static Point Coin_CarKeys_Offset;

		public static Point Coin_Taco_Offset;

		public static Point Coin_Bacon_Offset;

		public static Point Plant_CobCannon_Projectile_Offset;

		public static Point Plant_Squished_Offset;

		public static int IZombieBrainPosition;

		public static Point IZombie_SeedOffset;

		public static Rectangle IZombie_ClipOffset;

		public static Point[] ZombieOffsets;

		public static TRect LastStandButtonRect;

		public static int VasebreakerJackInTheBoxOffset;

		public static int JackInTheBoxPlantRadius;

		public static int JackInTheBoxZombieRadius;

		public static float ZenGardenGreenhouseMultiplierX;

		public static float ZenGardenGreenhouseMultiplierY;

		public static Point ZenGardenGreenhouseOffset;

		public static Point ZenGardenMushroomGardenOffset;

		public static int ZenGardenStoreButtonX;

		public static int ZenGardenStoreButtonY;

		public static int ZenGardenTopButtonStart;

		public static Point ZenGardenButtonCounterOffset;

		public static Point ZenGardenButton_GoldenWateringCan_Offset;

		public static Point ZenGardenButton_NormalWateringCan_Offset;

		public static Point ZenGardenButton_Fertiliser_Offset;

		public static Point ZenGardenButton_BugSpray_Offset;

		public static Point ZenGardenButton_Phonograph_Offset;

		public static Point ZenGardenButton_Chocolate_Offset;

		public static Point ZenGardenButton_Glove_Offset;

		public static Point ZenGardenButton_MoneySign_Offset;

		public static Point ZenGardenButton_NextGarden_Offset;

		public static Point ZenGardenButton_Wheelbarrow_Offset;

		public static Point ZenGardenButton_WheelbarrowPlant_Offset;

		public static float ZenGardenButton_Wheelbarrow_Facing_Offset;

		public static int ZenGarden_Backdrop_X;

		public static Point ZenGarden_SellDialog_Offset;

		public static Point ZenGarden_NextGarden_Pos;

		public static Point ZenGarden_RetardedDaveBubble_Pos;

		public static Point ZenGarden_PlantSpeechBubble_Pos;

		public static Point ZenGarden_StinkySpeechBubble_Pos;

		public static Point ZenGarden_WaterDrop_Pos;

		public static Point ZenGarden_Fertiliser_Pos;

		public static Point ZenGarden_Phonograph_Pos;

		public static Point ZenGarden_BugSpray_Pos;

		public static Point ZenGarden_GoldenWater_Pos;

		public static Point ZenGarden_Chocolate_Pos;

		public static int ZenGarden_MoneyTarget_X;

		public static int ZenGarden_TutorialArrow_Offset;

		public static int ZEN_XMIN;

		public static int ZEN_YMIN;

		public static int ZEN_XMAX;

		public static int ZEN_YMAX;

		public static Rectangle ZenGarden_GoldenWater_Size;

		public static int STINKY_SLEEP_POS_Y;

		public static SpecialGridPlacement[] gMushroomGridPlacement;

		public static Point ZenGarden_Marigold_Sprout_Offset;

		public static Point ZenGarden_Aquarium_ShadowOffset;

		public static int Challenge_SeeingStars_StarfruitPreview_Offset_Y;

		public static Point Challenge_SlotMachine_Pos;

		public static TRect Challenge_SlotMachineHandle_Pos;

		public static int Challenge_SlotMachine_Gap;

		public static int Challenge_SlotMachine_Offset;

		public static int Challenge_SlotMachine_Shadow_Offset;

		public static int Challenge_SlotMachine_Y_Offset;

		public static int Challenge_SlotMachine_Y_Pos;

		public static int Challenge_SlotMachine_ClipHeight;

		public static Point Challenge_BeghouldedTwist_Offset;

		public static Point GridItem_ScaryPot_SeedPacket_Offset;

		public static Point GridItem_ScaryPot_Zombie_Offset;

		public static Point GridItem_ScaryPot_ZombieFootball_Offset;

		public static Point GridItem_ScaryPot_ZombieGargantuar_Offset;

		public static Point GridItem_ScaryPot_Sun_Offset;

		public static LanguageIndex Language
		{
			get
			{
				return mLanguage;
			}
			set
			{
				mLanguage = value;
				LanguageSubDir = mLanguage.ToString();
			}
		}

		public static void Load320x480()
		{
			S = 8f / 15f;
			IS = 1.875f;
			FrameworkConstants.Font_Scale = 2f / 3f;
			Board_Offset_AspectRatio_Correction = 0;
			Loaded = true;
			BackBufferSize = new Point(320, 480);
			ImageSubPath = "\\320x480\\";
			BOARD_WIDTH = 480;
			BOARD_HEIGHT = 320;
			BOARD_EDGE = 72;
			BOARD_OFFSET = 119;
			BOARD_EXTRA_ROOM = 100;
			HIGH_GROUND_HEIGHT = 30;
			GRIDSIZEX = 9;
			MAX_GRIDSIZEY = 6;
			MORE_GAMES_ORIGIN_X = 0;
			MAIN_MENU_ORIGIN_X = 354;
			ACHIEVEMENTS_ORIGIN_X = 354;
			QUICKPLAY_ORIGIN_X = 834;
			MORE_GAMES_PLANK_HEIGHT = 100;
			MORE_GAMES_PLANK_WIDTH = 320;
			MORE_GAMES_ITEM_GAP = 2;
			SMALL_SEEDPACKET_WIDTH = 54;
			SMALL_SEEDPACKET_HEIGHT = 35;
			SEED_CHOOSER_OFFSETSCREEN_OFFSET = 320;
			SCROLL_AREA_OFFSET_X = 67;
			SCROLL_AREA_OFFSET_Y = 28;
			DIALOG_HEADER_OFFSET = 24;
			WIDE_BOARD_WIDTH = 900;
			BACKGROUND_IMAGE_WIDTH = 1400;
			LAWN_XMIN = 140;
			LAWN_YMIN = 80;
			TitleScreen_ReanimStart_X = 60;
			GameSelector_Width = 1311;
			GameSelector_Height = 640;
			GameSelector_AdventureButton_X = 228;
			GameSelector_MiniGameButton_X = 236;
			GameSelector_MiniGameButton_Y = (int)InvertAndScale(94f);
			GameSelector_PuzzleButton_X = GameSelector_MiniGameButton_X + 4;
			GameSelector_PuzzleButton_Y = (int)InvertAndScale(160f);
			GameSelector_OptionsButton_X = 340;
			GameSelector_OptionsButton_Y = (int)InvertAndScale(200f);
			GameSelector_ZenGardenButton_X = 260;
			GameSelector_ZenGardenButton_Y = (int)InvertAndScale(200f);
			GameSelector_AlmanacButton_X = 255;
			GameSelector_StoreButton_X = 212;
			GameSelector_StoreButton_Y = (int)InvertAndScale(168f);
			GameSelector_AchievementsButton_X = 126;
			GameSelector_AchievementsButton_Y = (int)InvertAndScale(250f);
			GameSelector_AchievementsStatue_X = 234;
			GameSelector_AchievementsStatue_Y = (int)InvertAndScale(170f);
			GameSelector_MoreWaysToPlay_MiniGames_Y = 276;
			GameSelector_MoreWaysToPlay_VaseBreaker_X = 65;
			GameSelector_MoreWaysToPlay_IZombie_X = 161;
			GameSelector_MoreWaysToPlay_Back_X = 174;
			GameSelector_PlayerName_Pos = new Point(112, -111);
			GameSelector_LevelNumber_1_Pos = new Point(306, 19);
			GameSelector_LevelNumber_2_Pos = new Point(324, 20);
			GameSelector_LevelNumber_3_Pos = new Point(333, 21);
			GameSelector_LevelNumber_Bar = new Rectangle(320, 28, 3, 1);
			GameSelector_LevelNumber_ButtonDown_Offset = 3;
			GameSelector_Update_Offset = 69;
			LeaderboardScreen_Vasebreaker_Button_X = 621;
			LeaderboardScreen_Vasebreaker_Button_Y = 335;
			LeaderboardScreen_IZombie_Button_X = 129;
			LeaderboardScreen_IZombie_Button_Y = 307;
			LeaderboardScreen_PileBase_X = 331;
			LeaderboardScreen_PileBase_Y = 53;
			Leaderboard_IZombie_Score_X = 198;
			Leaderboard_IZombie_Score_Y = 290;
			Leaderboard_Vasebreaker_Score_X = 698;
			Leaderboard_Vasebreaker_Score_Y = 322;
			Leaderboard_Pile_1_X = 297;
			LeaderboardDialog_CannotConnect_Rect = new TRect(100, 100, 300, 300);
			LeaderboardScreen_EdgeOfSpace_Overlay_Offset = 30;
			SEED_PACKET_HORIZ_GAP = 2;
			SEED_PACKET_VERT_GAP = 1;
			SeedPacket_Selector_Pos = new Point(-2, 2);
			AchievementWidget_ROW_HEIGHT = 82;
			AchievementWidget_ROW_START = 149;
			AchievementWidget_HOLE_DEPTH = 128;
			AchievementWidget_GAMERSCORE_POS = new Point(100, 100);
			AchievementWidget_BackButton_X = 52;
			AchievementWidget_BackButton_Y = 5;
			AchievementWidget_BackButton_Rect = new TRect(54, 0, 105, 80);
			AchievementWidget_Background_Offset_Y = 0;
			AchievementWidget_Description_Box = new Rectangle(180, 30, 212, 60);
			AchievementWidget_Pipe_Offset = new Point(27, 13);
			AchievementWidget_Worm_Offset = new Point(382, 18);
			AchievementWidget_ZombieWorm_Offset = new Point(29, 74);
			AchievementWidget_GemLeft_Offset = new Point(86, 43);
			AchievementWidget_GemRight_Offset = new Point(263, 43);
			AchievementWidget_Fossile_Offset = new Point(131, 49);
			AchievementWidget_Image_Pos = new Point(10, 10);
			AchievementWidget_Image_Size = (int)(FrameworkConstants.Font_Scale * 64f);
			AchievementWidget_Name_Pos = new Point(250, 10);
			AchievementWidget_Name_MaxWidth = 300;
			QuickPlayWidget_Thumb_X = 26;
			QuickPlayWidget_Thumb_Y = 22;
			QuickPlayWidget_Bungee_Y = -40;
			SeedChooserScreen_MenuButton_X = 390;
			SeedChooserScreen_Background_Top = new Point(58, 2);
			SeedChooserScreen_Background_Middle = new Point(58, 31);
			SeedChooserScreen_Background_Middle_Height = 220;
			SeedChooserScreen_Background_Bottom = new Point(58, 251);
			SeedChooserScreen_Gradient_Top = new Rectangle(0, 24, 222, 12);
			SeedChooserScreen_Gradient_Bottom = new Rectangle(-1, 246, 223, 12);
			SeedPacket_Cost = new Point(29, 18);
			SeedPacket_Cost_IZombie = new Point(31, 18);
			SeedPacket_CostText_Pos = new Point(51, 15);
			SeedPacket_CostText_IZombie_Pos = new Point(55, 15);
			ImitaterDialog_Size = new Point(0, 155);
			ImitaterDialog_ScrollWidget_Offset_X = 0;
			ImitaterDialog_ScrollWidget_Y = 70;
			ImitaterDialog_ScrollWidget_ExtraWidth = 13;
			ImitaterDialog_Height = 152;
			ImitaterDialog_BottomGradient_Y = 143;
			CutScene_ReadySetPlant_Pos = new Point(240, 173);
			CutScene_LogoEndPos = 255;
			CutScene_LogoBackRect_Height = 150;
			CutScene_LogoEnd_Particle_Pos = new Point(400, 300);
			CutScene_ExtraRoom_1_Particle_Pos = new Point(35, 348);
			CutScene_ExtraRoom_2_Particle_Pos = new Point(35, 246);
			CutScene_ExtraRoom_3_Particle_Pos = new Point(35, 459);
			CutScene_ExtraRoom_4_Particle_Pos = new Point(32, 150);
			CutScene_ExtraRoom_5_Particle_Pos = new Point(32, 551);
			CutScene_SodRoll_1_Pos = -102;
			CutScene_SodRoll_2_Pos = 111;
			CutScene_SodRoll_3_Pos = new Point(-3, -198);
			CutScene_SodRoll_4_Pos = new Point(-3, 203);
			CutScene_Upsell_TerraCotta_Arrow = new Point(592, 240);
			CutScene_Upsell_TerraCotta_Pot = new Point(565, 360);
			ConveyorBeltClipRect = new TRect(0, 5, 54, 310);
			StoreScreen_BackButton_X = 230;
			StoreScreen_BackButton_Y = 257;
			StoreScreen_Car_X = 120;
			StoreScreen_Car_Y = 50;
			StoreScreen_PrevButton_X = 158;
			StoreScreen_PrevButton_Y = 196;
			StoreScreen_NextButton_X = 380;
			StoreScreen_NextButton_Y = 196;
			StoreScreen_HatchOpen_X = 160;
			StoreScreen_HatchOpen_Y = -22;
			StoreScreen_HatchClosed_X = 150;
			StoreScreen_HatchClosed_Y = 49;
			StoreScreen_CarNight_X = 120;
			StoreScreen_CarNight_Y = 50;
			StoreScreen_StoreSign_X = 269;
			StoreScreen_StoreSign_Y_Min = -150;
			StoreScreen_StoreSign_Y_Max = -10;
			StoreScreen_Coinbank_X = 340;
			StoreScreen_Coinbank_Y = 270;
			StoreScreen_Coinbank_TextOffset = new Point(116, 5);
			StoreScreen_ItemOffset_1_X = 234;
			StoreScreen_ItemOffset_1_Y = 40;
			StoreScreen_ItemOffset_2_X = 214;
			StoreScreen_ItemOffset_2_Y = 109;
			StoreScreen_ItemSize = 54;
			StoreScreen_ItemSize_Offset = 4;
			StoreScreen_PriceTag_X = 3;
			StoreScreen_PriceTag_Y = 70;
			StoreScreen_PriceTag_Text_Offset_X = 23;
			StoreScreen_PriceTag_Text_Offset_Y = 67;
			StoreScreen_ComingSoon_X = 60;
			StoreScreen_ComingSoon_Y = 70;
			StoreScreen_SoldOut_Width = 50;
			StoreScreen_SoldOut_Y = 30;
			StoreScreen_SoldOut_Height = 60;
			StoreScreen_PacketUpgrade_X = 7;
			StoreScreen_PacketUpgrade_Y = 16;
			StoreScreen_PacketUpgrade_Text_Size = new Rectangle(0, 12, 55, 70);
			StoreScreen_RetardedDave_Offset_X = -40;
			StoreScreen_FirstAidNut_Offset_Y = 22;
			StoreScreen_PoolCleaner_Offset_X = 16;
			StoreScreen_PoolCleaner_Offset_Y = 34;
			StoreScreen_Rake_Offset_X = -10;
			StoreScreen_Rake_Offset_Y = 34;
			StoreScreen_RoofCleaner_Offset_X = 10;
			StoreScreen_RoofCleaner_Offset_Y = 42;
			StoreScreen_Imitater_Offset_X = -1;
			StoreScreen_Imitater_Offset_Y = 34;
			StoreScreen_Default_Offset_Y = 34;
			StoreScreen_MouseRegion = new Point(50, 87);
			StoreScreen_Dialog = new Rectangle(120, 32, 322, 280);
			StoreScreen_PotPlant_Offset = new Point(25, 67);
			StoreScreenMushroomGardenOffsetX = 15;
			StoreScreenAquariumGardenOffsetX = 15;
			StoreScreenGloveOffsetY = 20;
			StoreScreenWheelbarrowOffsetY = 20;
			StoreScreenBugSprayOffsetX = 17;
			StoreScreenBugSprayOffsetY = 20;
			StoreScreenPhonographOffsetY = 20;
			StoreScreenPlantFoodOffsetX = 9;
			StoreScreenPlantFoodOffsetY = 20;
			StoreScreenWateringCanOffsetX = 6;
			StoreScreenWateringCanOffsetY = 20;
			NewOptionsDialog_FXLabel_X = 240;
			NewOptionsDialog_FXLabel_Y = 92;
			NewOptionsDialog_MusicLabel_X = 240;
			NewOptionsDialog_MusicLabel_On_Y = 65;
			NewOptionsDialog_MusicLabel_Off_Y = 70;
			NewOptionsDialog_VibrationLabel_X = 240;
			NewOptionsDialog_VibrationLabel_Y = 125;
			NewOptionsDialog_VibrationLabel_MaxWidth = 200;
			NewOptionsDialog_FullScreenOffset = 25;
			NewOptionsDialog_FX_Offset = 15;
			NewOptionsDialog_Music_Offset = 5;
			NewOptionsDialog_Version_Low_Y = 185;
			NewOptionsDialog_Version_High_Y = 195;
			RetardedDave_Bubble_Tip_Offset = 4;
			RetardedDave_Bubble_Offset_Shop = new Point(-190, -78);
			RetardedDave_Bubble_Offset_Board = new Point(-150, -48);
			RetardedDave_Bubble_Size = 200;
			RetardedDave_Bubble_Rect = new TRect(8, 6, 0, 144);
			RetardedDave_Bubble_TapToContinue_Y = 142;
			PlantGallerySize = new Point(234, 492);
			ZombieGallerySize = new Point(249, 712);
			AlmanacHeaderY = 4;
			Almanac_PlantsButtonRect = new TRect(40, 100, 180, 160);
			Almanac_ZombiesButtonRect = new TRect(260, 100, 180, 160);
			Almanac_CloseButtonRect = new TRect(396, 290, 89, 26);
			Almanac_IndexButtonRect = new TRect(265, 290, 89, 26);
			Almanac_IndexPlantPos = new Point(110, 160);
			Almanac_IndexZombiePos = new Point(320, 150);
			Almanac_IndexPlantTextPos = new Point(Almanac_PlantsButtonRect.mX + Almanac_PlantsButtonRect.mWidth / 2, Almanac_PlantsButtonRect.mY + Almanac_PlantsButtonRect.mHeight - 50);
			Almanac_IndexZombieTextPos = new Point(Almanac_ZombiesButtonRect.mX + Almanac_ZombiesButtonRect.mWidth / 2, Almanac_ZombiesButtonRect.mY + Almanac_ZombiesButtonRect.mHeight - 50);
			Almanac_PlantScrollRect = new TRect(28, 50, 237, 245);
			Almanac_ZombieScrollRect = new TRect(17, 50, 250, 245);
			Almanac_DescriptionScrollRect = new TRect(282, 170, 183, 111);
			Almanac_PlantTopGradientY = 46;
			Almanac_ZombieTopGradientY = 48;
			Almanac_BottomGradientY = 288;
			Almanac_PlantGradientWidth = 222;
			Almanac_ZombieGradientWidth = 240;
			Almanac_ZombieStoneRect = new TRect(267, 1, 212, 310);
			Almanac_BackgroundPosition = new Point(320, 40);
			Almanac_ZombieClipRect = new TRect(-22, -30, 109, 109);
			Almanac_NavyRect = new TRect(306, 27, 134, 134);
			Almanac_NamePosition = new Point(373, 0);
			Almanac_ClayRect = new TRect(270, 4, 204, 310);
			Almanac_BrownRect = new TRect(306, 27, 133, 133);
			Almanac_ZombieSpace = new Point(82, 78);
			Almanac_ZombieOffset = new Point(0, 5);
			Almanac_BossPosition = new Point(123, 629);
			Almanac_ImpPosition = new Point(41, 629);
			Almanac_ImitatorPosition = new Point(PlantGallerySize.X / 2 - 28, 450);
			Almanac_SeedSpace = new Point(2, 2);
			Almanac_SeedOffset = new Point(0, 5);
			Almanac_ZombiePosition = new Point(640, 130);
			Almanac_PlantPosition = new Point(350, 70);
			Almanac_Text_Scale = 0.8f;
			Almanac_PlantsHeader_Pos = new Point(2, 2);
			Almanac_ZombieHeader_Pos = new Point(0, 2);
			Almanac_ItemName_MaxWidth = 230;
			Zombie_StartOffset = 120f;
			Zombie_StartRandom_Offset = 40f;
			AwardScreen_Note_Credits_Background = new Rectangle(-350, -180, 1400, 600);
			AwardScreen_Note_Credits_Paper = new Point((int)(75f * S) + 25, (int)(60f * S));
			AwardScreen_Note_Credits_Text = new Rectangle((int)(149f * S) + 25, (int)(103f * S), (int)(475f * S), (int)(325f * S));
			AwardScreen_Note_Help_Background = new Rectangle(-350, -150, 1400, 600);
			AwardScreen_Note_Help_Paper = new Point(80, 42);
			AwardScreen_Note_Help_Text = new Point(106, 70);
			AwardScreen_Note_1_Background = new Rectangle(-350, -150, 1400, 600);
			AwardScreen_Note_1_Paper = new Point(70, 30);
			AwardScreen_Note_1_Text = new Point(96, 58);
			AwardScreen_Note_Message = new Point(240, 0);
			AwardScreen_Note_2_Background = new Rectangle(-350, -150, 1400, 600);
			AwardScreen_Note_2_Paper = new Point(76, 30);
			AwardScreen_Note_2_Text = new Point(96, 58);
			AwardScreen_Note_3_Background = new Rectangle(-350, -150, 1400, 600);
			AwardScreen_Note_3_Paper = new Point(70, 26);
			AwardScreen_Note_3_Text = new Point(92, 46);
			AwardScreen_Note_4_Background = new Rectangle(-350, -150, 1400, 600);
			AwardScreen_Note_4_Paper = new Point(70, 30);
			AwardScreen_Note_4_Text = new Point(90, 52);
			AwardScreen_Note_Final_Background = new Rectangle(-350, -150, 1400, 600);
			AwardScreen_Note_Final_Paper = new Point(70, 30);
			AwardScreen_Note_Final_Text = new Point(96, 58);
			AwardScreen_Bacon = (AwardScreen_Taco = (AwardScreen_CarKeys = new Point(123, 140)));
			AwardScreen_WateringCan = new Point(123, 140);
			AwardScreen_Almanac = new Point(123, 120);
			AwardScreen_Trophy = new Point((int)(400f * S), (int)(137f * S));
			AwardScreen_Shovel = new Point(123, 132);
			AwardScreen_MenuButton = new Point(345, 280);
			AwardScreen_ClayTablet = new TRect(31, 54, 418, 190);
			AwardScreen_TitlePos = new Point(240, 13);
			AwardScreen_GroundDay_Pos = new Point(70, 112);
			AwardScreen_BrownRect = new TRect(56, 97, 133, 133);
			AwardScreen_BottomMessage_Pos = new Point(240, 56);
			AwardScreen_BottomText_Rect_Text = new TRect(208, 114, 218, 100);
			AwardScreen_BottomText_Rect_NoText = new TRect(131, 78, 218, 140);
			AwardScreen_CreditsButton = new TRect(50, 278, 140, 32);
			AwardScreen_CreditsButton_Offset = new Point(20, -5);
			AwardScreen_Seed_Pos = new Point(97, 145);
			AwardScreen_ContinueButton_Offset = 35;
			AwardScreen_AchievementImage_Pos = new Point(80, 4);
			AwardScreen_AchievementName_Pos = new Point(160, 25);
			AwardScreen_AchievementDescription_Rect = new Rectangle(160, 30, 212, 60);
			CreditScreen_ReplayButton = new Rectangle(10, 277, 125, 32);
			CreditScreen_ReplayButton_TextOffset = new Point(15, -5);
			CreditScreen_MainMenu = new Rectangle(330, 282, 140, 30);
			CreditScreen_TextStart = 120;
			CreditScreen_TextEnd = 200;
			CreditScreen_TextClip = 250;
			CreditScreen_LeftText_X = 0;
			CreditScreen_RightText_X = 245;
			CreditScreen_LeftRight_Text_Width = 230;
			ConfirmPurchaseDialog_Background = new TRect(26, 70, 262, 140);
			ConfirmPurchaseDialog_Item_Pos = new Point(42, 80);
			ConfirmPurchaseDialog_Text = new TRect(120, 70, 150, 140);
			LawnDialog_Insets = new Insets(40, 15, 46, 20);
			UILevelPosition = new Point(476, 297);
			UIMenuButtonPosition = new Point(390, 2);
			UIMenuButtonWidth = 90;
			UISunBankPositionX = 57;
			UISunBankTextOffset = new Point(60, 2);
			UIShovelButtonPosition = new Point(156, 2);
			UIProgressMeterPosition = new Point(222, 2);
			UIProgressMeterHeadEnd = 135;
			UIProgressMeterBarEnd = 143;
			UICoinBankPosition = new Point(66, 319);
			Board_Cutscene_ExtraScroll = 0;
			Board_SunCoinRange = 550;
			Board_SunCoin_CollectTarget = new Point(100, 0);
			Board_Cel_Y_Values_Normal = new int[7]
			{
				80,
				177,
				280,
				380,
				470,
				570,
				610
			};
			Board_Cel_Y_Values_Pool = new int[7]
			{
				80,
				170,
				275,
				360,
				447,
				522,
				610
			};
			Board_Cel_Y_Values_ZenGarden = new int[7]
			{
				80,
				177,
				280,
				380,
				470,
				570,
				610
			};
			Board_GameOver_Interior_Overlay_1 = new Point(-13, 121);
			Board_GameOver_Interior_Overlay_2 = new Point(-14, 108);
			Board_GameOver_Interior_Overlay_3 = new Point(-37, 129);
			Board_GameOver_Interior_Overlay_4 = new Point(-37, 129);
			Board_GameOver_Exterior_Overlay_1 = new Point(-15, 109);
			Board_GameOver_Exterior_Overlay_2 = new Point(-14, 112);
			Board_GameOver_Exterior_Overlay_3 = new Point(-37, 125);
			Board_GameOver_Exterior_Overlay_4 = new Point(-38, 71);
			Board_GameOver_Exterior_Overlay_5 = new Point(-63, 44);
			Board_GameOver_Exterior_Overlay_6 = new Point(-63, 44);
			Board_Ice_Start = 800;
			Board_ProgressBarText_Pos = 1;
			MessageWidget_SlotMachine_Y = 518;
			LawnMower_Coin_Offset = new Point(40, 40);
			DescriptionWidget_ScrollBar_Padding = 7;
			Zombie_Bungee_Offset = new Point(61, 15);
			Zombie_Bungee_Target_Offset = new Point(10, 60);
			ZOMBIE_BACKUP_DANCER_RISE_HEIGHT = -145;
			Zombie_Dancer_Dance_Limit_X = 700;
			Zombie_Dancer_Spotlight_Scale = 2.1f;
			Zombie_Dancer_Spotlight_Offset = new Point(-32, 60);
			Zombie_Dancer_Spotlight_Pos = new Point(-25, -480);
			Zombie_ClipOffset_Default = 100;
			Zombie_ClipOffset_Snorkel = 0;
			Zombie_ClipOffset_Snorkel_intoPool_Small = 40;
			Zombie_ClipOffset_Snorkel_Dying = 76;
			Zombie_ClipOffset_Snorkel_Dying_Small = 65;
			Zombie_ClipOffset_Pail = 78;
			Zombie_ClipOffset_Normal = 108;
			Zombie_ClipOffset_Digger = 75;
			Zombie_ClipOffset_Dolphin_Into_Pool = 65;
			Zombie_ClipOffset_Snorkel_Grabbed = 75;
			Zombie_ClipOffset_PeaHead_InPool = 78;
			Zombie_ClipOffset_RisingFromGrave = 28;
			Zombie_ClipOffset_RisingFromGrave_Small = -35;
			Zombie_ClipOffset_Snorkel_Into_Pool = 60;
			Zombie_ClipOffset_Normal_In_Pool = 30;
			Zombie_ClipOffset_Flag_In_Pool = 30;
			Zombie_ClipOffset_Normal_In_Pool_SMALL = -40;
			Zombie_ClipOffset_TrafficCone_In_Pool_SMALL = -55;
			Zombie_ClipOffset_Ducky_Dying_In_Pool = 30;
			Zombie_GameOver_ClipOffset_1 = 53;
			Zombie_GameOver_ClipOffset_2 = 56;
			Zombie_GameOver_ClipOffset_3 = 56;
			ZombieGalleryWidget_Window_Offset = new Point(0, 0);
			ZombieGalleryWidget_Window_Clip = new Rectangle(2, 2, 72, 72);
			Coin_AwardSeedpacket_Pos = new Point(460, 300);
			Coin_Glow_Offset = new Point(14, 12);
			Coin_Silver_Offset = 9f;
			Coin_MoneyBag_Offset = new Point(-45, -40);
			Coin_Shovel_Offset = new Point(3, 15);
			Coin_Silver_Award_Offset = new Point(21, 21);
			Coin_Almanac_Offset = new Point(0, -36);
			Coin_Note_Offset = new Point(0, -10);
			Coin_CarKeys_Offset = new Point(0, 10);
			Coin_Taco_Offset = new Point(0, 20);
			Coin_Bacon_Offset = new Point(0, 20);
			Plant_CobCannon_Projectile_Offset = new Point(-28, -165);
			Plant_Squished_Offset = new Point(0, 0);
			IZombieBrainPosition = 140;
			IZombie_SeedOffset = new Point(5, 6);
			IZombie_ClipOffset = new Rectangle(2, 2, 70, 34);
			ZombieOffsets = new Point[26]
			{
				new Point(20, 11),
				new Point(13, 15),
				new Point(22, 14),
				new Point(3, 13),
				new Point(20, 13),
				new Point(18, 13),
				new Point(19, 13),
				new Point(4, 11),
				new Point(14, 13),
				new Point(19, 12),
				new Point(20, 13),
				new Point(16, 10),
				new Point(1, 11),
				new Point(16, 10),
				new Point(16, 15),
				new Point(14, 10),
				new Point(13, 12),
				new Point(10, 13),
				new Point(20, 12),
				new Point(11, 12),
				new Point(0, 9),
				new Point(15, 14),
				new Point(4, 12),
				new Point(18, 10),
				new Point(23, 27),
				new Point(1, 15)
			};
			LastStandButtonRect = new TRect(200, 297, 140, 28);
			VasebreakerJackInTheBoxOffset = 0;
			JackInTheBoxPlantRadius = 90;
			JackInTheBoxZombieRadius = 115;
			ZenGardenGreenhouseMultiplierX = 1.125f;
			ZenGardenGreenhouseMultiplierY = 1.182f;
			ZenGardenGreenhouseOffset = new Point(-30, 30);
			ZenGardenMushroomGardenOffset = new Point(10, 30);
			ZenGardenStoreButtonX = 650;
			ZenGardenStoreButtonY = 33;
			ZenGardenTopButtonStart = -100;
			ZenGardenButtonCounterOffset = new Point(40, 40);
			ZenGardenButton_GoldenWateringCan_Offset = new Point(-2, -6);
			ZenGardenButton_NormalWateringCan_Offset = new Point(-2, -6);
			ZenGardenButton_Fertiliser_Offset = new Point(-6, -7);
			ZenGardenButton_BugSpray_Offset = new Point(0, -1);
			ZenGardenButton_Phonograph_Offset = new Point(2, 2);
			ZenGardenButton_Chocolate_Offset = new Point(6, 4);
			ZenGardenButton_Glove_Offset = new Point(-6, -4);
			ZenGardenButton_MoneySign_Offset = new Point(-5, -4);
			ZenGardenButton_NextGarden_Offset = new Point(2, 0);
			ZenGardenButton_Wheelbarrow_Offset = new Point(-7, -3);
			ZenGardenButton_WheelbarrowPlant_Offset = new Point(23, -8);
			ZenGardenButton_Wheelbarrow_Facing_Offset = 80f;
			ZenGarden_Backdrop_X = 0;
			ZenGarden_SellDialog_Offset = new Point(120, 60);
			ZenGarden_NextGarden_Pos = new Point(564, 0);
			ZenGarden_RetardedDaveBubble_Pos = new Point(-250, -120);
			ZenGarden_WaterDrop_Pos = new Point(67, 9);
			ZenGarden_Fertiliser_Pos = new Point(61, 7);
			ZenGarden_Phonograph_Pos = new Point(60, 7);
			ZenGarden_BugSpray_Pos = new Point(61, 7);
			ZenGarden_PlantSpeechBubble_Pos = new Point(50, 0);
			ZenGarden_StinkySpeechBubble_Pos = new Point(50, -20);
			ZenGarden_GoldenWater_Pos = new Point(-14, -14);
			ZenGarden_Chocolate_Pos = new Point(63, -28);
			ZenGarden_MoneyTarget_X = 442;
			ZenGarden_TutorialArrow_Offset = 100;
			ZEN_XMIN = 10;
			ZEN_YMIN = 65;
			ZEN_XMAX = 470;
			ZEN_YMAX = 310;
			ZenGarden_GoldenWater_Size = new Rectangle(-70, -90, 80, 80);
			STINKY_SLEEP_POS_Y = 461;
			gMushroomGridPlacement = new SpecialGridPlacement[8]
			{
				new SpecialGridPlacement(110, 441, 0, 0),
				new SpecialGridPlacement(237, 360, 1, 0),
				new SpecialGridPlacement(298, 458, 2, 0),
				new SpecialGridPlacement(355, 296, 3, 0),
				new SpecialGridPlacement(387, 203, 4, 0),
				new SpecialGridPlacement(460, 385, 5, 0),
				new SpecialGridPlacement(486, 478, 6, 0),
				new SpecialGridPlacement(552, 283, 7, 0)
			};
			ZenGarden_Marigold_Sprout_Offset = new Point(24, 30);
			ZenGarden_Aquarium_ShadowOffset = new Point(35, 0);
			Challenge_SeeingStars_StarfruitPreview_Offset_Y = 8;
			Challenge_SlotMachine_Pos = new Point(100, 200);
			Challenge_SlotMachineHandle_Pos = new TRect(473, 0, 55, 80);
			Challenge_SlotMachine_Gap = 9;
			Challenge_SlotMachine_Offset = 62;
			Challenge_SlotMachine_Shadow_Offset = 3;
			Challenge_SlotMachine_Y_Offset = 3;
			Challenge_SlotMachine_Y_Pos = 3;
			Challenge_SlotMachine_ClipHeight = 40;
			Challenge_BeghouldedTwist_Offset = new Point(40, 50);
			GridItem_ScaryPot_SeedPacket_Offset = new Point(23, 33);
			GridItem_ScaryPot_Zombie_Offset = new Point(6, 19);
			GridItem_ScaryPot_ZombieFootball_Offset = new Point(1, 16);
			GridItem_ScaryPot_ZombieGargantuar_Offset = new Point(9, 7);
			GridItem_ScaryPot_Sun_Offset = new Point(42, 62);
		}

		public static float InvertLowResValue(float x)
		{
			return 1.875f * x;
		}

		public static float InvertAndScale(float x)
		{
			return InvertLowResValue(x) * S;
		}

		public static float ScaleFrom480(float x)
		{
			return FrameworkConstants.Font_Scale * x;
		}

		public static void Load480x800()
		{
			S = 0.8f;
			IS = 1.25f;
			FrameworkConstants.Font_Scale = 1f;
			Board_Offset_AspectRatio_Correction = 80;
			if (Language == LanguageIndex.it)
			{
				ReanimTextCenterOffsetX = (int)InvertAndScale(50f);
			}
			else if (Language == LanguageIndex.de)
			{
				ReanimTextCenterOffsetX = (int)InvertAndScale(85f);
			}
			else if (Language == LanguageIndex.fr)
			{
				ReanimTextCenterOffsetX = (int)InvertAndScale(85f);
			}
			else if (Language == LanguageIndex.es)
			{
				ReanimTextCenterOffsetX = -(int)InvertAndScale(10f);
			}
			else
			{
				ReanimTextCenterOffsetX = (int)InvertAndScale(85f);
			}
			Loaded = true;
			BackBufferSize = new Point(480, 800);
			ImageSubPath = "\\480x800\\";
			BOARD_WIDTH = 800;
			BOARD_HEIGHT = 480;
			BOARD_EDGE = 50;
			BOARD_OFFSET = 119;
			BOARD_EXTRA_ROOM = 100;
			HIGH_GROUND_HEIGHT = 30;
			GRIDSIZEX = 9;
			MAX_GRIDSIZEY = 6;
			MORE_GAMES_ORIGIN_X = 100;
			MAIN_MENU_ORIGIN_X = 640;
			ACHIEVEMENTS_ORIGIN_X = 640;
			QUICKPLAY_ORIGIN_X = 1440;
			MORE_GAMES_PLANK_HEIGHT = 100;
			MORE_GAMES_PLANK_WIDTH = 320;
			MORE_GAMES_ITEM_GAP = 2;
			SMALL_SEEDPACKET_WIDTH = 81;
			SMALL_SEEDPACKET_HEIGHT = 52;
			SEED_CHOOSER_OFFSETSCREEN_OFFSET = 480;
			SCROLL_AREA_OFFSET_X = 102;
			SCROLL_AREA_OFFSET_Y = (int)InvertAndScale(28f);
			DIALOG_HEADER_OFFSET = 36;
			WIDE_BOARD_WIDTH = 900;
			BACKGROUND_IMAGE_WIDTH = (int)(S * 1400f);
			LAWN_XMIN = 140;
			LAWN_YMIN = 80;
			SEED_PACKET_HORIZ_GAP = (int)InvertAndScale(2f);
			SEED_PACKET_VERT_GAP = (int)InvertAndScale(1f);
			SeedPacket_Selector_Pos = new Point(-5, 2);
			TitleScreen_ReanimStart_X = 130;
			GameSelector_Width = 2348;
			GameSelector_Height = (int)InvertAndScale(640f);
			GameSelector_AdventureButton_X = 430;
			GameSelector_MiniGameButton_X = GameSelector_AdventureButton_X + 21;
			GameSelector_MiniGameButton_Y = (int)InvertAndScale(100f);
			GameSelector_OptionsButton_X = 662;
			GameSelector_OptionsButton_Y = 276;
			GameSelector_ZenGardenButton_X = 418;
			GameSelector_ZenGardenButton_Y = 349;
			GameSelector_AlmanacButton_X = 536;
			GameSelector_AlmanacButton_Y = 334;
			GameSelector_StoreButton_X = 375;
			GameSelector_StoreButton_Y = 273;
			GameSelector_LeaderboardButton_X = 0;
			GameSelector_LeaderboardButton_Y = (int)InvertAndScale(200f);
			GameSelector_AchievementsButton_X = 211;
			GameSelector_AchievementsButton_Y = 429;
			GameSelector_AchievementsStatue_X = 212;
			GameSelector_AchievementsStatue_Y = 252;
			GameSelector_MoreWaysToPlay_MiniGames_X = 236;
			GameSelector_MoreWaysToPlay_MiniGames_Y = (int)InvertAndScale(160f);
			GameSelector_MoreWaysToPlay_VaseBreaker_X = 525;
			GameSelector_MoreWaysToPlay_VaseBreaker_Y = (int)InvertAndScale(175f);
			GameSelector_MoreWaysToPlay_IZombie_X = 350;
			GameSelector_MoreWaysToPlay_IZombie_Y = (int)InvertAndScale(195f);
			GameSelector_MoreWaysToPlay_Back_X = 330;
			GameSelector_MoreWaysToPlay_Back_Y = (int)InvertAndScale(270f);
			GameSelector_PlayerName_Pos = new Point(167, -190);
			GameSelector_LevelNumber_1_Pos = new Point(553, 32);
			GameSelector_LevelNumber_2_Pos = new Point(580, 33);
			GameSelector_LevelNumber_3_Pos = new Point(592, 34);
			GameSelector_LevelNumber_Bar = new Rectangle(573, 44, 5, 2);
			GameSelector_LevelNumber_ButtonDown_Offset = 6;
			GameSelector_Update_Offset = 110;
			LeaderboardScreen_Vasebreaker_Button_X = 633;
			LeaderboardScreen_Vasebreaker_Button_Y = 339;
			LeaderboardScreen_IZombie_Button_X = 125;
			LeaderboardScreen_IZombie_Button_Y = 306;
			LeaderboardScreen_Killed_Button_X = 405;
			LeaderboardScreen_Killed_Button_Y = 306;
			LeaderboardScreen_PileBase_X = 331;
			LeaderboardScreen_PileBase_Y = 53;
			Leaderboard_IZombie_Score_X = 198;
			Leaderboard_IZombie_Score_Y = 290;
			Leaderboard_Vasebreaker_Score_X = 698;
			Leaderboard_Vasebreaker_Score_Y = 322;
			Leaderboard_Pile_1_X = 297;
			LeaderboardDialog_CannotConnect_Rect = new TRect(30, 70, 570, 300);
			LeaderboardScreen_EdgeOfSpace_Overlay_Offset = 30;
			AchievementWidget_ROW_HEIGHT = 135;
			AchievementWidget_ROW_START = 275;
			AchievementWidget_HOLE_DEPTH = 128;
			AchievementWidget_GAMERSCORE_POS = new Point(217, 345);
			AchievementWidget_BackButton_X = 152;
			AchievementWidget_BackButton_Y = 6;
			AchievementWidget_BackButton_Rect = new TRect(152, 0, 170, 120);
			AchievementWidget_Background_Offset_Y = 412;
			AchievementWidget_Description_Box = new Rectangle(270, 30, 400, 70);
			AchievementWidget_Pipe_Offset = new Point(0, 140);
			AchievementWidget_Worm_Offset = new Point(659, -104);
			AchievementWidget_ZombieWorm_Offset = new Point(76, -9);
			AchievementWidget_GemLeft_Offset = new Point(180, 0);
			AchievementWidget_GemRight_Offset = new Point(450, -69);
			AchievementWidget_Fossile_Offset = new Point(239, -55);
			AchievementWidget_Image_Pos = new Point(200, 10);
			AchievementWidget_Image_Size = 64;
			AchievementWidget_Name_Pos = new Point(270, 0);
			AchievementWidget_Name_MaxWidth = 400;
			QuickPlayWidget_Thumb_X = 20;
			QuickPlayWidget_Thumb_Y = 15;
			QuickPlayWidget_Bungee_Y = -52;
			SeedChooserScreen_MenuButton_X = 665;
			SeedChooserScreen_Background_Top = new Point(88, 4);
			SeedChooserScreen_Background_Middle = new Point(SeedChooserScreen_Background_Top.X, SeedChooserScreen_Background_Top.Y + 44);
			SeedChooserScreen_Background_Middle_Height = 325;
			SeedChooserScreen_Background_Bottom = new Point(SeedChooserScreen_Background_Top.X, SeedChooserScreen_Background_Middle.Y + SeedChooserScreen_Background_Middle_Height);
			SeedChooserScreen_Gradient_Top = new Rectangle(0, 40, 336, 22);
			SeedChooserScreen_Gradient_Bottom = new Rectangle(0, 362, 336, 22);
			SeedPacket_Cost = new Point(42, 26);
			SeedPacket_Cost_IZombie = new Point(41, 27);
			SeedPacket_CostText_Pos = new Point(74, 22);
			SeedPacket_CostText_IZombie_Pos = new Point(73, 23);
			ImitaterDialog_Size = new Point(0, 240);
			ImitaterDialog_ScrollWidget_Offset_X = 5;
			ImitaterDialog_ScrollWidget_Y = 120;
			ImitaterDialog_ScrollWidget_ExtraWidth = 20;
			ImitaterDialog_Height = 250;
			ImitaterDialog_BottomGradient_Y = 235;
			CutScene_ReadySetPlant_Pos = new Point(370, 260);
			CutScene_LogoEndPos = 350;
			CutScene_LogoBackRect_Height = 150;
			CutScene_LogoEnd_Particle_Pos = new Point(600, 400);
			CutScene_ExtraRoom_1_Particle_Pos = new Point(35, 348);
			CutScene_ExtraRoom_2_Particle_Pos = new Point(CutScene_ExtraRoom_1_Particle_Pos.X, 246);
			CutScene_ExtraRoom_3_Particle_Pos = new Point(CutScene_ExtraRoom_1_Particle_Pos.X, 459);
			CutScene_ExtraRoom_4_Particle_Pos = new Point(32, 150);
			CutScene_ExtraRoom_5_Particle_Pos = new Point(32, 551);
			CutScene_SodRoll_1_Pos = -105;
			CutScene_SodRoll_2_Pos = 107;
			CutScene_SodRoll_3_Pos = new Point(-6, -205);
			CutScene_SodRoll_4_Pos = new Point(-6, 205);
			CutScene_Upsell_TerraCotta_Arrow = new Point(733, 360);
			CutScene_Upsell_TerraCotta_Pot = new Point(565, 360);
			ConveyorBeltClipRect = new TRect(0, 8, 83, 465);
			StoreScreen_BackButton_X = 430;
			StoreScreen_BackButton_Y = 390;
			StoreScreen_Car_X = 260;
			StoreScreen_Car_Y = 80;
			StoreScreen_PrevButton_X = 317;
			StoreScreen_PrevButton_Y = 296;
			StoreScreen_NextButton_X = 645;
			StoreScreen_NextButton_Y = 296;
			StoreScreen_HatchOpen_X = 316;
			StoreScreen_HatchOpen_Y = -35;
			StoreScreen_HatchClosed_X = 309;
			StoreScreen_HatchClosed_Y = 81;
			StoreScreen_CarNight_X = 260;
			StoreScreen_CarNight_Y = 80;
			StoreScreen_StoreSign_X = 450;
			StoreScreen_StoreSign_Y_Min = -150;
			StoreScreen_StoreSign_Y_Max = -10;
			StoreScreen_Coinbank_X = 590;
			StoreScreen_Coinbank_Y = 405;
			StoreScreen_Coinbank_TextOffset = new Point(180, 8);
			StoreScreen_ItemOffset_1_X = 430;
			StoreScreen_ItemOffset_1_Y = 102;
			StoreScreen_ItemOffset_2_X = 390;
			StoreScreen_ItemOffset_2_Y = 190;
			StoreScreen_ItemSize = 85;
			StoreScreen_ItemSize_Offset = 4;
			StoreScreen_PriceTag_X = 3;
			StoreScreen_PriceTag_Y = 70;
			StoreScreen_PriceTag_Text_Offset_X = 35;
			StoreScreen_PriceTag_Text_Offset_Y = 67;
			StoreScreen_ComingSoon_X = 100;
			StoreScreen_ComingSoon_Y = 70;
			StoreScreen_SoldOut_Width = 80;
			StoreScreen_SoldOut_Y = 0;
			StoreScreen_SoldOut_Height = 100;
			StoreScreen_PacketUpgrade_X = 10;
			StoreScreen_PacketUpgrade_Y = 5;
			StoreScreen_PacketUpgrade_Text_Size = new Rectangle(0, 0, 80, 60);
			StoreScreen_RetardedDave_Offset_X = -10;
			StoreScreen_FirstAidNut_Offset_Y = 10;
			StoreScreen_PoolCleaner_Offset_X = 23;
			StoreScreen_PoolCleaner_Offset_Y = 24;
			StoreScreen_Rake_Offset_X = -10;
			StoreScreen_Rake_Offset_Y = 24;
			StoreScreen_RoofCleaner_Offset_X = 15;
			StoreScreen_RoofCleaner_Offset_Y = 38;
			StoreScreen_Imitater_Offset_X = -1;
			StoreScreen_Imitater_Offset_Y = 24;
			StoreScreen_Default_Offset_Y = 20;
			StoreScreen_MouseRegion = new Point(75, 100);
			StoreScreen_Dialog = new Rectangle(200, 50, 400, 450);
			StoreScreen_PotPlant_Offset = new Point(40, 70);
			StoreScreenMushroomGardenOffsetX = 15;
			StoreScreenAquariumGardenOffsetX = 15;
			StoreScreenGloveOffsetY = 15;
			StoreScreenWheelbarrowOffsetY = 15;
			StoreScreenBugSprayOffsetX = 17;
			StoreScreenBugSprayOffsetY = 15;
			StoreScreenPhonographOffsetY = 15;
			StoreScreenPlantFoodOffsetX = 13;
			StoreScreenPlantFoodOffsetY = 15;
			StoreScreenWateringCanOffsetX = 6;
			StoreScreenWateringCanOffsetY = 15;
			NewOptionsDialog_FXLabel_X = 340;
			NewOptionsDialog_FXLabel_Y = 143;
			NewOptionsDialog_MusicLabel_X = 340;
			NewOptionsDialog_MusicLabel_On_Y = 97;
			NewOptionsDialog_MusicLabel_Off_Y = 103;
			NewOptionsDialog_VibrationLabel_X = 340;
			NewOptionsDialog_VibrationLabel_Y = 193;
			NewOptionsDialog_LockedLabel_Y = 265;
			NewOptionsDialog_VibrationLabel_MaxWidth = 300;
			NewOptionsDialog_FullScreenOffset = 25;
			NewOptionsDialog_FX_Offset = 15;
			NewOptionsDialog_Music_Offset = 5;
			NewOptionsDialog_Version_Low_Y = 340;
			NewOptionsDialog_Version_High_Y = 345;
			RetardedDave_Bubble_Tip_Offset = 5;
			RetardedDave_Bubble_Offset_Shop = new Point(-280, -117);
			RetardedDave_Bubble_Offset_Board = new Point(-200, -60);
			RetardedDave_Bubble_Size = 400;
			RetardedDave_Bubble_Rect = new TRect(15, 6, 0, 250);
			RetardedDave_Bubble_TapToContinue_Y = 230;
			PlantGallerySize = new Point(390, 750);
			ZombieGallerySize = new Point(415, 1068);
			AlmanacHeaderY = 8;
			Almanac_PlantsButtonRect = new TRect(67, 150, 300, 240);
			Almanac_ZombiesButtonRect = new TRect(450, 150, 300, 240);
			Almanac_CloseButtonRect = new TRect(660, 435, 148, 39);
			Almanac_IndexButtonRect = new TRect(442, 435, 148, 39);
			Almanac_IndexPlantPos = new Point(183, 240);
			Almanac_IndexZombiePos = new Point(553, 225);
			Almanac_IndexPlantTextPos = new Point(Almanac_PlantsButtonRect.mX + Almanac_PlantsButtonRect.mWidth / 2, Almanac_PlantsButtonRect.mY + Almanac_PlantsButtonRect.mHeight - 65);
			Almanac_IndexZombieTextPos = new Point(Almanac_ZombiesButtonRect.mX + Almanac_ZombiesButtonRect.mWidth / 2, Almanac_ZombiesButtonRect.mY + Almanac_ZombiesButtonRect.mHeight - 65);
			Almanac_PlantScrollRect = new TRect(47, 75, 380, 368);
			Almanac_ZombieScrollRect = new TRect(28, 75, 410, 368);
			Almanac_DescriptionScrollRect = new TRect(470, 255, 305, 167);
			Almanac_PlantTopGradientY = 74;
			Almanac_ZombieTopGradientY = 72;
			Almanac_BottomGradientY = 432;
			Almanac_PlantGradientWidth = 360;
			Almanac_ZombieGradientWidth = 390;
			Almanac_ZombieStoneRect = new TRect(445, 2, 353, 465);
			Almanac_BackgroundPosition = new Point(533, 60);
			Almanac_ZombieClipRect = new TRect(-40, -45, 175, 175);
			Almanac_NavyRect = new TRect(515, 40, 196, 198);
			Almanac_NamePosition = new Point(622, 0);
			Almanac_ClayRect = new TRect(450, 6, 340, 465);
			Almanac_BrownRect = new TRect(513, 41, 198, 198);
			Almanac_ZombieSpace = new Point(137, 117);
			Almanac_ZombieOffset = new Point(0, 5);
			Almanac_BossPosition = new Point(205, 944);
			Almanac_ImpPosition = new Point(68, 944);
			Almanac_ImitatorPosition = new Point(PlantGallerySize.X / 2 - 52, 688);
			Almanac_SeedSpace = new Point(4, 4);
			Almanac_SeedOffset = new Point(16, 8);
			Almanac_ZombiePosition = new Point(700, 120);
			Almanac_PlantPosition = new Point(583, 105);
			Almanac_Text_Scale = 0.8f;
			Almanac_PlantsHeader_Pos = new Point(32, 2);
			Almanac_ZombieHeader_Pos = new Point(25, 2);
			Almanac_ItemName_MaxWidth = 330;
			Zombie_StartOffset = 110f;
			Zombie_StartRandom_Offset = 40f;
			AwardScreen_Note_Credits_Background = new Rectangle(-500, -170, 1600, 800);
			AwardScreen_Note_Credits_Paper = new Point(138, 50);
			AwardScreen_Note_Credits_Text = new Rectangle(180, 80, 429, 287);
			AwardScreen_Note_Help_Background = new Rectangle(-350, -150, 1700, 700);
			AwardScreen_Note_Help_Paper = new Point(138, 50);
			AwardScreen_Note_Help_Text = new Point(180, 95);
			AwardScreen_Note_1_Background = new Rectangle(-300, -150, 1600, 700);
			AwardScreen_Note_1_Paper = new Point(138, 50);
			AwardScreen_Note_1_Text = new Point(180, 85);
			AwardScreen_Note_Message = new Point(400, 0);
			AwardScreen_Note_2_Background = new Rectangle(-300, -150, 1600, 700);
			AwardScreen_Note_2_Paper = new Point(138, 50);
			AwardScreen_Note_2_Text = new Point(175, 85);
			AwardScreen_Note_3_Background = new Rectangle(-300, -150, 1600, 700);
			AwardScreen_Note_3_Paper = new Point(138, 50);
			AwardScreen_Note_3_Text = new Point(165, 80);
			AwardScreen_Note_4_Background = new Rectangle(-300, -150, 1600, 700);
			AwardScreen_Note_4_Paper = new Point(138, 50);
			AwardScreen_Note_4_Text = new Point(160, 80);
			AwardScreen_Note_Final_Background = new Rectangle(-300, -150, 1600, 700);
			AwardScreen_Note_Final_Paper = new Point(138, 50);
			AwardScreen_Note_Final_Text = new Point(170, 95);
			AwardScreen_Bacon = (AwardScreen_Taco = (AwardScreen_CarKeys = new Point(191, 210)));
			AwardScreen_WateringCan = new Point(180, 210);
			AwardScreen_Almanac = new Point(195, 185);
			AwardScreen_Trophy = new Point((int)(400f * S), (int)(137f * S));
			AwardScreen_Shovel = new Point(195, 195);
			AwardScreen_MenuButton = new Point(610, 420);
			AwardScreen_ClayTablet = new TRect(50, 70, 700, 300);
			AwardScreen_TitlePos = new Point(400, 10);
			AwardScreen_GroundDay_Pos = new Point(111, 167);
			AwardScreen_BrownRect = new TRect(90, 145, 200, 200);
			AwardScreen_BottomMessage_Pos = new Point(400, 70);
			AwardScreen_BottomText_Rect_Text = new TRect(317, 160, 400, 170);
			AwardScreen_BottomText_Rect_NoText = new TRect(150, 120, 500, 200);
			AwardScreen_CreditsButton = new TRect(180, 413, 140, 48);
			AwardScreen_CreditsButton_Offset = new Point(50, -7);
			if (Language == LanguageIndex.es)
			{
				AwardScreen_CreditsButton_Offset.X = 70;
			}
			else if (Language == LanguageIndex.it)
			{
				AwardScreen_CreditsButton_Offset.X = 70;
			}
			else if (Language == LanguageIndex.de)
			{
				AwardScreen_CreditsButton_Offset.X = 60;
			}
			AwardScreen_Seed_Pos = new Point(151, 220);
			AwardScreen_ContinueButton_Offset = 35;
			AwardScreen_AchievementImage_Pos = new Point(80, 4);
			AwardScreen_AchievementName_Pos = new Point(200, 5);
			AwardScreen_AchievementDescription_Rect = new Rectangle(200, 30, 400, 60);
			CreditScreen_ReplayButton = new Rectangle(100, 400, 200, 48);
			CreditScreen_ReplayButton_TextOffset = new Point(-5, -7);
			CreditScreen_MainMenu = new Rectangle(500, 400, 200, 50);
			CreditScreen_TextStart = 200;
			CreditScreen_TextEnd = 400;
			CreditScreen_TextClip = 370;
			CreditScreen_LeftText_X = 0;
			CreditScreen_RightText_X = 390;
			CreditScreen_LeftRight_Text_Width = 370;
			ConfirmPurchaseDialog_Background = new TRect(38, 100, 312, 250);
			ConfirmPurchaseDialog_Item_Pos = new Point(60, 155);
			ConfirmPurchaseDialog_Text = new TRect(145, 95, 190, 265);
			LawnDialog_Insets = new Insets(60, 20, 70, 35);
			UILevelPosition = new Point(793, 445);
			UIMenuButtonPosition = new Point(650 - Board_Offset_AspectRatio_Correction, 3);
			UIMenuButtonWidth = 150;
			UISunBankPositionX = 95;
			UISunBankTextOffset = new Point(92, 3);
			UIShovelButtonPosition = new Point(260, 3);
			UIProgressMeterPosition = new Point(370, 3);
			UIProgressMeterHeadEnd = 220;
			UIProgressMeterBarEnd = 225;
			UICoinBankPosition = new Point(110 - Board_Offset_AspectRatio_Correction, 479);
			Board_Cutscene_ExtraScroll = 170;
			Board_SunCoinRange = 550;
			Board_SunCoin_CollectTarget = new Point(115 - (int)((float)Board_Offset_AspectRatio_Correction * IS), 5);
			Board_Cel_Y_Values_Normal = new int[7]
			{
				80,
				177,
				280,
				380,
				470,
				570,
				610
			};
			Board_Cel_Y_Values_Pool = new int[7]
			{
				80,
				170,
				275,
				360,
				447,
				522,
				610
			};
			Board_Cel_Y_Values_ZenGarden = new int[7]
			{
				125,
				225,
				335,
				455,
				560,
				750,
				750
			};
			Board_GameOver_Interior_Overlay_1 = new Point(-15, 182);
			Board_GameOver_Interior_Overlay_2 = new Point(-17, 160);
			Board_GameOver_Interior_Overlay_3 = new Point(-60, 178);
			Board_GameOver_Interior_Overlay_4 = new Point(-56, 202);
			Board_GameOver_Exterior_Overlay_1 = new Point(-22, 142);
			Board_GameOver_Exterior_Overlay_2 = new Point(-21, 148);
			Board_GameOver_Exterior_Overlay_3 = new Point(-57, 166);
			Board_GameOver_Exterior_Overlay_4 = new Point(-58, 105);
			Board_GameOver_Exterior_Overlay_5 = new Point(5, 179);
			Board_GameOver_Exterior_Overlay_6 = new Point(5, 179);
			Board_Ice_Start = 1000;
			Board_ProgressBarText_Pos = 1;
			MessageWidget_SlotMachine_Y = 518;
			LawnMower_Coin_Offset = new Point(20, 5);
			DescriptionWidget_ScrollBar_Padding = 10;
			Zombie_Bungee_Offset = new Point(61, 15);
			Zombie_Bungee_Target_Offset = new Point(5, 40);
			ZOMBIE_BACKUP_DANCER_RISE_HEIGHT = -145;
			Zombie_Dancer_Dance_Limit_X = 1000;
			Zombie_Dancer_Spotlight_Scale = 2.7f;
			Zombie_Dancer_Spotlight_Offset = new Point(-45, 50);
			Zombie_Dancer_Spotlight_Pos = new Point(2, -560);
			Zombie_ClipOffset_Default = 100;
			Zombie_ClipOffset_Snorkel = 0;
			Zombie_ClipOffset_Snorkel_intoPool_Small = 40;
			Zombie_ClipOffset_Snorkel_Dying = 76;
			Zombie_ClipOffset_Snorkel_Dying_Small = 65;
			Zombie_ClipOffset_Pail = 78;
			Zombie_ClipOffset_Normal = 108;
			Zombie_ClipOffset_Digger = 75;
			Zombie_ClipOffset_Dolphin_Into_Pool = 65;
			Zombie_ClipOffset_Snorkel_Grabbed = 75;
			Zombie_ClipOffset_PeaHead_InPool = 78;
			Zombie_ClipOffset_RisingFromGrave = 20;
			Zombie_ClipOffset_RisingFromGrave_Small = -35;
			Zombie_ClipOffset_Snorkel_Into_Pool = 80;
			Zombie_ClipOffset_Normal_In_Pool = 30;
			Zombie_ClipOffset_Flag_In_Pool = 30;
			Zombie_ClipOffset_Normal_In_Pool_SMALL = -40;
			Zombie_ClipOffset_TrafficCone_In_Pool_SMALL = -55;
			Zombie_ClipOffset_Ducky_Dying_In_Pool = 27;
			Zombie_GameOver_ClipOffset_1 = 77;
			Zombie_GameOver_ClipOffset_2 = 80;
			Zombie_GameOver_ClipOffset_3 = 85;
			ZombieGalleryWidget_Window_Offset = new Point(3, 7);
			ZombieGalleryWidget_Window_Clip = new Rectangle(6, 4, 100, 100);
			Coin_AwardSeedpacket_Pos = new Point(500 - Board_Offset_AspectRatio_Correction, 300);
			Coin_Glow_Offset = new Point(8, 6);
			Coin_Silver_Offset = 6f;
			Coin_MoneyBag_Offset = new Point(-50, -10);
			Coin_Shovel_Offset = new Point(5, 30);
			Coin_Silver_Award_Offset = new Point(21, 40);
			Coin_Almanac_Offset = new Point(0, 0);
			Coin_Note_Offset = new Point(0, 10);
			Coin_CarKeys_Offset = new Point(10, 20);
			Coin_Taco_Offset = new Point(0, 20);
			Coin_Bacon_Offset = new Point(0, 20);
			Plant_CobCannon_Projectile_Offset = new Point(-30, -160);
			Plant_Squished_Offset = new Point(5, 20);
			IZombieBrainPosition = 140;
			IZombie_SeedOffset = new Point(4, 2);
			IZombie_ClipOffset = new Rectangle(0, 0, 80, 50);
			ZombieOffsets = new Point[26]
			{
				new Point(20, 11),
				new Point(13, 15),
				new Point(22, -30),
				new Point(-30, 0),
				new Point(20, -30),
				new Point(18, 13),
				new Point(19, 13),
				new Point(0, -10),
				new Point(14, -10),
				new Point(19, 12),
				new Point(20, 13),
				new Point(16, 10),
				new Point(1, 11),
				new Point(16, 10),
				new Point(16, 15),
				new Point(14, 10),
				new Point(13, 12),
				new Point(10, -10),
				new Point(20, 12),
				new Point(11, 12),
				new Point(-20, -10),
				new Point(15, -10),
				new Point(4, 12),
				new Point(18, 10),
				new Point(23, 23),
				new Point(1, 15)
			};
			LastStandButtonRect = new TRect(240, 439, 210, 46);
			VasebreakerJackInTheBoxOffset = -1;
			JackInTheBoxPlantRadius = 100;
			JackInTheBoxZombieRadius = 192;
			ZenGardenGreenhouseMultiplierX = 1.125f;
			ZenGardenGreenhouseMultiplierY = 1.182f;
			ZenGardenGreenhouseOffset = new Point(-30, 30);
			ZenGardenMushroomGardenOffset = new Point(10, 30);
			ZenGardenStoreButtonX = 628;
			ZenGardenStoreButtonY = 45;
			ZenGardenTopButtonStart = -77;
			ZenGardenButtonCounterOffset = new Point(60, 45);
			ZenGardenButton_GoldenWateringCan_Offset = new Point(3, 5);
			ZenGardenButton_NormalWateringCan_Offset = new Point(3, 5);
			ZenGardenButton_Fertiliser_Offset = new Point(9, 4);
			ZenGardenButton_BugSpray_Offset = new Point(17, 6);
			ZenGardenButton_Phonograph_Offset = new Point(2, 5);
			ZenGardenButton_Chocolate_Offset = new Point(12, 10);
			ZenGardenButton_Glove_Offset = new Point(5, 5);
			ZenGardenButton_MoneySign_Offset = new Point(5, 5);
			ZenGardenButton_NextGarden_Offset = new Point(2, 0);
			ZenGardenButton_Wheelbarrow_Offset = new Point(2, 2);
			ZenGardenButton_WheelbarrowPlant_Offset = new Point(40, 32);
			ZenGardenButton_Wheelbarrow_Facing_Offset = 43f;
			ZenGarden_Backdrop_X = 95;
			ZenGarden_SellDialog_Offset = new Point(214, 90);
			ZenGarden_NextGarden_Pos = new Point(500, 0);
			ZenGarden_RetardedDaveBubble_Pos = new Point(-250, -120);
			ZenGarden_WaterDrop_Pos = new Point(65, 6);
			ZenGarden_Fertiliser_Pos = new Point(60, 7);
			ZenGarden_Phonograph_Pos = new Point(60, 7);
			ZenGarden_BugSpray_Pos = new Point(67, 9);
			ZenGarden_PlantSpeechBubble_Pos = new Point(50, 0);
			ZenGarden_StinkySpeechBubble_Pos = new Point(50, -20);
			ZenGarden_GoldenWater_Pos = new Point(-10, -10);
			ZenGarden_Chocolate_Pos = new Point(65, -8);
			ZenGarden_MoneyTarget_X = 477;
			ZenGarden_TutorialArrow_Offset = 98;
			ZEN_XMIN = -50;
			ZEN_YMIN = 75;
			ZEN_XMAX = 700;
			ZEN_YMAX = 500;
			ZenGarden_GoldenWater_Size = new Rectangle(-100, -100, 100, 100);
			STINKY_SLEEP_POS_Y = 540;
			gMushroomGridPlacement = new SpecialGridPlacement[8]
			{
				new SpecialGridPlacement(80, 435, 0, 0),
				new SpecialGridPlacement(220, 360, 1, 0),
				new SpecialGridPlacement(290, 458, 2, 0),
				new SpecialGridPlacement(355, 296, 3, 0),
				new SpecialGridPlacement(387, 203, 4, 0),
				new SpecialGridPlacement(470, 380, 5, 0),
				new SpecialGridPlacement(500, 472, 6, 0),
				new SpecialGridPlacement(580, 283, 7, 0)
			};
			ZenGarden_Marigold_Sprout_Offset = new Point(24, 30);
			ZenGarden_Aquarium_ShadowOffset = new Point(35, 0);
			Challenge_SeeingStars_StarfruitPreview_Offset_Y = 14;
			Challenge_SlotMachine_Pos = new Point(210, 425);
			Challenge_SlotMachineHandle_Pos = new TRect(672, 425, 100, 80);
			Challenge_SlotMachine_Gap = 13;
			Challenge_SlotMachine_Offset = 92;
			Challenge_SlotMachine_Shadow_Offset = 3;
			Challenge_SlotMachine_Y_Offset = 7;
			Challenge_SlotMachine_Y_Pos = 3;
			Challenge_SlotMachine_ClipHeight = 40;
			Challenge_BeghouldedTwist_Offset = new Point(50, 50);
			GridItem_ScaryPot_SeedPacket_Offset = new Point(-2, 35);
			GridItem_ScaryPot_Zombie_Offset = new Point(23, 30);
			GridItem_ScaryPot_ZombieFootball_Offset = new Point(15, 25);
			GridItem_ScaryPot_ZombieGargantuar_Offset = new Point(-7, -5);
			GridItem_ScaryPot_Sun_Offset = new Point(42, 62);
		}
	}
}
