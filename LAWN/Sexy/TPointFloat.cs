using Microsoft.Xna.Framework;

namespace Sexy
{
	public struct TPointFloat
	{
		private Vector2 mPoint;

		public float mX
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

		public float x
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

		public float mY
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

		public float y
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

		public TPointFloat(float theX, float theY)
		{
			mPoint = new Vector2(theX, theY);
		}

		public TPointFloat(TPointFloat theTPoint)
		{
			mPoint = theTPoint.mPoint;
		}

		public static bool operator ==(TPointFloat a, TPointFloat b)
		{
			if (a.mPoint.X == b.mPoint.X)
			{
				return a.mPoint.Y == b.mPoint.Y;
			}
			return false;
		}

		public static bool operator !=(TPointFloat a, TPointFloat b)
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
			if (mX == (float)tPoint.mX)
			{
				return mY == (float)tPoint.mY;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return mPoint.GetHashCode();
		}

		public static TPointFloat operator +(TPointFloat a, TPointFloat b)
		{
			return new TPointFloat(a.mPoint.X + b.mPoint.X, a.mPoint.Y + b.mPoint.Y);
		}

		public static TPointFloat operator -(TPointFloat a, TPointFloat b)
		{
			return new TPointFloat(a.mPoint.X - b.mPoint.X, a.mPoint.Y - b.mPoint.Y);
		}

		public static TPointFloat operator *(TPointFloat a, TPointFloat b)
		{
			return new TPointFloat(a.mPoint.X * b.mPoint.X, a.mPoint.Y * b.mPoint.Y);
		}

		public static TPointFloat operator /(TPointFloat a, TPointFloat b)
		{
			return new TPointFloat(a.mPoint.X / b.mPoint.X, a.mPoint.Y / b.mPoint.Y);
		}

		public static TPointFloat operator *(TPointFloat a, int s)
		{
			return new TPointFloat(a.mPoint.X * (float)s, a.mPoint.Y * (float)s);
		}

		public static TPointFloat operator /(TPointFloat a, int s)
		{
			return new TPointFloat(a.mPoint.X / (float)s, a.mPoint.Y / (float)s);
		}

		public override string ToString()
		{
			return mPoint.ToString();
		}
	}
}
