using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Sexy
{
	internal class MemoryImage : Image, IDisposable
	{
		private static uint nextId;

		private RenderTarget2D renderTarget;

		private uint mId;

		public RenderTarget2D RenderTarget
		{
			get
			{
				return renderTarget;
			}
		}

		public MemoryImage()
		{
			renderTarget = null;
			mId = 0u;
		}

		public void Clear()
		{
			GlobalStaticVars.g.Clear(new Color(0, 0, 0, 0));
		}

		public override void Dispose()
		{
			if (renderTarget != null)
			{
				renderTarget.Dispose();
			}
			base.Dispose();
		}

		public void Create(int theWidth, int theHeight)
		{
			Create(theWidth, theHeight, PixelFormat.kPixelFormat_RGBA8888);
		}

		public void Create(int theWidth, int theHeight, PixelFormat thePixelFormat)
		{
			mId = nextId++;
			mParentWidth = (mWidth = theWidth);
			mParentHeight = (mHeight = theHeight);
			int closestPowerOf2Above = GraphicsState.GetClosestPowerOf2Above(mWidth);
			int closestPowerOf2Above2 = GraphicsState.GetClosestPowerOf2Above(mHeight);
			closestPowerOf2Above = Math.Max(16, closestPowerOf2Above);
			closestPowerOf2Above2 = Math.Max(16, closestPowerOf2Above2);
			mOwnsTexture = true;
			mMaxS = (float)mWidth / (float)closestPowerOf2Above;
			mMaxT = (float)mHeight / (float)closestPowerOf2Above2;
			renderTarget = new RenderTarget2D(GlobalStaticVars.g.GraphicsDevice, closestPowerOf2Above, closestPowerOf2Above2);
			Texture = renderTarget;
		}
	}
}
