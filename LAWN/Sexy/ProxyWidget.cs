namespace Sexy
{
	internal class ProxyWidget : Widget
	{
		private ProxyWidgetListener mListener;

		public ProxyWidget(ProxyWidgetListener listener)
		{
			mListener = listener;
		}

		public override void Draw(Graphics g)
		{
			if (mListener != null)
			{
				mListener.DrawProxyWidget(g, this);
			}
		}
	}
}
