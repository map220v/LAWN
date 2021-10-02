using Sexy;
using System;

internal static class Resources
{
	public enum ResourceId
	{
		IMAGE_POPCAP_LOGO_ID,
		IMAGE_POPCAP_LOGO_REGISTERED_ID,
		IMAGE_TITLESCREEN_ID,
		IMAGE_LOADING_ID,
		IMAGE_PVZ_LOGO_ID,
		SOUND_BUTTONCLICK_ID,
		SOUND_LOADINGBAR_FLOWER_ID,
		SOUND_LOADINGBAR_ZOMBIE_ID,
		FONT_BRIANNETOD16_ID,
		FONT_HOUSEOFTERROR16_ID,
		FONT_CONTINUUMBOLD14_ID,
		FONT_CONTINUUMBOLD14OUTLINE_ID,
		FONT_DWARVENTODCRAFT12_ID,
		FONT_DWARVENTODCRAFT15_ID,
		FONT_DWARVENTODCRAFT18_ID,
		FONT_PICO129_ID,
		FONT_BRIANNETOD12_ID,
		IMAGE_CHARREDZOMBIES_ID,
		IMAGE_ALMANACUI_ID,
		IMAGE_SEEDATLAS_ID,
		IMAGE_DAVE_ID,
		IMAGE_DIALOG_ID,
		IMAGE_CONVEYORBELT_BACKDROP_ID,
		IMAGE_CONVEYORBELT_BELT_ID,
		IMAGE_SPEECHBUBBLE_ID,
		IMAGE_LOC_EN_ID,
		IMAGE_ZOMBIE_NOTE_SMALL_ID,
		IMAGE_REANIM_ZOMBIESWON_ID,
		IMAGE_SCARY_POT_ID,
		SOUND_AWOOGA_ID,
		SOUND_BLEEP_ID,
		SOUND_BUZZER_ID,
		SOUND_CHOMP_ID,
		SOUND_CHOMP2_ID,
		SOUND_CHOMPSOFT_ID,
		SOUND_FLOOP_ID,
		SOUND_FROZEN_ID,
		SOUND_GULP_ID,
		SOUND_GROAN_ID,
		SOUND_GROAN2_ID,
		SOUND_GROAN3_ID,
		SOUND_GROAN4_ID,
		SOUND_GROAN5_ID,
		SOUND_GROAN6_ID,
		SOUND_LOSEMUSIC_ID,
		SOUND_MINDCONTROLLED_ID,
		SOUND_PAUSE_ID,
		SOUND_PLANT_ID,
		SOUND_PLANT2_ID,
		SOUND_POINTS_ID,
		SOUND_SEEDLIFT_ID,
		SOUND_SIREN_ID,
		SOUND_SLURP_ID,
		SOUND_SPLAT_ID,
		SOUND_SPLAT2_ID,
		SOUND_SPLAT3_ID,
		SOUND_SUKHBIR4_ID,
		SOUND_SUKHBIR5_ID,
		SOUND_SUKHBIR6_ID,
		SOUND_TAP_ID,
		SOUND_TAP2_ID,
		SOUND_THROW_ID,
		SOUND_THROW2_ID,
		SOUND_BLOVER_ID,
		SOUND_WINMUSIC_ID,
		SOUND_LAWNMOWER_ID,
		SOUND_BOING_ID,
		SOUND_JACKINTHEBOX_ID,
		SOUND_DIAMOND_ID,
		SOUND_DOLPHIN_APPEARS_ID,
		SOUND_DOLPHIN_BEFORE_JUMPING_ID,
		SOUND_POTATO_MINE_ID,
		SOUND_ZAMBONI_ID,
		SOUND_BALLOON_POP_ID,
		SOUND_THUNDER_ID,
		SOUND_ZOMBIESPLASH_ID,
		SOUND_BOWLING_ID,
		SOUND_BOWLINGIMPACT_ID,
		SOUND_BOWLINGIMPACT2_ID,
		SOUND_GRAVEBUSTERCHOMP_ID,
		SOUND_GRAVEBUTTON_ID,
		SOUND_LIMBS_POP_ID,
		SOUND_PLANTERN_ID,
		SOUND_POGO_ZOMBIE_ID,
		SOUND_SNOW_PEA_SPARKLES_ID,
		SOUND_PLANT_WATER_ID,
		SOUND_ZOMBIE_ENTERING_WATER_ID,
		SOUND_ZOMBIE_FALLING_1_ID,
		SOUND_ZOMBIE_FALLING_2_ID,
		SOUND_PUFF_ID,
		SOUND_FUME_ID,
		SOUND_HUGE_WAVE_ID,
		SOUND_SLOT_MACHINE_ID,
		SOUND_COIN_ID,
		SOUND_ROLL_IN_ID,
		SOUND_DIGGER_ZOMBIE_ID,
		SOUND_HATCHBACK_CLOSE_ID,
		SOUND_HATCHBACK_OPEN_ID,
		SOUND_KERNELPULT_ID,
		SOUND_KERNELPULT2_ID,
		SOUND_ZOMBAQUARIUM_DIE_ID,
		SOUND_BUNGEE_SCREAM_ID,
		SOUND_BUNGEE_SCREAM2_ID,
		SOUND_BUNGEE_SCREAM3_ID,
		SOUND_BUTTER_ID,
		SOUND_JACK_SURPRISE_ID,
		SOUND_JACK_SURPRISE2_ID,
		SOUND_NEWSPAPER_RARRGH_ID,
		SOUND_NEWSPAPER_RARRGH2_ID,
		SOUND_NEWSPAPER_RIP_ID,
		SOUND_SQUASH_HMM_ID,
		SOUND_SQUASH_HMM2_ID,
		SOUND_VASE_BREAKING_ID,
		SOUND_POOL_CLEANER_ID,
		SOUND_MAGNETSHROOM_ID,
		SOUND_LADDER_ZOMBIE_ID,
		SOUND_GARGANTUAR_THUMP_ID,
		SOUND_BASKETBALL_ID,
		SOUND_FIREPEA_ID,
		SOUND_IGNITE_ID,
		SOUND_IGNITE2_ID,
		SOUND_READYSETPLANT_ID,
		SOUND_DOOMSHROOM_ID,
		SOUND_EXPLOSION_ID,
		SOUND_FINALWAVE_ID,
		SOUND_REVERSE_EXPLOSION_ID,
		SOUND_RVTHROW_ID,
		SOUND_SHIELDHIT_ID,
		SOUND_SHIELDHIT2_ID,
		SOUND_BOSSEXPLOSION_ID,
		SOUND_CHERRYBOMB_ID,
		SOUND_BONK_ID,
		SOUND_SWING_ID,
		SOUND_RAIN_ID,
		SOUND_LIGHTFILL_ID,
		SOUND_PLASTICHIT_ID,
		SOUND_PLASTICHIT2_ID,
		SOUND_JALAPENO_ID,
		SOUND_BALLOONINFLATE_ID,
		SOUND_BIGCHOMP_ID,
		SOUND_MELONIMPACT_ID,
		SOUND_MELONIMPACT2_ID,
		SOUND_PLANTGROW_ID,
		SOUND_SHOOP_ID,
		SOUND_JUICY_ID,
		SOUND_COFFEE_ID,
		SOUND_WAKEUP_ID,
		SOUND_LOWGROAN_ID,
		SOUND_LOWGROAN2_ID,
		SOUND_PRIZE_ID,
		SOUND_YUCK_ID,
		SOUND_YUCK2_ID,
		SOUND_GRASSSTEP_ID,
		SOUND_SHOVEL_ID,
		SOUND_COBLAUNCH_ID,
		SOUND_WATERING_ID,
		SOUND_POLEVAULT_ID,
		SOUND_GRAVESTONE_RUMBLE_ID,
		SOUND_DIRT_RISE_ID,
		SOUND_FERTILIZER_ID,
		SOUND_PORTAL_ID,
		SOUND_SCREAM_ID,
		SOUND_PAPER_ID,
		SOUND_MONEYFALLS_ID,
		SOUND_IMP_ID,
		SOUND_IMP2_ID,
		SOUND_HYDRAULIC_SHORT_ID,
		SOUND_HYDRAULIC_ID,
		SOUND_GARGANTUDEATH_ID,
		SOUND_CERAMIC_ID,
		SOUND_BOSSBOULDERATTACK_ID,
		SOUND_CHIME_ID,
		SOUND_CRAZYDAVESHORT1_ID,
		SOUND_CRAZYDAVESHORT2_ID,
		SOUND_CRAZYDAVESHORT3_ID,
		SOUND_CRAZYDAVELONG1_ID,
		SOUND_CRAZYDAVELONG2_ID,
		SOUND_CRAZYDAVELONG3_ID,
		SOUND_CRAZYDAVEEXTRALONG1_ID,
		SOUND_CRAZYDAVEEXTRALONG2_ID,
		SOUND_CRAZYDAVEEXTRALONG3_ID,
		SOUND_CRAZYDAVECRAZY_ID,
		SOUND_DANCER_ID,
		SOUND_FINALFANFARE_ID,
		SOUND_CRAZYDAVESCREAM_ID,
		SOUND_CRAZYDAVESCREAM2_ID,
		SOUND_ACHIEVEMENT_ID,
		SOUND_BUGSPRAY_ID,
		SOUND_FERTILISER_ID,
		SOUND_PHONOGRAPH_ID,
		IMAGE_PILE_ID,
		IMAGE_PLANTSZOMBIES_ID,
		IMAGE_PARTICLES_ID,
		IMAGE_SLOTMACHINE_OVERLAY_ID,
		IMAGE_ZENGARDEN_ID,
		IMAGE_CACHED_ID,
		IMAGE_SELECTORSCREEN_ACHIEVEMENTS_TOP_BACKGROUND_ID,
		IMAGE_SELECTORSCREEN_MAIN_BACKGROUND_ID,
		IMAGE_SELECTORSCREEN_ACHIEVEMENTS_HOLE_ID,
		IMAGE_SELECTORSCREEN_ACHIEVEMENTS_HOLE_CHINA_ID,
		IMAGE_SELECTORSCREEN_QUICKPLAY_BACKGROUND_ID,
		IMAGE_GOODIES_ID,
		IMAGE_QUICKPLAY_ID,
		IMAGE_ACHIEVEMENT_GNOME_ID,
		IMAGE_MINIGAMES_ID,
		IMAGE_SELECTORSCREEN_MOREGAMES_BACKGROUND_ID,
		IMAGE_CREDITS_ZOMBIENOTE_ID,
		IMAGE_LEADERBOARDSCREEN_BACKGROUND_ID,
		IMAGE_BLACKHOLE_ID,
		IMAGE_EDGE_OF_SPACE_ID,
		IMAGE_STARS_1_ID,
		IMAGE_STARS_2_ID,
		IMAGE_STARS_3_ID,
		IMAGE_STARS_4_ID,
		IMAGE_BACKGROUND_GREENHOUSE_ID,
		IMAGE_AQUARIUM1_ID,
		IMAGE_BACKGROUND_MUSHROOMGARDEN_ID,
		IMAGE_BACKGROUND1_ID,
		IMAGE_BACKGROUND1_GAMEOVER_INTERIOR_OVERLAY_ID,
		IMAGE_BACKGROUND1_GAMEOVER_MASK_ID,
		IMAGE_BACKGROUND1UNSODDED_ID,
		IMAGE_BACKGROUND2_ID,
		IMAGE_BACKGROUND2_GAMEOVER_INTERIOR_OVERLAY_ID,
		IMAGE_BACKGROUND2_GAMEOVER_MASK_ID,
		IMAGE_BACKGROUND3_ID,
		IMAGE_BACKGROUND3_GAMEOVER_INTERIOR_OVERLAY_ID,
		IMAGE_BACKGROUND3_GAMEOVER_MASK_ID,
		IMAGE_BACKGROUND4_ID,
		IMAGE_BACKGROUND4_GAMEOVER_INTERIOR_OVERLAY_ID,
		IMAGE_BACKGROUND4_GAMEOVER_MASK_ID,
		IMAGE_FOG_ID,
		IMAGE_BACKGROUND5_ID,
		IMAGE_BACKGROUND5_GAMEOVER_MASK_ID,
		IMAGE_BACKGROUND6BOSS_ID,
		IMAGE_BACKGROUND6_GAMEOVER_MASK_ID,
		IMAGE_STORE_BACKGROUND_ID,
		IMAGE_STORE_BACKGROUNDNIGHT_ID,
		IMAGE_STORE_CAR_ID,
		IMAGE_STORE_CAR_NIGHT_ID,
		IMAGE_STORE_CARCLOSED_ID,
		IMAGE_STORE_CARCLOSED_NIGHT_ID,
		IMAGE_STORE_HATCHBACKOPEN_ID,
		IMAGE_ZOMBIE_NOTE_ID,
		IMAGE_ZOMBIE_NOTE1_ID,
		IMAGE_ZOMBIE_NOTE2_ID,
		IMAGE_ZOMBIE_NOTE3_ID,
		IMAGE_ZOMBIE_NOTE4_ID,
		IMAGE_ZOMBIE_FINAL_NOTE_ID,
		IMAGE_ZOMBIE_NOTE_HELP_ID,
		RESOURCE_ID_MAX
	}

