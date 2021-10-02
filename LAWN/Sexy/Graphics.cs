using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sexy
{
	internal class Graphics : GraphicsState
	{
		public enum DrawMode
		{
			DRAWMODE_NORMAL,
			DRAWMODE_ADDITIVE
		}

		private static BlendState additiveState = BlendState.AlphaBlend;

		public static readonly SamplerState WrapSamplerState = new SamplerState
		{
			AddressU = TextureAddressMode.Wrap,
			AddressV = TextureAddressMode.Wrap
		};

		private static readonly SamplerState NormalSamplerState = new SamplerState
		{
			AddressU = TextureAddressMode.Clamp,
			AddressV = TextureAddressMode.Clamp,
			AddressW = TextureAddressMode.Clamp,
			Filter = TextureFilter.Linear
		};

		private static bool hardwareClippingEnabled;

		private static TRect hardwareClippedRectangle;

		private static Stack<Graphics> unusedObjects = new Stack<Graphics>(20);

		public static SpriteBatch gSpriteBatch;

		public static PrimitiveBatch gPrimitiveBatch;

		public RenderTarget2D mDestImage;

		public Game mGame;

		protected static Stack<SexyTransform2D> gTransformStack = new Stack<SexyTransform2D>();

		internal static SpriteBatch spriteBatch;

		protected static PrimitiveBatch primitiveBatch;

		public static Stack<Graphics> mStateStack = new Stack<Graphics>();

		private static BasicEffect quadEffect;

		private bool add;

		private bool polyFillBegun;

		private static Image temp = new Image();

		private TriVertex[,] tempTriangles = new TriVertex[1, 3];

		private Image triangleBatchTexture;

		private static RasterizerState hardwareClipState = new RasterizerState
		{
			ScissorTestEnable = true,
			CullMode = CullMode.None
		};

		protected DrawMode currentlyActiveDrawMode
		{
			get
			{
				if (GraphicsDevice.BlendState != BlendState.AlphaBlend)
				{
					return DrawMode.DRAWMODE_ADDITIVE;
				}
				return DrawMode.DRAWMODE_NORMAL;
			}
			set
			{
				GraphicsDevice.BlendState = ((value == DrawMode.DRAWMODE_NORMAL) ? BlendState.AlphaBlend : additiveState);
			}
		}

		protected static bool spritebatchBegan
		{
			get;
			private set;
		}

		public GraphicsDevice GraphicsDevice
		{
			get
			{
				return GraphicsState.mGraphicsDeviceManager.GraphicsDevice;
			}
		}

		public GraphicsDeviceManager GraphicsDeviceManager
		{
			get
			{
				return GraphicsState.mGraphicsDeviceManager;
			}
		}

		public int PreferredBackBufferWidth
		{
			get
			{
				return GraphicsState.mGraphicsDeviceManager.PreferredBackBufferWidth;
			}
			set
			{
				GraphicsState.mGraphicsDeviceManager.PreferredBackBufferWidth = value;
			}
		}

		public int PreferredBackBufferHeight
		{
			get
			{
				return GraphicsState.mGraphicsDeviceManager.PreferredBackBufferHeight;
			}
			set
			{
				GraphicsState.mGraphicsDeviceManager.PreferredBackBufferHeight = value;
			}
		}

		public bool IsFullScreen
		{
			get
			{
				return GraphicsState.mGraphicsDeviceManager.IsFullScreen;
			}
			set
			{
				GraphicsState.mGraphicsDeviceManager.IsFullScreen = value;
			}
		}

		private Vector2 scale
		{
			get
			{
				return new Vector2(mScaleX, mScaleY);
			}
		}

		public static void PreAllocateMemory()
		{
			for (int i = 0; i < 10; i++)
			{
				new Graphics().PrepareForReuse();
			}
		}

		public bool IsHardWareClipping()
		{
			return hardwareClippingEnabled;
		}

		public static Graphics GetNew()
		{
			if (unusedObjects.Count > 0)
			{
				Graphics graphics = unusedObjects.Pop();
				graphics.Reset();
				return graphics;
			}
			return new Graphics();
		}

		public static Graphics GetNew(Graphics theGraphics)
		{
			if (unusedObjects.Count > 0)
			{
				Graphics graphics = unusedObjects.Pop();
				graphics.Reset();
				graphics.CopyStateFrom(theGraphics);
				return graphics;
			}
			return new Graphics(theGraphics);
		}

		public static Graphics GetNew(Game theGame)
		{
			if (unusedObjects.Count > 0)
			{
				Graphics graphics = unusedObjects.Pop();
				graphics.Reset();
				graphics.mGame = theGame;
				GraphicsState.mGraphicsDeviceManager = new GraphicsDeviceManager(theGame);
				return graphics;
			}
			return new Graphics(theGame);
		}

		public static Graphics GetNew(MemoryImage theDestImage)
		{
			if (unusedObjects.Count > 0)
			{
				Graphics graphics = unusedObjects.Pop();
				graphics.Reset();
				graphics.mDestImage = theDestImage.RenderTarget;
				graphics.mClipRect = new TRect(0, 0, graphics.mDestImage.Width, graphics.mDestImage.Height);
				graphics.SetRenderTarget(graphics.mDestImage);
				return graphics;
			}
			return new Graphics(theDestImage);
		}

		public void PrepareForReuse()
		{
			unusedObjects.Push(this);
		}

		private void ResetForReuse()
		{
			mTransX = 0;
			mTransY = 0;
			mFastStretch = false;
			mWriteColoredString = false;
			mLinearBlend = false;
			mScaleX = 1f;
			mScaleY = 1f;
			mScaleOrigX = 0f;
			mScaleOrigY = 0f;
			mFont = null;
			base.mColor = default(SexyColor);
			mColorizeImages = false;
			WorldRotation = 0f;
			mDrawMode = DrawMode.DRAWMODE_NORMAL;
			mClipRect = new TRect(0, 0, base.mScreenWidth, base.mScreenHeight);
		}

		private Graphics()
		{
			spriteBatch = gSpriteBatch;
			primitiveBatch = gPrimitiveBatch;
		}

		private Graphics(Graphics theGraphics)
		{
			CopyStateFrom(theGraphics);
			spriteBatch = gSpriteBatch;
			primitiveBatch = gPrimitiveBatch;
		}

		private Graphics(Game theGame)
			: base(theGame)
		{
			mGame = theGame;
			PreAllocateMemory();
		}

		private Graphics(MemoryImage theDestImage)
		{
			mDestImage = theDestImage.RenderTarget;
			mClipRect = new TRect(0, 0, mDestImage.Width, mDestImage.Height);
			spriteBatch = gSpriteBatch;
			primitiveBatch = gPrimitiveBatch;
			SetRenderTarget(mDestImage);
		}

		public new void Init()
		{
			gSpriteBatch = new SpriteBatch(GraphicsState.mGraphicsDeviceManager.GraphicsDevice);
			gPrimitiveBatch = new PrimitiveBatch(GraphicsState.mGraphicsDeviceManager.GraphicsDevice);
			spriteBatch = gSpriteBatch;
			primitiveBatch = gPrimitiveBatch;
			quadEffect = new BasicEffect(GraphicsState.mGraphicsDeviceManager.GraphicsDevice);
			quadEffect.LightingEnabled = false;
		}

		public virtual void Dispose()
		{
			RenderTarget2D mDestImage2 = mDestImage;
		}

		public void BeginFrame()
		{
			BeginFrame(SpriteSortMode.Deferred);
		}

		public void BeginFrame(SpriteSortMode sortmode)
		{
			BeginFrame(hardwareClippingEnabled ? hardwareClipState : null, sortmode);
		}

		public void BeginFrame(RasterizerState rasterState)
		{
			BeginFrame(rasterState, SpriteSortMode.Deferred);
		}

		public void BeginFrame(RasterizerState rasterState, SpriteSortMode sortmode)
		{
			if (NeedToSetWorldRotation)
			{
				ApplyWorldRotation();
				NeedToSetWorldRotation = false;
			}
			BlendState blendState = (mDrawMode == DrawMode.DRAWMODE_ADDITIVE) ? additiveState : BlendState.AlphaBlend;
			GraphicsDevice.BlendState = blendState;
			if (gTransformStack.empty())
			{
				spriteBatch.Begin(sortmode, blendState, NormalSamplerState, null, rasterState);
			}
			else
			{
				spriteBatch.Begin(sortmode, blendState, NormalSamplerState, null, rasterState, null, gTransformStack.Peek().mMatrix);
			}
			spritebatchBegan = true;
		}

		public void EndFrame()
		{
			EndDrawImageTransformed();
			spriteBatch.End();
			spritebatchBegan = false;
		}

		public static void OrientationChanged()
		{
			primitiveBatch.SetupMatrices();
		}

		protected void SetupDrawMode(DrawMode theDrawingMode)
		{
			add = (theDrawingMode == DrawMode.DRAWMODE_ADDITIVE);
			if (spritebatchBegan)
			{
				if (theDrawingMode != currentlyActiveDrawMode)
				{
					currentlyActiveDrawMode = (mDrawMode = theDrawingMode);
					EndFrame();
					BeginFrame();
				}
			}
			else if (primitiveBatch.HasBegun)
			{
				//DrawMode currentlyActiveDrawMode = currentlyActiveDrawMode;
				if (hardwareClippingEnabled)
				{
					GraphicsDevice.RasterizerState = hardwareClipState;
				}
			}
			else
			{
				if (theDrawingMode == DrawMode.DRAWMODE_ADDITIVE)
				{
					GraphicsDevice.BlendState = additiveState;
				}
				else
				{
					GraphicsDevice.BlendState = BlendState.AlphaBlend;
				}
				if (hardwareClippingEnabled)
				{
					GraphicsDevice.RasterizerState = hardwareClipState;
				}
			}
			DrawMode drawMode2 = mDrawMode = (currentlyActiveDrawMode = theDrawingMode);
		}

		public void SetRenderTarget(RenderTarget2D renderTarget)
		{
			bool spritebatchBegan = Graphics.spritebatchBegan;
			if (Graphics.spritebatchBegan)
			{
				EndFrame();
			}
			if (renderTarget == null && gTransformStack.Count > 0)
			{
				gTransformStack.Pop();
			}
			else if (gTransformStack.Count > 0)
			{
				gTransformStack.Push(new SexyTransform2D(Matrix.Identity));
			}
			mDestImage = renderTarget;
			GraphicsDevice.SetRenderTarget(mDestImage);
			ClearClipRect();
			if (spritebatchBegan)
			{
				BeginFrame();
			}
		}

		public void Clear()
		{
			Clear(Color.Black);
		}

		public void Clear(Color color)
		{
			GraphicsDevice.Clear(color);
		}

		public Graphics Create()
		{
			return new Graphics(this);
		}

		public void SetFont(Font theFont)
		{
			mFont = theFont;
			if (mFont != null)
			{
				mFont.mScaleX = mScaleX;
				mFont.mScaleY = mScaleY;
			}
		}

		public Font GetFont()
		{
			return mFont;
		}

		public static void PremultiplyColour(ref Color c)
		{
			float num = (float)(int)c.A / 255f;
			c *= num;
		}

		public static void PremultiplyColour(ref SexyColor c)
		{
			float num = (float)c.mAlpha / 255f;
			c.mRed = (int)((float)c.mRed * num);
			c.mGreen = (int)((float)c.mGreen * num);
			c.mBlue = (int)((float)c.mBlue * num);
		}

		public void SetColor(Color theColor)
		{
			SetColor(theColor, true);
		}

		public void SetColor(Color theColor, bool premultiply)
		{
			if (mDrawMode == DrawMode.DRAWMODE_NORMAL)
			{
				if (premultiply)
				{
					PremultiplyColour(ref theColor);
				}
			}
			else
			{
				theColor.A = 0;
			}
			base.mColor = theColor;
		}

		public Color GetColor()
		{
			return base.mColor;
		}

		public void SetDrawMode(DrawMode theDrawMode)
		{
			SetupDrawMode(theDrawMode);
		}

		public void SetDrawMode(int theDrawMode)
		{
			SetupDrawMode((DrawMode)theDrawMode);
		}

		public DrawMode GetDrawMode()
		{
			return mDrawMode;
		}

		public void SetColorizeImages(bool colorizeImages)
		{
			mColorizeImages = colorizeImages;
		}

		public bool GetColorizeImages()
		{
			return mColorizeImages;
		}

		public void SetScaleX(float scaleX)
		{
			mScaleX = scaleX;
			if (mFont != null)
			{
				mFont.mScaleX = mScaleX;
			}
			mScaleOrigX = 0.5f;
		}

		public void SetScaleY(float scaleY)
		{
			mScaleY = scaleY;
			if (mFont != null)
			{
				mFont.mScaleY = scaleY;
			}
			mScaleOrigY = 0.5f;
		}

		public void SetScale(float scale)
		{
			SetScale(scale, scale);
		}

		public void SetScale(float scaleX, float scaleY)
		{
			SetScaleX(scaleX);
			SetScaleY(scaleY);
		}

		public void SetScale(float theScaleX, float theScaleY, float theOrigX, float theOrigY)
		{
			mScaleX = theScaleX;
			mScaleY = theScaleY;
			mScaleOrigX = theOrigX;
			mScaleOrigY = theOrigY;
		}

		public void SetFastStretch(bool fastStretch)
		{
			mFastStretch = fastStretch;
		}

		public bool GetFastStretch()
		{
			return mFastStretch;
		}

		public void SetLinearBlend(bool linear)
		{
			mLinearBlend = linear;
		}

		public bool GetLinearBlend()
		{
			return mLinearBlend;
		}

		public void FillRect(TRect theRect)
		{
			FillRect(theRect.mX, theRect.mY, theRect.mWidth, theRect.mHeight);
		}

		public void FillRect(int theX, int theY, int theWidth, int theHeight)
		{
			bool mColorizeImages = base.mColorizeImages;
			SetColorizeImages(true);
			DrawImage(GraphicsState.dummy, theX, theY, theWidth, theHeight);
			SetColorizeImages(mColorizeImages);
		}

		public void DrawRect(TRect theRect)
		{
			DrawRect(theRect.mX, theRect.mY, theRect.mWidth, theRect.mHeight);
		}

		public void DrawRect(int theX, int theY, int theWidth, int theHeight)
		{
			if (base.mColor.A != 0)
			{
				FillRect(theX, theY, theWidth + 1, 1);
				FillRect(theX, theY + theHeight, theWidth + 1, 1);
				FillRect(theX, theY + 1, 1, theHeight - 1);
				FillRect(theX + theWidth, theY + 1, 1, theHeight - 1);
			}
		}

		public void DrawStringLayer(string theString, int theX, int theY, int theLayer)
		{
			EndDrawImageTransformed();
			if (mFont != null)
			{
				mFont.DrawStringLayer(this, theX + mTransX, theY + mTransY, theString, mColorizeImages ? base.mColor : Color.White, theLayer);
			}
		}

		public void DrawStringLayer(string theString, int theX, int theY, int theLayer, int maxWidth)
		{
			float num = 1f;
			float num2 = mFont.StringWidth(theString);
			if (num2 > (float)maxWidth)
			{
				num = (float)maxWidth / num2;
			}
			SetScale(num);
			DrawStringLayer(theString, theX, theY - (int)((num - 1f) * (float)mFont.GetHeight() / 2f), theLayer);
			SetScale(1f);
		}

		public void DrawString(string theString, int theX, int theY)
		{
			if (mFont != null)
			{
				mFont.DrawString(this, theX + mTransX, theY + mTransY, theString, base.mColor);
			}
		}

		public void DrawString(StringBuilder theString, int theX, int theY)
		{
			if (mFont != null)
			{
				mFont.DrawString(this, theX + mTransX, theY + mTransY, theString, mColorizeImages ? base.mColor : Color.White);
			}
		}

		public void DrawLine(int theStartX, int theStartY, int theEndX, int theEndY)
		{
		}

		public void BeginPolyFill()
		{
			EndFrame();
			Matrix? transform = null;
			if (gTransformStack.Count > 0)
			{
				transform = gTransformStack.Peek().mMatrix;
			}
			primitiveBatch.Begin(PrimitiveType.TriangleList, mTransX, mTransY, transform, null, NormalSamplerState);
			polyFillBegun = true;
		}

		public void EndPolyFill()
		{
			primitiveBatch.End();
			BeginFrame();
			polyFillBegun = false;
		}

		public void PolyFill(TPoint[] theVertexList, int theNumVertices)
		{
			for (int i = 0; i < theNumVertices; i++)
			{
				Vector2 vertex = new Vector2(theVertexList[i].mX + mTransX, theVertexList[i].mY + mTransY);
				primitiveBatch.AddVertex(vertex, base.mColor);
			}
		}

		private void PrepareRectsForClipping(ref TRect source, ref TRect destination)
		{
			destination.mX += (int)(mScaleOrigX * (float)destination.mWidth * ((1f - mScaleX) / 2f));
			destination.mY += (int)(mScaleOrigY * (float)destination.mHeight * ((1f - mScaleY) / 2f));
			destination.mWidth = (int)((float)destination.mWidth * mScaleX);
			destination.mHeight = (int)((float)destination.mHeight * mScaleY);
			Vector2 vector = new Vector2((float)destination.mWidth / (float)((source.mWidth != 0) ? source.mWidth : destination.mWidth), (float)destination.mHeight / (float)((source.mHeight != 0) ? source.mHeight : destination.mHeight));
			if (vector.X == 0f)
			{
				vector.X = 1f;
			}
			if (vector.Y == 0f)
			{
				vector.Y = 1f;
			}
			int num = Math.Max(0, (int)((float)(mClipRect.mX - destination.mX) / vector.X));
			int num2 = Math.Max(0, (int)((float)(mClipRect.mY - destination.mY) / vector.Y));
			source.mX += num;
			source.mY += num2;
			source.mWidth += -num + Math.Min(0, (int)((float)(mClipRect.mX + mClipRect.mWidth - (destination.mX + destination.mWidth)) / vector.X));
			source.mHeight += -num2 + Math.Min(0, (int)((float)(mClipRect.mY + mClipRect.mHeight - (destination.mY + destination.mHeight)) / vector.Y));
			destination = mClipRect.Intersection(destination);
		}

		private static void GetTransform(out Matrix? transform)
		{
			if (gTransformStack.Count > 0)
			{
				transform = gTransformStack.Peek().mMatrix;
			}
			else
			{
				transform = null;
			}
		}

		public void BeginPrimitiveBatch(Image texture)
		{
			if (!primitiveBatch.HasBegun)
			{
				EndFrame();
				Matrix? transform;
				GetTransform(out transform);
				primitiveBatch.Begin(PrimitiveType.TriangleList, mTransX, mTransY, transform, texture, NormalSamplerState);
			}
			else
			{
				Matrix? transform2;
				GetTransform(out transform2);
				if (transform2.HasValue)
				{
					primitiveBatch.Transform = transform2.Value;
				}
				else
				{
					primitiveBatch.Transform = Matrix.Identity;
				}
				primitiveBatch.Texture = texture;
				primitiveBatch.OffsetX = mTransX;
				primitiveBatch.OffsetY = mTransY;
			}
			SetDrawMode(mDrawMode);
		}

		public void EndDrawImageTransformed()
		{
			EndDrawImageTransformed(true);
		}

		public void EndDrawImageTransformed(bool startSpritebatch)
		{
			if (primitiveBatch.HasBegun)
			{
				primitiveBatch.End();
				if (startSpritebatch)
				{
					BeginFrame();
				}
			}
			else if (!spritebatchBegan && startSpritebatch)
			{
				BeginFrame();
			}
		}

		public void DrawImageWithBasicEffect(Image theImage, VertexPositionTexture[] verts, short[] indices, Matrix world, Matrix view, Matrix projection)
		{
			if (primitiveBatch.HasBegun)
			{
				primitiveBatch.End();
			}
			quadEffect.World = world;
			quadEffect.View = view;
			quadEffect.Projection = projection;
			quadEffect.TextureEnabled = true;
			quadEffect.Texture = theImage.Texture;
			foreach (EffectPass pass in quadEffect.CurrentTechnique.Passes)
			{
				pass.Apply();
				GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, verts, 0, 4, indices, 0, 2);
			}
		}

		public void DrawImageTransformed(Image theImage, ref Matrix theTransform, bool center, Color theColor, TRect theSrcRect, bool clip)
		{
			BeginPrimitiveBatch(theImage);
			TRect destination = new TRect(-mTransX, -mTransY, theImage.GetCelWidth(), theImage.GetCelHeight());
			Vector2 center2 = center ? new Vector2((float)theSrcRect.mWidth * 0.5f, (float)theSrcRect.mHeight * 0.5f) : Vector2.Zero;
			if (add)
			{
				theColor.A = 0;
			}
			primitiveBatch.Draw(theImage, destination, theSrcRect, ref theTransform, center2, theColor, false, true);
		}

		public void DrawImageRotatedScaled(Image theImage, TRect dest, TRect src, Color col, float rotation, Vector2 scale, Vector2 origin)
		{
			BeginPrimitiveBatch(theImage);
			primitiveBatch.DrawRotatedScaled(theImage, dest, src, origin, rotation, scale, col, false, false, PrimitiveBatchEffects.None);
		}

		public void DrawImage(Image theImage, float theX, float theY)
		{
			DrawImage(theImage, (int)theX, (int)theY);
		}

		public void DrawImage(Image theImage, int theX, int theY)
		{
			BeginPrimitiveBatch(theImage);
			TRect source = new TRect(theImage.mS, theImage.mT, theImage.mWidth, theImage.mHeight);
			TRect destination = new TRect(theX + mTransX, theY + mTransY, source.mWidth, source.mHeight);
			if (source.mWidth > 0 && source.mHeight > 0)
			{
				PrepareRectsForClipping(ref source, ref destination);
				primitiveBatch.Draw(theImage, destination, source, mColorizeImages ? base.mColor : Color.White, true, false);
			}
		}

		public void DrawImage(Image theImage, int theX, int theY, TRect theSrcRect)
		{
			BeginPrimitiveBatch(theImage);
			TRect source = new TRect(theImage.mS + theSrcRect.mX, theImage.mT + theSrcRect.mY, theSrcRect.mWidth, theSrcRect.mHeight);
			TRect destination = new TRect(theX + mTransX, theY + mTransY, source.mWidth, source.mHeight);
			if (theSrcRect.mWidth > 0 && theSrcRect.mHeight > 0)
			{
				PrepareRectsForClipping(ref source, ref destination);
				primitiveBatch.Draw(theImage, destination, source, mColorizeImages ? base.mColor : Color.White, true, false);
			}
		}

		public void DrawImage(Image theImage, TRect theDestRect, TRect theSrcRect)
		{
			BeginPrimitiveBatch(theImage);
			theDestRect.mX += mTransX;
			theDestRect.mY += mTransY;
			theSrcRect.mX += theImage.mS;
			theSrcRect.mY += theImage.mT;
			PrepareRectsForClipping(ref theSrcRect, ref theDestRect);
			primitiveBatch.Draw(theImage, theDestRect, theSrcRect, mColorizeImages ? base.mColor : Color.White, true, false);
		}

		public void DrawImage(Texture2D theImage, int theX, int theY, int theStretchedWidth, int theStretchedHeight)
		{
			temp.Reset(theImage);
			DrawImage(temp, theX, theY, theStretchedWidth, theStretchedHeight);
		}

		public void DrawImage(Image theImage, int theX, int theY, int theStretchedWidth, int theStretchedHeight)
		{
			BeginPrimitiveBatch(theImage);
			TRect destination = new TRect(theX + mTransX, theY + mTransY, theStretchedWidth, theStretchedHeight);
			TRect source = new TRect(theImage.mS, theImage.mT, theImage.mWidth, theImage.mHeight);
			PrepareRectsForClipping(ref source, ref destination);
			primitiveBatch.Draw(theImage, destination, source, mColorizeImages ? base.mColor : Color.White, true, false);
		}

		public void DrawImageMirrorVertical(Image theImage, int theX, int theY, int theStretchedWidth, int theStretchedHeight)
		{
			BeginPrimitiveBatch(theImage);
			TRect destination = new TRect(theX + mTransX, theY + mTransY, theStretchedWidth, theStretchedHeight);
			TRect source = new TRect(theImage.mS, theImage.mT, theImage.mWidth, theImage.mHeight);
			PrepareRectsForClipping(ref source, ref destination);
			primitiveBatch.Draw(theImage, destination, source, mColorizeImages ? base.mColor : Color.White, true, false, PrimitiveBatchEffects.MirrorVertically);
		}

		public void DrawImageF(Image theImage, float theX, float theY)
		{
			DrawImage(theImage, (int)theX, (int)theY);
		}

		public void DrawImageF(Image theImage, float theX, float theY, TRect theSrcRect)
		{
			DrawImage(theImage, (int)theX, (int)theY, theSrcRect);
		}

		public void DrawImageMirror(Image theImage, int theX, int theY)
		{
			DrawImageMirror(theImage, theX, theY, true);
		}

		public void DrawImageMirror(Image theImage, int theX, int theY, bool mirror)
		{
			DrawImageMirror(theImage, theX, theY, new TRect(theImage.mS, theImage.mT, theImage.mWidth, theImage.mHeight), mirror);
		}

		public void DrawImageMirror(Image theImage, int theX, int theY, TRect theSrcRect)
		{
			DrawImageMirror(theImage, theX, theY, theSrcRect, true);
		}

		public void DrawImageMirror(Image theImage, TRect theDestRect, TRect theSrcRect)
		{
			DrawImageMirror(theImage, theDestRect, theSrcRect, true);
		}

		public void DrawImageMirror(Image theImage, TRect theDestRect, TRect theSrcRect, bool mirror)
		{
			BeginPrimitiveBatch(theImage);
			PrepareRectsForClipping(ref theSrcRect, ref theDestRect);
			primitiveBatch.Draw(theImage, theDestRect, theSrcRect, mColorizeImages ? base.mColor : Color.White, false, true, PrimitiveBatchEffects.MirrorHorizontally);
		}

		public void DrawImageMirror(Image theImage, int theX, int theY, TRect theSrcRect, bool mirror)
		{
			BeginPrimitiveBatch(theImage);
			TRect destination = new TRect(theX + mTransX, theY + mTransY, theSrcRect.mWidth, theSrcRect.mHeight);
			PrepareRectsForClipping(ref theSrcRect, ref destination);
			primitiveBatch.Draw(theImage, destination, theSrcRect, mColorizeImages ? base.mColor : Color.White, true, true, mirror ? PrimitiveBatchEffects.MirrorHorizontally : PrimitiveBatchEffects.None);
		}

		public void DrawImageRotated(Image theImage, int theX, int theY, double theRot)
		{
			DrawImageRotated(theImage, theX, theY, theRot, new TRect(0, 0, theImage.GetWidth(), theImage.GetHeight()));
		}

		public void DrawImageRotated(Image theImage, int theX, int theY, double theRot, TRect theSrcRect)
		{
			theSrcRect.mX += theImage.mS;
			theSrcRect.mY += theImage.mT;
			DrawImageRotatedF(theImage, theX, theY, theRot, theSrcRect);
		}

		public void DrawImageRotated(Image theImage, int theX, int theY, double theRot, int theRotCenterX, int theRotCenterY)
		{
			DrawImageRotated(theImage, theX, theY, theRot, theRotCenterX, theRotCenterY, new TRect(0, 0, theImage.GetWidth(), theImage.GetHeight()));
		}

		public void DrawImageRotated(Image theImage, int theX, int theY, double theRot, int theRotCenterX, int theRotCenterY, TRect theSrcRect)
		{
			DrawImageRotatedF(theImage, theX, theY, theRot, theRotCenterX, theRotCenterY, theSrcRect);
		}

		public void DrawImageRotatedF(Image theImage, float theX, float theY, double theRot)
		{
			DrawImageRotatedF(theImage, theX, theY, theRot, new TRect(theImage.mS, theImage.mT, theImage.GetWidth(), theImage.GetHeight()));
		}

		public void DrawImageRotatedF(Image theImage, float theX, float theY, double theRot, TRect theSrcRect)
		{
			int num = theSrcRect.mWidth / 2;
			int num2 = theSrcRect.mHeight / 2;
			DrawImageRotatedF(theImage, theX, theY, theRot, num, num2, theSrcRect);
		}

		public void DrawImageRotatedF(Image theImage, float theX, float theY, double theRot, float theRotCenterX, float theRotCenterY)
		{
			DrawImageRotatedF(theImage, theX, theY, theRot, theRotCenterX, theRotCenterY, new TRect(theImage.mS, theImage.mT, theImage.GetWidth(), theImage.GetHeight()));
		}

		public void DrawImageRotatedF(Image theImage, float theX, float theY, double theRot, float theRotCenterX, float theRotCenterY, TRect? theSrcRect)
		{
			DrawImageRotatedScaled(theImage, theX, theY, theRot, theRotCenterX, theRotCenterY, theSrcRect, theImage.GetCelWidth(), theImage.GetCelHeight());
		}

		public void DrawImageRotatedScaled(Image theImage, float theX, float theY, double theRot, float theRotCenterX, float theRotCenterY, TRect? theSrcRect, int stretchedHeight, int stretchedWidth)
		{
			BeginPrimitiveBatch(theImage);
			if (!theSrcRect.HasValue)
			{
				TRect source = new TRect(theImage.mS, theImage.mT, theImage.mWidth, theImage.mHeight);
				TRect destination = new TRect((int)theX, (int)theY, stretchedHeight, stretchedWidth);
				primitiveBatch.DrawRotatedScaled(theImage, destination, source, new Vector2(theRotCenterX, theRotCenterY), (float)theRot, new Vector2((float)theImage.mWidth / (float)stretchedWidth, (float)theImage.mHeight / (float)stretchedHeight), mColorizeImages ? base.mColor : Color.White, false, true, PrimitiveBatchEffects.None);
			}
			else
			{
				TRect destination2 = new TRect((int)theX, (int)theY, stretchedHeight, stretchedWidth);
				primitiveBatch.DrawRotatedScaled(theImage, destination2, theSrcRect.Value, new Vector2(theRotCenterX, theRotCenterY), (float)theRot, new Vector2((float)theSrcRect.Value.mWidth / (float)stretchedWidth, (float)theSrcRect.Value.mHeight / (float)stretchedHeight), mColorizeImages ? base.mColor : Color.White, false, true, PrimitiveBatchEffects.None);
			}
		}

		public void DrawTriangle(TriVertex p1, TriVertex p2, TriVertex p3, Color theColor, DrawMode theDrawMode)
		{
			EndDrawImageTransformed();
			bool spritebatchBegan = Graphics.spritebatchBegan;
			if (Graphics.spritebatchBegan)
			{
				EndFrame();
			}
			SetupDrawMode(theDrawMode);
			Matrix? transform = null;
			if (gTransformStack.Count > 0)
			{
				transform = gTransformStack.Peek().mMatrix;
			}
			primitiveBatch.Begin(PrimitiveType.TriangleList, mTransX, mTransY, transform, null, NormalSamplerState);
			primitiveBatch.AddVertex(new Vector2(p1.x, p1.y), theColor);
			primitiveBatch.AddVertex(new Vector2(p2.x, p2.y), theColor);
			primitiveBatch.AddVertex(new Vector2(p3.x, p3.y), theColor);
			primitiveBatch.End();
			if (spritebatchBegan)
			{
				BeginFrame();
			}
		}

		public void DrawTriangle(TriVertex p1, Color c1, TriVertex p2, Color c2, TriVertex p3, Color c3, DrawMode theDrawMode)
		{
			EndDrawImageTransformed();
			bool spritebatchBegan = Graphics.spritebatchBegan;
			if (Graphics.spritebatchBegan)
			{
				EndFrame();
			}
			SetupDrawMode(theDrawMode);
			Matrix? transform = null;
			if (gTransformStack.Count > 0)
			{
				transform = gTransformStack.Peek().mMatrix;
			}
			primitiveBatch.Begin(PrimitiveType.TriangleList, mTransX, mTransY, transform, null, NormalSamplerState);
			primitiveBatch.AddVertex(new Vector2(p1.x, p1.y), c1);
			primitiveBatch.AddVertex(new Vector2(p2.x, p2.y), c2);
			primitiveBatch.AddVertex(new Vector2(p3.x, p3.y), c3);
			primitiveBatch.primitiveCount = 1;
			primitiveBatch.End();
			if (spritebatchBegan)
			{
				BeginFrame();
			}
		}

		public void DrawImageCel(Image theImageStrip, int theX, int theY, int theCel)
		{
			DrawImageCel(theImageStrip, theX, theY, theCel % theImageStrip.mNumCols, theCel / theImageStrip.mNumCols);
		}

		public void DrawImageCel(Image theImageStrip, TRect theDestRect, int theCel)
		{
			DrawImageCel(theImageStrip, theDestRect, theCel % theImageStrip.mNumCols, theCel / theImageStrip.mNumCols);
		}

		public void DrawImageCel(Image theImageStrip, int theX, int theY, int theCelCol, int theCelRow)
		{
			if (theCelRow >= 0 && theCelCol >= 0 && theCelRow < theImageStrip.mNumRows && theCelCol < theImageStrip.mNumCols)
			{
				int num = theImageStrip.mWidth / theImageStrip.mNumCols;
				int num2 = theImageStrip.mHeight / theImageStrip.mNumRows;
				TRect theSrcRect = new TRect(num * theCelCol, num2 * theCelRow, num, num2);
				DrawImage(theImageStrip, theX, theY, theSrcRect);
			}
		}

		public void DrawImageCel(Image theImageStrip, TRect theDestRect, int theCelCol, int theCelRow)
		{
			if (theCelRow >= 0 && theCelCol >= 0 && theCelRow < theImageStrip.mNumRows && theCelCol < theImageStrip.mNumCols)
			{
				int num = theImageStrip.mWidth / theImageStrip.mNumCols;
				int num2 = theImageStrip.mHeight / theImageStrip.mNumRows;
				TRect theSrcRect = new TRect(num * theCelCol, num2 * theCelRow, num, num2);
				if (num != theDestRect.mWidth || num2 != theDestRect.mHeight)
				{
					DrawImage(theSrcRect: new TRect(num * theCelCol, num2 * theCelRow, num, num2), theImage: theImageStrip, theDestRect: theDestRect);
				}
				else
				{
					DrawImage(theImageStrip, theDestRect.mX, theDestRect.mY, theSrcRect);
				}
			}
		}

		public void DrawImageAnim(Image theImageAnim, int theX, int theY, int theTime)
		{
			DrawImageCel(theImageAnim, theX, theY, theImageAnim.GetAnimCel(theTime));
		}

		public void ClearClipRect()
		{
			TRect tRect = mClipRect = ((mDestImage != null) ? new TRect(0, 0, mDestImage.Bounds.Width, mDestImage.Bounds.Height) : new TRect(0, 0, GlobalStaticVars.gSexyAppBase.mWidth, GlobalStaticVars.gSexyAppBase.mHeight));
		}

		public void SetClipRect(int theX, int theY, int theWidth, int theHeight)
		{
			ClearClipRect();
			mClipRect = mClipRect.Intersection(new TRect(theX + mTransX, theY + mTransY, theWidth, theHeight));
		}

		public void SetClipRect(TRect theRect)
		{
			SetClipRect(ref theRect);
		}

		public void SetClipRect(ref TRect theRect)
		{
			SetClipRect(theRect.mX, theRect.mY, theRect.mWidth, theRect.mHeight);
		}

		public TRect GetClipRect()
		{
			return mClipRect;
		}

		public void ClipRect(int theX, int theY, int theWidth, int theHeight)
		{
			TRect tRect = new TRect(theX + mTransX, theY + mTransY, theWidth, theHeight);
			if (!(tRect == mClipRect))
			{
				mClipRect = mClipRect.Intersection(tRect);
			}
		}

		public void ClipRect(TRect theRect)
		{
			ClipRect(theRect.mX, theRect.mY, theRect.mWidth, theRect.mHeight);
		}

		public int StringWidth(string theString)
		{
			return mFont.StringWidth(theString);
		}

		public void DrawImageBox(TRect theDest, Image theComponentImage)
		{
			DrawImageBox(new TRect(0, 0, theComponentImage.mWidth, theComponentImage.mHeight), theDest, theComponentImage);
		}

		public void DrawImageBox(TRect theSrc, TRect theDest, Image theComponentImage)
		{
			if (theSrc.mWidth <= 0 || theSrc.mHeight <= 0)
			{
				return;
			}
			int num = theSrc.mWidth / 3;
			int num2 = theSrc.mHeight / 3;
			int mX = theSrc.mX;
			int mY = theSrc.mY;
			int num3 = theSrc.mWidth - num * 2;
			int num4 = theSrc.mHeight - num2 * 2;
			DrawImage(theComponentImage, theDest.mX, theDest.mY, new TRect(mX, mY, num, num2));
			DrawImage(theComponentImage, theDest.mX + theDest.mWidth - num, theDest.mY, new TRect(mX + num + num3, mY, num, num2));
			DrawImage(theComponentImage, theDest.mX, theDest.mY + theDest.mHeight - num2, new TRect(mX, mY + num2 + num4, num, num2));
			DrawImage(theComponentImage, theDest.mX + theDest.mWidth - num, theDest.mY + theDest.mHeight - num2, new TRect(mX + num + num3, mY + num2 + num4, num, num2));
			Graphics @new = GetNew(this);
			@new.ClipRect(theDest.mX + num, theDest.mY, theDest.mWidth - num * 2, theDest.mHeight);
			for (int i = 0; i < (theDest.mWidth - num * 2 + num3 - 1) / num3; i++)
			{
				@new.DrawImage(theComponentImage, theDest.mX + num + i * num3, theDest.mY, new TRect(mX + num, mY, num3, num2));
				@new.DrawImage(theComponentImage, theDest.mX + num + i * num3, theDest.mY + theDest.mHeight - num2, new TRect(mX + num, mY + num2 + num4, num3, num2));
			}
			@new.PrepareForReuse();
			Graphics new2 = GetNew(this);
			new2.ClipRect(theDest.mX, theDest.mY + num2, theDest.mWidth, theDest.mHeight - num2 * 2);
			for (int j = 0; j < (theDest.mHeight - num2 * 2 + num4 - 1) / num4; j++)
			{
				new2.DrawImage(theComponentImage, theDest.mX, theDest.mY + num2 + j * num4, new TRect(mX, mY + num2, num, num4));
				new2.DrawImage(theComponentImage, theDest.mX + theDest.mWidth - num, theDest.mY + num2 + j * num4, new TRect(mX + num + num3, mY + num2, num, num4));
			}
			new2.PrepareForReuse();
			Graphics new3 = GetNew(this);
			new3.ClipRect(theDest.mX + num, theDest.mY + num2, theDest.mWidth - num * 2, theDest.mHeight - num2 * 2);
			for (int i = 0; i < (theDest.mWidth - num * 2 + num3 - 1) / num3; i++)
			{
				for (int j = 0; j < (theDest.mHeight - num2 * 2 + num4 - 1) / num4; j++)
				{
					new3.DrawImage(theComponentImage, theDest.mX + num + i * num3, theDest.mY + num2 + j * num4, new TRect(mX + num, mY + num2, num3, num4));
				}
			}
			new3.PrepareForReuse();
		}

		public int WriteString(string theString, int theX, int theY, int theWidth, int theJustification, bool drawString, int theOffset, int theLength)
		{
			return WriteString(theString, theX, theY, theWidth, theJustification, drawString, theOffset, theLength, -1);
		}

		public int WriteString(string theString, int theX, int theY, int theWidth, int theJustification, bool drawString, int theOffset, int theLength, int theOldColor)
		{
			mFont.DrawString(this, theX, theY, theString, new SexyColor(theOldColor.ToString()));
			return theX;
		}

		public int WriteWordWrappedLayer(TRect theRect, string theLine, int theLineSpacing, int theJustification, int layer)
		{
			return WriteWordWrappedLayer(theRect, theLine, theLineSpacing, theJustification, 0, -1, 0, 0, layer, false);
		}

		public int WriteWordWrappedLayer(TRect theRect, string theLine, int theLineSpacing, int theJustification, int layer, bool centerVertically)
		{
			return WriteWordWrappedLayer(theRect, theLine, theLineSpacing, theJustification, 0, -1, 0, 0, layer, centerVertically);
		}

		public int WriteWordWrappedLayer(TRect theRect, string theLine, int theLineSpacing, int theJustification, int theMaxWidth, int theMaxChars, int theLastWidth, int theLineCount, int layer, bool centerVertically)
		{
			Font.CachedStringInfo wordWrappedSubStrings = mFont.GetWordWrappedSubStrings(theLine, theRect);
			theRect.mX += mTransX;
			theRect.mY += mTransY;
			Vector2 vector = new Vector2(theRect.mX, theRect.mY);
			mFont.GetHeight();
			if (centerVertically)
			{
				float num = 0f;
				for (int i = 0; i < wordWrappedSubStrings.Strings.Length; i++)
				{
					num += wordWrappedSubStrings.StringDimensions[i].Y;
				}
				vector.Y += (float)(theRect.mHeight / 2) - num / 2f;
			}
			for (int j = 0; j < wordWrappedSubStrings.Strings.Length; j++)
			{
				vector.X = theRect.mX;
				if (theJustification == 0)
				{
					vector.X += ((float)theRect.mWidth - wordWrappedSubStrings.StringDimensions[j].X) / 2f;
				}
				mFont.DrawStringLayer(this, (int)(vector.X + 0.5f), (int)(vector.Y + 0.5f), wordWrappedSubStrings.Strings[j], base.mColor, layer);
				vector.Y += wordWrappedSubStrings.StringDimensions[j].Y;
			}
			return (int)vector.Y;
		}

		public int WriteWordWrapped(TRect theRect, string theLine, int theLineSpacing, int theJustification)
		{
			return WriteWordWrapped(theRect, theLine, theLineSpacing, theJustification, false);
		}

		public int WriteWordWrapped(TRect theRect, string theLine, int theLineSpacing, int theJustification, bool centerVertically)
		{
			return WriteWordWrapped(theRect, theLine, theLineSpacing, theJustification, 0, -1, 0, 0, centerVertically);
		}

		public int WriteWordWrapped(TRect theRect, string theLine, int theLineSpacing, int theJustification, int theMaxWidth, int theMaxChars, int theLastWidth, int theLineCount)
		{
			return WriteWordWrapped(theRect, theLine, theLineSpacing, theJustification, theMaxWidth, theMaxChars, theLastWidth, theLineCount, false);
		}

		public int WriteWordWrapped(TRect theRect, string theLine, int theLineSpacing, int theJustification, int theMaxWidth, int theMaxChars, int theLastWidth, int theLineCount, bool centerVertically)
		{
			return WriteWordWrapped(theRect, theLine, theLineSpacing, theJustification, theMaxWidth, theMaxChars, theLastWidth, theLineCount, centerVertically, true);
		}

		public int WriteWordWrapped(TRect theRect, string theLine, int theLineSpacing, int theJustification, int theMaxWidth, int theMaxChars, int theLastWidth, int theLineCount, bool centerVertically, bool doDraw)
		{
			Font.CachedStringInfo wordWrappedSubStrings = mFont.GetWordWrappedSubStrings(theLine, theRect);
			theRect.mX += mTransX;
			theRect.mY += mTransY;
			Vector2 vector = new Vector2(theRect.mX, theRect.mY);
			mFont.GetHeight();
			if (centerVertically)
			{
				float num = 0f;
				for (int i = 0; i < wordWrappedSubStrings.Strings.Length; i++)
				{
					num += wordWrappedSubStrings.StringDimensions[i].Y;
				}
				vector.Y += (float)(theRect.mHeight / 2) - num / 2f;
			}
			for (int j = 0; j < wordWrappedSubStrings.Strings.Length; j++)
			{
				vector.X = theRect.mX;
				if (theJustification == 0)
				{
					vector.X += ((float)theRect.mWidth - wordWrappedSubStrings.StringDimensions[j].X) / 2f;
				}
				if (doDraw)
				{
					mFont.DrawString(this, (int)(vector.X + 0.5f), (int)(vector.Y + 0.5f), wordWrappedSubStrings.Strings[j], base.mColor);
				}
				vector.Y += wordWrappedSubStrings.StringDimensions[j].Y;
			}
			return (int)vector.Y;
		}

		public void DrawStringColor(string theLine, int theX, int theY, int theOldColor)
		{
			mFont.DrawString(this, theX, theY, theLine, new SexyColor(theOldColor.ToString()));
		}

		public int GetWordWrappedHeight(int theWidth, string theLine, int theLineSpacing, ref int theMaxWidth, int theMaxChars)
		{
			Graphics @new = GetNew();
			@new.SetFont(mFont);
			@new.SetClipRect(0, 0, 0, 0);
			int result = @new.WriteWordWrapped(new TRect(0, 0, theWidth, 0), theLine, theLineSpacing, -1, theMaxWidth, theMaxChars, 0, 0, false, false);
			@new.PrepareForReuse();
			return result;
		}

		public bool Is3D()
		{
			return true;
		}

		internal void PopState()
		{
			if (mStateStack.Count > 0)
			{
				DrawMode mDrawMode2 = mDrawMode;
				DrawMode mDrawMode3 = mStateStack.Peek().mDrawMode;
				CopyStateFrom(mStateStack.Peek());
				Graphics graphics = mStateStack.Peek();
				bool flag = graphics.mDrawMode != mDrawMode;
				graphics.PrepareForReuse();
				mStateStack.Pop();
				if (flag && spritebatchBegan)
				{
					EndFrame();
					BeginFrame();
				}
			}
		}

		internal void PushState()
		{
			Graphics @new = GetNew(this);
			mStateStack.Push(@new);
			if (mDrawMode != @new.mDrawMode && spritebatchBegan)
			{
				EndFrame();
				BeginFrame();
			}
		}

		internal void Translate(int x, int y)
		{
			mTransX += x;
			mTransY += y;
		}

		public void Reset()
		{
			mTransX = 0;
			mTransY = 0;
			mScaleX = 1f;
			mScaleY = 1f;
			mScaleOrigX = 0f;
			mScaleOrigY = 0f;
			mFastStretch = false;
			mWriteColoredString = false;
			mLinearBlend = false;
			mClipRect = new TRect(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
			ClearClipRect();
			base.mColor = Color.White;
			mDrawMode = currentlyActiveDrawMode;
			mColorizeImages = false;
		}

		internal void PrepareDrawing()
		{
			BeginFrame();
		}

		internal void FinishedDrawing()
		{
			EndFrame();
		}

		public void DrawTriangleTex(TriVertex p1, TriVertex p2, TriVertex p3, Color theColor, DrawMode theDrawMode, Image theTexture)
		{
			DrawTriangleTex(p1, p2, p3, theColor, theDrawMode, theTexture, true);
		}

		public void DrawTriangleTex(TriVertex p1, TriVertex p2, TriVertex p3, Color theColor, DrawMode theDrawMode, Image theTexture, bool blend)
		{
			tempTriangles[0, 0] = p1;
			tempTriangles[0, 1] = p2;
			tempTriangles[0, 2] = p3;
			DrawTrianglesTex(tempTriangles, 1, theColor, theDrawMode, theTexture, mTransX, mTransY, blend);
		}

		public void DrawTriangleTex(Image theTexture, TriVertex v1, TriVertex v2, TriVertex v3)
		{
			DrawTriangleTex(v1, v2, v3, mColorizeImages ? base.mColor : Color.White, mDrawMode, theTexture);
		}

		public void DrawTrianglesTex(Image theTexture, TriVertex[,] theVertices, int theNumTriangles)
		{
			DrawTrianglesTex(theVertices, theNumTriangles, mColorizeImages ? base.mColor : Color.White, mDrawMode, theTexture, mTransX, mTransY, mLinearBlend);
		}

		public void DrawTrianglesTex(Image theTexture, TriVertex[,] theVertices, int theNumTriangles, Color? theColor, DrawMode theDrawMode)
		{
			DrawTrianglesTex(theVertices, theNumTriangles, theColor, theDrawMode, theTexture, mTransX, mTransY, mLinearBlend);
		}

		public void DrawTrianglesTex(SamplerState st, Image theTexture, TriVertex[,] theVertices, int theNumTriangles, Color? theColor, DrawMode theDrawMode)
		{
			DrawTrianglesTex(st, theVertices, theNumTriangles, theColor, theDrawMode, theTexture, mTransX, mTransY, mLinearBlend);
		}

		public void DrawTrianglesTex(TriVertex[,] theVertices, int theNumTriangles, Color? theColor, DrawMode theDrawMode, Image theTexture, float tx, float ty, bool blend)
		{
			DrawTrianglesTex(null, theVertices, theNumTriangles, theColor, theDrawMode, theTexture, tx, ty, blend);
		}

		public void DrawTrianglesTex(TriVertex[,] theVertices, int theNumTriangles, Color theColor, DrawMode theDrawMode, Image theTexture)
		{
			DrawTrianglesTex(theVertices, theNumTriangles, theColor, theDrawMode, theTexture, 0f, 0f, true);
		}

		public void DrawTrianglesTex(SamplerState st, TriVertex[,] theVertices, int theNumTriangles, Color? theColor, DrawMode theDrawMode, Image theTexture, float tx, float ty, bool blend)
		{
			bool spritebatchBegan = Graphics.spritebatchBegan;
			if (Graphics.spritebatchBegan)
			{
				EndFrame();
			}
			EndDrawImageTransformed(false);
			for (int i = 0; i < theNumTriangles; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					int width = theTexture.Texture.Width;
					int height = theTexture.Texture.Height;
					theVertices[i, j].u = (theVertices[i, j].u * (float)theTexture.GetWidth() + (float)theTexture.mS) / (float)width;
					theVertices[i, j].v = (theVertices[i, j].v * (float)theTexture.GetHeight() + (float)theTexture.mT) / (float)height;
				}
			}
			SetupDrawMode(theDrawMode);
			Matrix? transform = null;
			if (gTransformStack.Count > 0)
			{
				transform = gTransformStack.Peek().mMatrix;
			}
			if (st != null)
			{
				GraphicsDevice.SamplerStates[0] = st;
			}
			primitiveBatch.Begin(PrimitiveType.TriangleList, mTransX, mTransY, transform, theTexture, NormalSamplerState);
			primitiveBatch.AddTriVertices(theVertices, theNumTriangles, theColor);
			primitiveBatch.End();
			if (spritebatchBegan)
			{
				BeginFrame();
			}
		}

		public void BeginDrawTrianglesTexBatch(SamplerState st, DrawMode theDrawMode, Image theTexture)
		{
			if (spritebatchBegan)
			{
				EndFrame();
			}
			SetupDrawMode(theDrawMode);
			Matrix? transform = null;
			if (gTransformStack.Count > 0)
			{
				transform = gTransformStack.Peek().mMatrix;
			}
			if (st != null)
			{
				GraphicsDevice.SamplerStates[0] = st;
			}
			primitiveBatch.Begin(PrimitiveType.TriangleList, mTransX, mTransY, transform, theTexture, NormalSamplerState);
			triangleBatchTexture = theTexture;
		}

		public void DrawTrianglesTexBatch(TriVertex[,] theVertices, int theNumTriangles, Color? theColor)
		{
			for (int i = 0; i < theNumTriangles; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					int width = triangleBatchTexture.Texture.Width;
					int height = triangleBatchTexture.Texture.Height;
					theVertices[i, j].u = (theVertices[i, j].u * (float)triangleBatchTexture.GetWidth() + (float)triangleBatchTexture.mS) / (float)width;
					theVertices[i, j].v = (theVertices[i, j].v * (float)triangleBatchTexture.GetHeight() + (float)triangleBatchTexture.mT) / (float)height;
				}
			}
			primitiveBatch.AddTriVertices(theVertices, theNumTriangles, theColor);
		}

		public void EndDrawTrianglesTexBatch()
		{
			primitiveBatch.End();
			BeginFrame();
		}

		public void pushTransform(ref SexyTransform2D theTransform, bool concatenate)
		{
			if (gTransformStack.empty() || !concatenate)
			{
				gTransformStack.push_back(theTransform);
			}
			else
			{
				SexyTransform2D b = gTransformStack.back();
				gTransformStack.push_back(theTransform * b);
			}
			if (spritebatchBegan)
			{
				EndFrame();
				BeginFrame();
			}
		}

		public void popTransform()
		{
			if (!gTransformStack.empty())
			{
				gTransformStack.pop_back();
				if (spritebatchBegan)
				{
					EndFrame();
					BeginFrame();
				}
			}
		}

		public static void PushTransform(ref SexyTransform2D theTransform, bool concatenate)
		{
			GlobalStaticVars.g.pushTransform(ref theTransform, concatenate);
		}

		public static void PushTransform(ref SexyTransform2D theTransform)
		{
			PushTransform(ref theTransform, true);
		}

		public static void ClearTransformStack()
		{
			GlobalStaticVars.g.clearTransformStack();
		}

		public void clearTransformStack()
		{
			gTransformStack.Clear();
		}

		public static void PopTransform()
		{
			GlobalStaticVars.g.popTransform();
		}

		public bool MatchesHardWareClipRect(TRect clip)
		{
			return hardwareClippedRectangle == clip.Intersection(new TRect(0, 0, base.mScreenWidth, base.mScreenHeight));
		}

		public void HardwareClip()
		{
			HardwareClip(SpriteSortMode.Deferred);
		}

		public void HardwareClip(SpriteSortMode spriteSortMode)
		{
			EndFrame();
			TRect mClipRect = base.mClipRect;
			mClipRect = mClipRect.Intersection(new TRect(0, 0, base.mScreenWidth, base.mScreenHeight));
			GraphicsDevice.ScissorRectangle = mClipRect;
			hardwareClippingEnabled = true;
			hardwareClippedRectangle = base.mClipRect;
			BeginFrame(hardwareClipState, spriteSortMode);
		}

		public void EndHardwareClip()
		{
			hardwareClippingEnabled = false;
			hardwareClippedRectangle = TRect.Empty;
			EndFrame();
			BeginFrame();
		}

		public void HardwareClipRect(TRect theClip)
		{
			HardwareClipRect(theClip, SpriteSortMode.Deferred);
		}

		public void HardwareClipRect(TRect theClip, SpriteSortMode sortMode)
		{
			EndFrame();
			theClip.mX += mTransX;
			theClip.mY += mTransY;
			Rectangle rectangle = theClip.Intersection(new TRect(0, 0, base.mScreenWidth, base.mScreenHeight));
			hardwareClippingEnabled = true;
			hardwareClippedRectangle = (TRect)rectangle;
			GraphicsDevice.ScissorRectangle = rectangle;
			BeginFrame(hardwareClipState, sortMode);
		}

		public void ClearHardwareClipRect()
		{
			spriteBatch.End();
			BeginFrame();
		}
	}
}
