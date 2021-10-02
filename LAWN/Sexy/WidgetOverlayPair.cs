using System.Collections.Generic;

namespace Sexy
{
	internal class WidgetOverlayPair
	{
		public Widget aWidget;

		public int aPriority;

		private static Stack<WidgetOverlayPair> unusedObjects = new Stack<WidgetOverlayPair>(10);

		public static WidgetOverlayPair GetNewWidgetOverlayPair(Widget w, int p)
		{
			if (unusedObjects.Count > 0)
			{
				WidgetOverlayPair widgetOverlayPair = unusedObjects.Pop();
				widgetOverlayPair.Reset(w, p);
				return widgetOverlayPair;
			}
			return new WidgetOverlayPair(w, p);
		}

		public void PrepareForReuse()
		{
			unusedObjects.Push(this);
		}

		private WidgetOverlayPair(Widget w, int p)
		{
			Reset(w, p);
		}

		private void Reset(Widget w, int p)
		{
			aWidget = w;
			aPriority = p;
		}

		public void Clear()
		{
			aWidget = null;
			aPriority = 0;
		}

		public override int GetHashCode()
		{
			return aPriority;
		}

		public override bool Equals(object obj)
		{
			WidgetOverlayPair widgetOverlayPair = obj as WidgetOverlayPair;
			if (widgetOverlayPair == null)
			{
				return false;
			}
			return this == widgetOverlayPair;
		}
	}
}
