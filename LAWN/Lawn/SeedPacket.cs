using Sexy;
using Sexy.TodLib;
using System;

namespace Lawn
{
	internal class SeedPacket : GameObject
	{
		public int mRefreshCounter;

		public int mRefreshTime;

		public int mIndex;

		public int mOffsetY;

		public SeedType mPacketType;

		public SeedType mImitaterType;

		public int mSlotMachineCountDown;

		public SeedType mSlotMachiningNextSeed;

		public float mSlotMachiningPosition;

		public bool mActive;

		public bool mRefreshing;

		public int mTimesUsed;

		private TodWeightedArray[] aSeedWeightArray = new TodWeightedArray[53];

		private SeedType[] SLOT_SEED_TYPES = new SeedType[6]
		{
			SeedType.SEED_SUNFLOWER,
			SeedType.SEED_PEASHOOTER,
			SeedType.SEED_SNOWPEA,
			SeedType.SEED_WALLNUT,
			SeedType.SEED_SLOT_MACHINE_SUN,
			SeedType.SEED_SLOT_MACHINE_DIAMOND
		};

		public SeedPacket()
		{
			mPacketType = SeedType.SEED_NONE;
			mImitaterType = SeedType.SEED_NONE;
			mIndex = -1;
			mWidth = Constants.SMALL_SEEDPACKET_WIDTH;
			mHeight = Constants.SMALL_SEEDPACKET_HEIGHT;
			mRefreshCounter = 0;
			mRefreshTime = 0;
			mRefreshing = false;
			mActive = true;
			mOffsetY = 0;
			mSlotMachineCountDown = 0;
			mSlotMachiningNextSeed = SeedType.SEED_NONE;
			mSlotMachiningPosition = 0f;
			mTimesUsed = 0;
			mPosScaled = false;
		}

		public override bool SaveToFile(Sexy.Buffer b)
		{
			try
			{
				base.SaveToFile(b);
				b.WriteBoolean(mActive);
				b.WriteLong((int)mImitaterType);
				b.WriteLong(mIndex);
				b.WriteLong(mOffsetY);
				b.WriteLong((int)mPacketType);
				b.WriteLong(mRefreshCounter);
				b.WriteBoolean(mRefreshing);
				b.WriteLong(mRefreshTime);
				b.WriteLong(mSlotMachineCountDown);
				b.WriteLong((int)mSlotMachiningNextSeed);
				b.WriteFloat(mSlotMachiningPosition);
				b.WriteLong(mTimesUsed);
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
				mActive = b.ReadBoolean();
				mImitaterType = (SeedType)b.ReadLong();
				mIndex = b.ReadLong();
				mOffsetY = b.ReadLong();
				mPacketType = (SeedType)b.ReadLong();
				mRefreshCounter = b.ReadLong();
				mRefreshing = b.ReadBoolean();
				mRefreshTime = b.ReadLong();
				mSlotMachineCountDown = b.ReadLong();
				mSlotMachiningNextSeed = (SeedType)b.ReadLong();
				mSlotMachiningPosition = b.ReadFloat();
				mTimesUsed = b.ReadLong();
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				return false;
			}
			return true;
		}

		public void Update()
		{
			if (mApp.mGameScene != GameScenes.SCENE_PLAYING || mPacketType == SeedType.SEED_NONE)
			{
				return;
			}
			if (mBoard.mMainCounter == 0)
			{
				FlashIfReady();
			}
			if (!mActive && mRefreshing)
			{
				mRefreshCounter++;
				if (mRefreshCounter > mRefreshTime)
				{
					mRefreshCounter = 0;
					mRefreshing = false;
					Activate();
					FlashIfReady();
				}
			}
			if (mSlotMachineCountDown <= 0)
			{
				return;
			}
			mSlotMachineCountDown--;
			float num = TodCommon.TodAnimateCurveFloat(400, 0, mSlotMachineCountDown, 6f, 2f, TodCurves.CURVE_LINEAR);
			mSlotMachiningPosition += num * 0.01f;
			if (mSlotMachiningPosition >= 1f)
			{
				mPacketType = mSlotMachiningNextSeed;
				if (mSlotMachineCountDown >= 0 && mSlotMachineCountDown < 3)
				{
					mSlotMachiningPosition = 0f;
					Activate();
				}
				else
				{
					mSlotMachiningPosition -= 1f;
					PickNextSlotMachineSeed();
				}
			}
			else if (mSlotMachineCountDown == 0)
			{
				mSlotMachineCountDown = 3;
			}
		}

