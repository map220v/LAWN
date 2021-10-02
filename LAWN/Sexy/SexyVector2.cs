using Microsoft.Xna.Framework;
using System;

namespace Sexy
{
	public struct SexyVector2
	{
		public Vector2 mVector;

		public float x
		{
			get
			{
				return mVector.X;
			}
			set
			{
				mVector.X = value;
			}
		}

		public float y
		{
			get
			{
				return mVector.Y;
			}
			set
			{
				mVector.Y = value;
			}
		}

		public SexyVector2(float theX, float theY)
		{
			mVector = new Vector2(theX, theY);
		}

		public SexyVector2(Vector2 theVector)
		{
			mVector = theVector;
		}

		public float Dot(SexyVector2 v)
		{
			return Vector2.Dot(mVector, v.mVector);
		}

		public static SexyVector2 operator +(SexyVector2 lhs, SexyVector2 rhs)
		{
			return new SexyVector2(lhs.mVector + rhs.mVector);
		}

		public static SexyVector2 operator -(SexyVector2 lhs, SexyVector2 rhs)
		{
			return new SexyVector2(lhs.mVector - rhs.mVector);
		}

		public static SexyVector2 operator -(SexyVector2 rhs)
		{
			return new SexyVector2(0f - rhs.x, 0f - rhs.y);
		}

		public static SexyVector2 operator *(float t, SexyVector2 rhs)
		{
			return new SexyVector2(t * rhs.x, t * rhs.y);
		}

		public static SexyVector2 operator /(float t, SexyVector2 rhs)
		{
			return new SexyVector2(rhs.x / t, rhs.y / t);
		}

		public static bool operator ==(SexyVector2 lhs, SexyVector2 rhs)
		{
			if (lhs.x == rhs.x)
			{
				return lhs.y == rhs.y;
			}
			return false;
		}

		public static bool operator !=(SexyVector2 lhs, SexyVector2 rhs)
		{
			return !(lhs == rhs);
		}

		public float Magnitude()
		{
			return (float)Math.Sqrt(x * x + y * y);
		}

		public float MagnitudeSquared()
		{
			return x * x + y * y;
		}

		public SexyVector2 Normalize()
		{
			mVector.Normalize();
			return this;
		}

		public override string ToString()
		{
			return mVector.ToString();
		}

		public SexyVector2 Perp()
		{
			return new SexyVector2(0f - y, x);
		}

		public override int GetHashCode()
		{
			return mVector.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return mVector.Equals(obj);
		}
	}
}
