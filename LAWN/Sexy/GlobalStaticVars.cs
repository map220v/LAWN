using Lawn;

namespace Sexy
{
	internal static class GlobalStaticVars
	{
		public struct CGPoint
		{
			public float x;

			public float y;
		}

		public enum Phase
		{
			TOUCH_BEGAN,
			TOUCH_MOVED,
			TOUCH_STATIONARY,
			TOUCH_ENDED,
			TOUCH_CANCELLED
		}

		public struct Touch
		{
			public CGPoint location;
		}

		public static Graphics g;

		public static GlobalContentManager mGlobalContent;

		public static SexyAppBase gSexyAppBase;

		internal static int gProfileVersion = 14;

		public static bool gIsPartnerBuild = false;

		public static int gSlowMoCounter = 0;

		public static bool gSlowMo = false;

		public static bool gFastMo = false;

		public static bool gLowFramerate = false;

		public static string gGetCurrentLevelName;

		public static bool gAppCloseRequest;

		public static bool gAppHasUsedCheatKeys;

		public static LawnApp gLawnApp
		{
			get
			{
				return (LawnApp)gSexyAppBase;
			}
		}

		public static string LawnGetCurrentLevelName()
		{
			if (gLawnApp == null)
			{
				return "Before App";
			}
			if (gLawnApp.mGameScene == GameScenes.SCENE_LOADING)
			{
				return "Game Loading";
			}
			if (gLawnApp.mGameScene == GameScenes.SCENE_MENU)
			{
				return "Game Selector";
			}
			if (gLawnApp.mGameScene == GameScenes.SCENE_AWARD)
			{
				return "Award Screen";
			}
			if (gLawnApp.mGameScene == GameScenes.SCENE_MENU)
			{
				return "Game Selector";
			}
			if (gLawnApp.mGameScene == GameScenes.SCENE_CHALLENGE)
			{
				return "Challenge Screen";
			}
			if (gLawnApp.mGameScene == GameScenes.SCENE_CREDIT)
			{
				return "Credits";
			}
			if (gLawnApp.mBoard == null)
			{
				return "Not Playing";
			}
			if (gLawnApp.IsFirstTimeAdventureMode())
			{
				return gLawnApp.GetStageString(gLawnApp.mBoard.mLevel);
			}
			return Common.StrFormat_("F{0}", gLawnApp.GetStageString(gLawnApp.mBoard.mLevel));
		}

		public static bool LawnGetCloseRequest()
		{
			if (gLawnApp == null)
			{
				return false;
			}
			return gLawnApp.mCloseRequest;
		}

		public static bool LawnHasUsedCheatKeys()
		{
			if (gLawnApp == null || gLawnApp.mPlayerInfo == null)
			{
				return false;
			}
			return !gLawnApp.mPlayerInfo.mHasUsedCheatKeys;
		}

		public static void initialize(Main main)
		{
			g = Main.graphics;
			g.Init();
			mGlobalContent = new GlobalContentManager(main);
			gGetCurrentLevelName = LawnGetCurrentLevelName();
			gAppCloseRequest = LawnGetCloseRequest();
			gAppHasUsedCheatKeys = LawnHasUsedCheatKeys();
			gSexyAppBase = new LawnApp(main);
			gSexyAppBase.Init();
			gSexyAppBase.Start();
		}

		public static void shutdown()
		{
			gLawnApp.Shutdown();
			gLawnApp.Dispose();
		}

		internal static string GetResourceDir()
		{
			return "";
		}

		internal static string CommaSeperate_(int mDispPoints)
		{
			if (mDispPoints == 0)
			{
				return "0";
			}
			return string.Format("{0:#,#}", mDispPoints);
		}

		internal static string GetDocumentsDir()
		{
			return "docs/";
		}
	}
}
