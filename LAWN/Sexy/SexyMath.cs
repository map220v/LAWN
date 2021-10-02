using System;

namespace Sexy
{
	internal class SexyMath
	{
		public static float Fabs(float inX)
		{
			return Math.Abs(inX);
		}

		public static double Fabs(double inX)
		{
			return Math.Abs(inX);
		}

		public static float DegToRad(float inX)
		{
			return inX * (float)Math.PI / 180f;
		}

		public static float RadToDeg(float inX)
		{
			return inX * 180f / (float)Math.PI;
		}

		public static bool IsPowerOfTwo(uint inX)
		{
			if (inX != 0)
			{
				return (inX & (inX - 1)) == 0;
			}
			return false;
		}
	}
}