		public void DrawBackground(Graphics g)
		{
			float thePercentDark = 0f;
			int theGrayness = 255;
			if (mBoard.mCursorObject.mCursorType != CursorType.CURSOR_TYPE_PLANT_FROM_BANK || mBoard.mCursorObject.mSeedBankIndex != mIndex)
			{
				GetGraynessAndDarkness(ref theGrayness, ref thePercentDark);
			}
			if (mApp.IsSlotMachineLevel())
			{
				TRect tRect = mApp.mBoard.mChallenge.SlotMachineRect();
				int num = tRect.mY + Constants.Challenge_SlotMachine_Y_Offset;
				float num2 = 0.9f;
				int challenge_SlotMachine_ClipHeight = Constants.Challenge_SlotMachine_ClipHeight;
				float num3 = (float)AtlasResources.IMAGE_SEEDPACKETS.GetCelWidth() * num2;
				int num4 = num - Constants.Challenge_SlotMachine_Y_Pos + TodCommon.FloatRoundToInt(mSlotMachiningPosition * (float)(-challenge_SlotMachine_ClipHeight));
				Graphics @new = Graphics.GetNew(g);
				@new.mTransY = 0;
				@new.mTransX = mBoard.mX;
				int challenge_SlotMachine_Gap = Constants.Challenge_SlotMachine_Gap;
				int num5 = tRect.mX + Constants.Challenge_SlotMachine_Offset + mIndex * TodCommon.FloatRoundToInt((float)challenge_SlotMachine_Gap + num3);
				int challenge_SlotMachine_Shadow_Offset = Constants.Challenge_SlotMachine_Shadow_Offset;
				@new.ClipRect(num5, num, (int)num3, challenge_SlotMachine_ClipHeight);
				@new.HardwareClip();
				if (mSlotMachineCountDown > 0)
				{
					DrawSeedPacketSlotMachine(@new, num5, num4, mPacketType, SeedType.SEED_NONE, theGrayness, num2);
					DrawSeedPacketSlotMachine(@new, num5, (float)num4 + (float)AtlasResources.IMAGE_SEEDPACKETS.GetCelHeight() * num2 - (float)challenge_SlotMachine_Shadow_Offset, mSlotMachiningNextSeed, SeedType.SEED_NONE, theGrayness, num2);
				}
				else
				{
					DrawSeedPacketSlotMachine(@new, num5, num4, mPacketType, SeedType.SEED_NONE, theGrayness, num2);
				}
				@new.EndHardwareClip();
				@new.PrepareForReuse();
			}
			else
			{
				bool theDrawCostBackground = !mBoard.HasConveyorBeltSeedBank() && !mApp.IsSlotMachineLevel();
				DrawSmallSeedPacket(g, 0f, mOffsetY, mPacketType, mImitaterType, 0f, theGrayness, false, true, true, theDrawCostBackground);
			}
		}

		public void Draw(Graphics g)
		{
			if (!mBoard.HasConveyorBeltSeedBank() && !mApp.IsSlotMachineLevel())
			{
				DrawSmallSeedPacket(g, 0f, mOffsetY, mPacketType, mImitaterType, 0f, 255, true, true, false, false);
			}
		}

		public void DrawOverlay(Graphics g)
		{
			float thePercentDark = 0f;
			int theGrayness = 255;
			bool flag = mBoard.mCursorObject.mCursorType == CursorType.CURSOR_TYPE_PLANT_FROM_BANK && mBoard.mCursorObject.mSeedBankIndex == mIndex;
			if (!flag)
			{
				GetGraynessAndDarkness(ref theGrayness, ref thePercentDark);
			}
			if (mSlotMachineCountDown > 0)
			{
				int num = TodCommon.FloatRoundToInt(mSlotMachiningPosition * (float)(-mHeight));
				Graphics @new = Graphics.GetNew(g);
				@new.ClipRect(0, 0, mWidth, mHeight);
				DrawSmallSeedPacket(@new, 0f, num, mPacketType, SeedType.SEED_NONE, 0f, 128, false, false, false, false);
				DrawSmallSeedPacket(@new, 0f, num + mHeight, mSlotMachiningNextSeed, SeedType.SEED_NONE, 0f, 128, false, false, false, false);
				@new.PrepareForReuse();
			}
			else
			{
				DrawSmallSeedPacket(g, 0f, mOffsetY, mPacketType, mImitaterType, thePercentDark, theGrayness, false, true, false, false);
				if (flag)
				{
					g.DrawImage(AtlasResources.IMAGE_SELECTED_PACKET, Constants.SeedPacket_Selector_Pos.X, mOffsetY - Constants.SeedPacket_Selector_Pos.Y);
				}
			}
		}

		internal static void DrawSeedType(Graphics g, float x, float y, SeedType theSeedType, SeedType theImitaterType)
		{
			Plant.DrawSeedType(g, theSeedType, theImitaterType, DrawVariation.VARIATION_NORMAL, x, y);
		}

