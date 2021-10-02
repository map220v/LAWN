internal static class StringFunctions
{
	private static string activestring;

	private static int activeposition;

	internal static string ChangeCharacter(string sourcestring, int charindex, char changechar)
	{
		return ((charindex > 0) ? sourcestring.Substring(0, charindex) : "") + changechar.ToString() + ((charindex < sourcestring.Length - 1) ? sourcestring.Substring(charindex + 1) : "");
	}

	internal static bool IsXDigit(char character)
	{
		if (char.IsDigit(character))
		{
			return true;
		}
		if ("ABCDEFabcdef".IndexOf(character) > -1)
		{
			return true;
		}
		return false;
	}

	internal static string StrChr(string stringtosearch, char chartofind)
	{
		int num = stringtosearch.IndexOf(chartofind);
		if (num > -1)
		{
			return stringtosearch.Substring(num);
		}
		return null;
	}

	internal static string StrRChr(string stringtosearch, char chartofind)
	{
		int num = stringtosearch.LastIndexOf(chartofind);
		if (num > -1)
		{
			return stringtosearch.Substring(num);
		}
		return null;
	}

	internal static string StrStr(string stringtosearch, string stringtofind)
	{
		int num = stringtosearch.IndexOf(stringtofind);
		if (num > -1)
		{
			return stringtosearch.Substring(num);
		}
		return null;
	}

	internal static string StrTok(string stringtotokenize, string delimiters)
	{
		if (stringtotokenize != null)
		{
			activestring = stringtotokenize;
			activeposition = -1;
		}
		if (activestring == null)
		{
			return null;
		}
		if (activeposition == activestring.Length)
		{
			return null;
		}
		activeposition++;
		while (activeposition < activestring.Length && delimiters.IndexOf(activestring[activeposition]) > -1)
		{
			activeposition++;
		}
		if (activeposition == activestring.Length)
		{
			return null;
		}
		int num = activeposition;
		do
		{
			activeposition++;
		}
		while (activeposition < activestring.Length && delimiters.IndexOf(activestring[activeposition]) == -1);
		return activestring.Substring(num, activeposition - num);
	}
}
