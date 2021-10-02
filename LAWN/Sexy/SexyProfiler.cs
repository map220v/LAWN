using System;
using System.Collections.Generic;

namespace Sexy
{
	internal class SexyProfiler
	{
		private List<Profile> mProfiles;

		private bool mActive;

		public SexyProfiler()
		{
			mActive = false;
			mProfiles = new List<Profile>();
		}

		public void Active(bool active)
		{
			mActive = active;
		}

		public void StartFrame()
		{
			mProfiles.Clear();
		}

		public void StartProfile(string name)
		{
			if (mActive)
			{
				Profile profile = new Profile();
				profile.mSectionName = name;
				profile.mStart = GetCurrentTime();
				mProfiles.Add(profile);
			}
		}

		public void EndProfile(string name)
		{
			if (!mActive)
			{
				return;
			}
			int num = 0;
			while (true)
			{
				if (num < mProfiles.Count)
				{
					if (mProfiles[num].mSectionName == name)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			mProfiles[num].mEnd = GetCurrentTime();
		}

		public int GetCurrentTime()
		{
			return Environment.TickCount;
		}

		public void PrintProfiles()
		{
			if (mActive)
			{
				for (int i = 0; i < mProfiles.Count; i++)
				{
					Debug.OutputDebug(string.Concat(str3: (mProfiles[i].mEnd - mProfiles[i].mStart).ToString(), str0: "Section Name: ", str1: mProfiles[i].mSectionName, str2: "\n\tTotal Time(ms): "));
				}
			}
		}
	}
}
