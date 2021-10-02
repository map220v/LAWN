using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Sexy
{
	internal static class LeaderBoardComm
	{
		public enum ConnectionState
		{
			Connected,
			Connecting,
			CannotConnect
		}

		private enum LeaderboardMode
		{
			Adventure,
			IZombie,
			Vasebreaker,
			MAX
		}

		private enum ColumnIndices
		{
			Score
		}

		public delegate void LoadingCompletedHandler();

		private const int cannotConnectDelay = int.MaxValue;

		public static object LeaderboardLock;

		private static DateTime cannotConnectSince;

		private static string[] columnIndexStrings;

		private static LeaderBoardLoader[] leaderboardLoaders;

		private static Dictionary<string, Image> gamerImages;

		private static string e;

		//private static List<Gamer> loadingGamers;

		public static ConnectionState State
		{
			get;
			private set;
		}

		public static Image UnknownPlayerImage
		{
			get
			{
				return null;
			}
		}

		public static event LoadingCompletedHandler LoadingCompleted;

		static LeaderBoardComm()
		{
			LeaderboardLock = new object();
			columnIndexStrings = new string[1]
			{
				"SCORE"
			};
			leaderboardLoaders = new LeaderBoardLoader[3];
			gamerImages = new Dictionary<string, Image>();
			//loadingGamers = new List<Gamer>();
			State = ConnectionState.Connecting;
			for (int i = 0; i < 3; i++)
			{
				if (LeaderBoardHelper.IsModeSupported((LeaderboardGameMode)i))
				{
					leaderboardLoaders[i] = new LeaderBoardLoader((LeaderboardGameMode)i);
					leaderboardLoaders[i].LoadingCompleted += GetResultsCallBack;
				}
			}
		}

		public static void RecordResult(LeaderboardGameMode gameMode, int score)
		{
			/*if (SexyAppBase.UseLiveServers && !SexyAppBase.IsInTrialMode && LeaderBoardHelper.IsModeSupported(gameMode) && Gamer.SignedInGamers.Count != 0)
			{
				SignedInGamer gamer = Main.GetGamer();
				lock (LeaderboardLock)
				{
					try
					{
						int leaderboardNumber = LeaderBoardHelper.GetLeaderboardNumber(gameMode);
						LeaderboardIdentity leaderboardId = LeaderboardIdentity.Create(LeaderboardKey.BestScoreLifeTime, leaderboardNumber);
						LeaderboardWriter leaderboardWriter = gamer.LeaderboardWriter;
						LeaderboardEntry leaderboard = leaderboardWriter.GetLeaderboard(leaderboardId);
						leaderboard.Rating = score;
						LeaderBoardLoader[] array = leaderboardLoaders;
						foreach (LeaderBoardLoader leaderBoardLoader in array)
						{
							leaderBoardLoader.ResetCache();
						}
					}
					catch (GameUpdateRequiredException)
					{
						GlobalStaticVars.gSexyAppBase.ShowUpdateRequiredMessage();
					}
					catch (Exception)
					{
					}
				}
			}*/
		}

		/*public static bool IsPlayer(Gamer signedInGamer, int index, LeaderboardState state)
		{
			if (!SexyAppBase.UseLiveServers)
			{
				return false;
			}
			lock (LeaderboardLock)
			{
				try
				{
					if (signedInGamer == null)
					{
						return false;
					}
					LeaderboardEntry entry = GetEntry(index, state);
					if (entry != null)
					{
						return signedInGamer.Gamertag == entry.Gamer.Gamertag;
					}
				}
				catch (GameUpdateRequiredException)
				{
					GlobalStaticVars.gSexyAppBase.ShowUpdateRequiredMessage();
				}
				catch (Exception)
				{
				}
			}
			return false;
		}*/

		public static void LoadInitialLeaderboard()
		{
			for (int i = 0; i < 3; i++)
			{
				LoadResults((LeaderboardGameMode)i);
			}
		}

		public static int GetMaxEntries(LeaderboardState state)
		{
			LeaderBoardLoader loader = GetLoader(state);
			return loader.LeaderboardEntryCount;
		}

		private static void GetGamerCallBack(IAsyncResult result)
		{
			/*lock (LeaderboardLock)
			{
				try
				{
					Gamer gamer = result.AsyncState as Gamer;
					if (gamer != null)
					{
						if (gamerImages.ContainsKey(gamer.Gamertag))
						{
							gamerImages.Remove(gamer.Gamertag);
						}
						GamerProfile gamerProfile = gamer.EndGetProfile(result);
						Texture2D theTexture = Texture2D.FromStream(GlobalStaticVars.g.GraphicsDevice, gamerProfile.GetGamerPicture());
						Image value = new Image(theTexture);
						gamerImages.Add(gamer.Gamertag, value);
					}
				}
				catch (GameUpdateRequiredException)
				{
					GlobalStaticVars.gSexyAppBase.ShowUpdateRequiredMessage();
				}
				catch (Exception)
				{
				}
			}*/
		}

		/*public static Image GetGamerImage(Gamer gamer)
		{
			Image value = UnknownPlayerImage;
			if (!SexyAppBase.UseLiveServers || gamer == null)
			{
				return UnknownPlayerImage;
			}
			lock (LeaderboardLock)
			{
				if (!gamerImages.TryGetValue(gamer.Gamertag, out value))
				{
					value = UnknownPlayerImage;
					gamerImages.Add(gamer.Gamertag, value);
					try
					{
						gamer.BeginGetProfile(GetGamerCallBack, gamer);
						return value;
					}
					catch (GameUpdateRequiredException)
					{
						GlobalStaticVars.gSexyAppBase.ShowUpdateRequiredMessage();
						return value;
					}
					catch (Exception ex2)
					{
						e = ex2.Message;
						return value;
					}
				}
				return value;
			}
		}*/

		public static Image GetLeaderboardGamerImage(int index, LeaderboardState state)
		{
			//if (!SexyAppBase.UseLiveServers)
			//{
				return UnknownPlayerImage;
			/*}
			lock (LeaderboardLock)
			{
				Gamer gamer = null;
				try
				{
					LeaderboardEntry entry = GetEntry(index, state);
					if (entry != null)
					{
						gamer = entry.Gamer;
					}
				}
				catch (GameUpdateRequiredException)
				{
					GlobalStaticVars.gSexyAppBase.ShowUpdateRequiredMessage();
				}
				catch (Exception)
				{
				}
				return GetGamerImage(gamer);
			}*/
		}

		/*public static Gamer GetLeaderboardGamer(int index, LeaderboardState state)
		{
			if (!SexyAppBase.UseLiveServers)
			{
				return null;
			}
			lock (LeaderboardLock)
			{
				Gamer result = null;
				try
				{
					LeaderboardEntry entry = GetEntry(index, state);
					if (entry != null)
					{
						result = entry.Gamer;
					}
				}
				catch (GameUpdateRequiredException)
				{
					GlobalStaticVars.gSexyAppBase.ShowUpdateRequiredMessage();
				}
				catch (Exception)
				{
				}
				return result;
			}
		}*/

		public static int GetSignedInGamerIndex(LeaderboardState state)
		{
			LeaderBoardLoader loader = GetLoader(state);
			return loader.SignedInGamerIndex;
		}

		private static LeaderBoardLoader GetLoader(LeaderboardState state)
		{
			return leaderboardLoaders[LeaderBoardHelper.GetLeaderboardNumber(state)];
		}

		private static LeaderBoardLoader GetLoader(LeaderboardGameMode mode)
		{
			return leaderboardLoaders[LeaderBoardHelper.GetLeaderboardNumber(mode)];
		}

		/*private static LeaderboardEntry GetEntry(int index, LeaderboardState state)
		{
			LeaderBoardLoader leaderBoardLoader = leaderboardLoaders[LeaderBoardHelper.GetLeaderboardNumber(state)];
			LeaderboardEntry value;
			if (!leaderBoardLoader.LeaderboardEntries.TryGetValue(index, out value))
			{
				leaderBoardLoader.LoadEntry(index);
			}
			return value;
		}*/

		public static long GetLeaderboardScore(int index, LeaderboardState state)
		{
			/*if (!SexyAppBase.UseLiveServers)
			{
				return 0L;
			}
			lock (LeaderboardLock)
			{
				try
				{
					LeaderboardEntry entry = GetEntry(index, state);
					if (entry != null)
					{
						return entry.Rating;
					}
				}
				catch (GameUpdateRequiredException)
				{
					GlobalStaticVars.gSexyAppBase.ShowUpdateRequiredMessage();
				}
				catch (Exception)
				{
				}
			}*/
			return 0L;
		}

		public static void SetCache(LeaderboardGameMode gameMode)
		{
			LeaderBoardLoader loader = GetLoader(gameMode);
			loader.CACHE_DURATION = 10;
		}

		public static int LoadResults(LeaderboardGameMode gameMode)
		{
			if (!SexyAppBase.UseLiveServers || State == ConnectionState.CannotConnect)
			{
				if (State == ConnectionState.CannotConnect && (DateTime.UtcNow - cannotConnectSince).TotalSeconds > 2147483647.0)
				{
					State = ConnectionState.Connecting;
				}
				return -2;
			}
			lock (LeaderboardLock)
			{
				//if (Gamer.SignedInGamers.Count == 0)
				//{
					return -2;
				//}
				LeaderBoardLoader loader = GetLoader(gameMode);
				loader.SendRequest();
				State = ConnectionState.Connecting;
				if (loader.LeaderboardConnectionState == LeaderBoardLoader.LoaderState.Loaded || (loader.LeaderboardConnectionState == LeaderBoardLoader.LoaderState.Loading && loader.LeaderboardEntryCount > 0))
				{
					loader.CACHE_DURATION = int.MaxValue;
					return loader.LeaderboardEntryCount;
				}
				loader.CACHE_DURATION = int.MaxValue;
			}
			return -1;
		}

		private static void GetResultsCallBack(LeaderBoardLoader loader)
		{
			lock (LeaderboardLock)
			{
				switch (loader.ErrorState)
				{
				case LeaderBoardLoader.ErrorStates.None:
					State = ConnectionState.Connected;
					break;
				case LeaderBoardLoader.ErrorStates.GameUpdateRequired:
					GlobalStaticVars.gSexyAppBase.ShowUpdateRequiredMessage();
					break;
				case LeaderBoardLoader.ErrorStates.Error:
					State = ConnectionState.CannotConnect;
					cannotConnectSince = DateTime.UtcNow;
					break;
				}
				if (LeaderBoardComm.LoadingCompleted != null)
				{
					LeaderBoardComm.LoadingCompleted();
				}
			}
		}
	}
}
