using Sexy;

namespace Lawn
{
	internal class StoreScreenOverlay : Widget
	{
		public new StoreScreen mParent;

		public StoreScreenOverlay(StoreScreen theParent)
		{
			mParent = theParent;
			mMouseVisible = false;
			mHasAlpha = true;
		}

		public override void Draw(Graphics g)
		{
			mParent.DrawOverlay(g);
		}
	}
}