		public static void DrawSmallSeedPacket(Graphics g, float x, float y, SeedType theSeedType, SeedType theImitaterType, float thePercentDark, int theGrayness, bool theDrawCost, bool theUseCurrentCost, bool theDrawBackground, bool theDrawCostBackground)
		{
			SeedType seedType = theSeedType;
			if (theSeedType == SeedType.SEED_IMITATER && theImitaterType != SeedType.SEED_NONE)
			{
				seedType = theImitaterType;
			}
			if (seedType == SeedType.SEED_LEFTPEATER)
			{
				seedType = SeedType.SEED_SPROUT;
			}
			g.SetDrawMode(Graphics.DrawMode.DRAWMODE_NORMAL);
			g.SetColor(new SexyColor(255, 255, 255, 255));
			if (theGrayness != 255)
			{
				g.SetColor(new SexyColor(theGrayness, theGrayness, theGrayness));
				g.SetColorizeImages(true);
			}
			else if (thePercentDark > 0f)
			{
				g.SetColor(new SexyColor(128, 128, 128, 255));
				g.SetColorizeImages(true);
			}
			if (theDrawBackground)
			{
				switch (theSeedType)
				{
				case SeedType.SEED_SLOT_MACHINE_SUN:
					g.DrawImage(AtlasResources.IMAGE_SEEDPACKET_SUN, x, y);
					break;
				case SeedType.SEED_SLOT_MACHINE_DIAMOND:
					g.DrawImage(AtlasResources.IMAGE_SEEDPACKET_DIAMOND, x, y);
					break;
				case SeedType.SEED_ZOMBIQUARIUM_SNORKEL:
					g.DrawImage(AtlasResources.IMAGE_SEEDPACKET_ZOMBIEQUARIUM, x, y);
					break;
				case SeedType.SEED_ZOMBIQUARIUM_TROPHY:
					g.DrawImage(AtlasResources.IMAGE_SEEDPACKET_TROPHY, x, y);
					break;
				case SeedType.SEED_BEGHOULED_BUTTON_SHUFFLE:
					g.DrawImage(AtlasResources.IMAGE_SEEDPACKET_SHUFFLE, x, y);
					break;
				case SeedType.SEED_BEGHOULED_BUTTON_CRATER:
					g.DrawImage(AtlasResources.IMAGE_SEEDPACKET_CRATER, x, y);
					break;
				default:
					if (GlobalStaticVars.gLawnApp.IsIZombieLevel())
					{
						DrawIZombieSeedPacket(GlobalStaticVars.gLawnApp, g, x, y, seedType, thePercentDark, theGrayness, theDrawCost, theUseCurrentCost, theDrawBackground, theDrawCostBackground);
						break;
					}
					g.DrawImageCel(AtlasResources.IMAGE_SEEDPACKETS, (int)x, (int)y, (int)seedType);
					if (theSeedType == SeedType.SEED_IMITATER)
					{
						g.SetColor(new SexyColor(0, 255, 0, 128));
						g.FillRect((int)x, (int)y, Constants.SMALL_SEEDPACKET_WIDTH, Constants.SMALL_SEEDPACKET_HEIGHT);
					}
					break;
				}
			}
			if (theDrawCostBackground)
			{
				Image theImage = (theSeedType == SeedType.SEED_IMITATER) ? AtlasResources.IMAGE_SEEDPACKETS_GRAY_TAB : ((theSeedType >= SeedType.SEED_GATLINGPEA) ? AtlasResources.IMAGE_SEEDPACKETS_PURPLE_TAB : AtlasResources.IMAGE_SEEDPACKETS_GREEN_TAB);
				int x2;
				int y2;
				if (GlobalStaticVars.gLawnApp.IsIZombieLevel())
				{
					x2 = Constants.SeedPacket_Cost_IZombie.X;
					y2 = Constants.SeedPacket_Cost_IZombie.Y;
				}
				else
				{
					x2 = Constants.SeedPacket_Cost.X;
					y2 = Constants.SeedPacket_Cost.Y;
				}
				g.DrawImageF(theImage, x + (float)x2, y + (float)y2);
			}
			if (thePercentDark > 0f)
			{
				int theHeight = TodCommon.FloatRoundToInt((float)Constants.SMALL_SEEDPACKET_HEIGHT * thePercentDark) + 2;
				g.SetColor(new SexyColor(0, 0, 0, 150));
				g.FillRect((int)x, (int)y, Constants.SMALL_SEEDPACKET_WIDTH, theHeight);
			}
			if (theDrawCost)
			{
				string empty = string.Empty;
				if (((LawnApp)GlobalStaticVars.gSexyAppBase).mBoard != null && ((LawnApp)GlobalStaticVars.gSexyAppBase).mBoard.PlantUsesAcceleratedPricing(seedType))
				{
					if (theUseCurrentCost)
					{
						empty = ((LawnApp)GlobalStaticVars.gSexyAppBase).mBoard.GetCurrentPlantCost(theSeedType, theImitaterType).ToString();
					}
					else
					{
						int cost = Plant.GetCost(theSeedType, theImitaterType);
						empty = Common.StrFormat_(TodStringFile.TodStringTranslate("[SEED_PACKET_COST_PLUS]"), cost);
					}
				}
				else
				{
					int cost2 = Plant.GetCost(theSeedType, theImitaterType);
					empty = LawnApp.ToString(cost2);
				}
				int x3;
				int y3;
				if (GlobalStaticVars.gLawnApp.IsIZombieLevel())
				{
					x3 = Constants.SeedPacket_CostText_IZombie_Pos.X;
					y3 = Constants.SeedPacket_CostText_IZombie_Pos.Y;
				}
				else
				{
					x3 = Constants.SeedPacket_CostText_Pos.X;
					y3 = Constants.SeedPacket_CostText_Pos.Y;
				}
				TodCommon.TodDrawString(g, empty, (int)x + x3, (int)y + y3, Resources.FONT_PICO129, SexyColor.Black, DrawStringJustification.DS_ALIGN_RIGHT);
			}
			g.SetColorizeImages(false);
		}

