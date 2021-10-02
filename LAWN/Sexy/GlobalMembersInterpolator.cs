namespace Sexy
{
	internal static class GlobalMembersInterpolator
	{
		internal const bool kTween = true;

		internal const bool kNoTween = false;

		internal const bool kEase = true;

		internal const bool kNoEase = false;

		public static float tlerp(float t, float a, float b)
		{
			return a + t * (b - a);
		}

		public static TPoint tlerp(float t, TPoint a, TPoint b)
		{
			return new TPoint(tlerp(t, a.mX, b.mX), tlerp(t, a.mY, b.mY));
		}

		public static SexyColor tlerp(float t, SexyColor a, SexyColor b)
		{
			return new SexyColor(tlerp(t, a.mRed, b.mRed), tlerp(t, a.mGreen, b.mGreen), tlerp(t, a.mBlue, b.mBlue), tlerp(t, a.mAlpha, b.mAlpha));
		}

		public static int tlerp(float t, int a, int b)
		{
			return (int)((float)a + t * (float)(b - a));
		}
	}
}
