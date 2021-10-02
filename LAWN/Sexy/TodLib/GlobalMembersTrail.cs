namespace Sexy.TodLib
{
	internal static class GlobalMembersTrail
	{
		public static int gTrailDefCount;

		public static TrailDefinition[] gTrailDefArray;

		public static int gTrailParamArraySize;

		public static TrailParams[] gTrailParamArray;

		public static void TrailLoadDefinitions(TrailParams[] theTrailParamArray, int theTrailParamArraySize)
		{
			gTrailParamArraySize = theTrailParamArraySize;
			gTrailParamArray = theTrailParamArray;
			gTrailDefCount = theTrailParamArraySize;
			gTrailDefArray = new TrailDefinition[gTrailDefCount];
			for (int i = 0; i < gTrailParamArraySize; i++)
			{
				TrailParams trailParams = theTrailParamArray[i];
				TrailDefinition theTrailDef = gTrailDefArray[i];
				if (!TrailLoadADef(ref theTrailDef, trailParams.mTrailFileName))
				{
					new string(new char[256]);
					string.Format("Failed to load trail '{0}'", trailParams.mTrailFileName);
				}
			}
		}

		public static void TrailFreeDefinitions()
		{
			gTrailDefArray = null;
			gTrailDefArray = null;
			gTrailDefCount = 0;
			gTrailParamArray = null;
			gTrailParamArraySize = 0;
		}

		public static bool TrailLoadADef(ref TrailDefinition theTrailDef, string theTrailFileName)
		{
			if (!GlobalStaticVars.gSexyAppBase.mResourceManager.LoadTrail(theTrailFileName, ref theTrailDef))
			{
				return false;
			}
			Definition.FloatTrackSetDefault(ref theTrailDef.mWidthOverLength, 1f);
			Definition.FloatTrackSetDefault(ref theTrailDef.mWidthOverTime, 1f);
			Definition.FloatTrackSetDefault(ref theTrailDef.mTrailDuration, 100f);
			Definition.FloatTrackSetDefault(ref theTrailDef.mAlphaOverLength, 1f);
			Definition.FloatTrackSetDefault(ref theTrailDef.mAlphaOverTime, 1f);
			return true;
		}
	}
}
