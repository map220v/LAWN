using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;

namespace Sexy
{
	internal static class Common
	{
		internal class StringLessNoCase
		{
			public bool Equals(string s1, string s2)
			{
				return stricmp(s1, s2) < 0;
			}
		}

		internal class StringEqualNoCase
		{
			public bool Equals(string s1, string s2)
			{
				return stricmp(s1, s2) == 0;
			}
		}

		internal class StringGreaterNoCase
		{
			public bool Equals(string s1, string s2)
			{
				return stricmp(s1, s2) > 0;
			}
		}

		internal class CharToCharFunc
		{
			public static string Str(string theStr)
			{
				return theStr;
			}

			public static sbyte Char(sbyte theChar)
			{
				return theChar;
			}
		}

		public const uint SEXY_RAND_MAX = 2147483647u;

		private static string gAppDataFolder = "boogers";

		internal static char CommaChar = ',';

		private static Dictionary<string, string> StringToUpperCache = new Dictionary<string, string>(50);

		private static Dictionary<string, string> StringToLowerCache = new Dictionary<string, string>(50);

		public static double _wtof(string str)
		{
			return Convert.ToDouble(str);
		}

		public static double _wtof(char str)
		{
			return Convert.ToDouble(str);
		}

		public static int stricmp(string s1, string s2)
		{
			return string.Compare(s1, s2);
		}

		public static void SetCommaSeparator(char theSeparator)
		{
			CommaChar = theSeparator;
		}

		public static string CommaSeperate(int theValue)
		{
			return GlobalStaticVars.CommaSeperate_(theValue);
		}

		public static void inlineUpper(ref string theData)
		{
			theData = theData.ToUpper();
		}

		public static void inlineLower(ref string theData)
		{
			theData = theData.ToLower();
		}

		public static string URLEncode(string theString)
		{
			char[] array = new char[16]
			{
				'0',
				'1',
				'2',
				'3',
				'4',
				'5',
				'6',
				'7',
				'8',
				'9',
				'A',
				'B',
				'C',
				'D',
				'E',
				'F'
			};
			string text = theString;
			foreach (char c in theString)
			{
				switch (c)
				{
				case ' ':
					text.Insert(theString.Length - 1, "+");
					break;
				case '\t':
				case '\n':
				case '\r':
				case '%':
				case '&':
				case '+':
				case '?':
					text = text.Insert(theString.Length, "%");
					text = text.Insert(theString.Length, Convert.ToString(array[((int)c >> 4) & 0xF]));
					text = text.Insert(theString.Length, Convert.ToString(array[c & 0xF]));
					break;
				default:
					text = text.Insert(theString.Length, Convert.ToString(c));
					break;
				}
			}
			return text;
		}

		public static string StringToUpper(ref string theString)
		{
			string value;
			if (!StringToUpperCache.TryGetValue(theString, out value))
			{
				value = theString.ToUpper();
				StringToUpperCache.Add(theString, value);
			}
			return value;
		}

		public static string StringToLower(ref string theString)
		{
			string value;
			if (!StringToLowerCache.TryGetValue(theString, out value))
			{
				value = theString.ToLower();
				StringToLowerCache.Add(theString, value);
			}
			return value;
		}

		public static string Upper(string theString)
		{
			return StringToUpper(ref theString);
		}

		public static string Lower(string theString)
		{
			return StringToLower(ref theString);
		}

		public static string Trim(string theString)
		{
			int i;
			for (i = 0; i < theString.Length && char.IsWhiteSpace(theString[i]); i++)
			{
			}
			int num = theString.Length - 1;
			while (num >= 0 && char.IsWhiteSpace(theString[num]))
			{
				num--;
			}
			return theString.Substring(i, num - i + 1);
		}

		public static string ToString(string theString)
		{
			return theString;
		}

		public static char CharAtStringIndex(string theString, int theIndex)
		{
			Debug.ASSERT(theIndex <= theString.Length);
			int num = 0;
			foreach (char result in theString)
			{
				if (num == theIndex)
				{
					return result;
				}
				num++;
			}
			return '\0';
		}

		public static bool StringToInt(string theString, ref int theIntVal)
		{
			theIntVal = Convert.ToInt32(theString, 10);
			return true;
		}

		public static bool StringToDouble(string theString, ref double theDoubleVal)
		{
			theDoubleVal = Convert.ToDouble(theString);
			return true;
		}

