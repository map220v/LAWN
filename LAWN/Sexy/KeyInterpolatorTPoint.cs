using System.Collections.Generic;

namespace Sexy
{
	internal class KeyInterpolatorTPoint : KeyInterpolator<TPoint>
	{
		private static Stack<KeyInterpolatorTPoint> unusedObjects = new Stack<KeyInterpolatorTPoint>();

		protected override TPoint GetLerpResult(float f, TPoint a, TPoint b)
		{
			return InterpolationMethodTPoint(f, a, b);
		}

		public static TPoint InterpolationMethodTPoint(float f, TPoint a, TPoint b)
		{
			return GlobalMembersInterpolator.tlerp(f, a, b);
		}

		public static KeyInterpolatorTPoint GetNewKeyInterpolatorTPoint()
		{
			if (unusedObjects.Count > 0)
			{
				return unusedObjects.Pop();
			}
			return new KeyInterpolatorTPoint();
		}

		private KeyInterpolatorTPoint()
		{
		}

		public override void PrepareForReuse()
		{
			Reset();
			unusedObjects.Push(this);
		}

		public override TPoint Tick(float tick)
		{
			bool flag = !mEaseFuncSet;
			int num = mKeys.GetKeyAfter(mKey);
			bool flag2 = false;
			while (!flag2 && tick >= (float)num)
			{
				mKey = num;
				int keyAfter = mKeys.GetKeyAfter(num);
				if (num == keyAfter)
				{
					flag2 = true;
					num++;
				}
				else
				{
					num = keyAfter;
				}
				flag = true;
			}
			int firstKey = mKeys.GetFirstKey();
			while (mKey != firstKey && tick < (float)mKey)
			{
				num = mKey;
				mKey = mKeys.GetKeyBefore(mKey);
				flag = true;
			}
			if (num == mKeys.GetLastKey() + 1)
			{
				return mKeys[mKey].value;
			}
			if (tick < (float)mKey)
			{
				return mKeys[mKey].value;
			}
			if (flag)
			{
				SetupEaseFunc(mKeys[mKey], mKeys[num]);
			}
			if (mKeys[num].tween)
			{
				float num2 = (float)num - (float)mKey;
				float num3 = tick - (float)mKey;
				float t = num3 / num2;
				float f = mEaseFunc.Tick(t);
				return GetLerpResult(f, mKeys[mKey].value, mKeys[num].value);
			}
			return mKeys[mKey].value;
		}
	}
}