		private static void DrawIZombieSeedPacket(LawnApp theApp, Graphics g, float x, float y, SeedType aSeedType, float thePercentDark, int theGrayness, bool theDrawCost, bool theUseCurrentCost, bool theDrawBackground, bool theDrawCostBackground)
		{
			ZombieType iZombieTypeFromSeed = GetIZombieTypeFromSeed(aSeedType);
			if (iZombieTypeFromSeed != ZombieType.ZOMBIE_INVALID)
			{
				g.SetScale(0.75f, 0.5f, 0f, 0f);
				g.DrawImage(AtlasResources.IMAGE_ALMANAC_ZOMBIEWINDOW, x + (float)Constants.IZombie_SeedOffset.X, y + (float)Constants.IZombie_SeedOffset.Y);
				Graphics @new = Graphics.GetNew(g);
				@new.SetScale(1f);
				@new.ClipRect((int)x + Constants.IZombie_ClipOffset.X, (int)y + Constants.IZombie_ClipOffset.Y, Constants.IZombie_ClipOffset.Width, Constants.IZombie_ClipOffset.Height);
				@new.HardwareClip();
				theApp.mReanimatorCache.DrawCachedZombie(@new, x + (float)Constants.ZombieOffsets[(int)iZombieTypeFromSeed].X * g.mScaleX, y + (float)Constants.ZombieOffsets[(int)iZombieTypeFromSeed].Y * g.mScaleY, iZombieTypeFromSeed);
				@new.SetColorizeImages(false);
				@new.EndHardwareClip();
				@new.PrepareForReuse();
				g.DrawImage(AtlasResources.IMAGE_ALMANAC_ZOMBIEWINDOW2, x, y);
				g.SetScale(1f, 1f, 0f, 0f);
			}
		}

		private static ZombieType GetIZombieTypeFromSeed(SeedType theSeedType)
		{
			switch (theSeedType)
			{
			case SeedType.SEED_ZOMBIE_NORMAL:
				return ZombieType.ZOMBIE_NORMAL;
			case SeedType.SEED_ZOMBIE_TRAFFIC_CONE:
				return ZombieType.ZOMBIE_TRAFFIC_CONE;
			case SeedType.SEED_ZOMBIE_POLEVAULTER:
				return ZombieType.ZOMBIE_POLEVAULTER;
			case SeedType.SEED_ZOMBIE_DANCER:
				return ZombieType.ZOMBIE_DANCER;
			case SeedType.SEED_ZOMBIE_PAIL:
				return ZombieType.ZOMBIE_PAIL;
			case SeedType.SEED_ZOMBIE_LADDER:
				return ZombieType.ZOMBIE_LADDER;
			case SeedType.SEED_ZOMBIE_BUNGEE:
				return ZombieType.ZOMBIE_BUNGEE;
			case SeedType.SEED_ZOMBIE_DIGGER:
				return ZombieType.ZOMBIE_DIGGER;
			case SeedType.SEED_ZOMBIE_FOOTBALL:
				return ZombieType.ZOMBIE_FOOTBALL;
			case SeedType.SEED_ZOMBIE_BALLOON:
				return ZombieType.ZOMBIE_BALLOON;
			case SeedType.SEED_ZOMBIE_SCREEN_DOOR:
				return ZombieType.ZOMBIE_DOOR;
			case SeedType.SEED_ZOMBONI:
				return ZombieType.ZOMBIE_ZAMBONI;
			case SeedType.SEED_ZOMBIE_POGO:
				return ZombieType.ZOMBIE_POGO;
			case SeedType.SEED_ZOMBIE_GARGANTUAR:
				return ZombieType.ZOMBIE_GARGANTUAR;
			case SeedType.SEED_ZOMBIE_IMP:
				return ZombieType.ZOMBIE_IMP;
			default:
				return ZombieType.ZOMBIE_INVALID;
			}
		}

		private void DrawSeedPacketSlotMachine(Graphics g, float x, float y, SeedType theSeedType, SeedType theImitaterType, int theGrayness, float scale)
		{
			SeedType seedType = theSeedType;
			if (theSeedType == SeedType.SEED_IMITATER && theImitaterType != SeedType.SEED_NONE)
			{
				seedType = theImitaterType;
			}
			if (seedType == SeedType.SEED_LEFTPEATER)
			{
				seedType = SeedType.SEED_SPROUT;
			}
			if (theGrayness != 255)
			{
				g.SetColor(new SexyColor(theGrayness, theGrayness, theGrayness));
				g.SetColorizeImages(true);
			}
			switch (theSeedType)
			{
			case SeedType.SEED_SLOT_MACHINE_SUN:
				TodCommon.TodDrawImageScaledF(g, AtlasResources.IMAGE_SEEDPACKET_SUN, x, y, scale, scale);
				break;
			case SeedType.SEED_SLOT_MACHINE_DIAMOND:
				TodCommon.TodDrawImageScaledF(g, AtlasResources.IMAGE_SEEDPACKET_DIAMOND, x, y, scale, scale);
				break;
			default:
			{
				int theCelCol = (int)seedType % AtlasResources.IMAGE_SEEDPACKETS.mNumCols;
				int theCelRow = (int)seedType / AtlasResources.IMAGE_SEEDPACKETS.mNumCols;
				TodCommon.TodDrawImageCelScaledF(g, AtlasResources.IMAGE_SEEDPACKETS, x, y, theCelCol, theCelRow, scale, scale);
				break;
			}
			}
			g.SetColorizeImages(false);
		}

