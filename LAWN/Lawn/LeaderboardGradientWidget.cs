using Sexy;
using System;

namespace Lawn
{
	internal class LeaderboardGradientWidget : Widget
	{
		public LeaderboardGradientWidget()
		{
			mDisabled = true;
		}

		public override void Draw(Graphics g)
		{
			base.Draw(g);
			g.DrawImage(AtlasResources.IMAGE_PILE_LEADERBOARDSCREEN_GRADIENT, 0, 0);
			g.DrawImageRotated(AtlasResources.IMAGE_PILE_LEADERBOARDSCREEN_GRADIENT, 0, mHeight - AtlasResources.IMAGE_PILE_LEADERBOARDSCREEN_GRADIENT.mHeight, Math.PI, AtlasResources.IMAGE_PILE_LEADERBOARDSCREEN_GRADIENT.mWidth / 2, AtlasResources.IMAGE_PILE_LEADERBOARDSCREEN_GRADIENT.mHeight / 2);
		}
	}
}
