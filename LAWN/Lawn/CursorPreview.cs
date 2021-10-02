using Sexy;

namespace Lawn
{
	internal class CursorPreview : GameObject
	{
		public int mGridX;

		public int mGridY;

		public CursorPreview()
		{
			mX = 0;
			mY = 0;
			mWidth = 80;
			mHeight = 80;
			mGridX = 0;
			mGridY = 0;
			mVisible = false;
		}

		public void Update()
		{
		}

		public override bool LoadFromFile(Buffer b)
		{
			return false;
		}

		public override bool SaveToFile(Buffer b)
		{
			return false;
		}

		public void Draw(Graphics g)
		{
			SeedType seedTypeInCursor = mBoard.GetSeedTypeInCursor();
			int num = -1;
		}
	}
}