		public void MouseDown(int x, int y, int theClickCount)
		{
			if (mBoard.mPaused || mApp.mGameScene != GameScenes.SCENE_PLAYING || mPacketType == SeedType.SEED_NONE)
			{
				return;
			}
			if (mApp.IsSlotMachineLevel())
			{
				if (!mBoard.mAdvice.IsBeingDisplayed())
				{
					mBoard.DisplayAdvice("[ADVICE_SLOT_MACHINE_PULL]", MessageStyle.MESSAGE_STYLE_HINT_TALL_FAST, AdviceType.ADVICE_NONE);
				}
				mBoard.mChallenge.mSlotMachineRollCount = Math.Min(mBoard.mChallenge.mSlotMachineRollCount, 2);
				return;
			}
			SeedType seedType = mPacketType;
			if (seedType == SeedType.SEED_IMITATER && mImitaterType != SeedType.SEED_NONE)
			{
				seedType = mImitaterType;
			}
			if (!mApp.mEasyPlantingCheat)
			{
				if (!mActive)
				{
					mApp.PlaySample(Resources.SOUND_BUZZER);
					if (mApp.IsFirstTimeAdventureMode() && mBoard.mLevel == 1 && mBoard.mHelpDisplayed[0])
					{
						mBoard.DisplayAdvice("[ADVICE_SEED_REFRESH]", MessageStyle.MESSAGE_STYLE_TUTORIAL_LEVEL1, AdviceType.ADVICE_SEED_REFRESH);
					}
					return;
				}
				int currentPlantCost = mBoard.GetCurrentPlantCost(mPacketType, mImitaterType);
				if (!mBoard.CanTakeSunMoney(currentPlantCost) && !mBoard.HasConveyorBeltSeedBank())
				{
					mApp.PlaySample(Resources.SOUND_BUZZER);
					mBoard.mOutOfMoneyCounter = 70;
					if (mApp.IsFirstTimeAdventureMode() && mBoard.mLevel == 1 && mBoard.mHelpDisplayed[0])
					{
						mBoard.DisplayAdvice("[ADVICE_CANT_AFFORD_PLANT]", MessageStyle.MESSAGE_STYLE_TUTORIAL_LEVEL1, AdviceType.ADVICE_CANT_AFFORD_PLANT);
					}
					return;
				}
				if (!mBoard.PlantingRequirementsMet(seedType))
				{
					mApp.PlaySample(Resources.SOUND_BUZZER);
					switch (seedType)
					{
					case SeedType.SEED_GATLINGPEA:
						mBoard.DisplayAdvice("[ADVICE_PLANT_NEEDS_REPEATER]", MessageStyle.MESSAGE_STYLE_HINT_LONG, AdviceType.ADVICE_PLANT_NEEDS_REPEATER);
						break;
					case SeedType.SEED_WINTERMELON:
						mBoard.DisplayAdvice("[ADVICE_PLANT_NEEDS_MELONPULT]", MessageStyle.MESSAGE_STYLE_HINT_LONG, AdviceType.ADVICE_PLANT_NEEDS_MELONPULT);
						break;
					case SeedType.SEED_TWINSUNFLOWER:
						mBoard.DisplayAdvice("[ADVICE_PLANT_NEEDS_SUNFLOWER]", MessageStyle.MESSAGE_STYLE_HINT_LONG, AdviceType.ADVICE_PLANT_NEEDS_SUNFLOWER);
						break;
					case SeedType.SEED_SPIKEROCK:
						mBoard.DisplayAdvice("[ADVICE_PLANT_NEEDS_SPIKEWEED]", MessageStyle.MESSAGE_STYLE_HINT_LONG, AdviceType.ADVICE_PLANT_NEEDS_SPIKEWEED);
						break;
					case SeedType.SEED_COBCANNON:
						mBoard.DisplayAdvice("[ADVICE_PLANT_NEEDS_KERNELPULT]", MessageStyle.MESSAGE_STYLE_HINT_LONG, AdviceType.ADVICE_PLANT_NEEDS_KERNELPULT);
						break;
					case SeedType.SEED_GOLD_MAGNET:
						mBoard.DisplayAdvice("[ADVICE_PLANT_NEEDS_MAGNETSHROOM]", MessageStyle.MESSAGE_STYLE_HINT_LONG, AdviceType.ADVICE_PLANT_NEEDS_MAGNETSHROOM);
						break;
					case SeedType.SEED_GLOOMSHROOM:
						mBoard.DisplayAdvice("[ADVICE_PLANT_NEEDS_FUMESHROOM]", MessageStyle.MESSAGE_STYLE_HINT_LONG, AdviceType.ADVICE_PLANT_NEEDS_FUMESHROOM);
						break;
					case SeedType.SEED_CATTAIL:
						mBoard.DisplayAdvice("[ADVICE_PLANT_NEEDS_LILYPAD]", MessageStyle.MESSAGE_STYLE_HINT_LONG, AdviceType.ADVICE_PLANT_NEEDS_LILYPAD);
						break;
					default:
						Debug.ASSERT(false);
						break;
					}
					return;
				}
			}
			mBoard.ClearAdvice(AdviceType.ADVICE_CANT_AFFORD_PLANT);
			mBoard.ClearAdvice(AdviceType.ADVICE_PLANT_NEEDS_REPEATER);
			mBoard.ClearAdvice(AdviceType.ADVICE_PLANT_NEEDS_MELONPULT);
			mBoard.ClearAdvice(AdviceType.ADVICE_PLANT_NEEDS_SUNFLOWER);
			mBoard.ClearAdvice(AdviceType.ADVICE_PLANT_NEEDS_KERNELPULT);
			mBoard.ClearAdvice(AdviceType.ADVICE_PLANT_NEEDS_SPIKEWEED);
			mBoard.ClearAdvice(AdviceType.ADVICE_PLANT_NEEDS_MAGNETSHROOM);
			mBoard.ClearAdvice(AdviceType.ADVICE_PLANT_NEEDS_FUMESHROOM);
			mBoard.ClearAdvice(AdviceType.ADVICE_PLANT_NEEDS_LILYPAD);
			if (mApp.mGameMode == GameMode.GAMEMODE_CHALLENGE_BEGHOULED || mApp.mGameMode == GameMode.GAMEMODE_CHALLENGE_BEGHOULED_TWIST)
			{
				mBoard.mChallenge.BeghouledPacketClicked(this);
				return;
			}
			mBoard.mCursorObject.mType = mPacketType;
			mBoard.mCursorObject.mImitaterType = mImitaterType;
			mBoard.mCursorObject.mCursorType = CursorType.CURSOR_TYPE_PLANT_FROM_BANK;
			mBoard.mCursorObject.mSeedBankIndex = mIndex;
			mApp.PlaySample(Resources.SOUND_SEEDLIFT);
			if (mBoard.mTutorialState == TutorialState.TUTORIAL_LEVEL_1_PICK_UP_PEASHOOTER)
			{
				mBoard.SetTutorialState(TutorialState.TUTORIAL_LEVEL_1_PLANT_PEASHOOTER);
			}
			else if (mBoard.mTutorialState == TutorialState.TUTORIAL_LEVEL_2_PICK_UP_SUNFLOWER)
			{
				if (mPacketType == SeedType.SEED_SUNFLOWER)
				{
					mBoard.SetTutorialState(TutorialState.TUTORIAL_LEVEL_2_PLANT_SUNFLOWER);
				}
				else
				{
					mBoard.SetTutorialState(TutorialState.TUTORIAL_LEVEL_2_REFRESH_SUNFLOWER);
				}
			}
			else if (mBoard.mTutorialState == TutorialState.TUTORIAL_MORESUN_PICK_UP_SUNFLOWER)
			{
				if (mPacketType == SeedType.SEED_SUNFLOWER)
				{
					mBoard.SetTutorialState(TutorialState.TUTORIAL_MORESUN_PLANT_SUNFLOWER);
				}
				else
				{
					mBoard.SetTutorialState(TutorialState.TUTORIAL_MORESUN_REFRESH_SUNFLOWER);
				}
			}
			else if (mBoard.mTutorialState == TutorialState.TUTORIAL_WHACK_A_ZOMBIE_PICK_SEED || mBoard.mTutorialState == TutorialState.TUTORIAL_WHACK_A_ZOMBIE_BEFORE_PICK_SEED)
			{
				mBoard.SetTutorialState(TutorialState.TUTORIAL_WHACK_A_ZOMBIE_COMPLETED);
			}
			Deactivate();
		}

