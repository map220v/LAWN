using System.Collections.Generic;

namespace Sexy
{
	internal class WidgetContainer
	{
		public LinkedList<Widget> mWidgets = new LinkedList<Widget>();

		public WidgetManager mWidgetManager;

		public WidgetContainer mParent;

		public bool mUpdateIteratorModified;

		public LinkedListNode<Widget> mUpdateIterator;

		public int mLastWMUpdateCount;

		public int mUpdateCnt;

		public int mX;

		public int mY;

		public int mWidth;

		public int mHeight;

		public bool mHasAlpha;

		public bool mClip;

		public FlagsMod mWidgetFlagsMod = default(FlagsMod);

		public int mPriority;

		public int mZOrder;

		public TRect FullRect;

		public Widget GetWidgetAtHelper(int x, int y, int theFlags, out bool found, out int theWidgetX, out int theWidgetY)
		{
			bool flag = false;
			GlobalMembersFlags.ModFlags(ref theFlags, mWidgetFlagsMod);
			for (LinkedListNode<Widget> linkedListNode = mWidgets.Last; linkedListNode != null; linkedListNode = linkedListNode.Previous)
			{
				Widget value = linkedListNode.Value;
				int theFlags2 = theFlags;
				GlobalMembersFlags.ModFlags(ref theFlags2, value.mWidgetFlagsMod);
				if (flag)
				{
					GlobalMembersFlags.ModFlags(ref theFlags2, mWidgetManager.mBelowModalFlagsMod);
				}
				if ((theFlags2 & 0x10) != 0 && value.mVisible)
				{
					bool found2;
					Widget widgetAtHelper = value.GetWidgetAtHelper(x - value.mX, y - value.mY, theFlags2, out found2, out theWidgetX, out theWidgetY);
					if (widgetAtHelper != null || found2)
					{
						found = true;
						return widgetAtHelper;
					}
					if (value.mMouseVisible && (value.GetInsetRect().Contains(x, y) || value.FullRect.Contains(x, y)))
					{
						found = true;
						if (value.IsPointVisible(x - value.mX, y - value.mY))
						{
							if (theWidgetX != 0)
							{
								theWidgetX = x - value.mX;
							}
							if (theWidgetY != 0)
							{
								theWidgetY = y - value.mY;
							}
							return value;
						}
					}
				}
				flag |= (value == mWidgetManager.mBaseModalWidget);
			}
			found = false;
			theWidgetX = (theWidgetY = 0);
			return null;
		}

		public bool IsBelowHelper(Widget theWidget1, Widget theWidget2, ref bool found)
		{
			foreach (Widget mWidget in mWidgets)
			{
				if (mWidget == theWidget1)
				{
					found = true;
					return true;
				}
				if (mWidget == theWidget2)
				{
					found = true;
					return false;
				}
				bool result = mWidget.IsBelowHelper(theWidget1, theWidget2, ref found);
				if (found)
				{
					return result;
				}
			}
			return false;
		}

		public void InsertWidgetHelper(LinkedList<Widget>.Enumerator where, Widget theWidget)
		{
			LinkedListNode<Widget> anItr = mWidgets.Find(where.Current);
			InsertWidgetHelper(anItr, theWidget);
		}

		public void InsertWidgetHelper(LinkedListNode<Widget> anItr, Widget theWidget)
		{
			while (anItr != null)
			{
				Widget value = anItr.Value;
				if (value.mZOrder >= theWidget.mZOrder)
				{
					if (anItr.Value != mWidgets.First.Value)
					{
						value = anItr.Value;
						if (value.mZOrder > theWidget.mZOrder)
						{
							break;
						}
					}
					mWidgets.AddAfter(anItr, theWidget);
					return;
				}
				anItr = anItr.Next;
			}
			while (anItr != null && anItr.Value != mWidgets.First.Value)
			{
				anItr = anItr.Previous;
				Widget value2 = anItr.Value;
				if (value2.mZOrder <= theWidget.mZOrder)
				{
					anItr = anItr.Next;
					mWidgets.AddBefore(anItr, theWidget);
					return;
				}
			}
			mWidgets.AddLast(theWidget);
		}

