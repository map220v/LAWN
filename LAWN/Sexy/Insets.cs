namespace Sexy
{
	public struct Insets
	{
		public int mLeft;

		public int mTop;

		public int mRight;

		public int mBottom;

		public Insets(int theLeft, int theTop, int theRight, int theBottom)
		{
			mLeft = theLeft;
			mTop = theTop;
			mRight = theRight;
			mBottom = theBottom;
		}

		public Insets(Insets theInsets)
		{
			mLeft = theInsets.mLeft;
			mTop = theInsets.mTop;
			mRight = theInsets.mRight;
			mBottom = theInsets.mBottom;
		}
	}
}