		public static string XMLDecodeString(string theString)
		{
			string text = "";
			for (int i = 0; i < theString.Length; i++)
			{
				sbyte b = (sbyte)CharAtStringIndex(theString, i);
				if (b == 38)
				{
					int num = theString.IndexOf(';', i);
					if (num != -1)
					{
						string a = theString.Substring(i + 1, num - i - 1);
						i = num;
						if (a == "lt")
						{
							b = 60;
						}
						else if (a == "amp")
						{
							b = 38;
						}
						else if (a == "gt")
						{
							b = 62;
						}
						else if (a == "quot")
						{
							b = 34;
						}
						else if (a == "apos")
						{
							b = 39;
						}
						else if (a == "nbsp")
						{
							b = 32;
						}
						else if (a == "cr")
						{
							b = 10;
						}
					}
				}
				text.Insert(text.Length, Convert.ToString(b));
			}
			return text;
		}

		public static string XMLEncodeString(string theString)
		{
			string text = "";
			bool flag = false;
			for (int i = 0; i < theString.Length; i++)
			{
				sbyte b = (sbyte)CharAtStringIndex(theString, i);
				if (b == 32)
				{
					if (flag)
					{
						text += "&nbsp;";
						continue;
					}
					flag = true;
				}
				else
				{
					flag = false;
				}
				switch (b)
				{
				case 60:
					text += "&lt;";
					break;
				case 38:
					text += "&amp;";
					break;
				case 62:
					text += "&gt;";
					break;
				case 34:
					text += "&quot;";
					break;
				case 39:
					text += "&apos;";
					break;
				case 10:
					text += "&cr;";
					break;
				default:
					text += b;
					break;
				}
			}
			return text;
		}

		public static string GetFileName(string thePath)
		{
			return GetFileName(thePath, false);
		}

		public static string GetFileName(string thePath, bool noExtension)
		{
			if (noExtension)
			{
				return Path.GetFileNameWithoutExtension(thePath);
			}
			return Path.GetFileName(thePath);
		}

		public static string GetFileDir(string thePath)
		{
			return GetFileDir(thePath, false);
		}

		public static string GetFileDir(string thePath, bool withSlash)
		{
			string text = thePath;
			int length = text.LastIndexOf('/');
			text = text.Substring(0, length);
			if (withSlash)
			{
				text = text.Substring(0, text.Length - 1);
			}
			return text;
		}

		public static string GetPathFrom(string theRelPath, string theDir)
		{
			return Path.GetFullPath(theRelPath);
		}

		public static string GetFullPath(string theRelPath)
		{
			return GetPathFrom(theRelPath, GetCurDir());
		}

		public static string GetCurDir()
		{
			return Directory.GetCurrentDirectory();
		}

		public static string RemoveTrailingSlash(string theDirectory)
		{
			int length = theDirectory.Length;
			if (length > 0 && theDirectory[length - 1] == '/')
			{
				return theDirectory.Substring(0, length - 1);
			}
			return theDirectory;
		}

		public static string AddTrailingSlash(string theDirectory)
		{
			if (!string.IsNullOrEmpty(theDirectory))
			{
				sbyte b = (sbyte)theDirectory[theDirectory.Length - 1];
				if (b != 47)
				{
					return theDirectory + '/';
				}
				return theDirectory;
			}
			return "";
		}

		public static bool FileExists(string theFileName)
		{
			return File.Exists(theFileName);
		}

		public static long GetFileDate(string theFileName)
		{
			Debug.ASSERT(FileExists(theFileName));
			return File.GetLastWriteTime(theFileName).ToFileTime();
		}

		public static void Sleep(uint inTime)
		{
		}

		public static void MkDir(string theDir)
		{
			int startIndex = 0;
			IsolatedStorageFile userStoreForApplication;
			while (true)
			{
				int num = theDir.IndexOf('/', startIndex);
				userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();
				if (num == -1)
				{
					break;
				}
				startIndex = num + 1;
				theDir.Substring(0, num);
				userStoreForApplication.CreateDirectory(theDir);
			}
			userStoreForApplication.CreateDirectory(theDir);
		}

		public static bool Deltree(string thePath)
		{
			if (Directory.Exists(thePath))
			{
				Directory.Delete(thePath, true);
				return true;
			}
			return false;
		}

		public static bool DeleteFile(string lpFileName)
		{
			IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();
			if (userStoreForApplication.FileExists(lpFileName))
			{
				userStoreForApplication.DeleteFile(lpFileName);
				return true;
			}
			return false;
		}

		public static string GetAppDataFolder()
		{
			return gAppDataFolder;
		}

		public static void SetAppDataFolder(string thePath)
		{
			gAppDataFolder = AddTrailingSlash(thePath);
		}

		public static string StrFormat_(string fmt, params object[] LegacyParamArray)
		{
			return string.Format(fmt, LegacyParamArray);
		}

		public static string StrFormat_(string fmt, object a)
		{
			return string.Format(fmt, a.ToString());
		}

		public static string StrFormat_(string fmt, object a, object b)
		{
			return string.Format(fmt, a, b);
		}

		public static string StrFormat_(string fmt, object a, object b, object c)
		{
			return string.Format(fmt, a, b, c);
		}
	}
}
