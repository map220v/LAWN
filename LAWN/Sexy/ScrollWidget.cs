using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Sexy
{
	internal class ScrollWidget : Widget, ProxyWidgetListener
	{
		public enum ScrollMode
		{
			SCROLL_DISABLED,
			SCROLL_HORIZONTAL,
			SCROLL_VERTICAL,
			SCROLL_BOTH
		}

		public enum Colors
		{
			COLOR_BACKGROUND
		}

		protected class Overlay
		{
			public Image image;

			public CGPoint offset = default(CGPoint);
		}

		internal const float SCROLL_TARGET_THRESHOLD_NORM = 0.01f;

		internal const float SCROLL_VELOCITY_THRESHOLD_NORM = 0.0001f;

		internal const float SCROLL_DEVIATION_DAMPING = 0.5f;

		internal const float SCROLL_SPRINGBACK_TENSION = 0.1f;

		internal const float SCROLL_VELOCITY_FILTER_WINDOW = 0.1f;

		internal const float SCROLL_VELOCITY_DAMPING = 0.975f;

		internal const float SCROLL_VELOCITY_DEVIATION_DAMPING = 0.85f;

		internal const float SCROLL_DRAG_THRESHOLD = 4f;

		internal const float SCROLL_PAGE_FLICK_THRESHOLD = 40f;

		internal static readonly int SCROLL_TAP_DELAY_TICKS = SexyAppFrameworkConstants.ticksForSeconds(0.1f);

		internal static readonly int SCROLL_INDICATORS_FLASH_TICKS = SexyAppFrameworkConstants.ticksForSeconds(1f);

		internal static readonly float SCROLL_INDICATORS_FADE_IN_RATE = 0.05f;

		internal static readonly float SCROLL_INDICATORS_FADE_OUT_RATE = 0.02f;

		private bool clientAllowsScroll;

		private CGPoint oldTouch;

		private double oldTouchTime;

		protected ScrollWidgetListener mListener;

		protected Widget mClient;

		protected Widget mClientLastDown;

		protected PageControl mPageControl;

		protected ProxyWidget mIndicatorsProxy;

		protected Image mIndicatorsImage;

		protected Image mBackgroundImage;

		protected bool mFillBackground;

		protected List<Overlay> mOverlays = new List<Overlay>();

		protected bool mDrawOverlays;

		protected ScrollMode mScrollMode;

		protected Insets mScrollInsets = default(Insets);

		protected CGPoint mScrollTarget = default(CGPoint);

		protected CGPoint mScrollOffset = default(CGPoint);

		protected CGPoint mScrollVelocity = default(CGPoint);

		protected CGPoint mScrollTouchReference = default(CGPoint);

		protected CGPoint mScrollOffsetReference = default(CGPoint);

		protected bool mBounceEnabled;

		protected bool mPagingEnabled;

		protected bool mIndicatorsEnabled;

		protected Insets mIndicatorsInsets = default(Insets);

		protected int mIndicatorsFlashTimer;

		protected float mIndicatorsOpacity;

		protected int mCurrentPageHorizontal;

		protected int mCurrentPageVertical;

		protected bool mSeekScrollTarget;

		protected bool mScrollTracking;

		protected double mScrollLastTimestamp;

		public float mSpringOverride;

		protected CGPoint mScrollMin = default(CGPoint);

		protected CGPoint mScrollMax = default(CGPoint);

		protected CGPoint mPageSize = default(CGPoint);

		protected ScrollMode mScrollPractical;

		protected int mPageCountHorizontal;

		protected int mPageCountVertical;

		public ScrollWidget(ScrollWidgetListener listener)
		{
			Init(listener);
		}

		public ScrollWidget()
		{
			Init(null);
		}

		public override void Dispose()
		{
			RemoveAllWidgets(true, true);
		}

		public void SetPageControl(PageControl pageControl)
		{
			mPageControl = pageControl;
			if (mPagingEnabled)
			{
				mPageControl.SetNumberOfPages(mPageCountHorizontal);
			}
		}

		public void SetScrollMode(ScrollMode mode)
		{
			mScrollMode = mode;
			CacheDerivedValues();
		}

		public void SetScrollInsets(Insets insets)
		{
			mScrollInsets = insets;
			CacheDerivedValues();
		}

		public void SetScrollOffset(CGPoint offset, bool animated)
		{
			if (animated)
			{
				mScrollTarget = offset;
				mSeekScrollTarget = true;
				return;
			}
			mScrollOffset = offset;
			mScrollVelocity = CGMaths.CGPointMake(0f, 0f);
			if (mClient != null)
			{
				mClient.Move((int)mScrollOffset.x, (int)mScrollOffset.y);
			}
		}

		public void ScrollToMin(bool animated)
		{
			SetScrollOffset(CGMaths.CGPointMake(mScrollInsets.mLeft, mScrollInsets.mTop), animated);
		}

		public void ScrollToBottom(bool animated)
		{
			SetScrollOffset(CGMaths.CGPointMake(mScrollMin.x, mScrollMin.y), animated);
		}

		public void ScrollToPoint(CGPoint point, bool animated)
		{
			if (!mIsDown)
			{
				CGPoint offset = default(CGPoint);
				offset.x = 0f - point.mX;
				offset.y = 0f - point.mY;
				SetScrollOffset(offset, animated);
			}
		}

		public void ScrollRectIntoView(TRect rect, bool animated)
		{
			if (!mIsDown)
			{
				float num = rect.mX + rect.mWidth;
				float num2 = rect.mY + rect.mHeight;
				float val = Math.Max(Math.Min(0f, mScrollMin.x), -rect.mX);
				float val2 = Math.Max(Math.Min(0f, mScrollMin.y), -rect.mY);
				float val3 = Math.Min(mScrollMax.x, (float)mWidth - num);
				float val4 = Math.Min(mScrollMax.y, (float)mHeight - num2);
				CGPoint offset = default(CGPoint);
				offset.x = Math.Min(val3, Math.Max(val, mScrollOffset.x));
				offset.y = Math.Min(val4, Math.Max(val2, mScrollOffset.y));
				SetScrollOffset(offset, animated);
			}
		}

		public void EnableBounce(bool enable)
		{
			mBounceEnabled = enable;
		}

		public void EnablePaging(bool enable)
		{
			mPagingEnabled = enable;
		}

		public void EnableIndicators(Image indicatorsImage)
		{
			mIndicatorsImage = indicatorsImage;
			mIndicatorsEnabled = (null != indicatorsImage);
			if (mIndicatorsEnabled && mIndicatorsProxy == null)
			{
				mIndicatorsProxy = new ProxyWidget(this);
				mIndicatorsProxy.mMouseVisible = false;
				mIndicatorsProxy.mZOrder = int.MaxValue;
				mIndicatorsProxy.Resize(0, 0, mWidth, mHeight);
				base.AddWidget(mIndicatorsProxy);
			}
			else if (!mIndicatorsEnabled && mIndicatorsProxy != null)
			{
				base.RemoveWidget(mIndicatorsProxy);
				mIndicatorsProxy.Dispose();
				mIndicatorsProxy = null;
			}
		}

		public void SetIndicatorsInsets(Insets insets)
		{
			mIndicatorsInsets = insets;
		}

		public void FlashIndicators()
		{
			mIndicatorsFlashTimer = SCROLL_INDICATORS_FLASH_TICKS;
		}

		public void SetPageHorizontal(int page, bool animated)
		{
			SetPage(page, mCurrentPageVertical, animated);
		}

		public void SetPageVertical(int page, bool animated)
		{
			SetPage(mCurrentPageHorizontal, page, animated);
		}

		public void SetPage(int hpage, int vpage, bool animated)
		{
			if (mPagingEnabled)
			{
				mCurrentPageHorizontal = Math.Max(0, Math.Min(hpage, mPageCountHorizontal - 1));
				mCurrentPageVertical = Math.Max(0, Math.Min(vpage, mPageCountVertical - 1));
				CGPoint offset = default(CGPoint);
				offset.x = (float)mScrollInsets.mLeft - (float)mCurrentPageHorizontal * mPageSize.x;
				offset.y = (float)mScrollInsets.mTop - (float)mCurrentPageVertical * mPageSize.y;
				SetScrollOffset(offset, animated);
			}
		}

		public int GetPageHorizontal()
		{
			return mCurrentPageHorizontal;
		}

		public int GetPageVertical()
		{
			return mCurrentPageVertical;
		}

		public void SetBackgroundImage(Image image)
		{
			mBackgroundImage = image;
		}

		public void EnableBackgroundFill(bool enable)
		{
			mFillBackground = enable;
		}

		public void AddOverlayImage(Image image, CGPoint offset)
		{
			mDrawOverlays = true;
			foreach (Overlay mOverlay in mOverlays)
			{
				if (mOverlay.image == image)
				{
					mOverlay.offset = offset;
					return;
				}
			}
			Overlay overlay = new Overlay();
			overlay.image = image;
			overlay.offset = offset;
			mOverlays.Add(overlay);
		}

		public void EnableOverlays(bool enable)
		{
			mDrawOverlays = enable;
		}

		public override void AddWidget(Widget theWidget)
		{
			if (mClient == null)
			{
				mClient = theWidget;
				mClient.mWidgetFlagsMod.mRemoveFlags |= 16;
				mClient.Move((int)mScrollOffset.x, (int)mScrollOffset.y);
				base.AddWidget(mClient);
				CacheDerivedValues();
			}
		}

		public override void RemoveWidget(Widget theWidget)
		{
			if (theWidget == mClient)
			{
				mClient = null;
			}
			base.RemoveWidget(theWidget);
		}

		public override void Resize(int x, int y, int width, int height)
		{
			base.Resize(x, y, width, height);
			if (mIndicatorsProxy != null)
			{
				mIndicatorsProxy.Resize(0, 0, width, height);
			}
			CacheDerivedValues();
		}

		public override void Resize(TRect frame)
		{
			base.Resize(frame);
		}

		public void ClientSizeChanged()
		{
			if (mClient != null)
			{
				CacheDerivedValues();
			}
		}

		public override void TouchBegan(_Touch touch)
		{
			if (mClient == null)
			{
				return;
			}
			clientAllowsScroll = mClient.DoScroll(touch);
			if (mSeekScrollTarget)
			{
				if (mListener != null)
				{
					mListener.ScrollTargetInterrupted(this);
				}
				if (mPagingEnabled && mPageControl != null)
				{
					mPageControl.SetCurrentPage(mCurrentPageHorizontal);
				}
			}
			mScrollTouchReference = touch.location;
			mScrollOffsetReference = CGMaths.CGPointMake(mClient.mX, mClient.mY);
			mScrollOffset = mScrollOffsetReference;
			mScrollLastTimestamp = touch.timestamp;
			mScrollTracking = false;
			mSeekScrollTarget = false;
			mClientLastDown = GetClientWidgetAt(touch);
			mClientLastDown.mIsDown = true;
			mClientLastDown.mIsOver = true;
			mClientLastDown.TouchBegan(touch);
		}

		public override void TouchMoved(_Touch touch)
		{
			CGPoint cGPoint = CGMaths.CGPointSubtract(touch.location, mScrollTouchReference);
			if (mClient != null)
			{
				if (clientAllowsScroll)
				{
					if (!mScrollTracking && (mScrollPractical & ScrollMode.SCROLL_HORIZONTAL) != 0 && Math.Abs(cGPoint.x) > 4f)
					{
						mScrollTracking = true;
					}
					if (!mScrollTracking && (mScrollPractical & ScrollMode.SCROLL_VERTICAL) != 0 && Math.Abs(cGPoint.y) > 4f)
					{
						mScrollTracking = true;
					}
				}
				if (mScrollTracking && mClientLastDown != null)
				{
					mClientLastDown.TouchesCanceled();
					mClientLastDown.mIsDown = false;
					mClientLastDown = null;
				}
			}
			if (mScrollTracking)
			{
				TouchMotion(touch);
			}
			else if (mClientLastDown != null)
			{
				CGPoint b = GetAbsPos() - mClientLastDown.GetAbsPos();
				CGPoint a = new CGPoint(touch.location.X, touch.location.Y);
				CGPoint cGPoint2 = a + b;
				CGPoint a2 = new CGPoint(cGPoint2.mX + (float)mClientLastDown.mX, cGPoint2.mY + (float)mClientLastDown.mY);
				bool flag = mClientLastDown.GetInsetRect().Contains(a2);
				if (flag && !mClientLastDown.mIsOver)
				{
					mClientLastDown.mIsOver = true;
					mClientLastDown.MouseEnter();
				}
				else if (!flag && mClientLastDown.mIsOver)
				{
					mClientLastDown.MouseLeave();
					mClientLastDown.mIsOver = false;
				}
				CGMaths.CGPointTranslate(ref touch.location, b.mX, b.mY);
				CGMaths.CGPointTranslate(ref touch.previousLocation, b.mX, b.mY);
				mClientLastDown.TouchMoved(touch);
			}
		}

		public override void TouchEnded(_Touch touch)
		{
			if (mScrollTracking)
			{
				TouchMotion(touch);
				mScrollTracking = false;
				if (mPagingEnabled)
				{
					SnapToPage();
				}
			}
			else if (mClientLastDown != null)
			{
				CGPoint b = GetAbsPos() - mClientLastDown.GetAbsPos();
				CGPoint a = new CGPoint(touch.location.X, touch.location.Y);
				CGPoint cGPoint = a + b;
				CGMaths.CGPointTranslate(ref touch.location, b.mX, b.mY);
				CGMaths.CGPointTranslate(ref touch.previousLocation, b.mX, b.mY);
				mClientLastDown.TouchEnded(touch);
				mClientLastDown.mIsDown = false;
				mClientLastDown = null;
			}
		}

		public override void TouchesCanceled()
		{
			if (mClient != null && mClientLastDown != null && !mScrollTracking)
			{
				mClientLastDown.TouchesCanceled();
				mClientLastDown.mIsDown = false;
				mClientLastDown = null;
			}
			mScrollTracking = false;
		}

		public override void Update()
		{
			base.Update();
			DoScrollUpdate();
			DoScrollUpdate();
			DoScrollUpdate();
		}

		public void DoScrollUpdate()
		{
			if (!mVisible || mDisabled)
			{
				return;
			}
			if (mIsDown)
			{
				mIndicatorsFlashTimer = SCROLL_INDICATORS_FLASH_TICKS;
			}
			else
			{
				float num = Math.Min(0f, mScrollMin.x);
				float num2 = Math.Min(0f, mScrollMin.y);
				float x = mScrollMax.x;
				float y = mScrollMax.y;
				if (mSeekScrollTarget)
				{
					float num3 = CGMaths.CGVectorNorm(CGMaths.CGPointSubtract(mScrollTarget, mScrollOffset));
					if (num3 < 0.01f)
					{
						mScrollOffset = mScrollTarget;
						mSeekScrollTarget = false;
						if (mListener != null)
						{
							mListener.ScrollTargetReached(this);
						}
						if (mPagingEnabled && mPageControl != null)
						{
							mPageControl.SetCurrentPage(mCurrentPageHorizontal);
						}
					}
					else
					{
						num = (x = mScrollTarget.x);
						num2 = (y = mScrollTarget.y);
					}
				}
				float num4 = CGMaths.CGVectorNorm(mScrollVelocity);
				if (num4 < 0.0001f)
				{
					mScrollVelocity = CGMaths.CGPointMake(0f, 0f);
				}
				else
				{
					bool flag = mScrollOffset.x < num || mScrollOffset.x >= x;
					bool flag2 = mScrollOffset.y < num2 || mScrollOffset.y >= y;
					CGPoint multiplier = default(CGPoint);
					multiplier.x = (flag ? 0.85f : 0.975f);
					multiplier.y = (flag2 ? 0.85f : 0.975f);
					mScrollOffset = CGMaths.CGPointAddScaled(mScrollOffset, mScrollVelocity, 0.01f);
					mScrollVelocity = CGMaths.CGPointMultiply(mScrollVelocity, multiplier);
				}
				if (mScrollOffset.x < num)
				{
					if (mBounceEnabled || mSeekScrollTarget)
					{
						float num5 = (mSpringOverride == 0f) ? 0.1f : mSpringOverride;
						mScrollOffset.x += num5 * (num - mScrollOffset.x);
					}
					else
					{
						mScrollOffset.x = num;
						mScrollVelocity.x = 0f;
					}
				}
				else if (mScrollOffset.x > x)
				{
					if (mBounceEnabled || mSeekScrollTarget)
					{
						float num6 = (mSpringOverride == 0f) ? 0.1f : mSpringOverride;
						mScrollOffset.x += num6 * (x - mScrollOffset.x);
					}
					else
					{
						mScrollOffset.x = x;
						mScrollVelocity.x = 0f;
					}
				}
				if (mScrollOffset.y < num2)
				{
					if (mBounceEnabled || mSeekScrollTarget)
					{
						float num7 = (mSpringOverride == 0f) ? 0.1f : mSpringOverride;
						mScrollOffset.y += num7 * (num2 - mScrollOffset.y);
					}
					else
					{
						mScrollOffset.y = num2;
						mScrollVelocity.y = 0f;
					}
				}
				else if (mScrollOffset.y > y)
				{
					if (mBounceEnabled || mSeekScrollTarget)
					{
						float num8 = (mSpringOverride == 0f) ? 0.1f : mSpringOverride;
						mScrollOffset.y += num8 * (y - mScrollOffset.y);
					}
					else
					{
						mScrollOffset.y = y;
						mScrollVelocity.y = 0f;
					}
				}
				if (mClient != null)
				{
					mClient.Move((int)mScrollOffset.x, (int)mScrollOffset.y);
				}
				if (mIndicatorsFlashTimer > 0)
				{
					mIndicatorsFlashTimer--;
				}
			}
			if (mIndicatorsFlashTimer > 0 && mIndicatorsOpacity < 1f)
			{
				mIndicatorsOpacity = Math.Min(1f, mIndicatorsOpacity + SCROLL_INDICATORS_FADE_IN_RATE);
			}
			else if (mIndicatorsFlashTimer == 0 && mIndicatorsOpacity > 0f)
			{
				mIndicatorsOpacity = Math.Max(0f, mIndicatorsOpacity - SCROLL_INDICATORS_FADE_OUT_RATE);
			}
		}

		public static void DrawHorizontalStretchableImage(Graphics g, Image image, TRect destRect)
		{
			int width = image.GetWidth();
			int height = image.GetHeight();
			TRect theSrcRect = new TRect(0, 0, (width - 1) / 2, height);
			TRect theSrcRect2 = new TRect(theSrcRect.mWidth, 0, 1, height);
			TRect theSrcRect3 = new TRect(theSrcRect2.mX + theSrcRect2.mWidth, 0, width - theSrcRect.mWidth - theSrcRect2.mWidth, height);
			int theY = destRect.mY + (destRect.mHeight - height) / 2;
			TRect theDestRect = new TRect(destRect.mX + theSrcRect.mWidth, theY, destRect.mWidth - theSrcRect.mWidth - theSrcRect3.mWidth, height);
			g.DrawImage(image, destRect.mX, theY, theSrcRect);
			g.DrawImage(image, theDestRect, theSrcRect2);
			g.DrawImage(image, destRect.mX + destRect.mWidth - theSrcRect3.mWidth, theY, theSrcRect3);
		}

		public static void DrawVerticalStretchableImage(Graphics g, Image image, TRect destRect)
		{
			int width = image.GetWidth();
			int height = image.GetHeight();
			TRect theSrcRect = new TRect(0, 0, width, (height - 1) / 2);
			TRect theSrcRect2 = new TRect(0, theSrcRect.mHeight, width, 1);
			TRect theSrcRect3 = new TRect(0, theSrcRect2.mY + theSrcRect2.mHeight, width, height - theSrcRect.mHeight - theSrcRect2.mHeight);
			int theX = destRect.mX + (destRect.mWidth - width) / 2;
			TRect theDestRect = new TRect(theX, destRect.mY + theSrcRect.mHeight, width, destRect.mHeight - theSrcRect.mHeight - theSrcRect3.mHeight);
			g.DrawImage(image, theX, destRect.mY, theSrcRect);
			g.DrawImage(image, theDestRect, theSrcRect2);
			g.DrawImage(image, theX, destRect.mY + destRect.mHeight - theSrcRect3.mHeight, theSrcRect3);
		}

		public override void Draw(Graphics g)
		{
			base.Draw(g);
			if (mBackgroundImage != null)
			{
				g.DrawImage(mBackgroundImage, 0, 0);
			}
			else if (mFillBackground)
			{
				g.SetColor(GetColor(0));
				g.FillRect(0, 0, mWidth, mHeight);
			}
		}

		public void DrawProxyWidget(Graphics g, ProxyWidget proxyWidget)
		{
			Color color = new Color(255, 255, 255, (int)(255f * mIndicatorsOpacity));
			if (color.A != 0)
			{
				int width = mIndicatorsImage.GetWidth();
				int height = mIndicatorsImage.GetHeight();
				Insets insets = mIndicatorsInsets;
				g.SetColor(color);
				g.SetColorizeImages(true);
				if ((mScrollPractical & ScrollMode.SCROLL_HORIZONTAL) != 0)
				{
					float num = (float)mWidth / (float)mClient.Width();
					int num2 = mWidth - insets.mLeft - insets.mRight - (((mScrollMode & ScrollMode.SCROLL_VERTICAL) != 0) ? width : 0);
					int num3 = (int)((float)num2 * num);
					int num4 = num2 - num3;
					float num5 = Math.Min(0, mWidth - mClient.mWidth - mScrollInsets.mRight);
					float num6 = mScrollInsets.mLeft;
					float num7 = 1f - (mScrollOffset.x - num5) / (num6 - num5);
					int num8 = (int)((float)num4 * num7);
					int val = num8 + num3;
					num8 = Math.Min(Math.Max(0, num8), num2 - width);
					val = Math.Min(Math.Max(width, val), num2);
					TRect destRect = default(TRect);
					destRect.mX = insets.mLeft + num8;
					destRect.mY = mHeight - insets.mBottom - height;
					destRect.mWidth = val - num8;
					destRect.mHeight = height;
					DrawHorizontalStretchableImage(g, mIndicatorsImage, destRect);
				}
				if ((mScrollPractical & ScrollMode.SCROLL_VERTICAL) != 0)
				{
					float num9 = (float)mHeight / (float)mClient.Height();
					int num10 = mHeight - insets.mTop - insets.mBottom - (((mScrollMode & ScrollMode.SCROLL_HORIZONTAL) != 0) ? height : 0);
					int num11 = (int)((float)num10 * num9);
					int num12 = num10 - num11;
					float num13 = Math.Min(0, mHeight - mClient.mHeight - mScrollInsets.mBottom);
					float num14 = mScrollInsets.mTop;
					float num15 = 1f - (mScrollOffset.y - num13) / (num14 - num13);
					int num16 = (int)((float)num12 * num15);
					int val2 = num16 + num11;
					num16 = Math.Min(Math.Max(0, num16), num10 - height);
					val2 = Math.Min(Math.Max(height, val2), num10);
					TRect destRect2 = default(TRect);
					destRect2.mX = mWidth - insets.mRight - width;
					destRect2.mY = insets.mTop + num16;
					destRect2.mWidth = width;
					destRect2.mHeight = val2 - num16;
					DrawVerticalStretchableImage(g, mIndicatorsImage, destRect2);
				}
			}
			if (mDrawOverlays)
			{
				g.SetColorizeImages(false);
				foreach (Overlay mOverlay in mOverlays)
				{
					g.DrawImage(mOverlay.image, mOverlay.offset.mX, mOverlay.offset.mY);
				}
			}
		}

		protected void Init(ScrollWidgetListener listener)
		{
			mClient = null;
			mClientLastDown = null;
			mListener = listener;
			mPageControl = null;
			mIndicatorsProxy = null;
			mIndicatorsImage = null;
			mScrollMode = ScrollMode.SCROLL_VERTICAL;
			mScrollInsets = new Insets(0, 0, 0, 0);
			mScrollTracking = false;
			mSeekScrollTarget = false;
			mBounceEnabled = true;
			mPagingEnabled = false;
			mIndicatorsEnabled = false;
			mIndicatorsInsets = new Insets(0, 0, 0, 0);
			mIndicatorsFlashTimer = 0;
			mIndicatorsOpacity = 0f;
			mBackgroundImage = null;
			mFillBackground = false;
			mDrawOverlays = false;
			mScrollOffset = CGMaths.CGPointMake(0f, 0f);
			mScrollVelocity = CGMaths.CGPointMake(0f, 0f);
			mClip = true;
		}

		protected void SnapToPage()
		{
			CGPoint minuend = default(CGPoint);
			minuend.x = (float)mScrollInsets.mLeft + mPageSize.x / 2f;
			minuend.y = (float)mScrollInsets.mTop + mPageSize.y / 2f;
			CGPoint cGPoint = CGMaths.CGPointSubtract(minuend, mScrollOffset);
			int val = (int)Math.Floor(cGPoint.x / mPageSize.x);
			int val2 = (int)Math.Floor(cGPoint.y / mPageSize.y);
			val = Math.Max(0, Math.Min(val, mPageCountHorizontal - 1));
			val2 = Math.Max(0, Math.Min(val2, mPageCountVertical - 1));
			CGPoint cGPoint2 = default(CGPoint);
			cGPoint2.x = (float)mScrollInsets.mLeft - (float)val * mPageSize.x;
			cGPoint2.y = (float)mScrollInsets.mTop - (float)val2 * mPageSize.y;
			if (mScrollVelocity.x > 40f && cGPoint2.x < mScrollOffset.x)
			{
				val--;
			}
			else if (mScrollVelocity.x < -40f && cGPoint2.x > mScrollOffset.x)
			{
				val++;
			}
			if (mScrollVelocity.y > 40f && cGPoint2.y < mScrollOffset.y)
			{
				val2--;
			}
			else if (mScrollVelocity.y < -40f && cGPoint2.y > mScrollOffset.y)
			{
				val2++;
			}
			SetPage(val, val2, true);
		}

		protected void TouchMotion(_Touch touch)
		{
			CGPoint cGPoint = CGMaths.CGPointSubtract(touch.location, mScrollTouchReference);
			CGPoint cGPoint2 = mScrollOffset;
			if ((mScrollPractical & ScrollMode.SCROLL_HORIZONTAL) != 0)
			{
				cGPoint2.x = mScrollOffsetReference.x + cGPoint.X;
				float x = mScrollMin.x;
				float x2 = mScrollMax.x;
				if (cGPoint2.x < x)
				{
					cGPoint2.x = (mBounceEnabled ? (cGPoint2.x + 0.5f * (x - cGPoint2.x)) : x);
					mScrollVelocity.x = 0f;
				}
				else if (cGPoint2.x > x2)
				{
					cGPoint2.x = (mBounceEnabled ? (cGPoint2.x + 0.5f * (x2 - cGPoint2.x)) : x2);
					mScrollVelocity.x = 0f;
				}
				else
				{
					float num = cGPoint2.x - mScrollOffset.x;
					double num2 = touch.timestamp - mScrollLastTimestamp;
					if (num2 > 0.0)
					{
						double num3 = (double)num / num2;
						double num4 = Math.Min(1.0, num2 / 0.10000000149011612);
						mScrollVelocity.x = (float)(num4 * num3 + (1.0 - num4) * (double)mScrollVelocity.x);
					}
				}
			}
			if ((mScrollPractical & ScrollMode.SCROLL_VERTICAL) != 0)
			{
				cGPoint2.y = mScrollOffsetReference.y + cGPoint.Y;
				float y = mScrollMin.y;
				float y2 = mScrollMax.y;
				if (cGPoint2.y < y)
				{
					cGPoint2.y = (mBounceEnabled ? (cGPoint2.y + 0.5f * (y - cGPoint2.y)) : y);
					mScrollVelocity.y = 0f;
				}
				else if (cGPoint2.y > y2)
				{
					cGPoint2.y = (mBounceEnabled ? (cGPoint2.y + 0.5f * (y2 - cGPoint2.y)) : y2);
					mScrollVelocity.y = 0f;
				}
				else
				{
					float num5 = cGPoint2.y - mScrollOffset.mY;
					double num6 = touch.timestamp - mScrollLastTimestamp;
					if (num6 > 0.0)
					{
						double num7 = (double)num5 / num6;
						double num8 = Math.Min(1.0, num6 / 0.10000000149011612);
						mScrollVelocity.y = (float)(num8 * num7 + (1.0 - num8) * (double)mScrollVelocity.y);
					}
				}
			}
			mScrollOffset = cGPoint2;
			mScrollLastTimestamp = touch.timestamp;
			mClient.Move((int)mScrollOffset.x, (int)mScrollOffset.y);
			oldTouch = touch.location;
			oldTouchTime = touch.timestamp;
		}

		protected Widget GetClientWidgetAt(_Touch touch)
		{
			int num = (int)touch.location.X - mClient.mX;
			int num2 = (int)touch.location.Y - mClient.mY;
			int theFlags = 0x10 | mWidgetManager.GetWidgetFlags();
			Widget widgetAtHelper;
			int theWidgetX;
			int theWidgetY;
			if (mClientLastDown != null)
			{
				CGPoint absPos = mClient.GetAbsPos();
				CGPoint absPos2 = mClientLastDown.GetAbsPos();
				widgetAtHelper = mClientLastDown;
				theWidgetX = (int)(touch.location.X + absPos.mX - absPos2.mX);
				theWidgetY = (int)(touch.location.Y + absPos.mY - absPos2.mY);
			}
			else
			{
				mClient.mWidgetFlagsMod.mRemoveFlags &= -17;
				bool found;
				widgetAtHelper = mClient.GetWidgetAtHelper(num, num2, theFlags, out found, out theWidgetX, out theWidgetY);
				mClient.mWidgetFlagsMod.mRemoveFlags |= 16;
			}
			if (widgetAtHelper == null || widgetAtHelper.mDisabled)
			{
				theWidgetX = num;
				theWidgetY = num2;
				widgetAtHelper = mClient;
			}
			touch.previousLocation.X += (float)theWidgetX - touch.location.X;
			touch.previousLocation.Y += (float)theWidgetY - touch.location.Y;
			touch.location.X = theWidgetX;
			touch.location.Y = theWidgetY;
			return widgetAtHelper;
		}

		public CGPoint GetScrollOffset()
		{
			return mScrollOffset;
		}

		public void SetScrollOffset(float x, float y)
		{
			mScrollOffset.x = x;
			mScrollOffset.y = y;
		}

		protected void CacheDerivedValues()
		{
			if (mClient != null)
			{
				mScrollMin.x = mWidth - mClient.mWidth - mScrollInsets.mRight;
				mScrollMin.y = mHeight - mClient.mHeight - mScrollInsets.mBottom;
				mScrollMax.x = mScrollInsets.mLeft;
				mScrollMax.y = mScrollInsets.mTop;
				int num = ((mScrollMin.x < mScrollMax.x) ? 1 : 0) | ((mScrollMin.y < mScrollMax.y) ? 2 : 0);
				mScrollPractical = (ScrollMode)((int)mScrollMode & num);
			}
			else
			{
				ref CGPoint reference = ref mScrollMin;
				ref CGPoint reference2 = ref mScrollMax;
				ref CGPoint reference3 = ref mScrollMin;
				float num3 = mScrollMax.y = 0f;
				float num5 = reference3.y = num3;
				float num8 = reference.x = (reference2.x = num5);
				mScrollPractical = ScrollMode.SCROLL_DISABLED;
			}
			if (mPagingEnabled)
			{
				mPageSize.x = mWidth - mScrollInsets.mLeft - mScrollInsets.mRight;
				mPageSize.y = mHeight - mScrollInsets.mTop - mScrollInsets.mBottom;
				if (mClient != null)
				{
					mPageCountHorizontal = (int)Math.Floor((float)mClient.Width() / mPageSize.x);
					mPageCountVertical = (int)Math.Floor((float)mClient.Height() / mPageSize.y);
				}
				else
				{
					mPageCountHorizontal = (mPageCountVertical = 0);
				}
			}
		}
	}
}
