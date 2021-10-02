using Sexy;
using Sexy.TodLib;

namespace Lawn
{
	internal class DescriptionWidget : Widget
	{
		public int SCROLLBAR_PAD = Constants.DescriptionWidget_ScrollBar_Padding;

		private string mText = string.Empty;

		public DescriptionWidget()
		{
			mHeight = 0;
		}

		public void SetText(ref string theText)
		{
			mText = theText;
			Graphics @new = Graphics.GetNew();
			@new.SetFont(Resources.FONT_BRIANNETOD12);
			mHeight = TodStringFile.TodDrawStringWrappedHeight(@new, mText, new TRect(0, 0, mWidth - SCROLLBAR_PAD, 0), Resources.FONT_BRIANNETOD12, new SexyColor(40, 50, 90), DrawStringJustification.DS_ALIGN_LEFT);
			@new.PrepareForReuse();
		}

		public override void Draw(Graphics g)
		{
			g.HardwareClip();
			TodStringFile.TodDrawStringWrapped(g, mText, new TRect(0, 0, mWidth - SCROLLBAR_PAD, mHeight), Resources.FONT_BRIANNETOD12, new SexyColor(40, 50, 90), DrawStringJustification.DS_ALIGN_LEFT);
			g.EndHardwareClip();
		}
	}
}
