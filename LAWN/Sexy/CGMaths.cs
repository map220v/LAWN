using Microsoft.Xna.Framework;

namespace Sexy
{
	internal static class CGMaths
	{
		public static CGPoint CGPointAddScaled(CGPoint augend, CGPoint addend, float factor)
		{
			CGPoint result = default(CGPoint);
			result.X = augend.X + addend.X * factor;
			result.Y = augend.Y + addend.Y * factor;
			return result;
		}

		internal static CGPoint CGPointMultiply(CGPoint multiplicand, CGPoint multiplier)
		{
			CGPoint result = default(CGPoint);
			result.X = multiplicand.X * multiplier.X;
			result.Y = multiplicand.Y * multiplier.Y;
			return result;
		}

		internal static CGPoint CGPointSubtract(CGPoint minuend, CGPoint subtrahend)
		{
			CGPoint result = default(CGPoint);
			result.X = minuend.X - subtrahend.X;
			result.Y = minuend.Y - subtrahend.Y;
			return result;
		}

		internal static CGPoint CGPointSubtract(TPoint minuend, TPoint subtrahend)
		{
			CGPoint result = default(CGPoint);
			result.X = minuend.x - subtrahend.x;
			result.Y = minuend.y - subtrahend.y;
			return result;
		}

		internal static float CGVectorNorm(CGPoint v)
		{
			return v.X * v.X + v.Y * v.Y;
		}

		internal static CGPoint CGPointMake(float x, float y)
		{
			return new CGPoint(x, y);
		}

		internal static void CGPointTranslate(ref CGPoint point, float tx, float ty)
		{
			point.X += tx;
			point.Y += ty;
		}

		internal static void CGPointTranslate(ref CGPoint point, int tx, int ty)
		{
			point.X += tx;
			point.Y += ty;
		}

		internal static void CGPointTranslate(ref Point point, int tx, int ty)
		{
			point.X += tx;
			point.Y += ty;
		}

		internal static void CGPointTranslate(ref Point point, float tx, float ty)
		{
			point.X += (int)tx;
			point.Y += (int)ty;
		}
	}
}
