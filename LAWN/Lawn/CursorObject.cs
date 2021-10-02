using Sexy;
using Sexy.TodLib;

namespace Lawn
{
	internal class CursorObject : GameObject
	{
		public int mSeedBankIndex;

		public SeedType mType;

		public SeedType mImitaterType;

		public CursorType mCursorType;

		public Coin mCoinID;

		public Plant mGlovePlantID;

		public Plant mDuplicatorPlantID;

		public Plant mCobCannonPlantID;

		public int mHammerDownCounter;

		public Reanimation mReanimCursorID;

		public CursorObject()
		{
			mType = SeedType.SEED_NONE;
			mImitaterType = SeedType.SEED_NONE;
			mSeedBankIndex = -1;
			mX = 0;
			mY = 0;
			mCursorType = CursorType.CURSOR_TYPE_NORMAL;
			mCoinID = null;
			mDuplicatorPlantID = null;
			mCobCannonPlantID = null;
			mGlovePlantID = null;
			mReanimCursorID = null;
			mPosScaled = false;
			if (mApp.IsWhackAZombieLevel())
			{
				ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_HAMMER, true);
				Reanimation reanimation = mApp.AddReanimation(-25f, 16f, 0, ReanimationType.REANIM_HAMMER);
				reanimation.mIsAttachment = true;
				reanimation.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_anim_whack_zombie, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 0, 24f);
				reanimation.mAnimTime = 1f;
				mReanimCursorID = mApp.ReanimationGetID(reanimation);
			}
			mWidth = 80;
			mHeight = 80;
		}

		public void Update()
		{
			if (mApp.mGameScene != GameScenes.SCENE_PLAYING && !mBoard.mCutScene.IsInShovelTutorial())
			{
				mVisible = false;
				return;
			}
			if (!mApp.mWidgetManager.mMouseIn)
			{
				mVisible = false;
				return;
			}
			Reanimation reanimation = mApp.ReanimationTryToGet(mReanimCursorID);
			if (reanimation != null)
			{
				reanimation.Update();
			}
			mVisible = true;
			mX = mBoard.mLastToolX;
			mY = mBoard.mLastToolY;
		}

		public override bool LoadFromFile(Buffer b)
		{
			base.LoadFromFile(b);
			mCursorType = (CursorType)b.ReadLong();
			mHammerDownCounter = b.ReadLong();
			mImitaterType = (SeedType)b.ReadLong();
			mSeedBankIndex = b.ReadLong();
			mType = (SeedType)b.ReadLong();
			return true;
		}

		public override bool SaveToFile(Buffer b)
		{
			base.SaveToFile(b);
			b.WriteLong((int)mCursorType);
			b.WriteLong(mHammerDownCounter);
			b.WriteLong((int)mImitaterType);
			b.WriteLong(mSeedBankIndex);
			b.WriteLong((int)mType);
			return true;
		}

		public void DrawGroundLayer(Graphics g)
		{
			if (mCursorType == CursorType.CURSOR_TYPE_NORMAL)
			{
				return;
			}
			int theX = (int)((float)mX * Constants.IS);
			int theY = (int)((float)mY * Constants.IS);
			int num;
			int num2;
			if (mCursorType == CursorType.CURSOR_TYPE_PLANT_FROM_BANK)
			{
				if (mBoard.mIgnoreMouseUp)
				{
					return;
				}
				num = mBoard.PlantingPixelToGridX(theX, theY, mType);
				num2 = mBoard.PlantingPixelToGridY(theX, theY, mType);
				if (mBoard.CanPlantAt(num, num2, mType) != 0)
				{
					return;
				}
			}
			else
			{
				num = mBoard.PixelToGridX(theX, theY);
				num2 = mBoard.PixelToGridY(theX, theY);
				if (mCursorType == CursorType.CURSOR_TYPE_SHOVEL && (mBoard.mIgnoreMouseUp || mBoard.ToolHitTest(mX, mY, false).mObjectType == GameObjectType.OBJECT_TYPE_NONE))
				{
					return;
				}
			}
			if (num2 < 0 || num < 0 || mBoard.GridToPixelY(num, num2) < 0)
			{
				return;
			}
			Graphics @new = Graphics.GetNew();
			@new.mTransX = Constants.Board_Offset_AspectRatio_Correction;
			for (int i = 0; i < Constants.GRIDSIZEX; i++)
			{
				mBoard.DrawCelHighlight(@new, i, num2);
			}
			int num3 = mBoard.StageHas6Rows() ? 6 : 5;
			for (int j = 0; j < num3; j++)
			{
				if (j != num2)
				{
					mBoard.DrawCelHighlight(@new, num, j);
				}
			}
			@new.PrepareForReuse();
		}

		public void DrawTopLayer(Graphics g)
		{
			if (mCursorType == CursorType.CURSOR_TYPE_SHOVEL)
			{
				if (!mBoard.mIgnoreMouseUp && !((float)mX * Constants.IS < (float)Constants.LAWN_XMIN) && !((float)mY * Constants.IS < (float)Constants.LAWN_YMIN))
				{
					int mWidth = AtlasResources.IMAGE_SHOVEL_HI_RES.mWidth;
					int mHeight = AtlasResources.IMAGE_SHOVEL_HI_RES.mHeight;
					g.SetColor(SexyColor.White);
					int num = (int)TodCommon.TodAnimateCurveFloat(0, 39, mBoard.mEffectCounter % 40, 0f, mWidth / 2, TodCurves.CURVE_BOUNCE_SLOW_MIDDLE);
					g.DrawImage(AtlasResources.IMAGE_SHOVEL_HI_RES, num, -mHeight - num, mWidth, mHeight);
				}
			}
			else if (mCursorType == CursorType.CURSOR_TYPE_HAMMER)
			{
				Reanimation reanimation = mApp.ReanimationGet(mReanimCursorID);
				reanimation.Draw(g);
			}
			else if (mCursorType == CursorType.CURSOR_TYPE_COBCANNON_TARGET)
			{
				if (!((float)mX * Constants.IS < (float)Constants.LAWN_XMIN) && !((float)mY * Constants.IS < (float)Constants.LAWN_YMIN))
				{
					int mWidth2 = AtlasResources.IMAGE_COBCANNON_TARGET.mWidth;
					int mHeight2 = AtlasResources.IMAGE_COBCANNON_TARGET.mHeight;
					g.SetColorizeImages(true);
					g.SetColor(new SexyColor(255, 255, 255, 127));
					g.DrawImage(AtlasResources.IMAGE_COBCANNON_TARGET, -mWidth2 / 2, -mHeight2 / 2, mWidth2, mHeight2);
					g.SetColorizeImages(false);
				}
			}
			else if (mCursorType == CursorType.CURSOR_TYPE_WATERING_CAN && mApp.mPlayerInfo.mPurchases[13] > 0 && new TRect(Constants.ZEN_XMIN, Constants.ZEN_YMIN, Constants.ZEN_XMAX - Constants.ZEN_XMIN, Constants.ZEN_YMAX - Constants.ZEN_YMIN).Contains(mApp.mBoard.mLastToolX, mApp.mBoard.mLastToolY))
			{
				int mWidth3 = AtlasResources.IMAGE_ZEN_GOLDTOOLRETICLE.mWidth;
				int mHeight3 = AtlasResources.IMAGE_ZEN_GOLDTOOLRETICLE.mHeight;
				g.DrawImage(AtlasResources.IMAGE_ZEN_GOLDTOOLRETICLE, -mWidth3 / 2, -mHeight3 / 2, mWidth3, mHeight3);
			}
		}

		public void Die()
		{
			mApp.RemoveReanimation(ref mReanimCursorID);
		}
	}
}