		public WidgetContainer()
		{
			mX = 0;
			mY = 0;
			mWidth = 0;
			mHeight = 0;
			mParent = null;
			mWidgetManager = null;
			mUpdateIteratorModified = false;
			mUpdateIterator = mWidgets.Last;
			mLastWMUpdateCount = 0;
			mUpdateCnt = 0;
			mHasAlpha = false;
			mClip = true;
			mPriority = 0;
			mZOrder = 0;
		}

		public virtual void Dispose()
		{
			Debug.ASSERT(mParent == null);
			Debug.ASSERT(mWidgets.Count == 0);
		}

		public virtual TRect GetRect()
		{
			return new TRect(mX, mY, mWidth, mHeight);
		}

		public virtual bool Intersects(WidgetContainer theWidget)
		{
			return GetRect().Intersects(theWidget.GetRect());
		}

		public virtual void AddWidget(Widget theWidget)
		{
			if (!mWidgets.Contains(theWidget))
			{
				InsertWidgetHelper(mWidgets.Last, theWidget);
				theWidget.mWidgetManager = mWidgetManager;
				theWidget.mParent = this;
				if (mWidgetManager != null)
				{
					theWidget.AddedToManager(mWidgetManager);
					theWidget.MarkDirtyFull();
					mWidgetManager.RehupMouse();
				}
				MarkDirty();
			}
		}

		public virtual void RemoveWidget(Widget theWidget)
		{
			LinkedListNode<Widget> linkedListNode = mWidgets.Find(theWidget);
			if (linkedListNode != null)
			{
				theWidget.WidgetRemovedHelper();
				theWidget.mParent = null;
				bool flag = linkedListNode.Value == ((mUpdateIterator != null) ? mUpdateIterator.Value : null);
				LinkedListNode<Widget> next = linkedListNode.Next;
				mWidgets.Remove(linkedListNode);
				if (flag)
				{
					mUpdateIterator = next;
					mUpdateIteratorModified = true;
				}
			}
		}

		public virtual bool HasWidget(Widget theWidget)
		{
			return mWidgets.Count > 0;
		}

		public virtual void DisableWidget(Widget theWidget)
		{
		}

		public virtual void RemoveAllWidgets(bool doDelete)
		{
			RemoveAllWidgets(doDelete, false);
		}

		public virtual void RemoveAllWidgets()
		{
			RemoveAllWidgets(false, false);
		}

		public virtual void RemoveAllWidgets(bool doDelete, bool recursive)
		{
			while (mWidgets.Count != 0)
			{
				Widget value = mWidgets.First.Value;
				RemoveWidget(value);
				if (recursive)
				{
					value.RemoveAllWidgets(doDelete, recursive);
				}
				if (doDelete)
				{
					value.Dispose();
				}
			}
		}

		public virtual void SetFocus(Widget theWidget)
		{
		}

		public virtual bool IsBelow(Widget theWidget1, Widget theWidget2)
		{
			bool found = false;
			return IsBelowHelper(theWidget1, theWidget2, ref found);
		}

		public virtual void MarkAllDirty()
		{
		}

		public virtual void BringToFront(Widget theWidget)
		{
			LinkedListNode<Widget> linkedListNode = mWidgets.Find(theWidget);
			if (linkedListNode != null)
			{
				if (linkedListNode == mUpdateIterator)
				{
					mUpdateIteratorModified = true;
				}
				mWidgets.Remove(linkedListNode);
				InsertWidgetHelper(mWidgets.Last, theWidget);
				theWidget.OrderInManagerChanged();
			}
		}