		public bool MouseHitTest(int theX, int theY, HitResult theHitResult)
		{
			if (mSlotMachineCountDown > 0 || mPacketType == SeedType.SEED_NONE)
			{
				theHitResult.mObject = null;
				theHitResult.mObjectType = GameObjectType.OBJECT_TYPE_NONE;
				return false;
			}
			if (theX >= mX && theX < mX + mWidth && theY >= mY + mOffsetY && theY < mY + mHeight + mOffsetY)
			{
				theHitResult.mObject = this;
				theHitResult.mObjectType = GameObjectType.OBJECT_TYPE_SEEDPACKET;
				return true;
			}
			theHitResult.mObject = null;
			theHitResult.mObjectType = GameObjectType.OBJECT_TYPE_NONE;
			return false;
		}

		public void Deactivate()
		{
			mActive = false;
			mRefreshCounter = 0;
			mRefreshTime = 0;
			mRefreshing = false;
		}

		public void Activate()
		{
			Debug.ASSERT(mPacketType != SeedType.SEED_NONE);
			mActive = true;
		}

		public void PickNextSlotMachineSeed()
		{
			int theTimeAge = mBoard.CountPlantByType(SeedType.SEED_PEASHOOTER);
			for (int i = 0; i < aSeedWeightArray.Length; i++)
			{
				if (aSeedWeightArray[i] != null)
				{
					aSeedWeightArray[i].PrepareForReuse();
				}
				aSeedWeightArray[i] = TodWeightedArray.GetNewTodWeightedArray();
			}
			int num = 0;
			for (int j = 0; j < 6; j++)
			{
				SeedType seedType = SLOT_SEED_TYPES[j];
				int num2 = 100;
				if (seedType == SeedType.SEED_PEASHOOTER)
				{
					num2 = TodCommon.TodAnimateCurve(0, 5, theTimeAge, 200, 100, TodCurves.CURVE_LINEAR);
				}
				if (seedType == SeedType.SEED_SLOT_MACHINE_DIAMOND)
				{
					num2 = 30;
				}
				if (mIndex == 2 && seedType != SeedType.SEED_SLOT_MACHINE_DIAMOND && (seedType == mBoard.mSeedBank.mSeedPackets[0].mSlotMachiningNextSeed || seedType == mBoard.mSeedBank.mSeedPackets[1].mSlotMachiningNextSeed))
				{
					num2 += num2 / 2;
				}
				aSeedWeightArray[num].mItem = (int)seedType;
				aSeedWeightArray[num].mWeight = num2;
				num++;
			}
			mSlotMachiningNextSeed = (SeedType)TodCommon.TodPickFromWeightedArray(aSeedWeightArray, num);
		}