	public static readonly Image LOAD_LOGO_IMAGE_DATA;

	public static Image IMAGE_POPCAP_LOGO;

	public static Image IMAGE_POPCAP_LOGO_REGISTERED;

	public static Image IMAGE_TITLESCREEN;

	public static Image IMAGE_LOADING;

	public static Image IMAGE_PVZ_LOGO;

	public static int SOUND_BUTTONCLICK;

	public static int SOUND_LOADINGBAR_FLOWER;

	public static int SOUND_LOADINGBAR_ZOMBIE;

	public static Font FONT_BRIANNETOD16;

	public static Font FONT_HOUSEOFTERROR16;

	public static Font FONT_CONTINUUMBOLD14;

	public static Font FONT_CONTINUUMBOLD14OUTLINE;

	public static Font FONT_DWARVENTODCRAFT12;

	public static Font FONT_DWARVENTODCRAFT15;

	public static Font FONT_DWARVENTODCRAFT18;

	public static Font FONT_PICO129;

	public static Font FONT_BRIANNETOD12;

	public static Image IMAGE_CHARREDZOMBIES;

	public static Image IMAGE_ALMANACUI;

	public static Image IMAGE_SEEDATLAS;

	public static Image IMAGE_DAVE;

	public static Image IMAGE_DIALOG;

	public static Image IMAGE_CONVEYORBELT_BACKDROP;

	public static Image IMAGE_CONVEYORBELT_BELT;

	public static Image IMAGE_SPEECHBUBBLE;

	public static Image IMAGE_LOC_EN;

	public static Image IMAGE_ZOMBIE_NOTE_SMALL;

	public static Image IMAGE_REANIM_ZOMBIESWON;

	public static Image IMAGE_SCARY_POT;

	public static int SOUND_AWOOGA;

	public static int SOUND_BLEEP;

	public static int SOUND_BUZZER;

	public static int SOUND_CHOMP;

	public static int SOUND_CHOMP2;

	public static int SOUND_CHOMPSOFT;

	public static int SOUND_FLOOP;

	public static int SOUND_FROZEN;

	public static int SOUND_GULP;

	public static int SOUND_GROAN;

	public static int SOUND_GROAN2;

	public static int SOUND_GROAN3;

	public static int SOUND_GROAN4;

	public static int SOUND_GROAN5;

	public static int SOUND_GROAN6;

	public static int SOUND_LOSEMUSIC;

	public static int SOUND_MINDCONTROLLED;

	public static int SOUND_PAUSE;

	public static int SOUND_PLANT;

	public static int SOUND_PLANT2;

	public static int SOUND_POINTS;

	public static int SOUND_SEEDLIFT;

	public static int SOUND_SIREN;

	public static int SOUND_SLURP;

	public static int SOUND_SPLAT;

	public static int SOUND_SPLAT2;

	public static int SOUND_SPLAT3;

	public static int SOUND_SUKHBIR4;

	public static int SOUND_SUKHBIR5;

	public static int SOUND_SUKHBIR6;

	public static int SOUND_TAP;

	public static int SOUND_TAP2;

	public static int SOUND_THROW;

	public static int SOUND_THROW2;

	public static int SOUND_BLOVER;

	public static int SOUND_WINMUSIC;

	public static int SOUND_LAWNMOWER;

	public static int SOUND_BOING;

	public static int SOUND_JACKINTHEBOX;

	public static int SOUND_DIAMOND;

	public static int SOUND_DOLPHIN_APPEARS;

	public static int SOUND_DOLPHIN_BEFORE_JUMPING;

	public static int SOUND_POTATO_MINE;

	public static int SOUND_ZAMBONI;

	public static int SOUND_BALLOON_POP;

	public static int SOUND_THUNDER;

	public static int SOUND_ZOMBIESPLASH;

	public static int SOUND_BOWLING;

	public static int SOUND_BOWLINGIMPACT;

	public static int SOUND_BOWLINGIMPACT2;

	public static int SOUND_GRAVEBUSTERCHOMP;

	public static int SOUND_GRAVEBUTTON;

	public static int SOUND_LIMBS_POP;

	public static int SOUND_PLANTERN;

	public static int SOUND_POGO_ZOMBIE;

	public static int SOUND_SNOW_PEA_SPARKLES;

	public static int SOUND_PLANT_WATER;

	public static int SOUND_ZOMBIE_ENTERING_WATER;

	public static int SOUND_ZOMBIE_FALLING_1;

	public static int SOUND_ZOMBIE_FALLING_2;

	public static int SOUND_PUFF;

	public static int SOUND_FUME;

	public static int SOUND_HUGE_WAVE;

	public static int SOUND_SLOT_MACHINE;

	public static int SOUND_COIN;

	public static int SOUND_ROLL_IN;

	public static int SOUND_DIGGER_ZOMBIE;

	public static int SOUND_HATCHBACK_CLOSE;

	public static int SOUND_HATCHBACK_OPEN;

	public static int SOUND_KERNELPULT;

	public static int SOUND_KERNELPULT2;

	public static int SOUND_ZOMBAQUARIUM_DIE;

	public static int SOUND_BUNGEE_SCREAM;

	public static int SOUND_BUNGEE_SCREAM2;

	public static int SOUND_BUNGEE_SCREAM3;

	public static int SOUND_BUTTER;

	public static int SOUND_JACK_SURPRISE;

	public static int SOUND_JACK_SURPRISE2;

	public static int SOUND_NEWSPAPER_RARRGH;

	public static int SOUND_NEWSPAPER_RARRGH2;

	public static int SOUND_NEWSPAPER_RIP;

	public static int SOUND_SQUASH_HMM;

	public static int SOUND_SQUASH_HMM2;

	public static int SOUND_VASE_BREAKING;

	public static int SOUND_POOL_CLEANER;

	public static int SOUND_MAGNETSHROOM;

	public static int SOUND_LADDER_ZOMBIE;

	public static int SOUND_GARGANTUAR_THUMP;

	public static int SOUND_BASKETBALL;

	public static int SOUND_FIREPEA;

	public static int SOUND_IGNITE;

	public static int SOUND_IGNITE2;

	public static int SOUND_READYSETPLANT;

	public static int SOUND_DOOMSHROOM;

	public static int SOUND_EXPLOSION;

	public static int SOUND_FINALWAVE;

	public static int SOUND_REVERSE_EXPLOSION;

	public static int SOUND_RVTHROW;

	public static int SOUND_SHIELDHIT;

	public static int SOUND_SHIELDHIT2;

	public static int SOUND_BOSSEXPLOSION;

	public static int SOUND_CHERRYBOMB;

	public static int SOUND_BONK;

	public static int SOUND_SWING;

	public static int SOUND_RAIN;

	public static int SOUND_LIGHTFILL;

	public static int SOUND_PLASTICHIT;

	public static int SOUND_PLASTICHIT2;

	public static int SOUND_JALAPENO;

	public static int SOUND_BALLOONINFLATE;

	public static int SOUND_BIGCHOMP;

	public static int SOUND_MELONIMPACT;

	public static int SOUND_MELONIMPACT2;

	public static int SOUND_PLANTGROW;

	public static int SOUND_SHOOP;

	public static int SOUND_JUICY;

	public static int SOUND_COFFEE;

	public static int SOUND_WAKEUP;

	public static int SOUND_LOWGROAN;

	public static int SOUND_LOWGROAN2;

	public static int SOUND_PRIZE;

	public static int SOUND_YUCK;

	public static int SOUND_YUCK2;

	public static int SOUND_GRASSSTEP;

	public static int SOUND_SHOVEL;

	public static int SOUND_COBLAUNCH;

	public static int SOUND_WATERING;

	public static int SOUND_POLEVAULT;

	public static int SOUND_GRAVESTONE_RUMBLE;

	public static int SOUND_DIRT_RISE;

	public static int SOUND_FERTILIZER;

	public static int SOUND_PORTAL;

	public static int SOUND_SCREAM;

	public static int SOUND_PAPER;

	public static int SOUND_MONEYFALLS;

	public static int SOUND_IMP;

	public static int SOUND_IMP2;

	public static int SOUND_HYDRAULIC_SHORT;

	public static int SOUND_HYDRAULIC;

	public static int SOUND_GARGANTUDEATH;

	public static int SOUND_CERAMIC;

	public static int SOUND_BOSSBOULDERATTACK;

	public static int SOUND_CHIME;

	public static int SOUND_CRAZYDAVESHORT1;

	public static int SOUND_CRAZYDAVESHORT2;

	public static int SOUND_CRAZYDAVESHORT3;

	public static int SOUND_CRAZYDAVELONG1;

	public static int SOUND_CRAZYDAVELONG2;

	public static int SOUND_CRAZYDAVELONG3;

	public static int SOUND_CRAZYDAVEEXTRALONG1;

	public static int SOUND_CRAZYDAVEEXTRALONG2;

	public static int SOUND_CRAZYDAVEEXTRALONG3;

	public static int SOUND_CRAZYDAVECRAZY;

	public static int SOUND_DANCER;

	public static int SOUND_FINALFANFARE;

	public static int SOUND_CRAZYDAVESCREAM;

	public static int SOUND_CRAZYDAVESCREAM2;

	public static int SOUND_ACHIEVEMENT;

	public static int SOUND_BUGSPRAY;

	public static int SOUND_FERTILISER;

	public static int SOUND_PHONOGRAPH;

	public static Image IMAGE_PILE;

	public static Image IMAGE_PLANTSZOMBIES;

	public static Image IMAGE_PARTICLES;

	public static Image IMAGE_SLOTMACHINE_OVERLAY;

	public static Image IMAGE_ZENGARDEN;

	public static Image IMAGE_CACHED;

	public static Image IMAGE_SELECTORSCREEN_ACHIEVEMENTS_TOP_BACKGROUND;

	public static Image IMAGE_SELECTORSCREEN_MAIN_BACKGROUND;

	public static Image IMAGE_SELECTORSCREEN_ACHIEVEMENTS_HOLE;

	public static Image IMAGE_SELECTORSCREEN_ACHIEVEMENTS_HOLE_CHINA;

