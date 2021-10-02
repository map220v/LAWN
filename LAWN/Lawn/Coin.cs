using Sexy;
using Sexy.TodLib;
using System;

namespace Lawn
{
	internal class Coin : GameObject
	{
		public static SeedType LoadedSeedType;

		public float mPosX;

		public float mPosY;

		public float mVelX;

		public float mVelY;

		public float mScale;

		public bool mDead;

		public int mFadeCount;

		public float mCollectX;

		public float mCollectY;

		public int mGroundY;

		public int mCoinAge;

		public bool mIsBeingCollected;

		public int mDisappearCounter;

		public CoinType mType;

		public CoinMotion mCoinMotion;

		private Attachment mAttachmentID;

		public float mCollectionDistance;

		public SeedType mUsableSeedType;

		public PottedPlant mPottedPlantSpec = new PottedPlant();

		public bool mNeedsBouncyArrow;

		public bool mHasBouncyArrow;

		public bool mHitGround;

		public int mTimesDropped;

		public bool mScored;

		internal static void LoadSeedPacketImage(SeedType theSeedType)
		{
			LoadedSeedType = theSeedType;
		}

		public static void CoinFreeTextures()
		{
		}

		public void CoinInitialize(int theX, int theY, CoinType theCoinType, CoinMotion theCoinMotion)
		{
			mDead = false;
			mWidth = (int)Constants.InvertAndScale(23f);
			mHeight = (int)Constants.InvertAndScale(23f);
			mType = theCoinType;
			mPosX = theX;
			mPosY = theY;
			mDisappearCounter = 0;
			mIsBeingCollected = false;
			mFadeCount = 0;
			mCoinMotion = theCoinMotion;
			mCoinAge = 0;
			mAttachmentID = null;
			mCollectionDistance = 0f;
			mRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_COIN_BANK, 0, 1);
			mScale = 1f;
			mUsableSeedType = SeedType.SEED_NONE;
			mNeedsBouncyArrow = false;
			mHasBouncyArrow = false;
			mHitGround = false;
			mTimesDropped = 0;
			mPottedPlantSpec.InitializePottedPlant(SeedType.SEED_NONE);
			int num = Constants.LAWN_XMIN + (int)Constants.InvertAndScale(40f);
			if (num + mWidth < Constants.LAWN_XMIN && mApp.mGameMode != GameMode.GAMEMODE_CHALLENGE_ZEN_GARDEN)
			{
				mPosX = num + mWidth;
			}
			if (IsSun())
			{
				float num2 = (float)mWidth * Constants.IS * 0.5f;
				float num3 = (float)mHeight * Constants.IS * 0.5f;
				Reanimation reanimation = mApp.AddReanimation(0f, 0f, 0, ReanimationType.REANIM_SUN);
				reanimation.SetPosition((mPosX + num2) * Constants.S, (mPosY + num3) * Constants.S);
				reanimation.mLoopType = ReanimLoopType.REANIM_LOOP;
				reanimation.mAnimRate = 6f;
				GlobalMembersAttachment.AttachReanim(ref mAttachmentID, reanimation, num2, num3);
			}
			else if (mType == CoinType.COIN_SILVER)
			{
				mPosX -= 10f;
				mPosY -= 8f;
				float num4 = 9f;
				float num5 = 9f;
				Reanimation reanimation2 = mApp.AddReanimation(0f, 0f, 0, ReanimationType.REANIM_COIN_SILVER);
				reanimation2.SetPosition((mPosX + num4) * Constants.S, (mPosY + num5) * Constants.S);
				reanimation2.mLoopType = ReanimLoopType.REANIM_LOOP;
				reanimation2.mAnimTime = TodCommon.RandRangeFloat(0f, 0.99f);
				reanimation2.mAnimRate *= TodCommon.RandRangeFloat(0.6f, 1f);
				GlobalMembersAttachment.AttachReanim(ref mAttachmentID, reanimation2, num4, num5);
			}
			else if (mType == CoinType.COIN_GOLD)
			{
				mPosX -= 10f;
				mPosY -= 8f;
				float num6 = 9f;
				float num7 = 9f;
				Reanimation reanimation3 = mApp.AddReanimation(0f, 0f, 0, ReanimationType.REANIM_COIN_GOLD);
				reanimation3.SetPosition((mPosX + num6) * Constants.S, (mPosY + num7) * Constants.S);
				reanimation3.mLoopType = ReanimLoopType.REANIM_LOOP;
				reanimation3.mAnimTime = TodCommon.RandRangeFloat(0f, 0.99f);
				reanimation3.mAnimRate *= TodCommon.RandRangeFloat(0.6f, 1f);
				GlobalMembersAttachment.AttachReanim(ref mAttachmentID, reanimation3, num6, num7);
			}
			else if (mType == CoinType.COIN_DIAMOND)
			{
				mPosX -= 15f;
				mPosY -= 15f;
				float num8 = -3f;
				float num9 = 4f;
				Reanimation reanimation4 = mApp.AddReanimation(0f, 0f, 0, ReanimationType.REANIM_DIAMOND);
				reanimation4.SetPosition((mPosX + num8) * Constants.S, (mPosY + num9) * Constants.S);
				reanimation4.mLoopType = ReanimLoopType.REANIM_LOOP;
				reanimation4.mAnimTime = TodCommon.RandRangeFloat(0f, 0.99f);
				reanimation4.mAnimRate = TodCommon.RandRangeFloat(50f, 80f);
				GlobalMembersAttachment.AttachReanim(ref mAttachmentID, reanimation4, num8, num9);
			}
			if (mApp.IsStormyNightLevel())
			{
				mRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_ABOVE_UI, 0, 0);
			}
			if (mType == CoinType.COIN_FINAL_SEED_PACKET)
			{
				mWidth = AtlasResources.IMAGE_SEEDPACKETS.GetCelWidth();
				mHeight = AtlasResources.IMAGE_SEEDPACKETS.GetCelHeight();
				mRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_ABOVE_UI, 0, 0);
				LoadSeedPacketImage(GetFinalSeedPacketType());
			}
			else if (mType == CoinType.COIN_TROPHY)
			{
				mWidth = AtlasResources.IMAGE_TROPHY.GetCelWidth();
				mHeight = AtlasResources.IMAGE_TROPHY.GetCelHeight();
				mRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_ABOVE_UI, 0, 0);
			}
			else if (mType == CoinType.COIN_SHOVEL)
			{
				mWidth = AtlasResources.IMAGE_SHOVEL.GetCelWidth();
				mHeight = AtlasResources.IMAGE_SHOVEL.GetCelHeight();
				mRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_ABOVE_UI, 0, 0);
			}
			else if (mType == CoinType.COIN_CARKEYS)
			{
				mWidth = AtlasResources.IMAGE_CARKEYS.GetCelWidth();
				mHeight = AtlasResources.IMAGE_CARKEYS.GetCelHeight();
				mRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_ABOVE_UI, 0, 0);
			}
			else if (mType == CoinType.COIN_ALMANAC)
			{
				mWidth = AtlasResources.IMAGE_ALMANAC.GetCelWidth();
				mHeight = AtlasResources.IMAGE_ALMANAC.GetCelHeight();
				mRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_ABOVE_UI, 0, 0);
			}
			else if (mType == CoinType.COIN_WATERING_CAN)
			{
				mWidth = AtlasResources.IMAGE_REANIM_ZENGARDEN_WATERINGCAN1.GetCelWidth();
				mHeight = AtlasResources.IMAGE_REANIM_ZENGARDEN_WATERINGCAN1.GetCelHeight();
				mRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_ABOVE_UI, 0, 0);
			}
			else if (mType == CoinType.COIN_TACO)
			{
				mWidth = AtlasResources.IMAGE_TACO.GetCelWidth();
				mHeight = AtlasResources.IMAGE_TACO.GetCelHeight();
				mRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_ABOVE_UI, 0, 0);
			}
			else if (mType == CoinType.COIN_BACON)
			{
				mWidth = AtlasResources.IMAGE_BACON.GetCelWidth();
				mHeight = AtlasResources.IMAGE_BACON.GetCelHeight();
				mRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_ABOVE_UI, 0, 0);
			}
			else if (mType == CoinType.COIN_NOTE)
			{
				mWidth = Resources.IMAGE_ZOMBIE_NOTE_SMALL.GetCelWidth();
				mHeight = Resources.IMAGE_ZOMBIE_NOTE_SMALL.GetCelHeight();
				mRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_ABOVE_UI, 0, 0);
			}
			else if (mType == CoinType.COIN_USABLE_SEED_PACKET)
			{
				mWidth = AtlasResources.IMAGE_SEEDPACKETS.GetCelWidth();
				mHeight = AtlasResources.IMAGE_SEEDPACKETS.GetCelHeight();
				mRenderOrder = 500002;
			}
			else if (mType == CoinType.COIN_PRESENT_PLANT)
			{
				mWidth = AtlasResources.IMAGE_PRESENT.GetCelWidth();
				mHeight = AtlasResources.IMAGE_PRESENT.GetCelHeight();
				if (mApp.IsSurvivalEndless(mApp.mGameMode) || mApp.IsEndlessIZombie(mApp.mGameMode) || mApp.IsEndlessScaryPotter(mApp.mGameMode))
				{
					SeedType theSeedType = mApp.mZenGarden.PickRandomSeedType();
					mPottedPlantSpec.InitializePottedPlant(theSeedType);
				}
				else if (mBoard.mBackground == BackgroundType.BACKGROUND_1_DAY)
				{
					int[] array = new int[8]
					{
						0,
						1,
						2,
						3,
						7,
						4,
						5,
						6
					};
					SeedType theSeedType2 = (SeedType)TodCommon.TodPickFromArray(array, array.Length);
					mPottedPlantSpec.InitializePottedPlant(theSeedType2);
				}
				else if (mBoard.mBackground == BackgroundType.BACKGROUND_2_NIGHT)
				{
					int[] array2 = new int[8]
					{
						8,
						9,
						10,
						11,
						12,
						13,
						14,
						15
					};
					SeedType theSeedType3 = (SeedType)TodCommon.TodPickFromArray(array2, array2.Length);
					mPottedPlantSpec.InitializePottedPlant(theSeedType3);
				}
				else if (mBoard.mBackground == BackgroundType.BACKGROUND_3_POOL)
				{
					int[] array3 = new int[8]
					{
						16,
						17,
						18,
						19,
						20,
						21,
						22,
						23
					};
					SeedType theSeedType4 = (SeedType)TodCommon.TodPickFromArray(array3, array3.Length);
					mPottedPlantSpec.InitializePottedPlant(theSeedType4);
				}
				else if (mBoard.mBackground == BackgroundType.BACKGROUND_4_FOG)
				{
					int[] array4 = new int[8]
					{
						24,
						25,
						26,
						27,
						28,
						29,
						30,
						31
					};
					SeedType theSeedType5 = (SeedType)TodCommon.TodPickFromArray(array4, array4.Length);
					mPottedPlantSpec.InitializePottedPlant(theSeedType5);
				}
				else if (mBoard.mBackground == BackgroundType.BACKGROUND_5_ROOF)
				{
					int[] array5 = new int[6]
					{
						32,
						34,
						35,
						36,
						37,
						39
					};
					SeedType theSeedType6 = (SeedType)TodCommon.TodPickFromArray(array5, array5.Length);
					mPottedPlantSpec.InitializePottedPlant(theSeedType6);
				}
				else
				{
					SeedType theSeedType7 = mApp.mZenGarden.PickRandomSeedType();
					mPottedPlantSpec.InitializePottedPlant(theSeedType7);
				}
			}
			else if (mType == CoinType.COIN_AWARD_MONEY_BAG || mType == CoinType.COIN_AWARD_BAG_DIAMOND)
			{
				mWidth = AtlasResources.IMAGE_MONEYBAG_HI_RES.GetCelWidth();
				mHeight = AtlasResources.IMAGE_MONEYBAG_HI_RES.GetCelHeight();
				mRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_ABOVE_UI, 0, 0);
			}
			else if (mType == CoinType.COIN_CHOCOLATE || mType == CoinType.COIN_AWARD_CHOCOLATE)
			{
				mWidth = AtlasResources.IMAGE_CHOCOLATE.GetCelWidth();
				mHeight = AtlasResources.IMAGE_CHOCOLATE.GetCelHeight();
				mRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_ABOVE_UI, 0, 0);
			}
			else if (IsPresentWithAdvice())
			{
				mWidth = AtlasResources.IMAGE_PRESENT.GetCelWidth();
				mHeight = AtlasResources.IMAGE_PRESENT.GetCelHeight();
				mRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_ABOVE_UI, 0, 0);
			}
			mWidth = (int)((float)mWidth * Constants.IS);
			mHeight = (int)((float)mHeight * Constants.IS);
			switch (mCoinMotion)
			{
			case CoinMotion.COIN_MOTION_FROM_SKY:
				mVelY = 0.67f;
				mVelX = 0f;
				mGroundY = RandomNumbers.NextNumber(250) + 300;
				break;
			case CoinMotion.COIN_MOTION_FROM_SKY_SLOW:
				mVelY = 0.33f;
				mVelX = 0f;
				mGroundY = RandomNumbers.NextNumber(250) + 300;
				break;
			case CoinMotion.COIN_MOTION_FROM_PLANT:
				mVelY = -1.7f - TodCommon.RandRangeFloat(0f, 1.7f);
				mVelX = -0.4f + TodCommon.RandRangeFloat(0f, 0.8f);
				mGroundY = (int)mPosY + 15 + RandomNumbers.NextNumber(20);
				mScale = 0.4f;
				break;
			case CoinMotion.COIN_MOTION_COIN:
				mVelY = -3f - TodCommon.RandRangeFloat(0f, 2f);
				mVelX = -0.5f + TodCommon.RandRangeFloat(0f, 1f);
				mGroundY = (int)mPosY + 45 + RandomNumbers.NextNumber(20);
				if (mGroundY > Constants.BOARD_HEIGHT - 60 && mApp.mGameMode != GameMode.GAMEMODE_CHALLENGE_ZEN_GARDEN)
				{
					mGroundY = Constants.BOARD_HEIGHT - 60;
				}
				if (mGroundY < 80 && mApp.mGameMode != GameMode.GAMEMODE_CHALLENGE_ZEN_GARDEN)
				{
					mGroundY = 80;
				}
				if (mType == CoinType.COIN_FINAL_SEED_PACKET || mType == CoinType.COIN_USABLE_SEED_PACKET || mType == CoinType.COIN_TROPHY || mType == CoinType.COIN_SHOVEL || mType == CoinType.COIN_CARKEYS || mType == CoinType.COIN_ALMANAC || mType == CoinType.COIN_TACO || mType == CoinType.COIN_BACON || mType == CoinType.COIN_WATERING_CAN || mType == CoinType.COIN_NOTE)
				{
					mGroundY -= 30;
				}
				break;
			case CoinMotion.COIN_MOTION_FROM_BOSS:
				mVelY = -5f;
				mVelX = -3f;
				mPosX = 750f;
				mPosY = 245f;
				mGroundY = (int)mPosY + 40;
				break;
			case CoinMotion.COIN_MOTION_LAWNMOWER_COIN:
				mVelY = 0f;
				mVelX = 0f;
				mGroundY = 600;
				Collect();
				break;
			case CoinMotion.COIN_MOTION_FROM_PRESENT:
				mVelY = 0f;
				mVelX = 0f;
				mGroundY = 600;
				mRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_ABOVE_UI, 0, 1);
				break;
			default:
				Debug.ASSERT(false);
				break;
			}
			if (mCoinMotion != CoinMotion.COIN_MOTION_LAWNMOWER_COIN && mApp.mGameMode != GameMode.GAMEMODE_CHALLENGE_ZEN_GARDEN && mPosX - (float)mWidth < (float)Constants.LAWN_XMIN)
			{
				mPosX = Constants.LAWN_XMIN + mWidth;
			}
			mScale *= GetSunScale();
			if (CoinGetsBouncyArrow())
			{
				mNeedsBouncyArrow = true;
			}
			if (mCoinMotion != CoinMotion.COIN_MOTION_FROM_PRESENT)
			{
				PlayLaunchSound();
			}
			if (mApp.mGameMode != GameMode.GAMEMODE_CHALLENGE_ZEN_GARDEN)
			{
				CheckRange_X(ref mPosX, mWidth, theCoinMotion);
			}
			Update();
		}

		public void Dispose()
		{
			GlobalMembersAttachment.AttachmentDie(ref mAttachmentID);
		}

		public void MouseDown(int x, int y, int theClickCount)
		{
			if (mBoard != null && !mBoard.mPaused && mApp.mGameScene == GameScenes.SCENE_PLAYING && !mDead && theClickCount >= 0 && !mIsBeingCollected)
			{
				PlayCollectSound();
				Collect();
				if (mApp.IsFirstTimeAdventureMode() && mBoard.mLevel == 1)
				{
					mBoard.DisplayAdvice("[ADVICE_CLICKED_ON_SUN]", MessageStyle.MESSAGE_STYLE_TUTORIAL_LEVEL1_STAY, AdviceType.ADVICE_CLICKED_ON_SUN);
				}
			}
		}

		public bool MouseHitTest(int theX, int theY, out HitResult theHitResult)
		{
			theHitResult = default(HitResult);
			theX = (int)((float)theX * Constants.IS);
			theY = (int)((float)theY * Constants.IS);
			int num = 0;
			if (mType == CoinType.COIN_AWARD_PRESENT || IsPresentWithAdvice() || mType == CoinType.COIN_PRESENT_PLANT)
			{
				num = -60;
			}
			int num2 = 0;
			int num3 = 0;
			if (mApp.IsWhackAZombieLevel())
			{
				num3 = 30;
				num2 = 15;
			}
			if (IsMoney() || mType == CoinType.COIN_AWARD_MONEY_BAG || mType == CoinType.COIN_AWARD_BAG_DIAMOND)
			{
				num2 = 40;
			}
			if (mType == CoinType.COIN_SUN || mType == CoinType.COIN_SMALLSUN)
			{
				num2 = 50;
			}
			if (!mDead && (!mIsBeingCollected || (mType == CoinType.COIN_USABLE_SEED_PACKET && mBoard.mIgnoreNextMouseUpSeedPacket)) && (mType != CoinType.COIN_USABLE_SEED_PACKET || mBoard == null || mBoard.mCursorObject.mCursorType == CursorType.CURSOR_TYPE_NORMAL || mApp.IsWhackAZombieLevel() || mBoard.mIgnoreNextMouseUpSeedPacket) && (((float)theX >= mPosX - (float)num2 && (float)theX < mPosX + (float)mWidth + (float)num2 && (float)theY >= mPosY + (float)num - (float)num2 && (float)theY < mPosY + (float)mHeight + (float)num + (float)num2 + (float)num3) ? true : false))
			{
				theHitResult.mObject = this;
				theHitResult.mObjectType = GameObjectType.OBJECT_TYPE_COIN;
				return true;
			}
			theHitResult.mObject = null;
			theHitResult.mObjectType = GameObjectType.OBJECT_TYPE_NONE;
			return false;
		}

		public void Die()
		{
			Debug.ASSERT(mBoard == null || mBoard.mCursorObject.mCoinID != mBoard.mCoins[mBoard.mCoins.IndexOf(this)]);
			mDead = true;
			GlobalMembersAttachment.AttachmentDie(ref mAttachmentID);
		}

		public void StartFade()
		{
			mFadeCount = 15;
		}

		public void Update()
		{
			mCoinAge += 3;
			if (mApp.mGameScene != GameScenes.SCENE_PLAYING && mApp.mGameScene != GameScenes.SCENE_AWARD && mBoard != null && !mBoard.mCutScene.ShouldRunUpsellBoard())
			{
				return;
			}
			if (mFadeCount < 0 || mFadeCount >= 3)
			{
				UpdateFade();
			}
			else if (!mIsBeingCollected)
			{
				UpdateFall();
			}
			else
			{
				UpdateCollected();
			}
			if (mAttachmentID != null)
			{
				float num = 0f;
				float num2 = 0f;
				if (mType == CoinType.COIN_DIAMOND)
				{
					float num3 = Constants.InvertAndScale(18f);
					float num4 = Constants.InvertAndScale(13f);
					num = num3 - num3 * mScale;
					num2 = num4 - num4 * mScale;
				}
				GlobalMembersAttachment.AttachmentUpdateAndMove(ref mAttachmentID, mPosX + num, mPosY + num2);
				GlobalMembersAttachment.AttachmentOverrideColor(mAttachmentID, GetColor());
				GlobalMembersAttachment.AttachmentOverrideScale(mAttachmentID, mScale);
				if ((!mHitGround || mIsBeingCollected) && (mType == CoinType.COIN_SILVER || mType == CoinType.COIN_GOLD))
				{
					GlobalMembersAttachment.AttachmentOverrideColor(mAttachmentID, new SexyColor(0, 0, 0, 0));
				}
			}
		}

		public void Draw(Graphics g)
		{
			g.SetColor(GetColor());
			if (mType == CoinType.COIN_DIAMOND)
			{
				g.SetColorizeImages(true);
				g.DrawImage(AtlasResources.IMAGE_AWARDPICKUPGLOW, (mPosX - 56f) * Constants.S, (mPosY - 66f) * Constants.S);
				g.SetColorizeImages(false);
			}
			if (mType == CoinType.COIN_PRESENT_PLANT)
			{
				g.SetColorizeImages(true);
				g.DrawImage(AtlasResources.IMAGE_AWARDPICKUPGLOW, (mPosX - 50f) * Constants.S, (mPosY - 70f) * Constants.S);
				g.SetColorizeImages(false);
			}
			if (mType == CoinType.COIN_AWARD_PRESENT && mIsBeingCollected)
			{
				g.SetColorizeImages(true);
				g.DrawImage(AtlasResources.IMAGE_AWARDPICKUPGLOW, (mPosX - 50f) * Constants.S, (mPosY - 64f) * Constants.S);
				g.SetColorizeImages(false);
			}
			if (mType == CoinType.COIN_CHOCOLATE || mType == CoinType.COIN_AWARD_CHOCOLATE)
			{
				g.SetColorizeImages(true);
				g.DrawImage(AtlasResources.IMAGE_AWARDPICKUPGLOW, (mPosX - 56f) * Constants.S, (mPosY - 50f) * Constants.S);
				g.SetColorizeImages(false);
			}
			if (mAttachmentID != null)
			{
				Graphics @new = Graphics.GetNew(g);
				MakeParentGraphicsFrame(@new);
				GlobalMembersAttachment.AttachmentDraw(mAttachmentID, @new, false, true);
				@new.PrepareForReuse();
			}
			if (((mType == CoinType.COIN_SILVER || mType == CoinType.COIN_GOLD) && mHitGround && !mIsBeingCollected) || mType == CoinType.COIN_DIAMOND)
			{
				return;
			}
			if (IsLevelAward() && !mIsBeingCollected)
			{
				SexyColor flashingColor = TodCommon.GetFlashingColor(mCoinAge, 75);
				g.SetColor(flashingColor);
			}
			if (mType == CoinType.COIN_SILVER || mType == CoinType.COIN_GOLD)
			{
				g.SetColorizeImages(true);
				TodCommon.TodDrawImageCenterScaledF(g, AtlasResources.IMAGE_REANIM_COINGLOW, (mPosX - (float)Constants.Coin_Glow_Offset.X) * Constants.S, (mPosY - (float)Constants.Coin_Glow_Offset.Y) * Constants.S, mScale, mScale);
				g.SetColorizeImages(false);
			}
			Image theImageStrip = null;
			int theCelCol = 0;
			float num = mScale;
			float num2 = 0f;
			float num3 = 0f;
			if (mType == CoinType.COIN_SILVER)
			{
				theImageStrip = AtlasResources.IMAGE_REANIM_COIN_SILVER_DOLLAR;
				num2 = 8f;
				num3 = 10f;
			}
			else if (mType == CoinType.COIN_GOLD)
			{
				theImageStrip = AtlasResources.IMAGE_REANIM_COIN_GOLD_DOLLAR;
				num2 = 8f;
				num3 = 10f;
			}
			else
			{
				if (mType == CoinType.COIN_SUN || mType == CoinType.COIN_SMALLSUN || mType == CoinType.COIN_LARGESUN)
				{
					return;
				}
				if (mType == CoinType.COIN_FINAL_SEED_PACKET)
				{
					TodCommon.TodDrawImageCenterScaledF(g, AtlasResources.IMAGE_SEEDPACKETS, (int)(mPosX * Constants.S), (int)(mPosY * Constants.S), mScale, mScale, (int)LoadedSeedType, true);
					return;
				}
				if (mType == CoinType.COIN_PRESENT_PLANT || mType == CoinType.COIN_AWARD_PRESENT)
				{
					if (mIsBeingCollected)
					{
						mApp.mZenGarden.DrawPottedPlantIcon(g, (mPosX + 35f) * Constants.S, (mPosY + 15f) * Constants.S, mPottedPlantSpec);
						return;
					}
					theImageStrip = AtlasResources.IMAGE_PRESENT;
					num3 = -60f;
				}
				else if (IsPresentWithAdvice())
				{
					num3 = -60f;
					if (mIsBeingCollected)
					{
						num2 = -10f;
						num3 -= -10f;
						theImageStrip = AtlasResources.IMAGE_PRESENTOPEN;
					}
					else
					{
						theImageStrip = AtlasResources.IMAGE_PRESENT;
					}
				}
				else if (mType == CoinType.COIN_AWARD_MONEY_BAG || mType == CoinType.COIN_AWARD_BAG_DIAMOND)
				{
					if (mIsBeingCollected && mApp.IsQuickPlayMode())
					{
						return;
					}
					theImageStrip = AtlasResources.IMAGE_MONEYBAG_HI_RES;
					if (mScale == 1f)
					{
						num2 -= (float)(mWidth / 4 + 10);
						num3 -= (float)(mHeight / 4);
					}
					num *= 0.5f;
				}
				else if (mType == CoinType.COIN_CHOCOLATE || mType == CoinType.COIN_AWARD_CHOCOLATE)
				{
					theImageStrip = AtlasResources.IMAGE_CHOCOLATE;
				}
				else if (mType == CoinType.COIN_TROPHY)
				{
					theImageStrip = AtlasResources.IMAGE_TROPHY_HI_RES;
					num2 -= (float)(mWidth / 2);
					num3 -= (float)(mHeight / 2);
					num *= 0.5f;
				}
				else if (mType == CoinType.COIN_SHOVEL)
				{
					theImageStrip = AtlasResources.IMAGE_SHOVEL_HI_RES;
					num2 -= (float)Constants.Coin_Shovel_Offset.X;
					num3 -= (float)Constants.Coin_Shovel_Offset.Y;
					num *= 0.5f;
				}
				else if (mType == CoinType.COIN_CARKEYS)
				{
					theImageStrip = AtlasResources.IMAGE_CARKEYS;
				}
				else if (mType == CoinType.COIN_ALMANAC)
				{
					theImageStrip = AtlasResources.IMAGE_ALMANAC;
				}
				else if (mType == CoinType.COIN_TACO)
				{
					theImageStrip = AtlasResources.IMAGE_TACO;
				}
				else if (mType == CoinType.COIN_BACON)
				{
					theImageStrip = AtlasResources.IMAGE_BACON;
				}
				else if (mType == CoinType.COIN_WATERING_CAN)
				{
					num2 -= (float)Constants.Coin_Shovel_Offset.X;
					num3 -= (float)Constants.Coin_Shovel_Offset.Y;
					theImageStrip = AtlasResources.IMAGE_REANIM_ZENGARDEN_WATERINGCAN1;
				}
				else if (mType == CoinType.COIN_NOTE)
				{
					theImageStrip = Resources.IMAGE_ZOMBIE_NOTE_SMALL;
				}
				else
				{
					if (mType == CoinType.COIN_USABLE_SEED_PACKET)
					{
						int theGrayness = 255;
						if (mIsBeingCollected)
						{
							theGrayness = 128;
						}
						else
						{
							int disappearTime = GetDisappearTime();
							if (mDisappearCounter > disappearTime - 300 && mDisappearCounter % 60 < 30)
							{
								theGrayness = 192;
							}
						}
						g.SetColorizeImages(true);
						if (g.mDrawMode == Graphics.DrawMode.DRAWMODE_ADDITIVE)
						{
							int num4 = 0;
							num4++;
						}
						SeedPacket.DrawSmallSeedPacket(g, (int)(mPosX * Constants.S), (int)(mPosY * Constants.S), mUsableSeedType, SeedType.SEED_NONE, 0f, theGrayness, false, false, true, false);
						g.SetColorizeImages(false);
						return;
					}
					Debug.ASSERT(false);
				}
			}
			g.SetColorizeImages(true);
			TodCommon.TodDrawImageCelCenterScaledF(g, theImageStrip, (mPosX + num2) * Constants.S, (mPosY + num3) * Constants.S, theCelCol, num, num);
			g.SetColorizeImages(false);
		}

		public void Collect()
		{
			if (mDead)
			{
				return;
			}
			mCollectX = mPosX;
			mCollectY = mPosY;
			mIsBeingCollected = true;
			bool flag = false;
			if ((mApp.IsEndlessIZombie(mApp.mGameMode) || mApp.IsEndlessScaryPotter(mApp.mGameMode)) && IsLevelAward())
			{
				flag = true;
			}
			if (mType == CoinType.COIN_AWARD_PRESENT || mType == CoinType.COIN_PRESENT_PLANT)
			{
				Debug.ASSERT(mBoard != null);
				if (mApp.mZenGarden.IsZenGardenFull(false))
				{
					mBoard.DisplayAdvice("[DIALOG_ZEN_GARDEN_FULL]", MessageStyle.MESSAGE_STYLE_HINT_FAST, AdviceType.ADVICE_NONE);
				}
				else
				{
					mBoard.mPottedPlantsCollected++;
					mBoard.DisplayAdvice("[ADVICE_FOUND_PLANT]", MessageStyle.MESSAGE_STYLE_HINT_FAST, AdviceType.ADVICE_NONE);
					mApp.AddTodParticle(mPosX + Constants.InvertAndScale(30f), mPosY + Constants.InvertAndScale(30f), mRenderOrder + 1, ParticleEffect.PARTICLE_PRESENT_PICKUP);
					mApp.mZenGarden.AddPottedPlant(mPottedPlantSpec);
				}
				mDisappearCounter = 0;
				mFadeCount = 0;
				if (flag)
				{
					GlobalMembersAttachment.AttachmentDetachCrossFadeParticleType(ref mAttachmentID, ParticleEffect.PARTICLE_AWARD_PICKUP_ARROW, null);
					mBoard.FadeOutLevel();
				}
				return;
			}
			if (mType == CoinType.COIN_PRESENT_MINIGAMES)
			{
				mApp.AddTodParticle(mPosX + 30f, mPosY + 30f, mRenderOrder + 1, ParticleEffect.PARTICLE_PRESENT_PICKUP);
				mDisappearCounter = 0;
				mFadeCount = 0;
				GlobalMembersAttachment.AttachmentDetachCrossFadeParticleType(ref mAttachmentID, ParticleEffect.PARTICLE_AWARD_PICKUP_ARROW, null);
				mApp.mPlayerInfo.UnlockFirstMiniGames();
				return;
			}
			if (mType == CoinType.COIN_PRESENT_PUZZLE_MODE)
			{
				mApp.AddTodParticle(mPosX + 30f, mPosY + 30f, mRenderOrder + 1, ParticleEffect.PARTICLE_PRESENT_PICKUP);
				mDisappearCounter = 0;
				mFadeCount = 0;
				GlobalMembersAttachment.AttachmentDetachCrossFadeParticleType(ref mAttachmentID, ParticleEffect.PARTICLE_AWARD_PICKUP_ARROW, null);
				mApp.mPlayerInfo.UnlockPuzzleMode();
				return;
			}
			if (mType == CoinType.COIN_CHOCOLATE || mType == CoinType.COIN_AWARD_CHOCOLATE)
			{
				mBoard.mChocolateCollected++;
				mApp.AddTodParticle(mPosX + 30f, mPosY + 30f, mRenderOrder + 1, ParticleEffect.PARTICLE_PRESENT_PICKUP);
				if (mApp.mPlayerInfo.mPurchases[26] < 1000)
				{
					mBoard.DisplayAdvice("[ADVICE_FOUND_CHOCOLATE]", MessageStyle.MESSAGE_STYLE_HINT_TALL_FAST, AdviceType.ADVICE_NONE);
					mApp.mPlayerInfo.mPurchases[26] = 1001;
				}
				else
				{
					mApp.mPlayerInfo.mPurchases[26]++;
				}
				mDisappearCounter = 0;
				StartFade();
				if (flag)
				{
					GlobalMembersAttachment.AttachmentDetachCrossFadeParticleType(ref mAttachmentID, ParticleEffect.PARTICLE_AWARD_PICKUP_ARROW, null);
					mBoard.FadeOutLevel();
				}
				return;
			}
			if (IsLevelAward())
			{
				if (mApp.IsQuickPlayMode() && mType == CoinType.COIN_AWARD_MONEY_BAG)
				{
					mApp.PlayFoley(FoleyType.FOLEY_COIN);
					FanOutCoins(CoinType.COIN_GOLD, 5);
					StartFade();
				}
				else if (flag)
				{
					if (mType == CoinType.COIN_AWARD_BAG_DIAMOND)
					{
						mApp.PlaySample(Resources.SOUND_DIAMOND);
						FanOutCoins(CoinType.COIN_DIAMOND, 1);
						StartFade();
					}
					else if (mType == CoinType.COIN_AWARD_MONEY_BAG)
					{
						mApp.PlayFoley(FoleyType.FOLEY_COIN);
						FanOutCoins(CoinType.COIN_GOLD, 5);
						StartFade();
					}
				}
				else if (mApp.IsScaryPotterLevel())
				{
					if (mType == CoinType.COIN_TROPHY)
					{
						mApp.PlayFoley(FoleyType.FOLEY_COIN);
						FanOutCoins(CoinType.COIN_GOLD, 5);
					}
					else if (mType == CoinType.COIN_AWARD_MONEY_BAG)
					{
						mApp.PlayFoley(FoleyType.FOLEY_COIN);
						FanOutCoins(CoinType.COIN_GOLD, 2);
					}
				}
				else if (mApp.IsAdventureMode() && mBoard.mLevel == 50)
				{
					FanOutCoins(CoinType.COIN_DIAMOND, 3);
				}
				else if (mApp.IsFirstTimeAdventureMode() && mBoard.mLevel == 4)
				{
					mApp.PlaySample(Resources.SOUND_SHOVEL);
				}
				else if (mApp.IsFirstTimeAdventureMode() && (mBoard.mLevel == 24 || mBoard.mLevel == 34 || mBoard.mLevel == 44))
				{
					mApp.PlaySample(Resources.SOUND_TAP2);
				}
				else if (mType == CoinType.COIN_TROPHY)
				{
					mApp.PlaySample(Resources.SOUND_DIAMOND);
					FanOutCoins(CoinType.COIN_DIAMOND, 1);
				}
				else if (mType == CoinType.COIN_AWARD_MONEY_BAG)
				{
					int theNumCoins = 5;
					mApp.PlayFoley(FoleyType.FOLEY_COIN);
					FanOutCoins(CoinType.COIN_GOLD, theNumCoins);
				}
				else
				{
					mApp.PlaySample(Resources.SOUND_SEEDLIFT);
					mApp.PlaySample(Resources.SOUND_TAP2);
				}
				mApp.AddTodParticle(mPosX + 30f, mPosY + 30f, mRenderOrder + 1, ParticleEffect.PARTICLE_STARBURST);
				Debug.ASSERT(mBoard != null);
				mBoard.FadeOutLevel();
				GlobalMembersAttachment.AttachmentDetachCrossFadeParticleType(ref mAttachmentID, ParticleEffect.PARTICLE_SEED_PACKET, null);
				GlobalMembersAttachment.AttachmentDetachCrossFadeParticleType(ref mAttachmentID, ParticleEffect.PARTICLE_AWARD_PICKUP_ARROW, null);
				GlobalMembersAttachment.AttachmentDetachCrossFadeParticleType(ref mAttachmentID, ParticleEffect.PARTICLE_COIN_PICKUP_ARROW, null);
				if (mType == CoinType.COIN_NOTE)
				{
					mApp.AddTodParticle(mPosX + Constants.InvertAndScale(30f), mPosY + Constants.InvertAndScale(30f), mRenderOrder + 1, ParticleEffect.PARTICLE_PRESENT_PICKUP);
					StartFade();
				}
				else if (!flag && mApp.Is3DAccelerated() && !mApp.IsQuickPlayMode())
				{
					float num = mWidth / 2;
					float num2 = mHeight / 2;
					TodParticleSystem theParticleSystem = mApp.AddTodParticle(mPosX + num, mPosY + num2, mRenderOrder - 1, ParticleEffect.PARTICLE_SEED_PACKET_PICKUP);
					GlobalMembersAttachment.AttachParticle(ref mAttachmentID, theParticleSystem, num, num2);
				}
				mDisappearCounter = 0;
				return;
			}
			if (mType == CoinType.COIN_USABLE_SEED_PACKET)
			{
				Debug.ASSERT(mBoard != null);
				mBoard.mCursorObject.mType = mUsableSeedType;
				mBoard.mCursorObject.mCursorType = CursorType.CURSOR_TYPE_PLANT_FROM_USABLE_COIN;
				mBoard.mCursorObject.mCoinID = mBoard.mCoins[mBoard.mCoins.IndexOf(this)];
				mGroundY = (int)mPosY;
				mFadeCount = 0;
				mBoard.mIgnoreNextMouseUpSeedPacket = true;
				return;
			}
			if (IsMoney() && mBoard != null)
			{
				mBoard.ShowCoinBank();
			}
			mFadeCount = 0;
			if (IsSun() && mBoard != null && !mBoard.HasConveyorBeltSeedBank())
			{
				for (int i = 0; i < mBoard.mSeedBank.mNumPackets; i++)
				{
					SeedPacket seedPacket = mBoard.mSeedBank.mSeedPackets[i];
					int currentPlantCost = mBoard.GetCurrentPlantCost(seedPacket.mPacketType, seedPacket.mImitaterType);
					int num3 = mBoard.mSunMoney + mBoard.CountSunBeingCollected() - currentPlantCost;
					if (num3 >= 0 && num3 < GetSunValue())
					{
						seedPacket.FlashIfReady();
					}
				}
				if (mBoard.StageHasFog())
				{
					mRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_ABOVE_UI, 0, 2);
				}
			}
			GlobalMembersAttachment.AttachmentDetachCrossFadeParticleType(ref mAttachmentID, ParticleEffect.PARTICLE_COIN_PICKUP_ARROW, null);
			if (mApp.IsFirstTimeAdventureMode() && mBoard != null && mBoard.mLevel == 11 && (mType == CoinType.COIN_GOLD || mType == CoinType.COIN_SILVER))
			{
				mBoard.DisplayAdvice("[ADVICE_CLICKED_ON_COIN]", MessageStyle.MESSAGE_STYLE_HINT_FAST, AdviceType.ADVICE_CLICKED_ON_COIN);
			}
		}

		public int GetSunValue()
		{
			if (mType == CoinType.COIN_SUN)
			{
				return 25;
			}
			if (mType == CoinType.COIN_SMALLSUN)
			{
				return 15;
			}
			if (mType == CoinType.COIN_LARGESUN)
			{
				return 50;
			}
			return 0;
		}

		public static int GetCoinValue(CoinType theType)
		{
			switch (theType)
			{
			case CoinType.COIN_SILVER:
				return 1;
			case CoinType.COIN_GOLD:
				return 5;
			case CoinType.COIN_DIAMOND:
				return 100;
			default:
				return 0;
			}
		}

		private void UpdateFade()
		{
			if (!mApp.IsEndlessIZombie(mApp.mGameMode) && !mApp.IsEndlessScaryPotter(mApp.mGameMode) && mType != CoinType.COIN_NOTE && IsLevelAward())
			{
				return;
			}
			mFadeCount -= 3;
			if (mFadeCount >= 0 && mFadeCount < 3)
			{
				if (mType == CoinType.COIN_SILVER || mType == CoinType.COIN_GOLD)
				{
					mBoard.mCollectedCoinStreak = 0;
				}
				Die();
			}
		}

		private static void CheckRange_X(ref float mPosX, int mWidth, CoinMotion mCoinMotion)
		{
			if (mPosX > (float)(800 - mWidth) && mCoinMotion != CoinMotion.COIN_MOTION_FROM_BOSS)
			{
				mPosX = 800 - mWidth;
			}
			else if (mPosX < 0f)
			{
				mPosX = 0f;
			}
		}

		private void UpdateFall()
		{
			if (IsPresentWithAdvice())
			{
				mDisappearCounter = 0;
				if (mCoinAge > 500)
				{
					Collect();
				}
			}
			if (mCoinMotion == CoinMotion.COIN_MOTION_FROM_PRESENT)
			{
				mPosX += 3f * mVelX;
				mPosY += 3f * mVelY;
				mVelX *= 0.857f;
				mVelY *= 0.857f;
				if (mCoinAge >= 80)
				{
					Collect();
				}
			}
			else if (mPosY + mVelY < (float)mGroundY)
			{
				mPosY += 3f * mVelY;
				if (mCoinMotion == CoinMotion.COIN_MOTION_FROM_PLANT)
				{
					mVelY += 3f * Constants.InvertAndScale(0.09f);
				}
				else if (mCoinMotion == CoinMotion.COIN_MOTION_COIN || mCoinMotion == CoinMotion.COIN_MOTION_FROM_BOSS)
				{
					mVelY += 3f * Constants.InvertAndScale(0.15f);
				}
				mPosX += 3f * mVelX;
				if (mPosX > (float)(800 - mWidth) && mCoinMotion != CoinMotion.COIN_MOTION_FROM_BOSS)
				{
					mPosX = 800 - mWidth;
					mVelX = Constants.InvertAndScale(-0.4f - TodCommon.RandRangeFloat(0f, 0.4f));
				}
				else if (mPosX < 0f)
				{
					mPosX = 0f;
					mVelX = Constants.InvertAndScale(0.4f + TodCommon.RandRangeFloat(0f, 0.4f));
				}
			}
			else
			{
				if (mNeedsBouncyArrow && !mHasBouncyArrow)
				{
					float num = mWidth / 2;
					float num2 = (float)(mHeight / 2) - Constants.InvertAndScale(60f);
					if (mType == CoinType.COIN_TROPHY)
					{
						num += Constants.InvertAndScale(2f);
					}
					else if (mType == CoinType.COIN_AWARD_MONEY_BAG || mType == CoinType.COIN_AWARD_BAG_DIAMOND)
					{
						num += (float)Constants.Coin_MoneyBag_Offset.X;
						num2 += (float)Constants.Coin_MoneyBag_Offset.Y;
					}
					else if (mType == CoinType.COIN_AWARD_PRESENT || IsPresentWithAdvice())
					{
						num2 += Constants.InvertAndScale(-20f);
					}
					else if (IsMoney())
					{
						num += (float)Constants.Coin_Silver_Award_Offset.X;
						num2 += (float)Constants.Coin_Silver_Award_Offset.Y;
					}
					else if (mType == CoinType.COIN_NOTE)
					{
						num += (float)Constants.Coin_Note_Offset.X;
						num2 += (float)Constants.Coin_Note_Offset.Y;
					}
					else if (mType == CoinType.COIN_ALMANAC)
					{
						num += (float)Constants.Coin_Almanac_Offset.X;
						num2 += (float)Constants.Coin_Almanac_Offset.Y;
					}
					else if (mType == CoinType.COIN_SHOVEL)
					{
						num += (float)Constants.Coin_Shovel_Offset.X;
						num2 += (float)Constants.Coin_Shovel_Offset.Y;
					}
					else if (mType == CoinType.COIN_CARKEYS)
					{
						num += (float)Constants.Coin_CarKeys_Offset.X;
						num2 += (float)Constants.Coin_CarKeys_Offset.Y;
					}
					else if (mType == CoinType.COIN_TACO)
					{
						num += (float)Constants.Coin_Taco_Offset.X;
						num2 += (float)Constants.Coin_Taco_Offset.Y;
					}
					else if (mType == CoinType.COIN_BACON)
					{
						num += (float)Constants.Coin_Bacon_Offset.X;
						num2 += (float)Constants.Coin_Bacon_Offset.Y;
					}
					ParticleEffect theEffect = (mType == CoinType.COIN_FINAL_SEED_PACKET) ? ParticleEffect.PARTICLE_SEED_PACKET : ((!IsMoney()) ? ParticleEffect.PARTICLE_AWARD_PICKUP_ARROW : ParticleEffect.PARTICLE_COIN_PICKUP_ARROW);
					TodParticleSystem theParticleSystem = mApp.AddTodParticle(mPosX + num, mPosY + num2, 0, theEffect);
					GlobalMembersAttachment.AttachParticle(ref mAttachmentID, theParticleSystem, num, num2);
					mHasBouncyArrow = true;
				}
				if (!mHitGround)
				{
					mHitGround = true;
					PlayGroundSound();
				}
				mPosY = mGroundY;
				mPosX = TodCommon.FloatRoundToInt(mPosX);
				if ((mApp.mGameMode != GameMode.GAMEMODE_CHALLENGE_LAST_STAND || mBoard == null || mBoard.mChallenge.mChallengeState == ChallengeState.STATECHALLENGE_LAST_STAND_ONSLAUGHT) && !IsLevelAward() && !IsPresentWithAdvice())
				{
					mDisappearCounter += 3;
					int disappearTime = GetDisappearTime();
					if (mDisappearCounter >= disappearTime)
					{
						StartFade();
					}
				}
			}
			if (mCoinMotion == CoinMotion.COIN_MOTION_FROM_PLANT)
			{
				float sunScale = GetSunScale();
				if (mScale < sunScale)
				{
					mScale += 0.06f;
				}
				else
				{
					mScale = sunScale;
				}
			}
		}

		public override bool SaveToFile(Sexy.Buffer b)
		{
			try
			{
				base.SaveToFile(b);
				b.WriteLong(mCoinAge);
				b.WriteLong((int)mCoinMotion);
				b.WriteFloat(mCollectionDistance);
				b.WriteFloat(mCollectX);
				b.WriteFloat(mCollectY);
				b.WriteBoolean(mDead);
				b.WriteLong(mDisappearCounter);
				b.WriteLong(mFadeCount);
				b.WriteLong(mGroundY);
				b.WriteBoolean(mHitGround);
				b.WriteBoolean(mIsBeingCollected);
				b.WriteBoolean(mNeedsBouncyArrow);
				b.WriteFloat(mPosX);
				b.WriteFloat(mPosY);
				b.WriteBoolean(mPottedPlantSpec != null);
				if (mPottedPlantSpec != null)
				{
					mPottedPlantSpec.Save(b);
				}
				b.WriteLong(mRow);
				b.WriteFloat(mScale);
				b.WriteLong(mTimesDropped);
				b.WriteLong((int)mType);
				b.WriteLong((int)mUsableSeedType);
				b.WriteFloat(mVelX);
				b.WriteFloat(mVelY);
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
				mCoinAge = b.ReadLong();
				mCoinMotion = (CoinMotion)b.ReadLong();
				mCollectionDistance = b.ReadFloat();
				mCollectX = b.ReadFloat();
				mCollectY = b.ReadFloat();
				mDead = b.ReadBoolean();
				mDisappearCounter = b.ReadLong();
				mFadeCount = b.ReadLong();
				mGroundY = b.ReadLong();
				mHitGround = b.ReadBoolean();
				mIsBeingCollected = b.ReadBoolean();
				mNeedsBouncyArrow = b.ReadBoolean();
				mPosX = b.ReadFloat();
				mPosY = b.ReadFloat();
				if (b.ReadBoolean())
				{
					mPottedPlantSpec = new PottedPlant();
					mPottedPlantSpec.Load(b);
				}
				else
				{
					mPottedPlantSpec = null;
				}
				mRow = b.ReadLong();
				mScale = b.ReadFloat();
				mTimesDropped = b.ReadLong();
				mType = (CoinType)b.ReadLong();
				mUsableSeedType = (SeedType)b.ReadLong();
				mVelX = b.ReadFloat();
				mVelY = b.ReadFloat();
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				return false;
			}
			return true;
		}

		public void ScoreCoin()
		{
			Die();
			if (IsSun())
			{
				int sunValue = GetSunValue();
				mBoard.AddSunMoney(sunValue);
			}
			else if (IsMoney())
			{
				int coinValue = GetCoinValue(mType);
				mApp.mPlayerInfo.AddCoins(coinValue);
				if (mBoard != null)
				{
					mBoard.mCoinsCollected += coinValue;
				}
			}
			if (mType == CoinType.COIN_DIAMOND && mBoard != null)
			{
				mBoard.mDiamondsCollected++;
			}
		}

		public void UpdateCollected()
		{
			int num;
			int num2;
			if (IsSun())
			{
				num = Constants.Board_SunCoin_CollectTarget.X;
				num2 = Constants.Board_SunCoin_CollectTarget.Y;
			}
			else if (IsMoney())
			{
				num = 130 - Constants.Board_Offset_AspectRatio_Correction;
				num2 = 550;
				if (mApp.GetDialog(4) != null)
				{
					num = 662;
					num2 = 546;
				}
				else if (mApp.mCrazyDaveState != 0 || mApp.mGameMode == GameMode.GAMEMODE_CHALLENGE_ZEN_GARDEN)
				{
					num = Constants.ZenGarden_MoneyTarget_X;
				}
			}
			else if (IsPresentWithAdvice())
			{
				num = 35;
				num2 = 487;
			}
			else
			{
				if (mType == CoinType.COIN_AWARD_PRESENT || mType == CoinType.COIN_PRESENT_PLANT)
				{
					mDisappearCounter += 3;
					if (mDisappearCounter >= 200)
					{
						StartFade();
					}
					return;
				}
				if (!IsLevelAward())
				{
					if (mType == CoinType.COIN_USABLE_SEED_PACKET)
					{
						mDisappearCounter += 3;
					}
					return;
				}
				num = Constants.Coin_AwardSeedpacket_Pos.X - mWidth / 2;
				num2 = Constants.Coin_AwardSeedpacket_Pos.Y - mHeight / 2;
				mDisappearCounter += 3;
			}
			if (IsLevelAward())
			{
				mScale = TodCommon.TodAnimateCurveFloat(0, 400, mDisappearCounter, 1.01f, 2f, TodCurves.CURVE_EASE_IN_OUT);
				mPosX = TodCommon.TodAnimateCurveFloat(0, 350, mDisappearCounter, mCollectX, num, TodCurves.CURVE_EASE_OUT);
				mPosY = TodCommon.TodAnimateCurveFloat(0, 350, mDisappearCounter, mCollectY, num2, TodCurves.CURVE_EASE_OUT);
				return;
			}
			float num3 = Math.Abs(mPosX - (float)num);
			float num4 = Math.Abs(mPosY - (float)num2);
			if (mPosX > (float)num)
			{
				mPosX -= num3 / 7f;
			}
			else if (mPosX < (float)num)
			{
				mPosX += num3 / 7f;
			}
			if (mPosY > (float)num2)
			{
				mPosY -= num4 / 7f;
			}
			else if (mPosY < (float)num2)
			{
				mPosY += num4 / 7f;
			}
			mCollectionDistance = (float)Math.Sqrt(num4 * num4 + num3 * num3);
			if (IsPresentWithAdvice())
			{
				if (!(mCollectionDistance < Constants.InvertAndScale(15f)))
				{
					return;
				}
				if (!mBoard.mHelpDisplayed[64])
				{
					if (mType == CoinType.COIN_PRESENT_MINIGAMES)
					{
						mBoard.DisplayAdvice("[UNLOCKED_MINIGAMES]", MessageStyle.MESSAGE_STYLE_HINT_TALL_UNLOCKMESSAGE, AdviceType.ADVICE_UNLOCKED_MODE);
					}
					else if (mType == CoinType.COIN_PRESENT_PUZZLE_MODE)
					{
						mBoard.DisplayAdvice("[UNLOCKED_PUZZLE_MODE]", MessageStyle.MESSAGE_STYLE_HINT_TALL_UNLOCKMESSAGE, AdviceType.ADVICE_UNLOCKED_MODE);
					}
				}
				else if (mBoard.mHelpIndex != AdviceType.ADVICE_UNLOCKED_MODE || !mBoard.mAdvice.IsBeingDisplayed())
				{
					Die();
				}
			}
			else
			{
				float num5 = 8f;
				if (IsMoney())
				{
					num5 = 12f;
				}
				if (mCollectionDistance < num5 && !mScored)
				{
					ScoreCoin();
					mScored = true;
				}
				mScale = TodCommon.ClampFloat(mCollectionDistance * 0.05f, 0.5f, 1f);
				mScale *= GetSunScale();
			}
		}

		public SexyColor GetColor()
		{
			if ((IsSun() || IsMoney()) && mIsBeingCollected)
			{
				float num = TodCommon.ClampFloat(mCollectionDistance * 0.035f, 0.35f, 1f);
				return new SexyColor(255, 255, 255, (int)(255f * num), false);
			}
			if (mFadeCount > 0)
			{
				int theAlpha = TodCommon.TodAnimateCurve(15, 0, mFadeCount, 255, 0, TodCurves.CURVE_LINEAR);
				return new SexyColor(255, 255, 255, theAlpha, false);
			}
			return SexyColor.White;
		}

		public bool IsMoney()
		{
			if (mType == CoinType.COIN_SILVER || mType == CoinType.COIN_GOLD || mType == CoinType.COIN_DIAMOND)
			{
				return true;
			}
			return false;
		}

		public bool IsSun()
		{
			if (mType == CoinType.COIN_SUN || mType == CoinType.COIN_SMALLSUN || mType == CoinType.COIN_LARGESUN)
			{
				return true;
			}
			return false;
		}

		public float GetSunScale()
		{
			if (mType == CoinType.COIN_SMALLSUN)
			{
				return 0.5f;
			}
			if (mType == CoinType.COIN_LARGESUN)
			{
				return 2f;
			}
			return 1f;
		}

		public SeedType GetFinalSeedPacketType()
		{
			if (mApp.IsFirstTimeAdventureMode() && mBoard != null && mBoard.mLevel <= 50)
			{
				return mApp.GetAwardSeedForLevel(mBoard.mLevel);
			}
			return SeedType.SEED_NONE;
		}

		public bool IsLevelAward()
		{
			if (mType == CoinType.COIN_FINAL_SEED_PACKET || mType == CoinType.COIN_TROPHY || mType == CoinType.COIN_SHOVEL || mType == CoinType.COIN_CARKEYS || mType == CoinType.COIN_ALMANAC || mType == CoinType.COIN_TACO || mType == CoinType.COIN_BACON || mType == CoinType.COIN_NOTE || mType == CoinType.COIN_AWARD_MONEY_BAG || mType == CoinType.COIN_AWARD_BAG_DIAMOND || mType == CoinType.COIN_AWARD_PRESENT || mType == CoinType.COIN_WATERING_CAN || mType == CoinType.COIN_AWARD_CHOCOLATE)
			{
				return true;
			}
			return false;
		}

		public bool CoinGetsBouncyArrow()
		{
			if (IsLevelAward())
			{
				return true;
			}
			if ((mType == CoinType.COIN_SILVER || mType == CoinType.COIN_GOLD) && mApp.IsFirstTimeAdventureMode() && mBoard != null && mBoard.mLevel == 11 && !mBoard.mDroppedFirstCoin)
			{
				return true;
			}
			if (IsPresentWithAdvice())
			{
				return true;
			}
			return false;
		}

		public void FanOutCoins(CoinType theCoinType, int theNumCoins)
		{
			Debug.ASSERT(mBoard != null);
			for (int i = 0; i < theNumCoins; i++)
			{
				float num = (float)Math.PI / 2f + (float)Math.PI * (float)(i + 1) / (float)(theNumCoins + 1);
				float num2 = mPosX + 20f;
				float num3 = mPosY;
				Coin coin = mBoard.AddCoin((int)num2, (int)num3, theCoinType, CoinMotion.COIN_MOTION_FROM_PRESENT);
				coin.mVelX = 5f * (float)Math.Sin(num);
				coin.mVelY = 5f * (float)Math.Cos(num);
			}
		}

		public int GetDisappearTime()
		{
			int result = 750;
			if (mType == CoinType.COIN_DIAMOND || mType == CoinType.COIN_CHOCOLATE || mHasBouncyArrow || mType == CoinType.COIN_PRESENT_PLANT)
			{
				result = 1500;
			}
			if ((mApp.IsScaryPotterLevel() || mApp.IsSlotMachineLevel()) && mType == CoinType.COIN_USABLE_SEED_PACKET)
			{
				result = 1500;
			}
			if (mApp.mGameMode == GameMode.GAMEMODE_CHALLENGE_ZEN_GARDEN)
			{
				result = 6000;
			}
			return result;
		}

		public void DroppedUsableSeed()
		{
			mIsBeingCollected = false;
			if (mTimesDropped == 0)
			{
				mDisappearCounter = Math.Min(mDisappearCounter, 1200);
			}
			mTimesDropped++;
		}

		public void PlayCollectSound()
		{
			if (mType == CoinType.COIN_USABLE_SEED_PACKET)
			{
				mApp.PlaySample(Resources.SOUND_SEEDLIFT);
			}
			else if (mType == CoinType.COIN_SILVER || mType == CoinType.COIN_GOLD)
			{
				mApp.PlayFoley(FoleyType.FOLEY_COIN);
			}
			else if (mType == CoinType.COIN_DIAMOND)
			{
				mApp.PlaySample(Resources.SOUND_DIAMOND);
			}
			else if (IsSun())
			{
				mApp.PlayFoley(FoleyType.FOLEY_SUN);
			}
			else if (mType == CoinType.COIN_CHOCOLATE || IsPresentWithAdvice() || mType == CoinType.COIN_AWARD_PRESENT || mType == CoinType.COIN_AWARD_CHOCOLATE || mType == CoinType.COIN_PRESENT_PLANT)
			{
				mApp.PlayFoley(FoleyType.FOLEY_PRIZE);
			}
			else if (IsSun())
			{
				mApp.PlayFoley(FoleyType.FOLEY_SUN);
			}
		}

		public void TryAutoCollectAfterLevelAward()
		{
			bool flag = false;
			if (IsMoney() && mCoinMotion != CoinMotion.COIN_MOTION_FROM_PRESENT)
			{
				flag = true;
			}
			else if (IsSun())
			{
				flag = true;
			}
			else if (mType == CoinType.COIN_CHOCOLATE || IsPresentWithAdvice() || mType == CoinType.COIN_PRESENT_PLANT)
			{
				flag = true;
			}
			if (flag)
			{
				PlayCollectSound();
				Collect();
			}
		}

		public bool IsPresentWithAdvice()
		{
			if (mType == CoinType.COIN_PRESENT_MINIGAMES || mType == CoinType.COIN_PRESENT_PUZZLE_MODE)
			{
				return true;
			}
			return false;
		}

		public void PlayGroundSound()
		{
			if (mType == CoinType.COIN_GOLD)
			{
				mApp.PlayFoley(FoleyType.FOLEY_MONEYFALLS);
			}
			else if (mType != CoinType.COIN_PRESENT_PLANT && mType != CoinType.COIN_DIAMOND && mType != CoinType.COIN_CHOCOLATE && mType != CoinType.COIN_AWARD_CHOCOLATE && mType != CoinType.COIN_AWARD_PRESENT && !IsPresentWithAdvice())
			{
				IsLevelAward();
			}
		}

		public void PlayLaunchSound()
		{
			if (mType == CoinType.COIN_PRESENT_PLANT || mType == CoinType.COIN_DIAMOND || mType == CoinType.COIN_CHOCOLATE || mType == CoinType.COIN_AWARD_CHOCOLATE || mType == CoinType.COIN_AWARD_PRESENT || IsPresentWithAdvice())
			{
				mApp.PlayFoley(FoleyType.FOLEY_CHIME);
			}
		}

		public void Loaded()
		{
			if (mType == CoinType.COIN_FINAL_SEED_PACKET)
			{
				LoadSeedPacketImage(GetFinalSeedPacketType());
			}
		}

		public override void LoadingComplete()
		{
			base.LoadingComplete();
			Loaded();
			Coin coin = new Coin();
			coin.CoinInitialize(mX, mY, mType, mCoinMotion);
			mAttachmentID = coin.mAttachmentID;
		}
	}
}
