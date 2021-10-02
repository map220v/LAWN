using Sexy;
using System;
using System.Collections.Generic;

namespace Lawn
{
	internal class ProfileMgr
	{
		private const int saveFileVersion = 1;

		private const int saveCheckNumber = 555;

		private Dictionary<string, PlayerInfo> mProfileMap = new Dictionary<string, PlayerInfo>();

		private static uint mNextProfileId;

		private uint mNextProfileUseSeq;

		public DateTime mLastMoreGamesUpdate = default(DateTime);

		private string activeUser;

		protected void DeleteOldestProfile()
		{
			if (mProfileMap.Count == 0)
			{
				return;
			}
			Dictionary<string, PlayerInfo>.Enumerator enumerator = mProfileMap.GetEnumerator();
			Dictionary<string, PlayerInfo>.Enumerator enumerator2 = enumerator;
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Value.mUseSeq < enumerator2.Current.Value.mUseSeq)
				{
					enumerator2 = enumerator;
				}
			}
			enumerator2.Current.Value.DeleteUserFiles();
			mProfileMap.Remove(enumerator2.Current.Key);
		}

		protected void DeleteOldProfiles()
		{
			while (mProfileMap.Count > 200)
			{
				DeleteOldestProfile();
			}
		}

		public ProfileMgr()
		{
			Clear();
		}

		public void Dispose()
		{
		}

		public void Clear()
		{
			mProfileMap.Clear();
			mNextProfileId = 1u;
			mNextProfileUseSeq = 1u;
			mLastMoreGamesUpdate = DateTime.Now;
		}

		private static string GetSaveFile()
		{
			return GlobalStaticVars.GetDocumentsDir() + "userdata/users.dat";
		}

		public void Load()
		{
			try
			{
				string saveFile = GetSaveFile();
				Sexy.Buffer theBuffer = new Sexy.Buffer();
				if (GlobalStaticVars.gSexyAppBase.ReadBufferFromFile(saveFile, ref theBuffer, false))
				{
					int num = theBuffer.ReadLong();
					int num4 = 1;
					int num2 = theBuffer.ReadLong();
					for (int i = 0; i < num2; i++)
					{
						uint num3 = (uint)theBuffer.ReadLong();
						if (mNextProfileId <= num3)
						{
							mNextProfileId = num3 + 1;
						}
						PlayerInfo playerInfo = new PlayerInfo(num3);
						if (playerInfo.LoadDetails())
						{
							mProfileMap.Add(playerInfo.mName, playerInfo);
						}
					}
					activeUser = theBuffer.ReadString();
					if (theBuffer.ReadLong() != 555)
					{
						throw new Exception("Profile Manager: Save check number mismatch");
					}
				}
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				mProfileMap.Clear();
			}
		}

		public void Save()
		{
			try
			{
				string saveFile = GetSaveFile();
				Sexy.Buffer buffer = new Sexy.Buffer();
				buffer.WriteLong(1);
				buffer.WriteLong(mProfileMap.Count);
				foreach (PlayerInfo value in mProfileMap.Values)
				{
					buffer.WriteLong((int)value.mId);
					value.SaveDetails();
				}
				buffer.WriteString((GlobalStaticVars.gLawnApp.mPlayerInfo != null) ? GlobalStaticVars.gLawnApp.mPlayerInfo.mName : string.Empty);
				buffer.WriteLong(555);
				GlobalStaticVars.gSexyAppBase.WriteBufferToFile(saveFile, buffer);
			}
			catch (Exception ex)
			{
				string message = ex.Message;
			}
		}

		public int GetNumProfiles()
		{
			return mProfileMap.Count;
		}

		public PlayerInfo GetProfile(string theName)
		{
			if (mProfileMap.ContainsKey(theName))
			{
				PlayerInfo playerInfo = mProfileMap[theName];
				playerInfo.LoadDetails();
				playerInfo.mUseSeq = mNextProfileUseSeq++;
				return playerInfo;
			}
			return null;
		}

		public PlayerInfo AddProfile(string theName)
		{
			if (mProfileMap.ContainsKey(theName))
			{
				return mProfileMap[theName];
			}
			mProfileMap.Add(theName, new PlayerInfo());
			PlayerInfo playerInfo = mProfileMap[theName];
			playerInfo.mName = theName;
			playerInfo.mId = GetNewProfileId();
			playerInfo.mUseSeq = mNextProfileUseSeq++;
			DeleteOldProfiles();
			return playerInfo;
		}

		public static uint GetNewProfileId()
		{
			return mNextProfileId++;
		}

		public PlayerInfo GetAnyProfile()
		{
			if (mProfileMap.Count == 0)
			{
				return null;
			}
			if (!string.IsNullOrEmpty(activeUser))
			{
				foreach (PlayerInfo value2 in mProfileMap.Values)
				{
					if (value2.mName == activeUser)
					{
						return value2;
					}
				}
			}
			Dictionary<string, PlayerInfo>.Enumerator enumerator2 = mProfileMap.GetEnumerator();
			enumerator2.MoveNext();
			PlayerInfo value = enumerator2.Current.Value;
			value.LoadDetails();
			value.mUseSeq = mNextProfileUseSeq++;
			return value;
		}

		public bool DeleteProfile(string theName)
		{
			mProfileMap[theName].DeleteUserFiles();
			return mProfileMap.Remove(theName);
		}

		public bool RenameProfile(string theOldName, string theNewName)
		{
			if (theOldName == theNewName)
			{
				return true;
			}
			if (mProfileMap.ContainsKey(theOldName))
			{
				PlayerInfo playerInfo = mProfileMap[theOldName];
				mProfileMap.Remove(theOldName);
				playerInfo.mName = theNewName;
				mProfileMap.Add(theNewName, playerInfo);
				return true;
			}
			return false;
		}

		public Dictionary<string, PlayerInfo> GetProfileMap()
		{
			return mProfileMap;
		}
	}
}