		public virtual void BringToBack(Widget theWidget)
		{
			LinkedListNode<Widget> linkedListNode = mWidgets.Find(theWidget);
			if (linkedListNode != null)
			{
				if (linkedListNode == mUpdateIterator)
				{
					mUpdateIteratorModified = true;
				}
				mWidgets.Remove(linkedListNode);
				InsertWidgetHelper(mWidgets.First, theWidget);
				theWidget.OrderInManagerChanged();
			}
		}

		public virtual void PutBehind(Widget theWidget, Widget theRefWidget)
		{
			LinkedListNode<Widget> linkedListNode = mWidgets.Find(theWidget);
			if (linkedListNode != null)
			{
				if (linkedListNode == mUpdateIterator)
				{
					mUpdateIteratorModified = true;
				}
				mWidgets.Remove(linkedListNode);
				linkedListNode = mWidgets.Find(theRefWidget);
				InsertWidgetHelper(linkedListNode, theWidget);
				theWidget.OrderInManagerChanged();
			}
		}

		public virtual void PutInfront(Widget theWidget, Widget theRefWidget)
		{
			LinkedListNode<Widget> linkedListNode = mWidgets.Find(theWidget);
			if (linkedListNode != null)
			{
				if (linkedListNode == mUpdateIterator)
				{
					mUpdateIteratorModified = true;
				}
				mWidgets.Remove(linkedListNode);
				linkedListNode = mWidgets.Find(theRefWidget);
				linkedListNode = linkedListNode.Next;
				InsertWidgetHelper(linkedListNode, theWidget);
				theWidget.OrderInManagerChanged();
			}
		}

		public virtual CGPoint GetAbsPos()
		{
			if (mParent == null)
			{
				return new CGPoint(mX, mY);
			}
			return new CGPoint(mX, mY) + mParent.GetAbsPos();
		}

		public virtual void MarkDirty()
		{
		}

		public virtual void MarkDirtyFull()
		{
		}

		public virtual void MarkDirtyFull(WidgetContainer theWidget)
		{
		}

		public virtual void MarkDirty(WidgetContainer theWidget)
		{
		}

		public virtual void AddedToManager(WidgetManager theWidgetManager)
		{
			foreach (Widget mWidget in mWidgets)
			{
				mWidget.mWidgetManager = theWidgetManager;
				mWidget.AddedToManager(theWidgetManager);
				MarkDirty();
			}
		}

		public virtual void RemovedFromManager(WidgetManager theWidgetManager)
		{
			foreach (Widget mWidget in mWidgets)
			{
				theWidgetManager.DisableWidget(mWidget);
				mWidget.RemovedFromManager(theWidgetManager);
				mWidget.mWidgetManager = null;
			}
		}

		public virtual bool BackButtonPress()
		{
			return false;
		}

		public virtual void Update()
		{
			mUpdateCnt++;
		}

		public virtual void UpdateAll(ModalFlags theFlags)
		{
			new AutoModalFlags(theFlags, mWidgetFlagsMod);
			if ((theFlags.GetFlags() & 2) != 0)
			{
				MarkDirty();
			}
			WidgetManager widgetManager = mWidgetManager;
			if (widgetManager == null)
			{
				return;
			}
			if ((theFlags.GetFlags() & 1) != 0 && mLastWMUpdateCount != mWidgetManager.mUpdateCnt)
			{
				mLastWMUpdateCount = mWidgetManager.mUpdateCnt;
				Update();
			}
			mUpdateIterator = mWidgets.First;
			while (mUpdateIterator != null)
			{
				mUpdateIteratorModified = false;
				Widget value = mUpdateIterator.Value;
				if (value == widgetManager.mBaseModalWidget)
				{
					theFlags.mIsOver = true;
				}
				value.UpdateAll(theFlags);
				if (!mUpdateIteratorModified)
				{
					mUpdateIterator = mUpdateIterator.Next;
				}
			}
			mUpdateIteratorModified = true;
		}

