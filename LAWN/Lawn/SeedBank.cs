using Sexy;
using Sexy.TodLib;
using System;

namespace Lawn
{
	internal class SeedBank : GameObject
	{
		public int mNumPackets;

		public SeedPacket[] mSeedPackets = new SeedPacket[9];

		public int mCutSceneDarken;

		public int mConveyorBeltCounter;

		public SeedBank()
		{
			mWidth = (int)Constants.InvertAndScale(62f);
			mHeight = mApp.mHeight;
			mNumPackets = 0;
			mCutSceneDarken = 255;
			mConveyorBeltCounter = 0;
			mPosScaled = false;
			for (int i = 0; i < mSeedPackets.Length; i++)
			{
				mSeedPackets[i] = new SeedPacket();
			}
		}

		public void Draw(Graphics g)
		{
			if (mBoard.mCutScene != null && mBoard.mCutScene.IsBeforePreloading())
			{
				return;
			}
			if (mApp.mGameScene != GameScenes.SCENE_PLAYING)
			{
				g.mTransX -= mBoard.mX;
				g.mTransY -= mBoard.mY;
			}
			else
			{
				g.mTransX -= Constants.Board_Offset_AspectRatio_Correction;
			}
			if (!mApp.IsSlotMachineLevel() && mBoard.HasConveyorBeltSeedBank())
			{
				for (int i = -(mConveyorBeltCounter / 4) % Resources.IMAGE_CONVEYORBELT_BELT.mHeight; i < mHeight; i += Resources.IMAGE_CONVEYORBELT_BELT.mHeight)
				{
					g.DrawImage(Resources.IMAGE_CONVEYORBELT_BELT, 0, i);
				}
				g.DrawImage(Resources.IMAGE_CONVEYORBELT_BACKDROP, 0, 0);
				g.SetClipRect(ref Constants.ConveyorBeltClipRect);
			}
			for (int j = 0; j < mNumPackets; j++)
			{
				SeedPacket seedPacket = mSeedPackets[j];
				if (seedPacket.mPacketType != SeedType.SEED_NONE && seedPacket.BeginDraw(g))
				{
					seedPacket.DrawBackground(g);
					seedPacket.EndDraw(g);
				}
			}
			for (int k = 0; k < mNumPackets; k++)
			{
				SeedPacket seedPacket2 = mSeedPackets[k];
				if (seedPacket2.mPacketType != SeedType.SEED_NONE && seedPacket2.BeginDraw(g))
				{
					seedPacket2.Draw(g);
					seedPacket2.EndDraw(g);
				}
			}
			for (int l = 0; l < mNumPackets; l++)
			{
				SeedPacket seedPacket3 = mSeedPackets[l];
				if (seedPacket3.mPacketType != SeedType.SEED_NONE && seedPacket3.BeginDraw(g))
				{
					seedPacket3.DrawOverlay(g);
					seedPacket3.EndDraw(g);
				}
			}
			g.ClearClipRect();
			mApp.IsSlotMachineLevel();
			if (mApp.mGameScene != GameScenes.SCENE_PLAYING)
			{
				g.mTransX += mBoard.mX;
				g.mTransY += mBoard.mY;
			}
		}

		public override bool SaveToFile(Sexy.Buffer b)
		{
			try
			{
				base.SaveToFile(b);
				b.WriteLong(mConveyorBeltCounter);
				b.WriteLong(mCutSceneDarken);
				b.WriteLong(mNumPackets);
				for (int i = 0; i < mSeedPackets.Length; i++)
				{
					b.WriteBoolean(mSeedPackets[i] != null);
					if (mSeedPackets[i] != null)
					{
						mSeedPackets[i].SaveToFile(b);
					}
				}
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				return false;
			}
			return true;
		}

		public override bool LoadFromFile(Sexy.Buffer b)
		{
			try
			{
				base.LoadFromFile(b);
				mConveyorBeltCounter = b.ReadLong();
				mCutSceneDarken = b.ReadLong();
				mNumPackets = b.ReadLong();
				for (int i = 0; i < mSeedPackets.Length; i++)
				{
					if (b.ReadBoolean())
					{
						mSeedPackets[i] = new SeedPacket();
						mSeedPackets[i].LoadFromFile(b);
					}
					else
					{
						mSeedPackets[i] = null;
					}
				}
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				return false;
			}
			return true;
		}

		public void DrawSun(Graphics g)
		{
			if ((mBoard.mCutScene == null || (!mBoard.mCutScene.IsBeforePreloading() && mApp.mGameMode != GameMode.GAMEMODE_INTRO)) && !mBoard.HasConveyorBeltSeedBank())
			{
				int i = Math.Max(mBoard.mSunMoney, 0);
				string theText = LawnApp.ToString(i);
				SexyColor theColor = new SexyColor(0, 0, 0);
				if (mBoard.mOutOfMoneyCounter > 0 && mBoard.mOutOfMoneyCounter % 20 < 10)
				{
					theColor = new SexyColor(255, 0, 0);
				}
				int num = Constants.UISunBankPositionX - Constants.Board_Offset_AspectRatio_Correction;
				int mX = base.mX;
				g.DrawImage(AtlasResources.IMAGE_DAN_SUNBANK, num, mX);
				TodCommon.TodDrawString(g, theText, num + Constants.UISunBankTextOffset.X, mX + Constants.UISunBankTextOffset.Y, Resources.FONT_CONTINUUMBOLD14OUTLINE, theColor, DrawStringJustification.DS_ALIGN_CENTER);
			}
		}

