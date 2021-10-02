using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sexy.TodLib
{
	internal static class TodStringFile
	{
		public static TodStringListFormat[] gTodDefaultStringFormats = new TodStringListFormat[2]
		{
			new TodStringListFormat("NORMAL", null, new SexyColor(40, 50, 90), 0, 0u),
			new TodStringListFormat("KEYWORD", null, new SexyColor(143, 67, 27), 0, 0u)
		};

		public static TodStringListFormat[] gTodStringFormats = gTodDefaultStringFormats;

		public static int gTodStringFormatCount = gTodDefaultStringFormats.Length;

		internal static Dictionary<string, string> gStringProperties = new Dictionary<string, string>();

		public static bool StringsLoaded = false;

		private static Dictionary<string, string> todStringTranslateCache = new Dictionary<string, string>(100);

		private static StringBuilder TodWriteStringBuilder = new StringBuilder();

		private static TodStringListFormat aCurrentFormat = new TodStringListFormat();

		public static void TodStringListSetColors(TodStringListFormat[] theFormats, int theCount)
		{
			gTodStringFormats = theFormats;
			gTodStringFormatCount = theCount;
		}

		public static void TodStringListLoad(string theFileName)
		{
			if (!TodStringListReadFile(theFileName))
			{
				Common.StrFormat_("Failed to load string list file '{0}'", theFileName);
			}
			StringsLoaded = true;
		}

		public static string TodStringTranslate(string theString)
		{
			if (theString.Length >= 3 && theString[0] == '[')
			{
				string value;
				if (!todStringTranslateCache.TryGetValue(theString, out value))
				{
					value = theString.Substring(1, theString.Length - 2);
					todStringTranslateCache.Add(theString, value);
				}
				return TodStringListFind(value);
			}
			return theString;
		}

		public static bool TodStringListExists(string theString)
		{
			int num = theString.length();
			if (num < 3 || theString[0] != '[')
			{
				return false;
			}
			string key = theString.Substring(1, num - 2);
			return gStringProperties.ContainsKey(key);
		}

		public static int TodDrawStringWrapped(Graphics g, string theText, TRect theRect, Font theFont, SexyColor theColor, DrawStringJustification theJustification)
		{
			return TodDrawStringWrapped(g, theText, theRect, theFont, theColor, theJustification, '\n');
		}

		public static int TodDrawStringWrapped(Graphics g, string theText, TRect theRect, Font theFont, SexyColor theColor, DrawStringJustification theJustification, char lineBreakChar)
		{
			return TodDrawStringWrapped(g, theText, theRect, theFont, theColor, theJustification, false, lineBreakChar);
		}

		public static int TodDrawStringWrappedHeight(Graphics g, string theText, TRect theRect, Font theFont, SexyColor theColor, DrawStringJustification theJustification)
		{
			return TodDrawStringWrappedHeight(g, theText, theRect, theFont, theColor, theJustification, false);
		}

		public static int TodDrawStringWrappedHeight(Graphics g, string theText, TRect theRect, Font theFont, SexyColor theColor, DrawStringJustification theJustification, bool widowSupression)
		{
			string theText2 = TodStringTranslate(theText);
			TRect theRect2 = theRect;
			if (theJustification == DrawStringJustification.DS_ALIGN_LEFT_VERTICAL_MIDDLE || theJustification == DrawStringJustification.DS_ALIGN_RIGHT_VERTICAL_MIDDLE || theJustification == DrawStringJustification.DS_ALIGN_CENTER_VERTICAL_MIDDLE)
			{
				int num = TodDrawStringWrappedHelper(g, theText2, theRect2, theFont, theColor, theJustification, false, widowSupression);
				theRect2.mY += (theRect.mHeight - num) / 2;
			}
			return TodDrawStringWrappedHelper(g, theText2, theRect2, theFont, theColor, theJustification, false, widowSupression);
		}

		public static int TodDrawStringWrapped(Graphics g, string theText, TRect theRect, Font theFont, SexyColor theColor, DrawStringJustification theJustification, bool widowSupression)
		{
			return TodDrawStringWrapped(g, theText, theRect, theFont, theColor, theJustification, widowSupression, '\n');
		}

		public static int TodDrawStringWrapped(Graphics g, string theText, TRect theRect, Font theFont, SexyColor theColor, DrawStringJustification theJustification, bool widowSupression, char lineBreakChar)
		{
			string theText2 = TodStringTranslate(theText);
			TRect theRect2 = theRect;
			if (theJustification == DrawStringJustification.DS_ALIGN_LEFT_VERTICAL_MIDDLE || theJustification == DrawStringJustification.DS_ALIGN_RIGHT_VERTICAL_MIDDLE || theJustification == DrawStringJustification.DS_ALIGN_CENTER_VERTICAL_MIDDLE)
			{
				int num = TodDrawStringWrappedHelper(g, theText2, theRect2, theFont, theColor, theJustification, false, widowSupression);
				theRect2.mY += (theRect.mHeight - num) / 2;
			}
			return TodDrawStringWrappedHelper(g, theText2, theRect2, theFont, theColor, theJustification, true, widowSupression, lineBreakChar);
		}

		public static bool TodStringListReadName(string thePtr, ref string theName, ref int startIndex)
		{
			int num = thePtr.IndexOf('[', startIndex);
			if (num < 0)
			{
				if (strspn(thePtr, "\n\r\t".ToCharArray(), startIndex) != -1)
				{
					Debug.OutputDebug("Failed to find string name");
					return false;
				}
				theName = string.Empty;
				return true;
			}
			int num2 = num + 1;
			int num3 = thePtr.IndexOf(']', startIndex);
			if (num3 < 0)
			{
				Debug.OutputDebug("Failed to find ']'");
				return false;
			}
			int length = num3 - num2;
			theName = thePtr.Substring(num2, length);
			theName = theName.Trim();
			if (theName.Length == 0)
			{
				Debug.OutputDebug("Name too short");
				return false;
			}
			startIndex = num3 + 1;
			return true;
		}

		public static void TodStringRemoveReturnChars(ref string theValue)
		{
			theValue = theValue.Replace("\r", "");
		}

		public static bool TodStringListReadValue(string thePtr, ref string theValue, ref int startIndex)
		{
			int num = thePtr.IndexOf('[', startIndex);
			if (num < 0)
			{
				theValue = thePtr.Substring(startIndex);
			}
			else
			{
				theValue = thePtr.Substring(startIndex, num - startIndex);
			}
			if (theValue.IndexOf("/c/") != -1)
			{
				theValue = theValue.Replace("/c/", "©");
			}
			theValue = theValue.Trim();
			TodStringRemoveReturnChars(ref theValue);
			int num2 = 0;
			int num3;
			do
			{
				num3 = theValue.IndexOf("%");
				if (num3 >= 0)
				{
					if (!char.IsWhiteSpace(theValue[num3 + 1]))
					{
						theValue = theValue.Remove(num3, 2);
						theValue = theValue.Insert(num3, "{" + num2.ToString() + "}");
						num2++;
					}
					else
					{
						num3 = -1;
					}
				}
			}
			while (num3 != -1);
			return true;
		}

		public static bool TodStringListReadItems(string theFileText)
		{
			int startIndex = 0;
			while (true)
			{
				string theName = string.Empty;
				if (!TodStringListReadName(theFileText, ref theName, ref startIndex))
				{
					return false;
				}
				if (theName.Length == 0)
				{
					return true;
				}
				string theValue = string.Empty;
				if (!TodStringListReadValue(theFileText, ref theValue, ref startIndex))
				{
					break;
				}
				string text = theName.ToUpper();
				gStringProperties[text] = theValue;
				GlobalStaticVars.gSexyAppBase.SetString(text, theValue);
			}
			return false;
		}

		public static bool TodStringListReadFile(string theFileName)
		{
			try
			{
				using (Stream stream = TitleContainer.OpenStream(theFileName))
				{
					using (StreamReader streamReader = new StreamReader(stream))
					{
						string theFileText = streamReader.ReadToEnd();
						if (!TodStringListReadItems(theFileText))
						{
							return false;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.OutputDebug("The file: " + theFileName + " could not be read");
				Debug.OutputDebug(ex.Message);
				return false;
			}
			return true;
		}

		public static string TodStringListFind(string theName)
		{
			if (gStringProperties.ContainsKey(theName))
			{
				return gStringProperties[theName];
			}
			return Common.StrFormat_("<Missing {0}>", theName);
		}

		internal static bool CharIsSpaceInFormat(char theChar, TodStringListFormat theCurrentFormat)
		{
			if (theChar == ' ')
			{
				return true;
			}
			if (TodCommon.TestBit(theCurrentFormat.mFormatFlags, 0) && theChar == '\n')
			{
				return true;
			}
			return false;
		}

		internal static void TodWriteStringSetFormat(string theFormat, ref TodStringListFormat theCurrentFormat)
		{
			int num = 0;
			TodStringListFormat todStringListFormat;
			while (true)
			{
				if (num < gTodStringFormatCount)
				{
					todStringListFormat = gTodStringFormats[num];
					int length = todStringListFormat.mFormatName.Length;
					if (string.Compare(theFormat, todStringListFormat.mFormatName) == 0)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			if (todStringListFormat.mNewFont != null)
			{
				theCurrentFormat.mNewFont = todStringListFormat.mNewFont;
			}
			theCurrentFormat.mNewColor = todStringListFormat.mNewColor;
			theCurrentFormat.mLineSpacingOffset = todStringListFormat.mLineSpacingOffset;
			theCurrentFormat.mFormatFlags = todStringListFormat.mFormatFlags;
		}

		internal static int TodWriteString(Graphics g, string theString, int theX, int theY, TodStringListFormat theCurrentFormat, int theWidth, DrawStringJustification theJustification, bool drawString, int theOffset, int theLength)
		{
			Font mNewFont = theCurrentFormat.mNewFont;
			if (drawString)
			{
				switch (theJustification)
				{
				case DrawStringJustification.DS_ALIGN_CENTER:
				case DrawStringJustification.DS_ALIGN_CENTER_VERTICAL_MIDDLE:
					theX += (theWidth - TodWriteString(g, theString, theX, theY, theCurrentFormat, theWidth, DrawStringJustification.DS_ALIGN_LEFT, false, theOffset, theLength)) / 2;
					break;
				case DrawStringJustification.DS_ALIGN_RIGHT:
				case DrawStringJustification.DS_ALIGN_RIGHT_VERTICAL_MIDDLE:
					theX += theWidth - TodWriteString(g, theString, theX, theY, theCurrentFormat, theWidth, DrawStringJustification.DS_ALIGN_LEFT, false, theOffset, theLength);
					break;
				}
			}
			theLength = ((theLength >= 0 && theOffset + theLength <= theString.length()) ? (theOffset + theLength) : theString.length());
			TodWriteStringBuilder.Remove(0, TodWriteStringBuilder.Length);
			int num = 0;
			bool flag = false;
			int num2 = 0;
			for (int i = theOffset; i < theLength; i++)
			{
				if (theString[i] == '{')
				{
					int num3 = num2 + i;
					int startIndex = num3 + 1;
					int num4 = theString.IndexOf('}', startIndex);
					if (num4 >= 0)
					{
						i += num4 - num3;
						if (drawString)
						{
							mNewFont.DrawString(g, g.mTransX + theX + num, g.mTransY + theY, TodWriteStringBuilder, theCurrentFormat.mNewColor);
						}
						num += mNewFont.StringWidth(TodWriteStringBuilder);
						TodWriteStringBuilder.Remove(0, TodWriteStringBuilder.Length);
						string theFormat = theString.Substring(startIndex, num4 - (num3 + 1));
						TodWriteStringSetFormat(theFormat, ref theCurrentFormat);
						mNewFont = theCurrentFormat.mNewFont;
					}
					continue;
				}
				if (TodCommon.TestBit(theCurrentFormat.mFormatFlags, 0))
				{
					if (CharIsSpaceInFormat(theString[i], theCurrentFormat))
					{
						if (!flag)
						{
							TodWriteStringBuilder.Append(" ");
							flag = true;
						}
						continue;
					}
					flag = false;
				}
				TodWriteStringBuilder.Append(theString[i]);
			}
			if (drawString)
			{
				mNewFont.DrawString(g, g.mTransX + theX + num, g.mTransY + theY, TodWriteStringBuilder, theCurrentFormat.mNewColor);
			}
			return num + mNewFont.StringWidth(TodWriteStringBuilder);
		}

		internal static int TodWriteWordWrappedHelper(Graphics g, string theString, int theX, int theY, TodStringListFormat theCurrentFormat, int theWidth, DrawStringJustification theJustification, bool drawString, int theOffset, int theLength, int theMaxChars)
		{
			if (theOffset + theLength > theMaxChars)
			{
				theLength = theMaxChars - theOffset;
				if (theLength <= 0)
				{
					return -1;
				}
			}
			return TodWriteString(g, theString, theX, theY, theCurrentFormat, theWidth, theJustification, drawString, theOffset, theLength);
		}

		internal static void GetWidowRange(string theText, ref int theStartPos, ref int theEndPos)
		{
			int num = theText.length() - 1;
			int num2 = 0;
			theStartPos = (theEndPos = -1);
			while (num >= num2 && theText[num] != ' ')
			{
				num--;
			}
			if (num >= num2)
			{
				theEndPos = num - num2;
				while (num >= num2 && theText[num] == ' ')
				{
					num--;
				}
				theStartPos = num - num2 + 1;
			}
		}

		internal static int TodDrawStringWrappedHelper(Graphics g, string theText, TRect theRect, Font theFont, SexyColor theColor, DrawStringJustification theJustification, bool drawString, bool widowSuppression)
		{
			return TodDrawStringWrappedHelper(g, theText, theRect, theFont, theColor, theJustification, drawString, widowSuppression, '\n');
		}

		internal static int TodDrawStringWrappedHelper(Graphics g, string theText, TRect theRect, Font theFont, SexyColor theColor, DrawStringJustification theJustification, bool drawString, bool widowSuppression, char lineBreakChar)
		{
			int theMaxChars = theText.length();
			aCurrentFormat.Reset();
			aCurrentFormat.mFormatName = string.Empty;
			aCurrentFormat.mNewFont = theFont;
			aCurrentFormat.mNewColor.CopyFrom(theColor);
			aCurrentFormat.mLineSpacingOffset = 0;
			aCurrentFormat.mFormatFlags = 0u;
			Font mNewFont = aCurrentFormat.mNewFont;
			int num = 0;
			int num2 = mNewFont.GetLineSpacing() + aCurrentFormat.mLineSpacingOffset;
			string empty = string.Empty;
			int i = 0;
			int num3 = 0;
			int num4 = 0;
			char c = ' ';
			char thePrevChar = ' ';
			int num5 = -1;
			int num6 = 0;
			int num7 = 0;
			int theStartPos = -1;
			int theEndPos = -1;
			if (widowSuppression)
			{
				GetWidowRange(theText, ref theStartPos, ref theEndPos);
			}
			while (i < theText.length())
			{
				c = theText[i];
				if (c == '{')
				{
					int num8 = i;
					int num9 = theText.IndexOf('}', num8 + 1);
					string theFormat = theText.Substring(num8 + 1, num9 - (num8 + 1));
					if (num9 != -1)
					{
						i += num9 - num8 + 1;
						int num10 = mNewFont.GetAscent() - mNewFont.GetAscentPadding();
						SexyColor mNewColor = aCurrentFormat.mNewColor;
						TodWriteStringSetFormat(theFormat, ref aCurrentFormat);
						aCurrentFormat.mNewColor = mNewColor;
						if (aCurrentFormat.mNewColor.mAlpha == 0)
						{
							int num11 = 0;
							num11++;
						}
						int num12 = mNewFont.GetAscent() - mNewFont.GetAscentPadding();
						num2 = mNewFont.GetLineSpacing() + aCurrentFormat.mLineSpacingOffset;
						num += num12 - num10;
						continue;
					}
				}
				else if (CharIsSpaceInFormat(c, aCurrentFormat))
				{
					if (!widowSuppression || i < theStartPos || i > theEndPos)
					{
						num5 = i;
					}
					c = ' ';
				}
				else if (c == '\n' || c == lineBreakChar)
				{
					num4 = theRect.mWidth + 1;
					num5 = i;
					i++;
				}
				num4 += mNewFont.CharWidthKern(c, thePrevChar);
				thePrevChar = c;
				if (num4 > theRect.mWidth)
				{
					int num13;
					if (num5 != -1)
					{
						TodWriteWordWrappedHelper(g, theText, theRect.mX + num7, theRect.mY + num, aCurrentFormat, theRect.mWidth, theJustification, drawString, num3, num5 - num3, theMaxChars);
						num13 = num4 + num7;
						if (num13 < 0)
						{
							break;
						}
						i = num5 + 1;
						if (c != '\n')
						{
							for (; i < theText.length() && CharIsSpaceInFormat(theText[i], aCurrentFormat); i++)
							{
							}
						}
						num3 = i;
					}
					else
					{
						if (i < num3 + 1)
						{
							i++;
						}
						num13 = TodWriteWordWrappedHelper(g, theText, theRect.mX + num7, theRect.mY + num, aCurrentFormat, theRect.mWidth, theJustification, drawString, num3, i - num3, theMaxChars);
						if (num13 < 0)
						{
							break;
						}
					}
					if (num13 > num6)
					{
						num6 = num13;
					}
					num3 = i;
					num5 = -1;
					num4 = 0;
					num7 = 0;
					num += num2;
				}
				else
				{
					i++;
				}
			}
			if (num3 < theText.length())
			{
				int num14 = TodWriteWordWrappedHelper(g, theText, theRect.mX + num7, theRect.mY + num, aCurrentFormat, theRect.mWidth, theJustification, drawString, num3, theText.length() - num3, theMaxChars);
				if (num14 >= 0)
				{
					if (num14 > num6)
					{
						num6 = num14;
					}
					num += num2;
				}
			}
			else if (c == '\n')
			{
				num += num2;
			}
			return num - mNewFont.GetDescent();
		}

		private static int strspn(string InputString, char[] Mask, int start)
		{
			int num = 0;
			bool flag = false;
			for (int i = start; i < InputString.Length; i++)
			{
				if (!Mask.Contains(InputString[i]))
				{
					flag = true;
					break;
				}
				num++;
			}
			if (!flag)
			{
				return -1;
			}
			return num;
		}
	}
}