		public void SlotMachineStart()
		{
			mSlotMachineCountDown = 400;
			mSlotMachiningPosition = 0f;
			PickNextSlotMachineSeed();
		}

		public void WasPlanted()
		{
			Debug.ASSERT(mPacketType != SeedType.SEED_NONE);
			if (mBoard.HasConveyorBeltSeedBank())
			{
				mBoard.mSeedBank.RemoveSeed(mIndex);
			}
			else if (mApp.IsSlotMachineLevel())
			{
				Deactivate();
			}
			else if (mApp.mGameMode == GameMode.GAMEMODE_CHALLENGE_LAST_STAND && mBoard.mChallenge.mChallengeState != ChallengeState.STATECHALLENGE_LAST_STAND_ONSLAUGHT)
			{
				mTimesUsed++;
				mActive = true;
				FlashIfReady();
			}
			else
			{
				mTimesUsed++;
				mRefreshing = true;
				mRefreshTime = Plant.GetRefreshTime(mPacketType, mImitaterType);
			}
		}

		public void FlashIfReady()
		{
			if (CanPickUp() && !mApp.mEasyPlantingCheat)
			{
				if (mBoard.mTutorialState == TutorialState.TUTORIAL_LEVEL_1_REFRESH_PEASHOOTER)
				{
					mBoard.SetTutorialState(TutorialState.TUTORIAL_LEVEL_1_PICK_UP_PEASHOOTER);
				}
				else if (mBoard.mTutorialState == TutorialState.TUTORIAL_LEVEL_2_REFRESH_SUNFLOWER && mPacketType == SeedType.SEED_SUNFLOWER)
				{
					mBoard.SetTutorialState(TutorialState.TUTORIAL_LEVEL_2_PICK_UP_SUNFLOWER);
				}
				else if (mBoard.mTutorialState == TutorialState.TUTORIAL_MORESUN_REFRESH_SUNFLOWER && mPacketType == SeedType.SEED_SUNFLOWER)
				{
					mBoard.SetTutorialState(TutorialState.TUTORIAL_MORESUN_PICK_UP_SUNFLOWER);
				}
			}
		}

		public bool CanPickUp()
		{
			if (mBoard.mPaused || mApp.mGameScene != GameScenes.SCENE_PLAYING)
			{
				return false;
			}
			if (mPacketType == SeedType.SEED_NONE)
			{
				return false;
			}
			SeedType theSeedType = mPacketType;
			if (mPacketType == SeedType.SEED_IMITATER && mImitaterType != SeedType.SEED_NONE)
			{
				theSeedType = mImitaterType;
			}
			if (mApp.IsSlotMachineLevel())
			{
				return false;
			}
			if (!mApp.mEasyPlantingCheat)
			{
				if (!mActive)
				{
					return false;
				}
				int currentPlantCost = mBoard.GetCurrentPlantCost(mPacketType, mImitaterType);
				if (!mBoard.CanTakeSunMoney(currentPlantCost) && !mBoard.HasConveyorBeltSeedBank())
				{
					return false;
				}
				if (!mBoard.PlantingRequirementsMet(theSeedType))
				{
					return false;
				}
			}
			return true;
		}

