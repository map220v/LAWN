using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace Sexy
{
	internal class InputController
	{
		public static Vector2 touchPos = Vector2.Zero;

		public static void TestTouchCaps()
		{
			TouchPanelCapabilities capabilities = TouchPanel.GetCapabilities();
			if (capabilities.IsConnected)
			{
				int maximumTouchCount = capabilities.MaximumTouchCount;
				Debug.OutputDebug("maxPoints", maximumTouchCount);
			}
		}

		public static void HandleTouchInput()
		{
		}
	}
}
