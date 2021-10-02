using Sexy;

namespace Lawn
{
	internal class LawnEditWidget : EditWidget
	{
		public Dialog mDialog;

		public bool mAutoCapFirstLetter;

		public LawnEditWidget(int theId, EditListener theEditListener, Dialog theDialog, string title, string description)
			: base(theId, theEditListener, title, description)
		{
			mDialog = theDialog;
			mAutoCapFirstLetter = true;
		}

		public override void KeyDown(KeyCode theKey)
		{
			base.KeyDown(theKey);
			if (theKey == KeyCode.KEYCODE_ESCAPE)
			{
				mDialog.KeyDown(theKey);
			}
		}

		public override void KeyChar(SexyChar theChar)
		{
			if (mAutoCapFirstLetter && char.IsLetter(theChar.value_type))
			{
				theChar.value_type = char.ToUpper(theChar.value_type);
				mAutoCapFirstLetter = false;
			}
			base.KeyChar(theChar);
		}
	}
}