		public void SetPacketType(SeedType theSeedType, SeedType theImitaterType)
		{
			mPacketType = theSeedType;
			mImitaterType = theImitaterType;
			mRefreshCounter = 0;
			mRefreshTime = 0;
			mRefreshing = false;
			mActive = true;
			SeedType theSeedtype = theSeedType;
			if (theSeedType == SeedType.SEED_IMITATER && theImitaterType != SeedType.SEED_NONE)
			{
				theSeedtype = theImitaterType;
			}
			if (mApp.mGameMode != GameMode.GAMEMODE_CHALLENGE_BEGHOULED && mApp.mGameMode != GameMode.GAMEMODE_CHALLENGE_BEGHOULED_TWIST && mApp.mGameMode != GameMode.GAMEMODE_CHALLENGE_LAST_STAND && !mApp.IsIZombieLevel() && !mApp.IsScaryPotterLevel() && !mApp.IsWhackAZombieLevel() && (!mApp.IsSurvivalMode() || mBoard.mChallenge.mSurvivalStage <= 0))
			{
				if ((Plant.IsUpgrade(theSeedtype) && !((LawnApp)GlobalStaticVars.gSexyAppBase).IsSurvivalMode()) || Plant.GetRefreshTime(mPacketType, mImitaterType) == 5000)
				{
					mRefreshTime = 3500;
					mRefreshing = true;
					mActive = false;
				}
				else if (Plant.IsUpgrade(theSeedtype) && ((LawnApp)GlobalStaticVars.gSexyAppBase).IsSurvivalMode())
				{
					mRefreshTime = 8000;
					mRefreshing = true;
					mActive = false;
				}
				else if (Plant.GetRefreshTime(mPacketType, mImitaterType) == 3000)
				{
					mRefreshTime = 2000;
					mRefreshing = true;
					mActive = false;
				}
			}
		}

		public void GetGraynessAndDarkness(ref int theGrayness, ref float thePercentDark)
		{
			float num = 0f;
			if (!mActive)
			{
				num = ((mRefreshTime != 0) ? ((float)(mRefreshTime - mRefreshCounter) / (float)mRefreshTime) : 1f);
			}
			SeedType theSeedType = mPacketType;
			if (mPacketType == SeedType.SEED_IMITATER && mImitaterType != SeedType.SEED_NONE)
			{
				theSeedType = mImitaterType;
			}
			bool flag = true;
			if (mBoard.HasConveyorBeltSeedBank() || mApp.IsSlotMachineLevel())
			{
				flag = false;
			}
			int currentPlantCost = mBoard.GetCurrentPlantCost(mPacketType, mImitaterType);
			int num2 = 255;
			if (mApp.mGameMode == GameMode.GAMEMODE_CHALLENGE_BEGHOULED && !mActive)
			{
				num2 = 64;
			}
			else if (mApp.mGameMode == GameMode.GAMEMODE_CHALLENGE_BEGHOULED_TWIST && !mActive)
			{
				num2 = 64;
			}
			else if (mApp.mGameScene != GameScenes.SCENE_PLAYING)
			{
				num2 = mBoard.mSeedBank.mCutSceneDarken;
				num = 0f;
			}
			else if (mBoard.mTutorialState == TutorialState.TUTORIAL_LEVEL_1_PICK_UP_PEASHOOTER && mBoard.mTutorialTimer == -1 && mPacketType == SeedType.SEED_PEASHOOTER)
			{
				num2 = TodCommon.GetFlashingColor(mBoard.mMainCounter, 75).mRed;
			}
			else if (mBoard.mTutorialState == TutorialState.TUTORIAL_LEVEL_2_PICK_UP_SUNFLOWER && mPacketType == SeedType.SEED_SUNFLOWER)
			{
				num2 = TodCommon.GetFlashingColor(mBoard.mMainCounter, 75).mRed;
			}
			else if (mBoard.mTutorialState == TutorialState.TUTORIAL_MORESUN_PICK_UP_SUNFLOWER && mPacketType == SeedType.SEED_SUNFLOWER)
			{
				num2 = TodCommon.GetFlashingColor(mBoard.mMainCounter, 75).mRed;
			}
			else if (mBoard.mTutorialState == TutorialState.TUTORIAL_WHACK_A_ZOMBIE_PICK_SEED)
			{
				num2 = TodCommon.GetFlashingColor(mBoard.mMainCounter, 75).mRed;
			}
			else if (mApp.mEasyPlantingCheat)
			{
				num2 = 255;
				num = 0f;
			}
			else if (!mBoard.CanTakeSunMoney(currentPlantCost) && flag)
			{
				num2 = 128;
			}
			else if (num > 0f)
			{
				num2 = 128;
			}
			else if (!mBoard.PlantingRequirementsMet(theSeedType))
			{
				num2 = 128;
			}
			theGrayness = num2;
			thePercentDark = num;
		}

		public int CenterY()
		{
			return mY + mOffsetY + mHeight / 2;
		}

		public override string ToString()
		{
			return string.Format("X:{0}, Y:{1},Seed:{2}", mX, mY, mPacketType);
		}
	}
}