		public bool MouseHitTest(int x, int y, out HitResult theHitResult)
		{
			theHitResult = default(HitResult);
			if ((float)x > (float)mWidth * Constants.S - 50f)
			{
				return false;
			}
			SeedPacket seedPacket = null;
			int num = Constants.SMALL_SEEDPACKET_HEIGHT * 2;
			for (int i = 0; i < mNumPackets; i++)
			{
				SeedPacket seedPacket2 = mSeedPackets[i];
				if (seedPacket2.mPacketType == SeedType.SEED_NONE)
				{
					continue;
				}
				int num2 = seedPacket2.CenterY();
				if (num2 <= mHeight)
				{
					int num3 = Math.Abs(num2 - y);
					if (num3 < num)
					{
						num = num3;
						seedPacket = seedPacket2;
					}
				}
			}
			if (seedPacket != null)
			{
				theHitResult.mObject = seedPacket;
				theHitResult.mObjectType = GameObjectType.OBJECT_TYPE_SEEDPACKET;
				return true;
			}
			theHitResult.mObject = null;
			theHitResult.mObjectType = GameObjectType.OBJECT_TYPE_NONE;
			return false;
		}

		public void Move(int x, int y)
		{
			mX = x - ((mApp.mGameScene == GameScenes.SCENE_PLAYING) ? Constants.Board_Offset_AspectRatio_Correction : 0);
			mY = y;
		}

		public bool ContainsPoint(int theX, int theY)
		{
			if (theX >= mX && theX < mX + mWidth && theY >= mY)
			{
				return theY < mY + mHeight;
			}
			return false;
		}

		public int GetNumSeedsOnConveyorBelt()
		{
			for (int i = 0; i < mNumPackets; i++)
			{
				SeedPacket seedPacket = mSeedPackets[i];
				if (seedPacket.mPacketType == SeedType.SEED_NONE)
				{
					return i;
				}
			}
			return mNumPackets;
		}

		public void AddSeed(SeedType theSeedType)
		{
			Debug.ASSERT(mBoard.HasConveyorBeltSeedBank());
			Debug.ASSERT(theSeedType != SeedType.SEED_NONE);
			int numSeedsOnConveyorBelt = GetNumSeedsOnConveyorBelt();
			if (numSeedsOnConveyorBelt == mNumPackets)
			{
				return;
			}
			SeedPacket seedPacket = mSeedPackets[numSeedsOnConveyorBelt];
			seedPacket.mPacketType = theSeedType;
			seedPacket.mRefreshCounter = 0;
			seedPacket.mRefreshTime = 0;
			seedPacket.mRefreshing = false;
			seedPacket.mActive = true;
			seedPacket.mOffsetY = (int)Constants.InvertAndScale(400f) - 35 * numSeedsOnConveyorBelt;
			if (numSeedsOnConveyorBelt > 0)
			{
				SeedPacket seedPacket2 = mSeedPackets[numSeedsOnConveyorBelt - 1];
				if (seedPacket.mOffsetY < seedPacket2.mOffsetY)
				{
					seedPacket.mOffsetY = seedPacket2.mOffsetY + 40;
				}
			}
		}

		public void RemoveSeed(int theIndex)
		{
			Debug.ASSERT(mBoard.HasConveyorBeltSeedBank());
			Debug.ASSERT(theIndex >= 0 && theIndex < GetNumSeedsOnConveyorBelt());
			for (int i = theIndex; i < mNumPackets; i++)
			{
				SeedPacket seedPacket = mSeedPackets[i];
				if (seedPacket.mPacketType == SeedType.SEED_NONE)
				{
					break;
				}
				if (i == mNumPackets - 1)
				{
					seedPacket.mPacketType = SeedType.SEED_NONE;
					seedPacket.mOffsetY = 0;
				}
				else
				{
					SeedPacket seedPacket2 = mSeedPackets[i + 1];
					seedPacket.mPacketType = seedPacket2.mPacketType;
					seedPacket.mOffsetY = seedPacket2.mOffsetY + Constants.SMALL_SEEDPACKET_HEIGHT;
				}
				seedPacket.mRefreshCounter = 0;
				seedPacket.mRefreshTime = 0;
				seedPacket.mRefreshing = false;
				seedPacket.mActive = true;
			}
		}

		public int CountOfTypeOnConveyorBelt(SeedType aSeedType)
		{
			int num = 0;
			for (int i = 0; i < mNumPackets; i++)
			{
				SeedPacket seedPacket = mSeedPackets[i];
				if (seedPacket.mPacketType == aSeedType)
				{
					num++;
				}
			}
			return num;
		}

		public void UpdateConveyorBelt()
		{
			mConveyorBeltCounter++;
			if (mConveyorBeltCounter % 4 != 0)
			{
				return;
			}
			for (int i = 0; i < mNumPackets; i++)
			{
				SeedPacket seedPacket = mSeedPackets[i];
				if (seedPacket.mOffsetY > 0)
				{
					seedPacket.mOffsetY = Math.Max(seedPacket.mOffsetY - 1, 0);
				}
			}
		}

		public void UpdateHeight()
		{
			mNumPackets = mBoard.GetNumSeedsInBank();
			mHeight = mApp.mHeight;
			for (int i = 0; i < mNumPackets; i++)
			{
				SeedPacket seedPacket = mSeedPackets[i];
				seedPacket.mY = mBoard.GetSeedPacketPositionY(i);
			}
		}

		public void RefreshAllPackets()
		{
			for (int i = 0; i < mNumPackets; i++)
			{
				SeedPacket seedPacket = mSeedPackets[i];
				if (seedPacket.mPacketType == SeedType.SEED_NONE)
				{
					break;
				}
				if (seedPacket.mRefreshing)
				{
					seedPacket.mRefreshCounter = 0;
					seedPacket.mRefreshing = false;
					seedPacket.Activate();
					seedPacket.FlashIfReady();
				}
			}
		}
	}
}
