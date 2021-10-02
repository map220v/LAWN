using Microsoft.Xna.Framework;

namespace Sexy
{
	public struct TPoint
	{
		private Point mPoint;

		public Point Point
		{
			get
			{
				return mPoint;
			}
			set
			{
				mPoint = value;
			}
		}

		public int mX
		{
			get
			{
				return mPoint.X;
			}
			set
			{
				mPoint.X = value;
			}
		}

		public int x
		{
			get
			{
				return mPoint.X;
			}
			set
			{
				mPoint.X = value;
			}
		}

		public int mY
		{
			get
			{
				return mPoint.Y;
			}
			set
			{
				mPoint.Y = value;
			}
		}

		public int y
		{
			get
			{
				return mPoint.Y;
			}
			set
			{
				mPoint.Y = value;
			}
		}

		public TPoint(int theX, int theY)
		{
			mPoint = new Point(theX, theY);
		}

		public TPoint(TPoint theTPoint)
		{
			mPoint = theTPoint.mPoint;
		}

		public static bool operator ==(TPoint a, TPoint b)
		{
			if (a.mPoint.X == b.mPoint.X)
			{
				return a.mPoint.Y == b.mPoint.Y;
			}
			return false;
		}

		public static bool operator !=(TPoint a, TPoint b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}
			TPoint tPoint = (TPoint)obj;
			if (mX == tPoint.mX)
			{
				return mY == tPoint.mY;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return mPoint.GetHashCode();
		}

		public static TPoint operator +(TPoint a, TPoint b)
		{
			return new TPoint(a.mPoint.X + b.mPoint.X, a.mPoint.Y + b.mPoint.Y);
		}

		public static TPoint operator -(TPoint a, TPoint b)
		{
			return new TPoint(a.mPoint.X - b.mPoint.X, a.mPoint.Y - b.mPoint.Y);
		}

		public static TPoint operator *(TPoint a, TPoint b)
		{
			return new TPoint(a.mPoint.X * b.mPoint.X, a.mPoint.Y * b.mPoint.Y);
		}

		public static TPoint operator /(TPoint a, TPoint b)
		{
			return new TPoint(a.mPoint.X / b.mPoint.X, a.mPoint.Y / b.mPoint.Y);
		}

		public static TPoint operator *(TPoint a, int s)
		{
			return new TPoint(a.mPoint.X * s, a.mPoint.Y * s);
		}

		public static TPoint operator /(TPoint a, int s)
		{
			return new TPoint(a.mPoint.X / s, a.mPoint.Y / s);
		}

		public static explicit operator TPoint(Point point)
		{
			TPoint result = default(TPoint);
			result.Point = point;
			return result;
		}

		public static implicit operator Point(TPoint aPoint)
		{
			return aPoint.Point;
		}

		public override string ToString()
		{
			return mPoint.ToString();
		}
	}
}