		public virtual void UpdateF(float theFrac)
		{
		}

		public void UpdateFAll(ModalFlags theFlags, float theFrac)
		{
			new AutoModalFlags(theFlags, mWidgetFlagsMod);
			if ((theFlags.GetFlags() & 1) != 0)
			{
				UpdateF(theFrac);
			}
			mUpdateIterator = mWidgets.First;
			while (mUpdateIterator.Value != null)
			{
				mUpdateIteratorModified = false;
				Widget value = mUpdateIterator.Value;
				if (value == mWidgetManager.mBaseModalWidget)
				{
					theFlags.mIsOver = true;
				}
				value.UpdateFAll(theFlags, theFrac);
				if (!mUpdateIteratorModified)
				{
					mUpdateIterator = mUpdateIterator.Next;
				}
			}
			mUpdateIteratorModified = true;
		}

		public virtual void Draw(Graphics g)
		{
		}

		public void DrawAll(ModalFlags theFlags, Graphics g)
		{
			if (mWidgetManager != null && mPriority > mWidgetManager.mMinDeferredOverlayPriority)
			{
				mWidgetManager.FlushDeferredOverlayWidgets(mPriority);
			}
			new AutoModalFlags(theFlags, mWidgetFlagsMod);
			if (mClip && (theFlags.GetFlags() & 8) != 0)
			{
				g.ClipRect(0, 0, mWidth, mHeight);
			}
			if (mWidgets.Count == 0)
			{
				if ((theFlags.GetFlags() & 4) != 0)
				{
					Draw(g);
				}
				return;
			}
			if ((theFlags.GetFlags() & 4) != 0)
			{
				g.PushState();
				Draw(g);
				g.PopState();
			}
			for (LinkedListNode<Widget> linkedListNode = mWidgets.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				if (linkedListNode.Value.mVisible)
				{
					if (mWidgetManager != null && linkedListNode.Value == mWidgetManager.mBaseModalWidget)
					{
						theFlags.mIsOver = true;
					}
					Graphics @new = Graphics.GetNew(g);
					@new.Translate(linkedListNode.Value.mX, linkedListNode.Value.mY);
					linkedListNode.Value.DrawAll(theFlags, @new);
					@new.PrepareForReuse();
				}
			}
		}

		public void SysColorChanged()
		{
		}

		public void SysColorChangedAll()
		{
			SysColorChanged();
			foreach (Widget mWidget in mWidgets)
			{
				mWidget.SysColorChangedAll();
			}
		}

		public virtual void InterfaceOrientationChanged(UI_ORIENTATION toOrientation)
		{
			foreach (Widget mWidget in mWidgets)
			{
				mWidget.InterfaceOrientationChanged(toOrientation);
			}
		}

		public void KeyboardWillShow(ref TRect theRect)
		{
			foreach (Widget mWidget in mWidgets)
			{
				mWidget.KeyboardWillShow(ref theRect);
			}
		}

		public void KeyboardDidShow(ref TRect theRect)
		{
			foreach (Widget mWidget in mWidgets)
			{
				mWidget.KeyboardDidShow(ref theRect);
			}
		}

		public void KeyboardWillHide(ref TRect theRect)
		{
			foreach (Widget mWidget in mWidgets)
			{
				mWidget.KeyboardWillHide(ref theRect);
			}
		}

		public void KeyboardDidHide(ref TRect theRect)
		{
			foreach (Widget mWidget in mWidgets)
			{
				mWidget.KeyboardDidHide(ref theRect);
			}
		}

		protected int S(int i)
		{
			return (int)((float)i * Constants.S);
		}

		protected int M(int i)
		{
			return i;
		}

		protected int M1(int i)
		{
			return i;
		}

		protected int M2(int i)
		{
			return i;
		}

		protected int M3(int i)
		{
			return i;
		}
	}
}
