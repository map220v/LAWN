namespace Sexy
{
	internal static class LeaderBoardHelper
	{
		public static int GetLeaderboardNumber(LeaderboardGameMode mode)
		{
			return (int)mode;
		}

		public static int GetLeaderboardNumber(LeaderboardState state)
		{
			return (int)state;
		}

		public static bool IsModeSupported(LeaderboardGameMode mode)
		{
			if (mode != 0 && mode != LeaderboardGameMode.IZombie)
			{
				return mode == LeaderboardGameMode.Vasebreaker;
			}
			return true;
		}
	}
}