	public static Image IMAGE_SELECTORSCREEN_QUICKPLAY_BACKGROUND;

	public static Image IMAGE_GOODIES;

	public static Image IMAGE_QUICKPLAY;

	public static Image IMAGE_ACHIEVEMENT_GNOME;

	public static Image IMAGE_MINIGAMES;

	public static Image IMAGE_SELECTORSCREEN_MOREGAMES_BACKGROUND;

	public static Image IMAGE_CREDITS_ZOMBIENOTE;

	public static Image IMAGE_LEADERBOARDSCREEN_BACKGROUND;

	public static Image IMAGE_BLACKHOLE;

	public static Image IMAGE_EDGE_OF_SPACE;

	public static Image IMAGE_STARS_1;

	public static Image IMAGE_STARS_2;

	public static Image IMAGE_STARS_3;

	public static Image IMAGE_STARS_4;

	public static Image IMAGE_BACKGROUND_GREENHOUSE;

	public static Image IMAGE_AQUARIUM1;

	public static Image IMAGE_BACKGROUND_MUSHROOMGARDEN;

	public static Image IMAGE_BACKGROUND1;

	public static Image IMAGE_BACKGROUND1_GAMEOVER_INTERIOR_OVERLAY;

	public static Image IMAGE_BACKGROUND1_GAMEOVER_MASK;

	public static Image IMAGE_BACKGROUND1UNSODDED;

	public static Image IMAGE_BACKGROUND2;

	public static Image IMAGE_BACKGROUND2_GAMEOVER_INTERIOR_OVERLAY;

	public static Image IMAGE_BACKGROUND2_GAMEOVER_MASK;

	public static Image IMAGE_BACKGROUND3;

	public static Image IMAGE_BACKGROUND3_GAMEOVER_INTERIOR_OVERLAY;

	public static Image IMAGE_BACKGROUND3_GAMEOVER_MASK;

	public static Image IMAGE_BACKGROUND4;

	public static Image IMAGE_BACKGROUND4_GAMEOVER_INTERIOR_OVERLAY;

	public static Image IMAGE_BACKGROUND4_GAMEOVER_MASK;

	public static Image IMAGE_FOG;

	public static Image IMAGE_BACKGROUND5;

	public static Image IMAGE_BACKGROUND5_GAMEOVER_MASK;

	public static Image IMAGE_BACKGROUND6BOSS;

	public static Image IMAGE_BACKGROUND6_GAMEOVER_MASK;

	public static Image IMAGE_STORE_BACKGROUND;

	public static Image IMAGE_STORE_BACKGROUNDNIGHT;

	public static Image IMAGE_STORE_CAR;

	public static Image IMAGE_STORE_CAR_NIGHT;

	public static Image IMAGE_STORE_CARCLOSED;

	public static Image IMAGE_STORE_CARCLOSED_NIGHT;

	public static Image IMAGE_STORE_HATCHBACKOPEN;

	public static Image IMAGE_ZOMBIE_NOTE;

	public static Image IMAGE_ZOMBIE_NOTE1;

	public static Image IMAGE_ZOMBIE_NOTE2;

	public static Image IMAGE_ZOMBIE_NOTE3;

	public static Image IMAGE_ZOMBIE_NOTE4;

	public static Image IMAGE_ZOMBIE_FINAL_NOTE;

	public static Image IMAGE_ZOMBIE_NOTE_HELP;

	public static bool gNeedRecalcVariableToIdMap = false;

	public static object[] gResources;

	public static bool ExtractResourcesByName(ResourceManager theManager, string theName)
	{
		if (theName == "Init")
		{
			return ExtractInitResources(theManager);
		}
		if (theName == "InitRegistered")
		{
			return ExtractInitRegisteredResources(theManager);
		}
		if (theName == "LoaderBar")
		{
			return ExtractLoaderBarResources(theManager);
		}
		if (theName == "LoaderBarFont")
		{
			return ExtractLoaderBarFontResources(theManager);
		}
		if (theName == "LoadingFonts")
		{
			return ExtractLoadingFontsResources(theManager);
		}
		if (theName == "LoadingImages")
		{
			return ExtractLoadingImagesResources(theManager);
		}
		if (theName == "LoadingSounds")
		{
			return ExtractLoadingSoundsResources(theManager);
		}
		if (theName == "DelayLoad_Pile")
		{
			return ExtractDelayLoad_PileResources(theManager);
		}
		if (theName == "DelayLoad_GamePlay")
		{
			return ExtractDelayLoad_GamePlayResources(theManager);
		}
		if (theName == "DelayLoad_ZenGarden")
		{
			return ExtractDelayLoad_ZenGardenResources(theManager);
		}
		if (theName == "DelayLoad_Cached")
		{
			return ExtractDelayLoad_CachedResources(theManager);
		}
		if (theName == "DelayLoad_MainMenu")
		{
			return ExtractDelayLoad_MainMenuResources(theManager);
		}
		if (theName == "DelayLoad_Credits")
		{
			return ExtractDelayLoad_CreditsResources(theManager);
		}
		if (theName == "DelayLoad_Leaderboard_Background")
		{
			return ExtractDelayLoad_Leaderboard_BackgroundResources(theManager);
		}
		if (theName == "DelayLoad_Leaderboard")
		{
			return ExtractDelayLoad_LeaderboardResources(theManager);
		}
		if (theName == "DelayLoad_Stars")
		{
			return ExtractDelayLoad_StarsResources(theManager);
		}
		if (theName == "DelayLoad_GreenHouseGarden")
		{
			return ExtractDelayLoad_GreenHouseGardenResources(theManager);
		}
		if (theName == "DelayLoad_Zombiquarium")
		{
			return ExtractDelayLoad_ZombiquariumResources(theManager);
		}
		if (theName == "DelayLoad_MushroomGarden")
		{
			return ExtractDelayLoad_MushroomGardenResources(theManager);
		}
		if (theName == "DelayLoad_Background1")
		{
			return ExtractDelayLoad_Background1Resources(theManager);
		}
		if (theName == "DelayLoad_BackgroundUnsodded")
		{
			return ExtractDelayLoad_BackgroundUnsoddedResources(theManager);
		}
		if (theName == "DelayLoad_Background2")
		{
			return ExtractDelayLoad_Background2Resources(theManager);
		}
		if (theName == "DelayLoad_Background3")
		{
			return ExtractDelayLoad_Background3Resources(theManager);
		}
		if (theName == "DelayLoad_Background4")
		{
			return ExtractDelayLoad_Background4Resources(theManager);
		}
		if (theName == "DelayLoad_Background5")
		{
			return ExtractDelayLoad_Background5Resources(theManager);
		}
		if (theName == "DelayLoad_Background6")
		{
			return ExtractDelayLoad_Background6Resources(theManager);
		}
		if (theName == "DelayLoad_Almanac")
		{
			return ExtractDelayLoad_AlmanacResources(theManager);
		}
		if (theName == "DelayLoad_Store")
		{
			return ExtractDelayLoad_StoreResources(theManager);
		}
		if (theName == "DelayLoad_ZombieNote")
		{
			return ExtractDelayLoad_ZombieNoteResources(theManager);
		}
		if (theName == "DelayLoad_ZombieNote1")
		{
			return ExtractDelayLoad_ZombieNote1Resources(theManager);
		}
		if (theName == "DelayLoad_ZombieNote2")
		{
			return ExtractDelayLoad_ZombieNote2Resources(theManager);
		}
		if (theName == "DelayLoad_ZombieNote3")
		{
			return ExtractDelayLoad_ZombieNote3Resources(theManager);
		}
		if (theName == "DelayLoad_ZombieNote4")
		{
			return ExtractDelayLoad_ZombieNote4Resources(theManager);
		}
		if (theName == "DelayLoad_ZombieFinalNote")
		{
			return ExtractDelayLoad_ZombieFinalNoteResources(theManager);
		}
		if (theName == "DelayLoad_ZombieNoteHelp")
		{
			return ExtractDelayLoad_ZombieNoteHelpResources(theManager);
		}
		return false;
	}

	internal static void ExtractResources(ResourceManager theManager, AtlasResources theRes)
	{
		ExtractInitResources(theManager);
		ExtractInitRegisteredResources(theManager);
		ExtractLoaderBarResources(theManager);
		ExtractLoaderBarFontResources(theManager);
		ExtractLoadingFontsResources(theManager);
		ExtractLoadingImagesResources(theManager);
		ExtractLoadingSoundsResources(theManager);
		ExtractDelayLoad_PileResources(theManager);
		ExtractDelayLoad_GamePlayResources(theManager);
		ExtractDelayLoad_ZenGardenResources(theManager);
		ExtractDelayLoad_CachedResources(theManager);
		ExtractDelayLoad_MainMenuResources(theManager);
		ExtractDelayLoad_CreditsResources(theManager);
		ExtractDelayLoad_Leaderboard_BackgroundResources(theManager);
		ExtractDelayLoad_LeaderboardResources(theManager);
		ExtractDelayLoad_StarsResources(theManager);
		ExtractDelayLoad_GreenHouseGardenResources(theManager);
		ExtractDelayLoad_ZombiquariumResources(theManager);
		ExtractDelayLoad_MushroomGardenResources(theManager);
		ExtractDelayLoad_Background1Resources(theManager);
		ExtractDelayLoad_BackgroundUnsoddedResources(theManager);
		ExtractDelayLoad_Background2Resources(theManager);
		ExtractDelayLoad_Background3Resources(theManager);
		ExtractDelayLoad_Background4Resources(theManager);
		ExtractDelayLoad_Background5Resources(theManager);
		ExtractDelayLoad_Background6Resources(theManager);
		ExtractDelayLoad_AlmanacResources(theManager);
		ExtractDelayLoad_StoreResources(theManager);
		ExtractDelayLoad_ZombieNoteResources(theManager);
		ExtractDelayLoad_ZombieNote1Resources(theManager);
		ExtractDelayLoad_ZombieNote2Resources(theManager);
		ExtractDelayLoad_ZombieNote3Resources(theManager);
		ExtractDelayLoad_ZombieNote4Resources(theManager);
		ExtractDelayLoad_ZombieFinalNoteResources(theManager);
		ExtractDelayLoad_ZombieNoteHelpResources(theManager);
		theRes.ExtractResources();
	}

	public static bool ExtractInitResources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			IMAGE_POPCAP_LOGO = theManager.GetImageThrow("IMAGE_POPCAP_LOGO");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractInitRegisteredResources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			IMAGE_POPCAP_LOGO_REGISTERED = theManager.GetImageThrow("IMAGE_POPCAP_LOGO_REGISTERED");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractLoaderBarResources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			IMAGE_TITLESCREEN = theManager.GetImageThrow("IMAGE_TITLESCREEN");
			IMAGE_LOADING = theManager.GetImageThrow("IMAGE_LOADING");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractLoaderBarFontResources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			IMAGE_PVZ_LOGO = theManager.GetImageThrow("IMAGE_PVZ_LOGO");
			SOUND_BUTTONCLICK = theManager.GetSoundThrow("SOUND_BUTTONCLICK");
			SOUND_LOADINGBAR_FLOWER = theManager.GetSoundThrow("SOUND_LOADINGBAR_FLOWER");
			SOUND_LOADINGBAR_ZOMBIE = theManager.GetSoundThrow("SOUND_LOADINGBAR_ZOMBIE");
			FONT_BRIANNETOD16 = theManager.GetFontThrow("FONT_BRIANNETOD16");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractLoadingFontsResources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			FONT_HOUSEOFTERROR16 = theManager.GetFontThrow("FONT_HOUSEOFTERROR16");
			FONT_CONTINUUMBOLD14 = theManager.GetFontThrow("FONT_CONTINUUMBOLD14");
			FONT_CONTINUUMBOLD14OUTLINE = theManager.GetFontThrow("FONT_CONTINUUMBOLD14OUTLINE");
			FONT_DWARVENTODCRAFT12 = theManager.GetFontThrow("FONT_DWARVENTODCRAFT12");
			FONT_DWARVENTODCRAFT15 = theManager.GetFontThrow("FONT_DWARVENTODCRAFT15");
			FONT_DWARVENTODCRAFT18 = theManager.GetFontThrow("FONT_DWARVENTODCRAFT18");
			FONT_PICO129 = theManager.GetFontThrow("FONT_PICO129");
			FONT_BRIANNETOD12 = theManager.GetFontThrow("FONT_BRIANNETOD12");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractLoadingImagesResources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			IMAGE_CHARREDZOMBIES = theManager.GetImageThrow("IMAGE_CHARREDZOMBIES");
			IMAGE_ALMANACUI = theManager.GetImageThrow("IMAGE_ALMANACUI");
			IMAGE_SEEDATLAS = theManager.GetImageThrow("IMAGE_SEEDATLAS");
			IMAGE_DAVE = theManager.GetImageThrow("IMAGE_DAVE");
			IMAGE_DIALOG = theManager.GetImageThrow("IMAGE_DIALOG");
			IMAGE_CONVEYORBELT_BACKDROP = theManager.GetImageThrow("IMAGE_CONVEYORBELT_BACKDROP");
			IMAGE_CONVEYORBELT_BELT = theManager.GetImageThrow("IMAGE_CONVEYORBELT_BELT");
			IMAGE_SPEECHBUBBLE = theManager.GetImageThrow("IMAGE_SPEECHBUBBLE");
			IMAGE_LOC_EN = theManager.GetImageThrow("IMAGE_LOC_EN");
			IMAGE_ZOMBIE_NOTE_SMALL = theManager.GetImageThrow("IMAGE_ZOMBIE_NOTE_SMALL");
			IMAGE_REANIM_ZOMBIESWON = theManager.GetImageThrow("IMAGE_REANIM_ZOMBIESWON");
			IMAGE_SCARY_POT = theManager.GetImageThrow("IMAGE_SCARY_POT");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractLoadingSoundsResources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			SOUND_AWOOGA = theManager.GetSoundThrow("SOUND_AWOOGA");
			SOUND_BLEEP = theManager.GetSoundThrow("SOUND_BLEEP");
			SOUND_BUZZER = theManager.GetSoundThrow("SOUND_BUZZER");
			SOUND_CHOMP = theManager.GetSoundThrow("SOUND_CHOMP");
			SOUND_CHOMP2 = theManager.GetSoundThrow("SOUND_CHOMP2");
			SOUND_CHOMPSOFT = theManager.GetSoundThrow("SOUND_CHOMPSOFT");
			SOUND_FLOOP = theManager.GetSoundThrow("SOUND_FLOOP");
			SOUND_FROZEN = theManager.GetSoundThrow("SOUND_FROZEN");
			SOUND_GULP = theManager.GetSoundThrow("SOUND_GULP");
			SOUND_GROAN = theManager.GetSoundThrow("SOUND_GROAN");
			SOUND_GROAN2 = theManager.GetSoundThrow("SOUND_GROAN2");
			SOUND_GROAN3 = theManager.GetSoundThrow("SOUND_GROAN3");
			SOUND_GROAN4 = theManager.GetSoundThrow("SOUND_GROAN4");
			SOUND_GROAN5 = theManager.GetSoundThrow("SOUND_GROAN5");
			SOUND_GROAN6 = theManager.GetSoundThrow("SOUND_GROAN6");
			SOUND_LOSEMUSIC = theManager.GetSoundThrow("SOUND_LOSEMUSIC");
			SOUND_MINDCONTROLLED = theManager.GetSoundThrow("SOUND_MINDCONTROLLED");
			SOUND_PAUSE = theManager.GetSoundThrow("SOUND_PAUSE");
			SOUND_PLANT = theManager.GetSoundThrow("SOUND_PLANT");
			SOUND_PLANT2 = theManager.GetSoundThrow("SOUND_PLANT2");
			SOUND_POINTS = theManager.GetSoundThrow("SOUND_POINTS");
			SOUND_SEEDLIFT = theManager.GetSoundThrow("SOUND_SEEDLIFT");
			SOUND_SIREN = theManager.GetSoundThrow("SOUND_SIREN");
			SOUND_SLURP = theManager.GetSoundThrow("SOUND_SLURP");
			SOUND_SPLAT = theManager.GetSoundThrow("SOUND_SPLAT");
			SOUND_SPLAT2 = theManager.GetSoundThrow("SOUND_SPLAT2");
			SOUND_SPLAT3 = theManager.GetSoundThrow("SOUND_SPLAT3");
			SOUND_SUKHBIR4 = theManager.GetSoundThrow("SOUND_SUKHBIR4");
			SOUND_SUKHBIR5 = theManager.GetSoundThrow("SOUND_SUKHBIR5");
			SOUND_SUKHBIR6 = theManager.GetSoundThrow("SOUND_SUKHBIR6");
			SOUND_TAP = theManager.GetSoundThrow("SOUND_TAP");
			SOUND_TAP2 = theManager.GetSoundThrow("SOUND_TAP2");
			SOUND_THROW = theManager.GetSoundThrow("SOUND_THROW");
			SOUND_THROW2 = theManager.GetSoundThrow("SOUND_THROW2");
			SOUND_BLOVER = theManager.GetSoundThrow("SOUND_BLOVER");
			SOUND_WINMUSIC = theManager.GetSoundThrow("SOUND_WINMUSIC");
			SOUND_LAWNMOWER = theManager.GetSoundThrow("SOUND_LAWNMOWER");
			SOUND_BOING = theManager.GetSoundThrow("SOUND_BOING");
			SOUND_JACKINTHEBOX = theManager.GetSoundThrow("SOUND_JACKINTHEBOX");
			SOUND_DIAMOND = theManager.GetSoundThrow("SOUND_DIAMOND");
			SOUND_DOLPHIN_APPEARS = theManager.GetSoundThrow("SOUND_DOLPHIN_APPEARS");
			SOUND_DOLPHIN_BEFORE_JUMPING = theManager.GetSoundThrow("SOUND_DOLPHIN_BEFORE_JUMPING");
			SOUND_POTATO_MINE = theManager.GetSoundThrow("SOUND_POTATO_MINE");
			SOUND_ZAMBONI = theManager.GetSoundThrow("SOUND_ZAMBONI");
			SOUND_BALLOON_POP = theManager.GetSoundThrow("SOUND_BALLOON_POP");
			SOUND_THUNDER = theManager.GetSoundThrow("SOUND_THUNDER");
			SOUND_ZOMBIESPLASH = theManager.GetSoundThrow("SOUND_ZOMBIESPLASH");
			SOUND_BOWLING = theManager.GetSoundThrow("SOUND_BOWLING");
			SOUND_BOWLINGIMPACT = theManager.GetSoundThrow("SOUND_BOWLINGIMPACT");
			SOUND_BOWLINGIMPACT2 = theManager.GetSoundThrow("SOUND_BOWLINGIMPACT2");
			SOUND_GRAVEBUSTERCHOMP = theManager.GetSoundThrow("SOUND_GRAVEBUSTERCHOMP");
			SOUND_GRAVEBUTTON = theManager.GetSoundThrow("SOUND_GRAVEBUTTON");
			SOUND_LIMBS_POP = theManager.GetSoundThrow("SOUND_LIMBS_POP");
			SOUND_PLANTERN = theManager.GetSoundThrow("SOUND_PLANTERN");
			SOUND_POGO_ZOMBIE = theManager.GetSoundThrow("SOUND_POGO_ZOMBIE");
			SOUND_SNOW_PEA_SPARKLES = theManager.GetSoundThrow("SOUND_SNOW_PEA_SPARKLES");
			SOUND_PLANT_WATER = theManager.GetSoundThrow("SOUND_PLANT_WATER");
			SOUND_ZOMBIE_ENTERING_WATER = theManager.GetSoundThrow("SOUND_ZOMBIE_ENTERING_WATER");
			SOUND_ZOMBIE_FALLING_1 = theManager.GetSoundThrow("SOUND_ZOMBIE_FALLING_1");
			SOUND_ZOMBIE_FALLING_2 = theManager.GetSoundThrow("SOUND_ZOMBIE_FALLING_2");
			SOUND_PUFF = theManager.GetSoundThrow("SOUND_PUFF");
			SOUND_FUME = theManager.GetSoundThrow("SOUND_FUME");
			SOUND_HUGE_WAVE = theManager.GetSoundThrow("SOUND_HUGE_WAVE");
			SOUND_SLOT_MACHINE = theManager.GetSoundThrow("SOUND_SLOT_MACHINE");
			SOUND_COIN = theManager.GetSoundThrow("SOUND_COIN");
			SOUND_ROLL_IN = theManager.GetSoundThrow("SOUND_ROLL_IN");
			SOUND_DIGGER_ZOMBIE = theManager.GetSoundThrow("SOUND_DIGGER_ZOMBIE");
			SOUND_HATCHBACK_CLOSE = theManager.GetSoundThrow("SOUND_HATCHBACK_CLOSE");
			SOUND_HATCHBACK_OPEN = theManager.GetSoundThrow("SOUND_HATCHBACK_OPEN");
			SOUND_KERNELPULT = theManager.GetSoundThrow("SOUND_KERNELPULT");
			SOUND_KERNELPULT2 = theManager.GetSoundThrow("SOUND_KERNELPULT2");
			SOUND_ZOMBAQUARIUM_DIE = theManager.GetSoundThrow("SOUND_ZOMBAQUARIUM_DIE");
			SOUND_BUNGEE_SCREAM = theManager.GetSoundThrow("SOUND_BUNGEE_SCREAM");
			SOUND_BUNGEE_SCREAM2 = theManager.GetSoundThrow("SOUND_BUNGEE_SCREAM2");
			SOUND_BUNGEE_SCREAM3 = theManager.GetSoundThrow("SOUND_BUNGEE_SCREAM3");
			SOUND_BUTTER = theManager.GetSoundThrow("SOUND_BUTTER");
			SOUND_JACK_SURPRISE = theManager.GetSoundThrow("SOUND_JACK_SURPRISE");
			SOUND_JACK_SURPRISE2 = theManager.GetSoundThrow("SOUND_JACK_SURPRISE2");
			SOUND_NEWSPAPER_RARRGH = theManager.GetSoundThrow("SOUND_NEWSPAPER_RARRGH");
			SOUND_NEWSPAPER_RARRGH2 = theManager.GetSoundThrow("SOUND_NEWSPAPER_RARRGH2");
			SOUND_NEWSPAPER_RIP = theManager.GetSoundThrow("SOUND_NEWSPAPER_RIP");
			SOUND_SQUASH_HMM = theManager.GetSoundThrow("SOUND_SQUASH_HMM");
			SOUND_SQUASH_HMM2 = theManager.GetSoundThrow("SOUND_SQUASH_HMM2");
			SOUND_VASE_BREAKING = theManager.GetSoundThrow("SOUND_VASE_BREAKING");
			SOUND_POOL_CLEANER = theManager.GetSoundThrow("SOUND_POOL_CLEANER");
			SOUND_MAGNETSHROOM = theManager.GetSoundThrow("SOUND_MAGNETSHROOM");
			SOUND_LADDER_ZOMBIE = theManager.GetSoundThrow("SOUND_LADDER_ZOMBIE");
			SOUND_GARGANTUAR_THUMP = theManager.GetSoundThrow("SOUND_GARGANTUAR_THUMP");
			SOUND_BASKETBALL = theManager.GetSoundThrow("SOUND_BASKETBALL");
			SOUND_FIREPEA = theManager.GetSoundThrow("SOUND_FIREPEA");
			SOUND_IGNITE = theManager.GetSoundThrow("SOUND_IGNITE");
			SOUND_IGNITE2 = theManager.GetSoundThrow("SOUND_IGNITE2");
			SOUND_READYSETPLANT = theManager.GetSoundThrow("SOUND_READYSETPLANT");
			SOUND_DOOMSHROOM = theManager.GetSoundThrow("SOUND_DOOMSHROOM");
			SOUND_EXPLOSION = theManager.GetSoundThrow("SOUND_EXPLOSION");
			SOUND_FINALWAVE = theManager.GetSoundThrow("SOUND_FINALWAVE");
			SOUND_REVERSE_EXPLOSION = theManager.GetSoundThrow("SOUND_REVERSE_EXPLOSION");
			SOUND_RVTHROW = theManager.GetSoundThrow("SOUND_RVTHROW");
			SOUND_SHIELDHIT = theManager.GetSoundThrow("SOUND_SHIELDHIT");
			SOUND_SHIELDHIT2 = theManager.GetSoundThrow("SOUND_SHIELDHIT2");
			SOUND_BOSSEXPLOSION = theManager.GetSoundThrow("SOUND_BOSSEXPLOSION");
			SOUND_CHERRYBOMB = theManager.GetSoundThrow("SOUND_CHERRYBOMB");
			SOUND_BONK = theManager.GetSoundThrow("SOUND_BONK");
			SOUND_SWING = theManager.GetSoundThrow("SOUND_SWING");
			SOUND_RAIN = theManager.GetSoundThrow("SOUND_RAIN");
			SOUND_LIGHTFILL = theManager.GetSoundThrow("SOUND_LIGHTFILL");
			SOUND_PLASTICHIT = theManager.GetSoundThrow("SOUND_PLASTICHIT");
			SOUND_PLASTICHIT2 = theManager.GetSoundThrow("SOUND_PLASTICHIT2");
			SOUND_JALAPENO = theManager.GetSoundThrow("SOUND_JALAPENO");
			SOUND_BALLOONINFLATE = theManager.GetSoundThrow("SOUND_BALLOONINFLATE");
			SOUND_BIGCHOMP = theManager.GetSoundThrow("SOUND_BIGCHOMP");
			SOUND_MELONIMPACT = theManager.GetSoundThrow("SOUND_MELONIMPACT");
			SOUND_MELONIMPACT2 = theManager.GetSoundThrow("SOUND_MELONIMPACT2");
			SOUND_PLANTGROW = theManager.GetSoundThrow("SOUND_PLANTGROW");
			SOUND_SHOOP = theManager.GetSoundThrow("SOUND_SHOOP");
			SOUND_JUICY = theManager.GetSoundThrow("SOUND_JUICY");
			SOUND_COFFEE = theManager.GetSoundThrow("SOUND_COFFEE");
			SOUND_WAKEUP = theManager.GetSoundThrow("SOUND_WAKEUP");
			SOUND_LOWGROAN = theManager.GetSoundThrow("SOUND_LOWGROAN");
			SOUND_LOWGROAN2 = theManager.GetSoundThrow("SOUND_LOWGROAN2");
			SOUND_PRIZE = theManager.GetSoundThrow("SOUND_PRIZE");
			SOUND_YUCK = theManager.GetSoundThrow("SOUND_YUCK");
			SOUND_YUCK2 = theManager.GetSoundThrow("SOUND_YUCK2");
			SOUND_GRASSSTEP = theManager.GetSoundThrow("SOUND_GRASSSTEP");
			SOUND_SHOVEL = theManager.GetSoundThrow("SOUND_SHOVEL");
			SOUND_COBLAUNCH = theManager.GetSoundThrow("SOUND_COBLAUNCH");
			SOUND_WATERING = theManager.GetSoundThrow("SOUND_WATERING");
			SOUND_POLEVAULT = theManager.GetSoundThrow("SOUND_POLEVAULT");
			SOUND_GRAVESTONE_RUMBLE = theManager.GetSoundThrow("SOUND_GRAVESTONE_RUMBLE");
			SOUND_DIRT_RISE = theManager.GetSoundThrow("SOUND_DIRT_RISE");
			SOUND_FERTILIZER = theManager.GetSoundThrow("SOUND_FERTILIZER");
			SOUND_PORTAL = theManager.GetSoundThrow("SOUND_PORTAL");
			SOUND_SCREAM = theManager.GetSoundThrow("SOUND_SCREAM");
			SOUND_PAPER = theManager.GetSoundThrow("SOUND_PAPER");
			SOUND_MONEYFALLS = theManager.GetSoundThrow("SOUND_MONEYFALLS");
			SOUND_IMP = theManager.GetSoundThrow("SOUND_IMP");
			SOUND_IMP2 = theManager.GetSoundThrow("SOUND_IMP2");
			SOUND_HYDRAULIC_SHORT = theManager.GetSoundThrow("SOUND_HYDRAULIC_SHORT");
			SOUND_HYDRAULIC = theManager.GetSoundThrow("SOUND_HYDRAULIC");
			SOUND_GARGANTUDEATH = theManager.GetSoundThrow("SOUND_GARGANTUDEATH");
			SOUND_CERAMIC = theManager.GetSoundThrow("SOUND_CERAMIC");
			SOUND_BOSSBOULDERATTACK = theManager.GetSoundThrow("SOUND_BOSSBOULDERATTACK");
			SOUND_CHIME = theManager.GetSoundThrow("SOUND_CHIME");
			SOUND_CRAZYDAVESHORT1 = theManager.GetSoundThrow("SOUND_CRAZYDAVESHORT1");
			SOUND_CRAZYDAVESHORT2 = theManager.GetSoundThrow("SOUND_CRAZYDAVESHORT2");
			SOUND_CRAZYDAVESHORT3 = theManager.GetSoundThrow("SOUND_CRAZYDAVESHORT3");
			SOUND_CRAZYDAVELONG1 = theManager.GetSoundThrow("SOUND_CRAZYDAVELONG1");
			SOUND_CRAZYDAVELONG2 = theManager.GetSoundThrow("SOUND_CRAZYDAVELONG2");
			SOUND_CRAZYDAVELONG3 = theManager.GetSoundThrow("SOUND_CRAZYDAVELONG3");
			SOUND_CRAZYDAVEEXTRALONG1 = theManager.GetSoundThrow("SOUND_CRAZYDAVEEXTRALONG1");
			SOUND_CRAZYDAVEEXTRALONG2 = theManager.GetSoundThrow("SOUND_CRAZYDAVEEXTRALONG2");
			SOUND_CRAZYDAVEEXTRALONG3 = theManager.GetSoundThrow("SOUND_CRAZYDAVEEXTRALONG3");
			SOUND_CRAZYDAVECRAZY = theManager.GetSoundThrow("SOUND_CRAZYDAVECRAZY");
			SOUND_DANCER = theManager.GetSoundThrow("SOUND_DANCER");
			SOUND_FINALFANFARE = theManager.GetSoundThrow("SOUND_FINALFANFARE");
			SOUND_CRAZYDAVESCREAM = theManager.GetSoundThrow("SOUND_CRAZYDAVESCREAM");
			SOUND_CRAZYDAVESCREAM2 = theManager.GetSoundThrow("SOUND_CRAZYDAVESCREAM2");
			SOUND_ACHIEVEMENT = theManager.GetSoundThrow("SOUND_ACHIEVEMENT");
			SOUND_BUGSPRAY = theManager.GetSoundThrow("SOUND_BUGSPRAY");
			SOUND_FERTILISER = theManager.GetSoundThrow("SOUND_FERTILISER");
			SOUND_PHONOGRAPH = theManager.GetSoundThrow("SOUND_PHONOGRAPH");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractDelayLoad_PileResources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			IMAGE_PILE = theManager.GetImageThrow("IMAGE_PILE");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractDelayLoad_GamePlayResources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			IMAGE_PLANTSZOMBIES = theManager.GetImageThrow("IMAGE_PLANTSZOMBIES");
			IMAGE_PARTICLES = theManager.GetImageThrow("IMAGE_PARTICLES");
			IMAGE_SLOTMACHINE_OVERLAY = theManager.GetImageThrow("IMAGE_SLOTMACHINE_OVERLAY");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractDelayLoad_ZenGardenResources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			IMAGE_ZENGARDEN = theManager.GetImageThrow("IMAGE_ZENGARDEN");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractDelayLoad_CachedResources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			IMAGE_CACHED = theManager.GetImageThrow("IMAGE_CACHED");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractDelayLoad_MainMenuResources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			IMAGE_SELECTORSCREEN_ACHIEVEMENTS_TOP_BACKGROUND = theManager.GetImageThrow("IMAGE_SELECTORSCREEN_ACHIEVEMENTS_TOP_BACKGROUND");
			IMAGE_SELECTORSCREEN_MAIN_BACKGROUND = theManager.GetImageThrow("IMAGE_SELECTORSCREEN_MAIN_BACKGROUND");
			IMAGE_SELECTORSCREEN_ACHIEVEMENTS_HOLE = theManager.GetImageThrow("IMAGE_SELECTORSCREEN_ACHIEVEMENTS_HOLE");
			IMAGE_SELECTORSCREEN_ACHIEVEMENTS_HOLE_CHINA = theManager.GetImageThrow("IMAGE_SELECTORSCREEN_ACHIEVEMENTS_HOLE_CHINA");
			IMAGE_SELECTORSCREEN_QUICKPLAY_BACKGROUND = theManager.GetImageThrow("IMAGE_SELECTORSCREEN_QUICKPLAY_BACKGROUND");
			IMAGE_GOODIES = theManager.GetImageThrow("IMAGE_GOODIES");
			IMAGE_QUICKPLAY = theManager.GetImageThrow("IMAGE_QUICKPLAY");
			IMAGE_ACHIEVEMENT_GNOME = theManager.GetImageThrow("IMAGE_ACHIEVEMENT_GNOME");
			IMAGE_MINIGAMES = theManager.GetImageThrow("IMAGE_MINIGAMES");
			IMAGE_SELECTORSCREEN_MOREGAMES_BACKGROUND = theManager.GetImageThrow("IMAGE_SELECTORSCREEN_MOREGAMES_BACKGROUND");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractDelayLoad_CreditsResources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			IMAGE_CREDITS_ZOMBIENOTE = theManager.GetImageThrow("IMAGE_CREDITS_ZOMBIENOTE");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractDelayLoad_Leaderboard_BackgroundResources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			IMAGE_LEADERBOARDSCREEN_BACKGROUND = theManager.GetImageThrow("IMAGE_LEADERBOARDSCREEN_BACKGROUND");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractDelayLoad_LeaderboardResources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			IMAGE_BLACKHOLE = theManager.GetImageThrow("IMAGE_BLACKHOLE");
			IMAGE_EDGE_OF_SPACE = theManager.GetImageThrow("IMAGE_EDGE_OF_SPACE");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractDelayLoad_StarsResources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			IMAGE_STARS_1 = theManager.GetImageThrow("IMAGE_STARS_1");
			IMAGE_STARS_2 = theManager.GetImageThrow("IMAGE_STARS_2");
			IMAGE_STARS_3 = theManager.GetImageThrow("IMAGE_STARS_3");
			IMAGE_STARS_4 = theManager.GetImageThrow("IMAGE_STARS_4");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractDelayLoad_GreenHouseGardenResources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			IMAGE_BACKGROUND_GREENHOUSE = theManager.GetImageThrow("IMAGE_BACKGROUND_GREENHOUSE");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractDelayLoad_ZombiquariumResources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			IMAGE_AQUARIUM1 = theManager.GetImageThrow("IMAGE_AQUARIUM1");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractDelayLoad_MushroomGardenResources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			IMAGE_BACKGROUND_MUSHROOMGARDEN = theManager.GetImageThrow("IMAGE_BACKGROUND_MUSHROOMGARDEN");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractDelayLoad_Background1Resources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			IMAGE_BACKGROUND1 = theManager.GetImageThrow("IMAGE_BACKGROUND1");
			IMAGE_BACKGROUND1_GAMEOVER_INTERIOR_OVERLAY = theManager.GetImageThrow("IMAGE_BACKGROUND1_GAMEOVER_INTERIOR_OVERLAY");
			IMAGE_BACKGROUND1_GAMEOVER_MASK = theManager.GetImageThrow("IMAGE_BACKGROUND1_GAMEOVER_MASK");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractDelayLoad_BackgroundUnsoddedResources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			IMAGE_BACKGROUND1UNSODDED = theManager.GetImageThrow("IMAGE_BACKGROUND1UNSODDED");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractDelayLoad_Background2Resources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			IMAGE_BACKGROUND2 = theManager.GetImageThrow("IMAGE_BACKGROUND2");
			IMAGE_BACKGROUND2_GAMEOVER_INTERIOR_OVERLAY = theManager.GetImageThrow("IMAGE_BACKGROUND2_GAMEOVER_INTERIOR_OVERLAY");
			IMAGE_BACKGROUND2_GAMEOVER_MASK = theManager.GetImageThrow("IMAGE_BACKGROUND2_GAMEOVER_MASK");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractDelayLoad_Background3Resources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			IMAGE_BACKGROUND3 = theManager.GetImageThrow("IMAGE_BACKGROUND3");
			IMAGE_BACKGROUND3_GAMEOVER_INTERIOR_OVERLAY = theManager.GetImageThrow("IMAGE_BACKGROUND3_GAMEOVER_INTERIOR_OVERLAY");
			IMAGE_BACKGROUND3_GAMEOVER_MASK = theManager.GetImageThrow("IMAGE_BACKGROUND3_GAMEOVER_MASK");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractDelayLoad_Background4Resources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			IMAGE_BACKGROUND4 = theManager.GetImageThrow("IMAGE_BACKGROUND4");
			IMAGE_BACKGROUND4_GAMEOVER_INTERIOR_OVERLAY = theManager.GetImageThrow("IMAGE_BACKGROUND4_GAMEOVER_INTERIOR_OVERLAY");
			IMAGE_BACKGROUND4_GAMEOVER_MASK = theManager.GetImageThrow("IMAGE_BACKGROUND4_GAMEOVER_MASK");
			IMAGE_FOG = theManager.GetImageThrow("IMAGE_FOG");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractDelayLoad_Background5Resources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			IMAGE_BACKGROUND5 = theManager.GetImageThrow("IMAGE_BACKGROUND5");
			IMAGE_BACKGROUND5_GAMEOVER_MASK = theManager.GetImageThrow("IMAGE_BACKGROUND5_GAMEOVER_MASK");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractDelayLoad_Background6Resources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			IMAGE_BACKGROUND6BOSS = theManager.GetImageThrow("IMAGE_BACKGROUND6BOSS");
			IMAGE_BACKGROUND6_GAMEOVER_MASK = theManager.GetImageThrow("IMAGE_BACKGROUND6_GAMEOVER_MASK");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractDelayLoad_AlmanacResources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		return true;
	}

	public static bool ExtractDelayLoad_StoreResources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			IMAGE_STORE_BACKGROUND = theManager.GetImageThrow("IMAGE_STORE_BACKGROUND");
			IMAGE_STORE_BACKGROUNDNIGHT = theManager.GetImageThrow("IMAGE_STORE_BACKGROUNDNIGHT");
			IMAGE_STORE_CAR = theManager.GetImageThrow("IMAGE_STORE_CAR");
			IMAGE_STORE_CAR_NIGHT = theManager.GetImageThrow("IMAGE_STORE_CAR_NIGHT");
			IMAGE_STORE_CARCLOSED = theManager.GetImageThrow("IMAGE_STORE_CARCLOSED");
			IMAGE_STORE_CARCLOSED_NIGHT = theManager.GetImageThrow("IMAGE_STORE_CARCLOSED_NIGHT");
			IMAGE_STORE_HATCHBACKOPEN = theManager.GetImageThrow("IMAGE_STORE_HATCHBACKOPEN");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractDelayLoad_ZombieNoteResources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			IMAGE_ZOMBIE_NOTE = theManager.GetImageThrow("IMAGE_ZOMBIE_NOTE");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractDelayLoad_ZombieNote1Resources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			IMAGE_ZOMBIE_NOTE1 = theManager.GetImageThrow("IMAGE_ZOMBIE_NOTE1");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractDelayLoad_ZombieNote2Resources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			IMAGE_ZOMBIE_NOTE2 = theManager.GetImageThrow("IMAGE_ZOMBIE_NOTE2");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractDelayLoad_ZombieNote3Resources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			IMAGE_ZOMBIE_NOTE3 = theManager.GetImageThrow("IMAGE_ZOMBIE_NOTE3");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractDelayLoad_ZombieNote4Resources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			IMAGE_ZOMBIE_NOTE4 = theManager.GetImageThrow("IMAGE_ZOMBIE_NOTE4");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractDelayLoad_ZombieFinalNoteResources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			IMAGE_ZOMBIE_FINAL_NOTE = theManager.GetImageThrow("IMAGE_ZOMBIE_FINAL_NOTE");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool ExtractDelayLoad_ZombieNoteHelpResources(ResourceManager theManager)
	{
		gNeedRecalcVariableToIdMap = true;
		try
		{
			IMAGE_ZOMBIE_NOTE_HELP = theManager.GetImageThrow("IMAGE_ZOMBIE_NOTE_HELP");
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static Image GetImageById(int theId)
	{
		return (Image)gResources[theId];
	}

	public static Font GetFontById(int theId)
	{
		return (Font)gResources[theId];
	}

	public static int GetSoundById(int theId)
	{
		return (int)gResources[theId];
	}

	public static Image GetImageRefById(int theId)
	{
		return (Image)gResources[theId];
	}

	public static Font GetFontRefById(int theId)
	{
		return (Font)gResources[theId];
	}

	public static int GetSoundRefById(int theId)
	{
		return (int)gResources[theId];
	}

	public static ResourceId GetIdByImage(Image theImage)
	{
		return GetIdByVariable(theImage);
	}

	public static ResourceId GetIdByFont(Font theFont)
	{
		return GetIdByVariable(theFont);
	}

	public static ResourceId GetIdBySound(int theSound)
	{
		return GetIdByVariable((IntPtr)theSound);
	}

	public static ResourceId GetIdByStringId(string theStringId)
	{
		return ResourceId.RESOURCE_ID_MAX;
	}

	public static ResourceId GetIdByVariable(object theVariable)
	{
		return ResourceId.RESOURCE_ID_MAX;
	}

	public static void LinkUpResArray()
	{
		gResources = new object[250]
		{
			IMAGE_POPCAP_LOGO,
			IMAGE_POPCAP_LOGO_REGISTERED,
			IMAGE_TITLESCREEN,
			IMAGE_LOADING,
			IMAGE_PVZ_LOGO,
			FONT_BRIANNETOD16,
			SOUND_BUTTONCLICK,
			SOUND_LOADINGBAR_FLOWER,
			SOUND_LOADINGBAR_ZOMBIE,
			FONT_HOUSEOFTERROR16,
			FONT_CONTINUUMBOLD14,
			FONT_CONTINUUMBOLD14OUTLINE,
			FONT_DWARVENTODCRAFT12,
			FONT_DWARVENTODCRAFT15,
			FONT_DWARVENTODCRAFT18,
			FONT_PICO129,
			FONT_BRIANNETOD12,
			IMAGE_CHARREDZOMBIES,
			IMAGE_ALMANACUI,
			IMAGE_SEEDATLAS,
			IMAGE_DAVE,
			IMAGE_DIALOG,
			IMAGE_CONVEYORBELT_BACKDROP,
			IMAGE_CONVEYORBELT_BELT,
			IMAGE_SPEECHBUBBLE,
			IMAGE_LOC_EN,
			IMAGE_ZOMBIE_NOTE_SMALL,
			IMAGE_REANIM_ZOMBIESWON,
			IMAGE_SCARY_POT,
			SOUND_AWOOGA,
			SOUND_BLEEP,
			SOUND_BUZZER,
			SOUND_CHOMP,
			SOUND_CHOMP2,
			SOUND_CHOMPSOFT,
			SOUND_FLOOP,
			SOUND_FROZEN,
			SOUND_GULP,
			SOUND_GROAN,
			SOUND_GROAN2,
			SOUND_GROAN3,
			SOUND_GROAN4,
			SOUND_GROAN5,
			SOUND_GROAN6,
			SOUND_LOSEMUSIC,
			SOUND_MINDCONTROLLED,
			SOUND_PAUSE,
			SOUND_PLANT,
			SOUND_PLANT2,
			SOUND_POINTS,
			SOUND_SEEDLIFT,
			SOUND_SIREN,
			SOUND_SLURP,
			SOUND_SPLAT,
			SOUND_SPLAT2,
			SOUND_SPLAT3,
			SOUND_SUKHBIR4,
			SOUND_SUKHBIR5,
			SOUND_SUKHBIR6,
			SOUND_TAP,
			SOUND_TAP2,
			SOUND_THROW,
			SOUND_THROW2,
			SOUND_BLOVER,
			SOUND_WINMUSIC,
			SOUND_LAWNMOWER,
			SOUND_BOING,
			SOUND_JACKINTHEBOX,
			SOUND_DIAMOND,
			SOUND_DOLPHIN_APPEARS,
			SOUND_DOLPHIN_BEFORE_JUMPING,
			SOUND_POTATO_MINE,
			SOUND_ZAMBONI,
			SOUND_BALLOON_POP,
			SOUND_THUNDER,
			SOUND_ZOMBIESPLASH,
			SOUND_BOWLING,
			SOUND_BOWLINGIMPACT,
			SOUND_BOWLINGIMPACT2,
			SOUND_GRAVEBUSTERCHOMP,
			SOUND_GRAVEBUTTON,
			SOUND_LIMBS_POP,
			SOUND_PLANTERN,
			SOUND_POGO_ZOMBIE,
			SOUND_SNOW_PEA_SPARKLES,
			SOUND_PLANT_WATER,
			SOUND_ZOMBIE_ENTERING_WATER,
			SOUND_ZOMBIE_FALLING_1,
			SOUND_ZOMBIE_FALLING_2,
			SOUND_PUFF,
			SOUND_FUME,
			SOUND_HUGE_WAVE,
			SOUND_SLOT_MACHINE,
			SOUND_COIN,
			SOUND_ROLL_IN,
			SOUND_DIGGER_ZOMBIE,
			SOUND_HATCHBACK_CLOSE,
			SOUND_HATCHBACK_OPEN,
			SOUND_KERNELPULT,
			SOUND_KERNELPULT2,
			SOUND_ZOMBAQUARIUM_DIE,
			SOUND_BUNGEE_SCREAM,
			SOUND_BUNGEE_SCREAM2,
			SOUND_BUNGEE_SCREAM3,
			SOUND_BUTTER,
			SOUND_JACK_SURPRISE,
			SOUND_JACK_SURPRISE2,
			SOUND_NEWSPAPER_RARRGH,
			SOUND_NEWSPAPER_RARRGH2,
			SOUND_NEWSPAPER_RIP,
			SOUND_SQUASH_HMM,
			SOUND_SQUASH_HMM2,
			SOUND_VASE_BREAKING,
			SOUND_POOL_CLEANER,
			SOUND_MAGNETSHROOM,
			SOUND_LADDER_ZOMBIE,
			SOUND_GARGANTUAR_THUMP,
			SOUND_BASKETBALL,
			SOUND_FIREPEA,
			SOUND_IGNITE,
			SOUND_IGNITE2,
			SOUND_READYSETPLANT,
			SOUND_DOOMSHROOM,
			SOUND_EXPLOSION,
			SOUND_FINALWAVE,
			SOUND_REVERSE_EXPLOSION,
			SOUND_RVTHROW,
			SOUND_SHIELDHIT,
			SOUND_SHIELDHIT2,
			SOUND_BOSSEXPLOSION,
			SOUND_CHERRYBOMB,
			SOUND_BONK,
			SOUND_SWING,
			SOUND_RAIN,
			SOUND_LIGHTFILL,
			SOUND_PLASTICHIT,
			SOUND_PLASTICHIT2,
			SOUND_JALAPENO,
			SOUND_BALLOONINFLATE,
			SOUND_BIGCHOMP,
			SOUND_MELONIMPACT,
			SOUND_MELONIMPACT2,
			SOUND_PLANTGROW,
			SOUND_SHOOP,
			SOUND_JUICY,
			SOUND_COFFEE,
			SOUND_WAKEUP,
			SOUND_LOWGROAN,
			SOUND_LOWGROAN2,
			SOUND_PRIZE,
			SOUND_YUCK,
			SOUND_YUCK2,
			SOUND_GRASSSTEP,
			SOUND_SHOVEL,
			SOUND_COBLAUNCH,
			SOUND_WATERING,
			SOUND_POLEVAULT,
			SOUND_GRAVESTONE_RUMBLE,
			SOUND_DIRT_RISE,
			SOUND_FERTILIZER,
			SOUND_PORTAL,
			SOUND_SCREAM,
			SOUND_PAPER,
			SOUND_MONEYFALLS,
			SOUND_IMP,
			SOUND_IMP2,
			SOUND_HYDRAULIC_SHORT,
			SOUND_HYDRAULIC,
			SOUND_GARGANTUDEATH,
			SOUND_CERAMIC,
			SOUND_BOSSBOULDERATTACK,
			SOUND_CHIME,
			SOUND_CRAZYDAVESHORT1,
			SOUND_CRAZYDAVESHORT2,
			SOUND_CRAZYDAVESHORT3,
			SOUND_CRAZYDAVELONG1,
			SOUND_CRAZYDAVELONG2,
			SOUND_CRAZYDAVELONG3,
			SOUND_CRAZYDAVEEXTRALONG1,
			SOUND_CRAZYDAVEEXTRALONG2,
			SOUND_CRAZYDAVEEXTRALONG3,
			SOUND_CRAZYDAVECRAZY,
			SOUND_DANCER,
			SOUND_FINALFANFARE,
			SOUND_CRAZYDAVESCREAM,
			SOUND_CRAZYDAVESCREAM2,
			SOUND_ACHIEVEMENT,
			SOUND_BUGSPRAY,
			SOUND_FERTILISER,
			SOUND_PHONOGRAPH,
			IMAGE_PILE,
			IMAGE_PLANTSZOMBIES,
			IMAGE_PARTICLES,
			IMAGE_SLOTMACHINE_OVERLAY,
			IMAGE_ZENGARDEN,
			IMAGE_CACHED,
			IMAGE_SELECTORSCREEN_ACHIEVEMENTS_TOP_BACKGROUND,
			IMAGE_SELECTORSCREEN_MAIN_BACKGROUND,
			IMAGE_SELECTORSCREEN_ACHIEVEMENTS_HOLE,
			IMAGE_SELECTORSCREEN_ACHIEVEMENTS_HOLE_CHINA,
			IMAGE_SELECTORSCREEN_QUICKPLAY_BACKGROUND,
			IMAGE_GOODIES,
			IMAGE_QUICKPLAY,
			IMAGE_ACHIEVEMENT_GNOME,
			IMAGE_MINIGAMES,
			IMAGE_SELECTORSCREEN_MOREGAMES_BACKGROUND,
			IMAGE_CREDITS_ZOMBIENOTE,
			IMAGE_LEADERBOARDSCREEN_BACKGROUND,
			IMAGE_BLACKHOLE,
			IMAGE_EDGE_OF_SPACE,
			IMAGE_STARS_1,
			IMAGE_STARS_2,
			IMAGE_STARS_3,
			IMAGE_STARS_4,
			IMAGE_BACKGROUND_GREENHOUSE,
			IMAGE_AQUARIUM1,
			IMAGE_BACKGROUND_MUSHROOMGARDEN,
			IMAGE_BACKGROUND1,
			IMAGE_BACKGROUND1_GAMEOVER_INTERIOR_OVERLAY,
			IMAGE_BACKGROUND1_GAMEOVER_MASK,
			IMAGE_BACKGROUND1UNSODDED,
			IMAGE_BACKGROUND2,
			IMAGE_BACKGROUND2_GAMEOVER_INTERIOR_OVERLAY,
			IMAGE_BACKGROUND2_GAMEOVER_MASK,
			IMAGE_BACKGROUND3,
			IMAGE_BACKGROUND3_GAMEOVER_INTERIOR_OVERLAY,
			IMAGE_BACKGROUND3_GAMEOVER_MASK,
			IMAGE_BACKGROUND4,
			IMAGE_BACKGROUND4_GAMEOVER_INTERIOR_OVERLAY,
			IMAGE_BACKGROUND4_GAMEOVER_MASK,
			IMAGE_FOG,
			IMAGE_BACKGROUND5,
			IMAGE_BACKGROUND5_GAMEOVER_MASK,
			IMAGE_BACKGROUND6BOSS,
			IMAGE_BACKGROUND6_GAMEOVER_MASK,
			IMAGE_STORE_BACKGROUND,
			IMAGE_STORE_BACKGROUNDNIGHT,
			IMAGE_STORE_CAR,
			IMAGE_STORE_CAR_NIGHT,
			IMAGE_STORE_CARCLOSED,
			IMAGE_STORE_CARCLOSED_NIGHT,
			IMAGE_STORE_HATCHBACKOPEN,
			IMAGE_ZOMBIE_NOTE,
			IMAGE_ZOMBIE_NOTE1,
			IMAGE_ZOMBIE_NOTE2,
			IMAGE_ZOMBIE_NOTE3,
			IMAGE_ZOMBIE_NOTE4,
			IMAGE_ZOMBIE_FINAL_NOTE,
			IMAGE_ZOMBIE_NOTE_HELP,
			null
		};
	}

	public static string GetStringIdById(int theId)
	{
		switch (theId)
		{
		case 0:
			return "IMAGE_POPCAP_LOGO";
		case 1:
			return "IMAGE_POPCAP_LOGO_REGISTERED";
		case 2:
			return "IMAGE_TITLESCREEN";
		case 3:
			return "IMAGE_LOADING";
		case 4:
			return "IMAGE_PVZ_LOGO";
		case 8:
			return "FONT_BRIANNETOD16";
		case 5:
			return "SOUND_BUTTONCLICK";
		case 6:
			return "SOUND_LOADINGBAR_FLOWER";
		case 7:
			return "SOUND_LOADINGBAR_ZOMBIE";
		case 9:
			return "FONT_HOUSEOFTERROR16";
		case 10:
			return "FONT_CONTINUUMBOLD14";
		case 11:
			return "FONT_CONTINUUMBOLD14OUTLINE";
		case 12:
			return "FONT_DWARVENTODCRAFT12";
		case 13:
			return "FONT_DWARVENTODCRAFT15";
		case 14:
			return "FONT_DWARVENTODCRAFT18";
		case 15:
			return "FONT_PICO129";
		case 16:
			return "FONT_BRIANNETOD12";
		case 17:
			return "IMAGE_CHARREDZOMBIES";
		case 18:
			return "IMAGE_ALMANACUI";
		case 19:
			return "IMAGE_SEEDATLAS";
		case 20:
			return "IMAGE_DAVE";
		case 21:
			return "IMAGE_DIALOG";
		case 22:
			return "IMAGE_CONVEYORBELT_BACKDROP";
		case 23:
			return "IMAGE_CONVEYORBELT_BELT";
		case 24:
			return "IMAGE_SPEECHBUBBLE";
		case 25:
			return "IMAGE_LOC_EN";
		case 26:
			return "IMAGE_ZOMBIE_NOTE_SMALL";
		case 27:
			return "IMAGE_REANIM_ZOMBIESWON";
		case 28:
			return "IMAGE_SCARY_POT";
		case 29:
			return "SOUND_AWOOGA";
		case 30:
			return "SOUND_BLEEP";
		case 31:
			return "SOUND_BUZZER";
		case 32:
			return "SOUND_CHOMP";
		case 33:
			return "SOUND_CHOMP2";
		case 34:
			return "SOUND_CHOMPSOFT";
		case 35:
			return "SOUND_FLOOP";
		case 36:
			return "SOUND_FROZEN";
		case 37:
			return "SOUND_GULP";
		case 38:
			return "SOUND_GROAN";
		case 39:
			return "SOUND_GROAN2";
		case 40:
			return "SOUND_GROAN3";
		case 41:
			return "SOUND_GROAN4";
		case 42:
			return "SOUND_GROAN5";
		case 43:
			return "SOUND_GROAN6";
		case 44:
			return "SOUND_LOSEMUSIC";
		case 45:
			return "SOUND_MINDCONTROLLED";
		case 46:
			return "SOUND_PAUSE";
		case 47:
			return "SOUND_PLANT";
		case 48:
			return "SOUND_PLANT2";
		case 49:
			return "SOUND_POINTS";
		case 50:
			return "SOUND_SEEDLIFT";
		case 51:
			return "SOUND_SIREN";
		case 52:
			return "SOUND_SLURP";
		case 53:
			return "SOUND_SPLAT";
		case 54:
			return "SOUND_SPLAT2";
		case 55:
			return "SOUND_SPLAT3";
		case 56:
			return "SOUND_SUKHBIR4";
		case 57:
			return "SOUND_SUKHBIR5";
		case 58:
			return "SOUND_SUKHBIR6";
		case 59:
			return "SOUND_TAP";
		case 60:
			return "SOUND_TAP2";
		case 61:
			return "SOUND_THROW";
		case 62:
			return "SOUND_THROW2";
		case 63:
			return "SOUND_BLOVER";
		case 64:
			return "SOUND_WINMUSIC";
		case 65:
			return "SOUND_LAWNMOWER";
		case 66:
			return "SOUND_BOING";
		case 67:
			return "SOUND_JACKINTHEBOX";
		case 68:
			return "SOUND_DIAMOND";
		case 69:
			return "SOUND_DOLPHIN_APPEARS";
		case 70:
			return "SOUND_DOLPHIN_BEFORE_JUMPING";
		case 71:
			return "SOUND_POTATO_MINE";
		case 72:
			return "SOUND_ZAMBONI";
		case 73:
			return "SOUND_BALLOON_POP";
		case 74:
			return "SOUND_THUNDER";
		case 75:
			return "SOUND_ZOMBIESPLASH";
		case 76:
			return "SOUND_BOWLING";
		case 77:
			return "SOUND_BOWLINGIMPACT";
		case 78:
			return "SOUND_BOWLINGIMPACT2";
		case 79:
			return "SOUND_GRAVEBUSTERCHOMP";
		case 80:
			return "SOUND_GRAVEBUTTON";
		case 81:
			return "SOUND_LIMBS_POP";
		case 82:
			return "SOUND_PLANTERN";
		case 83:
			return "SOUND_POGO_ZOMBIE";
		case 84:
			return "SOUND_SNOW_PEA_SPARKLES";
		case 85:
			return "SOUND_PLANT_WATER";
		case 86:
			return "SOUND_ZOMBIE_ENTERING_WATER";
		case 87:
			return "SOUND_ZOMBIE_FALLING_1";
		case 88:
			return "SOUND_ZOMBIE_FALLING_2";
		case 89:
			return "SOUND_PUFF";
		case 90:
			return "SOUND_FUME";
		case 91:
			return "SOUND_HUGE_WAVE";
		case 92:
			return "SOUND_SLOT_MACHINE";
		case 93:
			return "SOUND_COIN";
		case 94:
			return "SOUND_ROLL_IN";
		case 95:
			return "SOUND_DIGGER_ZOMBIE";
		case 96:
			return "SOUND_HATCHBACK_CLOSE";
		case 97:
			return "SOUND_HATCHBACK_OPEN";
		case 98:
			return "SOUND_KERNELPULT";
		case 99:
			return "SOUND_KERNELPULT2";
		case 100:
			return "SOUND_ZOMBAQUARIUM_DIE";
		case 101:
			return "SOUND_BUNGEE_SCREAM";
		case 102:
			return "SOUND_BUNGEE_SCREAM2";
		case 103:
			return "SOUND_BUNGEE_SCREAM3";
		case 104:
			return "SOUND_BUTTER";
		case 105:
			return "SOUND_JACK_SURPRISE";
		case 106:
			return "SOUND_JACK_SURPRISE2";
		case 107:
			return "SOUND_NEWSPAPER_RARRGH";
		case 108:
			return "SOUND_NEWSPAPER_RARRGH2";
		case 109:
			return "SOUND_NEWSPAPER_RIP";
		case 110:
			return "SOUND_SQUASH_HMM";
		case 111:
			return "SOUND_SQUASH_HMM2";
		case 112:
			return "SOUND_VASE_BREAKING";
		case 113:
			return "SOUND_POOL_CLEANER";
		case 114:
			return "SOUND_MAGNETSHROOM";
		case 115:
			return "SOUND_LADDER_ZOMBIE";
		case 116:
			return "SOUND_GARGANTUAR_THUMP";
		case 117:
			return "SOUND_BASKETBALL";
		case 118:
			return "SOUND_FIREPEA";
		case 119:
			return "SOUND_IGNITE";
		case 120:
			return "SOUND_IGNITE2";
		case 121:
			return "SOUND_READYSETPLANT";
		case 122:
			return "SOUND_DOOMSHROOM";
		case 123:
			return "SOUND_EXPLOSION";
		case 124:
			return "SOUND_FINALWAVE";
		case 125:
			return "SOUND_REVERSE_EXPLOSION";
		case 126:
			return "SOUND_RVTHROW";
		case 127:
			return "SOUND_SHIELDHIT";
		case 128:
			return "SOUND_SHIELDHIT2";
		case 129:
			return "SOUND_BOSSEXPLOSION";
		case 130:
			return "SOUND_CHERRYBOMB";
		case 131:
			return "SOUND_BONK";
		case 132:
			return "SOUND_SWING";
		case 133:
			return "SOUND_RAIN";
		case 134:
			return "SOUND_LIGHTFILL";
		case 135:
			return "SOUND_PLASTICHIT";
		case 136:
			return "SOUND_PLASTICHIT2";
		case 137:
			return "SOUND_JALAPENO";
		case 138:
			return "SOUND_BALLOONINFLATE";
		case 139:
			return "SOUND_BIGCHOMP";
		case 140:
			return "SOUND_MELONIMPACT";
		case 141:
			return "SOUND_MELONIMPACT2";
		case 142:
			return "SOUND_PLANTGROW";
		case 143:
			return "SOUND_SHOOP";
		case 144:
			return "SOUND_JUICY";
		case 145:
			return "SOUND_COFFEE";
		case 146:
			return "SOUND_WAKEUP";
		case 147:
			return "SOUND_LOWGROAN";
		case 148:
			return "SOUND_LOWGROAN2";
		case 149:
			return "SOUND_PRIZE";
		case 150:
			return "SOUND_YUCK";
		case 151:
			return "SOUND_YUCK2";
		case 152:
			return "SOUND_GRASSSTEP";
		case 153:
			return "SOUND_SHOVEL";
		case 154:
			return "SOUND_COBLAUNCH";
		case 155:
			return "SOUND_WATERING";
		case 156:
			return "SOUND_POLEVAULT";
		case 157:
			return "SOUND_GRAVESTONE_RUMBLE";
		case 158:
			return "SOUND_DIRT_RISE";
		case 159:
			return "SOUND_FERTILIZER";
		case 160:
			return "SOUND_PORTAL";
		case 161:
			return "SOUND_SCREAM";
		case 162:
			return "SOUND_PAPER";
		case 163:
			return "SOUND_MONEYFALLS";
		case 164:
			return "SOUND_IMP";
		case 165:
			return "SOUND_IMP2";
		case 166:
			return "SOUND_HYDRAULIC_SHORT";
		case 167:
			return "SOUND_HYDRAULIC";
		case 168:
			return "SOUND_GARGANTUDEATH";
		case 169:
			return "SOUND_CERAMIC";
		case 170:
			return "SOUND_BOSSBOULDERATTACK";
		case 171:
			return "SOUND_CHIME";
		case 172:
			return "SOUND_CRAZYDAVESHORT1";
		case 173:
			return "SOUND_CRAZYDAVESHORT2";
		case 174:
			return "SOUND_CRAZYDAVESHORT3";
		case 175:
			return "SOUND_CRAZYDAVELONG1";
		case 176:
			return "SOUND_CRAZYDAVELONG2";
		case 177:
			return "SOUND_CRAZYDAVELONG3";
		case 178:
			return "SOUND_CRAZYDAVEEXTRALONG1";
		case 179:
			return "SOUND_CRAZYDAVEEXTRALONG2";
		case 180:
			return "SOUND_CRAZYDAVEEXTRALONG3";
		case 181:
			return "SOUND_CRAZYDAVECRAZY";
		case 182:
			return "SOUND_DANCER";
		case 183:
			return "SOUND_FINALFANFARE";
		case 184:
			return "SOUND_CRAZYDAVESCREAM";
		case 185:
			return "SOUND_CRAZYDAVESCREAM2";
		case 186:
			return "SOUND_ACHIEVEMENT";
		case 187:
			return "SOUND_BUGSPRAY";
		case 188:
			return "SOUND_FERTILISER";
		case 189:
			return "SOUND_PHONOGRAPH";
		case 190:
			return "IMAGE_PILE";
		case 191:
			return "IMAGE_PLANTSZOMBIES";
		case 192:
			return "IMAGE_PARTICLES";
		case 193:
			return "IMAGE_SLOTMACHINE_OVERLAY";
		case 194:
			return "IMAGE_ZENGARDEN";
		case 195:
			return "IMAGE_CACHED";
		case 196:
			return "IMAGE_SELECTORSCREEN_ACHIEVEMENTS_TOP_BACKGROUND";
		case 197:
			return "IMAGE_SELECTORSCREEN_MAIN_BACKGROUND";
		case 198:
			return "IMAGE_SELECTORSCREEN_ACHIEVEMENTS_HOLE";
		case 199:
			return "IMAGE_SELECTORSCREEN_ACHIEVEMENTS_HOLE_CHINA";
		case 200:
			return "IMAGE_SELECTORSCREEN_QUICKPLAY_BACKGROUND";
		case 201:
			return "IMAGE_GOODIES";
		case 202:
			return "IMAGE_QUICKPLAY";
		case 203:
			return "IMAGE_ACHIEVEMENT_GNOME";
		case 204:
			return "IMAGE_MINIGAMES";
		case 205:
			return "IMAGE_SELECTORSCREEN_MOREGAMES_BACKGROUND";
		case 206:
			return "IMAGE_CREDITS_ZOMBIENOTE";
		case 207:
			return "IMAGE_LEADERBOARDSCREEN_BACKGROUND";
		case 208:
			return "IMAGE_BLACKHOLE";
		case 209:
			return "IMAGE_EDGE_OF_SPACE";
		case 210:
			return "IMAGE_STARS_1";
		case 211:
			return "IMAGE_STARS_2";
		case 212:
			return "IMAGE_STARS_3";
		case 213:
			return "IMAGE_STARS_4";
		case 214:
			return "IMAGE_BACKGROUND_GREENHOUSE";
		case 215:
			return "IMAGE_AQUARIUM1";
		case 216:
			return "IMAGE_BACKGROUND_MUSHROOMGARDEN";
		case 217:
			return "IMAGE_BACKGROUND1";
		case 218:
			return "IMAGE_BACKGROUND1_GAMEOVER_INTERIOR_OVERLAY";
		case 219:
			return "IMAGE_BACKGROUND1_GAMEOVER_MASK";
		case 220:
			return "IMAGE_BACKGROUND1UNSODDED";
		case 221:
			return "IMAGE_BACKGROUND2";
		case 222:
			return "IMAGE_BACKGROUND2_GAMEOVER_INTERIOR_OVERLAY";
		case 223:
			return "IMAGE_BACKGROUND2_GAMEOVER_MASK";
		case 224:
			return "IMAGE_BACKGROUND3";
		case 225:
			return "IMAGE_BACKGROUND3_GAMEOVER_INTERIOR_OVERLAY";
		case 226:
			return "IMAGE_BACKGROUND3_GAMEOVER_MASK";
		case 227:
			return "IMAGE_BACKGROUND4";
		case 228:
			return "IMAGE_BACKGROUND4_GAMEOVER_INTERIOR_OVERLAY";
		case 229:
			return "IMAGE_BACKGROUND4_GAMEOVER_MASK";
		case 230:
			return "IMAGE_FOG";
		case 231:
			return "IMAGE_BACKGROUND5";
		case 232:
			return "IMAGE_BACKGROUND5_GAMEOVER_MASK";
		case 233:
			return "IMAGE_BACKGROUND6BOSS";
		case 234:
			return "IMAGE_BACKGROUND6_GAMEOVER_MASK";
		case 235:
			return "IMAGE_STORE_BACKGROUND";
		case 236:
			return "IMAGE_STORE_BACKGROUNDNIGHT";
		case 237:
			return "IMAGE_STORE_CAR";
		case 238:
			return "IMAGE_STORE_CAR_NIGHT";
		case 239:
			return "IMAGE_STORE_CARCLOSED";
		case 240:
			return "IMAGE_STORE_CARCLOSED_NIGHT";
		case 241:
			return "IMAGE_STORE_HATCHBACKOPEN";
		case 242:
			return "IMAGE_ZOMBIE_NOTE";
		case 243:
			return "IMAGE_ZOMBIE_NOTE1";
		case 244:
			return "IMAGE_ZOMBIE_NOTE2";
		case 245:
			return "IMAGE_ZOMBIE_NOTE3";
		case 246:
			return "IMAGE_ZOMBIE_NOTE4";
		case 247:
			return "IMAGE_ZOMBIE_FINAL_NOTE";
		case 248:
			return "IMAGE_ZOMBIE_NOTE_HELP";
		default:
			return "";
		}
	}
}
