using Microsoft.Xna.Framework;
using Sexy;
using Sexy.TodLib;
using System;
using System.Collections.Generic;

namespace Lawn
{
	internal class Zombie : GameObject, IComparable
	{
		public enum ZombieAttackType
		{
			ATTACKTYPE_CHEW,
			ATTACKTYPE_DRIVE_OVER,
			ATTACKTYPE_VAULT,
			ATTACKTYPE_LADDER
		}

		public enum ZombieRenderLayerOffset
		{
			ZOMBIE_LAYER_OFFSET_BOBSLED_4,
			ZOMBIE_LAYER_OFFSET_BOBSLED_3,
			ZOMBIE_LAYER_OFFSET_BOBSLED_2,
			ZOMBIE_LAYER_OFFSET_BOBSLED_1,
			ZOMBIE_LAYER_OFFSET_NORMAL,
			ZOMBIE_LAYER_OFFSET_DOG_WALKER,
			ZOMBIE_LAYER_OFFSET_DOG,
			ZOMBIE_LAYER_OFFSET_DIGGER,
			ZOMBIE_LAYER_OFFSET_ZAMBONI
		}

		public enum ZombieParts
		{
			PART_BODY,
			PART_HEAD,
			PART_HEAD_EATING,
			PART_TONGUE,
			PART_ARM,
			PART_HAIR,
			PART_HEAD_YUCKY,
			PART_ARM_PICKAXE,
			PART_ARM_POLEVAULT,
			PART_ARM_LEASH,
			PART_ARM_FLAG,
			PART_POGO,
			PART_DIGGER
		}

		private const float FADE_SPEED = 30f;

		private const float FADE_TIME_LAND = 100f;

		private const float FADE_SPEED_BUNGEE = 50f;

		private const float FADE_SPEED_BALOON = 50f;

		public ZombieType mZombieType;

		public ZombiePhase mZombiePhase;

		public float mPosX;

		public float mPosY;

		public float mVelX;

		public int mAnimCounter;

		public int mGroanCounter;

		public int mAnimTicksPerFrame;

		public int mAnimFrames;

		public int mFrame;

		public int mPrevFrame;

		public bool mVariant;

		public bool mIsEating;

		public int mJustGotShotCounter;

		public int mShieldJustGotShotCounter;

		public int mShieldRecoilCounter;

		public int mZombieAge;

		public ZombieHeight mZombieHeight;

		public int mPhaseCounter;

		public int mFromWave;

		public bool mDroppedLoot;

		public int mZombieFade;

		public bool mFlatTires;

		public int mUseLadderCol;

		public int mTargetCol;

		public float mAltitude;

		public bool mHitUmbrella;

		public TRect mZombieRect = default(TRect);

		public TRect mZombieAttackRect = default(TRect);

		public int mChilledCounter;

		public int mButteredCounter;

		public int mIceTrapCounter;

		public bool mMindControlled;

		public bool mBlowingAway;

		public bool mHasHead;

		public bool mHasArm;

		public bool mHasObject;

		public bool mInPool;

		public bool mOnHighGround;

		public bool mYuckyFace;

		public int mYuckyFaceCounter;

		public HelmType mHelmType;

		public int mBodyHealth;

		public int mBodyMaxHealth;

		public int mHelmHealth;

		public int mHelmMaxHealth;

		public ShieldType mShieldType;

		public int mShieldHealth;

		public int mShieldMaxHealth;

		public int mFlyingHealth;

		public int mFlyingMaxHealth;

		public bool mDead;

		public Zombie mRelatedZombieID;

		private int mRelatedZombieIDSaved;

		public Zombie[] mFollowerZombieID = new Zombie[GameConstants.MAX_ZOMBIE_FOLLOWERS];

		private int[] mFollowerZombieIDSaved = new int[GameConstants.MAX_ZOMBIE_FOLLOWERS];

		public Zombie mLeaderZombie;

		private int mLeaderZombieIDSaved = -1;

		public bool mPlayingSong;

		public int mParticleOffsetX;

		public int mParticleOffsetY;

		public Attachment mAttachmentID;

		public int mSummonCounter;

		public Reanimation mBodyReanimID;

		public float mScaleZombie;

		public float mVelZ;

		public float mOrginalAnimRate;

		public Plant mTargetPlantID;

		private int mTargetPlantIDSaved;

		public int mBossMode;

		public int mTargetRow;

		public int mBossBungeeCounter;

		public int mBossStompCounter;

		public int mBossHeadCounter;

		public Reanimation mBossFireBallReanimID;

		public Reanimation mSpecialHeadReanimID;

		public int mFireballRow;

		public bool mIsFireBall;

		public Reanimation mMoweredReanimID;

		public int mLastPortalX;

		public bool mHasGroundTrack;

		public bool mSummonedDancers;

		public bool mSurprised;

		private int mGroundTrackIndex;

		public bool mUsesClipping;

		private bool doLoot = true;

		private bool doParticle = true;

		private static Stack<Zombie> unusedObjects;

		private static TodWeightedGridArray[] aPicks;

		public bool mIsButterShowing;

		public bool cachedZombieRectUpToDate;

		private TRect cachedZombieRect;

		public int mYuckyToRow;

		public bool mYuckySwitchRowsLate;

		public bool draggedByTangleKelp;

		private bool justLoaded;

		private string lastPlayedReanimName;

		private ReanimLoopType lastPlayedReanimLoopType;

		private byte lastPlayedReanimBlendTime;

		private float lastPlayedReanimAnimRate;

		public static bool WinningZombieReachedDesiredY;

		public bool mHasHelm = true;

		public bool mHasShield = true;

		public static void PreallocateMemory()
		{
			for (int i = 0; i < 200; i++)
			{
				new Zombie().PrepareForReuse();
			}
		}

		public static Zombie GetNewZombie()
		{
			if (unusedObjects.Count > 0)
			{
				Zombie zombie = unusedObjects.Pop();
				zombie.Reset();
				return zombie;
			}
			return new Zombie();
		}

		static Zombie()
		{
			unusedObjects = new Stack<Zombie>();
			aPicks = new TodWeightedGridArray[Constants.GRIDSIZEX * Constants.MAX_GRIDSIZEY];
			WinningZombieReachedDesiredY = false;
			for (int i = 0; i < aPicks.Length; i++)
			{
				aPicks[i] = TodWeightedGridArray.GetNewTodWeightedGridArray();
			}
		}

		private Zombie()
		{
			Reset();
		}

		public override void PrepareForReuse()
		{
			unusedObjects.Push(this);
		}

		protected override void Reset()
		{
			base.Reset();
			lastPlayedReanimName = string.Empty;
			lastPlayedReanimLoopType = ReanimLoopType.REANIM_PLAY_ONCE;
			lastPlayedReanimBlendTime = 0;
			lastPlayedReanimAnimRate = 0f;
			doLoot = true;
			doParticle = true;
			draggedByTangleKelp = false;
			cachedZombieRectUpToDate = false;
			mZombieType = ZombieType.ZOMBIE_NORMAL;
			mZombiePhase = ZombiePhase.PHASE_ZOMBIE_NORMAL;
			mPosX = 0f;
			mPosY = 0f;
			mVelX = 0f;
			mAnimCounter = 0;
			mGroanCounter = 0;
			mAnimTicksPerFrame = 0;
			mAnimFrames = 0;
			mFrame = 0;
			mPrevFrame = 0;
			mVariant = false;
			mIsEating = false;
			mJustGotShotCounter = 0;
			mShieldJustGotShotCounter = 0;
			mShieldRecoilCounter = 0;
			mZombieAge = 0;
			mZombieHeight = ZombieHeight.HEIGHT_ZOMBIE_NORMAL;
			mPhaseCounter = 0;
			mFromWave = 0;
			mDroppedLoot = false;
			mZombieFade = 0;
			mFlatTires = false;
			mUseLadderCol = 0;
			mTargetCol = 0;
			mAltitude = 0f;
			mHitUmbrella = false;
			mZombieRect = default(TRect);
			mZombieAttackRect = default(TRect);
			mChilledCounter = 0;
			mButteredCounter = 0;
			mIceTrapCounter = 0;
			mMindControlled = false;
			mBlowingAway = false;
			mHasHead = false;
			mHasArm = false;
			mHasObject = false;
			mInPool = false;
			mOnHighGround = false;
			mYuckyFace = false;
			mYuckyFaceCounter = 0;
			mHelmType = HelmType.HELMTYPE_NONE;
			mBodyHealth = 0;
			mBodyMaxHealth = 0;
			mHelmHealth = 0;
			mHelmMaxHealth = 0;
			mShieldType = ShieldType.SHIELDTYPE_NONE;
			mShieldHealth = 0;
			mShieldMaxHealth = 0;
			mFlyingHealth = 0;
			mFlyingMaxHealth = 0;
			mDead = false;
			mRelatedZombieID = null;
			for (int i = 0; i < mFollowerZombieID.Length; i++)
			{
				mFollowerZombieID[i] = null;
			}
			mLeaderZombieIDSaved = -1;
			mLeaderZombie = null;
			mPlayingSong = false;
			mParticleOffsetX = 0;
			mParticleOffsetY = 0;
			mSummonCounter = 0;
			mBodyReanimID = null;
			mScaleZombie = 0f;
			mVelZ = 0f;
			mOrginalAnimRate = 0f;
			mTargetPlantID = null;
			mBossMode = 0;
			mTargetRow = 0;
			mBossBungeeCounter = 0;
			mBossStompCounter = 0;
			mBossHeadCounter = 0;
			mBossFireBallReanimID = null;
			mSpecialHeadReanimID = null;
			mFireballRow = 0;
			mIsFireBall = false;
			mMoweredReanimID = null;
			mLastPortalX = 0;
			mHasGroundTrack = false;
			mSummonedDancers = false;
			mUsesClipping = false;
			mSurprised = false;
			mGroundTrackIndex = -1;
			mHasArm = true;
			mHasHead = true;
			mHasHelm = true;
			mHasShield = true;
		}

		int IComparable.CompareTo(object toCompare)
		{
			Zombie zombie = (Zombie)toCompare;
			return mX.CompareTo(zombie.mX);
		}

		public void ZombieInitialize(int theRow, ZombieType theType, bool theVariant, Zombie theParentZombie, int theFromWave)
		{
			Debug.ASSERT(theType >= ZombieType.ZOMBIE_NORMAL && theType < ZombieType.NUM_ZOMBIE_TYPES);
			mRow = theRow;
			mFromWave = theFromWave;
			mPosX = 800f - Constants.InvertAndScale(20f) + RandomNumbers.NextNumber(Constants.Zombie_StartRandom_Offset);
			mPosX += Constants.Zombie_StartOffset;
			mPosY = GetPosYBasedOnRow(theRow);
			mWidth = 120;
			mHeight = 120;
			mVelX = 0f;
			mVelZ = 0f;
			mFrame = 0;
			mPrevFrame = 0;
			mZombieType = theType;
			mVariant = theVariant;
			mIsEating = false;
			mJustGotShotCounter = 0;
			mShieldJustGotShotCounter = 0;
			mShieldRecoilCounter = 0;
			mChilledCounter = 0;
			mIceTrapCounter = 0;
			mButteredCounter = 0;
			mMindControlled = false;
			mBlowingAway = false;
			mHasHead = true;
			mHasArm = true;
			mHasObject = false;
			mInPool = false;
			mOnHighGround = false;
			mHelmType = HelmType.HELMTYPE_NONE;
			mShieldType = ShieldType.SHIELDTYPE_NONE;
			mYuckyFace = false;
			mYuckyFaceCounter = 0;
			mAnimCounter = 0;
			mGroanCounter = TodCommon.RandRangeInt(400, 500);
			mAnimTicksPerFrame = 12;
			mAnimFrames = 12;
			mZombieAge = 0;
			mTargetCol = -1;
			mZombiePhase = ZombiePhase.PHASE_ZOMBIE_NORMAL;
			mZombieHeight = ZombieHeight.HEIGHT_ZOMBIE_NORMAL;
			mPhaseCounter = 0;
			mHitUmbrella = false;
			mDroppedLoot = false;
			mRelatedZombieID = null;
			mZombieRect = new TRect(36, 0, 42, 115);
			mZombieAttackRect = new TRect(50, 0, 20, 115);
			mPlayingSong = false;
			mZombieFade = -1;
			mFlatTires = false;
			mUseLadderCol = -1;
			mShieldHealth = 0;
			mHelmHealth = 0;
			mFlyingHealth = 0;
			mAttachmentID = null;
			mSummonCounter = 0;
			mBossStompCounter = -1;
			mBossBungeeCounter = -1;
			mBossHeadCounter = -1;
			mBodyReanimID = null;
			mScaleZombie = 1f;
			mAltitude = 0f;
			mOrginalAnimRate = 0f;
			mTargetPlantID = null;
			mBossMode = 0;
			mBossFireBallReanimID = null;
			mSpecialHeadReanimID = null;
			mTargetRow = -1;
			mFireballRow = -1;
			mIsFireBall = false;
			mMoweredReanimID = null;
			mLastPortalX = -1;
			mHasGroundTrack = false;
			mSummonedDancers = false;
			mSurprised = false;
			for (int i = 0; i < GameConstants.MAX_ZOMBIE_FOLLOWERS; i++)
			{
				mFollowerZombieID[i] = null;
			}
			if (mBoard != null && mBoard.IsFlagWave(mFromWave))
			{
				mPosX += 40f;
			}
			PickRandomSpeed();
			mBodyHealth = 270;
			RenderLayer theRenderLayer = RenderLayer.RENDER_LAYER_ZOMBIE;
			int theLayerOffset = 4;
			ZombieDefinition zombieDefinition = GetZombieDefinition(mZombieType);
			if (zombieDefinition.mReanimationType != ReanimationType.REANIM_NONE)
			{
				LoadReanim(zombieDefinition.mReanimationType);
			}
			switch (theType)
			{
			case ZombieType.ZOMBIE_NORMAL:
				LoadPlainZombieReanim();
				break;
			case ZombieType.ZOMBIE_DUCKY_TUBE:
				LoadPlainZombieReanim();
				break;
			case ZombieType.ZOMBIE_TRAFFIC_CONE:
				LoadPlainZombieReanim();
				ReanimShowPrefix("anim_cone", 0);
				ReanimShowPrefix("anim_hair", -1);
				mHelmType = HelmType.HELMTYPE_TRAFFIC_CONE;
				mHelmHealth = 370;
				break;
			case ZombieType.ZOMBIE_PAIL:
				LoadPlainZombieReanim();
				ReanimShowPrefix("anim_bucket", 0);
				ReanimShowPrefix("anim_hair", -1);
				mHelmType = HelmType.HELMTYPE_PAIL;
				mHelmHealth = 1100;
				break;
			case ZombieType.ZOMBIE_DOOR:
				mShieldType = ShieldType.SHIELDTYPE_DOOR;
				mShieldHealth = 1100;
				mPosX += 60f;
				LoadPlainZombieReanim();
				AttachShield();
				break;
			case ZombieType.ZOMBIE_YETI:
				mBodyHealth = 1350;
				mPhaseCounter = TodCommon.RandRangeInt(1500, 2000);
				mHasObject = true;
				mZombieAttackRect = new TRect(20, 0, 50, 115);
				mPosX += 60f;
				break;
			case ZombieType.ZOMBIE_LADDER:
				mBodyHealth = 500;
				mShieldType = ShieldType.SHIELDTYPE_LADDER;
				mShieldHealth = 500;
				mZombieAttackRect = new TRect(10, 0, 50, 115);
				if (IsOnBoard())
				{
					mZombiePhase = ZombiePhase.PHASE_LADDER_CARRYING;
					StartWalkAnim(0);
				}
				AttachShield();
				break;
			default:
				if (mZombieType == ZombieType.ZOMBIE_BUNGEE)
				{
					mBodyHealth = 450;
					mAnimFrames = 4;
					mAltitude = GameConstants.BUNGEE_ZOMBIE_HEIGHT + TodCommon.RandRangeInt(0, 150);
					mVelX = 0f;
					if (IsOnBoard())
					{
						PickBungeeZombieTarget(-1);
						if (mDead)
						{
							return;
						}
						mZombiePhase = ZombiePhase.PHASE_BUNGEE_DIVING;
					}
					else
					{
						mZombiePhase = ZombiePhase.PHASE_BUNGEE_CUTSCENE;
						mPhaseCounter = TodCommon.RandRangeInt(0, 200);
					}
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_drop, ReanimLoopType.REANIM_LOOP, 0, 24f);
					Reanimation reanimation = mApp.ReanimationGet(mBodyReanimID);
					reanimation.AssignRenderGroupToPrefix("Zombie_bungi_rightarm_lower2", GameConstants.RENDER_GROUP_ARMS);
					reanimation.AssignRenderGroupToPrefix("Zombie_bungi_rightarm_hand2", GameConstants.RENDER_GROUP_ARMS);
					reanimation.AssignRenderGroupToPrefix("Zombie_bungi_leftarm_lower2", GameConstants.RENDER_GROUP_ARMS);
					reanimation.AssignRenderGroupToPrefix("Zombie_bungi_leftarm_hand2", GameConstants.RENDER_GROUP_ARMS);
					reanimation.SetTruncateDisappearingFrames(string.Empty, false);
					theRenderLayer = RenderLayer.RENDER_LAYER_GRAVE_STONE;
					theLayerOffset = 7;
					mZombieRect = new TRect(-20, 22, 110, 94);
					mZombieAttackRect = new TRect(0, 0, 0, 0);
					mVariant = false;
					break;
				}
				if (mZombieType == ZombieType.ZOMBIE_FOOTBALL)
				{
					mZombieRect = new TRect(50, 0, 57, 115);
					ReanimShowPrefix("anim_hair", -1);
					mHelmType = HelmType.HELMTYPE_FOOTBALL;
					mHelmHealth = 1400;
					mAnimTicksPerFrame = 6;
					mVariant = false;
					break;
				}
				if (mZombieType == ZombieType.ZOMBIE_DIGGER)
				{
					mHelmType = HelmType.HELMTYPE_DIGGER;
					mHelmHealth = 100;
					mVariant = false;
					mHasObject = true;
					mZombieRect = new TRect(50, 0, 28, 115);
					Reanimation reanimation2 = mApp.ReanimationGet(mBodyReanimID);
					reanimation2.SetTruncateDisappearingFrames(string.Empty, false);
					if (!IsOnBoard())
					{
						mZombiePhase = ZombiePhase.PHASE_DIGGER_CUTSCENE;
						break;
					}
					mZombiePhase = ZombiePhase.PHASE_DIGGER_TUNNELING;
					AddAttachedParticle(60, 100, ParticleEffect.PARTICLE_DIGGER_TUNNEL);
					theLayerOffset = 7;
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_dig, ReanimLoopType.REANIM_LOOP_FULL_LAST_FRAME, 0, 12f);
					PickRandomSpeed();
					break;
				}
				if (mZombieType == ZombieType.ZOMBIE_POLEVAULTER)
				{
					mBodyHealth = 500;
					mAnimTicksPerFrame = 6;
					mZombiePhase = ZombiePhase.PHASE_POLEVAULTER_PRE_VAULT;
					mHasObject = true;
					mVariant = false;
					mPosX = Constants.WIDE_BOARD_WIDTH + 70 + RandomNumbers.NextNumber(10);
					if (IsOnBoard())
					{
						PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_run, ReanimLoopType.REANIM_LOOP, 0, 0f);
						PickRandomSpeed();
					}
					if (mApp.IsWallnutBowlingLevel())
					{
						mZombieAttackRect = new TRect(-229, 0, 270, 115);
					}
					else
					{
						mZombieAttackRect = new TRect(-29, 0, 70, 115);
					}
					break;
				}
				if (mZombieType == ZombieType.ZOMBIE_DOLPHIN_RIDER)
				{
					mBodyHealth = 500;
					mAnimTicksPerFrame = 6;
					mZombiePhase = ZombiePhase.PHASE_DOLPHIN_WALKING;
					mVariant = false;
					if (IsOnBoard())
					{
						PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_walkdolphin, ReanimLoopType.REANIM_LOOP, 0, 0f);
						PickRandomSpeed();
					}
					SetupWaterTrack(ref GlobalMembersReanimIds.ReanimTrackId_zombie_dolphinrider_whitewater);
					SetupWaterTrack(ref GlobalMembersReanimIds.ReanimTrackId_zombie_dolphinrider_dolphininwater);
					break;
				}
				if (mZombieType == ZombieType.ZOMBIE_GARGANTUAR || mZombieType == ZombieType.ZOMBIE_REDEYE_GARGANTUAR)
				{
					mBodyHealth = 3000;
					mAnimFrames = 24;
					mAnimTicksPerFrame = 8;
					mWidth = 180;
					mHeight = 180;
					mPosX = Constants.WIDE_BOARD_WIDTH + 45 + RandomNumbers.NextNumber(10);
					mZombieRect = new TRect(-17, -38, 125, 154);
					mZombieAttackRect = new TRect(-30, -38, 89, 154);
					mVariant = false;
					theLayerOffset = 8;
					mHasObject = true;
					int num = 0;
					int num2 = RandomNumbers.NextNumber(100);
					num = ((IsOnBoard() && mBoard.mLevel != 48) ? ((num2 < 10) ? 2 : ((num2 < 35) ? 1 : 0)) : 0);
					Reanimation reanimation3 = mApp.ReanimationGet(mBodyReanimID);
					switch (num)
					{
					case 2:
						reanimation3.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_gargantuar_telephonepole, AtlasResources.IMAGE_REANIM_ZOMBIE_GARGANTUAR_ZOMBIE);
						break;
					case 1:
						reanimation3.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_gargantuar_telephonepole, AtlasResources.IMAGE_REANIM_ZOMBIE_GARGANTUAR_DUCKXING);
						break;
					}
					if (mZombieType == ZombieType.ZOMBIE_REDEYE_GARGANTUAR)
					{
						reanimation3.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_anim_head1, AtlasResources.IMAGE_REANIM_ZOMBIE_GARGANTUAR_HEAD_REDEYE);
						mBodyHealth = 6000;
					}
					break;
				}
				if (mZombieType == ZombieType.ZOMBIE_ZAMBONI)
				{
					mBodyHealth = 1350;
					mAnimFrames = 2;
					mAnimTicksPerFrame = 8;
					mPosX = Constants.WIDE_BOARD_WIDTH + RandomNumbers.NextNumber(10) + 20;
					theLayerOffset = 8;
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_drive, ReanimLoopType.REANIM_LOOP, 0, 12f);
					mZombieRect = new TRect(0, -13, 153, 140);
					mZombieAttackRect = new TRect(10, -13, 133, 140);
					mVariant = false;
					break;
				}
				if (mZombieType == ZombieType.ZOMBIE_CATAPULT)
				{
					mBodyHealth = 850;
					mPosX = Constants.WIDE_BOARD_WIDTH + 25 + RandomNumbers.NextNumber(10);
					mSummonCounter = 20;
					if (IsOnBoard())
					{
						PlayZombieReanim(ref Reanimation.ReanimTrackId_anim_walk, ReanimLoopType.REANIM_LOOP, 0, 5.5f);
					}
					else
					{
						PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_idle, ReanimLoopType.REANIM_LOOP, 0, 8f);
					}
					mZombieRect = new TRect(0, -13, 153, 140);
					mZombieAttackRect = new TRect(10, -13, 133, 140);
					mVariant = false;
					break;
				}
				if (mZombieType == ZombieType.ZOMBIE_SNORKEL)
				{
					mZombieRect = new TRect(12, 0, 62, 115);
					mZombieAttackRect = new TRect(-5, 0, 55, 115);
					SetupWaterTrack(ref GlobalMembersReanimIds.ReanimTrackId_zombie_snorkle_whitewater);
					SetupWaterTrack(ref GlobalMembersReanimIds.ReanimTrackId_zombie_snorkle_whitewater2);
					mVariant = false;
					mZombiePhase = ZombiePhase.PHASE_SNORKEL_WALKING;
					break;
				}
				if (mZombieType == ZombieType.ZOMBIE_JACK_IN_THE_BOX)
				{
					mBodyHealth = 500;
					mAnimTicksPerFrame = 6;
					int num3 = 450 + RandomNumbers.NextNumber(300);
					if (RandomNumbers.NextNumber(20) == 0)
					{
						num3 /= 3;
					}
					mPhaseCounter = (int)((float)num3 / mVelX) * GameConstants.ZOMBIE_LIMP_SPEED_FACTOR;
					mZombieAttackRect = new TRect(20, 0, 50, 115);
					if (mApp.IsScaryPotterLevel())
					{
						mPhaseCounter = 10;
					}
					if (IsOnBoard())
					{
						mZombiePhase = ZombiePhase.PHASE_JACK_IN_THE_BOX_RUNNING;
					}
					break;
				}
				if (mZombieType == ZombieType.ZOMBIE_BOBSLED)
				{
					theLayerOffset = 3;
					if (theParentZombie != null)
					{
						int j;
						for (j = 0; j < 3 && theParentZombie.mFollowerZombieID[j] != null; j++)
						{
						}
						Debug.ASSERT(j < 3);
						theParentZombie.mFollowerZombieID[j] = mBoard.ZombieGetID(this);
						mRelatedZombieID = mBoard.ZombieGetID(theParentZombie);
						mPosX = theParentZombie.mPosX + (float)((j + 1) * 50);
						switch (j)
						{
						case 0:
							theLayerOffset = 1;
							mAltitude = 9f;
							break;
						case 1:
							theLayerOffset = 2;
							mAltitude = -7f;
							break;
						default:
							theLayerOffset = 0;
							mAltitude = 9f;
							break;
						}
					}
					else
					{
						mPosX = Constants.WIDE_BOARD_WIDTH + 80;
						mAltitude = -10f;
						mHelmType = HelmType.HELMTYPE_BOBSLED;
						mHelmHealth = 300;
						mZombieRect = new TRect(-50, 0, 275, 115);
					}
					mVelX = 0.6f;
					mZombiePhase = ZombiePhase.PHASE_BOBSLED_SLIDING;
					mPhaseCounter = 500;
					mVariant = false;
					if (theFromWave == GameConstants.ZOMBIE_WAVE_CUTSCENE)
					{
						PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_jump, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 0, 20f);
						Reanimation reanimation4 = mApp.ReanimationGet(mBodyReanimID);
						reanimation4.mAnimTime = 1f;
						mAltitude = 18f;
					}
					else if (IsOnBoard())
					{
						PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_push, ReanimLoopType.REANIM_LOOP, 0, 30f);
					}
					break;
				}
				if (theType == ZombieType.ZOMBIE_FLAG)
				{
					mHasObject = true;
					LoadPlainZombieReanim();
					Reanimation reanimation5 = mApp.ReanimationGet(mBodyReanimID);
					Reanimation reanimation6 = mApp.AddReanimation(0f, 0f, 0, ReanimationType.REANIM_ZOMBIE_FLAGPOLE);
					reanimation6.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_zombie_flag, ReanimLoopType.REANIM_LOOP, 0, 15f);
					mSpecialHeadReanimID = mApp.ReanimationGetID(reanimation6);
					ReanimatorTrackInstance trackInstanceByName = reanimation5.GetTrackInstanceByName(GlobalMembersReanimIds.ReanimTrackId_zombie_flaghand);
					GlobalMembersAttachment.AttachReanim(ref trackInstanceByName.mAttachmentID, reanimation6, 0f, 0f);
					reanimation5.mFrameBasePose = 0;
					mPosX = Constants.WIDE_BOARD_WIDTH;
					break;
				}
				if (mZombieType == ZombieType.ZOMBIE_POGO)
				{
					mVariant = false;
					mZombiePhase = ZombiePhase.PHASE_POGO_BOUNCING;
					mPhaseCounter = RandomNumbers.NextNumber(GameConstants.POGO_BOUNCE_TIME) + 1;
					mHasObject = true;
					mBodyHealth = 500;
					mZombieAttackRect = new TRect(10, 0, 30, 115);
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_pogo, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 0, 40f);
					Reanimation reanimation7 = mApp.ReanimationGet(mBodyReanimID);
					reanimation7.mAnimTime = 1f;
					break;
				}
				switch (theType)
				{
				case ZombieType.ZOMBIE_NEWSPAPER:
					mZombieAttackRect = new TRect(20, 0, 50, 115);
					mZombiePhase = ZombiePhase.PHASE_NEWSPAPER_READING;
					mShieldType = ShieldType.SHIELDTYPE_NEWSPAPER;
					mShieldHealth = 150;
					mVariant = false;
					AttachShield();
					break;
				case ZombieType.ZOMBIE_BALLOON:
				{
					Reanimation theAttachReanim = mApp.ReanimationGet(mBodyReanimID);
					theAttachReanim.SetTruncateDisappearingFrames(string.Empty, false);
					if (IsOnBoard())
					{
						mZombiePhase = ZombiePhase.PHASE_BALLOON_FLYING;
						mAltitude = 25f;
						PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_idle, ReanimLoopType.REANIM_LOOP, 0, theAttachReanim.mAnimRate);
					}
					else
					{
						float animRate = TodCommon.RandRangeFloat(8f, 10f);
						SetAnimRate(animRate);
					}
					Reanimation reanimation20 = mApp.AddReanimation(0f, 0f, 0, zombieDefinition.mReanimationType);
					reanimation20.SetFramesForLayer(GlobalMembersReanimIds.ReanimTrackId_propeller);
					reanimation20.mLoopType = ReanimLoopType.REANIM_LOOP_FULL_LAST_FRAME;
					reanimation20.AttachToAnotherReanimation(ref theAttachReanim, GlobalMembersReanimIds.ReanimTrackId_hat);
					mFlyingHealth = 20;
					mZombieRect = new TRect(36, 30, 42, 115);
					mZombieAttackRect = new TRect(20, 30, 50, 115);
					mVariant = false;
					break;
				}
				case ZombieType.ZOMBIE_DANCER:
					if (!IsOnBoard())
					{
						PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_moonwalk, ReanimLoopType.REANIM_LOOP, 0, 12f);
					}
					else
					{
						mZombiePhase = ZombiePhase.PHASE_DANCER_DANCING_IN;
						mPhaseCounter = 200 + RandomNumbers.NextNumber(12);
						mVelX = 0.5f;
						PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_moonwalk, ReanimLoopType.REANIM_LOOP, 0, 24f);
					}
					mBodyHealth = 500;
					mVariant = false;
					break;
				case ZombieType.ZOMBIE_BACKUP_DANCER:
					if (!IsOnBoard())
					{
						PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_armraise, ReanimLoopType.REANIM_LOOP, 0, 12f);
					}
					ReanimShowPrefix("Zombie_disco_outerarm_upper_bone", -1);
					mZombiePhase = ZombiePhase.PHASE_DANCER_DANCING_LEFT;
					mVariant = false;
					break;
				default:
					if (mZombieType == ZombieType.ZOMBIE_IMP)
					{
						if (!IsOnBoard())
						{
							PlayZombieReanim(ref Reanimation.ReanimTrackId_anim_walk, ReanimLoopType.REANIM_LOOP, 0, 12f);
						}
						if (mApp.IsIZombieLevel())
						{
							mBodyHealth = 70;
						}
						break;
					}
					if (mZombieType == ZombieType.ZOMBIE_BOSS)
					{
						mPosX = Constants.BOARD_EXTRA_ROOM;
						mPosY = 0f;
						mZombieRect = new TRect(700, 80, 90, 430);
						mZombieAttackRect = default(TRect);
						theRenderLayer = RenderLayer.RENDER_LAYER_TOP;
						if (mApp.IsAdventureMode() || mApp.IsQuickPlayMode())
						{
							mBodyHealth = 40000;
						}
						else
						{
							mBodyHealth = 60000;
						}
						if (IsOnBoard())
						{
							PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_enter, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 0, 12f);
							mSummonCounter = 500;
							mBossHeadCounter = 5000;
							mZombiePhase = ZombiePhase.PHASE_BOSS_ENTER;
						}
						else
						{
							PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_head_idle, ReanimLoopType.REANIM_LOOP, 0, 12f);
						}
						BossSetupReanim();
						break;
					}
					switch (theType)
					{
					case ZombieType.ZOMBIE_PEA_HEAD:
					{
						LoadPlainZombieReanim();
						ReanimShowPrefix("anim_hair", -1);
						ReanimShowPrefix("anim_head2", -1);
						Reanimation reanimation18 = mApp.ReanimationGet(mBodyReanimID);
						if (IsOnBoard())
						{
							reanimation18.SetFramesForLayer(GlobalMembersReanimIds.ReanimTrackId_anim_walk2);
						}
						ReanimatorTrackInstance trackInstanceByName7 = reanimation18.GetTrackInstanceByName(GlobalMembersReanimIds.ReanimTrackId_anim_head1);
						trackInstanceByName7.mImageOverride = AtlasResources.IMAGE_BLANK;
						Reanimation reanimation19 = mApp.AddReanimation(0f, 0f, 0, ReanimationType.REANIM_PEASHOOTER);
						reanimation19.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_anim_head_idle, ReanimLoopType.REANIM_LOOP, 0, 15f);
						mSpecialHeadReanimID = mApp.ReanimationGetID(reanimation19);
						AttachEffect attachEffect6 = GlobalMembersAttachment.AttachReanim(ref trackInstanceByName7.mAttachmentID, reanimation19, 0f, 0f);
						reanimation18.mFrameBasePose = 0;
						TodCommon.TodScaleRotateTransformMatrix(ref attachEffect6.mOffset.mMatrix, 65f * Constants.S, -8f * Constants.S, 0.2f, -1f, 1f);
						mPhaseCounter = 150;
						mVariant = false;
						break;
					}
					case ZombieType.ZOMBIE_WALLNUT_HEAD:
					{
						LoadPlainZombieReanim();
						ReanimShowPrefix("anim_hair", -1);
						ReanimShowPrefix("anim_head", -1);
						ReanimShowPrefix("Zombie_tie", -1);
						Reanimation reanimation16 = mApp.ReanimationGet(mBodyReanimID);
						if (IsOnBoard())
						{
							reanimation16.SetFramesForLayer(GlobalMembersReanimIds.ReanimTrackId_anim_walk2);
						}
						ReanimatorTrackInstance trackInstanceByName6 = reanimation16.GetTrackInstanceByName(GlobalMembersReanimIds.ReanimTrackId_zombie_body);
						Reanimation reanimation17 = mApp.AddReanimation(0f, 0f, 0, ReanimationType.REANIM_WALLNUT);
						reanimation17.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_anim_idle, ReanimLoopType.REANIM_LOOP, 0, 15f);
						mSpecialHeadReanimID = mApp.ReanimationGetID(reanimation17);
						AttachEffect attachEffect5 = GlobalMembersAttachment.AttachReanim(ref trackInstanceByName6.mAttachmentID, reanimation17, 0f, 0f);
						reanimation16.mFrameBasePose = 0;
						TodCommon.TodScaleRotateTransformMatrix(ref attachEffect5.mOffset.mMatrix, 50f * Constants.S, 0f, 0.2f, -0.8f, 0.8f);
						mHelmType = HelmType.HELMTYPE_WALLNUT;
						mHelmHealth = 1100;
						mVariant = false;
						break;
					}
					case ZombieType.ZOMBIE_TALLNUT_HEAD:
					{
						LoadPlainZombieReanim();
						ReanimShowPrefix("anim_hair", -1);
						ReanimShowPrefix("anim_head", -1);
						ReanimShowPrefix("Zombie_tie", -1);
						Reanimation reanimation14 = mApp.ReanimationGet(mBodyReanimID);
						if (IsOnBoard())
						{
							reanimation14.SetFramesForLayer(GlobalMembersReanimIds.ReanimTrackId_anim_walk2);
						}
						ReanimatorTrackInstance trackInstanceByName5 = reanimation14.GetTrackInstanceByName(GlobalMembersReanimIds.ReanimTrackId_zombie_body);
						Reanimation reanimation15 = mApp.AddReanimation(0f, 0f, 0, ReanimationType.REANIM_TALLNUT);
						reanimation15.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_anim_idle, ReanimLoopType.REANIM_LOOP, 0, 15f);
						mSpecialHeadReanimID = mApp.ReanimationGetID(reanimation15);
						AttachEffect attachEffect4 = GlobalMembersAttachment.AttachReanim(ref trackInstanceByName5.mAttachmentID, reanimation15, 0f, 0f);
						reanimation14.mFrameBasePose = 0;
						TodCommon.TodScaleRotateTransformMatrix(ref attachEffect4.mOffset.mMatrix, 37f * Constants.S, 0f, 0.2f, -0.8f, 0.8f);
						mHelmType = HelmType.HELMTYPE_TALLNUT;
						mHelmHealth = 2200;
						mVariant = false;
						mPosX += 30f;
						break;
					}
					default:
						if (mZombieType == ZombieType.ZOMBIE_JALAPENO_HEAD)
						{
							LoadPlainZombieReanim();
							ReanimShowPrefix("anim_hair", -1);
							ReanimShowPrefix("anim_head", -1);
							ReanimShowPrefix("Zombie_tie", -1);
							Reanimation reanimation8 = mApp.ReanimationGet(mBodyReanimID);
							if (IsOnBoard())
							{
								reanimation8.SetFramesForLayer(GlobalMembersReanimIds.ReanimTrackId_anim_walk2);
							}
							ReanimatorTrackInstance trackInstanceByName2 = reanimation8.GetTrackInstanceByName(GlobalMembersReanimIds.ReanimTrackId_zombie_body);
							Reanimation reanimation9 = mApp.AddReanimation(0f, 0f, 0, ReanimationType.REANIM_JALAPENO);
							reanimation9.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_anim_idle, ReanimLoopType.REANIM_LOOP, 0, 15f);
							mSpecialHeadReanimID = mApp.ReanimationGetID(reanimation9);
							AttachEffect attachEffect = GlobalMembersAttachment.AttachReanim(ref trackInstanceByName2.mAttachmentID, reanimation9, 0f, 0f);
							reanimation8.mFrameBasePose = 0;
							TodCommon.TodScaleRotateTransformMatrix(ref attachEffect.mOffset.mMatrix, 55f * Constants.S, -5f * Constants.S, 0.2f, -1f, 1f);
							mVariant = false;
							mBodyHealth = 500;
							int num4 = 275 + RandomNumbers.NextNumber(175);
							mPhaseCounter = (int)((float)num4 / mVelX) * GameConstants.ZOMBIE_LIMP_SPEED_FACTOR;
							break;
						}
						switch (theType)
						{
						case ZombieType.ZOMBIE_GATLING_HEAD:
						{
							LoadPlainZombieReanim();
							ReanimShowPrefix("anim_hair", -1);
							ReanimShowPrefix("anim_head2", -1);
							Reanimation reanimation12 = mApp.ReanimationGet(mBodyReanimID);
							if (IsOnBoard())
							{
								reanimation12.SetFramesForLayer(GlobalMembersReanimIds.ReanimTrackId_anim_walk2);
							}
							ReanimatorTrackInstance trackInstanceByName4 = reanimation12.GetTrackInstanceByName(GlobalMembersReanimIds.ReanimTrackId_anim_head1);
							trackInstanceByName4.mImageOverride = AtlasResources.IMAGE_BLANK;
							Reanimation reanimation13 = mApp.AddReanimation(0f, 0f, 0, ReanimationType.REANIM_GATLINGPEA);
							reanimation13.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_anim_head_idle, ReanimLoopType.REANIM_LOOP, 0, 15f);
							mSpecialHeadReanimID = mApp.ReanimationGetID(reanimation13);
							AttachEffect attachEffect3 = GlobalMembersAttachment.AttachReanim(ref trackInstanceByName4.mAttachmentID, reanimation13, 0f, 0f);
							reanimation12.mFrameBasePose = 0;
							TodCommon.TodScaleRotateTransformMatrix(ref attachEffect3.mOffset.mMatrix, 65f * Constants.S, -18f * Constants.S, 0.2f, -1f, 1f);
							mPhaseCounter = 150;
							mVariant = false;
							break;
						}
						case ZombieType.ZOMBIE_SQUASH_HEAD:
						{
							LoadPlainZombieReanim();
							ReanimShowPrefix("anim_hair", -1);
							ReanimShowPrefix("anim_head2", -1);
							Reanimation reanimation10 = mApp.ReanimationGet(mBodyReanimID);
							if (IsOnBoard())
							{
								reanimation10.SetFramesForLayer(GlobalMembersReanimIds.ReanimTrackId_anim_walk2);
							}
							ReanimatorTrackInstance trackInstanceByName3 = reanimation10.GetTrackInstanceByName(GlobalMembersReanimIds.ReanimTrackId_anim_head1);
							trackInstanceByName3.mImageOverride = AtlasResources.IMAGE_BLANK;
							Reanimation reanimation11 = mApp.AddReanimation(0f, 0f, 0, ReanimationType.REANIM_SQUASH);
							reanimation11.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_anim_idle, ReanimLoopType.REANIM_LOOP, 0, 15f);
							mSpecialHeadReanimID = mApp.ReanimationGetID(reanimation11);
							AttachEffect attachEffect2 = GlobalMembersAttachment.AttachReanim(ref trackInstanceByName3.mAttachmentID, reanimation11, 0f, 0f);
							reanimation10.mFrameBasePose = 0;
							TodCommon.TodScaleRotateTransformMatrix(ref attachEffect2.mOffset.mMatrix, 55f * Constants.S, -15f * Constants.S, 0.2f, -0.75f, 0.75f);
							mZombiePhase = ZombiePhase.PHASE_SQUASH_PRE_LAUNCH;
							mVariant = false;
							break;
						}
						}
						break;
					}
					break;
				}
				break;
			}
			if (mApp.IsLittleTroubleLevel() && (IsOnBoard() || theFromWave == GameConstants.ZOMBIE_WAVE_CUTSCENE))
			{
				mScaleZombie = 0.5f;
				mBodyHealth /= 4;
				mHelmHealth /= 4;
				mShieldHealth /= 4;
				mFlyingHealth /= 4;
			}
			UpdateAnimSpeed();
			if (mVariant)
			{
				ReanimShowPrefix("anim_tongue", 0);
			}
			mBodyMaxHealth = mBodyHealth;
			mHelmMaxHealth = mHelmHealth;
			mShieldMaxHealth = mShieldHealth;
			mFlyingMaxHealth = mFlyingHealth;
			mDead = false;
			mX = (int)mPosX;
			mY = (int)mPosY;
			mRenderOrder = Board.MakeRenderOrder(theRenderLayer, mRow, theLayerOffset);
			if (mZombieHeight == ZombieHeight.HEIGHT_ZOMBIQUARIUM)
			{
				mBodyMaxHealth = 300;
			}
			if (IsOnBoard())
			{
				PlayZombieAppearSound();
				StartZombieSound();
			}
			UpdateReanim();
			if (mBodyReanimID != null && mBodyReanimID.TrackExists("zombie_butter"))
			{
				mBodyReanimID.AssignRenderGroupToTrack("zombie_butter", -1);
			}
		}

		public void Dispose()
		{
			GlobalMembersAttachment.AttachmentDie(ref mAttachmentID);
			StopZombieSound();
			PrepareForReuse();
		}

		public void Animate()
		{
			mPrevFrame = mFrame;
			if (mZombiePhase == ZombiePhase.PHASE_JACK_IN_THE_BOX_POPPING || mZombiePhase == ZombiePhase.PHASE_NEWSPAPER_MADDENING || mZombiePhase == ZombiePhase.PHASE_DIGGER_RISING || mZombiePhase == ZombiePhase.PHASE_DIGGER_TUNNELING_PAUSE_WITHOUT_AXE || mZombiePhase == ZombiePhase.PHASE_DIGGER_RISE_WITHOUT_AXE || mZombiePhase == ZombiePhase.PHASE_DIGGER_STUNNED || IsImmobilizied())
			{
				return;
			}
			mAnimCounter += 3;
			if (mYuckyFace)
			{
				for (int i = 0; i < 3; i++)
				{
					if (mYuckyFace)
					{
						UpdateYuckyFace();
					}
				}
			}
			if (mIsEating && mHasHead)
			{
				int num = 6;
				if (mChilledCounter > 0)
				{
					num = 12;
				}
				if (mAnimCounter >= mAnimFrames * num)
				{
					mAnimCounter = num;
				}
				mFrame = mAnimCounter / num;
				Reanimation reanimation = mApp.ReanimationTryToGet(mBodyReanimID);
				if (reanimation != null)
				{
					float theEventTime = 0.14f;
					float theEventTime2 = 0.68f;
					if (mZombieType == ZombieType.ZOMBIE_POLEVAULTER)
					{
						theEventTime = 0.38f;
						theEventTime2 = 0.8f;
					}
					else if (mZombieType == ZombieType.ZOMBIE_NEWSPAPER || mZombieType == ZombieType.ZOMBIE_LADDER)
					{
						theEventTime = 0.42f;
						theEventTime2 = 0.42f;
					}
					else if (mZombieType == ZombieType.ZOMBIE_JACK_IN_THE_BOX)
					{
						theEventTime = 0.53f;
						theEventTime2 = 0.53f;
					}
					else if (mZombieType == ZombieType.ZOMBIE_BOBSLED)
					{
						theEventTime = 0.33f;
						theEventTime2 = 0.83f;
					}
					else if (mZombieType == ZombieType.ZOMBIE_IMP)
					{
						theEventTime = 0.33f;
						theEventTime2 = 0.79f;
					}
					if (reanimation.ShouldTriggerTimedEvent(theEventTime) || reanimation.ShouldTriggerTimedEvent(theEventTime2))
					{
						AnimateChewSound();
						AnimateChewEffect();
					}
				}
				else
				{
					if (mAnimCounter == 3 * num)
					{
						AnimateChewSound();
					}
					if (mAnimCounter == 6 * num && !mMindControlled)
					{
						AnimateChewEffect();
					}
				}
			}
			else
			{
				if (mAnimCounter >= mAnimFrames * mAnimTicksPerFrame)
				{
					mAnimCounter = 0;
				}
				mFrame = mAnimCounter / mAnimTicksPerFrame;
			}
		}

		public void CheckIfPreyCaught()
		{
			if (mZombieType == ZombieType.ZOMBIE_BUNGEE || mZombieType == ZombieType.ZOMBIE_GARGANTUAR || mZombieType == ZombieType.ZOMBIE_REDEYE_GARGANTUAR || mZombieType == ZombieType.ZOMBIE_ZAMBONI || mZombieType == ZombieType.ZOMBIE_CATAPULT || mZombieType == ZombieType.ZOMBIE_BOSS || IsBouncingPogo() || IsBobsledTeamWithSled() || mZombiePhase == ZombiePhase.PHASE_POLEVAULTER_IN_VAULT || mZombiePhase == ZombiePhase.PHASE_POLEVAULTER_PRE_VAULT || mZombiePhase == ZombiePhase.PHASE_NEWSPAPER_MADDENING || mZombiePhase == ZombiePhase.PHASE_DIGGER_RISING || mZombiePhase == ZombiePhase.PHASE_DIGGER_TUNNELING_PAUSE_WITHOUT_AXE || mZombiePhase == ZombiePhase.PHASE_DIGGER_RISE_WITHOUT_AXE || mZombiePhase == ZombiePhase.PHASE_DIGGER_STUNNED || mZombiePhase == ZombiePhase.PHASE_RISING_FROM_GRAVE || mZombiePhase == ZombiePhase.PHASE_IMP_GETTING_THROWN || mZombiePhase == ZombiePhase.PHASE_IMP_LANDING || mZombiePhase == ZombiePhase.PHASE_DANCER_RISING || mZombiePhase == ZombiePhase.PHASE_DANCER_SNAPPING_FINGERS || mZombiePhase == ZombiePhase.PHASE_DANCER_SNAPPING_FINGERS_WITH_LIGHT || mZombiePhase == ZombiePhase.PHASE_DANCER_SNAPPING_FINGERS_HOLD || mZombiePhase == ZombiePhase.PHASE_DOLPHIN_WALKING || mZombiePhase == ZombiePhase.PHASE_DOLPHIN_WALKING_WITHOUT_DOLPHIN || mZombiePhase == ZombiePhase.PHASE_DOLPHIN_INTO_POOL || mZombiePhase == ZombiePhase.PHASE_DOLPHIN_RIDING || mZombiePhase == ZombiePhase.PHASE_DOLPHIN_IN_JUMP || mZombiePhase == ZombiePhase.PHASE_SNORKEL_INTO_POOL || mZombiePhase == ZombiePhase.PHASE_SNORKEL_WALKING || mZombiePhase == ZombiePhase.PHASE_LADDER_PLACING || mZombieHeight == ZombieHeight.HEIGHT_GETTING_BUNGEE_DROPPED || mZombieHeight == ZombieHeight.HEIGHT_UP_LADDER || mZombieHeight == ZombieHeight.HEIGHT_IN_TO_POOL || mZombieHeight == ZombieHeight.HEIGHT_OUT_OF_POOL || IsTangleKelpTarget() || mZombieHeight == ZombieHeight.HEIGHT_FALLING || !mHasHead || IsFlying())
			{
				return;
			}
			int num = GameConstants.TICKS_BETWEEN_EATS;
			if (mChilledCounter > 0)
			{
				num *= 6;
			}
			if (mZombieAge % num != 0)
			{
				return;
			}
			Zombie zombie = FindZombieTarget();
			if (zombie != null)
			{
				EatZombie(zombie);
				return;
			}
			if (!mMindControlled)
			{
				Plant plant = FindPlantTarget(ZombieAttackType.ATTACKTYPE_CHEW);
				if (plant != null)
				{
					EatPlant(plant);
					return;
				}
			}
			if ((!mApp.IsIZombieLevel() || !mBoard.mChallenge.IZombieEatBrain(this)) && mIsEating)
			{
				StopEating();
			}
		}

		public void EatZombie(Zombie theZombie)
		{
			theZombie.TakeDamage(GameConstants.TICKS_BETWEEN_EATS, 9u);
			StartEating();
			if (theZombie.mBodyHealth <= 0)
			{
				mApp.PlaySample(Resources.SOUND_GULP);
			}
		}

		public void EatPlant(Plant thePlant)
		{
			if (mZombiePhase == ZombiePhase.PHASE_DANCER_DANCING_IN)
			{
				mPhaseCounter = 1;
			}
			else
			{
				if (mYuckyFace)
				{
					return;
				}
				if (mBoard.GetLadderAt(thePlant.mPlantCol, thePlant.mRow) != null && mZombieType != ZombieType.ZOMBIE_DIGGER)
				{
					StopEating();
					if (mZombieHeight == ZombieHeight.HEIGHT_ZOMBIE_NORMAL && mUseLadderCol != thePlant.mPlantCol)
					{
						mZombieHeight = ZombieHeight.HEIGHT_UP_LADDER;
						mUseLadderCol = thePlant.mPlantCol;
					}
					return;
				}
				StartEating();
				if ((thePlant.mSeedType == SeedType.SEED_JALAPENO || thePlant.mSeedType == SeedType.SEED_CHERRYBOMB || thePlant.mSeedType == SeedType.SEED_DOOMSHROOM || thePlant.mSeedType == SeedType.SEED_ICESHROOM || thePlant.mSeedType == SeedType.SEED_HYPNOSHROOM || thePlant.mState == PlantState.STATE_FLOWERPOT_INVULNERABLE || thePlant.mState == PlantState.STATE_LILYPAD_INVULNERABLE || thePlant.mState == PlantState.STATE_SQUASH_LOOK || thePlant.mState == PlantState.STATE_SQUASH_PRE_LAUNCH) && !thePlant.mIsAsleep)
				{
					if (mZombieType == ZombieType.ZOMBIE_DANCER && thePlant.mSeedType == SeedType.SEED_HYPNOSHROOM)
					{
						mBoard.GrantAchievement(AchievementId.DiscoisUndead);
					}
				}
				else
				{
					if (thePlant.mSeedType == SeedType.SEED_POTATOMINE && thePlant.mState != 0)
					{
						return;
					}
					bool flag = false;
					if (thePlant.mSeedType == SeedType.SEED_BLOVER)
					{
						flag = true;
					}
					if (thePlant.mSeedType == SeedType.SEED_ICESHROOM && !thePlant.mIsAsleep)
					{
						flag = true;
					}
					if (flag)
					{
						thePlant.DoSpecial();
					}
					else
					{
						if (mChilledCounter > 0 && mZombieAge % 2 == 1)
						{
							return;
						}
						if (mApp.IsIZombieLevel() && thePlant.mSeedType == SeedType.SEED_SUNFLOWER)
						{
							int num = thePlant.mPlantHealth / 40;
							int num2 = (thePlant.mPlantHealth - 3 * GameConstants.TICKS_BETWEEN_EATS) / 40;
							if (num2 < num)
							{
								mBoard.AddCoin(thePlant.mX, thePlant.mY, CoinType.COIN_SUN, CoinMotion.COIN_MOTION_FROM_PLANT);
							}
						}
						thePlant.mPlantHealth -= 3 * GameConstants.TICKS_BETWEEN_EATS;
						thePlant.mRecentlyEatenCountdown = 50;
						if (mApp.IsIZombieLevel() && mJustGotShotCounter < -500 && (thePlant.mSeedType == SeedType.SEED_WALLNUT || thePlant.mSeedType == SeedType.SEED_TALLNUT || thePlant.mSeedType == SeedType.SEED_PUMPKINSHELL))
						{
							thePlant.mPlantHealth -= 3 * GameConstants.TICKS_BETWEEN_EATS;
						}
						if (thePlant.mPlantHealth <= 0)
						{
							mApp.PlaySample(Resources.SOUND_GULP);
							mBoard.mPlantsEaten++;
							thePlant.Die();
							mBoard.mChallenge.ZombieAtePlant(this, thePlant);
							if (mBoard.mLevel >= 2 && mBoard.mLevel <= 4 && mApp.IsFirstTimeAdventureMode() && thePlant.mPlantCol > 4 && mBoard.mPlants.Count < 15 && thePlant.mSeedType == SeedType.SEED_PEASHOOTER)
							{
								mBoard.DisplayAdvice("[ADVICE_PEASHOOTER_DIED]", MessageStyle.MESSAGE_STYLE_HINT_TALL_FAST, AdviceType.ADVICE_PEASHOOTER_DIED);
							}
						}
					}
				}
			}
		}

		public override bool SaveToFile(Sexy.Buffer b)
		{
			base.SaveToFile(b);
			b.WriteLong((int)mZombieType);
			b.WriteBoolean(mVariant);
			b.WriteLong(mFromWave);
			b.WriteFloat(mAltitude);
			b.WriteLong(mAnimCounter);
			b.WriteLong(mAnimFrames);
			b.WriteLong(mAnimTicksPerFrame);
			b.WriteBoolean(mBlowingAway);
			b.WriteLong(mBodyHealth);
			b.WriteLong(mBodyMaxHealth);
			b.WriteLong(mBossBungeeCounter);
			b.WriteLong(mBossHeadCounter);
			b.WriteLong(mBossMode);
			b.WriteLong(mBossStompCounter);
			b.WriteLong(mButteredCounter);
			b.WriteLong(mChilledCounter);
			b.WriteBoolean(mDroppedLoot);
			b.WriteLong(mFireballRow);
			b.WriteBoolean(mFlatTires);
			b.WriteLong(mFlyingHealth);
			b.WriteLong(mFlyingMaxHealth);
			for (int i = 0; i < mFollowerZombieID.Length; i++)
			{
				GameObject.SaveId(mFollowerZombieID[i], b);
			}
			GameObject.SaveId(mLeaderZombie, b);
			b.WriteLong(mFrame);
			b.WriteLong(mGroanCounter);
			b.WriteBoolean(mHasGroundTrack);
			b.WriteBoolean(mHasObject);
			b.WriteLong(mHelmHealth);
			b.WriteLong(mHelmMaxHealth);
			b.WriteBoolean(mHitUmbrella);
			b.WriteLong(mIceTrapCounter);
			b.WriteBoolean(mInPool);
			b.WriteBoolean(mIsEating);
			b.WriteBoolean(mIsFireBall);
			b.WriteLong(mJustGotShotCounter);
			b.WriteLong(mLastPortalX);
			b.WriteBoolean(mMindControlled);
			b.WriteBoolean(mOnHighGround);
			b.WriteFloat(mOrginalAnimRate);
			b.WriteLong(mParticleOffsetX);
			b.WriteLong(mParticleOffsetY);
			b.WriteLong(mPhaseCounter);
			b.WriteBoolean(mPlayingSong);
			b.WriteFloat(mPosX);
			b.WriteFloat(mPosY);
			b.WriteLong(mPrevFrame);
			b.WriteFloat(mPrevTransX);
			b.WriteFloat(mPrevTransY);
			GameObject.SaveId(mRelatedZombieID, b);
			b.WriteFloat(mScaleZombie);
			b.WriteLong(mShieldHealth);
			b.WriteLong(mShieldJustGotShotCounter);
			b.WriteLong(mShieldMaxHealth);
			b.WriteLong(mShieldRecoilCounter);
			b.WriteLong(mSummonCounter);
			b.WriteBoolean(mSummonedDancers);
			b.WriteLong(mTargetCol);
			GameObject.SaveId(mTargetPlantID, b);
			b.WriteLong(mTargetRow);
			b.WriteLong(mUseLadderCol);
			b.WriteBoolean(mUsesClipping);
			b.WriteFloat(mVelX);
			b.WriteFloat(mVelZ);
			b.WriteBoolean(mYuckyFace);
			b.WriteLong(mYuckyFaceCounter);
			b.WriteLong(mZombieAge);
			b.WriteRect(mZombieAttackRect);
			b.WriteLong((int)mZombieHeight);
			b.WriteLong((int)mZombiePhase);
			b.WriteRect(mZombieRect);
			b.WriteString(lastPlayedReanimName);
			b.WriteFloat(lastPlayedReanimAnimRate);
			b.WriteByte(lastPlayedReanimBlendTime);
			b.WriteLong((int)lastPlayedReanimLoopType);
			b.WriteBoolean(mHasArm);
			b.WriteBoolean(mHasHead);
			b.WriteBoolean(mHasHelm);
			b.WriteBoolean(mHasShield);
			return true;
		}

		public override bool LoadFromFile(Sexy.Buffer b)
		{
			base.LoadFromFile(b);
			mZombieType = (ZombieType)b.ReadLong();
			mVariant = b.ReadBoolean();
			mFromWave = b.ReadLong();
			doLoot = false;
			doParticle = false;
			int mRow = base.mRow;
			ZombieInitialize(base.mRow, mZombieType, mVariant, null, mFromWave);
			mAltitude = b.ReadFloat();
			mAnimCounter = b.ReadLong();
			mAnimFrames = b.ReadLong();
			mAnimTicksPerFrame = b.ReadLong();
			mBlowingAway = b.ReadBoolean();
			mBodyHealth = b.ReadLong();
			mBodyMaxHealth = b.ReadLong();
			mBossBungeeCounter = b.ReadLong();
			mBossHeadCounter = b.ReadLong();
			mBossMode = b.ReadLong();
			mBossStompCounter = b.ReadLong();
			mButteredCounter = b.ReadLong();
			mChilledCounter = b.ReadLong();
			mDroppedLoot = b.ReadBoolean();
			mFireballRow = b.ReadLong();
			mFlatTires = b.ReadBoolean();
			mFlyingHealth = b.ReadLong();
			mFlyingMaxHealth = b.ReadLong();
			for (int i = 0; i < mFollowerZombieID.Length; i++)
			{
				mFollowerZombieIDSaved[i] = GameObject.LoadId(b);
			}
			mLeaderZombieIDSaved = GameObject.LoadId(b);
			mFrame = b.ReadLong();
			mGroanCounter = b.ReadLong();
			mHasGroundTrack = b.ReadBoolean();
			mHasObject = b.ReadBoolean();
			mHelmHealth = b.ReadLong();
			mHelmMaxHealth = b.ReadLong();
			mHitUmbrella = b.ReadBoolean();
			mIceTrapCounter = b.ReadLong();
			mInPool = b.ReadBoolean();
			mIsEating = b.ReadBoolean();
			mIsFireBall = b.ReadBoolean();
			mJustGotShotCounter = b.ReadLong();
			mLastPortalX = b.ReadLong();
			mMindControlled = b.ReadBoolean();
			mOnHighGround = b.ReadBoolean();
			mOrginalAnimRate = b.ReadFloat();
			mParticleOffsetX = b.ReadLong();
			mParticleOffsetY = b.ReadLong();
			mPhaseCounter = b.ReadLong();
			mPlayingSong = b.ReadBoolean();
			mPosX = b.ReadFloat();
			mPosY = b.ReadFloat();
			mPrevFrame = b.ReadLong();
			mPrevTransX = b.ReadFloat();
			mPrevTransY = b.ReadFloat();
			mRelatedZombieIDSaved = GameObject.LoadId(b);
			mScaleZombie = b.ReadFloat();
			mShieldHealth = b.ReadLong();
			mShieldJustGotShotCounter = b.ReadLong();
			mShieldMaxHealth = b.ReadLong();
			mShieldRecoilCounter = b.ReadLong();
			mSummonCounter = b.ReadLong();
			mSummonedDancers = b.ReadBoolean();
			mTargetCol = b.ReadLong();
			mTargetPlantIDSaved = GameObject.LoadId(b);
			mTargetRow = b.ReadLong();
			mUseLadderCol = b.ReadLong();
			mUsesClipping = b.ReadBoolean();
			mVelX = b.ReadFloat();
			mVelZ = b.ReadFloat();
			mYuckyFace = b.ReadBoolean();
			mYuckyFaceCounter = b.ReadLong();
			mZombieAge = b.ReadLong();
			mZombieAttackRect = b.ReadRect();
			mZombieHeight = (ZombieHeight)b.ReadLong();
			mZombiePhase = (ZombiePhase)b.ReadLong();
			mZombieRect = b.ReadRect();
			lastPlayedReanimName = b.ReadString();
			lastPlayedReanimAnimRate = b.ReadFloat();
			lastPlayedReanimBlendTime = b.ReadByte();
			lastPlayedReanimLoopType = (ReanimLoopType)b.ReadLong();
			mHasArm = b.ReadBoolean();
			mHasHead = b.ReadBoolean();
			mHasHelm = b.ReadBoolean();
			mHasShield = b.ReadBoolean();
			if (!mHasArm)
			{
				mHasArm = true;
				DropArm(16u);
			}
			if (!mHasHead)
			{
				mHasHead = true;
				DropHead(16u);
			}
			if (!mHasShield && mShieldType != 0)
			{
				DropShield(16u);
			}
			if (!mHasHelm && mHelmType != 0)
			{
				DropHelm(16u);
			}
			if (!mHasObject && mZombieType == ZombieType.ZOMBIE_DIGGER)
			{
				DiggerLoseAxe();
			}
			if (mButteredCounter > 0 && !IsZombotany())
			{
				mBodyReanimID.AssignRenderGroupToTrack("zombie_butter", 0);
			}
			if (mZombieType == ZombieType.ZOMBIE_BUNGEE)
			{
				SetRow(mRow);
				mPosX = mBoard.GridToPixelX(mTargetCol, mRow);
				mPosY = GetPosYBasedOnRow(mRow);
				mRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_GRAVE_STONE, mRow, 7);
			}
			return true;
		}

		public override void LoadingComplete()
		{
			justLoaded = true;
			base.LoadingComplete();
			for (int i = 0; i < mFollowerZombieID.Length; i++)
			{
				mFollowerZombieID[i] = (GameObject.GetObjectById(mFollowerZombieIDSaved[i]) as Zombie);
			}
			mLeaderZombie = (GameObject.GetObjectById(mLeaderZombieIDSaved) as Zombie);
			mTargetPlantID = (GameObject.GetObjectById(mTargetPlantIDSaved) as Plant);
			mRelatedZombieID = (GameObject.GetObjectById(mRelatedZombieIDSaved) as Zombie);
			if (mZombieType != ZombieType.ZOMBIE_BOSS || lastPlayedReanimName != "anim_spawn_1")
			{
				PlayZombieReanim(ref lastPlayedReanimName, lastPlayedReanimLoopType, lastPlayedReanimBlendTime, lastPlayedReanimAnimRate);
			}
			int num = mJustGotShotCounter;
			TakeHelmDamage(0, 0u);
			if (IsFlying())
			{
				TakeFlyingDamage(0, 0u);
			}
			TakeShieldDamage(0, 0u);
			TakeBodyDamage(0, 0u);
			TakeDamage(0, 0u);
			UpdateDamageStates(0u);
			mJustGotShotCounter = num;
			if (mZombieType == ZombieType.ZOMBIE_BOSS)
			{
				int bodyDamageIndex = GetBodyDamageIndex();
				Reanimation reanimation = mApp.ReanimationGet(mBodyReanimID);
				switch (bodyDamageIndex)
				{
				case 1:
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_boss_head, AtlasResources.IMAGE_REANIM_ZOMBIE_BOSS_HEAD_DAMAGE1);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_boss_jaw, AtlasResources.IMAGE_REANIM_ZOMBIE_BOSS_JAW_DAMAGE1);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_boss_outerarm_hand, AtlasResources.IMAGE_REANIM_ZOMBIE_BOSS_OUTERARM_HAND_DAMAGE1);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_boss_outerarm_thumb2, AtlasResources.IMAGE_REANIM_ZOMBIE_BOSS_OUTERARM_THUMB_DAMAGE1);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_boss_innerleg_foot, AtlasResources.IMAGE_REANIM_ZOMBIE_BOSS_FOOT_DAMAGE1);
					break;
				case 2:
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_boss_head, AtlasResources.IMAGE_REANIM_ZOMBIE_BOSS_HEAD_DAMAGE2);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_boss_jaw, AtlasResources.IMAGE_REANIM_ZOMBIE_BOSS_JAW_DAMAGE2);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_boss_outerarm_hand, AtlasResources.IMAGE_REANIM_ZOMBIE_BOSS_OUTERARM_HAND_DAMAGE2);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_boss_outerarm_thumb2, AtlasResources.IMAGE_REANIM_ZOMBIE_BOSS_OUTERARM_THUMB_DAMAGE2);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_boss_outerleg_foot, AtlasResources.IMAGE_REANIM_ZOMBIE_BOSS_FOOT_DAMAGE2);
					ApplyBossSmokeParticles(true);
					break;
				}
			}
			Update();
			Update();
			doLoot = true;
			doParticle = true;
			justLoaded = false;
		}

		public void RemoveSurprise()
		{
			int num = 0;
			while (true)
			{
				if (num >= mAttachmentID.mEffectArray.Length)
				{
					return;
				}
				if (mAttachmentID.mEffectArray[num].mEffectType == EffectType.EFFECT_REANIM)
				{
					Reanimation reanimation = (Reanimation)mAttachmentID.mEffectArray[num].mEffectID;
					if (reanimation.mReanimationType == ReanimationType.REANIM_ZOMBIE_SURPRISE && reanimation.mLoopCount == 1)
					{
						break;
					}
				}
				num++;
			}
			GlobalMembersAttachment.AttachmentDetach(ref mAttachmentID);
		}

		public void Update()
		{
			cachedZombieRectUpToDate = false;
			Debug.ASSERT(!mDead);
			mZombieAge += 3;
			bool mSurprised2 = mSurprised;
			if ((mApp.mGameScene != GameScenes.SCENE_LEVEL_INTRO || mZombieType != ZombieType.ZOMBIE_BOSS) && (!IsOnBoard() || !mBoard.mCutScene.ShouldRunUpsellBoard()) && mApp.mGameScene != GameScenes.SCENE_PLAYING && IsOnBoard() && mFromWave != GameConstants.ZOMBIE_WAVE_WINNER)
			{
				return;
			}
			if (mZombiePhase == ZombiePhase.PHASE_ZOMBIE_BURNED)
			{
				UpdateBurn();
			}
			else if (mZombiePhase == ZombiePhase.PHASE_ZOMBIE_MOWERED)
			{
				UpdateMowered();
			}
			else if (mZombiePhase == ZombiePhase.PHASE_ZOMBIE_DYING)
			{
				UpdateDeath();
				UpdateZombieWalking();
			}
			else
			{
				if (mPhaseCounter > 0 && !IsImmobilizied())
				{
					mPhaseCounter -= 3;
				}
				if (mApp.mGameScene == GameScenes.SCENE_ZOMBIES_WON)
				{
					if (mBoard.mCutScene.ShowZombieWalking())
					{
						UpdateZombieChimney();
						UpdateZombieWalkingIntoHouse();
					}
				}
				else if (IsOnBoard())
				{
					UpdatePlaying();
				}
				if (mZombieType == ZombieType.ZOMBIE_BUNGEE)
				{
					UpdateZombieBungee();
				}
				if (mZombieType == ZombieType.ZOMBIE_POGO)
				{
					UpdateZombiePogo();
				}
				Animate();
			}
			mJustGotShotCounter -= 3;
			if (mShieldJustGotShotCounter > 0)
			{
				mShieldJustGotShotCounter -= 3;
			}
			if (mShieldRecoilCounter > 0)
			{
				mShieldRecoilCounter -= 3;
			}
			if (mZombieFade > 0)
			{
				mZombieFade -= 3;
				if (mZombieFade <= 0)
				{
					DieNoLoot(true);
				}
			}
			mX = (int)mPosX;
			mY = (int)mPosY;
			GlobalMembersAttachment.AttachmentUpdateAndMove(ref mAttachmentID, mPosX, mPosY);
			UpdateReanim();
		}

		public void DieNoLoot(bool giveAchievements)
		{
			StopZombieSound();
			GlobalMembersAttachment.AttachmentDie(ref mAttachmentID);
			mApp.RemoveReanimation(ref mBodyReanimID);
			mApp.RemoveReanimation(ref mMoweredReanimID);
			mApp.RemoveReanimation(ref mSpecialHeadReanimID);
			mDead = true;
			TrySpawnLevelAward();
			if (mApp.mPlayerInfo != null && mFromWave != GameConstants.ZOMBIE_WAVE_UI && giveAchievements)
			{
				mApp.mPlayerInfo.mZombiesKilled++;
			}
			if (mZombieType == ZombieType.ZOMBIE_BOBSLED)
			{
				BobsledDie();
			}
			if (mZombieType == ZombieType.ZOMBIE_BUNGEE)
			{
				BungeeDie();
			}
			if (mZombieType == ZombieType.ZOMBIE_BOSS)
			{
				BossDie();
			}
			if (giveAchievements && mZombieType == ZombieType.ZOMBIE_GARGANTUAR && mBoard != null)
			{
				mBoard.GrantAchievement(AchievementId.CrashoftheTitan);
			}
			if (mLeaderZombie == null || mLeaderZombie.mFollowerZombieID == null)
			{
				return;
			}
			for (int i = 0; i < mLeaderZombie.mFollowerZombieID.Length; i++)
			{
				if (mLeaderZombie.mFollowerZombieID[i] == this)
				{
					mLeaderZombie.mFollowerZombieID[i] = null;
				}
			}
		}

		public void DieWithLoot()
		{
			DieNoLoot(true);
			if (doLoot)
			{
				DropLoot();
			}
		}

		public void Draw(Graphics g)
		{
			g.SetDrawMode(Graphics.DrawMode.DRAWMODE_NORMAL);
			if (mZombieHeight == ZombieHeight.HEIGHT_GETTING_BUNGEE_DROPPED)
			{
				return;
			}
			ZombieDrawPosition theDrawPos = default(ZombieDrawPosition);
			GetDrawPos(ref theDrawPos);
			if (mApp.mGameScene == GameScenes.SCENE_ZOMBIES_WON && !SetupDrawZombieWon(g))
			{
				return;
			}
			if (mIceTrapCounter > 0)
			{
				DrawIceTrap(g, ref theDrawPos, false);
			}
			if (mApp.mGameMode != GameMode.GAMEMODE_CHALLENGE_INVISIGHOUL || mFromWave == GameConstants.ZOMBIE_WAVE_UI)
			{
				if (mBodyReanimID != null)
				{
					DrawReanim(g, ref theDrawPos, 0);
				}
				else
				{
					DrawZombie(g, ref theDrawPos);
				}
			}
			if (mIceTrapCounter > 0)
			{
				DrawIceTrap(g, ref theDrawPos, true);
			}
			if (mButteredCounter > 0)
			{
				if (!mIsButterShowing)
				{
					if (!IsZombotany())
					{
						mBodyReanimID.AssignRenderGroupToTrack("zombie_butter", 0);
					}
					mIsButterShowing = true;
				}
				if (IsZombotany() || mApp.mGameMode == GameMode.GAMEMODE_CHALLENGE_INVISIGHOUL)
				{
					DrawButter(g, ref theDrawPos);
				}
			}
			else if (mIsButterShowing)
			{
				if (!IsZombotany())
				{
					mBodyReanimID.AssignRenderGroupToTrack("zombie_butter", -1);
				}
				mIsButterShowing = false;
			}
			if (mAttachmentID != null)
			{
				Graphics @new = Graphics.GetNew(g);
				MakeParentGraphicsFrame(@new);
				@new.mTransY += (int)(theDrawPos.mBodyY * Constants.S);
				if (theDrawPos.mClipHeight > GameConstants.CLIP_HEIGHT_LIMIT)
				{
					float num = 120f - theDrawPos.mClipHeight + 21f;
					float mImageOffsetX = theDrawPos.mImageOffsetX;
					float num2 = theDrawPos.mImageOffsetY - 28f;
					@new.ClipRect((int)(((float)mX + mImageOffsetX - 400f) * Constants.S), (int)(((float)mY + num2) * Constants.S), (int)(920f * Constants.S), (int)(num * Constants.S));
				}
				GlobalMembersAttachment.AttachmentDraw(mAttachmentID, @new, false, true);
				@new.PrepareForReuse();
			}
			g.ClearClipRect();
		}

		public bool IsZombotany()
		{
			if (mZombieType != ZombieType.ZOMBIE_SQUASH_HEAD && mZombieType != ZombieType.ZOMBIE_WALLNUT_HEAD && mZombieType != ZombieType.ZOMBIE_TALLNUT_HEAD && mZombieType != ZombieType.ZOMBIE_PEA_HEAD && mZombieType != ZombieType.ZOMBIE_JALAPENO_HEAD)
			{
				return mZombieType == ZombieType.ZOMBIE_GATLING_HEAD;
			}
			return true;
		}

		public void DrawZombie(Graphics g, ref ZombieDrawPosition theDrawPos)
		{
			Image theImage = null;
			int theRow = 0;
			bool flag = false;
			switch (mZombieType)
			{
			case ZombieType.ZOMBIE_NORMAL:
			case ZombieType.ZOMBIE_FLAG:
			case ZombieType.ZOMBIE_TRAFFIC_CONE:
			case ZombieType.ZOMBIE_PAIL:
			case ZombieType.ZOMBIE_NEWSPAPER:
			case ZombieType.ZOMBIE_DOOR:
			case ZombieType.ZOMBIE_FOOTBALL:
			case ZombieType.ZOMBIE_DOLPHIN_RIDER:
			case ZombieType.ZOMBIE_LADDER:
				flag = true;
				break;
			default:
				Debug.ASSERT(false);
				break;
			}
			if (flag)
			{
				DrawZombieWithParts(g, ref theDrawPos);
			}
			else
			{
				DrawZombiePart(g, theImage, mFrame, theRow, ref theDrawPos);
			}
		}

		public void DrawZombieWithParts(Graphics g, ref ZombieDrawPosition theDrawPos)
		{
		}

		public void DrawZombiePart(Graphics g, Image theImage, int theFrame, int theRow, ref ZombieDrawPosition theDrawPos)
		{
			int celWidth = theImage.GetCelWidth();
			int celHeight = theImage.GetCelHeight();
			float num = theDrawPos.mImageOffsetX;
			float num2 = theDrawPos.mImageOffsetY + theDrawPos.mBodyY;
			if (mZombiePhase == ZombiePhase.PHASE_POLEVAULTER_IN_VAULT)
			{
				num += -120f;
				num2 += -120f;
			}
			if (mZombiePhase == ZombiePhase.PHASE_DIGGER_TUNNELING)
			{
				num2 += 50f;
			}
			if (mZombieType == ZombieType.ZOMBIE_ZAMBONI)
			{
				num2 += -19f;
			}
			float num3 = celHeight;
			if (theDrawPos.mClipHeight > GameConstants.CLIP_HEIGHT_LIMIT)
			{
				num3 = TodCommon.ClampFloat((float)celHeight - theDrawPos.mClipHeight, 0f, celHeight);
			}
			int num4 = 255;
			if (mZombieFade >= 0)
			{
				num4 = TodCommon.ClampInt((int)((float)(255 * mZombieFade) / 30f), 0, 255);
				g.SetColorizeImages(true);
				g.SetColor(new SexyColor(255, 255, 255, num4));
			}
			bool flag = false;
			if (mZombiePhase == ZombiePhase.PHASE_DANCER_DANCING_IN || mZombiePhase == ZombiePhase.PHASE_DANCER_DANCING_LEFT)
			{
				int dancerFrame = GetDancerFrame();
				if (!mIsEating && (dancerFrame == 12 || dancerFrame == 13 || dancerFrame == 14 || dancerFrame == 18 || dancerFrame == 19 || dancerFrame == 20))
				{
					flag = true;
					num -= 30f;
				}
			}
			if (flag)
			{
				num = 0f - num;
			}
			TRect theSrcRect = new TRect(theFrame * celWidth, theRow * celHeight, celWidth, (int)num3);
			TRect theDestRect = new TRect((int)num, (int)num2, celWidth, (int)num3);
			if (mZombiePhase == ZombiePhase.PHASE_ZOMBIE_BURNED)
			{
				if (mMindControlled)
				{
					flag = true;
				}
				g.SetColorizeImages(true);
				g.SetColor(SexyColor.Black);
				g.DrawImageMirror(theImage, theDestRect, theSrcRect, flag);
			}
			else if (mMindControlled)
			{
				flag = true;
				g.SetColorizeImages(true);
				SexyColor zOMBIE_MINDCONTROLLED_COLOR = GameConstants.ZOMBIE_MINDCONTROLLED_COLOR;
				zOMBIE_MINDCONTROLLED_COLOR.mAlpha = num4;
				g.SetColor(zOMBIE_MINDCONTROLLED_COLOR);
				g.DrawImageMirror(theImage, theDestRect, theSrcRect, flag);
				g.SetDrawMode(Graphics.DrawMode.DRAWMODE_ADDITIVE);
				g.DrawImageMirror(theImage, theDestRect, theSrcRect, flag);
				g.SetDrawMode(Graphics.DrawMode.DRAWMODE_NORMAL);
			}
			else if (mChilledCounter > 0 || mIceTrapCounter > 0)
			{
				g.SetColorizeImages(true);
				g.SetColor(new SexyColor(75, 75, 255, num4));
				g.DrawImageMirror(theImage, theDestRect, theSrcRect, flag);
				g.SetDrawMode(Graphics.DrawMode.DRAWMODE_ADDITIVE);
				g.DrawImageMirror(theImage, theDestRect, theSrcRect, flag);
				g.SetDrawMode(Graphics.DrawMode.DRAWMODE_NORMAL);
			}
			else
			{
				g.DrawImageMirror(theImage, theDestRect, theSrcRect, flag);
			}
			if (mJustGotShotCounter > 0)
			{
				g.SetDrawMode(Graphics.DrawMode.DRAWMODE_ADDITIVE);
				g.SetColorizeImages(true);
				int num5 = mJustGotShotCounter * 10;
				g.SetColor(new SexyColor(num5, num5, num5, 255));
				g.DrawImageMirror(theImage, theDestRect, theSrcRect, flag);
				g.SetDrawMode(Graphics.DrawMode.DRAWMODE_NORMAL);
			}
			g.SetColorizeImages(false);
		}

		public void DrawBungeeCord(Graphics g, int theOffsetX, int theOffsetY)
		{
			int num = (int)((float)AtlasResources.IMAGE_BUNGEECORD.GetCelHeight() * mScaleZombie);
			float thePosX = 0f;
			float thePosY = 0f;
			GetTrackPosition(ref GlobalMembersReanimIds.ReanimTrackId_zombie_bungi_body, ref thePosX, ref thePosY);
			bool flag = false;
			if (IsOnBoard() && mApp.IsFinalBossLevel())
			{
				Zombie bossZombie = mBoard.GetBossZombie();
				int num2 = 55;
				if (bossZombie.mZombiePhase == ZombiePhase.PHASE_BOSS_BUNGEES_LEAVE)
				{
					Reanimation reanimation = mApp.ReanimationGet(bossZombie.mBodyReanimID);
					num2 = (int)TodCommon.TodAnimateCurveFloatTime(0f, 0.2f, reanimation.mAnimTime, 55f, 0f, TodCurves.CURVE_LINEAR);
				}
				if (mTargetCol > bossZombie.mTargetCol)
				{
					g.SetClipRect(new TRect(-g.mTransX, (int)((float)num2 * Constants.S) - g.mTransY, Constants.BOARD_WIDTH, Constants.BOARD_HEIGHT));
					flag = true;
				}
			}
			bool mColorizeImages = g.mColorizeImages;
			Color mColor = g.mColor;
			if (mZombiePhase == ZombiePhase.PHASE_ZOMBIE_BURNED)
			{
				int a = TodCommon.ClampInt((int)((float)(255 * mZombieFade) / 30f), 0, 255);
				g.SetColor(new Color(0, 0, 0, a));
				g.SetColorizeImages(true);
			}
			for (float num3 = thePosY - (float)num; num3 > (float)(-num); num3 -= (float)num)
			{
				float thePosX2 = (float)(theOffsetX + Constants.Zombie_Bungee_Offset.X) - 4f / mScaleZombie;
				float thePosY2 = num3 - mPosY - (float)Constants.Zombie_Bungee_Offset.Y;
				TodCommon.TodDrawImageScaledF(g, AtlasResources.IMAGE_BUNGEECORD, thePosX2, thePosY2, mScaleZombie, mScaleZombie);
			}
			if (mZombiePhase == ZombiePhase.PHASE_ZOMBIE_BURNED)
			{
				g.SetColor(mColor, false);
				g.SetColorizeImages(mColorizeImages);
			}
			if (flag)
			{
				g.ClearClipRect();
			}
		}

		public void TakeDamage(int theDamage, uint theDamageFlags)
		{
			if (mZombiePhase == ZombiePhase.PHASE_JACK_IN_THE_BOX_POPPING || IsDeadOrDying())
			{
				return;
			}
			int num = theDamage;
			if (IsFlying())
			{
				num = TakeFlyingDamage(theDamage, theDamageFlags);
			}
			if (num > 0 && mShieldType != 0 && !TodCommon.TestBit(theDamageFlags, 0))
			{
				num = TakeShieldDamage(theDamage, theDamageFlags);
				if (TodCommon.TestBit(theDamageFlags, 1))
				{
					num = theDamage;
				}
			}
			if (num > 0 && mHelmType != 0)
			{
				num = TakeHelmDamage(theDamage, theDamageFlags);
			}
			if (num > 0)
			{
				TakeBodyDamage(num, theDamageFlags);
			}
		}

		public void SetRow(int theRow)
		{
			Debug.ASSERT(theRow >= 0 && theRow < Constants.MAX_GRIDSIZEY);
			mRow = theRow;
			mRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_ZOMBIE, mRow, 4);
		}

		public float GetPosYBasedOnRow(int theRow)
		{
			if (!IsOnBoard())
			{
				return 0f;
			}
			if (IsOnHighGround())
			{
				if (mAltitude < (float)Constants.HIGH_GROUND_HEIGHT)
				{
					mZombieHeight = ZombieHeight.HEIGHT_UP_TO_HIGH_GROUND;
				}
				mOnHighGround = true;
			}
			float num = mBoard.GetPosYBasedOnRow(mPosX + 40f, theRow) - 30f;
			if (mZombieType == ZombieType.ZOMBIE_BALLOON)
			{
				num -= 30f;
			}
			if (mZombieType == ZombieType.ZOMBIE_POGO)
			{
				num -= 16f;
			}
			return num;
		}

		public void ApplyChill(bool theIsIceTrap)
		{
			if (CanBeChilled())
			{
				if (mChilledCounter == 0)
				{
					mApp.PlayFoley(FoleyType.FOLEY_FROZEN);
				}
				int val = 1000;
				if (theIsIceTrap)
				{
					val = 2000;
				}
				mChilledCounter = Math.Max(val, mChilledCounter);
				UpdateAnimSpeed();
			}
		}

		public void UpdateZombieBungee()
		{
			if (IsDeadOrDying() || IsImmobilizied())
			{
				return;
			}
			if (mZombiePhase == ZombiePhase.PHASE_BUNGEE_DIVING || mZombiePhase == ZombiePhase.PHASE_BUNGEE_DIVING_SCREAMING)
			{
				float num = (float)GameConstants.BUNGEE_ZOMBIE_HEIGHT - 404f;
				float num2 = mAltitude;
				mAltitude -= 24f;
				if (mAltitude <= num && num2 > num && mRelatedZombieID == null)
				{
					mApp.PlayFoley(FoleyType.FOLEY_GRASSSTEP);
				}
				BungeeLanding();
			}
			else if (mZombiePhase == ZombiePhase.PHASE_BUNGEE_AT_BOTTOM)
			{
				if (mPhaseCounter <= 0)
				{
					BungeeStealTarget();
					mZombiePhase = ZombiePhase.PHASE_BUNGEE_GRABBING;
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_BUNGEE_GRABBING)
			{
				Reanimation reanimation = mApp.ReanimationGet(mBodyReanimID);
				if (reanimation.mLoopCount > 0)
				{
					BungeeLiftTarget();
					mZombiePhase = ZombiePhase.PHASE_BUNGEE_RISING;
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_BUNGEE_HIT_OUCHY)
			{
				if (mPhaseCounter <= 0)
				{
					DieWithLoot();
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_BUNGEE_RISING)
			{
				mAltitude += 24f;
				if (mAltitude >= 600f)
				{
					DieNoLoot(false);
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_BUNGEE_CUTSCENE)
			{
				mAltitude = TodCommon.TodAnimateCurve(200, 0, mPhaseCounter, 40, 0, TodCurves.CURVE_SIN_WAVE);
				if (mPhaseCounter <= 0)
				{
					mPhaseCounter = 200;
				}
			}
			mX = (int)mPosX;
			mY = (int)mPosY;
		}

		public void BungeeLanding()
		{
			if (mZombiePhase == ZombiePhase.PHASE_BUNGEE_DIVING && mAltitude < 1500f && !mApp.IsFinalBossLevel())
			{
				mApp.PlayFoley(FoleyType.FOLEY_BUNGEE_SCREAM);
				mZombiePhase = ZombiePhase.PHASE_BUNGEE_DIVING_SCREAMING;
			}
			if (mAltitude > 40f)
			{
				return;
			}
			Plant plant = mBoard.FindUmbrellaPlant(mTargetCol, mRow);
			if (plant != null)
			{
				mApp.PlaySample(Resources.SOUND_BOING);
				mApp.PlayFoley(FoleyType.FOLEY_UMBRELLA);
				plant.DoSpecial();
				mZombiePhase = ZombiePhase.PHASE_BUNGEE_RISING;
				mRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_TOP, 0, 1);
				mHitUmbrella = true;
				return;
			}
			mBoard.GetTopPlantAt(mTargetCol, mRow, PlantPriority.TOPPLANT_BUNGEE_ORDER);
			if (!(mAltitude > 0f))
			{
				mAltitude = 0f;
				Zombie zombie = null;
				int num = mBoard.mZombies.IndexOf(mRelatedZombieID);
				if (num != -1)
				{
					zombie = mBoard.mZombies[num];
				}
				if (zombie != null)
				{
					zombie.mZombieHeight = ZombieHeight.HEIGHT_ZOMBIE_NORMAL;
					zombie.StartWalkAnim(0);
					mRelatedZombieID = null;
					mZombiePhase = ZombiePhase.PHASE_BUNGEE_RISING;
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_raise, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 0, 36f);
				}
				else
				{
					mZombiePhase = ZombiePhase.PHASE_BUNGEE_AT_BOTTOM;
					mPhaseCounter = 300;
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_idle, ReanimLoopType.REANIM_LOOP, 5, 24f);
					Reanimation reanimation = mApp.ReanimationGet(mBodyReanimID);
					reanimation.mAnimTime = 0.5f;
				}
			}
		}

		public bool EffectedByDamage(uint theDamageRangeFlags)
		{
			if (!TodCommon.TestBit(theDamageRangeFlags, 5) && IsDeadOrDying())
			{
				return false;
			}
			if (TodCommon.TestBit(theDamageRangeFlags, 7))
			{
				if (!mMindControlled)
				{
					return false;
				}
			}
			else if (mMindControlled)
			{
				return false;
			}
			if (mZombieType == ZombieType.ZOMBIE_BUNGEE && mZombiePhase != ZombiePhase.PHASE_BUNGEE_AT_BOTTOM && mZombiePhase != ZombiePhase.PHASE_BUNGEE_GRABBING)
			{
				return false;
			}
			if (mZombieHeight == ZombieHeight.HEIGHT_GETTING_BUNGEE_DROPPED)
			{
				return false;
			}
			if (mZombieType == ZombieType.ZOMBIE_BOSS)
			{
				Reanimation reanimation = mApp.ReanimationGet(mBodyReanimID);
				if (mZombiePhase == ZombiePhase.PHASE_BOSS_HEAD_ENTER && reanimation.mAnimTime < 0.5f)
				{
					return false;
				}
				if (mZombiePhase == ZombiePhase.PHASE_BOSS_HEAD_LEAVE && reanimation.mAnimTime > 0.5f)
				{
					return false;
				}
				if (mZombiePhase != ZombiePhase.PHASE_BOSS_HEAD_IDLE_BEFORE_SPIT && mZombiePhase != ZombiePhase.PHASE_BOSS_HEAD_IDLE_AFTER_SPIT && mZombiePhase != ZombiePhase.PHASE_BOSS_HEAD_SPIT)
				{
					return false;
				}
			}
			if (mZombieType == ZombieType.ZOMBIE_BOBSLED && GetBobsledPosition() > 0)
			{
				return false;
			}
			if (mZombiePhase == ZombiePhase.PHASE_POLEVAULTER_IN_VAULT || mZombiePhase == ZombiePhase.PHASE_IMP_GETTING_THROWN || mZombiePhase == ZombiePhase.PHASE_DIGGER_RISING || mZombiePhase == ZombiePhase.PHASE_DIGGER_TUNNELING_PAUSE_WITHOUT_AXE || mZombiePhase == ZombiePhase.PHASE_DIGGER_RISE_WITHOUT_AXE || mZombiePhase == ZombiePhase.PHASE_DOLPHIN_INTO_POOL || mZombiePhase == ZombiePhase.PHASE_DOLPHIN_IN_JUMP || mZombiePhase == ZombiePhase.PHASE_SNORKEL_INTO_POOL || mZombiePhase == ZombiePhase.PHASE_BALLOON_POPPING || mZombiePhase == ZombiePhase.PHASE_RISING_FROM_GRAVE || mZombiePhase == ZombiePhase.PHASE_BOBSLED_CRASHING || mZombiePhase == ZombiePhase.PHASE_DANCER_RISING)
			{
				if (TodCommon.TestBit(theDamageRangeFlags, 4))
				{
					return true;
				}
				return false;
			}
			if (mZombieType != ZombieType.ZOMBIE_BOBSLED && mZombieType != ZombieType.ZOMBIE_BOSS && GetZombieRect().mX > Constants.WIDE_BOARD_WIDTH)
			{
				return false;
			}
			bool flag = mZombieType == ZombieType.ZOMBIE_SNORKEL && mInPool && !mIsEating;
			if (TodCommon.TestBit(theDamageRangeFlags, 2) && flag)
			{
				return true;
			}
			bool flag2 = mZombiePhase == ZombiePhase.PHASE_DIGGER_TUNNELING;
			if (TodCommon.TestBit(theDamageRangeFlags, 6) && flag2)
			{
				return true;
			}
			if (TodCommon.TestBit(theDamageRangeFlags, 1) && IsFlying())
			{
				return true;
			}
			if (TodCommon.TestBit(theDamageRangeFlags, 0) && !IsFlying() && !flag && !flag2)
			{
				return true;
			}
			return false;
		}

		public void PickRandomSpeed()
		{
			if (mZombiePhase == ZombiePhase.PHASE_SNORKEL_WALKING_IN_POOL)
			{
				mVelX = 0.3f;
			}
			else if (mZombiePhase == ZombiePhase.PHASE_DIGGER_WALKING)
			{
				if (mApp.IsIZombieLevel())
				{
					mVelX = 0.23f;
				}
				else
				{
					mVelX = 0.12f;
				}
			}
			else if (mZombieType == ZombieType.ZOMBIE_IMP && mApp.IsIZombieLevel())
			{
				mVelX = 0.9f;
			}
			else if (mZombiePhase == ZombiePhase.PHASE_YETI_RUNNING)
			{
				mVelX = 0.8f;
			}
			else if (mZombieType == ZombieType.ZOMBIE_YETI)
			{
				mVelX = 0.4f;
			}
			else if (mZombieType == ZombieType.ZOMBIE_DANCER || mZombieType == ZombieType.ZOMBIE_BACKUP_DANCER || mZombieType == ZombieType.ZOMBIE_POGO || mZombieType == ZombieType.ZOMBIE_FLAG)
			{
				mVelX = 0.45f;
			}
			else if (mZombiePhase == ZombiePhase.PHASE_DIGGER_TUNNELING || mZombiePhase == ZombiePhase.PHASE_POLEVAULTER_PRE_VAULT || mZombieType == ZombieType.ZOMBIE_FOOTBALL || mZombieType == ZombieType.ZOMBIE_SNORKEL || mZombieType == ZombieType.ZOMBIE_JACK_IN_THE_BOX)
			{
				mVelX = TodCommon.RandRangeFloat(0.66f, 0.68f);
			}
			else if (mZombiePhase == ZombiePhase.PHASE_LADDER_CARRYING || mZombieType == ZombieType.ZOMBIE_SQUASH_HEAD)
			{
				mVelX = TodCommon.RandRangeFloat(0.79f, 0.81f);
			}
			else if (mZombiePhase == ZombiePhase.PHASE_NEWSPAPER_MAD || mZombiePhase == ZombiePhase.PHASE_DOLPHIN_WALKING || mZombiePhase == ZombiePhase.PHASE_DOLPHIN_WALKING_WITHOUT_DOLPHIN)
			{
				mVelX = TodCommon.RandRangeFloat(0.89f, 0.91f);
			}
			else
			{
				mVelX = TodCommon.RandRangeFloat(0.23f, 0.37f);
				if ((double)mVelX < 0.3)
				{
					mAnimTicksPerFrame = 12;
				}
				else
				{
					mAnimTicksPerFrame = 15;
				}
			}
			UpdateAnimSpeed();
		}

		public void UpdateZombiePolevaulter()
		{
			if (mZombiePhase == ZombiePhase.PHASE_POLEVAULTER_PRE_VAULT && mHasHead && mZombieHeight == ZombieHeight.HEIGHT_ZOMBIE_NORMAL)
			{
				Plant plant = FindPlantTarget(ZombieAttackType.ATTACKTYPE_VAULT);
				if (plant != null)
				{
					if (mBoard.GetLadderAt(plant.mPlantCol, plant.mRow) != null)
					{
						if ((float)(mBoard.GridToPixelX(plant.mPlantCol, plant.mRow) + 40) > mPosX && mZombieHeight == ZombieHeight.HEIGHT_ZOMBIE_NORMAL && mUseLadderCol != plant.mPlantCol)
						{
							mZombieHeight = ZombieHeight.HEIGHT_UP_LADDER;
							mUseLadderCol = plant.mPlantCol;
						}
						return;
					}
					mZombiePhase = ZombiePhase.PHASE_POLEVAULTER_IN_VAULT;
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_jump, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 20, 24f);
					Reanimation reanimation = mApp.ReanimationGet(mBodyReanimID);
					float num = (float)reanimation.mFrameCount / reanimation.mAnimRate * 100f;
					int num2 = mX - plant.mX - 80;
					if (mApp.IsWallnutBowlingLevel())
					{
						num2 = 0;
					}
					mVelX = (float)num2 / num;
					mHasObject = false;
				}
				if (mApp.IsIZombieLevel() && mBoard.mChallenge.IZombieGetBrainTarget(this) != null)
				{
					mZombiePhase = ZombiePhase.PHASE_POLEVAULTER_POST_VAULT;
					StartWalkAnim(0);
				}
			}
			else
			{
				if (mZombiePhase != ZombiePhase.PHASE_POLEVAULTER_IN_VAULT)
				{
					return;
				}
				Reanimation reanimation2 = mApp.ReanimationGet(mBodyReanimID);
				bool flag = false;
				if (reanimation2.mAnimTime > 0.6f && reanimation2.mAnimTime <= 0.7f)
				{
					Plant plant2 = FindPlantTarget(ZombieAttackType.ATTACKTYPE_VAULT);
					if (plant2 != null && plant2.mSeedType == SeedType.SEED_TALLNUT)
					{
						mApp.PlayFoley(FoleyType.FOLEY_BONK);
						flag = true;
						mApp.AddTodParticle(plant2.mX + 60, plant2.mY - 20, mRenderOrder + 1, ParticleEffect.PARTICLE_TALL_NUT_BLOCK);
						mPosX = plant2.mX;
						mPosY -= 30f;
						mZombieHeight = ZombieHeight.HEIGHT_FALLING;
					}
				}
				if (reanimation2.mLoopCount > 0)
				{
					flag = true;
					mPosX -= 150f;
				}
				if (reanimation2.ShouldTriggerTimedEvent(0.2f))
				{
					mApp.PlayFoley(FoleyType.FOLEY_GRASSSTEP);
				}
				if (reanimation2.ShouldTriggerTimedEvent(0.4f))
				{
					mApp.PlayFoley(FoleyType.FOLEY_POLEVAULT);
				}
				if (flag)
				{
					mX = (int)mPosX;
					mZombiePhase = ZombiePhase.PHASE_POLEVAULTER_POST_VAULT;
					mZombieAttackRect = new TRect(50, 0, 20, 115);
					StartWalkAnim(0);
				}
				else
				{
					float num3 = mPosX;
					mPosX -= 150f * reanimation2.mAnimTime;
					mPosY = GetPosYBasedOnRow(mRow);
					mPosX = num3;
				}
			}
		}

		public void UpdateZombieDolphinRider()
		{
			if (IsTangleKelpTarget())
			{
				return;
			}
			bool flag = IsWalkingBackwards();
			mUsesClipping = false;
			if (mZombiePhase == ZombiePhase.PHASE_DOLPHIN_WALKING && !flag)
			{
				if (mX > 800 && mX <= 820)
				{
					mZombiePhase = ZombiePhase.PHASE_DOLPHIN_INTO_POOL;
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_jumpinpool, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 20, 16f);
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_DOLPHIN_INTO_POOL)
			{
				mUsesClipping = true;
				Reanimation reanimation = mApp.ReanimationGet(mBodyReanimID);
				if (reanimation.ShouldTriggerTimedEvent(0.56f))
				{
					Reanimation reanimation2 = mApp.AddReanimation(mX - 83, mY + 73, mRenderOrder + 1, ReanimationType.REANIM_SPLASH);
					reanimation2.OverrideScale(1.2f, 0.8f);
					mApp.AddTodParticle(mX - 46, mY + 115, mRenderOrder + 1, ParticleEffect.PARTICLE_PLANTING_POOL);
					mApp.PlayFoley(FoleyType.FOLEY_ZOMBIE_ENTERING_WATER);
				}
				if (reanimation.mLoopCount > 0)
				{
					mZombiePhase = ZombiePhase.PHASE_DOLPHIN_RIDING;
					mInPool = true;
					mPosX -= 70f;
					mZombieAttackRect = new TRect(-29, 0, 70, 115);
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_ride, ReanimLoopType.REANIM_LOOP_FULL_LAST_FRAME, 0, 12f);
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_DOLPHIN_RIDING)
			{
				if (mX <= 120)
				{
					mZombieHeight = ZombieHeight.HEIGHT_OUT_OF_POOL;
					mZombiePhase = ZombiePhase.PHASE_DOLPHIN_WALKING;
					mAltitude = -40f;
					PoolSplash(false);
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_walkdolphin, ReanimLoopType.REANIM_LOOP, 0, 0f);
					PickRandomSpeed();
					return;
				}
				if (mHasHead && !IsTanglekelpTarget())
				{
					Plant plant = FindPlantTarget(ZombieAttackType.ATTACKTYPE_VAULT);
					if (plant != null)
					{
						mApp.PlayFoley(FoleyType.FOLEY_DOLPHIN_BEFORE_JUMPING);
						mApp.PlayFoley(FoleyType.FOLEY_PLANT_WATER);
						mZombiePhase = ZombiePhase.PHASE_DOLPHIN_IN_JUMP;
						mPhaseCounter = GameConstants.DOLPHIN_JUMP_TIME;
						mVelX = 0.5f;
						PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_dolphinjump, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 0, 10f);
					}
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_DOLPHIN_IN_JUMP)
			{
				Reanimation reanimation3 = mApp.ReanimationGet(mBodyReanimID);
				bool flag2 = false;
				mAltitude = TodCommon.TodAnimateCurveFloat(GameConstants.DOLPHIN_JUMP_TIME, 0, mPhaseCounter, 0f, 10f, TodCurves.CURVE_LINEAR);
				if (reanimation3.ShouldTriggerTimedEvent(0.3f))
				{
					Plant plant2 = FindPlantTarget(ZombieAttackType.ATTACKTYPE_VAULT);
					if (plant2 != null && plant2.mSeedType == SeedType.SEED_TALLNUT)
					{
						mApp.PlayFoley(FoleyType.FOLEY_BONK);
						flag2 = true;
						mApp.AddTodParticle(plant2.mX + 60, plant2.mY - 20, mRenderOrder + 1, ParticleEffect.PARTICLE_TALL_NUT_BLOCK);
						mPosX = (float)plant2.mX + 25f;
						mAltitude = 30f;
						mZombieHeight = ZombieHeight.HEIGHT_FALLING;
					}
				}
				else if (reanimation3.ShouldTriggerTimedEvent(0.49f))
				{
					Reanimation reanimation4 = mApp.AddReanimation(mX - 63, mY + 73, mRenderOrder + 1, ReanimationType.REANIM_SPLASH);
					reanimation4.OverrideScale(1.2f, 0.8f);
					mApp.AddTodParticle(mX - 26, mY + 115, mRenderOrder + 1, ParticleEffect.PARTICLE_PLANTING_POOL);
					mApp.PlayFoley(FoleyType.FOLEY_ZOMBIE_ENTERING_WATER);
					mVelX = 0f;
				}
				else if (reanimation3.mLoopCount > 0)
				{
					flag2 = true;
					mPosX -= 94f;
					mAltitude = 0f;
				}
				if (flag2)
				{
					mZombiePhase = ZombiePhase.PHASE_DOLPHIN_WALKING_IN_POOL;
					mZombieAttackRect = new TRect(30, 0, 30, 115);
					mZombieRect = new TRect(20, 0, 42, 115);
					StartWalkAnim(0);
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_DOLPHIN_WALKING_IN_POOL)
			{
				if (mX <= 140 && !flag)
				{
					mZombieHeight = ZombieHeight.HEIGHT_OUT_OF_POOL;
					mZombiePhase = ZombiePhase.PHASE_DOLPHIN_WALKING_WITHOUT_DOLPHIN;
					mAltitude = -40f;
					PoolSplash(false);
					PlayZombieReanim(ref Reanimation.ReanimTrackId_anim_walk, ReanimLoopType.REANIM_LOOP, 0, 0f);
					PickRandomSpeed();
				}
				else if (mX > 770 && flag)
				{
					mZombieHeight = ZombieHeight.HEIGHT_OUT_OF_POOL;
					mZombiePhase = ZombiePhase.PHASE_DOLPHIN_WALKING_WITHOUT_DOLPHIN;
					mAltitude = -40f;
					PoolSplash(false);
					PlayZombieReanim(ref Reanimation.ReanimTrackId_anim_walk, ReanimLoopType.REANIM_LOOP, 0, 0f);
					PickRandomSpeed();
				}
			}
			if (mZombiePhase == ZombiePhase.PHASE_DOLPHIN_WALKING_WITHOUT_DOLPHIN)
			{
				mUsesClipping = (mAltitude < 0f);
			}
		}

		public void PickBungeeZombieTarget(int theColumn)
		{
			int num = CountBungeesTargetingSunFlowers();
			int num2 = mBoard.CountSunFlowers();
			bool flag = true;
			if (num == num2 - 1)
			{
				flag = false;
			}
			int num3 = 0;
			for (int i = 0; i < aPicks.Length; i++)
			{
				aPicks[i].Reset();
			}
			for (int j = 0; j < Constants.GRIDSIZEX; j++)
			{
				if (theColumn != -1 && theColumn != j)
				{
					continue;
				}
				for (int k = 0; k < Constants.MAX_GRIDSIZEY; k++)
				{
					int mWeight = 1;
					if (mBoard.GetGraveStoneAt(j, k) != null || mBoard.mGridSquareType[j, k] == GridSquareType.GRIDSQUARE_DIRT)
					{
						continue;
					}
					Plant topPlantAt = mBoard.GetTopPlantAt(j, k, PlantPriority.TOPPLANT_BUNGEE_ORDER);
					if (topPlantAt != null)
					{
						if ((!flag && topPlantAt.MakesSun()) || topPlantAt.mSeedType == SeedType.SEED_GRAVEBUSTER || topPlantAt.mSeedType == SeedType.SEED_COBCANNON)
						{
							continue;
						}
						mWeight = 10000;
					}
					if (!mBoard.BungeeIsTargetingCell(j, k))
					{
						aPicks[num3].mX = j;
						aPicks[num3].mY = k;
						aPicks[num3].mWeight = mWeight;
						num3++;
					}
				}
			}
			if (num3 == 0)
			{
				DieNoLoot(false);
				return;
			}
			TodWeightedGridArray todWeightedGridArray = TodCommon.TodPickFromWeightedGridArray(aPicks, num3);
			mTargetCol = todWeightedGridArray.mX;
			SetRow(todWeightedGridArray.mY);
			mPosX = mBoard.GridToPixelX(mTargetCol, mRow);
			mPosY = GetPosYBasedOnRow(mRow);
		}

		public int CountBungeesTargetingSunFlowers()
		{
			int num = 0;
			int count = mBoard.mZombies.Count;
			for (int i = 0; i < count; i++)
			{
				Zombie zombie = mBoard.mZombies[i];
				if (!zombie.mDead && !zombie.IsDeadOrDying() && zombie.mZombieType == ZombieType.ZOMBIE_BUNGEE && zombie.mTargetCol != -1)
				{
					Plant topPlantAt = mBoard.GetTopPlantAt(zombie.mTargetCol, zombie.mRow, PlantPriority.TOPPLANT_BUNGEE_ORDER);
					if (topPlantAt != null && topPlantAt.MakesSun())
					{
						num++;
					}
				}
			}
			return num;
		}

		public Plant FindPlantTarget(ZombieAttackType theAttackType)
		{
			TRect zombieAttackRect = GetZombieAttackRect();
			int count = mBoard.mPlants.Count;
			for (int i = 0; i < count; i++)
			{
				Plant plant = mBoard.mPlants[i];
				if (!plant.mDead && mRow == plant.mRow)
				{
					TRect plantRect = plant.GetPlantRect();
					int rectOverlap = GameConstants.GetRectOverlap(zombieAttackRect, plantRect);
					int num = (mZombieType == ZombieType.ZOMBIE_DIGGER) ? 5 : 20;
					if (rectOverlap >= num && CanTargetPlant(plant, theAttackType))
					{
						return plant;
					}
				}
			}
			return null;
		}

		public void CheckSquish(ZombieAttackType theAttackType)
		{
			TRect zombieAttackRect = GetZombieAttackRect();
			int count = mBoard.mPlants.Count;
			for (int i = 0; i < count; i++)
			{
				Plant plant = mBoard.mPlants[i];
				if (!plant.mDead && mRow == plant.mRow)
				{
					TRect plantRect = plant.GetPlantRect();
					int rectOverlap = GameConstants.GetRectOverlap(zombieAttackRect, plantRect);
					if (rectOverlap >= 20 && CanTargetPlant(plant, theAttackType) && !plant.IsSpiky())
					{
						SquishAllInSquare(plant.mPlantCol, plant.mRow, theAttackType);
						break;
					}
				}
			}
			if (mApp.IsIZombieLevel())
			{
				GridItem gridItem = mBoard.mChallenge.IZombieGetBrainTarget(this);
				if (gridItem != null)
				{
					mBoard.mChallenge.IZombieSquishBrain(gridItem);
				}
			}
		}

		public void RiseFromGrave(int theCol, int theRow)
		{
			Debug.ASSERT(mZombiePhase == ZombiePhase.PHASE_ZOMBIE_NORMAL);
			mPosX = mBoard.GridToPixelX(theCol, mRow) - 25;
			mPosY = GetPosYBasedOnRow(theRow);
			SetRow(theRow);
			mX = (int)mPosX;
			mY = (int)mPosY;
			mZombiePhase = ZombiePhase.PHASE_RISING_FROM_GRAVE;
			mPhaseCounter = 150;
			mAltitude = -200f;
			mUsesClipping = true;
			if (mBoard.StageHasPool())
			{
				mInPool = true;
				mPhaseCounter = 50;
				mAltitude = -150f;
				mZombieHeight = ZombieHeight.HEIGHT_ZOMBIE_NORMAL;
				StartWalkAnim(0);
				ReanimIgnoreClipRect(GlobalMembersReanimIds.ReanimTrackId_zombie_duckytube, false);
				ReanimIgnoreClipRect(GlobalMembersReanimIds.ReanimTrackId_zombie_whitewater, false);
				ReanimIgnoreClipRect(GlobalMembersReanimIds.ReanimTrackId_zombie_outerarm_hand, false);
				ReanimIgnoreClipRect(GlobalMembersReanimIds.ReanimTrackId_zombie_innerarm3, false);
				Reanimation reanimation = mApp.ReanimationGet(mBodyReanimID);
				TodParticleSystem theParticleSystem = mApp.AddTodParticle(0f, 0f, 0, ParticleEffect.PARTICLE_ZOMBIE_SEAWEED);
				OverrideParticleScale(theParticleSystem);
				if (mZombieType == ZombieType.ZOMBIE_TRAFFIC_CONE && theParticleSystem != null)
				{
					reanimation.AttachParticleToTrack(GlobalMembersReanimIds.ReanimTrackId_anim_cone, ref theParticleSystem, 37f * Constants.S, 20f * Constants.S);
				}
				else if (mZombieType == ZombieType.ZOMBIE_PAIL && theParticleSystem != null)
				{
					reanimation.AttachParticleToTrack(GlobalMembersReanimIds.ReanimTrackId_anim_bucket, ref theParticleSystem, 37f * Constants.S, 20f * Constants.S);
				}
				else if (theParticleSystem != null)
				{
					reanimation.AttachParticleToTrack(GlobalMembersReanimIds.ReanimTrackId_anim_head1, ref theParticleSystem, 30f * Constants.S, 20f * Constants.S);
				}
				PoolSplash(false);
			}
			else
			{
				int num = (int)mPosX + 60;
				int num2 = (int)mPosY + 110;
				if (IsOnHighGround())
				{
					num2 -= Constants.HIGH_GROUND_HEIGHT;
				}
				int aRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_PARTICLE, theRow, 0);
				ParticleEffect theEffect = ParticleEffect.PARTICLE_ZOMBIE_RISE;
				if (mApp.IsWhackAZombieLevel())
				{
					theEffect = ParticleEffect.PARTICLE_WHACK_A_ZOMBIE_RISE;
					mApp.PlayFoley(FoleyType.FOLEY_DIRT_RISE);
				}
				else
				{
					mApp.PlayFoley(FoleyType.FOLEY_GRAVESTONE_RUMBLE);
				}
				mApp.AddTodParticle(num, num2, aRenderOrder, theEffect);
			}
		}

		public void UpdateZombieRiseFromGrave()
		{
			if (mInPool)
			{
				mBodyReanimID.mClip = false;
				mAltitude = (float)TodCommon.TodAnimateCurve(50, 0, mPhaseCounter, -150, -40, TodCurves.CURVE_LINEAR) * mScaleZombie;
				mUsesClipping = true;
			}
			else
			{
				mBodyReanimID.mClip = true;
				mAltitude = TodCommon.TodAnimateCurve(50, 0, mPhaseCounter, -200, 0, TodCurves.CURVE_LINEAR);
				mUsesClipping = (mAltitude < 0f);
			}
			if (mPhaseCounter <= 0)
			{
				mZombiePhase = ZombiePhase.PHASE_ZOMBIE_NORMAL;
				if (IsOnHighGround())
				{
					mAltitude = Constants.HIGH_GROUND_HEIGHT;
				}
				if (mInPool)
				{
					ReanimIgnoreClipRect(GlobalMembersReanimIds.ReanimTrackId_zombie_duckytube, true);
					ReanimIgnoreClipRect(GlobalMembersReanimIds.ReanimTrackId_zombie_whitewater, true);
					ReanimIgnoreClipRect(GlobalMembersReanimIds.ReanimTrackId_zombie_outerarm_hand, true);
					ReanimIgnoreClipRect(GlobalMembersReanimIds.ReanimTrackId_zombie_innerarm3, true);
				}
			}
		}

		public void UpdateDamageStates(uint theDamageFlags)
		{
			if (!CanLoseBodyParts())
			{
				return;
			}
			if (mHasArm && mBodyHealth < 2 * mBodyMaxHealth / 3 && mBodyHealth > 0)
			{
				DropArm(theDamageFlags);
			}
			if (mHasHead && mBodyHealth < mBodyMaxHealth / 3)
			{
				DropHead(theDamageFlags);
				DropLoot();
				StopZombieSound();
				if (mBoard.HasLevelAwardDropped())
				{
					PlayDeathAnim(theDamageFlags);
				}
				if (mZombiePhase == ZombiePhase.PHASE_SNORKEL_WALKING_IN_POOL)
				{
					DieNoLoot(false);
				}
			}
		}

		public void UpdateZombiePool()
		{
			if (mZombieHeight == ZombieHeight.HEIGHT_OUT_OF_POOL)
			{
				mAltitude += 3f;
				if (mZombieType == ZombieType.ZOMBIE_SNORKEL)
				{
					mAltitude += 3f;
				}
				if (mAltitude >= 0f)
				{
					mAltitude = 0f;
					mZombieHeight = ZombieHeight.HEIGHT_ZOMBIE_NORMAL;
					mInPool = false;
				}
			}
			else if (mZombieHeight == ZombieHeight.HEIGHT_IN_TO_POOL)
			{
				mAltitude -= 3f;
				int num = -40;
				num *= (int)mScaleZombie;
				if (mAltitude <= (float)num)
				{
					mAltitude = num;
					mZombieHeight = ZombieHeight.HEIGHT_ZOMBIE_NORMAL;
					StartWalkAnim(0);
				}
			}
			else if (mZombieHeight == ZombieHeight.HEIGHT_DRAGGED_UNDER)
			{
				mAltitude -= 3f;
			}
		}

		public void CheckForPool()
		{
			if (!ZombieTypeCanGoInPool(mZombieType) || IsFlying() || mZombieType == ZombieType.ZOMBIE_DOLPHIN_RIDER || mZombieType == ZombieType.ZOMBIE_SNORKEL || mZombieHeight == ZombieHeight.HEIGHT_IN_TO_POOL || mZombieHeight == ZombieHeight.HEIGHT_OUT_OF_POOL)
			{
				return;
			}
			int theGridX = mBoard.PixelToGridX(mX + 75, mY);
			int theGridX2 = mBoard.PixelToGridX(mX + 45, mY);
			bool flag = false;
			if (mBoard.IsPoolSquare(theGridX, mRow) && mBoard.IsPoolSquare(theGridX2, mRow) && mPosX < 800f)
			{
				flag = true;
			}
			if (!mInPool && flag)
			{
				if (mBoard.mIceTrapCounter > 0)
				{
					mIceTrapCounter = mBoard.mIceTrapCounter;
					ApplyChill(true);
					return;
				}
				mZombieHeight = ZombieHeight.HEIGHT_IN_TO_POOL;
				mInPool = true;
				PoolSplash(true);
			}
			else if (mInPool && !flag)
			{
				mZombieHeight = ZombieHeight.HEIGHT_OUT_OF_POOL;
				StartWalkAnim(0);
				PoolSplash(false);
			}
			if (flag)
			{
				mUsesClipping = true;
			}
		}

		public void GetDrawPos(ref ZombieDrawPosition theDrawPos)
		{
			theDrawPos.mImageOffsetX = mPosX - (float)mX;
			theDrawPos.mImageOffsetY = mPosY - (float)mY;
			if (mIsEating)
			{
				theDrawPos.mHeadX = 47;
				theDrawPos.mHeadY = 4;
			}
			else if (mFrame == 0)
			{
				theDrawPos.mHeadX = 50;
				theDrawPos.mHeadY = 2;
			}
			else if (mFrame == 1)
			{
				theDrawPos.mHeadX = 49;
				theDrawPos.mHeadY = 1;
			}
			else if (mFrame == 2)
			{
				theDrawPos.mHeadX = 49;
				theDrawPos.mHeadY = 2;
			}
			else if (mFrame == 3)
			{
				theDrawPos.mHeadX = 48;
				theDrawPos.mHeadY = 4;
			}
			else if (mFrame == 4)
			{
				theDrawPos.mHeadX = 48;
				theDrawPos.mHeadY = 5;
			}
			else if (mFrame == 5)
			{
				theDrawPos.mHeadX = 48;
				theDrawPos.mHeadY = 4;
			}
			else if (mFrame == 6)
			{
				theDrawPos.mHeadX = 48;
				theDrawPos.mHeadY = 2;
			}
			else if (mFrame == 7)
			{
				theDrawPos.mHeadX = 49;
				theDrawPos.mHeadY = 1;
			}
			else if (mFrame == 8)
			{
				theDrawPos.mHeadX = 49;
				theDrawPos.mHeadY = 2;
			}
			else if (mFrame == 9)
			{
				theDrawPos.mHeadX = 50;
				theDrawPos.mHeadY = 4;
			}
			else if (mFrame == 10)
			{
				theDrawPos.mHeadX = 50;
				theDrawPos.mHeadY = 5;
			}
			else
			{
				theDrawPos.mHeadX = 50;
				theDrawPos.mHeadY = 4;
			}
			theDrawPos.mArmY = theDrawPos.mHeadY / 2;
			if (mZombieType != ZombieType.ZOMBIE_DOLPHIN_RIDER)
			{
				if (mZombieType == ZombieType.ZOMBIE_FOOTBALL)
				{
					theDrawPos.mImageOffsetY += -16f;
				}
				else if (mZombieType == ZombieType.ZOMBIE_YETI)
				{
					theDrawPos.mImageOffsetY += -20f;
				}
				else if (mZombieType == ZombieType.ZOMBIE_CATAPULT)
				{
					theDrawPos.mImageOffsetX += -25f;
					theDrawPos.mImageOffsetY += -18f;
				}
				else if (mZombieType == ZombieType.ZOMBIE_POGO)
				{
					theDrawPos.mImageOffsetY += 16f;
				}
				else if (mZombieType == ZombieType.ZOMBIE_BALLOON)
				{
					theDrawPos.mImageOffsetY += 17f;
				}
				else if (mZombieType == ZombieType.ZOMBIE_POLEVAULTER)
				{
					theDrawPos.mImageOffsetX += -6f;
					theDrawPos.mImageOffsetY += -11f;
				}
				else if (mZombieType == ZombieType.ZOMBIE_ZAMBONI)
				{
					theDrawPos.mImageOffsetX += 68f;
					theDrawPos.mImageOffsetY += -23f;
				}
				else if (mZombieType == ZombieType.ZOMBIE_GARGANTUAR || mZombieType == ZombieType.ZOMBIE_REDEYE_GARGANTUAR)
				{
					theDrawPos.mImageOffsetY += -8f;
				}
				else if (mZombieType == ZombieType.ZOMBIE_BOBSLED)
				{
					theDrawPos.mImageOffsetY += -12f;
				}
				else if (mZombieType == ZombieType.ZOMBIE_DANCER || mZombieType == ZombieType.ZOMBIE_BACKUP_DANCER)
				{
					theDrawPos.mImageOffsetY += 15f;
				}
			}
			if (mZombiePhase == ZombiePhase.PHASE_RISING_FROM_GRAVE)
			{
				theDrawPos.mBodyY = 0f - mAltitude;
				if (mInPool)
				{
					theDrawPos.mClipHeight = theDrawPos.mBodyY;
				}
				else
				{
					float num = Math.Min(mPhaseCounter, 40f);
					theDrawPos.mClipHeight = theDrawPos.mBodyY + num;
				}
				if (IsOnHighGround())
				{
					theDrawPos.mBodyY -= Constants.HIGH_GROUND_HEIGHT;
				}
			}
			else if (mZombieType == ZombieType.ZOMBIE_DOLPHIN_RIDER)
			{
				theDrawPos.mBodyY = 0f - mAltitude;
				theDrawPos.mClipHeight = GameConstants.CLIP_HEIGHT_OFF;
				if (mZombiePhase == ZombiePhase.PHASE_DOLPHIN_INTO_POOL)
				{
					Reanimation reanimation = mBodyReanimID;
					if (reanimation.mAnimTime >= 0.56f && reanimation.mAnimTime <= 0.65f)
					{
						theDrawPos.mClipHeight = 0f;
					}
					else if (reanimation.mAnimTime >= 0.75f)
					{
						theDrawPos.mClipHeight = 0f - mAltitude - 10f;
					}
				}
				else if (mZombiePhase == ZombiePhase.PHASE_DOLPHIN_RIDING)
				{
					theDrawPos.mImageOffsetX += 70f;
					if (mZombieHeight == ZombieHeight.HEIGHT_DRAGGED_UNDER)
					{
						theDrawPos.mClipHeight = 0f - mAltitude - 15f;
					}
					else
					{
						theDrawPos.mClipHeight = 0f - mAltitude - 10f;
					}
				}
				else if (mZombiePhase == ZombiePhase.PHASE_DOLPHIN_IN_JUMP)
				{
					theDrawPos.mImageOffsetX += 70f + mAltitude;
					Reanimation reanimation2 = mBodyReanimID;
					if (reanimation2.mAnimTime <= 0.06f)
					{
						theDrawPos.mClipHeight = 0f - mAltitude - 10f;
					}
					else if (reanimation2.mAnimTime >= 0.5f && reanimation2.mAnimTime <= 0.76f)
					{
						theDrawPos.mClipHeight = -13f;
					}
				}
				else if (mZombiePhase == ZombiePhase.PHASE_DOLPHIN_WALKING_IN_POOL || mZombiePhase == ZombiePhase.PHASE_ZOMBIE_DYING)
				{
					theDrawPos.mImageOffsetY += 50f;
					if (mZombiePhase == ZombiePhase.PHASE_ZOMBIE_DYING)
					{
						theDrawPos.mClipHeight = 0f - mAltitude + 44f;
					}
					else if (mZombieHeight == ZombieHeight.HEIGHT_DRAGGED_UNDER)
					{
						theDrawPos.mClipHeight = 0f - mAltitude + 36f;
					}
				}
				else if (mZombiePhase == ZombiePhase.PHASE_DOLPHIN_WALKING && mZombieHeight == ZombieHeight.HEIGHT_OUT_OF_POOL)
				{
					theDrawPos.mClipHeight = 0f - mAltitude;
				}
				else if (mZombiePhase == ZombiePhase.PHASE_DOLPHIN_WALKING_WITHOUT_DOLPHIN && mZombieHeight == ZombieHeight.HEIGHT_OUT_OF_POOL)
				{
					theDrawPos.mClipHeight = 0f - mAltitude;
				}
			}
			else if (mZombieType == ZombieType.ZOMBIE_SNORKEL)
			{
				theDrawPos.mBodyY = 0f - mAltitude;
				theDrawPos.mClipHeight = GameConstants.CLIP_HEIGHT_OFF;
				if (mZombiePhase == ZombiePhase.PHASE_SNORKEL_INTO_POOL)
				{
					Reanimation reanimation3 = mBodyReanimID;
					if (reanimation3.mAnimTime >= 0.8f)
					{
						theDrawPos.mClipHeight = -10f;
					}
				}
				else if (mInPool)
				{
					theDrawPos.mClipHeight = 0f - mAltitude - 5f;
					theDrawPos.mClipHeight += 20f - 20f * mScaleZombie;
				}
			}
			else if (mInPool)
			{
				theDrawPos.mBodyY = 0f - mAltitude;
				theDrawPos.mClipHeight = 0f - mAltitude - 7f;
				theDrawPos.mClipHeight += 10f - 10f * mScaleZombie;
				if (mIsEating)
				{
					theDrawPos.mClipHeight += 7f;
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_DANCER_RISING)
			{
				theDrawPos.mBodyY = 0f - mAltitude;
				theDrawPos.mClipHeight = 0f - mAltitude;
				if (IsOnHighGround())
				{
					theDrawPos.mBodyY -= Constants.HIGH_GROUND_HEIGHT;
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_DIGGER_RISING || mZombiePhase == ZombiePhase.PHASE_DIGGER_RISE_WITHOUT_AXE)
			{
				theDrawPos.mBodyY = 0f - mAltitude;
				if (mPhaseCounter > 20)
				{
					theDrawPos.mClipHeight = 0f - mAltitude;
				}
				else
				{
					theDrawPos.mClipHeight = GameConstants.CLIP_HEIGHT_OFF;
				}
			}
			else if (mZombieType == ZombieType.ZOMBIE_BUNGEE)
			{
				theDrawPos.mBodyY = 0f - mAltitude;
				theDrawPos.mImageOffsetX += -18f;
				if (IsOnHighGround())
				{
					theDrawPos.mBodyY -= Constants.HIGH_GROUND_HEIGHT;
				}
				theDrawPos.mClipHeight = GameConstants.CLIP_HEIGHT_OFF;
			}
			else
			{
				theDrawPos.mBodyY = 0f - mAltitude;
				theDrawPos.mClipHeight = GameConstants.CLIP_HEIGHT_OFF;
			}
		}

		public void UpdateZombieHighGround()
		{
			if (mZombieType == ZombieType.ZOMBIE_POGO)
			{
				return;
			}
			if (mZombieHeight == ZombieHeight.HEIGHT_UP_TO_HIGH_GROUND)
			{
				mAltitude += 3f;
				if (mAltitude >= (float)Constants.HIGH_GROUND_HEIGHT)
				{
					mAltitude = Constants.HIGH_GROUND_HEIGHT;
					mZombieHeight = ZombieHeight.HEIGHT_ZOMBIE_NORMAL;
				}
			}
			else if (mZombieHeight == ZombieHeight.HEIGHT_DOWN_OFF_HIGH_GROUND)
			{
				mAltitude -= 3f;
				if (mAltitude <= 0f)
				{
					mAltitude = 0f;
					mZombieHeight = ZombieHeight.HEIGHT_ZOMBIE_NORMAL;
					mOnHighGround = false;
				}
			}
		}

		public void CheckForHighGround()
		{
			if (mZombieHeight == ZombieHeight.HEIGHT_ZOMBIE_NORMAL && mZombieType != ZombieType.ZOMBIE_BUNGEE)
			{
				bool flag = IsOnHighGround();
				if (!mOnHighGround && flag)
				{
					mZombieHeight = ZombieHeight.HEIGHT_UP_TO_HIGH_GROUND;
					mOnHighGround = true;
				}
				else if (mOnHighGround && !flag)
				{
					mZombieHeight = ZombieHeight.HEIGHT_DOWN_OFF_HIGH_GROUND;
				}
			}
		}

		public bool IsOnHighGround()
		{
			if (!IsOnBoard())
			{
				return false;
			}
			int num = mBoard.PixelToGridXKeepOnBoard(mX + 75, mY);
			if (mBoard.mGridSquareType[num, mRow] == GridSquareType.GRIDSQUARE_HIGH_GROUND)
			{
				return true;
			}
			return false;
		}

		public void DropLoot()
		{
			if (!IsOnBoard())
			{
				return;
			}
			AlmanacDialog.AlmanacPlayerDefeatedZombie(mZombieType);
			if (mZombieType == ZombieType.ZOMBIE_YETI)
			{
				mBoard.mKilledYeti = true;
			}
			TrySpawnLevelAward();
			if (mDroppedLoot || mBoard.HasLevelAwardDropped() || !mBoard.CanDropLoot())
			{
				return;
			}
			mDroppedLoot = true;
			ZombieDefinition zombieDefinition = GetZombieDefinition(mZombieType);
			int mZombieValue = zombieDefinition.mZombieValue;
			if ((!mApp.IsLittleTroubleLevel() || RandomNumbers.NextNumber(4) == 0) && !mApp.IsIZombieLevel())
			{
				TRect zombieRect = GetZombieRect();
				int num = zombieRect.mX + zombieRect.mWidth / 2;
				int num2 = zombieRect.mY + zombieRect.mHeight / 4;
				if (mZombieType == ZombieType.ZOMBIE_YETI)
				{
					mApp.PlayFoley(FoleyType.FOLEY_SPAWN_SUN);
					mBoard.AddCoin(num - 20, num2, CoinType.COIN_DIAMOND, CoinMotion.COIN_MOTION_COIN);
					mBoard.AddCoin(num - 30, num2, CoinType.COIN_DIAMOND, CoinMotion.COIN_MOTION_COIN);
					mBoard.AddCoin(num - 40, num2, CoinType.COIN_DIAMOND, CoinMotion.COIN_MOTION_COIN);
					mBoard.AddCoin(num - 50, num2, CoinType.COIN_DIAMOND, CoinMotion.COIN_MOTION_COIN);
				}
				else
				{
					mBoard.DropLootPiece(num, num2, mZombieValue);
				}
			}
		}

		public bool TrySpawnLevelAward()
		{
			if (!IsOnBoard())
			{
				return false;
			}
			if (mBoard.HasLevelAwardDropped())
			{
				return false;
			}
			if (mBoard.mLevelComplete)
			{
				return false;
			}
			if (mDroppedLoot)
			{
				return false;
			}
			if (mApp.IsFinalBossLevel())
			{
				if (mZombieType != ZombieType.ZOMBIE_BOSS)
				{
					return false;
				}
			}
			else if (mApp.IsScaryPotterLevel())
			{
				if (!mBoard.mChallenge.ScaryPotterIsCompleted())
				{
					return false;
				}
			}
			else
			{
				if (mApp.IsContinuousChallenge())
				{
					return false;
				}
				if (mBoard.mCurrentWave < mBoard.mNumWaves)
				{
					return false;
				}
				if (mBoard.AreEnemyZombiesOnScreen())
				{
					return false;
				}
			}
			if (mApp.IsWhackAZombieLevel() && mBoard.mZombieCountDown > 0)
			{
				return false;
			}
			mBoard.mLevelAwardSpawned = true;
			mApp.mBoardResult = BoardResult.BOARDRESULT_WON;
			TRect zombieRect = GetZombieRect();
			int num = zombieRect.mX + zombieRect.mWidth / 2;
			int theY = zombieRect.mY + zombieRect.mHeight / 2;
			if (!mBoard.IsSurvivalStageWithRepick())
			{
				mBoard.RemoveAllZombies();
			}
			CoinType coinType;
			if (mApp.IsScaryPotterLevel() && !mBoard.IsFinalScaryPotterStage())
			{
				coinType = CoinType.COIN_NONE;
				int theGridX = mBoard.PixelToGridXKeepOnBoard((int)mPosX + 75, (int)mPosY);
				mBoard.mChallenge.PuzzlePhaseComplete(theGridX, mRow);
			}
			else if (mApp.IsAdventureMode() && mBoard.mLevel <= 50)
			{
				coinType = ((mBoard.mLevel != 9 && mBoard.mLevel != 19 && mBoard.mLevel != 29 && mBoard.mLevel != 39 && mBoard.mLevel != 49) ? ((mBoard.mLevel == 50) ? ((!mApp.HasFinishedAdventure()) ? CoinType.COIN_AWARD_MONEY_BAG : CoinType.COIN_AWARD_MONEY_BAG) : (mApp.HasFinishedAdventure() ? CoinType.COIN_AWARD_MONEY_BAG : ((mBoard.mLevel == 4) ? CoinType.COIN_SHOVEL : ((mBoard.mLevel == 14) ? CoinType.COIN_ALMANAC : ((mBoard.mLevel == 24) ? CoinType.COIN_CARKEYS : ((mBoard.mLevel == 34) ? CoinType.COIN_TACO : ((mBoard.mLevel != 44) ? CoinType.COIN_FINAL_SEED_PACKET : CoinType.COIN_WATERING_CAN))))))) : CoinType.COIN_NOTE);
			}
			else if (mBoard.IsSurvivalStageWithRepick())
			{
				coinType = CoinType.COIN_NONE;
				mBoard.FadeOutLevel();
			}
			else if (mApp.IsQuickPlayMode())
			{
				coinType = CoinType.COIN_AWARD_MONEY_BAG;
			}
			else if (!mBoard.IsLastStandStageWithRepick())
			{
				coinType = (mApp.IsAdventureMode() ? CoinType.COIN_AWARD_MONEY_BAG : ((!mApp.HasBeatenChallenge(mApp.mGameMode)) ? CoinType.COIN_TROPHY : CoinType.COIN_AWARD_MONEY_BAG));
			}
			else
			{
				coinType = CoinType.COIN_NONE;
				mBoard.FadeOutLevel();
				mApp.PlayFoley(FoleyType.FOLEY_SPAWN_SUN);
				for (int i = 0; i < 10; i++)
				{
					mBoard.AddCoin(num + i * 5, theY, CoinType.COIN_SUN, CoinMotion.COIN_MOTION_COIN);
				}
			}
			CoinMotion theCoinMotion = CoinMotion.COIN_MOTION_COIN;
			if (mZombieType == ZombieType.ZOMBIE_BOSS)
			{
				theCoinMotion = CoinMotion.COIN_MOTION_FROM_BOSS;
			}
			if (coinType != 0)
			{
				mApp.PlayFoley(FoleyType.FOLEY_SPAWN_SUN);
				mBoard.AddCoin(num, theY, coinType, theCoinMotion);
			}
			mDroppedLoot = true;
			return true;
		}

		public void StartZombieSound()
		{
			if (!mPlayingSong)
			{
				if (mZombiePhase == ZombiePhase.PHASE_JACK_IN_THE_BOX_RUNNING && mHasHead)
				{
					mApp.PlayFoley(FoleyType.FOLEY_JACKINTHEBOX);
					mPlayingSong = true;
				}
				else if (mZombiePhase == ZombiePhase.PHASE_DIGGER_TUNNELING)
				{
					mApp.PlayFoley(FoleyType.FOLEY_DIGGER);
					mPlayingSong = true;
				}
			}
		}

		public void StopZombieSound()
		{
			if (mZombieType == ZombieType.ZOMBIE_DANCER || mZombieType == ZombieType.ZOMBIE_BACKUP_DANCER)
			{
				if (mApp.mBoard != null)
				{
					bool flag = false;
					int count = mApp.mBoard.mZombies.Count;
					for (int i = 0; i < count; i++)
					{
						Zombie zombie = mApp.mBoard.mZombies[i];
						if (!zombie.mDead && zombie.mHasHead && !zombie.IsDeadOrDying() && zombie.IsOnBoard() && (zombie.mZombieType == ZombieType.ZOMBIE_DANCER || zombie.mZombieType == ZombieType.ZOMBIE_BACKUP_DANCER))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						mApp.mSoundSystem.StopFoley(FoleyType.FOLEY_DANCER);
					}
				}
				else
				{
					mApp.mSoundSystem.StopFoley(FoleyType.FOLEY_DANCER);
				}
			}
			if (mPlayingSong)
			{
				mPlayingSong = false;
				if (mZombieType == ZombieType.ZOMBIE_JACK_IN_THE_BOX)
				{
					mApp.mSoundSystem.StopFoley(FoleyType.FOLEY_JACKINTHEBOX);
				}
				else if (mZombieType == ZombieType.ZOMBIE_DIGGER)
				{
					mApp.mSoundSystem.StopFoley(FoleyType.FOLEY_DIGGER);
				}
			}
		}

		public void UpdateZombieJackInTheBox()
		{
			if (mZombiePhase == ZombiePhase.PHASE_JACK_IN_THE_BOX_RUNNING)
			{
				if (mPhaseCounter <= 0 && mHasHead)
				{
					mPhaseCounter = 110;
					mZombiePhase = ZombiePhase.PHASE_JACK_IN_THE_BOX_POPPING;
					StopZombieSound();
					mApp.PlaySample(Resources.SOUND_BOING);
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_pop, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 20, 28f);
				}
			}
			else
			{
				if (mZombiePhase != ZombiePhase.PHASE_JACK_IN_THE_BOX_POPPING)
				{
					return;
				}
				if (mPhaseCounter == 80)
				{
					mApp.PlayFoley(FoleyType.FOLEY_JACK_SURPRISE);
				}
				if (mPhaseCounter <= 0)
				{
					mApp.PlayFoley(FoleyType.FOLEY_EXPLOSION);
					int num = mX + mWidth / 2;
					int num2 = mY + mHeight / 2;
					int num3 = 127;
					if (mMindControlled)
					{
						mBoard.KillAllZombiesInRadius(mRow, num, num2, Constants.JackInTheBoxZombieRadius, 1, true, num3);
					}
					else
					{
						num3 |= 0x80;
						mBoard.KillAllZombiesInRadius(mRow, num, num2, Constants.JackInTheBoxZombieRadius, 1, true, num3);
						mBoard.KillAllPlantsInRadius(num, num2, Constants.JackInTheBoxPlantRadius);
					}
					mApp.AddTodParticle(num, num2, 400000, ParticleEffect.PARTICLE_JACKEXPLODE);
					mBoard.ShakeBoard(4, -6);
					DieNoLoot(false);
					if (mApp.IsScaryPotterLevel())
					{
						mBoard.mChallenge.ScaryPotterJackExplode(num, num2);
					}
				}
			}
		}

		public void DrawZombieHead(Graphics g, ref ZombieDrawPosition theDrawPos, int theFrame)
		{
		}

		public void UpdateZombiePosition()
		{
			if (mZombieType == ZombieType.ZOMBIE_BUNGEE || mZombieType == ZombieType.ZOMBIE_BOSS || mZombiePhase == ZombiePhase.PHASE_RISING_FROM_GRAVE || mZombieHeight == ZombieHeight.HEIGHT_ZOMBIQUARIUM)
			{
				return;
			}
			UpdateZombieWalking();
			CheckForZombieStep();
			if (mBlowingAway)
			{
				mPosX += 30f;
				if (mX > 850)
				{
					DieWithLoot();
					return;
				}
			}
			if (mZombieHeight != 0)
			{
				return;
			}
			float posYBasedOnRow = GetPosYBasedOnRow(mRow);
			if (mPosY < posYBasedOnRow)
			{
				mPosY += 3f * Math.Min(1f, posYBasedOnRow - mPosY);
				if (mPosY > posYBasedOnRow)
				{
					mPosY = posYBasedOnRow;
				}
			}
			else if (mPosY > posYBasedOnRow)
			{
				mPosY -= 3f * Math.Min(1f, mPosY - posYBasedOnRow);
				if (mPosY < posYBasedOnRow)
				{
					mPosY = posYBasedOnRow;
				}
			}
		}

		public TRect GetZombieRect()
		{
			if (cachedZombieRectUpToDate)
			{
				return cachedZombieRect;
			}
			cachedZombieRect = mZombieRect;
			if (IsWalkingBackwards())
			{
				cachedZombieRect.mX = mWidth - cachedZombieRect.mX - cachedZombieRect.mWidth;
			}
			ZombieDrawPosition theDrawPos = default(ZombieDrawPosition);
			GetDrawPos(ref theDrawPos);
			cachedZombieRect.Offset(mX, mY + (int)theDrawPos.mBodyY);
			if (theDrawPos.mClipHeight > GameConstants.CLIP_HEIGHT_LIMIT)
			{
				cachedZombieRect.mHeight -= (int)theDrawPos.mClipHeight;
			}
			cachedZombieRectUpToDate = true;
			return cachedZombieRect;
		}

		public TRect GetZombieAttackRect()
		{
			TRect result = mZombieAttackRect;
			if (mZombiePhase == ZombiePhase.PHASE_POLEVAULTER_IN_VAULT || mZombiePhase == ZombiePhase.PHASE_DOLPHIN_IN_JUMP)
			{
				result = new TRect(-40, 0, 100, 115);
			}
			if (IsWalkingBackwards())
			{
				result.mX = mWidth - result.mX - result.mWidth;
			}
			ZombieDrawPosition theDrawPos = default(ZombieDrawPosition);
			GetDrawPos(ref theDrawPos);
			result.Offset(mX, mY + (int)theDrawPos.mBodyY);
			if (theDrawPos.mClipHeight > GameConstants.CLIP_HEIGHT_LIMIT)
			{
				result.mHeight -= (int)theDrawPos.mClipHeight;
			}
			return result;
		}

		public void UpdateZombieWalking()
		{
			if (ZombieNotWalking())
			{
				return;
			}
			Reanimation reanimation = mApp.ReanimationTryToGet(mBodyReanimID);
			if (reanimation != null)
			{
				float num;
				if (IsBouncingPogo() || mZombiePhase == ZombiePhase.PHASE_BALLOON_FLYING || mZombiePhase == ZombiePhase.PHASE_DOLPHIN_RIDING || mZombiePhase == ZombiePhase.PHASE_SNORKEL_WALKING_IN_POOL || mZombieType == ZombieType.ZOMBIE_CATAPULT)
				{
					num = mVelX * 3f;
					if (IsMovingAtChilledSpeed())
					{
						num *= GameConstants.CHILLED_SPEED_FACTOR;
					}
				}
				else if (mZombieType == ZombieType.ZOMBIE_ZAMBONI || mZombiePhase == ZombiePhase.PHASE_DIGGER_TUNNELING || mZombiePhase == ZombiePhase.PHASE_DOLPHIN_IN_JUMP || IsBobsledTeamWithSled() || mZombiePhase == ZombiePhase.PHASE_POLEVAULTER_IN_VAULT || mZombiePhase == ZombiePhase.PHASE_SNORKEL_INTO_POOL)
				{
					num = mVelX * 3f;
				}
				else if (mHasGroundTrack)
				{
					mGroundTrackIndex = reanimation.GetTrackIndex(Reanimation.ReanimTrackId__ground);
					num = reanimation.GetTrackVelocity(mGroundTrackIndex) * mScaleZombie;
				}
				else
				{
					num = mVelX * 3f;
					if (IsMovingAtChilledSpeed())
					{
						num *= GameConstants.CHILLED_SPEED_FACTOR;
					}
				}
				if (mApp.mGameScene == GameScenes.SCENE_ZOMBIES_WON)
				{
					ZombieType mZombieType2 = mZombieType;
					int num3 = 9;
					if (num > 0.3f)
					{
						num = 0.3f;
					}
				}
				if (IsWalkingBackwards() || mZombiePhase == ZombiePhase.PHASE_DANCER_DANCING_IN)
				{
					mPosX += num;
				}
				else
				{
					mPosX -= num;
				}
				if (mZombieType == ZombieType.ZOMBIE_FOOTBALL && mFromWave != GameConstants.ZOMBIE_WAVE_WINNER)
				{
					if (reanimation.ShouldTriggerTimedEvent(0.03f))
					{
						mApp.AddTodParticle(mX + 81, mY + 106, mRenderOrder - 1, ParticleEffect.PARTICLE_DUST_FOOT);
					}
					if (reanimation.ShouldTriggerTimedEvent(0.61f))
					{
						mApp.AddTodParticle(mX + 87, mY + 110, mRenderOrder - 1, ParticleEffect.PARTICLE_DUST_FOOT);
					}
				}
				if (mZombiePhase == ZombiePhase.PHASE_POLEVAULTER_PRE_VAULT)
				{
					if (reanimation.ShouldTriggerTimedEvent(0.16f))
					{
						mApp.AddTodParticle(mX + 81, mY + 106, mRenderOrder - 1, ParticleEffect.PARTICLE_DUST_FOOT);
					}
					if (reanimation.ShouldTriggerTimedEvent(0.67f))
					{
						mApp.AddTodParticle(mX + 87, mY + 110, mRenderOrder - 1, ParticleEffect.PARTICLE_DUST_FOOT);
					}
				}
				return;
			}
			bool flag = false;
			if (mZombiePhase == ZombiePhase.PHASE_POLEVAULTER_IN_VAULT || mZombiePhase == ZombiePhase.PHASE_DIGGER_TUNNELING || mZombieType == ZombieType.ZOMBIE_DANCER || mZombieType == ZombieType.ZOMBIE_BACKUP_DANCER || mZombieType == ZombieType.ZOMBIE_BOBSLED || mZombieType == ZombieType.ZOMBIE_POGO || mZombieType == ZombieType.ZOMBIE_DOLPHIN_RIDER || mZombieType == ZombieType.ZOMBIE_BALLOON)
			{
				flag = true;
			}
			else if (mZombieType == ZombieType.ZOMBIE_SNORKEL && mInPool)
			{
				flag = true;
			}
			else if (mFrame >= 0 && mFrame <= 2)
			{
				flag = true;
			}
			else if (mFrame >= 6 && mFrame <= 8)
			{
				flag = true;
			}
			if (flag)
			{
				float num2 = mVelX * 3f;
				if (IsMovingAtChilledSpeed())
				{
					num2 *= GameConstants.CHILLED_SPEED_FACTOR;
				}
				if (IsWalkingBackwards())
				{
					mPosX += num2;
				}
				else
				{
					mPosX -= num2;
				}
			}
		}

		public void UpdateZombieWalkingIntoHouse()
		{
			if (mZombieType == ZombieType.ZOMBIE_BUNGEE || mZombieType == ZombieType.ZOMBIE_BOSS || mZombiePhase == ZombiePhase.PHASE_RISING_FROM_GRAVE || mZombieHeight == ZombieHeight.HEIGHT_ZOMBIQUARIUM)
			{
				return;
			}
			int num = 1;
			if (mZombieType == ZombieType.ZOMBIE_NORMAL || mZombieType == ZombieType.ZOMBIE_PAIL || mZombieType == ZombieType.ZOMBIE_TRAFFIC_CONE)
			{
				num = 2;
			}
			else if (mZombieType == ZombieType.ZOMBIE_ZAMBONI || mZombieType == ZombieType.ZOMBIE_GARGANTUAR || mZombieType == ZombieType.ZOMBIE_REDEYE_GARGANTUAR)
			{
				num = 4;
			}
			num *= 3;
			while (num-- != 0)
			{
				UpdateZombieWalking();
				if (mZombieHeight != 0)
				{
					continue;
				}
				float num2 = GameConstants.ZOMBIE_WALK_IN_FRONT_DOOR_Y;
				float val = 1f;
				if (mZombieType == ZombieType.ZOMBIE_GARGANTUAR || mZombieType == ZombieType.ZOMBIE_REDEYE_GARGANTUAR)
				{
					num2 += 30f;
				}
				else if (mZombiePhase == ZombiePhase.PHASE_POLEVAULTER_PRE_VAULT)
				{
					num2 += 35f;
				}
				else if (mZombieType == ZombieType.ZOMBIE_ZAMBONI)
				{
					num2 += 15f;
				}
				else if (mZombieType == ZombieType.ZOMBIE_FOOTBALL)
				{
					num2 += 15f;
					if (mRow == 0 || mRow == 1)
					{
						val = 2f;
					}
				}
				if (!WinningZombieReachedDesiredY && (mZombieType == ZombieType.ZOMBIE_GARGANTUAR || mZombieType == ZombieType.ZOMBIE_REDEYE_GARGANTUAR))
				{
					WinningZombieReachedDesiredY = true;
					ReanimReenableClipping();
				}
				if (mPosY < num2)
				{
					mPosY += Math.Min(val, num2 - mPosY);
				}
				else if (mPosY > num2)
				{
					mPosY -= Math.Min(val, mPosY - num2);
				}
				else if (!WinningZombieReachedDesiredY)
				{
					WinningZombieReachedDesiredY = true;
					ReanimReenableClipping();
				}
			}
		}

		public void UpdateZombieBobsled()
		{
			if (mZombiePhase == ZombiePhase.PHASE_BOBSLED_CRASHING)
			{
				if (mPhaseCounter > 0)
				{
					return;
				}
				mZombiePhase = ZombiePhase.PHASE_ZOMBIE_NORMAL;
				if (GetBobsledPosition() == 0)
				{
					for (int i = 0; i < 3; i++)
					{
						Zombie zombie = mBoard.ZombieGet(mFollowerZombieID[i]);
						zombie.mRelatedZombieID = null;
						mFollowerZombieID[i] = null;
						zombie.PickRandomSpeed();
					}
					PickRandomSpeed();
				}
				return;
			}
			if (mZombiePhase == ZombiePhase.PHASE_BOBSLED_SLIDING)
			{
				if (mPhaseCounter >= 0 && mPhaseCounter < 3)
				{
					mZombiePhase = ZombiePhase.PHASE_BOBSLED_BOARDING;
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_jump, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 0, 20f);
				}
			}
			else
			{
				if (mZombiePhase != ZombiePhase.PHASE_BOBSLED_BOARDING)
				{
					return;
				}
				Reanimation reanimation = mApp.ReanimationGet(mBodyReanimID);
				int theTimeAge = (int)(reanimation.mAnimTime * 50f);
				int bobsledPosition = GetBobsledPosition();
				if (bobsledPosition == 1 || bobsledPosition == 3)
				{
					mAltitude = TodCommon.TodAnimateCurveFloat(0, 50, theTimeAge, 8f, 18f, TodCurves.CURVE_LINEAR);
				}
				else
				{
					mAltitude = TodCommon.TodAnimateCurveFloat(0, 50, theTimeAge, -9f, 18f, TodCurves.CURVE_LINEAR);
				}
			}
			mBoard.mIceTimer[mRow] = Math.Max(500, mBoard.mIceTimer[mRow]);
			if (mPosX + 10f < (float)mBoard.mIceMinX[mRow] && GetBobsledPosition() == 0)
			{
				TakeDamage(6, 8u);
			}
		}

		public void BobsledCrash()
		{
			mZombiePhase = ZombiePhase.PHASE_BOBSLED_CRASHING;
			mPhaseCounter = GameConstants.BOBSLED_CRASH_TIME;
			mAltitude = 0f;
			mZombieRect = new TRect(36, 0, 42, 115);
			StartWalkAnim(0);
			Reanimation reanimation = mApp.ReanimationGet(mBodyReanimID);
			for (int i = 0; i < 3; i++)
			{
				Zombie zombie = mBoard.ZombieGet(mFollowerZombieID[i]);
				zombie.mZombiePhase = ZombiePhase.PHASE_BOBSLED_CRASHING;
				zombie.mPhaseCounter = GameConstants.BOBSLED_CRASH_TIME;
				zombie.mPosY = GetPosYBasedOnRow(mRow);
				zombie.mAltitude = 0f;
				zombie.StartWalkAnim(0);
				Reanimation reanimation2 = mApp.ReanimationGet(zombie.mBodyReanimID);
				if (reanimation2 != null)
				{
					zombie.mVelX = mVelX;
					reanimation2.mAnimTime = TodCommon.RandRangeFloat(0f, 1f);
					reanimation2.mAnimRate = reanimation.mAnimRate;
				}
			}
		}

		public Plant IsStandingOnSpikeweed()
		{
			if (mZombieType == ZombieType.ZOMBIE_ZAMBONI || mZombieType == ZombieType.ZOMBIE_CATAPULT)
			{
				return null;
			}
			TRect zombieRect = GetZombieRect();
			int count = mBoard.mPlants.Count;
			for (int i = 0; i < count; i++)
			{
				Plant plant = mBoard.mPlants[i];
				if (!plant.mDead && mRow == plant.mRow && plant.IsSpiky() && !plant.NotOnGround() && (!mOnHighGround || plant.IsOnHighGround()))
				{
					TRect plantAttackRect = plant.GetPlantAttackRect(PlantWeapon.WEAPON_PRIMARY);
					int rectOverlap = GameConstants.GetRectOverlap(plantAttackRect, zombieRect);
					if (rectOverlap > 0)
					{
						return plant;
					}
				}
			}
			return null;
		}

		public void CheckForZombieStep()
		{
			if ((mZombieType == ZombieType.ZOMBIE_ZAMBONI || mZombieType == ZombieType.ZOMBIE_CATAPULT) && !mFlatTires)
			{
				CheckSquish(ZombieAttackType.ATTACKTYPE_DRIVE_OVER);
			}
		}

		public void OverrideParticleColor(TodParticleSystem aParticle)
		{
			if (aParticle != null)
			{
				if (mMindControlled)
				{
					aParticle.OverrideColor(null, GameConstants.ZOMBIE_MINDCONTROLLED_COLOR);
					aParticle.OverrideExtraAdditiveDraw(null, true);
				}
				else if (mChilledCounter > 0 || mIceTrapCounter > 0)
				{
					aParticle.OverrideColor(null, new SexyColor(75, 75, 255, 255));
					aParticle.OverrideExtraAdditiveDraw(null, true);
				}
			}
		}

		public void OverrideParticleScale(TodParticleSystem aParticle)
		{
			if (aParticle != null)
			{
				aParticle.OverrideScale(null, mScaleZombie);
			}
		}

		public void PoolSplash(bool theInToPoolSound)
		{
			float num = 23f;
			float num2 = 78f;
			if (mZombiePhase == ZombiePhase.PHASE_SNORKEL_WALKING_IN_POOL)
			{
				num += -37f;
				num2 += -8f;
			}
			int num3 = (mAltitude != 0f) ? ((int)num2) : ((int)(num2 * mScaleZombie));
			Reanimation reanimation = mApp.AddReanimation(mX + (int)(num * mScaleZombie), mY + num3, mRenderOrder + 1, ReanimationType.REANIM_SPLASH);
			reanimation.OverrideScale(0.8f, 0.8f);
			mApp.AddTodParticle((float)mX + num + 37f, (float)mY + num2 + 42f, mRenderOrder + 1, ParticleEffect.PARTICLE_PLANTING_POOL);
			if (theInToPoolSound)
			{
				mApp.PlayFoley(FoleyType.FOLEY_ZOMBIESPLASH);
			}
			else
			{
				mApp.PlayFoley(FoleyType.FOLEY_PLANT_WATER);
			}
		}

		public void UpdateZombieFlyer()
		{
			if (mApp.mGameMode == GameMode.GAMEMODE_CHALLENGE_HIGH_GRAVITY && mPosX < 720f + (float)Constants.BOARD_EXTRA_ROOM)
			{
				mAltitude -= 0.1f;
				if (mAltitude < -35f)
				{
					LandFlyer(0u);
				}
			}
			if (mZombiePhase == ZombiePhase.PHASE_BALLOON_POPPING)
			{
				Reanimation reanimation = mApp.ReanimationGet(mBodyReanimID);
				if (reanimation.mLoopCount > 0)
				{
					mZombiePhase = ZombiePhase.PHASE_BALLOON_WALKING;
					StartWalkAnim(0);
				}
			}
			if (mApp.IsIZombieLevel() && mZombiePhase == ZombiePhase.PHASE_BALLOON_FLYING && mBoard.mChallenge.IZombieGetBrainTarget(this) != null)
			{
				LandFlyer(0u);
			}
		}

		public void UpdateZombiePogo()
		{
			if (IsDeadOrDying() || IsImmobilizied() || !IsBouncingPogo() || mZombieHeight == ZombieHeight.HEIGHT_IN_TO_CHIMNEY)
			{
				return;
			}
			float num = 40f;
			if (mZombiePhase >= ZombiePhase.PHASE_POGO_HIGH_BOUNCE_1 && mZombiePhase <= ZombiePhase.PHASE_POGO_HIGH_BOUNCE_6)
			{
				num = 50f + 20f * (float)(mZombiePhase - 21);
			}
			else if (mZombiePhase == ZombiePhase.PHASE_POGO_FORWARD_BOUNCE_2)
			{
				num = 90f;
			}
			else if (mZombiePhase == ZombiePhase.PHASE_POGO_FORWARD_BOUNCE_7)
			{
				num = 170f;
			}
			float num2 = 9f;
			mAltitude = TodCommon.TodAnimateCurveFloat(GameConstants.POGO_BOUNCE_TIME, 0, mPhaseCounter, num2, num + num2, TodCurves.CURVE_BOUNCE_SLOW_MIDDLE);
			mFrame = TodCommon.ClampInt(3 - (int)mAltitude / 3, 0, 3);
			if (mPhaseCounter >= 8 && mPhaseCounter < 11)
			{
				Reanimation reanimation = mApp.ReanimationGet(mBodyReanimID);
				reanimation.mAnimTime = 0f;
				reanimation.mLoopType = ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD;
			}
			if (IsOnBoard() && mPhaseCounter >= 5 && mPhaseCounter < 8)
			{
				mApp.PlayFoley(FoleyType.FOLEY_POGO_ZOMBIE);
			}
			if (mZombieHeight == ZombieHeight.HEIGHT_UP_TO_HIGH_GROUND)
			{
				mAltitude += Constants.HIGH_GROUND_HEIGHT;
				mZombieHeight = ZombieHeight.HEIGHT_ZOMBIE_NORMAL;
			}
			else if (mZombieHeight == ZombieHeight.HEIGHT_DOWN_OFF_HIGH_GROUND)
			{
				mOnHighGround = false;
				mZombieHeight = ZombieHeight.HEIGHT_ZOMBIE_NORMAL;
			}
			else if (mOnHighGround)
			{
				mAltitude += Constants.HIGH_GROUND_HEIGHT;
			}
			if (mZombiePhase == ZombiePhase.PHASE_POGO_FORWARD_BOUNCE_2 && mPhaseCounter >= 71 && mPhaseCounter < 74)
			{
				Plant plant = FindPlantTarget(ZombieAttackType.ATTACKTYPE_VAULT);
				if (plant != null && plant.mSeedType == SeedType.SEED_TALLNUT)
				{
					mApp.PlayFoley(FoleyType.FOLEY_BONK);
					mApp.AddTodParticle(plant.mX + 60, plant.mY - 20, mRenderOrder + 1, ParticleEffect.PARTICLE_TALL_NUT_BLOCK);
					mShieldType = ShieldType.SHIELDTYPE_NONE;
					PogoBreak(0u);
					return;
				}
			}
			if (mPhaseCounter <= 0)
			{
				Plant plant = null;
				if (IsOnBoard())
				{
					plant = FindPlantTarget(ZombieAttackType.ATTACKTYPE_VAULT);
				}
				if (plant == null)
				{
					mZombiePhase = ZombiePhase.PHASE_POGO_BOUNCING;
					PickRandomSpeed();
					mPhaseCounter = GameConstants.POGO_BOUNCE_TIME;
				}
				else if (mZombiePhase == ZombiePhase.PHASE_POGO_HIGH_BOUNCE_1)
				{
					mZombiePhase = ZombiePhase.PHASE_POGO_FORWARD_BOUNCE_2;
					int num3 = mX - plant.mX + 60;
					mVelX = (float)num3 / (float)GameConstants.POGO_BOUNCE_TIME;
					mPhaseCounter = GameConstants.POGO_BOUNCE_TIME;
				}
				else
				{
					mZombiePhase = ZombiePhase.PHASE_POGO_HIGH_BOUNCE_1;
					mVelX = 0f;
					mPhaseCounter = GameConstants.POGO_BOUNCE_TIME;
				}
			}
		}

		public void UpdateZombieNewspaper()
		{
			if (mZombiePhase != ZombiePhase.PHASE_NEWSPAPER_MADDENING)
			{
				return;
			}
			Reanimation reanimation = mApp.ReanimationGet(mBodyReanimID);
			if (reanimation.mLoopCount > 0)
			{
				mZombiePhase = ZombiePhase.PHASE_NEWSPAPER_MAD;
				if (mBoard.CountZombiesOnScreen() <= 10 && mHasHead)
				{
					mApp.PlayFoley(FoleyType.FOLEY_NEWSPAPER_RARRGH);
				}
				StartWalkAnim(20);
				reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_anim_head1, AtlasResources.IMAGE_REANIM_ZOMBIE_PAPER_MADHEAD);
			}
		}

		public void LandFlyer(uint theDamageFlags)
		{
			if (!TodCommon.TestBit(theDamageFlags, 4) && mZombiePhase == ZombiePhase.PHASE_BALLOON_FLYING)
			{
				mApp.PlaySample(Resources.SOUND_BALLOON_POP);
				mZombiePhase = ZombiePhase.PHASE_BALLOON_POPPING;
				PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_pop, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 20, 24f);
			}
			if (mBoard.mPlantRow[mRow] == PlantRowType.PLANTROW_POOL)
			{
				DieWithLoot();
			}
			else
			{
				mZombieHeight = ZombieHeight.HEIGHT_FALLING;
			}
		}

		public void UpdateZombieDigger()
		{
			if (mZombiePhase == ZombiePhase.PHASE_DIGGER_TUNNELING)
			{
				if (mPosX < 90f)
				{
					mZombiePhase = ZombiePhase.PHASE_DIGGER_RISING;
					mPhaseCounter = 130;
					mAltitude = -120f;
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_drill, ReanimLoopType.REANIM_LOOP, 0, 20f);
					mApp.PlayFoley(FoleyType.FOLEY_DIRT_RISE);
					mApp.PlayFoley(FoleyType.FOLEY_WAKEUP);
					GlobalMembersAttachment.AttachmentDetachCrossFadeParticleType(ref mAttachmentID, ParticleEffect.PARTICLE_DIGGER_TUNNEL, null);
					StopZombieSound();
					mApp.AddTodParticle(mPosX + 60f, mPosY + 118f, mRenderOrder + 1, ParticleEffect.PARTICLE_DIGGER_RISE);
					Reanimation reanimation = mApp.AddReanimation(mPosX + 13f, mPosY + 97f, mRenderOrder + 1, ReanimationType.REANIM_DIGGER_DIRT);
					reanimation.mAnimRate = 24f;
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_DIGGER_RISING)
			{
				if (mPhaseCounter > 40)
				{
					mAltitude = TodCommon.TodAnimateCurve(130, 40, mPhaseCounter, -120, 20, TodCurves.CURVE_EASE_OUT);
				}
				else
				{
					mAltitude = TodCommon.TodAnimateCurve(30, 0, mPhaseCounter, 20, 0, TodCurves.CURVE_EASE_IN);
				}
				if (mPhaseCounter >= 30 && mPhaseCounter < 33)
				{
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_landing, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 0, 12f);
				}
				if (mPhaseCounter >= 0 && mPhaseCounter < 3)
				{
					mZombiePhase = ZombiePhase.PHASE_DIGGER_STUNNED;
					mAltitude = 0f;
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_dizzy, ReanimLoopType.REANIM_LOOP, 10, 12f);
				}
				mRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_LAWN_MOWER, mRow, 1);
			}
			else if (mZombiePhase == ZombiePhase.PHASE_DIGGER_TUNNELING_PAUSE_WITHOUT_AXE)
			{
				if (mPhaseCounter >= 150 && mPhaseCounter < 153)
				{
					AddAttachedReanim(23, 93, ReanimationType.REANIM_ZOMBIE_SURPRISE);
				}
				if (mPhaseCounter >= 0 && mPhaseCounter < 3)
				{
					mZombiePhase = ZombiePhase.PHASE_DIGGER_RISE_WITHOUT_AXE;
					mPhaseCounter = 130;
					mAltitude = -120f;
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_landing, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 0, 0f);
					mApp.PlayFoley(FoleyType.FOLEY_DIRT_RISE);
					mApp.AddTodParticle(mPosX + 60f, mPosY + 118f, mRenderOrder + 1, ParticleEffect.PARTICLE_DIGGER_RISE);
					Reanimation reanimation2 = mApp.AddReanimation(mPosX + 13f, mPosY + 97f, mRenderOrder + 1, ReanimationType.REANIM_DIGGER_DIRT);
					reanimation2.mAnimRate = 24f;
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_DIGGER_RISE_WITHOUT_AXE)
			{
				if (mPhaseCounter > 40)
				{
					mAltitude = TodCommon.TodAnimateCurve(130, 40, mPhaseCounter, -120, 20, TodCurves.CURVE_EASE_OUT);
				}
				else
				{
					mAltitude = TodCommon.TodAnimateCurve(30, 0, mPhaseCounter, 20, 0, TodCurves.CURVE_EASE_IN);
				}
				if (mPhaseCounter >= 30 && mPhaseCounter < 33)
				{
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_landing, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 20, 12f);
				}
				if (mPhaseCounter >= 0 && mPhaseCounter < 3)
				{
					mZombiePhase = ZombiePhase.PHASE_DIGGER_WALKING_WITHOUT_AXE;
					mAltitude = 0f;
					StartWalkAnim(20);
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_DIGGER_STUNNED)
			{
				Reanimation reanimation3 = mApp.ReanimationGet(mBodyReanimID);
				if (reanimation3.mLoopCount > 1)
				{
					mZombiePhase = ZombiePhase.PHASE_DIGGER_WALKING;
					StartWalkAnim(20);
				}
			}
			mUsesClipping = (mAltitude < 0f);
		}

		public bool IsWalkingBackwards()
		{
			if (mMindControlled)
			{
				return true;
			}
			if (mZombieHeight == ZombieHeight.HEIGHT_ZOMBIQUARIUM)
			{
				float num = mVelZ;
				if (num < (float)Math.PI / 2f || num > 4.712389f)
				{
					return true;
				}
			}
			if (mZombieType == ZombieType.ZOMBIE_DIGGER)
			{
				if (mZombiePhase == ZombiePhase.PHASE_DIGGER_RISING || mZombiePhase == ZombiePhase.PHASE_DIGGER_STUNNED || mZombiePhase == ZombiePhase.PHASE_DIGGER_WALKING)
				{
					return true;
				}
				if (mZombiePhase == ZombiePhase.PHASE_ZOMBIE_DYING || mZombiePhase == ZombiePhase.PHASE_ZOMBIE_BURNED || mZombiePhase == ZombiePhase.PHASE_ZOMBIE_MOWERED)
				{
					if (mHasObject)
					{
						return true;
					}
					return false;
				}
				return false;
			}
			if (mZombieType == ZombieType.ZOMBIE_YETI && !mHasObject)
			{
				return true;
			}
			return false;
		}

		public TodParticleSystem AddAttachedParticle(int thePosX, int thePosY, ParticleEffect theEffect)
		{
			if (mDead)
			{
				return null;
			}
			if (!doParticle)
			{
				return null;
			}
			if (GlobalMembersAttachment.IsFullOfAttachments(ref mAttachmentID))
			{
				return null;
			}
			TodParticleSystem todParticleSystem = mApp.AddTodParticle(mX + thePosX, mY + thePosY, 0, theEffect);
			if (todParticleSystem != null && !todParticleSystem.mDead)
			{
				GlobalMembersAttachment.AttachParticle(ref mAttachmentID, todParticleSystem, thePosX, thePosY);
			}
			return todParticleSystem;
		}

		public void PogoBreak(uint theDamageFlags)
		{
			if (mHasObject)
			{
				if (!TodCommon.TestBit(theDamageFlags, 4))
				{
					ZombieDrawPosition theDrawPos = default(ZombieDrawPosition);
					GetDrawPos(ref theDrawPos);
					int aRenderOrder = mRenderOrder + 1;
					float thePosX = 0f;
					float thePosY = 0f;
					GetTrackPosition(ref GlobalMembersReanimIds.ReanimTrackId_zombie_pogo_stick, ref thePosX, ref thePosY);
					TodParticleSystem aParticle = mApp.AddTodParticle(thePosX, thePosY + 30f, aRenderOrder, ParticleEffect.PARTICLE_ZOMBIE_POGO);
					OverrideParticleScale(aParticle);
				}
				Debug.ASSERT(mZombiePhase != ZombiePhase.PHASE_ZOMBIE_DYING && mZombiePhase != ZombiePhase.PHASE_ZOMBIE_BURNED && !mDead);
				mZombieHeight = ZombieHeight.HEIGHT_FALLING;
				mZombiePhase = ZombiePhase.PHASE_ZOMBIE_NORMAL;
				StartWalkAnim(0);
				mZombieRect = new TRect(36, 17, 42, 115);
				mZombieAttackRect = new TRect(20, 17, 50, 115);
				mShieldHealth = 0;
				mShieldType = ShieldType.SHIELDTYPE_NONE;
				mHasObject = false;
			}
		}

		public void UpdateZombieFalling()
		{
			mAltitude -= 3f;
			if (mZombiePhase == ZombiePhase.PHASE_POLEVAULTER_PRE_VAULT)
			{
				mAltitude -= 3f;
			}
			int num = 0;
			if (IsOnHighGround())
			{
				num = Constants.HIGH_GROUND_HEIGHT;
			}
			if (mAltitude <= (float)num)
			{
				mAltitude = num;
				mZombieHeight = ZombieHeight.HEIGHT_ZOMBIE_NORMAL;
			}
		}

		public void UpdateZombieDancer()
		{
			if (mIsEating)
			{
				return;
			}
			if (mSummonCounter > 0)
			{
				mSummonCounter--;
				if (mSummonCounter <= 0)
				{
					int dancerFrame = GetDancerFrame();
					if (dancerFrame == 12 && mHasHead && mPosX < (float)Constants.Zombie_Dancer_Dance_Limit_X)
					{
						mZombiePhase = ZombiePhase.PHASE_DANCER_SNAPPING_FINGERS_WITH_LIGHT;
						PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_point, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 20, 24f);
					}
					else
					{
						mSummonCounter = 1;
					}
				}
			}
			if (mZombiePhase == ZombiePhase.PHASE_DANCER_DANCING_IN)
			{
				if (mHasHead && mPhaseCounter <= 0)
				{
					mZombiePhase = ZombiePhase.PHASE_DANCER_SNAPPING_FINGERS;
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_point, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 20, 24f);
					PickRandomSpeed();
				}
				return;
			}
			if (mZombiePhase == ZombiePhase.PHASE_DANCER_SNAPPING_FINGERS || mZombiePhase == ZombiePhase.PHASE_DANCER_SNAPPING_FINGERS_WITH_LIGHT)
			{
				Reanimation reanimation = mApp.ReanimationGet(mBodyReanimID);
				if (reanimation.mLoopCount > 0)
				{
					if (mZombiePhase == ZombiePhase.PHASE_DANCER_SNAPPING_FINGERS && mBoard.CountZombiesOnScreen() <= 15)
					{
						mApp.PlayFoley(FoleyType.FOLEY_DANCER);
					}
					SummonBackupDancers();
					mZombiePhase = ZombiePhase.PHASE_DANCER_SNAPPING_FINGERS_HOLD;
					mPhaseCounter = 200;
				}
				return;
			}
			if (mZombiePhase == ZombiePhase.PHASE_DANCER_SNAPPING_FINGERS_HOLD)
			{
				if (mPhaseCounter > 0)
				{
					return;
				}
				mZombiePhase = ZombiePhase.PHASE_DANCER_DANCING_LEFT;
				PlayZombieReanim(ref Reanimation.ReanimTrackId_anim_walk, ReanimLoopType.REANIM_LOOP, 20, 0f);
			}
			ZombiePhase dancerPhase = GetDancerPhase();
			if (dancerPhase != mZombiePhase)
			{
				switch (dancerPhase)
				{
				case ZombiePhase.PHASE_DANCER_DANCING_LEFT:
					mZombiePhase = dancerPhase;
					PlayZombieReanim(ref Reanimation.ReanimTrackId_anim_walk, ReanimLoopType.REANIM_LOOP, 10, 0f);
					break;
				case ZombiePhase.PHASE_DANCER_WALK_TO_RAISE:
				{
					mZombiePhase = dancerPhase;
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_armraise, ReanimLoopType.REANIM_LOOP, 10, 18f);
					Reanimation reanimation2 = mApp.ReanimationGet(mBodyReanimID);
					reanimation2.mAnimTime = 0.6f;
					break;
				}
				case ZombiePhase.PHASE_DANCER_RAISE_LEFT_1:
				case ZombiePhase.PHASE_DANCER_RAISE_RIGHT_1:
				case ZombiePhase.PHASE_DANCER_RAISE_LEFT_2:
				case ZombiePhase.PHASE_DANCER_RAISE_RIGHT_2:
					mZombiePhase = dancerPhase;
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_armraise, ReanimLoopType.REANIM_LOOP, 10, 18f);
					break;
				}
			}
			if (mHasHead && mSummonCounter == 0 && NeedsMoreBackupDancers())
			{
				mSummonCounter = 100;
			}
		}

		public Zombie SummonBackupDancer(int theRow, int thePosX)
		{
			if (!mBoard.RowCanHaveZombieType(theRow, ZombieType.ZOMBIE_BACKUP_DANCER))
			{
				return null;
			}
			Zombie zombie = mBoard.AddZombie(ZombieType.ZOMBIE_BACKUP_DANCER, mFromWave);
			if (zombie == null)
			{
				return null;
			}
			zombie.mPosX = thePosX;
			zombie.mPosY = GetPosYBasedOnRow(theRow);
			zombie.SetRow(theRow);
			zombie.mX = (int)zombie.mPosX;
			zombie.mY = (int)zombie.mPosY;
			zombie.mZombiePhase = ZombiePhase.PHASE_DANCER_RISING;
			zombie.mPhaseCounter = 150;
			zombie.mAltitude = Constants.ZOMBIE_BACKUP_DANCER_RISE_HEIGHT;
			zombie.mUsesClipping = true;
			zombie.mRelatedZombieID = mBoard.ZombieGetID(this);
			zombie.SetAnimRate(0f);
			zombie.mMindControlled = mMindControlled;
			int num = (int)zombie.mPosX + 60;
			int num2 = (int)zombie.mPosY + 110;
			if (zombie.IsOnHighGround())
			{
				num2 -= Constants.HIGH_GROUND_HEIGHT;
			}
			int aRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_PARTICLE, theRow, 0);
			mApp.AddTodParticle(num, num2, aRenderOrder, ParticleEffect.PARTICLE_DANCER_RISE);
			mApp.PlayFoley(FoleyType.FOLEY_GRAVESTONE_RUMBLE);
			return mBoard.ZombieGetID(zombie);
		}

		public void SummonBackupDancers()
		{
			if (!mHasHead)
			{
				return;
			}
			for (int i = 0; i < GameConstants.NUM_BACKUP_DANCERS; i++)
			{
				if (mBoard.ZombieTryToGet(mFollowerZombieID[i]) != null)
				{
					continue;
				}
				int theRow = 0;
				int thePosX = 0;
				switch (i)
				{
				case 0:
					theRow = mRow - 1;
					thePosX = (int)mPosX;
					break;
				case 1:
					theRow = mRow + 1;
					thePosX = (int)mPosX;
					break;
				case 2:
					if (mPosX < 130f)
					{
						continue;
					}
					theRow = mRow;
					thePosX = (int)mPosX - 100;
					break;
				case 3:
					theRow = mRow;
					thePosX = (int)mPosX + 100;
					break;
				default:
					Debug.ASSERT(false);
					break;
				}
				mFollowerZombieID[i] = SummonBackupDancer(theRow, thePosX);
				if (mFollowerZombieID[i] != null)
				{
					mFollowerZombieID[i].mLeaderZombie = this;
				}
				mSummonedDancers = true;
			}
		}

		public int GetDancerFrame()
		{
			if (mFromWave == GameConstants.ZOMBIE_WAVE_UI)
			{
				return 0;
			}
			if (IsImmobilizied())
			{
				return 0;
			}
			int num = 20;
			int num2 = 23;
			if (mZombiePhase == ZombiePhase.PHASE_DANCER_DANCING_IN)
			{
				num2 = 11;
				num = 10;
			}
			return mApp.mAppCounter * 3 % (num * num2) / num;
		}

		public void BungeeStealTarget()
		{
			PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_grab, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 20, 24f);
			Plant topPlantAt = mBoard.GetTopPlantAt(mTargetCol, mRow, PlantPriority.TOPPLANT_BUNGEE_ORDER);
			if (topPlantAt != null && !topPlantAt.NotOnGround() && topPlantAt.mSeedType != SeedType.SEED_COBCANNON)
			{
				Debug.ASSERT(topPlantAt.mSeedType != SeedType.SEED_GRAVEBUSTER);
				mTargetPlantID = topPlantAt;
				topPlantAt.mOnBungeeState = PlantOnBungeeState.PLANT_GETTING_GRABBED_BY_BUNGEE;
				mRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_PROJECTILE, mRow, 0);
			}
		}

		public void UpdateYuckyFace()
		{
			mYuckyFaceCounter++;
			if (mYuckyFaceCounter > GameConstants.YUCKI_SHORT_PAUSE_TIME && mYuckyFaceCounter < GameConstants.YUCKI_HOLD_TIME && !HasYuckyFaceImage())
			{
				StopEating();
				mYuckyFaceCounter = GameConstants.YUCKI_HOLD_TIME;
				if (mBoard.CountZombiesOnScreen() <= 5 && mHasHead)
				{
					mApp.PlayFoley(FoleyType.FOLEY_YUCK);
				}
				else if (mBoard.CountZombiesOnScreen() <= 10 && mHasHead && RandomNumbers.NextNumber(2) == 0)
				{
					mApp.PlayFoley(FoleyType.FOLEY_YUCK);
				}
			}
			if (mYuckyFaceCounter > GameConstants.YUCKI_WALK_TIME)
			{
				ShowYuckyFace(false);
				mYuckyFace = false;
				mYuckyFaceCounter = 0;
				if (mYuckySwitchRowsLate)
				{
					mYuckySwitchRowsLate = false;
					SetRow(mYuckyToRow);
				}
				return;
			}
			if (mYuckyFaceCounter == GameConstants.YUCKI_PAUSE_TIME)
			{
				StopEating();
				ShowYuckyFace(true);
				if (mBoard.CountZombiesOnScreen() <= 5 && mHasHead)
				{
					mApp.PlayFoley(FoleyType.FOLEY_YUCK);
				}
				else if (mBoard.CountZombiesOnScreen() <= 10 && mHasHead && RandomNumbers.NextNumber(2) == 0)
				{
					mApp.PlayFoley(FoleyType.FOLEY_YUCK);
				}
			}
			if (mYuckyFaceCounter != GameConstants.YUCKI_HOLD_TIME)
			{
				return;
			}
			StartWalkAnim(20);
			bool flag = true;
			bool flag2 = true;
			bool flag3 = mBoard.mPlantRow[mRow] == PlantRowType.PLANTROW_POOL;
			if (!mBoard.RowCanHaveZombies(mRow - 1))
			{
				flag = false;
			}
			else if (mBoard.mPlantRow[mRow - 1] == PlantRowType.PLANTROW_POOL && !flag3)
			{
				flag = false;
			}
			else if (mBoard.mPlantRow[mRow - 1] != PlantRowType.PLANTROW_POOL && flag3)
			{
				flag = false;
			}
			if (!mBoard.RowCanHaveZombies(mRow + 1))
			{
				flag2 = false;
			}
			else if (mBoard.mPlantRow[mRow + 1] == PlantRowType.PLANTROW_POOL && !flag3)
			{
				flag2 = false;
			}
			else if (mBoard.mPlantRow[mRow + 1] != PlantRowType.PLANTROW_POOL && flag3)
			{
				flag2 = false;
			}
			if (flag && !flag2)
			{
				mBoard.ZombieSwitchRow(this, mRow - 1);
				SetRow(mRow - 1);
			}
			else if (!flag && flag2)
			{
				mBoard.ZombieSwitchRow(this, mRow + 1);
				mYuckyToRow = mRow + 1;
				mYuckySwitchRowsLate = true;
				mRow = mYuckyToRow;
			}
			else if (flag && flag2)
			{
				if (RandomNumbers.NextNumber(2) == 0)
				{
					mBoard.ZombieSwitchRow(this, mRow + 1);
					mYuckyToRow = mRow + 1;
					mYuckySwitchRowsLate = true;
					mRow = mYuckyToRow;
				}
				else
				{
					mBoard.ZombieSwitchRow(this, mRow - 1);
					SetRow(mRow - 1);
				}
			}
			else
			{
				Debug.ASSERT(false);
			}
		}

		public void DrawIceTrap(Graphics g, ref ZombieDrawPosition theDrawPos, bool theFront)
		{
			if (!mInPool && mZombieType != ZombieType.ZOMBIE_BOSS)
			{
				float num = 46f;
				float num2 = 92f + theDrawPos.mBodyY;
				float num3 = 1f;
				if (mZombieType == ZombieType.ZOMBIE_POGO)
				{
					num -= 10f;
					num2 += 20f;
				}
				else if (mZombieType == ZombieType.ZOMBIE_GARGANTUAR || mZombieType == ZombieType.ZOMBIE_REDEYE_GARGANTUAR)
				{
					num -= 20f;
					num2 -= 7f;
					num3 = 1.6f;
				}
				else if (mZombieType == ZombieType.ZOMBIE_BUNGEE)
				{
					num -= 45f;
					num2 -= 23f;
					num3 = 1.2f;
				}
				else if (mZombieType == ZombieType.ZOMBIE_DIGGER)
				{
					num -= 27f;
				}
				else if (mZombieType == ZombieType.ZOMBIE_CATAPULT)
				{
					num += 32f;
				}
				else if (mZombieType == ZombieType.ZOMBIE_BALLOON)
				{
					num -= 9f;
					num2 += 27f;
				}
				if (theFront)
				{
					TodCommon.TodDrawImageScaledF(g, AtlasResources.IMAGE_ICETRAP, num * Constants.S, num2 * Constants.S, num3, num3);
				}
				else
				{
					TodCommon.TodDrawImageScaledF(g, AtlasResources.IMAGE_ICETRAP2, num * Constants.S, num2 * Constants.S, num3, num3);
				}
			}
		}

		public bool HitIceTrap()
		{
			bool flag = false;
			if (mChilledCounter > 0 || mIceTrapCounter != 0)
			{
				flag = true;
			}
			ApplyChill(true);
			if (!CanBeFrozen())
			{
				return false;
			}
			if (mInPool)
			{
				mIceTrapCounter = 300;
			}
			else if (flag)
			{
				mIceTrapCounter = TodCommon.RandRangeInt(300, 400);
			}
			else
			{
				mIceTrapCounter = TodCommon.RandRangeInt(400, 600);
			}
			StopZombieSound();
			if (mZombieType == ZombieType.ZOMBIE_BALLOON)
			{
				BalloonPropellerHatSpin(false);
			}
			if (mZombiePhase == ZombiePhase.PHASE_BOSS_HEAD_SPIT)
			{
				mBoard.RemoveParticleByType(ParticleEffect.PARTICLE_ZOMBIE_BOSS_FIREBALL);
			}
			TakeDamage(20, 1u);
			UpdateAnimSpeed();
			return true;
		}

		public int GetHelmDamageIndex()
		{
			if (mHelmHealth < mHelmMaxHealth / 3)
			{
				return 2;
			}
			if (mHelmHealth < mHelmMaxHealth * 2 / 3)
			{
				return 1;
			}
			return 0;
		}

		public int GetShieldDamageIndex()
		{
			if (mShieldHealth < mShieldMaxHealth / 3)
			{
				return 2;
			}
			if (mShieldHealth < mShieldMaxHealth * 2 / 3)
			{
				return 1;
			}
			return 0;
		}

		public void DrawReanim(Graphics g, ref ZombieDrawPosition theDrawPos, int theBaseRenderGroup)
		{
			Reanimation reanimation = mApp.ReanimationTryToGet(mBodyReanimID);
			if (reanimation == null)
			{
				return;
			}
			g.SetDrawMode(Graphics.DrawMode.DRAWMODE_NORMAL);
			float mImageOffsetX = theDrawPos.mImageOffsetX;
			float num = theDrawPos.mImageOffsetY + theDrawPos.mBodyY;
			switch (mZombieType)
			{
			case ZombieType.ZOMBIE_SNORKEL:
				if (mZombiePhase != ZombiePhase.PHASE_ZOMBIE_DYING && mZombiePhase != ZombiePhase.PHASE_SNORKEL_WALKING)
				{
					num = ((mZombiePhase != ZombiePhase.PHASE_SNORKEL_INTO_POOL || mScaleZombie == 1f) ? (num - (float)(int)((float)Constants.Zombie_ClipOffset_Snorkel * mScaleZombie)) : (num - (float)Constants.Zombie_ClipOffset_Snorkel_intoPool_Small));
				}
				else if (mScaleZombie == 1f)
				{
					num -= (float)(int)((float)Constants.Zombie_ClipOffset_Snorkel_Dying * mScaleZombie);
				}
				else
				{
					num -= (float)Constants.Zombie_ClipOffset_Snorkel_Dying_Small;
					mUsesClipping = true;
				}
				if (draggedByTangleKelp)
				{
					num -= (float)(int)((float)Constants.Zombie_ClipOffset_Snorkel_Grabbed * mScaleZombie);
					mUsesClipping = true;
				}
				break;
			case ZombieType.ZOMBIE_TRAFFIC_CONE:
			case ZombieType.ZOMBIE_PAIL:
				num -= (float)(int)((float)Constants.Zombie_ClipOffset_Pail * mScaleZombie);
				break;
			case ZombieType.ZOMBIE_NORMAL:
				num -= (float)(int)((float)Constants.Zombie_ClipOffset_Normal * mScaleZombie);
				break;
			case ZombieType.ZOMBIE_FLAG:
				num -= (float)(int)((float)Constants.Zombie_ClipOffset_Pail * mScaleZombie);
				break;
			case ZombieType.ZOMBIE_DIGGER:
				num -= (float)(int)((float)Constants.Zombie_ClipOffset_Digger * mScaleZombie);
				break;
			case ZombieType.ZOMBIE_DOLPHIN_RIDER:
				num -= (float)(int)((float)Constants.Zombie_ClipOffset_Dolphin_Into_Pool * mScaleZombie);
				break;
			case ZombieType.ZOMBIE_PEA_HEAD:
			case ZombieType.ZOMBIE_WALLNUT_HEAD:
			case ZombieType.ZOMBIE_JALAPENO_HEAD:
			case ZombieType.ZOMBIE_GATLING_HEAD:
			case ZombieType.ZOMBIE_TALLNUT_HEAD:
				num -= (float)(int)((float)Constants.Zombie_ClipOffset_PeaHead_InPool * mScaleZombie);
				break;
			default:
				num -= (float)(int)((float)Constants.Zombie_ClipOffset_Default * mScaleZombie);
				break;
			}
			switch (mZombiePhase)
			{
			case ZombiePhase.PHASE_RISING_FROM_GRAVE:
				num = ((mScaleZombie != 1f) ? (num + (float)(int)((float)Constants.Zombie_ClipOffset_RisingFromGrave_Small * mScaleZombie)) : (num + (float)(int)((float)Constants.Zombie_ClipOffset_RisingFromGrave * mScaleZombie)));
				break;
			case ZombiePhase.PHASE_SNORKEL_INTO_POOL:
				num -= (float)(int)((float)Constants.Zombie_ClipOffset_Snorkel_Into_Pool * mScaleZombie);
				break;
			}
			if (mZombieType == ZombieType.ZOMBIE_NORMAL && mZombiePhase == ZombiePhase.PHASE_ZOMBIE_NORMAL && mInPool)
			{
				num = ((mScaleZombie != 1f) ? (num + (float)Constants.Zombie_ClipOffset_Normal_In_Pool_SMALL) : (num + (float)Constants.Zombie_ClipOffset_Normal_In_Pool));
			}
			if (mZombieType == ZombieType.ZOMBIE_TRAFFIC_CONE && mZombiePhase == ZombiePhase.PHASE_ZOMBIE_NORMAL && mInPool && mScaleZombie != 1f)
			{
				num += (float)Constants.Zombie_ClipOffset_TrafficCone_In_Pool_SMALL;
			}
			if (mZombieType == ZombieType.ZOMBIE_NORMAL && mZombiePhase == ZombiePhase.PHASE_ZOMBIE_DYING && mInPool)
			{
				num = ((mScaleZombie != 1f) ? (num + (float)Constants.Zombie_ClipOffset_Normal_In_Pool_SMALL) : (num + (float)Constants.Zombie_ClipOffset_Normal_In_Pool));
			}
			if (mZombieType == ZombieType.ZOMBIE_FLAG && mInPool)
			{
				num += (float)Constants.Zombie_ClipOffset_Flag_In_Pool;
			}
			if (mScaleZombie != 1f && mAltitude != 0f && mZombiePhase != ZombiePhase.PHASE_RISING_FROM_GRAVE && mInPool)
			{
				num -= mAltitude;
			}
			if (theDrawPos.mClipHeight > GameConstants.CLIP_HEIGHT_LIMIT)
			{
				float num2 = 120f - theDrawPos.mClipHeight + 71f;
				g.SetClipRect((int)((mImageOffsetX - 200f) * Constants.S), (int)(num * Constants.S), (int)(520f * Constants.S), (int)(num2 * Constants.S));
			}
			if (mUsesClipping)
			{
				g.mClipRect.mX++;
				g.HardwareClip();
			}
			int num3 = 255;
			if (mZombieFade >= 0)
			{
				num3 = TodCommon.ClampInt((int)((float)(255 * mZombieFade) / 30f), 0, 255);
			}
			SexyColor sexyColor = new SexyColor(255, 255, 255, num3);
			SexyColor sexyColor2 = SexyColor.Black;
			bool mEnableExtraAdditiveDraw = false;
			if (mZombiePhase == ZombiePhase.PHASE_ZOMBIE_BURNED)
			{
				sexyColor = new SexyColor(0, 0, 0, num3);
				sexyColor2 = SexyColor.Black;
				mEnableExtraAdditiveDraw = false;
			}
			else if (mZombieType == ZombieType.ZOMBIE_BOSS && mZombiePhase != ZombiePhase.PHASE_ZOMBIE_DYING && mBodyHealth < mBodyMaxHealth / GameConstants.BOSS_FLASH_HEALTH_FRACTION)
			{
				int num4 = TodCommon.TodAnimateCurve(0, 39, mBoard.mMainCounter % 40, (int)Constants.InvertAndScale(155f), (int)Constants.InvertAndScale(255f), TodCurves.CURVE_BOUNCE);
				if (mChilledCounter <= 0 && mIceTrapCounter <= 0)
				{
					sexyColor = new SexyColor(num4, num4, num4, num3);
				}
				else
				{
					int num5 = TodCommon.TodAnimateCurve(0, 39, mBoard.mMainCounter % 40, 65, 75, TodCurves.CURVE_BOUNCE);
					sexyColor = new SexyColor(num5, num5, num4, num3);
				}
				sexyColor2 = SexyColor.Black;
				mEnableExtraAdditiveDraw = false;
			}
			else if (mMindControlled)
			{
				sexyColor = GameConstants.ZOMBIE_MINDCONTROLLED_COLOR;
				sexyColor.mAlpha = num3;
				sexyColor2 = sexyColor;
				mEnableExtraAdditiveDraw = true;
			}
			else if (mChilledCounter > 0 || mIceTrapCounter > 0)
			{
				sexyColor = new SexyColor(75, 75, 255, num3);
				sexyColor2 = sexyColor;
				mEnableExtraAdditiveDraw = true;
			}
			else if (mZombieHeight == ZombieHeight.HEIGHT_ZOMBIQUARIUM && mBodyHealth < 100)
			{
				sexyColor = new SexyColor(100, 150, 25, num3);
				sexyColor2 = sexyColor;
				mEnableExtraAdditiveDraw = true;
			}
			if (mJustGotShotCounter > 0 && !IsBobsledTeamWithSled() && !GlobalStaticVars.gLowFramerate)
			{
				int num6 = mJustGotShotCounter * 10;
				SexyColor theColor = new SexyColor(num6, num6, num6, 255);
				sexyColor2 = TodCommon.ColorAdd(theColor, sexyColor2);
				mEnableExtraAdditiveDraw = true;
			}
			reanimation.mColorOverride = sexyColor;
			reanimation.mExtraAdditiveColor = sexyColor2;
			reanimation.mEnableExtraAdditiveDraw = mEnableExtraAdditiveDraw;
			if (mZombieType == ZombieType.ZOMBIE_BOBSLED)
			{
				DrawBobsledReanim(g, ref theDrawPos, true);
				reanimation.DrawRenderGroup(g, theBaseRenderGroup);
				DrawBobsledReanim(g, ref theDrawPos, false);
			}
			else if (mZombieType == ZombieType.ZOMBIE_BUNGEE)
			{
				DrawBungeeReanim(g, ref theDrawPos);
			}
			else if (mZombieType == ZombieType.ZOMBIE_DANCER)
			{
				DrawDancerReanim(g, ref theDrawPos);
			}
			else
			{
				reanimation.DrawRenderGroup(g, theBaseRenderGroup);
			}
			if (mShieldType != 0)
			{
				if (mZombiePhase == ZombiePhase.PHASE_ZOMBIE_BURNED)
				{
					reanimation.mColorOverride = new SexyColor(0, 0, 0, num3);
					reanimation.mExtraAdditiveColor = SexyColor.Black;
					reanimation.mEnableExtraAdditiveDraw = false;
				}
				else if (mShieldJustGotShotCounter > 0)
				{
					reanimation.mColorOverride = new SexyColor(255, 255, 255, num3);
					reanimation.mExtraAdditiveColor = SexyColor.White;
					reanimation.mEnableExtraAdditiveDraw = true;
				}
				else
				{
					reanimation.mColorOverride = new SexyColor(255, 255, 255, num3);
					reanimation.mExtraAdditiveColor = SexyColor.Black;
					reanimation.mEnableExtraAdditiveDraw = false;
				}
				float num7 = 0f;
				if (mShieldRecoilCounter > 0)
				{
					num7 = TodCommon.TodAnimateCurveFloat(12, 0, mShieldRecoilCounter, 3f, 0f, TodCurves.CURVE_LINEAR);
				}
				g.mTransX += (int)num7;
				reanimation.DrawRenderGroup(g, GameConstants.RENDER_GROUP_SHIELD);
				g.mTransX -= (int)num7;
			}
			if (mShieldType == ShieldType.SHIELDTYPE_NEWSPAPER || mShieldType == ShieldType.SHIELDTYPE_DOOR || mShieldType == ShieldType.SHIELDTYPE_LADDER)
			{
				reanimation.mColorOverride = sexyColor;
				reanimation.mExtraAdditiveColor = sexyColor2;
				reanimation.mEnableExtraAdditiveDraw = mEnableExtraAdditiveDraw;
				reanimation.DrawRenderGroup(g, GameConstants.RENDER_GROUP_OVER_SHIELD);
			}
			g.ClearClipRect();
			if (mUsesClipping)
			{
				g.EndHardwareClip();
			}
		}

		public void UpdatePlaying()
		{
			Debug.ASSERT(mBodyHealth > 0 || mZombiePhase == ZombiePhase.PHASE_BOBSLED_CRASHING);
			mGroanCounter -= 3;
			int count = mBoard.mZombies.Count;
			if (mGroanCounter <= 0 && RandomNumbers.NextNumber(count) == 0 && mHasHead && mZombieType != ZombieType.ZOMBIE_BOSS && !mBoard.HasLevelAwardDropped())
			{
				float aPitch = 0f;
				if (mApp.IsLittleTroubleLevel())
				{
					aPitch = TodCommon.RandRangeFloat(40f, 50f);
				}
				if (mZombieType == ZombieType.ZOMBIE_GARGANTUAR)
				{
					mApp.PlayFoley(FoleyType.FOLEY_LOWGROAN);
				}
				else if (mVariant)
				{
					mApp.PlayFoleyPitch(FoleyType.FOLEY_BRAINS, aPitch);
				}
				else
				{
					mApp.PlayFoleyPitch(FoleyType.FOLEY_GROAN, aPitch);
				}
				mGroanCounter = RandomNumbers.NextNumber(1000) + 500;
			}
			if (mIceTrapCounter > 0)
			{
				mIceTrapCounter -= 3;
				if (mIceTrapCounter <= 0)
				{
					RemoveIceTrap();
					AddAttachedParticle(75, 106, ParticleEffect.PARTICLE_ICE_TRAP_RELEASE);
				}
			}
			if (mChilledCounter > 0)
			{
				mChilledCounter -= 3;
				if (mChilledCounter <= 0)
				{
					UpdateAnimSpeed();
				}
			}
			if (mButteredCounter > 0)
			{
				mButteredCounter -= 3;
				if (mButteredCounter <= 0)
				{
					RemoveButter();
				}
			}
			if (mZombiePhase == ZombiePhase.PHASE_RISING_FROM_GRAVE)
			{
				UpdateZombieRiseFromGrave();
				return;
			}
			mBodyReanimID.mClip = false;
			if (!IsImmobilizied())
			{
				UpdateActions();
				UpdateZombiePosition();
				CheckIfPreyCaught();
				CheckForPool();
				CheckForHighGround();
				CheckForBoardEdge();
			}
			if (mZombieType == ZombieType.ZOMBIE_BOSS)
			{
				UpdateBoss();
			}
			if (IsDeadOrDying() || mFromWave == GameConstants.ZOMBIE_WAVE_WINNER)
			{
				return;
			}
			bool flag = !mHasHead;
			if ((mZombieType == ZombieType.ZOMBIE_ZAMBONI || mZombieType == ZombieType.ZOMBIE_CATAPULT) && mBodyHealth < 200)
			{
				flag = true;
			}
			if (flag)
			{
				int theDamage = 1;
				if (mZombieType == ZombieType.ZOMBIE_YETI)
				{
					theDamage = 10;
				}
				if (mBodyMaxHealth >= 500)
				{
					theDamage = 3;
				}
				if (RandomNumbers.NextNumber(5) == 0)
				{
					TakeDamage(theDamage, 9u);
				}
			}
		}

		public bool NeedsMoreBackupDancers()
		{
			for (int i = 0; i < GameConstants.NUM_BACKUP_DANCERS; i++)
			{
				if (mBoard.ZombieTryToGet(mFollowerZombieID[i]) == null && (i != 0 || mBoard.RowCanHaveZombieType(mRow - 1, ZombieType.ZOMBIE_BACKUP_DANCER)) && (i != 1 || mBoard.RowCanHaveZombieType(mRow + 1, ZombieType.ZOMBIE_BACKUP_DANCER)))
				{
					return true;
				}
			}
			return false;
		}

		public void ConvertToNormalZombie()
		{
			StopZombieSound();
			mPosY = GetPosYBasedOnRow(mRow);
			mX = (int)mPosX;
			mY = (int)mPosY;
			mZombieType = ZombieType.ZOMBIE_NORMAL;
			mZombiePhase = ZombiePhase.PHASE_ZOMBIE_NORMAL;
			mZombieAttackRect = new TRect(50, 0, 20, 115);
			mAnimFrames = 12;
			mAnimTicksPerFrame = 12;
			mPhaseCounter = 0;
			PickRandomSpeed();
		}

		public void StartEating()
		{
			if (mIsEating)
			{
				return;
			}
			mIsEating = true;
			if (mZombiePhase == ZombiePhase.PHASE_DIGGER_TUNNELING)
			{
				return;
			}
			if (mZombiePhase == ZombiePhase.PHASE_LADDER_CARRYING)
			{
				PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_laddereat, ReanimLoopType.REANIM_LOOP, 20, 0f);
				return;
			}
			if (mZombiePhase == ZombiePhase.PHASE_NEWSPAPER_MAD)
			{
				PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_eat_nopaper, ReanimLoopType.REANIM_LOOP, 20, 0f);
				return;
			}
			if (mZombieType != ZombieType.ZOMBIE_SNORKEL)
			{
				PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_eat, ReanimLoopType.REANIM_LOOP, 20, 0f);
			}
			if (mShieldType == ShieldType.SHIELDTYPE_DOOR)
			{
				ShowDoorArms(false);
			}
		}

		public void StopEating()
		{
			if (!mIsEating)
			{
				return;
			}
			mIsEating = false;
			Reanimation reanimation = mApp.ReanimationTryToGet(mBodyReanimID);
			if (mZombiePhase != ZombiePhase.PHASE_DIGGER_TUNNELING)
			{
				if (reanimation != null && mZombieType != ZombieType.ZOMBIE_SNORKEL)
				{
					StartWalkAnim(20);
				}
				if (mShieldType == ShieldType.SHIELDTYPE_DOOR)
				{
					ShowDoorArms(true);
				}
				UpdateAnimSpeed();
			}
		}

		public void UpdateAnimSpeed()
		{
			if (!IsOnBoard())
			{
				return;
			}
			Reanimation reanimation = mApp.ReanimationTryToGet(mBodyReanimID);
			if (reanimation == null)
			{
				return;
			}
			if (IsImmobilizied())
			{
				ApplyAnimRate(0f);
			}
			else if (mYuckyFace && mYuckyFaceCounter < GameConstants.YUCKI_HOLD_TIME)
			{
				ApplyAnimRate(0f);
			}
			else if (mZombiePhase == ZombiePhase.PHASE_SNORKEL_UP_TO_EAT || mZombiePhase == ZombiePhase.PHASE_SNORKEL_DOWN_FROM_EAT || IsDeadOrDying())
			{
				ApplyAnimRate(mOrginalAnimRate);
			}
			else if (mIsEating)
			{
				if (mZombieType == ZombieType.ZOMBIE_POLEVAULTER || mZombieType == ZombieType.ZOMBIE_BALLOON || mZombieType == ZombieType.ZOMBIE_IMP || mZombieType == ZombieType.ZOMBIE_DIGGER || mZombieType == ZombieType.ZOMBIE_JACK_IN_THE_BOX || mZombieType == ZombieType.ZOMBIE_SNORKEL || mZombieType == ZombieType.ZOMBIE_YETI)
				{
					ApplyAnimRate(20f);
				}
				else
				{
					ApplyAnimRate(36f);
				}
			}
			else if (ZombieNotWalking())
			{
				ApplyAnimRate(mOrginalAnimRate);
			}
			else if (IsBobsledTeamWithSled() || mZombieType == ZombieType.ZOMBIE_CATAPULT || mZombiePhase == ZombiePhase.PHASE_DOLPHIN_RIDING || mZombiePhase == ZombiePhase.PHASE_SNORKEL_WALKING_IN_POOL)
			{
				ApplyAnimRate(mOrginalAnimRate);
			}
			else if (mHasGroundTrack)
			{
				int num = reanimation.FindTrackIndex(Reanimation.ReanimTrackId__ground);
				ReanimatorTrack reanimatorTrack = reanimation.mDefinition.mTracks[num];
				ReanimatorTransform reanimatorTransform = reanimatorTrack.mTransforms[reanimation.mFrameStart];
				ReanimatorTransform reanimatorTransform2 = reanimatorTrack.mTransforms[reanimation.mFrameStart + reanimation.mFrameCount - 1];
				float num2 = reanimatorTransform2.mTransX - reanimatorTransform.mTransX;
				if (!(num2 < 1E-06f))
				{
					float num3 = (float)reanimation.mFrameCount / num2;
					float theAnimRate = mVelX * num3 * 47f / mScaleZombie;
					ApplyAnimRate(theAnimRate);
				}
			}
		}

		public void ReanimShowPrefix(string theTrackPrefix, int theRenderGroup)
		{
			Reanimation reanimation = mApp.ReanimationTryToGet(mBodyReanimID);
			if (reanimation != null)
			{
				reanimation.AssignRenderGroupToPrefix(theTrackPrefix, theRenderGroup);
			}
		}

		public void DetachPlantHead()
		{
			ReanimatorTrackInstance trackInstanceByName = mBodyReanimID.GetTrackInstanceByName(GlobalMembersReanimIds.ReanimTrackId_anim_head1);
			GlobalMembersAttachment.AttachmentDetach(ref trackInstanceByName.mAttachmentID);
		}

		public void DetachFlag()
		{
			ReanimatorTrackInstance trackInstanceByName = mBodyReanimID.GetTrackInstanceByName(GlobalMembersReanimIds.ReanimTrackId_zombie_flaghand);
			GlobalMembersAttachment.AttachmentDetach(ref trackInstanceByName.mAttachmentID);
		}

		public void PlayDeathAnim(uint theDamageFlags)
		{
			if (mZombiePhase == ZombiePhase.PHASE_ZOMBIE_DYING || mZombiePhase == ZombiePhase.PHASE_ZOMBIE_BURNED || mZombiePhase == ZombiePhase.PHASE_ZOMBIE_MOWERED)
			{
				return;
			}
			Reanimation reanimation = mApp.ReanimationTryToGet(mBodyReanimID);
			if (reanimation == null || !reanimation.TrackExists(GlobalMembersReanimIds.ReanimTrackId_anim_death))
			{
				DieNoLoot(true);
				return;
			}
			if (mZombieType == ZombieType.ZOMBIE_DOLPHIN_RIDER && mZombiePhase != ZombiePhase.PHASE_DOLPHIN_WALKING_IN_POOL)
			{
				DieNoLoot(true);
				return;
			}
			if (mZombiePhase == ZombiePhase.PHASE_SNORKEL_INTO_POOL || mZombiePhase == ZombiePhase.PHASE_SNORKEL_WALKING)
			{
				DieNoLoot(true);
				return;
			}
			if (mIceTrapCounter > 0)
			{
				AddAttachedParticle(75, 106, ParticleEffect.PARTICLE_ICE_TRAP_RELEASE);
				mIceTrapCounter = 0;
			}
			if (mButteredCounter > 0)
			{
				mButteredCounter = 0;
			}
			if (mYuckyFace)
			{
				ShowYuckyFace(false);
				mYuckyFace = false;
				mYuckyFaceCounter = 0;
			}
			if (TodCommon.TestBit(theDamageFlags, 4) && mZombieType != ZombieType.ZOMBIE_BOSS && mZombieType != ZombieType.ZOMBIE_GARGANTUAR && mZombieType != ZombieType.ZOMBIE_REDEYE_GARGANTUAR)
			{
				DieNoLoot(true);
				return;
			}
			if (mZombieType == ZombieType.ZOMBIE_POGO)
			{
				mAltitude = 0f;
			}
			GlobalMembersAttachment.AttachmentReanimTypeDie(ref mAttachmentID, ReanimationType.REANIM_ZOMBIE_SURPRISE);
			StopEating();
			if (mShieldType != 0)
			{
				DropShield(1u);
			}
			if (mZombieType == ZombieType.ZOMBIE_SQUASH_HEAD && !mHasHead)
			{
				DetachPlantHead();
				mApp.RemoveReanimation(ref mSpecialHeadReanimID);
				mSpecialHeadReanimID = null;
			}
			mZombiePhase = ZombiePhase.PHASE_ZOMBIE_DYING;
			mVelX = 0f;
			if (mZombieHeight == ZombieHeight.HEIGHT_ZOMBIQUARIUM)
			{
				PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_aquarium_death, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 20, 14f);
				return;
			}
			if (mZombieHeight == ZombieHeight.HEIGHT_UP_LADDER)
			{
				mZombieHeight = ZombieHeight.HEIGHT_FALLING;
			}
			float theAnimRate;
			if (mZombieType == ZombieType.ZOMBIE_FOOTBALL)
			{
				theAnimRate = 24f;
			}
			else if (mZombieType == ZombieType.ZOMBIE_GARGANTUAR || mZombieType == ZombieType.ZOMBIE_REDEYE_GARGANTUAR)
			{
				theAnimRate = 14f;
				mApp.PlayFoley(FoleyType.FOLEY_GARGANTUDEATH);
			}
			else if (mZombieType == ZombieType.ZOMBIE_SNORKEL)
			{
				theAnimRate = 14f;
			}
			else if (mZombieType == ZombieType.ZOMBIE_DIGGER)
			{
				theAnimRate = 18f;
			}
			else if (mZombieType == ZombieType.ZOMBIE_YETI)
			{
				theAnimRate = 14f;
			}
			else if (mZombieType == ZombieType.ZOMBIE_BOSS)
			{
				theAnimRate = 18f;
				BossDie();
				Reanimation reanimation2 = mApp.ReanimationGet(mSpecialHeadReanimID);
				reanimation2.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_anim_death, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 20, theAnimRate);
			}
			else
			{
				theAnimRate = TodCommon.RandRangeFloat(24f, 30f);
			}
			string theTrackName = GlobalMembersReanimIds.ReanimTrackId_anim_death;
			int num = RandomNumbers.NextNumber(100);
			bool flag = mApp.HasFinishedAdventure() || mBoard.mLevel > 5;
			if (mInPool && reanimation.TrackExists(GlobalMembersReanimIds.ReanimTrackId_anim_waterdeath))
			{
				theTrackName = GlobalMembersReanimIds.ReanimTrackId_anim_waterdeath;
				ReanimIgnoreClipRect(GlobalMembersReanimIds.ReanimTrackId_zombie_duckytube, false);
			}
			else if (num == 99 && reanimation.TrackExists(GlobalMembersReanimIds.ReanimTrackId_anim_superlongdeath) && flag && mChilledCounter == 0 && mBoard.CountZombiesOnScreen() <= 5)
			{
				theAnimRate = 14f;
				theTrackName = GlobalMembersReanimIds.ReanimTrackId_anim_superlongdeath;
			}
			else if (num > 50 && reanimation.TrackExists(GlobalMembersReanimIds.ReanimTrackId_anim_death2))
			{
				theTrackName = GlobalMembersReanimIds.ReanimTrackId_anim_death2;
			}
			PlayZombieReanim(ref theTrackName, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 20, theAnimRate);
			ReanimShowPrefix("anim_tongue", -1);
		}

		public void UpdateDeath()
		{
			Reanimation reanimation = mApp.ReanimationTryToGet(mBodyReanimID);
			if (reanimation == null)
			{
				DieNoLoot(true);
				return;
			}
			if (mZombieHeight == ZombieHeight.HEIGHT_FALLING)
			{
				UpdateZombieFalling();
			}
			if (mZombieType == ZombieType.ZOMBIE_GARGANTUAR || mZombieType == ZombieType.ZOMBIE_REDEYE_GARGANTUAR)
			{
				if (reanimation.ShouldTriggerTimedEvent(0.89f))
				{
					mBoard.ShakeBoard(0, 3);
				}
				else if (reanimation.ShouldTriggerTimedEvent(0.98f))
				{
					mBoard.ShakeBoard(0, 1);
				}
			}
			float num = -1f;
			if (mInPool || mZombieType == ZombieType.ZOMBIE_SNORKEL || mZombieType == ZombieType.ZOMBIE_ZAMBONI || mZombieType == ZombieType.ZOMBIE_DOLPHIN_RIDER || mZombieType == ZombieType.ZOMBIE_BUNGEE || mZombieType == ZombieType.ZOMBIE_CATAPULT || mZombieType == ZombieType.ZOMBIE_IMP || mZombieType == ZombieType.ZOMBIE_BOSS)
			{
				num = -1f;
			}
			else if (mZombieType == ZombieType.ZOMBIE_NORMAL || mZombieType == ZombieType.ZOMBIE_FLAG || mZombieType == ZombieType.ZOMBIE_TRAFFIC_CONE || mZombieType == ZombieType.ZOMBIE_PAIL || mZombieType == ZombieType.ZOMBIE_DOOR || mZombieType == ZombieType.ZOMBIE_PEA_HEAD || mZombieType == ZombieType.ZOMBIE_WALLNUT_HEAD || mZombieType == ZombieType.ZOMBIE_TALLNUT_HEAD || mZombieType == ZombieType.ZOMBIE_JALAPENO_HEAD || mZombieType == ZombieType.ZOMBIE_GATLING_HEAD || mZombieType == ZombieType.ZOMBIE_SQUASH_HEAD || mZombieType == ZombieType.ZOMBIE_DUCKY_TUBE)
			{
				num = (reanimation.IsAnimPlaying(GlobalMembersReanimIds.ReanimTrackId_anim_superlongdeath) ? 0.788f : ((!reanimation.IsAnimPlaying(GlobalMembersReanimIds.ReanimTrackId_anim_death2)) ? 0.77f : 0.71f));
			}
			else if (mZombieType == ZombieType.ZOMBIE_POLEVAULTER)
			{
				num = 0.68f;
			}
			else if (mZombieType == ZombieType.ZOMBIE_FOOTBALL)
			{
				num = 0.52f;
			}
			else if (mZombieType == ZombieType.ZOMBIE_NEWSPAPER)
			{
				num = 0.63f;
			}
			else if (mZombieType == ZombieType.ZOMBIE_DANCER || mZombieType == ZombieType.ZOMBIE_BACKUP_DANCER)
			{
				num = 0.83f;
			}
			else if (mZombieType == ZombieType.ZOMBIE_BOBSLED)
			{
				num = 0.81f;
			}
			else if (mZombieType == ZombieType.ZOMBIE_JACK_IN_THE_BOX)
			{
				num = 0.64f;
			}
			else if (mZombieType == ZombieType.ZOMBIE_BALLOON)
			{
				num = 0.68f;
			}
			else if (mZombieType == ZombieType.ZOMBIE_DIGGER)
			{
				num = 0.85f;
			}
			else if (mZombieType == ZombieType.ZOMBIE_POGO)
			{
				num = 0.84f;
			}
			else if (mZombieType == ZombieType.ZOMBIE_YETI)
			{
				num = 0.68f;
			}
			else if (mZombieType == ZombieType.ZOMBIE_LADDER)
			{
				num = 0.62f;
			}
			else if (mZombieType == ZombieType.ZOMBIE_GARGANTUAR || mZombieType == ZombieType.ZOMBIE_REDEYE_GARGANTUAR)
			{
				num = 0.86f;
			}
			if (num > 0f && reanimation.ShouldTriggerTimedEvent(num))
			{
				mApp.PlayFoley(FoleyType.FOLEY_ZOMBIE_FALLING);
				if (mZombieType == ZombieType.ZOMBIE_GARGANTUAR || mZombieType == ZombieType.ZOMBIE_REDEYE_GARGANTUAR)
				{
					mApp.PlayFoley(FoleyType.FOLEY_THUMP);
					mApp.Vibrate();
				}
				if (mBoard.mDaisyMode)
				{
					DoDaisies();
				}
			}
			if (mZombieType == ZombieType.ZOMBIE_BOSS)
			{
				if (reanimation.ShouldTriggerTimedEvent(0.1f) || reanimation.ShouldTriggerTimedEvent(0.12f) || reanimation.ShouldTriggerTimedEvent(0.15f) || reanimation.ShouldTriggerTimedEvent(0.19f) || reanimation.ShouldTriggerTimedEvent(0.2f) || reanimation.ShouldTriggerTimedEvent(0.26f) || reanimation.ShouldTriggerTimedEvent(0.3f) || reanimation.ShouldTriggerTimedEvent(0.4f) || reanimation.ShouldTriggerTimedEvent(0.42f) || reanimation.ShouldTriggerTimedEvent(0.5f) || reanimation.ShouldTriggerTimedEvent(0.58f) || reanimation.ShouldTriggerTimedEvent(0.61f) || reanimation.ShouldTriggerTimedEvent(0.71f))
				{
					float theX = TodCommon.RandRangeFloat(600f, 750f);
					float theY = TodCommon.RandRangeFloat(50f, 300f);
					mApp.AddTodParticle(theX, theY, 400000, ParticleEffect.PARTICLE_BOSS_EXPLOSION);
					mApp.PlayFoley(FoleyType.FOLEY_BOSS_EXPLOSION_SMALL);
				}
				Reanimation reanimation2 = mApp.ReanimationTryToGet(mSpecialHeadReanimID);
				if (reanimation.ShouldTriggerTimedEvent(0.93f))
				{
					mBoard.ShakeBoard(1, 2);
					mApp.PlayFoley(FoleyType.FOLEY_BOSS_EXPLOSION_SMALL);
					mApp.PlayFoley(FoleyType.FOLEY_THUMP);
					mApp.Vibrate();
				}
				if (reanimation.ShouldTriggerTimedEvent(0.99f))
				{
					reanimation2.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_anim_flag, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 20, 30f);
				}
				if (reanimation2.IsAnimPlaying(GlobalMembersReanimIds.ReanimTrackId_anim_flag) && reanimation2.mLoopCount > 0)
				{
					reanimation2.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_anim_flag_loop, ReanimLoopType.REANIM_LOOP, 20, 17f);
				}
				if (reanimation.mLoopCount > 0)
				{
					DropLoot();
				}
			}
			if (mZombieType == ZombieType.ZOMBIE_ZAMBONI && mPhaseCounter > 0)
			{
				mPhaseCounter -= 3;
				if (mPhaseCounter <= 0)
				{
					reanimation = mApp.ReanimationTryToGet(mBodyReanimID);
					if (reanimation.IsTrackShowing(GlobalMembersReanimIds.ReanimTrackId_anim_wheelie2))
					{
						mApp.AddTodParticle(mPosX + 80f, mPosY + 60f, mRenderOrder + 1, ParticleEffect.PARTICLE_ZAMBONI_EXPLOSION2);
					}
					else
					{
						mApp.AddTodParticle(mPosX + 80f, mPosY + 60f, mRenderOrder + 1, ParticleEffect.PARTICLE_ZAMBONI_EXPLOSION);
					}
					DieWithLoot();
					mApp.PlayFoley(FoleyType.FOLEY_EXPLOSION);
				}
			}
			else if (mZombieType == ZombieType.ZOMBIE_CATAPULT)
			{
				mPhaseCounter -= 3;
				if (mPhaseCounter <= 0)
				{
					mApp.AddTodParticle(mPosX + 80f, mPosY + 60f, mRenderOrder + 1, ParticleEffect.PARTICLE_CATAPULT_EXPLOSION);
					DieWithLoot();
					mApp.PlayFoley(FoleyType.FOLEY_EXPLOSION);
				}
			}
			else if (mZombieFade == -1 && reanimation.mLoopCount > 0 && mZombieType != ZombieType.ZOMBIE_BOSS)
			{
				if (mInPool)
				{
					mZombieFade = 30;
				}
				else
				{
					mZombieFade = 100;
				}
			}
		}

		public void DrawShadow(Graphics g)
		{
			ZombieDrawPosition theDrawPos = default(ZombieDrawPosition);
			GetDrawPos(ref theDrawPos);
			if (mApp.mGameScene == GameScenes.SCENE_ZOMBIES_WON && !SetupDrawZombieWon(g))
			{
				return;
			}
			int num = 0;
			float mImageOffsetX = theDrawPos.mImageOffsetX;
			float num2 = theDrawPos.mImageOffsetY + theDrawPos.mBodyY;
			float num3 = mScaleZombie;
			mImageOffsetX += mScaleZombie * 20f - 20f;
			if (IsOnBoard() && mBoard.StageIsNight())
			{
				num = 1;
			}
			if (mZombieType == ZombieType.ZOMBIE_FOOTBALL)
			{
				mImageOffsetX = ((!IsWalkingBackwards()) ? (mImageOffsetX + (20f + 21f * mScaleZombie)) : (mImageOffsetX + -11f * mScaleZombie));
				num2 += 16f;
			}
			else if (mZombieType == ZombieType.ZOMBIE_NEWSPAPER)
			{
				mImageOffsetX = ((!IsWalkingBackwards()) ? (mImageOffsetX + 29f) : (mImageOffsetX + 5f));
			}
			else if (mZombieType == ZombieType.ZOMBIE_POLEVAULTER)
			{
				mImageOffsetX = ((!IsWalkingBackwards()) ? (mImageOffsetX + 36f) : (mImageOffsetX + -5f));
				num2 += 11f;
			}
			else if (mZombieType == ZombieType.ZOMBIE_BOBSLED)
			{
				mImageOffsetX = ((!IsWalkingBackwards()) ? (mImageOffsetX + 20f) : (mImageOffsetX + 13f));
				num2 += 13f;
			}
			else if (mZombieType == ZombieType.ZOMBIE_IMP)
			{
				num3 *= 0.6f;
				num2 += 7f;
				mImageOffsetX = ((!IsWalkingBackwards()) ? (mImageOffsetX + 25f) : (mImageOffsetX + 13f));
			}
			else if (mZombieType == ZombieType.ZOMBIE_DIGGER)
			{
				num2 += 5f;
				mImageOffsetX = ((!IsWalkingBackwards()) ? (mImageOffsetX + 17f) : (mImageOffsetX + 14f));
			}
			else if (mZombieType == ZombieType.ZOMBIE_SNORKEL)
			{
				num2 += 5f;
				mImageOffsetX = ((!IsWalkingBackwards()) ? (mImageOffsetX + 35f) : (mImageOffsetX + -2f));
			}
			else if (mZombieType == ZombieType.ZOMBIE_DOLPHIN_RIDER)
			{
				num2 += 11f;
				mImageOffsetX = ((!IsWalkingBackwards()) ? (mImageOffsetX + 19f) : (mImageOffsetX + 15f));
			}
			else if (mZombieType == ZombieType.ZOMBIE_YETI)
			{
				num2 += 20f;
				mImageOffsetX = ((!IsWalkingBackwards()) ? (mImageOffsetX + 3f) : (mImageOffsetX + 20f));
			}
			else if (mZombieType != ZombieType.ZOMBIE_GARGANTUAR && mZombieType != ZombieType.ZOMBIE_REDEYE_GARGANTUAR)
			{
				mImageOffsetX = ((mApp.ReanimationTryToGet(mBodyReanimID) != null) ? ((!IsWalkingBackwards()) ? (mImageOffsetX + 23f) : (mImageOffsetX + 11f)) : ((!IsWalkingBackwards()) ? (mImageOffsetX + 35f) : (mImageOffsetX + -2f)));
			}
			else
			{
				num3 *= 1.5f;
				mImageOffsetX += 27f;
				num2 += 7f;
			}
			if (mZombieType == ZombieType.ZOMBIE_NEWSPAPER)
			{
				num2 += 4f;
			}
			else if (mZombieType == ZombieType.ZOMBIE_BALLOON)
			{
				num2 += 13f;
			}
			else if (mZombieType == ZombieType.ZOMBIE_BUNGEE)
			{
				mImageOffsetX += -12f;
				num3 = TodCommon.TodAnimateCurveFloat(GameConstants.BUNGEE_ZOMBIE_HEIGHT - 1000, 100, (int)mAltitude, 0.1f, 1.5f, TodCurves.CURVE_LINEAR);
			}
			else if (mZombieType == ZombieType.ZOMBIE_DANCER || mZombieType == ZombieType.ZOMBIE_BACKUP_DANCER)
			{
				num2 -= 18f;
			}
			if (mZombieHeight == ZombieHeight.HEIGHT_UP_LADDER || mZombieHeight == ZombieHeight.HEIGHT_FALLING || mZombiePhase == ZombiePhase.PHASE_IMP_GETTING_THROWN || mZombieType == ZombieType.ZOMBIE_BUNGEE || IsBouncingPogo() || IsFlying())
			{
				num2 += mAltitude;
				if (mOnHighGround)
				{
					num2 -= (float)Constants.HIGH_GROUND_HEIGHT;
				}
			}
			if (mUsesClipping)
			{
				g.HardwareClip();
			}
			if (mInPool)
			{
				num2 += 67f;
				TodCommon.TodDrawImageCenterScaledF(g, AtlasResources.IMAGE_WHITEWATER_SHADOW, mImageOffsetX * Constants.S, num2 * Constants.S, num3, num3);
			}
			else
			{
				num2 += 92f;
				if (num == 0)
				{
					TodCommon.TodDrawImageCenterScaledF(g, AtlasResources.IMAGE_PLANTSHADOW, mImageOffsetX * Constants.S, num2 * Constants.S, num3, num3);
				}
				else
				{
					TodCommon.TodDrawImageCenterScaledF(g, AtlasResources.IMAGE_PLANTSHADOW2, mImageOffsetX * Constants.S, num2 * Constants.S, num3, num3);
				}
			}
			if (mUsesClipping)
			{
				g.EndHardwareClip();
			}
			g.ClearClipRect();
		}

		public bool HasShadow()
		{
			if (mZombiePhase == ZombiePhase.PHASE_ZOMBIE_DYING || mZombiePhase == ZombiePhase.PHASE_DIGGER_RISING || mZombiePhase == ZombiePhase.PHASE_DIGGER_TUNNELING_PAUSE_WITHOUT_AXE || mZombiePhase == ZombiePhase.PHASE_DIGGER_RISE_WITHOUT_AXE || mZombiePhase == ZombiePhase.PHASE_DIGGER_TUNNELING || mZombiePhase == ZombiePhase.PHASE_RISING_FROM_GRAVE || mZombiePhase == ZombiePhase.PHASE_DANCER_RISING || mZombiePhase == ZombiePhase.PHASE_BOBSLED_BOARDING || mZombiePhase == ZombiePhase.PHASE_POLEVAULTER_IN_VAULT || mZombiePhase == ZombiePhase.PHASE_DOLPHIN_INTO_POOL || mZombiePhase == ZombiePhase.PHASE_SNORKEL_INTO_POOL)
			{
				return false;
			}
			if (mZombieType == ZombieType.ZOMBIE_ZAMBONI || mZombieType == ZombieType.ZOMBIE_CATAPULT || mZombieType == ZombieType.ZOMBIE_BOSS)
			{
				return false;
			}
			if (mZombieType == ZombieType.ZOMBIE_BUNGEE && (!IsOnBoard() || mHitUmbrella))
			{
				return false;
			}
			if (mZombieHeight == ZombieHeight.HEIGHT_DRAGGED_UNDER || mZombieHeight == ZombieHeight.HEIGHT_IN_TO_CHIMNEY || mZombieHeight == ZombieHeight.HEIGHT_GETTING_BUNGEE_DROPPED)
			{
				return false;
			}
			if (mInPool)
			{
				return false;
			}
			if (mApp.mGameMode == GameMode.GAMEMODE_CHALLENGE_INVISIGHOUL && mFromWave != GameConstants.ZOMBIE_WAVE_UI)
			{
				return false;
			}
			return true;
		}

		public Reanimation LoadReanim(ReanimationType theReanimationType)
		{
			Reanimation reanimation = mApp.AddReanimation(0f, 0f, 0, theReanimationType);
			mBodyReanimID = mApp.ReanimationGetID(reanimation);
			reanimation.mLoopType = ReanimLoopType.REANIM_LOOP;
			reanimation.mIsAttachment = true;
			mHasGroundTrack = reanimation.TrackExists(Reanimation.ReanimTrackId__ground);
			if (!IsOnBoard())
			{
				int num = RandomNumbers.NextNumber(4);
				if (num > 0 && reanimation.TrackExists(GlobalMembersReanimIds.ReanimTrackId_anim_idle2))
				{
					float theAnimRate = TodCommon.RandRangeFloat(12f, 24f);
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_idle2, ReanimLoopType.REANIM_LOOP, 0, theAnimRate);
				}
				else if (reanimation.TrackExists(GlobalMembersReanimIds.ReanimTrackId_anim_idle))
				{
					float theAnimRate2 = TodCommon.RandRangeFloat(12f, 18f);
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_idle, ReanimLoopType.REANIM_LOOP, 0, theAnimRate2);
				}
				reanimation.mAnimTime = TodCommon.RandRangeFloat(0f, 0.99f);
			}
			else
			{
				StartWalkAnim(0);
			}
			return reanimation;
		}

		public int TakeFlyingDamage(int theDamage, uint theDamageFlags)
		{
			if (!TodCommon.TestBit(theDamageFlags, 3))
			{
				mJustGotShotCounter = 25;
			}
			int num = Math.Min(mFlyingHealth, theDamage);
			int result = theDamage - num;
			mFlyingHealth -= num;
			if (mFlyingHealth == 0)
			{
				LandFlyer(theDamageFlags);
			}
			return result;
		}

		public int TakeShieldDamage(int theDamage, uint theDamageFlags)
		{
			if (!TodCommon.TestBit(theDamageFlags, 3))
			{
				mShieldJustGotShotCounter = 25;
				if (mJustGotShotCounter < 0)
				{
					mJustGotShotCounter = 0;
				}
			}
			if (!TodCommon.TestBit(theDamageFlags, 3) && !TodCommon.TestBit(theDamageFlags, 1))
			{
				mShieldRecoilCounter = 12;
				if (mShieldType == ShieldType.SHIELDTYPE_DOOR || mShieldType == ShieldType.SHIELDTYPE_LADDER)
				{
					mApp.PlayFoley(FoleyType.FOLEY_SHIELD_HIT);
				}
			}
			int shieldDamageIndex = GetShieldDamageIndex();
			int num = Math.Min(mShieldHealth, theDamage);
			int result = theDamage - num;
			mShieldHealth -= num;
			if (mShieldHealth == 0)
			{
				DropShield(theDamageFlags);
				return result;
			}
			int shieldDamageIndex2 = GetShieldDamageIndex();
			if (shieldDamageIndex != shieldDamageIndex2 || justLoaded)
			{
				Reanimation reanimation = mApp.ReanimationTryToGet(mBodyReanimID);
				if (mShieldType == ShieldType.SHIELDTYPE_DOOR && shieldDamageIndex2 == 1)
				{
					Debug.ASSERT(reanimation != null);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_anim_screendoor, AtlasResources.IMAGE_REANIM_ZOMBIE_SCREENDOOR2);
				}
				else if (mShieldType == ShieldType.SHIELDTYPE_DOOR && shieldDamageIndex2 == 2)
				{
					Debug.ASSERT(reanimation != null);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_anim_screendoor, AtlasResources.IMAGE_REANIM_ZOMBIE_SCREENDOOR3);
				}
				else if (mShieldType == ShieldType.SHIELDTYPE_NEWSPAPER && shieldDamageIndex2 == 1)
				{
					Debug.ASSERT(reanimation != null);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_paper_paper, AtlasResources.IMAGE_REANIM_ZOMBIE_PAPER_PAPER2);
				}
				else if (mShieldType == ShieldType.SHIELDTYPE_NEWSPAPER && shieldDamageIndex2 == 2)
				{
					Debug.ASSERT(reanimation != null);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_paper_paper, AtlasResources.IMAGE_REANIM_ZOMBIE_PAPER_PAPER3);
				}
				else if (mShieldType == ShieldType.SHIELDTYPE_LADDER && shieldDamageIndex2 == 1)
				{
					Debug.ASSERT(reanimation != null);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_ladder_1, AtlasResources.IMAGE_REANIM_ZOMBIE_LADDER_1_DAMAGE1);
				}
				else if (mShieldType == ShieldType.SHIELDTYPE_LADDER && shieldDamageIndex2 == 2)
				{
					Debug.ASSERT(reanimation != null);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_ladder_1, AtlasResources.IMAGE_REANIM_ZOMBIE_LADDER_1_DAMAGE2);
				}
			}
			return result;
		}

		public int TakeHelmDamage(int theDamage, uint theDamageFlags)
		{
			if (!TodCommon.TestBit(theDamageFlags, 3))
			{
				mJustGotShotCounter = 25;
			}
			int helmDamageIndex = GetHelmDamageIndex();
			int num = Math.Min(mHelmHealth, theDamage);
			int result = theDamage - num;
			mHelmHealth -= num;
			if (TodCommon.TestBit(theDamageFlags, 2))
			{
				ApplyChill(false);
			}
			if (mHelmHealth == 0)
			{
				DropHelm(theDamageFlags);
				return result;
			}
			int helmDamageIndex2 = GetHelmDamageIndex();
			if ((helmDamageIndex != helmDamageIndex2 && mBodyReanimID.mActive) || (justLoaded && mBodyReanimID.mActive))
			{
				Reanimation reanimation = mApp.ReanimationTryToGet(mBodyReanimID);
				if (mHelmType == HelmType.HELMTYPE_TRAFFIC_CONE && helmDamageIndex2 == 1 && reanimation != null)
				{
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_anim_cone, AtlasResources.IMAGE_REANIM_ZOMBIE_CONE2);
				}
				else if (mHelmType == HelmType.HELMTYPE_TRAFFIC_CONE && helmDamageIndex2 == 2 && reanimation != null)
				{
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_anim_cone, AtlasResources.IMAGE_REANIM_ZOMBIE_CONE3);
				}
				else if (mHelmType == HelmType.HELMTYPE_PAIL && helmDamageIndex2 == 1)
				{
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_anim_bucket, AtlasResources.IMAGE_REANIM_ZOMBIE_BUCKET2);
				}
				else if (mHelmType == HelmType.HELMTYPE_PAIL && helmDamageIndex2 == 2)
				{
					Debug.ASSERT(reanimation != null);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_anim_bucket, AtlasResources.IMAGE_REANIM_ZOMBIE_BUCKET3);
				}
				else if (mHelmType == HelmType.HELMTYPE_DIGGER && helmDamageIndex2 == 1)
				{
					Debug.ASSERT(reanimation != null);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_digger_hardhat, AtlasResources.IMAGE_REANIM_ZOMBIE_DIGGER_HARDHAT2);
				}
				else if (mHelmType == HelmType.HELMTYPE_DIGGER && helmDamageIndex2 == 2)
				{
					Debug.ASSERT(reanimation != null);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_digger_hardhat, AtlasResources.IMAGE_REANIM_ZOMBIE_DIGGER_HARDHAT3);
				}
				else if (mHelmType == HelmType.HELMTYPE_FOOTBALL && helmDamageIndex2 == 1)
				{
					Debug.ASSERT(reanimation != null);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_football_helmet, AtlasResources.IMAGE_REANIM_ZOMBIE_FOOTBALL_HELMET2);
				}
				else if (mHelmType == HelmType.HELMTYPE_FOOTBALL && helmDamageIndex2 == 2)
				{
					Debug.ASSERT(reanimation != null);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_football_helmet, AtlasResources.IMAGE_REANIM_ZOMBIE_FOOTBALL_HELMET3);
				}
				else if (mHelmType == HelmType.HELMTYPE_WALLNUT && helmDamageIndex2 == 1)
				{
					Reanimation reanimation2 = mApp.ReanimationGet(mSpecialHeadReanimID);
					if (reanimation2 != null)
					{
						reanimation2.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_anim_face, AtlasResources.IMAGE_REANIM_WALLNUT_CRACKED1);
					}
				}
				else if (mHelmType == HelmType.HELMTYPE_WALLNUT && helmDamageIndex2 == 2)
				{
					Reanimation reanimation3 = mApp.ReanimationGet(mSpecialHeadReanimID);
					if (reanimation3 != null)
					{
						reanimation3.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_anim_face, AtlasResources.IMAGE_REANIM_WALLNUT_CRACKED2);
					}
				}
				else if (mHelmType == HelmType.HELMTYPE_TALLNUT && helmDamageIndex2 == 1)
				{
					Reanimation reanimation4 = mApp.ReanimationGet(mSpecialHeadReanimID);
					if (reanimation4 != null)
					{
						reanimation4.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_anim_idle, AtlasResources.IMAGE_REANIM_TALLNUT_CRACKED1);
					}
				}
				else if (mHelmType == HelmType.HELMTYPE_TALLNUT && helmDamageIndex2 == 2)
				{
					Reanimation reanimation5 = mApp.ReanimationGet(mSpecialHeadReanimID);
					if (reanimation5 != null)
					{
						reanimation5.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_anim_idle, AtlasResources.IMAGE_REANIM_TALLNUT_CRACKED2);
					}
				}
			}
			return result;
		}

		public void TakeBodyDamage(int theDamage, uint theDamageFlags)
		{
			if (!TodCommon.TestBit(theDamageFlags, 3))
			{
				mJustGotShotCounter = 25;
			}
			if (TodCommon.TestBit(theDamageFlags, 2))
			{
				ApplyChill(false);
			}
			int num = mBodyHealth;
			int bodyDamageIndex = GetBodyDamageIndex();
			mBodyHealth -= theDamage;
			int bodyDamageIndex2 = GetBodyDamageIndex();
			if (mZombieType == ZombieType.ZOMBIE_ZAMBONI)
			{
				Reanimation reanimation = mApp.ReanimationGet(mBodyReanimID);
				if (!TodCommon.TestBit(theDamageFlags, 3))
				{
					mApp.PlayFoley(FoleyType.FOLEY_SHIELD_HIT);
				}
				if (TodCommon.TestBit(theDamageFlags, 5))
				{
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_zamboni_1, AtlasResources.IMAGE_REANIM_ZOMBIE_ZAMBONI_1_DAMAGE2);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_zamboni_2, AtlasResources.IMAGE_REANIM_ZOMBIE_ZAMBONI_2_DAMAGE2);
					ZamboniDeath(theDamageFlags);
				}
				else if (mBodyHealth <= 0)
				{
					ZamboniDeath(theDamageFlags);
				}
				else if (bodyDamageIndex != bodyDamageIndex2 && bodyDamageIndex2 == 1)
				{
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_zamboni_1, AtlasResources.IMAGE_REANIM_ZOMBIE_ZAMBONI_1_DAMAGE1);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_zamboni_2, AtlasResources.IMAGE_REANIM_ZOMBIE_ZAMBONI_2_DAMAGE1);
				}
				else if (bodyDamageIndex != bodyDamageIndex2 && bodyDamageIndex2 == 2)
				{
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_zamboni_1, AtlasResources.IMAGE_REANIM_ZOMBIE_ZAMBONI_1_DAMAGE2);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_zamboni_2, AtlasResources.IMAGE_REANIM_ZOMBIE_ZAMBONI_2_DAMAGE2);
					AddAttachedParticle(27, 72, ParticleEffect.PARTICLE_ZAMBONI_SMOKE);
				}
			}
			else if (mZombieType == ZombieType.ZOMBIE_CATAPULT)
			{
				Reanimation reanimation2 = mApp.ReanimationGet(mBodyReanimID);
				if (TodCommon.TestBit(theDamageFlags, 5) || mBodyHealth <= 0)
				{
					reanimation2.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_catapult_siding, AtlasResources.IMAGE_REANIM_ZOMBIE_CATAPULT_SIDING_DAMAGE);
					CatapultDeath(theDamageFlags);
				}
				else if (bodyDamageIndex != bodyDamageIndex2 && bodyDamageIndex2 == 1)
				{
					reanimation2.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_catapult_siding, AtlasResources.IMAGE_REANIM_ZOMBIE_CATAPULT_SIDING_DAMAGE);
				}
				else if (bodyDamageIndex != bodyDamageIndex2 && bodyDamageIndex2 == 2)
				{
					AddAttachedParticle(47, 77, ParticleEffect.PARTICLE_ZAMBONI_SMOKE);
				}
			}
			else if (mZombieType == ZombieType.ZOMBIE_GARGANTUAR || mZombieType == ZombieType.ZOMBIE_REDEYE_GARGANTUAR)
			{
				Reanimation reanimation3 = mApp.ReanimationGet(mBodyReanimID);
				if (bodyDamageIndex != bodyDamageIndex2 && bodyDamageIndex2 == 1)
				{
					reanimation3.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_gargantua_body1, AtlasResources.IMAGE_REANIM_ZOMBIE_GARGANTUAR_BODY1_2);
					reanimation3.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_gargantuar_outerarm_lower, AtlasResources.IMAGE_REANIM_ZOMBIE_GARGANTUAR_OUTERARM_LOWER2);
				}
				else if (bodyDamageIndex != bodyDamageIndex2 && bodyDamageIndex2 == 2)
				{
					reanimation3.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_gargantua_body1, AtlasResources.IMAGE_REANIM_ZOMBIE_GARGANTUAR_BODY1_3);
					reanimation3.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_gargantuar_outerleg_foot, AtlasResources.IMAGE_REANIM_ZOMBIE_GARGANTUAR_FOOT2);
					reanimation3.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_gargantuar_outerarm_lower, AtlasResources.IMAGE_REANIM_ZOMBIE_GARGANTUAR_OUTERARM_LOWER2);
					if (mZombieType == ZombieType.ZOMBIE_REDEYE_GARGANTUAR)
					{
						reanimation3.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_anim_head1, AtlasResources.IMAGE_REANIM_ZOMBIE_GARGANTUAR_HEAD2_REDEYE);
					}
					else
					{
						reanimation3.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_anim_head1, AtlasResources.IMAGE_REANIM_ZOMBIE_GARGANTUAR_HEAD2);
					}
				}
			}
			else if (mZombieType == ZombieType.ZOMBIE_BOSS)
			{
				if (!TodCommon.TestBit(theDamageFlags, 3))
				{
					mApp.PlayFoley(FoleyType.FOLEY_SHIELD_HIT);
				}
				Reanimation reanimation4 = mApp.ReanimationGet(mBodyReanimID);
				if (bodyDamageIndex != bodyDamageIndex2 && bodyDamageIndex2 == 1)
				{
					reanimation4.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_boss_head, AtlasResources.IMAGE_REANIM_ZOMBIE_BOSS_HEAD_DAMAGE1);
					reanimation4.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_boss_jaw, AtlasResources.IMAGE_REANIM_ZOMBIE_BOSS_JAW_DAMAGE1);
					reanimation4.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_boss_outerarm_hand, AtlasResources.IMAGE_REANIM_ZOMBIE_BOSS_OUTERARM_HAND_DAMAGE1);
					reanimation4.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_boss_outerarm_thumb2, AtlasResources.IMAGE_REANIM_ZOMBIE_BOSS_OUTERARM_THUMB_DAMAGE1);
					reanimation4.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_boss_innerleg_foot, AtlasResources.IMAGE_REANIM_ZOMBIE_BOSS_FOOT_DAMAGE1);
				}
				else if (bodyDamageIndex != bodyDamageIndex2 && bodyDamageIndex2 == 2)
				{
					reanimation4.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_boss_head, AtlasResources.IMAGE_REANIM_ZOMBIE_BOSS_HEAD_DAMAGE2);
					reanimation4.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_boss_jaw, AtlasResources.IMAGE_REANIM_ZOMBIE_BOSS_JAW_DAMAGE2);
					reanimation4.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_boss_outerarm_hand, AtlasResources.IMAGE_REANIM_ZOMBIE_BOSS_OUTERARM_HAND_DAMAGE2);
					reanimation4.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_boss_outerarm_thumb2, AtlasResources.IMAGE_REANIM_ZOMBIE_BOSS_OUTERARM_THUMB_DAMAGE2);
					reanimation4.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_boss_outerleg_foot, AtlasResources.IMAGE_REANIM_ZOMBIE_BOSS_FOOT_DAMAGE2);
					ApplyBossSmokeParticles(true);
				}
				if (num >= mBodyMaxHealth / GameConstants.BOSS_FLASH_HEALTH_FRACTION && mBodyHealth < mBodyMaxHealth / GameConstants.BOSS_FLASH_HEALTH_FRACTION)
				{
					mApp.AddTodParticle(770f, 260f, 400000, ParticleEffect.PARTICLE_BOSS_EXPLOSION);
					mApp.PlayFoley(FoleyType.FOLEY_BOSS_EXPLOSION_SMALL);
					ApplyBossSmokeParticles(true);
				}
				if (mBodyHealth <= 0)
				{
					mBodyHealth = 1;
				}
			}
			else
			{
				UpdateDamageStates(theDamageFlags);
			}
			if (mBodyHealth <= 0)
			{
				mBodyHealth = 0;
				PlayDeathAnim(theDamageFlags);
				DropLoot();
			}
		}

		public void AttachShield()
		{
			Reanimation reanimation = mApp.ReanimationGet(mBodyReanimID);
			string theTrackName = string.Empty;
			if (mShieldType == ShieldType.SHIELDTYPE_DOOR)
			{
				ShowDoorArms(true);
				ReanimShowPrefix("Zombie_outerarm_screendoor", GameConstants.RENDER_GROUP_OVER_SHIELD);
				theTrackName = GlobalMembersReanimIds.ReanimTrackId_anim_screendoor;
			}
			else if (mShieldType == ShieldType.SHIELDTYPE_NEWSPAPER)
			{
				ReanimShowPrefix("Zombie_paper_hands", GameConstants.RENDER_GROUP_OVER_SHIELD);
				theTrackName = GlobalMembersReanimIds.ReanimTrackId_zombie_paper_paper;
			}
			else if (mShieldType == ShieldType.SHIELDTYPE_LADDER)
			{
				ReanimShowPrefix("Zombie_outerarm", GameConstants.RENDER_GROUP_OVER_SHIELD);
				theTrackName = GlobalMembersReanimIds.ReanimTrackId_zombie_ladder_1;
			}
			else
			{
				Debug.ASSERT(false);
			}
			reanimation.AssignRenderGroupToTrack(theTrackName, GameConstants.RENDER_GROUP_SHIELD);
		}

		public void DetachShield()
		{
			Reanimation reanimation = mApp.ReanimationTryToGet(mBodyReanimID);
			if (reanimation == null)
			{
				mShieldType = ShieldType.SHIELDTYPE_NONE;
				mShieldHealth = 0;
				return;
			}
			if (mShieldType == ShieldType.SHIELDTYPE_DOOR)
			{
				ShowDoorArms(false);
			}
			else if (mShieldType == ShieldType.SHIELDTYPE_NEWSPAPER)
			{
				ReanimShowPrefix("Zombie_paper_hands", 0);
			}
			else if (mShieldType == ShieldType.SHIELDTYPE_LADDER)
			{
				ReanimShowPrefix("Zombie_outerarm", 0);
				mZombiePhase = ZombiePhase.PHASE_ZOMBIE_NORMAL;
				if (mIsEating)
				{
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_eat, ReanimLoopType.REANIM_LOOP, 20, 0f);
				}
				else
				{
					StartWalkAnim(0);
				}
			}
			else
			{
				Debug.ASSERT(false);
			}
			mShieldType = ShieldType.SHIELDTYPE_NONE;
			mShieldHealth = 0;
			mHasShield = false;
		}

		public void UpdateReanim()
		{
			Reanimation reanimation = mApp.ReanimationTryToGet(mBodyReanimID);
			if (reanimation == null || reanimation.mDead)
			{
				return;
			}
			bool flag = false;
			if (mZombieType == ZombieType.ZOMBIE_CATAPULT)
			{
				flag = true;
				int bodyDamageIndex = GetBodyDamageIndex();
				bool flag2 = mSummonCounter == 0;
				if (bodyDamageIndex == 2 || mZombiePhase == ZombiePhase.PHASE_ZOMBIE_DYING)
				{
					reanimation = mApp.ReanimationGet(mBodyReanimID);
					Image currentTrackImage = reanimation.GetCurrentTrackImage(GlobalMembersReanimIds.ReanimTrackId_zombie_catapult_pole);
					if (currentTrackImage == AtlasResources.IMAGE_REANIM_ZOMBIE_CATAPULT_POLE_WITHBALL && !flag2)
					{
						reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_catapult_pole, AtlasResources.IMAGE_REANIM_ZOMBIE_CATAPULT_POLE_DAMAGE_WITHBALL);
					}
					else
					{
						reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_catapult_pole, AtlasResources.IMAGE_REANIM_ZOMBIE_CATAPULT_POLE_DAMAGE);
					}
				}
				else if (flag2)
				{
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_catapult_pole, AtlasResources.IMAGE_REANIM_ZOMBIE_CATAPULT_POLE);
				}
			}
			ZombieDrawPosition theDrawPos = default(ZombieDrawPosition);
			GetDrawPos(ref theDrawPos);
			float mImageOffsetX = theDrawPos.mImageOffsetX;
			float num = theDrawPos.mImageOffsetY + theDrawPos.mBodyY - 28f;
			mImageOffsetX += 15f;
			num += 20f;
			if ((mZombieType == ZombieType.ZOMBIE_ZAMBONI || mZombieType == ZombieType.ZOMBIE_CATAPULT) && mZombiePhase != ZombiePhase.PHASE_ZOMBIE_BURNED)
			{
				if (mZombiePhase == ZombiePhase.PHASE_ZOMBIE_DYING)
				{
					float num2 = TodCommon.TodAnimateCurveFloatTime(0.7f, 1f, reanimation.mAnimTime, 0f, 1f, TodCurves.CURVE_EASE_OUT);
					mImageOffsetX += TodCommon.RandRangeFloat(0f - num2, num2);
					num += TodCommon.RandRangeFloat(0f - num2, num2);
				}
				else if (mBodyHealth < 200)
				{
					mImageOffsetX += TodCommon.RandRangeFloat(-1f, 1f);
					num += TodCommon.RandRangeFloat(-1f, 1f);
				}
			}
			if (mZombieType == ZombieType.ZOMBIE_FOOTBALL && mScaleZombie < 1f)
			{
				num += 20f - mScaleZombie * 20f;
			}
			bool flag3 = false;
			if (IsWalkingBackwards())
			{
				flag3 = true;
			}
			if (mZombieType == ZombieType.ZOMBIE_DANCER || mZombieType == ZombieType.ZOMBIE_BACKUP_DANCER)
			{
				bool flag4 = false;
				if ((mZombiePhase == ZombiePhase.PHASE_DANCER_DANCING_IN || mZombiePhase == ZombiePhase.PHASE_DANCER_RAISE_RIGHT_1 || mZombiePhase == ZombiePhase.PHASE_DANCER_RAISE_RIGHT_2) && !mIsEating)
				{
					flag4 = true;
				}
				if (mMindControlled)
				{
					flag4 = !flag4;
				}
				flag3 = flag4;
			}
			if (flag3)
			{
				mImageOffsetX += 90f * mScaleZombie;
			}
			Matrix identity = Matrix.Identity;
			identity.M11 = mScaleZombie;
			identity.M22 = mScaleZombie;
			identity.M41 = (mImageOffsetX + 30f - mScaleZombie * 30f) * Constants.S;
			identity.M42 = (num + 120f - mScaleZombie * 120f) * Constants.S;
			if (flag3)
			{
				identity.M11 = 0f - mScaleZombie;
			}
			if (reanimation.mOverlayMatrix.mMatrix != identity)
			{
				flag = true;
				reanimation.mOverlayMatrix.mMatrix = identity;
			}
			Reanimation reanimation2 = mApp.ReanimationTryToGet(mMoweredReanimID);
			if (reanimation2 != null)
			{
				reanimation2.Update();
				SexyTransform2D theOverlayMatrix = default(SexyTransform2D);
				reanimation2.GetAttachmentOverlayMatrix(0, out theOverlayMatrix);
				theOverlayMatrix.mMatrix.M11 *= reanimation.mOverlayMatrix.mMatrix.M11;
				theOverlayMatrix.mMatrix.M21 *= reanimation.mOverlayMatrix.mMatrix.M11;
				theOverlayMatrix.mMatrix.M12 *= reanimation.mOverlayMatrix.mMatrix.M22;
				theOverlayMatrix.mMatrix.M22 *= reanimation.mOverlayMatrix.mMatrix.M22;
				theOverlayMatrix.mMatrix.M41 *= reanimation.mOverlayMatrix.mMatrix.M11;
				theOverlayMatrix.mMatrix.M42 *= reanimation.mOverlayMatrix.mMatrix.M22;
				theOverlayMatrix.mMatrix.M41 += reanimation.mOverlayMatrix.mMatrix.M22;
				theOverlayMatrix.mMatrix.M42 += reanimation.mOverlayMatrix.mMatrix.M42;
				if (reanimation.mOverlayMatrix != theOverlayMatrix)
				{
					flag = true;
					reanimation.mOverlayMatrix = theOverlayMatrix;
				}
			}
			reanimation.Update();
			if (flag)
			{
				reanimation.PropogateColorToAttachments();
			}
		}

		public void GetTrackPosition(ref string theTrackName, ref float thePosX, ref float thePosY)
		{
			Reanimation reanimation = mApp.ReanimationTryToGet(mBodyReanimID);
			if (reanimation == null)
			{
				thePosX = mPosX;
				thePosY = mPosY;
				return;
			}
			int theTrackIndex = reanimation.FindTrackIndex(theTrackName);
			SexyTransform2D theMatrix = default(SexyTransform2D);
			reanimation.GetTrackTranslationMatrix(theTrackIndex, ref theMatrix);
			thePosX = theMatrix.mMatrix.M41 * Constants.IS + mPosX;
			thePosY = theMatrix.mMatrix.M42 * Constants.IS + mPosY;
		}

		public void LoadPlainZombieReanim()
		{
			mZombieAttackRect = new TRect(20, 0, 50, 115);
			Reanimation reanimation = mApp.ReanimationTryToGet(mBodyReanimID);
			if (reanimation != null)
			{
				SetupReanimLayers(reanimation, mZombieType);
				if (mBoard != null)
				{
					EnableMustache(mBoard.mMustacheMode);
					EnableFuture(mBoard.mFutureMode);
				}
				bool flag = false;
				if (mBoard != null && mBoard.mPlantRow[mRow] == PlantRowType.PLANTROW_POOL)
				{
					flag = true;
				}
				else if (mZombieType == ZombieType.ZOMBIE_DUCKY_TUBE)
				{
					flag = true;
				}
				if (flag)
				{
					ReanimShowPrefix("zombie_duckytube", 0);
					ReanimIgnoreClipRect(GlobalMembersReanimIds.ReanimTrackId_zombie_duckytube, true);
					ReanimIgnoreClipRect(GlobalMembersReanimIds.ReanimTrackId_zombie_outerarm_hand, true);
					ReanimIgnoreClipRect(GlobalMembersReanimIds.ReanimTrackId_zombie_innerarm3, true);
					SetupWaterTrack(ref GlobalMembersReanimIds.ReanimTrackId_zombie_whitewater);
					SetupWaterTrack(ref GlobalMembersReanimIds.ReanimTrackId_zombie_whitewater2);
				}
			}
		}

		public void ShowDoorArms(bool theShow)
		{
			Reanimation reanimation = mApp.ReanimationTryToGet(mBodyReanimID);
			if (reanimation != null)
			{
				SetupDoorArms(reanimation, theShow);
				if (!mHasArm)
				{
					ReanimShowPrefix("Zombie_outerarm_lower", -1);
					ReanimShowPrefix("Zombie_outerarm_hand", -1);
				}
			}
		}

		public void ReanimShowTrack(ref string theTrackName, int theRenderGroup)
		{
			Reanimation reanimation = mApp.ReanimationTryToGet(mBodyReanimID);
			if (reanimation != null)
			{
				reanimation.AssignRenderGroupToTrack(theTrackName, theRenderGroup);
			}
		}

		public void PlayZombieAppearSound()
		{
			if (mZombieType == ZombieType.ZOMBIE_DOLPHIN_RIDER)
			{
				mApp.PlayFoley(FoleyType.FOLEY_DOLPHIN_APPEARS);
			}
			else if (mZombieType == ZombieType.ZOMBIE_BALLOON)
			{
				mApp.PlayFoley(FoleyType.FOLEY_BALLOONINFLATE);
			}
			else if (mZombieType == ZombieType.ZOMBIE_ZAMBONI)
			{
				mApp.PlayFoley(FoleyType.FOLEY_ZAMBONI);
			}
		}

		public void StartMindControlled()
		{
			mApp.PlaySample(Resources.SOUND_MINDCONTROLLED);
			mMindControlled = true;
			mLastPortalX = -1;
			if (mZombieType == ZombieType.ZOMBIE_DANCER)
			{
				for (int i = 0; i < GameConstants.NUM_BACKUP_DANCERS; i++)
				{
					mFollowerZombieID[i] = null;
				}
			}
			else if (mZombieType == ZombieType.ZOMBIE_BACKUP_DANCER)
			{
				Zombie zombie = mBoard.ZombieTryToGet(mRelatedZombieID);
				if (zombie != null)
				{
					Zombie zombie2 = mBoard.ZombieGetID(this);
					for (int j = 0; j < GameConstants.NUM_BACKUP_DANCERS; j++)
					{
						if (zombie.mFollowerZombieID[j] == zombie2)
						{
							zombie.mFollowerZombieID[j] = null;
							break;
						}
					}
				}
				mRelatedZombieID = null;
			}
			else
			{
				Zombie zombie3 = mBoard.ZombieTryToGet(mRelatedZombieID);
				if (zombie3 != null)
				{
					zombie3.mRelatedZombieID = null;
					mRelatedZombieID = null;
				}
			}
		}

		public bool IsFlying()
		{
			if (mZombiePhase == ZombiePhase.PHASE_BALLOON_FLYING || mZombiePhase == ZombiePhase.PHASE_BALLOON_POPPING)
			{
				return true;
			}
			return false;
		}

		private void SetupReanimForLostHead()
		{
			ReanimShowPrefix("anim_head", -1);
			ReanimShowPrefix("anim_hair", -1);
			ReanimShowPrefix("anim_tongue", -1);
		}

		public void DropHead(uint theDamageFlags)
		{
			if (!CanLoseBodyParts() || !mHasHead)
			{
				return;
			}
			if (mButteredCounter > 0)
			{
				mButteredCounter = 0;
				UpdateAnimSpeed();
			}
			mHasHead = false;
			SetupReanimForLostHead();
			if (TodCommon.TestBit(theDamageFlags, 4))
			{
				return;
			}
			if (mZombieType == ZombieType.ZOMBIE_PEA_HEAD || mZombieType == ZombieType.ZOMBIE_WALLNUT_HEAD || mZombieType == ZombieType.ZOMBIE_TALLNUT_HEAD || mZombieType == ZombieType.ZOMBIE_JALAPENO_HEAD || mZombieType == ZombieType.ZOMBIE_GATLING_HEAD || mZombieType == ZombieType.ZOMBIE_SQUASH_HEAD)
			{
				DetachPlantHead();
				Reanimation reanimation = mApp.ReanimationGet(mSpecialHeadReanimID);
				if (reanimation != null)
				{
					reanimation.ReanimationDie();
				}
				mSpecialHeadReanimID = null;
				return;
			}
			int aRenderOrder = mRenderOrder + 1;
			ZombieDrawPosition theDrawPos = default(ZombieDrawPosition);
			GetDrawPos(ref theDrawPos);
			float thePosX = mPosX + theDrawPos.mImageOffsetX + (float)theDrawPos.mHeadX + 11f;
			float thePosY = mPosY + theDrawPos.mImageOffsetY + (float)theDrawPos.mHeadY + theDrawPos.mBodyY + 21f;
			if (mBodyReanimID != null)
			{
				GetTrackPosition(ref GlobalMembersReanimIds.ReanimTrackId_anim_head1, ref thePosX, ref thePosY);
			}
			ParticleEffect theEffect = ParticleEffect.PARTICLE_ZOMBIE_HEAD;
			if (mZombiePhase == ZombiePhase.PHASE_ZOMBIE_MOWERED)
			{
				theEffect = ParticleEffect.PARTICLE_MOWERED_ZOMBIE_HEAD;
				thePosX -= 40f;
				thePosY -= 50f;
			}
			else if (mInPool)
			{
				theEffect = ParticleEffect.PARTICLE_ZOMBIE_HEAD_POOL;
			}
			if (mZombieType == ZombieType.ZOMBIE_DANCER)
			{
				aRenderOrder = mRenderOrder - 1;
			}
			if (mZombieType == ZombieType.ZOMBIE_NEWSPAPER)
			{
				theEffect = ParticleEffect.PARTICLE_ZOMBIE_NEWSPAPER_HEAD;
			}
			else if (mZombieType == ZombieType.ZOMBIE_POGO)
			{
				PogoBreak(theDamageFlags);
				theEffect = ParticleEffect.PARTICLE_ZOMBIE_POGO_HEAD;
			}
			else if (mZombieType == ZombieType.ZOMBIE_BALLOON)
			{
				ReanimShowPrefix("anim_hat", -1);
				ReanimShowPrefix("hat", -1);
				theEffect = ParticleEffect.PARTICLE_ZOMBIE_BALLOON_HEAD;
			}
			else if (mZombieType == ZombieType.ZOMBIE_POLEVAULTER)
			{
				DropPole();
			}
			else if (mZombieType == ZombieType.ZOMBIE_FLAG)
			{
				DropFlag();
			}
			TodParticleSystem todParticleSystem = mApp.AddTodParticle(thePosX, thePosY, aRenderOrder, theEffect);
			OverrideParticleColor(todParticleSystem);
			OverrideParticleScale(todParticleSystem);
			if (todParticleSystem != null && mZombieType == ZombieType.ZOMBIE_DANCER)
			{
				todParticleSystem.OverrideImage(null, AtlasResources.IMAGE_REANIM_ZOMBIE_HEAD_DISCO);
				todParticleSystem.OverrideScale(null, 1.2f);
				ReanimShowPrefix("Zombie_disco_glasses", -1);
				TodParticleSystem todParticleSystem2 = mApp.AddTodParticle(thePosX, thePosY, aRenderOrder, ParticleEffect.PARTICLE_ZOMBIE_MUSTACHE);
				OverrideParticleColor(todParticleSystem2);
				OverrideParticleScale(todParticleSystem2);
				if (todParticleSystem2 != null)
				{
					todParticleSystem2.OverrideImage(null, AtlasResources.IMAGE_REANIM_ZOMBIE_DISCO_GLASSES);
				}
			}
			else if (todParticleSystem != null && mZombieType == ZombieType.ZOMBIE_BACKUP_DANCER)
			{
				todParticleSystem.OverrideImage(null, AtlasResources.IMAGE_REANIM_ZOMBIE_DANCER_BACKUP_HEAD);
				todParticleSystem.OverrideScale(null, 1.2f);
				ReanimShowPrefix("Zombie_backup_stash", -1);
				todParticleSystem = mApp.AddTodParticle(thePosX, thePosY, aRenderOrder, ParticleEffect.PARTICLE_ZOMBIE_MUSTACHE);
				OverrideParticleColor(todParticleSystem);
				OverrideParticleScale(todParticleSystem);
				if (todParticleSystem != null)
				{
					todParticleSystem.OverrideImage(null, AtlasResources.IMAGE_REANIM_ZOMBIE_BACKUP_STASH);
				}
				ReanimShowPrefix("anim_head2", -1);
				todParticleSystem = mApp.AddTodParticle(thePosX, thePosY, aRenderOrder, ParticleEffect.PARTICLE_ZOMBIE_MUSTACHE);
				OverrideParticleColor(todParticleSystem);
				OverrideParticleScale(todParticleSystem);
				if (todParticleSystem != null)
				{
					todParticleSystem.OverrideImage(null, AtlasResources.IMAGE_REANIM_ZOMBIE_JAW);
				}
			}
			else if (todParticleSystem != null && mZombieType == ZombieType.ZOMBIE_BOBSLED)
			{
				todParticleSystem.OverrideImage(null, AtlasResources.IMAGE_ZOMBIEBOBSLEDHEAD);
			}
			else if (todParticleSystem != null && mZombieType == ZombieType.ZOMBIE_LADDER)
			{
				todParticleSystem.OverrideImage(null, AtlasResources.IMAGE_ZOMBIELADDERHEAD);
			}
			else if (todParticleSystem != null && mZombieType == ZombieType.ZOMBIE_IMP)
			{
				todParticleSystem.OverrideImage(null, AtlasResources.IMAGE_ZOMBIEIMPHEAD);
			}
			else if (todParticleSystem != null && mZombieType == ZombieType.ZOMBIE_FOOTBALL)
			{
				todParticleSystem.OverrideImage(null, AtlasResources.IMAGE_ZOMBIEFOOTBALLHEAD);
			}
			else if (todParticleSystem != null && mZombieType == ZombieType.ZOMBIE_POLEVAULTER)
			{
				todParticleSystem.OverrideImage(null, AtlasResources.IMAGE_ZOMBIEPOLEVAULTERHEAD);
			}
			else if (todParticleSystem != null && mZombieType == ZombieType.ZOMBIE_SNORKEL)
			{
				todParticleSystem.OverrideImage(null, AtlasResources.IMAGE_REANIM_ZOMBIE_SNORKLE_HEAD);
			}
			else if (todParticleSystem != null && mZombieType == ZombieType.ZOMBIE_DIGGER)
			{
				todParticleSystem.OverrideImage(null, AtlasResources.IMAGE_ZOMBIEDIGGERHEAD);
			}
			else if (todParticleSystem != null && mZombieType == ZombieType.ZOMBIE_DOLPHIN_RIDER)
			{
				todParticleSystem.OverrideImage(null, AtlasResources.IMAGE_ZOMBIEDOLPHINRIDERHEAD);
			}
			else if (todParticleSystem != null && mZombieType == ZombieType.ZOMBIE_YETI)
			{
				todParticleSystem.OverrideImage(null, AtlasResources.IMAGE_ZOMBIEYETIHEAD);
			}
			Reanimation reanimation2 = mApp.ReanimationTryToGet(mBodyReanimID);
			if (mBoard.mMustacheMode && reanimation2.TrackExists(GlobalMembersReanimIds.ReanimTrackId_zombie_mustache))
			{
				ReanimShowPrefix("Zombie_mustache", -1);
				TodParticleSystem todParticleSystem3 = mApp.AddTodParticle(thePosX, thePosY, aRenderOrder, ParticleEffect.PARTICLE_ZOMBIE_MUSTACHE);
				OverrideParticleColor(todParticleSystem3);
				OverrideParticleScale(todParticleSystem3);
				Image imageOverride = reanimation2.GetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_mustache);
				if (todParticleSystem3 != null && imageOverride != null)
				{
					todParticleSystem3.OverrideImage(null, imageOverride);
				}
			}
			if (mBoard.mFutureMode)
			{
				Image imageOverride2 = reanimation2.GetImageOverride(GlobalMembersReanimIds.ReanimTrackId_anim_head1);
				int num = -1;
				if (imageOverride2 != null && imageOverride2 == AtlasResources.IMAGE_REANIM_ZOMBIE_HEAD_SUNGLASSES1)
				{
					num = 0;
				}
				else if (imageOverride2 != null && imageOverride2 == AtlasResources.IMAGE_REANIM_ZOMBIE_HEAD_SUNGLASSES2)
				{
					num = 1;
				}
				else if (imageOverride2 != null && imageOverride2 == AtlasResources.IMAGE_REANIM_ZOMBIE_HEAD_SUNGLASSES3)
				{
					num = 2;
				}
				else if (imageOverride2 != null && imageOverride2 == AtlasResources.IMAGE_REANIM_ZOMBIE_HEAD_SUNGLASSES4)
				{
					num = 3;
				}
				if (num != -1)
				{
					TodParticleSystem todParticleSystem4 = mApp.AddTodParticle(thePosX, thePosY, aRenderOrder, ParticleEffect.PARTICLE_ZOMBIE_FUTURE_GLASSES);
					OverrideParticleColor(todParticleSystem4);
					OverrideParticleScale(todParticleSystem4);
					if (todParticleSystem4 != null)
					{
						todParticleSystem4.OverrideFrame(null, num);
					}
				}
			}
			if (mBoard.mPinataMode && mZombiePhase != ZombiePhase.PHASE_ZOMBIE_MOWERED)
			{
				TodParticleSystem aParticle = mApp.AddTodParticle(thePosX, thePosY, aRenderOrder, ParticleEffect.PARTICLE_PINATA);
				OverrideParticleScale(aParticle);
			}
			mApp.PlayFoley(FoleyType.FOLEY_LIMBS_POP);
		}

		public bool CanTargetPlant(Plant thePlant, ZombieAttackType theAttackType)
		{
			if (mApp.IsWallnutBowlingLevel() && theAttackType != ZombieAttackType.ATTACKTYPE_VAULT)
			{
				return false;
			}
			if (thePlant.NotOnGround())
			{
				return false;
			}
			if (thePlant.mSeedType == SeedType.SEED_TANGLEKELP)
			{
				return false;
			}
			if (!mInPool && mBoard.IsPoolSquare(thePlant.mPlantCol, thePlant.mRow))
			{
				return false;
			}
			if (mZombiePhase == ZombiePhase.PHASE_DIGGER_TUNNELING)
			{
				if (thePlant.mSeedType == SeedType.SEED_POTATOMINE && thePlant.mState == PlantState.STATE_NOTREADY)
				{
					return true;
				}
				return false;
			}
			if (thePlant.IsSpiky())
			{
				if (mZombieType == ZombieType.ZOMBIE_GARGANTUAR || mZombieType == ZombieType.ZOMBIE_REDEYE_GARGANTUAR || mZombieType == ZombieType.ZOMBIE_ZAMBONI)
				{
					return true;
				}
				if (mBoard.IsPoolSquare(thePlant.mPlantCol, thePlant.mRow) || mBoard.GetFlowerPotAt(thePlant.mPlantCol, thePlant.mRow) != null)
				{
					return true;
				}
				return false;
			}
			if (theAttackType == ZombieAttackType.ATTACKTYPE_DRIVE_OVER)
			{
				if (thePlant.mSeedType == SeedType.SEED_CHERRYBOMB || thePlant.mSeedType == SeedType.SEED_JALAPENO || thePlant.mSeedType == SeedType.SEED_BLOVER || thePlant.mSeedType == SeedType.SEED_SQUASH)
				{
					return false;
				}
				if (thePlant.mSeedType == SeedType.SEED_DOOMSHROOM || thePlant.mSeedType == SeedType.SEED_ICESHROOM)
				{
					if (thePlant.mIsAsleep)
					{
						return true;
					}
					return false;
				}
			}
			if (mZombiePhase == ZombiePhase.PHASE_LADDER_CARRYING || mZombiePhase == ZombiePhase.PHASE_LADDER_PLACING)
			{
				bool flag = false;
				if (thePlant.mSeedType == SeedType.SEED_WALLNUT || thePlant.mSeedType == SeedType.SEED_TALLNUT || thePlant.mSeedType == SeedType.SEED_PUMPKINSHELL)
				{
					flag = true;
				}
				if (mBoard.GetLadderAt(thePlant.mPlantCol, thePlant.mRow) != null)
				{
					flag = false;
				}
				if (theAttackType == ZombieAttackType.ATTACKTYPE_CHEW && flag)
				{
					return false;
				}
				if (theAttackType == ZombieAttackType.ATTACKTYPE_LADDER && !flag)
				{
					return false;
				}
			}
			if (theAttackType == ZombieAttackType.ATTACKTYPE_CHEW)
			{
				Plant topPlantAt = mBoard.GetTopPlantAt(thePlant.mPlantCol, thePlant.mRow, PlantPriority.TOPPLANT_EATING_ORDER);
				if (topPlantAt != thePlant && topPlantAt != null && CanTargetPlant(topPlantAt, theAttackType))
				{
					return false;
				}
			}
			if (theAttackType == ZombieAttackType.ATTACKTYPE_VAULT)
			{
				Plant topPlantAt2 = mBoard.GetTopPlantAt(thePlant.mPlantCol, thePlant.mRow, PlantPriority.TOPPLANT_ONLY_NORMAL_POSITION);
				if (topPlantAt2 != thePlant && topPlantAt2 != null && CanTargetPlant(topPlantAt2, theAttackType))
				{
					return false;
				}
			}
			return true;
		}

		public void UpdateZombieCatapult()
		{
			if (mZombiePhase == ZombiePhase.PHASE_ZOMBIE_NORMAL)
			{
				if (mPosX <= (float)(650 + Constants.BOARD_EXTRA_ROOM) && FindCatapultTarget() != null && mSummonCounter > 0)
				{
					mZombiePhase = ZombiePhase.PHASE_CATAPULT_LAUNCHING;
					mPhaseCounter = 300;
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_shoot, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 0, 24f);
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_CATAPULT_LAUNCHING)
			{
				Reanimation reanimation = mApp.ReanimationGet(mBodyReanimID);
				if (reanimation.ShouldTriggerTimedEvent(0.545f))
				{
					Plant thePlant = FindCatapultTarget();
					ZombieCatapultFire(thePlant);
				}
				if (reanimation.mLoopCount > 0)
				{
					mSummonCounter--;
					if (mSummonCounter == 4)
					{
						ReanimShowTrack(ref GlobalMembersReanimIds.ReanimTrackId_zombie_catapult_basketball, -1);
					}
					else if (mSummonCounter == 3)
					{
						ReanimShowTrack(ref GlobalMembersReanimIds.ReanimTrackId_zombie_catapult_basketball2, -1);
					}
					else if (mSummonCounter == 2)
					{
						ReanimShowTrack(ref GlobalMembersReanimIds.ReanimTrackId_zombie_catapult_basketball3, -1);
					}
					else if (mSummonCounter == 1)
					{
						ReanimShowTrack(ref GlobalMembersReanimIds.ReanimTrackId_zombie_catapult_basketball4, -1);
					}
					if (mSummonCounter == 0)
					{
						PlayZombieReanim(ref Reanimation.ReanimTrackId_anim_walk, ReanimLoopType.REANIM_LOOP, 20, 6f);
						mZombiePhase = ZombiePhase.PHASE_ZOMBIE_NORMAL;
					}
					else
					{
						PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_idle, ReanimLoopType.REANIM_LOOP, 20, 12f);
						mZombiePhase = ZombiePhase.PHASE_CATAPULT_RELOADING;
					}
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_CATAPULT_RELOADING && mPhaseCounter == 0)
			{
				Plant plant = FindCatapultTarget();
				if (plant != null)
				{
					mZombiePhase = ZombiePhase.PHASE_CATAPULT_LAUNCHING;
					mPhaseCounter = 300;
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_shoot, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 20, 24f);
				}
				else
				{
					PlayZombieReanim(ref Reanimation.ReanimTrackId_anim_walk, ReanimLoopType.REANIM_LOOP, 20, 6f);
					mZombiePhase = ZombiePhase.PHASE_ZOMBIE_NORMAL;
				}
			}
		}

		public Plant FindCatapultTarget()
		{
			Plant plant = null;
			int count = mBoard.mPlants.Count;
			for (int i = 0; i < count; i++)
			{
				Plant plant2 = mBoard.mPlants[i];
				if (!plant2.mDead && mRow == plant2.mRow && mX >= plant2.mX + 100 && !plant2.NotOnGround() && !plant2.IsSpiky() && (plant == null || plant2.mPlantCol < plant.mPlantCol))
				{
					plant = mBoard.GetTopPlantAt(plant2.mPlantCol, plant2.mRow, PlantPriority.TOPPLANT_CATAPULT_ORDER);
				}
			}
			return plant;
		}

		public void ZombieCatapultFire(Plant thePlant)
		{
			float num = mPosX + 113f;
			float num2 = mPosY - 44f;
			int num3;
			int num4;
			if (thePlant != null)
			{
				num3 = thePlant.mX;
				num4 = thePlant.mY;
			}
			else
			{
				num3 = (int)mPosX - 300;
				num4 = 0;
			}
			mApp.PlayFoley(FoleyType.FOLEY_BASKETBALL);
			Projectile projectile = mBoard.AddProjectile((int)num, (int)num2, mRenderOrder, mRow, ProjectileType.PROJECTILE_BASKETBALL);
			float num5 = num - (float)num3 - 20f;
			float num6 = (float)num4 - num2;
			if (num5 < 40f)
			{
				num5 = 40f;
			}
			projectile.mMotionType = ProjectileMotion.MOTION_LOBBED;
			float num7 = 120f;
			projectile.mVelX = (0f - num5) / num7;
			projectile.mVelY = 0f;
			projectile.mVelZ = -7f + num6 / num7;
			projectile.mAccZ = 0.115f;
		}

		public void UpdateClimbingLadder()
		{
			float num = mAltitude;
			if (mOnHighGround)
			{
				num -= (float)Constants.HIGH_GROUND_HEIGHT;
			}
			int theGridX = mBoard.PixelToGridXKeepOnBoard((int)((float)(mX + 5) + num * 0.5f), mY);
			GridItem ladderAt = mBoard.GetLadderAt(theGridX, mRow);
			if (ladderAt == null)
			{
				mZombieHeight = ZombieHeight.HEIGHT_FALLING;
				return;
			}
			mAltitude += 0.8f;
			if (mVelX < 0.5f)
			{
				mPosX -= 0.5f;
			}
			float num2 = 90f;
			if (mOnHighGround)
			{
				num2 += (float)Constants.HIGH_GROUND_HEIGHT;
			}
			if (mAltitude >= num2)
			{
				mZombieHeight = ZombieHeight.HEIGHT_FALLING;
			}
		}

		public void UpdateZombieGargantuar()
		{
			if (mZombiePhase == ZombiePhase.PHASE_GARGANTUAR_SMASHING)
			{
				Reanimation reanimation = mApp.ReanimationGet(mBodyReanimID);
				if (reanimation.ShouldTriggerTimedEvent(0.64f))
				{
					Plant plant = FindPlantTarget(ZombieAttackType.ATTACKTYPE_CHEW);
					if (plant != null && plant.mSeedType == SeedType.SEED_SPIKEROCK)
					{
						TakeDamage(20, 32u);
						plant.SpikeRockTakeDamage();
						if (plant.mPlantHealth <= 0)
						{
							SquishAllInSquare(plant.mPlantCol, plant.mRow, ZombieAttackType.ATTACKTYPE_CHEW);
						}
					}
					else if (plant != null)
					{
						SquishAllInSquare(plant.mPlantCol, plant.mRow, ZombieAttackType.ATTACKTYPE_CHEW);
					}
					if (mApp.IsScaryPotterLevel())
					{
						int theGridX = mBoard.PixelToGridX((int)mPosX, (int)mPosY);
						GridItem scaryPotAt = mBoard.GetScaryPotAt(theGridX, mRow);
						if (scaryPotAt != null)
						{
							mBoard.mChallenge.ScaryPotterOpenPot(scaryPotAt);
						}
					}
					if (mApp.IsIZombieLevel())
					{
						GridItem gridItem = mBoard.mChallenge.IZombieGetBrainTarget(this);
						if (gridItem != null)
						{
							mBoard.mChallenge.IZombieSquishBrain(gridItem);
						}
					}
					mApp.PlayFoley(FoleyType.FOLEY_THUMP);
					mApp.Vibrate();
					mBoard.ShakeBoard(0, 3);
				}
				if (reanimation.mLoopCount > 0)
				{
					mZombiePhase = ZombiePhase.PHASE_ZOMBIE_NORMAL;
					StartWalkAnim(20);
				}
				return;
			}
			float num = mPosX - 460f;
			if (mZombiePhase == ZombiePhase.PHASE_GARGANTUAR_THROWING)
			{
				Reanimation reanimation2 = mApp.ReanimationGet(mBodyReanimID);
				if (reanimation2.ShouldTriggerTimedEvent(0.74f))
				{
					mHasObject = false;
					ReanimShowPrefix("Zombie_imp", -1);
					ReanimShowTrack(ref GlobalMembersReanimIds.ReanimTrackId_zombie_gargantuar_whiterope, -1);
					mApp.PlayFoley(FoleyType.FOLEY_SWING);
					Zombie zombie = mBoard.AddZombie(ZombieType.ZOMBIE_IMP, mFromWave);
					if (zombie == null)
					{
						return;
					}
					float num2 = 40f;
					if (mBoard.StageHasRoof())
					{
						num -= 180f;
						num2 = -140f;
					}
					if (num < num2)
					{
						num = num2;
					}
					else if (num > 140f)
					{
						num -= TodCommon.RandRangeFloat(0f, 100f);
					}
					zombie.mPosX = mPosX - 133f;
					zombie.mPosY = GetPosYBasedOnRow(mRow);
					zombie.SetRow(mRow);
					zombie.mVariant = false;
					zombie.mRenderOrder = mRenderOrder + 1;
					zombie.mZombiePhase = ZombiePhase.PHASE_IMP_GETTING_THROWN;
					zombie.mAltitude = 88f;
					zombie.mVelX = 3f;
					zombie.mChilledCounter = mChilledCounter;
					float num3 = num / zombie.mVelX;
					zombie.mVelZ = 0.5f * num3 * GameConstants.THOWN_ZOMBIE_GRAVITY;
					zombie.PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_thrown, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 0, 18f);
					zombie.UpdateReanim();
					mApp.PlayFoley(FoleyType.FOLEY_IMP);
				}
				if (reanimation2.mLoopCount > 0)
				{
					mZombiePhase = ZombiePhase.PHASE_ZOMBIE_NORMAL;
					StartWalkAnim(20);
				}
			}
			else
			{
				if (IsImmobilizied() || !mHasHead)
				{
					return;
				}
				if (mHasObject && mBodyHealth < mBodyMaxHealth / 2 && num > 40f)
				{
					mZombiePhase = ZombiePhase.PHASE_GARGANTUAR_THROWING;
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_throw, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 20, 24f);
					return;
				}
				bool flag = false;
				Plant plant = FindPlantTarget(ZombieAttackType.ATTACKTYPE_CHEW);
				if (plant != null)
				{
					flag = true;
				}
				else if (mApp.IsScaryPotterLevel())
				{
					int theGridX2 = mBoard.PixelToGridX((int)mPosX, (int)mPosY);
					if (mBoard.GetScaryPotAt(theGridX2, mRow) != null)
					{
						flag = true;
					}
				}
				else if (mApp.IsIZombieLevel() && mBoard.mChallenge.IZombieGetBrainTarget(this) != null)
				{
					flag = true;
				}
				if (flag)
				{
					mZombiePhase = ZombiePhase.PHASE_GARGANTUAR_SMASHING;
					mApp.PlayFoley(FoleyType.FOLEY_LOWGROAN);
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_smash, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 20, 16f);
				}
			}
		}

		public int GetBodyDamageIndex()
		{
			if (mZombieType == ZombieType.ZOMBIE_BOSS)
			{
				if (mBodyHealth < mBodyMaxHealth / 2)
				{
					return 2;
				}
				if (mBodyHealth < mBodyMaxHealth * 4 / 5)
				{
					return 1;
				}
				return 0;
			}
			if (mBodyHealth < mBodyMaxHealth / 3)
			{
				return 2;
			}
			if (mBodyHealth < mBodyMaxHealth * 2 / 3)
			{
				return 1;
			}
			return 0;
		}

		public void ApplyBurn()
		{
			if (mDead || mZombiePhase == ZombiePhase.PHASE_ZOMBIE_BURNED)
			{
				return;
			}
			if (mBodyHealth >= 1800 || mZombieType == ZombieType.ZOMBIE_BOSS)
			{
				TakeDamage(1800, 18u);
				return;
			}
			if (mZombieType == ZombieType.ZOMBIE_SQUASH_HEAD && !mHasHead)
			{
				mApp.RemoveReanimation(ref mSpecialHeadReanimID);
				mSpecialHeadReanimID = null;
			}
			if (mIceTrapCounter > 0)
			{
				RemoveIceTrap();
			}
			if (mButteredCounter > 0)
			{
				mButteredCounter = 0;
			}
			GlobalMembersAttachment.AttachmentDetachCrossFadeParticleType(ref mAttachmentID, ParticleEffect.PARTICLE_ZAMBONI_SMOKE, null);
			BungeeDropPlant();
			if (mZombiePhase == ZombiePhase.PHASE_ZOMBIE_DYING || mZombiePhase == ZombiePhase.PHASE_POLEVAULTER_IN_VAULT || mZombiePhase == ZombiePhase.PHASE_IMP_GETTING_THROWN || mZombiePhase == ZombiePhase.PHASE_RISING_FROM_GRAVE || mZombiePhase == ZombiePhase.PHASE_DANCER_RISING || mZombiePhase == ZombiePhase.PHASE_DOLPHIN_INTO_POOL || mZombiePhase == ZombiePhase.PHASE_DOLPHIN_IN_JUMP || mZombiePhase == ZombiePhase.PHASE_DOLPHIN_RIDING || mZombiePhase == ZombiePhase.PHASE_SNORKEL_INTO_POOL || mZombiePhase == ZombiePhase.PHASE_DIGGER_TUNNELING || mZombiePhase == ZombiePhase.PHASE_DIGGER_TUNNELING_PAUSE_WITHOUT_AXE || mZombiePhase == ZombiePhase.PHASE_DIGGER_RISING || mZombiePhase == ZombiePhase.PHASE_DIGGER_RISE_WITHOUT_AXE || mZombiePhase == ZombiePhase.PHASE_ZOMBIE_MOWERED || mInPool)
			{
				DieWithLoot();
			}
			else if (mZombieType == ZombieType.ZOMBIE_BUNGEE || mZombieType == ZombieType.ZOMBIE_YETI || mZombieType == ZombieType.ZOMBIE_PEA_HEAD || mZombieType == ZombieType.ZOMBIE_WALLNUT_HEAD || mZombieType == ZombieType.ZOMBIE_JALAPENO_HEAD || mZombieType == ZombieType.ZOMBIE_GATLING_HEAD || mZombieType == ZombieType.ZOMBIE_SQUASH_HEAD || mZombieType == ZombieType.ZOMBIE_TALLNUT_HEAD || IsBobsledTeamWithSled() || IsFlying() || !mHasHead)
			{
				SetAnimRate(0f);
				Reanimation reanimation = mApp.ReanimationTryToGet(mSpecialHeadReanimID);
				if (reanimation != null)
				{
					reanimation.mAnimRate = 0f;
					reanimation.mColorOverride = Color.Black;
					reanimation.mEnableExtraAdditiveDraw = false;
					reanimation.mEnableExtraOverlayDraw = false;
				}
				mZombiePhase = ZombiePhase.PHASE_ZOMBIE_BURNED;
				mPhaseCounter = 300;
				mJustGotShotCounter = 0;
				DropLoot();
				if (mZombieType == ZombieType.ZOMBIE_BUNGEE)
				{
					mZombieFade = 50;
				}
				if (mZombieType == ZombieType.ZOMBIE_BALLOON)
				{
					Reanimation reanimation2 = mApp.ReanimationGet(mBodyReanimID);
					ReanimatorTrackInstance trackInstanceByName = reanimation2.GetTrackInstanceByName(GlobalMembersReanimIds.ReanimTrackId_hat);
					Reanimation reanimation3 = GlobalMembersAttachment.FindReanimAttachment(trackInstanceByName.mAttachmentID);
					if (reanimation3 != null)
					{
						reanimation3.mAnimRate = 0f;
					}
					mZombieFade = 50;
				}
			}
			else
			{
				ReanimationType theReanimationType = ReanimationType.REANIM_ZOMBIE_CHARRED;
				float num = mPosX + 22f;
				float num2 = mPosY - 10f;
				if (mZombieType == ZombieType.ZOMBIE_BALLOON)
				{
					num2 += 31f;
				}
				if (mZombieType == ZombieType.ZOMBIE_IMP)
				{
					num -= 6f;
					theReanimationType = ReanimationType.REANIM_ZOMBIE_CHARRED_IMP;
				}
				if (mZombieType == ZombieType.ZOMBIE_DIGGER)
				{
					if (IsWalkingBackwards())
					{
						num += 14f;
					}
					theReanimationType = ReanimationType.REANIM_ZOMBIE_CHARRED_DIGGER;
				}
				if (mZombieType == ZombieType.ZOMBIE_ZAMBONI)
				{
					num += 61f;
					num2 += -16f;
					theReanimationType = ReanimationType.REANIM_ZOMBIE_CHARRED_ZAMBONI;
				}
				if (mZombieType == ZombieType.ZOMBIE_CATAPULT)
				{
					num += -36f;
					num2 += -20f;
					theReanimationType = ReanimationType.REANIM_ZOMBIE_CHARRED_CATAPULT;
				}
				if (mZombieType == ZombieType.ZOMBIE_GARGANTUAR || mZombieType == ZombieType.ZOMBIE_REDEYE_GARGANTUAR)
				{
					num += -15f;
					num2 += -10f;
					theReanimationType = ReanimationType.REANIM_ZOMBIE_CHARRED_GARGANTUAR;
				}
				Reanimation reanimation4 = mApp.AddReanimation(num, num2, mRenderOrder, theReanimationType);
				reanimation4.mAnimRate *= TodCommon.RandRangeFloat(0.9f, 1.1f);
				if (mZombiePhase == ZombiePhase.PHASE_DIGGER_WALKING_WITHOUT_AXE)
				{
					reanimation4.SetFramesForLayer(GlobalMembersReanimIds.ReanimTrackId_anim_crumble_noaxe);
				}
				else if (mZombieType == ZombieType.ZOMBIE_DIGGER)
				{
					reanimation4.SetFramesForLayer(GlobalMembersReanimIds.ReanimTrackId_anim_crumble);
				}
				else if ((mZombieType == ZombieType.ZOMBIE_GARGANTUAR || mZombieType == ZombieType.ZOMBIE_REDEYE_GARGANTUAR) && !mHasObject)
				{
					reanimation4.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_impblink, AtlasResources.IMAGE_BLANK);
					reanimation4.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_imphead, AtlasResources.IMAGE_BLANK);
				}
				if (mScaleZombie != 1f)
				{
					reanimation4.mOverlayMatrix.mMatrix.M11 = mScaleZombie;
					reanimation4.mOverlayMatrix.mMatrix.M22 = mScaleZombie;
					reanimation4.mOverlayMatrix.mMatrix.M41 += (20f - mScaleZombie * 20f) * Constants.S;
					reanimation4.mOverlayMatrix.mMatrix.M42 += (120f - mScaleZombie * 120f) * Constants.S;
					reanimation4.OverrideScale(mScaleZombie, mScaleZombie);
				}
				if (IsWalkingBackwards())
				{
					reanimation4.OverrideScale(0f - mScaleZombie, mScaleZombie);
					reanimation4.mOverlayMatrix.mMatrix.M41 += 60f * mScaleZombie * Constants.S;
				}
				DieWithLoot();
			}
			if (mZombieType == ZombieType.ZOMBIE_BOBSLED)
			{
				BobsledBurn();
			}
		}

		public void UpdateBurn()
		{
			mPhaseCounter -= 3;
			if (mPhaseCounter == 0)
			{
				DieWithLoot();
			}
		}

		public bool ZombieNotWalking()
		{
			if (mIsEating || IsImmobilizied())
			{
				return true;
			}
			if (mZombiePhase == ZombiePhase.PHASE_JACK_IN_THE_BOX_POPPING || mZombiePhase == ZombiePhase.PHASE_NEWSPAPER_MADDENING || mZombiePhase == ZombiePhase.PHASE_GARGANTUAR_THROWING || mZombiePhase == ZombiePhase.PHASE_GARGANTUAR_SMASHING || mZombiePhase == ZombiePhase.PHASE_CATAPULT_LAUNCHING || mZombiePhase == ZombiePhase.PHASE_CATAPULT_RELOADING || mZombiePhase == ZombiePhase.PHASE_DIGGER_RISING || mZombiePhase == ZombiePhase.PHASE_DIGGER_TUNNELING_PAUSE_WITHOUT_AXE || mZombiePhase == ZombiePhase.PHASE_DIGGER_RISE_WITHOUT_AXE || mZombiePhase == ZombiePhase.PHASE_DIGGER_STUNNED || mZombiePhase == ZombiePhase.PHASE_DANCER_SNAPPING_FINGERS || mZombiePhase == ZombiePhase.PHASE_DANCER_SNAPPING_FINGERS_WITH_LIGHT || mZombiePhase == ZombiePhase.PHASE_DANCER_SNAPPING_FINGERS_HOLD || mZombiePhase == ZombiePhase.PHASE_DANCER_RISING || mZombiePhase == ZombiePhase.PHASE_IMP_GETTING_THROWN || mZombiePhase == ZombiePhase.PHASE_IMP_LANDING || mZombiePhase == ZombiePhase.PHASE_LADDER_PLACING || mZombieHeight == ZombieHeight.HEIGHT_IN_TO_CHIMNEY || mZombieHeight == ZombieHeight.HEIGHT_GETTING_BUNGEE_DROPPED || mZombieHeight == ZombieHeight.HEIGHT_ZOMBIQUARIUM || mZombieType == ZombieType.ZOMBIE_BUNGEE || mZombieType == ZombieType.ZOMBIE_BOSS)
			{
				return true;
			}
			if (mZombiePhase == ZombiePhase.PHASE_DANCER_RAISE_LEFT_1 || mZombiePhase == ZombiePhase.PHASE_DANCER_WALK_TO_RAISE || mZombiePhase == ZombiePhase.PHASE_DANCER_RAISE_RIGHT_1 || mZombiePhase == ZombiePhase.PHASE_DANCER_RAISE_LEFT_2 || mZombiePhase == ZombiePhase.PHASE_DANCER_RAISE_RIGHT_2)
			{
				return true;
			}
			if (mZombieType == ZombieType.ZOMBIE_DANCER || mZombieType == ZombieType.ZOMBIE_BACKUP_DANCER)
			{
				Zombie zombie = (mZombieType != ZombieType.ZOMBIE_DANCER) ? mBoard.ZombieTryToGet(mRelatedZombieID) : this;
				if (zombie == null)
				{
					return false;
				}
				if (zombie.IsImmobilizied() || zombie.mIsEating)
				{
					return true;
				}
				for (int i = 0; i < GameConstants.NUM_BACKUP_DANCERS; i++)
				{
					Zombie zombie2 = mBoard.ZombieTryToGet(zombie.mFollowerZombieID[i]);
					if (zombie2 != null && (zombie2.IsImmobilizied() || zombie2.mIsEating))
					{
						return true;
					}
				}
			}
			return false;
		}

		public Zombie FindZombieTarget()
		{
			if (mZombiePhase == ZombiePhase.PHASE_DIGGER_TUNNELING)
			{
				return null;
			}
			TRect zombieAttackRect = GetZombieAttackRect();
			int count = mBoard.mZombies.Count;
			for (int i = 0; i < count; i++)
			{
				Zombie zombie = mBoard.mZombies[i];
				if (!zombie.mDead && mMindControlled != zombie.mMindControlled && !zombie.IsFlying() && zombie.mZombiePhase != ZombiePhase.PHASE_DIGGER_TUNNELING && zombie.mZombiePhase != ZombiePhase.PHASE_BUNGEE_DIVING && zombie.mZombiePhase != ZombiePhase.PHASE_BUNGEE_DIVING_SCREAMING && zombie.mZombiePhase != ZombiePhase.PHASE_BUNGEE_RISING && zombie.mZombieHeight != ZombieHeight.HEIGHT_GETTING_BUNGEE_DROPPED && !zombie.IsDeadOrDying() && zombie.mRow == mRow)
				{
					TRect zombieRect = zombie.GetZombieRect();
					int rectOverlap = GameConstants.GetRectOverlap(zombieAttackRect, zombieRect);
					if (rectOverlap >= 20 || (rectOverlap >= 0 && zombie.mIsEating))
					{
						return zombie;
					}
				}
			}
			return null;
		}

		public void PlayZombieReanim(ref string theTrackName, ReanimLoopType theLoopType, byte theBlendTime, float theAnimRate)
		{
			lastPlayedReanimName = theTrackName;
			lastPlayedReanimLoopType = theLoopType;
			lastPlayedReanimBlendTime = theBlendTime;
			lastPlayedReanimAnimRate = theAnimRate;
			Reanimation reanimation = mApp.ReanimationTryToGet(mBodyReanimID);
			if (reanimation != null)
			{
				reanimation.PlayReanim(theTrackName, theLoopType, theBlendTime, theAnimRate);
				if (theAnimRate != 0f)
				{
					mOrginalAnimRate = theAnimRate;
				}
				UpdateAnimSpeed();
			}
		}

		public void UpdateZombieBackupDancer()
		{
			if (mIsEating)
			{
				return;
			}
			if (mZombiePhase == ZombiePhase.PHASE_DANCER_RISING)
			{
				mAltitude = TodCommon.TodAnimateCurve(150, 0, mPhaseCounter, Constants.ZOMBIE_BACKUP_DANCER_RISE_HEIGHT, 0, TodCurves.CURVE_LINEAR);
				mUsesClipping = (mAltitude < 0f);
				if (mPhaseCounter != 0)
				{
					return;
				}
				if (IsOnHighGround())
				{
					mAltitude = Constants.HIGH_GROUND_HEIGHT;
				}
			}
			ZombiePhase dancerPhase = GetDancerPhase();
			if (dancerPhase != mZombiePhase)
			{
				switch (dancerPhase)
				{
				case ZombiePhase.PHASE_DANCER_DANCING_LEFT:
					mZombiePhase = dancerPhase;
					PlayZombieReanim(ref Reanimation.ReanimTrackId_anim_walk, ReanimLoopType.REANIM_LOOP, 10, 0f);
					break;
				case ZombiePhase.PHASE_DANCER_WALK_TO_RAISE:
				{
					mZombiePhase = dancerPhase;
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_armraise, ReanimLoopType.REANIM_LOOP, 10, 18f);
					Reanimation reanimation = mApp.ReanimationGet(mBodyReanimID);
					reanimation.mAnimTime = 0.6f;
					break;
				}
				case ZombiePhase.PHASE_DANCER_RAISE_LEFT_1:
				case ZombiePhase.PHASE_DANCER_RAISE_RIGHT_1:
				case ZombiePhase.PHASE_DANCER_RAISE_LEFT_2:
				case ZombiePhase.PHASE_DANCER_RAISE_RIGHT_2:
					mZombiePhase = dancerPhase;
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_armraise, ReanimLoopType.REANIM_LOOP, 10, 18f);
					break;
				}
			}
		}

		public ZombiePhase GetDancerPhase()
		{
			int dancerFrame = GetDancerFrame();
			if (dancerFrame <= 11)
			{
				return ZombiePhase.PHASE_DANCER_DANCING_LEFT;
			}
			if (dancerFrame <= 12)
			{
				return ZombiePhase.PHASE_DANCER_WALK_TO_RAISE;
			}
			if (dancerFrame <= 15)
			{
				return ZombiePhase.PHASE_DANCER_RAISE_RIGHT_1;
			}
			if (dancerFrame <= 18)
			{
				return ZombiePhase.PHASE_DANCER_RAISE_LEFT_1;
			}
			if (dancerFrame <= 21)
			{
				return ZombiePhase.PHASE_DANCER_RAISE_RIGHT_2;
			}
			return ZombiePhase.PHASE_DANCER_RAISE_LEFT_2;
		}

		public bool IsMovingAtChilledSpeed()
		{
			if (mChilledCounter > 0)
			{
				return true;
			}
			if (mZombieType == ZombieType.ZOMBIE_DANCER || mZombieType == ZombieType.ZOMBIE_BACKUP_DANCER)
			{
				Zombie zombie = (mZombieType != ZombieType.ZOMBIE_DANCER) ? mBoard.ZombieTryToGet(mRelatedZombieID) : this;
				if (zombie == null)
				{
					return false;
				}
				if (zombie.mChilledCounter > 0)
				{
					return true;
				}
				for (int i = 0; i < GameConstants.NUM_BACKUP_DANCERS; i++)
				{
					Zombie zombie2 = mBoard.ZombieTryToGet(zombie.mFollowerZombieID[i]);
					if (zombie2 != null && zombie2.mChilledCounter > 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		public void StartWalkAnim(byte theBlendTime)
		{
			Reanimation reanimation = mApp.ReanimationTryToGet(mBodyReanimID);
			if (reanimation == null)
			{
				return;
			}
			PickRandomSpeed();
			if (mZombiePhase == ZombiePhase.PHASE_LADDER_CARRYING)
			{
				PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_ladderwalk, ReanimLoopType.REANIM_LOOP, theBlendTime, 0f);
				return;
			}
			if (mZombiePhase == ZombiePhase.PHASE_NEWSPAPER_MAD)
			{
				PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_walk_nopaper, ReanimLoopType.REANIM_LOOP, theBlendTime, 0f);
				return;
			}
			if (mInPool && mZombieHeight != ZombieHeight.HEIGHT_IN_TO_POOL && mZombieHeight != ZombieHeight.HEIGHT_OUT_OF_POOL && reanimation.TrackExists(GlobalMembersReanimIds.ReanimTrackId_anim_swim))
			{
				PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_swim, ReanimLoopType.REANIM_LOOP, theBlendTime, 0f);
				return;
			}
			if ((mZombieType == ZombieType.ZOMBIE_NORMAL || mZombieType == ZombieType.ZOMBIE_TRAFFIC_CONE || mZombieType == ZombieType.ZOMBIE_PAIL) && mBoard.mDanceMode)
			{
				PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_dance, ReanimLoopType.REANIM_LOOP, theBlendTime, 0f);
				return;
			}
			int num = RandomNumbers.NextNumber(2);
			if (IsZombotany())
			{
				num = 0;
			}
			if (mZombieType == ZombieType.ZOMBIE_FLAG)
			{
				num = 0;
			}
			if (num == 0 && reanimation.TrackExists(GlobalMembersReanimIds.ReanimTrackId_anim_walk2))
			{
				PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_walk2, ReanimLoopType.REANIM_LOOP, theBlendTime, 0f);
			}
			else if (reanimation.TrackExists(Reanimation.ReanimTrackId_anim_walk))
			{
				PlayZombieReanim(ref Reanimation.ReanimTrackId_anim_walk, ReanimLoopType.REANIM_LOOP, theBlendTime, 0f);
			}
		}

		public Reanimation AddAttachedReanim(int thePosX, int thePosY, ReanimationType theReanimType)
		{
			if (mDead)
			{
				return null;
			}
			Reanimation reanimation = mApp.AddReanimation(mX + thePosX, mY + thePosY, 0, theReanimType);
			if (reanimation != null)
			{
				GlobalMembersAttachment.AttachReanim(ref mAttachmentID, reanimation, (float)thePosX * Constants.S, (float)thePosY * Constants.S);
			}
			return reanimation;
		}

		public void DragUnder()
		{
			mZombieHeight = ZombieHeight.HEIGHT_DRAGGED_UNDER;
			StopEating();
			ReanimReenableClipping();
		}

		public static void SetupDoorArms(Reanimation aReanim, bool theShow)
		{
			int theRenderGroup = 0;
			int theRenderGroup2 = -1;
			if (theShow)
			{
				theRenderGroup = -1;
				theRenderGroup2 = 0;
			}
			aReanim.AssignRenderGroupToPrefix("Zombie_outerarm_hand", theRenderGroup);
			aReanim.AssignRenderGroupToPrefix("Zombie_outerarm_lower", theRenderGroup);
			aReanim.AssignRenderGroupToPrefix("Zombie_outerarm_upper", theRenderGroup);
			aReanim.AssignRenderGroupToPrefix("anim_innerarm", theRenderGroup);
			aReanim.AssignRenderGroupToPrefix("Zombie_outerarm_screendoor", theRenderGroup2);
			aReanim.AssignRenderGroupToPrefix("Zombie_innerarm_screendoor", theRenderGroup2);
			aReanim.AssignRenderGroupToPrefix("Zombie_innerarm_screendoor_hand", theRenderGroup2);
		}

		public static void SetupReanimLayers(Reanimation aReanim, ZombieType theZombieType)
		{
			aReanim.AssignRenderGroupToPrefix("anim_cone", -1);
			aReanim.AssignRenderGroupToPrefix("anim_bucket", -1);
			aReanim.AssignRenderGroupToPrefix("anim_screendoor", -1);
			aReanim.AssignRenderGroupToPrefix("Zombie_flaghand", -1);
			aReanim.AssignRenderGroupToPrefix("Zombie_duckytube", -1);
			aReanim.AssignRenderGroupToPrefix("anim_tongue", -1);
			aReanim.AssignRenderGroupToPrefix("Zombie_mustache", -1);
			SetupDoorArms(aReanim, false);
			switch (theZombieType)
			{
			case ZombieType.ZOMBIE_TRAFFIC_CONE:
				aReanim.AssignRenderGroupToPrefix("anim_cone", 0);
				aReanim.AssignRenderGroupToPrefix("anim_hair", -1);
				break;
			case ZombieType.ZOMBIE_PAIL:
				aReanim.AssignRenderGroupToPrefix("anim_bucket", 0);
				aReanim.AssignRenderGroupToPrefix("anim_hair", -1);
				break;
			case ZombieType.ZOMBIE_DOOR:
				SetupDoorArms(aReanim, true);
				break;
			case ZombieType.ZOMBIE_NEWSPAPER:
				aReanim.AssignRenderGroupToPrefix("Zombie_paper_paper", -1);
				break;
			case ZombieType.ZOMBIE_FLAG:
				aReanim.AssignRenderGroupToPrefix("anim_innerarm", -1);
				aReanim.AssignRenderGroupToTrack(GlobalMembersReanimIds.ReanimTrackId_zombie_flaghand, 0);
				aReanim.AssignRenderGroupToTrack(GlobalMembersReanimIds.ReanimTrackId_zombie_innerarm_screendoor, 0);
				break;
			case ZombieType.ZOMBIE_DUCKY_TUBE:
				aReanim.AssignRenderGroupToPrefix("Zombie_duckytube", 0);
				break;
			}
		}

		public bool IsOnBoard()
		{
			if (mFromWave == GameConstants.ZOMBIE_WAVE_CUTSCENE || mFromWave == GameConstants.ZOMBIE_WAVE_UI)
			{
				return false;
			}
			Debug.ASSERT(mBoard != null);
			return true;
		}

		public void DrawButter(Graphics g, ref ZombieDrawPosition theDrawPos)
		{
			float thePosX = mPosX + theDrawPos.mImageOffsetX + (float)theDrawPos.mHeadX + 11f;
			float thePosY = mPosY + theDrawPos.mImageOffsetY + (float)theDrawPos.mHeadY + theDrawPos.mBodyY + 21f;
			float num = 1f;
			if (mBodyReanimID != null)
			{
				GetTrackPosition(ref GlobalMembersReanimIds.ReanimTrackId_anim_head1, ref thePosX, ref thePosY);
			}
			thePosX += 0f - mPosX;
			thePosY += 0f - mPosY;
			if (mZombieType == ZombieType.ZOMBIE_SQUASH_HEAD)
			{
				thePosX += 6f;
				thePosY -= 9f;
			}
			else if (mZombieType == ZombieType.ZOMBIE_WALLNUT_HEAD)
			{
				thePosX -= 10f;
				if (mInPool && mIsEating)
				{
					thePosX -= 5f;
					thePosY += 10f;
				}
			}
			else if (mZombieType == ZombieType.ZOMBIE_TALLNUT_HEAD)
			{
				thePosX -= 30f;
				thePosY -= 30f;
				if (mInPool && mIsEating)
				{
					thePosY += 10f;
				}
			}
			else if (mZombieType == ZombieType.ZOMBIE_GATLING_HEAD)
			{
				thePosY -= 10f;
			}
			TodCommon.TodDrawImageScaledF(g, AtlasResources.IMAGE_REANIM_CORNPULT_BUTTER_SPLAT, thePosX * Constants.S + 0f, thePosY * Constants.S - 6f, num, num);
		}

		public bool IsImmobilizied()
		{
			if (mIceTrapCounter > 0 || mButteredCounter > 0)
			{
				return true;
			}
			return false;
		}

		public void ApplyButter()
		{
			if (!mHasHead || !CanBeFrozen() || mZombieType == ZombieType.ZOMBIE_ZAMBONI || mZombieType == ZombieType.ZOMBIE_BOSS || IsTangleKelpTarget() || IsBobsledTeamWithSled() || IsFlying())
			{
				return;
			}
			mButteredCounter = 400;
			Zombie zombie = mBoard.ZombieTryToGet(mRelatedZombieID);
			if (zombie != null)
			{
				zombie.mRelatedZombieID = null;
				mRelatedZombieID = null;
			}
			if (mZombieType == ZombieType.ZOMBIE_POGO)
			{
				mAltitude = 0f;
				if (mOnHighGround)
				{
					mAltitude += Constants.HIGH_GROUND_HEIGHT;
				}
			}
			if (mZombieType == ZombieType.ZOMBIE_BALLOON)
			{
				BalloonPropellerHatSpin(false);
			}
			if (mZombieType == ZombieType.ZOMBIE_PEA_HEAD || mZombieType == ZombieType.ZOMBIE_WALLNUT_HEAD || mZombieType == ZombieType.ZOMBIE_TALLNUT_HEAD || mZombieType == ZombieType.ZOMBIE_JALAPENO_HEAD || mZombieType == ZombieType.ZOMBIE_GATLING_HEAD || mZombieType == ZombieType.ZOMBIE_SQUASH_HEAD)
			{
				Reanimation reanimation = mApp.ReanimationTryToGet(mSpecialHeadReanimID);
				if (reanimation != null)
				{
					reanimation.mAnimRate = 0f;
				}
			}
			UpdateAnimSpeed();
			StopZombieSound();
		}

		public float ZombieTargetLeadX(float theTime)
		{
			float num = mVelX;
			if (mChilledCounter > 0)
			{
				num *= GameConstants.CHILLED_SPEED_FACTOR;
			}
			if (IsWalkingBackwards())
			{
				num = 0f - num;
			}
			if (ZombieNotWalking())
			{
				num = 0f;
			}
			float num2 = num * theTime;
			TRect zombieRect = GetZombieRect();
			int num3 = zombieRect.mX + zombieRect.mWidth / 2;
			return (float)num3 - num2;
		}

		public void UpdateZombieImp()
		{
			if (mZombiePhase == ZombiePhase.PHASE_IMP_GETTING_THROWN)
			{
				mVelZ -= GameConstants.THOWN_ZOMBIE_GRAVITY;
				mAltitude += mVelZ;
				mPosX -= mVelX;
				float num = GetPosYBasedOnRow(mRow) - mPosY;
				mPosY += num;
				mAltitude += num;
				if (mAltitude <= 0f)
				{
					mAltitude = 0f;
					mZombiePhase = ZombiePhase.PHASE_IMP_LANDING;
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_land, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 0, 24f);
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_IMP_LANDING)
			{
				Reanimation reanimation = mApp.ReanimationGet(mBodyReanimID);
				if (reanimation.mLoopCount > 0)
				{
					mZombiePhase = ZombiePhase.PHASE_ZOMBIE_NORMAL;
					StartWalkAnim(0);
				}
			}
		}

		public void SquishAllInSquare(int theX, int theY, ZombieAttackType theAttackType)
		{
			int count = mBoard.mPlants.Count;
			for (int i = 0; i < count; i++)
			{
				Plant plant = mBoard.mPlants[i];
				if (!plant.mDead && theY == plant.mRow && theX == plant.mPlantCol && (theAttackType != ZombieAttackType.ATTACKTYPE_DRIVE_OVER || !plant.IsSpiky()) && plant.mSeedType != SeedType.SEED_SPIKEROCK)
				{
					mBoard.mPlantsEaten++;
					plant.Squish();
				}
			}
		}

		public void RemoveIceTrap()
		{
			mIceTrapCounter = 0;
			if (mZombieType == ZombieType.ZOMBIE_BALLOON)
			{
				BalloonPropellerHatSpin(true);
			}
			UpdateAnimSpeed();
			StartZombieSound();
		}

		public bool IsBouncingPogo()
		{
			if (mZombiePhase >= ZombiePhase.PHASE_POGO_BOUNCING && mZombiePhase <= ZombiePhase.PHASE_POGO_FORWARD_BOUNCE_7)
			{
				return true;
			}
			return false;
		}

		public int GetBobsledPosition()
		{
			if (mZombieType != ZombieType.ZOMBIE_BOBSLED)
			{
				return -1;
			}
			if (mRelatedZombieID == null && mFollowerZombieID[0] == null)
			{
				return -1;
			}
			if (mRelatedZombieID == null)
			{
				return 0;
			}
			Zombie zombie = mBoard.ZombieGetID(this);
			Zombie zombie2 = mBoard.ZombieGet(mRelatedZombieID);
			for (int i = 0; i < 3; i++)
			{
				if (zombie2.mFollowerZombieID[i] == zombie)
				{
					return i + 1;
				}
			}
			Debug.ASSERT(false);
			return -666;
		}

		public void DrawBobsledReanim(Graphics g, ref ZombieDrawPosition theDrawPos, bool theBeforeZombie)
		{
			int bobsledPosition = GetBobsledPosition();
			bool flag = false;
			bool flag2 = false;
			Zombie zombie;
			if (mFromWave == GameConstants.ZOMBIE_WAVE_CUTSCENE)
			{
				zombie = this;
			}
			else
			{
				switch (bobsledPosition)
				{
				case -1:
					return;
				case 0:
					zombie = this;
					break;
				default:
					zombie = mBoard.ZombieGet(mRelatedZombieID);
					break;
				}
			}
			if (mFromWave == GameConstants.ZOMBIE_WAVE_CUTSCENE)
			{
				if (theBeforeZombie)
				{
					flag2 = true;
				}
				else
				{
					flag = true;
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_BOBSLED_CRASHING)
			{
				if (bobsledPosition == 0 && !theBeforeZombie)
				{
					flag = true;
					flag2 = true;
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_BOBSLED_SLIDING || mZombiePhase == ZombiePhase.PHASE_ZOMBIE_BURNED)
			{
				if (bobsledPosition == 2 && theBeforeZombie)
				{
					flag = true;
					flag2 = true;
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_BOBSLED_BOARDING)
			{
				Reanimation reanimation = mApp.ReanimationGet(mBodyReanimID);
				if (reanimation.mAnimTime < 0.5f)
				{
					if (bobsledPosition == 2 && theBeforeZombie)
					{
						flag = true;
						flag2 = true;
					}
				}
				else if (bobsledPosition == 0 && !theBeforeZombie)
				{
					flag = true;
				}
				else if (bobsledPosition == 3 && theBeforeZombie)
				{
					flag2 = true;
				}
			}
			int num = 0;
			int num2 = 255;
			float num3 = theDrawPos.mImageOffsetX + zombie.mPosX - mPosX - 76f;
			float num4 = 15f;
			if (mZombiePhase == ZombiePhase.PHASE_BOBSLED_CRASHING)
			{
				num = 3;
				num2 = TodCommon.TodAnimateCurve(30, 0, mPhaseCounter, 255, 0, TodCurves.CURVE_LINEAR);
				num3 += (float)(GameConstants.BOBSLED_CRASH_TIME - mPhaseCounter) * mVelX / (float)GameConstants.ZOMBIE_LIMP_SPEED_FACTOR;
				num3 -= TodCommon.TodAnimateCurveFloat(GameConstants.BOBSLED_CRASH_TIME, 0, mPhaseCounter, 0f, 50f, TodCurves.CURVE_EASE_OUT);
				num4 += TodCommon.TodAnimateCurveFloat(GameConstants.BOBSLED_CRASH_TIME, 75, mPhaseCounter, 5f, 10f, TodCurves.CURVE_LINEAR);
			}
			else
			{
				num = zombie.GetHelmDamageIndex();
			}
			if (num2 != 255)
			{
				g.SetColorizeImages(true);
				g.SetColor(new SexyColor(255, 255, 255, num2));
			}
			Image theImage;
			switch (num)
			{
			case 0:
				theImage = AtlasResources.IMAGE_ZOMBIE_BOBSLED1;
				break;
			case 1:
				theImage = AtlasResources.IMAGE_ZOMBIE_BOBSLED2;
				break;
			case 2:
				theImage = AtlasResources.IMAGE_ZOMBIE_BOBSLED3;
				break;
			default:
				theImage = AtlasResources.IMAGE_ZOMBIE_BOBSLED4;
				break;
			}
			if (mZombiePhase == ZombiePhase.PHASE_ZOMBIE_BURNED)
			{
				g.SetColorizeImages(true);
				g.SetColor(SexyColor.Black);
			}
			num3 *= Constants.S;
			num4 *= Constants.S;
			if (flag2 && num != 3)
			{
				g.DrawImageF(AtlasResources.IMAGE_ZOMBIE_BOBSLED_INSIDE, num3, num4);
			}
			if (flag)
			{
				g.DrawImageF(theImage, num3, num4);
			}
			if (zombie.mJustGotShotCounter > 0)
			{
				g.SetDrawMode(Graphics.DrawMode.DRAWMODE_ADDITIVE);
				g.SetColorizeImages(true);
				int num5 = zombie.mJustGotShotCounter * 10;
				g.SetColor(new SexyColor(num5, num5, num5, 255));
				if (flag2 && num != 3)
				{
					g.DrawImageF(AtlasResources.IMAGE_ZOMBIE_BOBSLED_INSIDE, num3, num4);
				}
				if (flag)
				{
					g.DrawImageF(theImage, num3, num4);
				}
				g.SetDrawMode(Graphics.DrawMode.DRAWMODE_NORMAL);
			}
			g.SetColorizeImages(false);
		}

		public void BobsledDie()
		{
			if (!IsBobsledTeamWithSled() || !IsOnBoard())
			{
				return;
			}
			Zombie zombie = (mRelatedZombieID != null) ? mBoard.ZombieGet(mRelatedZombieID) : this;
			if (!zombie.mDead)
			{
				zombie.DieNoLoot(true);
			}
			for (int i = 0; i < 3; i++)
			{
				Zombie zombie2 = mBoard.ZombieGet(zombie.mFollowerZombieID[i]);
				if (!zombie2.mDead)
				{
					zombie2.DieNoLoot(true);
				}
			}
		}

		public void BobsledBurn()
		{
			if (IsBobsledTeamWithSled())
			{
				Zombie zombie = (mRelatedZombieID != null) ? mBoard.ZombieGet(mRelatedZombieID) : this;
				zombie.ApplyBurn();
				for (int i = 0; i < 3; i++)
				{
					Zombie zombie2 = mBoard.ZombieGet(zombie.mFollowerZombieID[i]);
					zombie2.ApplyBurn();
				}
			}
		}

		public bool IsBobsledTeamWithSled()
		{
			if (GetBobsledPosition() == -1)
			{
				return false;
			}
			return true;
		}

		public bool CanBeFrozen()
		{
			if (!CanBeChilled() || mZombiePhase == ZombiePhase.PHASE_POLEVAULTER_IN_VAULT || mZombiePhase == ZombiePhase.PHASE_DOLPHIN_INTO_POOL || mZombiePhase == ZombiePhase.PHASE_DOLPHIN_IN_JUMP || mZombiePhase == ZombiePhase.PHASE_SNORKEL_INTO_POOL || IsFlying() || mZombiePhase == ZombiePhase.PHASE_IMP_GETTING_THROWN || mZombiePhase == ZombiePhase.PHASE_IMP_LANDING || mZombiePhase == ZombiePhase.PHASE_BOBSLED_CRASHING || mZombiePhase == ZombiePhase.PHASE_JACK_IN_THE_BOX_POPPING || mZombiePhase == ZombiePhase.PHASE_SQUASH_RISING || mZombiePhase == ZombiePhase.PHASE_SQUASH_FALLING || mZombiePhase == ZombiePhase.PHASE_SQUASH_DONE_FALLING || IsBouncingPogo())
			{
				return false;
			}
			if (mZombieType == ZombieType.ZOMBIE_BUNGEE && mZombiePhase != ZombiePhase.PHASE_BUNGEE_AT_BOTTOM)
			{
				return false;
			}
			return true;
		}

		public bool CanBeChilled()
		{
			if (mZombieType == ZombieType.ZOMBIE_ZAMBONI || IsBobsledTeamWithSled() || IsDeadOrDying() || mZombiePhase == ZombiePhase.PHASE_DIGGER_TUNNELING || mZombiePhase == ZombiePhase.PHASE_DIGGER_RISING || mZombiePhase == ZombiePhase.PHASE_DIGGER_TUNNELING_PAUSE_WITHOUT_AXE || mZombiePhase == ZombiePhase.PHASE_DIGGER_RISE_WITHOUT_AXE || mZombiePhase == ZombiePhase.PHASE_RISING_FROM_GRAVE || mZombiePhase == ZombiePhase.PHASE_DANCER_RISING || mMindControlled)
			{
				return false;
			}
			if (mZombieType == ZombieType.ZOMBIE_BOSS && mZombiePhase != ZombiePhase.PHASE_BOSS_HEAD_IDLE_BEFORE_SPIT && mZombiePhase != ZombiePhase.PHASE_BOSS_HEAD_IDLE_AFTER_SPIT && mZombiePhase != ZombiePhase.PHASE_BOSS_HEAD_SPIT)
			{
				return false;
			}
			return true;
		}

		public void UpdateZombieSnorkel()
		{
			bool flag = IsWalkingBackwards();
			if (mZombiePhase == ZombiePhase.PHASE_SNORKEL_WALKING && !flag)
			{
				if (mX > 770 && mX <= 800)
				{
					mZombiePhase = ZombiePhase.PHASE_SNORKEL_INTO_POOL;
					mVelX = 0.2f;
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_jumpinpool, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 20, 16f);
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_SNORKEL_INTO_POOL)
			{
				Reanimation reanimation = mApp.ReanimationGet(mBodyReanimID);
				mAltitude = TodCommon.TodAnimateCurveFloat(0, 1000, (int)reanimation.mAnimTime * 1000, 0f, 10f, TodCurves.CURVE_LINEAR);
				if (reanimation.ShouldTriggerTimedEvent(0.83f))
				{
					Reanimation reanimation2 = mApp.AddReanimation(mX - 47, mY + 73, mRenderOrder + 1, ReanimationType.REANIM_SPLASH);
					reanimation2.OverrideScale(1.2f, 0.8f);
					mApp.AddTodParticle(mX - 10, mY + 115, mRenderOrder + 1, ParticleEffect.PARTICLE_PLANTING_POOL);
					mApp.PlayFoley(FoleyType.FOLEY_ZOMBIE_ENTERING_WATER);
				}
				if (reanimation.mLoopCount > 0)
				{
					mZombiePhase = ZombiePhase.PHASE_SNORKEL_WALKING_IN_POOL;
					mInPool = true;
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_swim, ReanimLoopType.REANIM_LOOP_FULL_LAST_FRAME, 0, 12f);
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_SNORKEL_WALKING_IN_POOL)
			{
				if (!mHasHead)
				{
					TakeDamage(1800, 9u);
				}
				else if (mX <= 140 && !flag)
				{
					mZombieHeight = ZombieHeight.HEIGHT_OUT_OF_POOL;
					mZombiePhase = ZombiePhase.PHASE_SNORKEL_WALKING;
					mAltitude = -90f;
					mPosX -= 15f;
					PoolSplash(false);
					StartWalkAnim(0);
				}
				else if ((float)mX > 730f && flag)
				{
					mZombieHeight = ZombieHeight.HEIGHT_OUT_OF_POOL;
					mZombiePhase = ZombiePhase.PHASE_SNORKEL_WALKING;
					mAltitude = -90f;
					mPosX += 15f;
					PoolSplash(false);
					StartWalkAnim(0);
				}
				else if (mIsEating)
				{
					mZombiePhase = ZombiePhase.PHASE_SNORKEL_UP_TO_EAT;
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_uptoeat, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 0, 24f);
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_SNORKEL_UP_TO_EAT)
			{
				Reanimation reanimation3 = mApp.ReanimationGet(mBodyReanimID);
				if (!mIsEating)
				{
					mZombiePhase = ZombiePhase.PHASE_SNORKEL_DOWN_FROM_EAT;
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_uptoeat, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 0, -24f);
				}
				else if (reanimation3.mLoopCount > 0)
				{
					mZombiePhase = ZombiePhase.PHASE_SNORKEL_EATING_IN_POOL;
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_eat, ReanimLoopType.REANIM_LOOP, 0, 0f);
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_SNORKEL_EATING_IN_POOL)
			{
				if (!mIsEating)
				{
					mZombiePhase = ZombiePhase.PHASE_SNORKEL_DOWN_FROM_EAT;
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_uptoeat, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 0, -24f);
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_SNORKEL_DOWN_FROM_EAT)
			{
				Reanimation reanimation4 = mApp.ReanimationGet(mBodyReanimID);
				if (reanimation4.mLoopCount > 0)
				{
					mZombiePhase = ZombiePhase.PHASE_SNORKEL_WALKING_IN_POOL;
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_swim, ReanimLoopType.REANIM_LOOP_FULL_LAST_FRAME, 0, 0f);
					PickRandomSpeed();
				}
			}
			mUsesClipping = (mZombiePhase == ZombiePhase.PHASE_SNORKEL_WALKING || mZombiePhase == ZombiePhase.PHASE_ZOMBIE_DYING || mZombiePhase == ZombiePhase.PHASE_SNORKEL_INTO_POOL);
		}

		public void ReanimIgnoreClipRect(string theTrackName, bool theIgnoreClipRect)
		{
			Reanimation reanimation = mApp.ReanimationTryToGet(mBodyReanimID);
			if (reanimation == null)
			{
				return;
			}
			for (int i = 0; i < reanimation.mDefinition.mTrackCount; i++)
			{
				ReanimatorTrack reanimatorTrack = reanimation.mDefinition.mTracks[i];
				ReanimatorTrackInstance reanimatorTrackInstance = reanimation.mTrackInstances[i];
				if (reanimatorTrack.mName == theTrackName)
				{
					reanimatorTrackInstance.mIgnoreClipRect = theIgnoreClipRect;
				}
			}
			mUsesClipping = !theIgnoreClipRect;
		}

		public void SetAnimRate(float theAnimRate)
		{
			mOrginalAnimRate = theAnimRate;
			ApplyAnimRate(theAnimRate);
		}

		public void ApplyAnimRate(float theAnimRate)
		{
			Reanimation reanimation = mApp.ReanimationTryToGet(mBodyReanimID);
			if (reanimation != null)
			{
				reanimation.mAnimRate = theAnimRate;
				if (IsMovingAtChilledSpeed())
				{
					reanimation.mAnimRate *= 0.5f;
				}
			}
		}

		public bool IsDeadOrDying()
		{
			if (mDead || mZombiePhase == ZombiePhase.PHASE_ZOMBIE_DYING || mZombiePhase == ZombiePhase.PHASE_ZOMBIE_BURNED || mZombiePhase == ZombiePhase.PHASE_ZOMBIE_MOWERED)
			{
				return true;
			}
			return false;
		}

		public void DrawDancerReanim(Graphics g, ref ZombieDrawPosition theDrawPos)
		{
			bool flag = false;
			SexyColor aColor = default(SexyColor);
			float zombie_Dancer_Spotlight_Scale = Constants.Zombie_Dancer_Spotlight_Scale;
			float num = Constants.Zombie_Dancer_Spotlight_Offset.X;
			if (mZombiePhase != ZombiePhase.PHASE_DANCER_DANCING_IN && mZombiePhase != ZombiePhase.PHASE_DANCER_SNAPPING_FINGERS && mZombiePhase != 0 && mZombiePhase != ZombiePhase.PHASE_ZOMBIE_DYING && mApp.mGameScene != GameScenes.SCENE_ZOMBIES_WON)
			{
				flag = true;
				int num2 = mZombieAge / 100 * 7 % 5;
				if (mZombieAge < 700)
				{
					num2 = 0;
				}
				switch (num2)
				{
				case 0:
					aColor = new SexyColor(250, 250, 160);
					break;
				case 1:
					aColor = new SexyColor(114, 234, 170);
					break;
				case 2:
					aColor = new SexyColor(216, 126, 202);
					break;
				case 3:
					aColor = new SexyColor(90, 110, 140);
					break;
				case 4:
					aColor = new SexyColor(240, 90, 130);
					break;
				}
				g.SetColorizeImages(true);
				g.SetColor(aColor);
				TodCommon.TodDrawImageScaledF(g, AtlasResources.IMAGE_SPOTLIGHT2, num * Constants.S, (float)Constants.Zombie_Dancer_Spotlight_Offset.Y * Constants.S, zombie_Dancer_Spotlight_Scale, zombie_Dancer_Spotlight_Scale);
				g.SetColorizeImages(false);
			}
			Reanimation reanimation = mApp.ReanimationTryToGet(mBodyReanimID);
			reanimation.DrawRenderGroup(g, 0);
			if (flag)
			{
				g.SetColorizeImages(true);
				g.SetColor(aColor);
				TodCommon.TodDrawImageScaledF(g, AtlasResources.IMAGE_SPOTLIGHT, ((float)Constants.Zombie_Dancer_Spotlight_Pos.X + num) * Constants.S, (float)(Constants.Zombie_Dancer_Spotlight_Pos.Y + Constants.Zombie_Dancer_Spotlight_Offset.Y) * Constants.S, zombie_Dancer_Spotlight_Scale, zombie_Dancer_Spotlight_Scale);
				g.SetColorizeImages(false);
			}
		}

		public void DrawBungeeReanim(Graphics g, ref ZombieDrawPosition theDrawPos)
		{
			Reanimation reanimation = mApp.ReanimationTryToGet(mBodyReanimID);
			float num = Constants.InvertAndScale(-22f);
			float num2 = Constants.InvertAndScale(14f) + theDrawPos.mBodyY + theDrawPos.mImageOffsetY;
			DrawBungeeCord(g, (int)num, (int)num2);
			reanimation.DrawRenderGroup(g, 0);
			Zombie zombie = null;
			int num3 = -1;
			if (mBoard != null)
			{
				num3 = mBoard.mZombies.IndexOf(mRelatedZombieID);
			}
			if (num3 != -1)
			{
				zombie = mBoard.mZombies[num3];
			}
			if (zombie != null)
			{
				Graphics @new = Graphics.GetNew(g);
				@new.mTransY += (int)((0f - mAltitude) * Constants.S);
				@new.mTransX += (int)((zombie.mPosX - mPosX) * Constants.S);
				ZombieDrawPosition theDrawPos2 = default(ZombieDrawPosition);
				zombie.GetDrawPos(ref theDrawPos2);
				zombie.DrawReanim(@new, ref theDrawPos2, 0);
				@new.PrepareForReuse();
			}
			else
			{
				Plant plant = null;
				int num4 = -1;
				if (mBoard != null)
				{
					num4 = mBoard.mPlants.IndexOf(mTargetPlantID);
				}
				if (num4 != -1)
				{
					plant = mBoard.mPlants[num4];
				}
				if (plant != null)
				{
					Graphics new2 = Graphics.GetNew(g);
					new2.mTransY += (int)((30f - mAltitude) * Constants.S);
					if (mZombiePhase == ZombiePhase.PHASE_BUNGEE_RISING && (plant.mSeedType == SeedType.SEED_SPIKEWEED || plant.mSeedType == SeedType.SEED_SPIKEROCK))
					{
						new2.mTransY -= 34;
					}
					if (plant.mPlantCol <= 4 && mBoard.StageHasRoof())
					{
						new2.mTransY += 10;
					}
					plant.Draw(new2);
					new2.PrepareForReuse();
				}
			}
			reanimation.DrawRenderGroup(g, GameConstants.RENDER_GROUP_ARMS);
		}

		public void DrawBungeeTarget(Graphics g)
		{
			if (IsOnBoard() && !mApp.IsFinalBossLevel() && mZombiePhase != ZombiePhase.PHASE_BUNGEE_HIT_OUCHY && mZombiePhase != ZombiePhase.PHASE_BUNGEE_RISING && mRelatedZombieID == null)
			{
				g.SetDrawMode(Graphics.DrawMode.DRAWMODE_NORMAL);
				ZombieDrawPosition theDrawPos = default(ZombieDrawPosition);
				GetDrawPos(ref theDrawPos);
				float num = mX + Constants.Zombie_Bungee_Target_Offset.X;
				float num2 = (float)(mY + Constants.Zombie_Bungee_Target_Offset.Y) + theDrawPos.mBodyY + theDrawPos.mImageOffsetY;
				if (mZombiePhase == ZombiePhase.PHASE_BUNGEE_DIVING || mZombiePhase == ZombiePhase.PHASE_BUNGEE_DIVING_SCREAMING)
				{
					num += TodCommon.TodAnimateCurveFloat(GameConstants.BUNGEE_ZOMBIE_HEIGHT, GameConstants.BUNGEE_ZOMBIE_HEIGHT - 400, (int)mAltitude, 30f, 0f, TodCurves.CURVE_LINEAR);
					num2 += TodCommon.TodAnimateCurveFloat(GameConstants.BUNGEE_ZOMBIE_HEIGHT, GameConstants.BUNGEE_ZOMBIE_HEIGHT - 400, (int)mAltitude, -600f, 0f, TodCurves.CURVE_LINEAR);
				}
				num2 += mAltitude;
				g.DrawImageF(AtlasResources.IMAGE_BUNGEETARGET, num * Constants.S, num2 * Constants.S);
			}
		}

		public void BungeeDie()
		{
			BungeeDropPlant();
			Plant plant = null;
			int num = -1;
			if (mBoard != null)
			{
				num = mBoard.mPlants.IndexOf(mTargetPlantID);
			}
			if (num != -1)
			{
				plant = mBoard.mPlants[num];
			}
			if (plant != null)
			{
				mBoard.mPlantsEaten++;
				plant.Die();
			}
			Zombie zombie = null;
			if (mBoard != null)
			{
				zombie = mBoard.ZombieTryToGet(mRelatedZombieID);
			}
			if (zombie != null && !zombie.mDead)
			{
				zombie.DieNoLoot(true);
			}
		}

		public void ZamboniDeath(uint theDamageFlags)
		{
			if (TodCommon.TestBit(theDamageFlags, 5))
			{
				mFlatTires = true;
				mApp.PlayFoley(FoleyType.FOLEY_TIRE_POP);
				mZombiePhase = ZombiePhase.PHASE_ZOMBIE_DYING;
				mApp.AddTodParticle(mPosX + 29f, mPosY + 114f, mRenderOrder + 1, ParticleEffect.PARTICLE_ZAMBONI_TIRE);
				mVelX = 0f;
				if (RandomNumbers.NextNumber(4) == 0 && mPosX < 600f + (float)Constants.BOARD_EXTRA_ROOM)
				{
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_wheelie2, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 10, 10f);
					mPhaseCounter = 280;
					return;
				}
				Reanimation reanimation = mApp.ReanimationTryToGet(mBodyReanimID);
				TodParticleSystem theParticleSystem = mApp.AddTodParticle(0f, 0f, 0, ParticleEffect.PARTICLE_ZAMBONI_SMOKE);
				if (theParticleSystem != null)
				{
					reanimation.AttachParticleToTrack(GlobalMembersReanimIds.ReanimTrackId_zombie_zamboni_1, ref theParticleSystem, 35f, 85f);
				}
				mPhaseCounter = 280;
				PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_wheelie1, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 10, 12f);
			}
			else
			{
				mApp.AddTodParticle(mPosX + 80f, mPosY + 60f, mRenderOrder + 1, ParticleEffect.PARTICLE_ZAMBONI_EXPLOSION);
				DieWithLoot();
				mApp.PlayFoley(FoleyType.FOLEY_EXPLOSION);
			}
		}

		public void CatapultDeath(uint theDamageFlags)
		{
			if (TodCommon.TestBit(theDamageFlags, 5))
			{
				mApp.PlayFoley(FoleyType.FOLEY_TIRE_POP);
				mZombiePhase = ZombiePhase.PHASE_ZOMBIE_DYING;
				mApp.AddTodParticle(mPosX + 29f, mPosY + 114f, mRenderOrder + 1, ParticleEffect.PARTICLE_ZAMBONI_TIRE);
				mVelX = 0f;
				AddAttachedParticle(47, 77, ParticleEffect.PARTICLE_ZAMBONI_SMOKE);
				mPhaseCounter = 280;
				PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_bounce, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 10, 12f);
			}
			else
			{
				mApp.AddTodParticle(mPosX + 80f, mPosY + 60f, mRenderOrder + 1, ParticleEffect.PARTICLE_CATAPULT_EXPLOSION);
				DieWithLoot();
				mApp.PlayFoley(FoleyType.FOLEY_EXPLOSION);
			}
		}

		public bool SetupDrawZombieWon(Graphics g)
		{
			if (mFromWave != GameConstants.ZOMBIE_WAVE_WINNER)
			{
				return true;
			}
			if (!mBoard.mCutScene.ShowZombieWalking())
			{
				return false;
			}
			if (mBoard.mBackground == BackgroundType.BACKGROUND_1_DAY || mBoard.mBackground == BackgroundType.BACKGROUND_2_NIGHT)
			{
				g.ClipRect((int)((float)(-123 - mX) * Constants.S) + Constants.Zombie_GameOver_ClipOffset_1, (int)((float)(-mY) * Constants.S), 800, 600);
			}
			else if (mBoard.mBackground == BackgroundType.BACKGROUND_3_POOL || mBoard.mBackground == BackgroundType.BACKGROUND_4_FOG)
			{
				g.ClipRect((int)((float)(-172 - mX) * Constants.S) + Constants.Zombie_GameOver_ClipOffset_2, (int)((float)(-mY) * Constants.S), 800, 600);
			}
			else if (mBoard.mBackground == BackgroundType.BACKGROUND_5_ROOF || mBoard.mBackground == BackgroundType.BACKGROUND_6_BOSS)
			{
				g.ClipRect((int)((float)(-95 - mX) * Constants.S) + Constants.Zombie_GameOver_ClipOffset_3, (int)((float)(-mY) * Constants.S), 800, 600);
			}
			return true;
		}

		public void WalkIntoHouse()
		{
			WinningZombieReachedDesiredY = false;
			GlobalMembersAttachment.AttachmentDetachCrossFadeParticleType(ref mAttachmentID, ParticleEffect.PARTICLE_ZAMBONI_SMOKE, null);
			mFromWave = GameConstants.ZOMBIE_WAVE_WINNER;
			if (mZombiePhase == ZombiePhase.PHASE_POLEVAULTER_PRE_VAULT)
			{
				mZombiePhase = ZombiePhase.PHASE_POLEVAULTER_POST_VAULT;
				StartWalkAnim(0);
			}
			if (mBoard.mBackground == BackgroundType.BACKGROUND_1_DAY || mBoard.mBackground == BackgroundType.BACKGROUND_2_NIGHT || mBoard.mBackground == BackgroundType.BACKGROUND_3_POOL || mBoard.mBackground == BackgroundType.BACKGROUND_4_FOG || mBoard.mBackground == BackgroundType.BACKGROUND_5_ROOF || mBoard.mBackground == BackgroundType.BACKGROUND_6_BOSS)
			{
				mRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_ZOMBIE, 2, 100);
				for (int i = 0; i < mBoard.mLawnMowers.Count; i++)
				{
					mBoard.mLawnMowers[i].PrepareForReuse();
				}
				mBoard.mLawnMowers.Clear();
				if (mZombiePhase == ZombiePhase.PHASE_POLEVAULTER_PRE_VAULT)
				{
					mPosX += 35f;
				}
				if (mBoard.mBackground == BackgroundType.BACKGROUND_3_POOL || mBoard.mBackground == BackgroundType.BACKGROUND_4_FOG)
				{
					ZombieType mZombieType2 = mZombieType;
					int num = 7;
				}
			}
		}

		public void UpdateZamboni()
		{
			if (mPosX > 400f && !mFlatTires)
			{
				mVelX = TodCommon.TodAnimateCurveFloat(700, 300, (int)mPosX, 0.25f, 0.05f, TodCurves.CURVE_LINEAR);
			}
			else if (mFlatTires && mVelX > 0.0005f)
			{
				mVelX -= 0.0005f;
			}
			int val = (int)mPosX + 118;
			val = ((!mBoard.StageHasRoof()) ? Math.Max(val, 25) : Math.Max(val, 500));
			if (val < mBoard.mIceMinX[mRow])
			{
				mBoard.mIceMinX[mRow] = val;
			}
			if (val < 860)
			{
				mBoard.mIceTimer[mRow] = 3000;
				if (mApp.mGameMode == GameMode.GAMEMODE_CHALLENGE_BOBSLED_BONANZA)
				{
					mBoard.mIceTimer[mRow] = int.MaxValue;
				}
			}
		}

		public void UpdateZombieChimney()
		{
		}

		public void UpdateLadder()
		{
			if (mMindControlled || !mHasHead || IsDeadOrDying())
			{
				return;
			}
			if (mZombiePhase == ZombiePhase.PHASE_LADDER_CARRYING && mZombieHeight == ZombieHeight.HEIGHT_ZOMBIE_NORMAL)
			{
				Plant plant = FindPlantTarget(ZombieAttackType.ATTACKTYPE_LADDER);
				if (plant != null)
				{
					StopEating();
					mZombiePhase = ZombiePhase.PHASE_LADDER_PLACING;
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_placeladder, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 10, 24f);
				}
			}
			else
			{
				if (mZombiePhase != ZombiePhase.PHASE_LADDER_PLACING)
				{
					return;
				}
				Reanimation reanimation = mApp.ReanimationTryToGet(mBodyReanimID);
				if (reanimation.mLoopCount > 0)
				{
					Plant plant2 = FindPlantTarget(ZombieAttackType.ATTACKTYPE_LADDER);
					if (plant2 != null)
					{
						mBoard.AddALadder(plant2.mPlantCol, plant2.mRow);
						mApp.PlaySample(Resources.SOUND_LADDER_ZOMBIE);
						mZombieHeight = ZombieHeight.HEIGHT_UP_LADDER;
						mUseLadderCol = plant2.mPlantCol;
						DetachShield();
					}
					else
					{
						mZombiePhase = ZombiePhase.PHASE_LADDER_CARRYING;
						StartWalkAnim(0);
					}
				}
			}
		}

		private void SetupReanimForLostArm(uint theDamageFlags)
		{
			if (mZombieType == ZombieType.ZOMBIE_FOOTBALL)
			{
				ReanimShowPrefix("Zombie_football_leftarm_lower", -1);
				ReanimShowPrefix("Zombie_football_leftarm_hand", -1);
			}
			else if (mZombieType == ZombieType.ZOMBIE_NEWSPAPER)
			{
				ReanimShowTrack(ref GlobalMembersReanimIds.ReanimTrackId_zombie_paper_hands, -1);
				ReanimShowTrack(ref GlobalMembersReanimIds.ReanimTrackId_zombie_paper_leftarm_lower, -1);
			}
			else if (mZombieType == ZombieType.ZOMBIE_POLEVAULTER)
			{
				ReanimShowTrack(ref GlobalMembersReanimIds.ReanimTrackId_zombie_polevaulter_outerarm_lower, -1);
				ReanimShowTrack(ref GlobalMembersReanimIds.ReanimTrackId_zombie_outerarm_hand, -1);
			}
			else if (mZombieType == ZombieType.ZOMBIE_DANCER)
			{
				ReanimShowPrefix("Zombie_disco_outerarm_lower", -1);
				ReanimShowPrefix("Zombie_disco_outerhand_point", -1);
				ReanimShowPrefix("Zombie_disco_outerhand", -1);
				ReanimShowPrefix("Zombie_disco_outerarm_upper", -1);
			}
			else if (mZombieType == ZombieType.ZOMBIE_BACKUP_DANCER)
			{
				ReanimShowPrefix("Zombie_disco_outerarm_lower", -1);
				ReanimShowPrefix("Zombie_disco_outerhand", -1);
			}
			else
			{
				ReanimShowPrefix("Zombie_outerarm_lower", -1);
				ReanimShowPrefix("Zombie_outerarm_hand", -1);
			}
			ZombieDrawPosition theDrawPos = default(ZombieDrawPosition);
			GetDrawPos(ref theDrawPos);
			float thePosX = mPosX + theDrawPos.mImageOffsetX + 45f;
			float thePosY = mPosY + theDrawPos.mImageOffsetY + theDrawPos.mBodyY + 78f;
			if (IsWalkingBackwards())
			{
				thePosX += 36f;
			}
			Reanimation reanimation = mApp.ReanimationTryToGet(mBodyReanimID);
			if (reanimation != null)
			{
				if (mZombieType == ZombieType.ZOMBIE_FOOTBALL)
				{
					GetTrackPosition(ref GlobalMembersReanimIds.ReanimTrackId_zombie_football_leftarm_hand, ref thePosX, ref thePosY);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_football_leftarm_upper, AtlasResources.IMAGE_REANIM_ZOMBIE_FOOTBALL_LEFTARM_UPPER2);
				}
				else if (mZombieType == ZombieType.ZOMBIE_NEWSPAPER)
				{
					GetTrackPosition(ref GlobalMembersReanimIds.ReanimTrackId_zombie_paper_leftarm_lower, ref thePosX, ref thePosY);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_paper_leftarm_upper, AtlasResources.IMAGE_REANIM_ZOMBIE_PAPER_LEFTARM_UPPER2);
				}
				else if (mZombieType == ZombieType.ZOMBIE_POLEVAULTER)
				{
					GetTrackPosition(ref GlobalMembersReanimIds.ReanimTrackId_zombie_polevaulter_outerarm_lower, ref thePosX, ref thePosY);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_polevaulter_outerarm_upper, AtlasResources.IMAGE_REANIM_ZOMBIE_POLEVAULTER_OUTERARM_UPPER2);
				}
				else if (mZombieType == ZombieType.ZOMBIE_BALLOON)
				{
					GetTrackPosition(ref GlobalMembersReanimIds.ReanimTrackId_zombie_outerarm_lower, ref thePosX, ref thePosY);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_outerarm_upper, AtlasResources.IMAGE_REANIM_ZOMBIE_BALLOON_OUTERARM_UPPER2);
				}
				else if (mZombieType == ZombieType.ZOMBIE_IMP)
				{
					GetTrackPosition(ref GlobalMembersReanimIds.ReanimTrackId_zombie_outerarm_lower, ref thePosX, ref thePosY);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_imp_outerarm_upper, AtlasResources.IMAGE_REANIM_ZOMBIE_IMP_ARM1_BONE);
				}
				else if (mZombieType == ZombieType.ZOMBIE_DIGGER)
				{
					GetTrackPosition(ref GlobalMembersReanimIds.ReanimTrackId_zombie_outerarm_lower, ref thePosX, ref thePosY);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_digger_outerarm_upper, AtlasResources.IMAGE_REANIM_ZOMBIE_DIGGER_OUTERARM_UPPER2);
				}
				else if (mZombieType == ZombieType.ZOMBIE_BOBSLED)
				{
					GetTrackPosition(ref GlobalMembersReanimIds.ReanimTrackId_zombie_outerarm_lower, ref thePosX, ref thePosY);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_dolphinrider_outerarm_upper, AtlasResources.IMAGE_REANIM_ZOMBIE_BOBSLED_OUTERARM_UPPER2);
				}
				else if (mZombieType == ZombieType.ZOMBIE_JACK_IN_THE_BOX)
				{
					GetTrackPosition(ref GlobalMembersReanimIds.ReanimTrackId_zombie_jackbox_outerarm_lower, ref thePosX, ref thePosY);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_jackbox_outerarm_lower, AtlasResources.IMAGE_REANIM_ZOMBIE_JACKBOX_OUTERARM_LOWER2);
				}
				else if (mZombieType == ZombieType.ZOMBIE_SNORKEL)
				{
					GetTrackPosition(ref GlobalMembersReanimIds.ReanimTrackId_zombie_outerarm_lower, ref thePosX, ref thePosY);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_snorkle_outerarm_upper, AtlasResources.IMAGE_REANIM_ZOMBIE_SNORKLE_OUTERARM_UPPER2);
				}
				else if (mZombieType == ZombieType.ZOMBIE_DOLPHIN_RIDER)
				{
					GetTrackPosition(ref GlobalMembersReanimIds.ReanimTrackId_zombie_outerarm_lower, ref thePosX, ref thePosY);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_dolphinrider_outerarm_upper, AtlasResources.IMAGE_REANIM_ZOMBIE_DOLPHINRIDER_OUTERARM_UPPER2);
				}
				else if (mZombieType == ZombieType.ZOMBIE_POGO)
				{
					GetTrackPosition(ref GlobalMembersReanimIds.ReanimTrackId_zombie_outerarm_lower, ref thePosX, ref thePosY);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_outerarm_upper, AtlasResources.IMAGE_REANIM_ZOMBIE_POGO_OUTERARM_UPPER2);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_pogo_stickhands, AtlasResources.IMAGE_REANIM_ZOMBIE_POGO_STICKHANDS2);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_pogo_stick, AtlasResources.IMAGE_REANIM_ZOMBIE_POGO_STICKDAMAGE2);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_pogo_stick2, AtlasResources.IMAGE_REANIM_ZOMBIE_POGO_STICK2DAMAGE2);
				}
				else if (mZombieType == ZombieType.ZOMBIE_FLAG)
				{
					GetTrackPosition(ref GlobalMembersReanimIds.ReanimTrackId_zombie_outerarm_lower, ref thePosX, ref thePosY);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_outerarm_upper, AtlasResources.IMAGE_REANIM_ZOMBIE_OUTERARM_UPPER2);
					Reanimation reanimation2 = mApp.ReanimationTryToGet(mSpecialHeadReanimID);
					if (reanimation2 != null)
					{
						reanimation2.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_flag, AtlasResources.IMAGE_REANIM_ZOMBIE_FLAG3);
					}
				}
				else if (mZombieType == ZombieType.ZOMBIE_DANCER)
				{
					GetTrackPosition(ref GlobalMembersReanimIds.ReanimTrackId_zombie_disco_outerhand, ref thePosX, ref thePosY);
					ReanimShowPrefix("Zombie_disco_outerarm_upper_bone", 0);
				}
				else if (mZombieType == ZombieType.ZOMBIE_BACKUP_DANCER)
				{
					GetTrackPosition(ref GlobalMembersReanimIds.ReanimTrackId_zombie_disco_outerhand, ref thePosX, ref thePosY);
					ReanimShowPrefix("Zombie_disco_outerarm_upper_bone", 0);
				}
				else if (mZombieType == ZombieType.ZOMBIE_LADDER)
				{
					GetTrackPosition(ref GlobalMembersReanimIds.ReanimTrackId_zombie_outerarm_hand, ref thePosX, ref thePosY);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_ladder_outerarm_upper, AtlasResources.IMAGE_REANIM_ZOMBIE_LADDER_OUTERARM_UPPER2);
				}
				else if (mZombieType == ZombieType.ZOMBIE_YETI)
				{
					GetTrackPosition(ref GlobalMembersReanimIds.ReanimTrackId_zombie_outerarm_hand, ref thePosX, ref thePosY);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_yeti_outerarm_upper, AtlasResources.IMAGE_REANIM_ZOMBIE_YETI_OUTERARM_UPPER2);
				}
				else
				{
					GetTrackPosition(ref GlobalMembersReanimIds.ReanimTrackId_zombie_outerarm_lower, ref thePosX, ref thePosY);
					reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_outerarm_upper, AtlasResources.IMAGE_REANIM_ZOMBIE_OUTERARM_UPPER2);
				}
			}
			if (mInPool || TodCommon.TestBit(theDamageFlags, 4))
			{
				return;
			}
			ParticleEffect theEffect = ParticleEffect.PARTICLE_ZOMBIE_ARM;
			if (mZombiePhase == ZombiePhase.PHASE_ZOMBIE_MOWERED)
			{
				theEffect = ParticleEffect.PARTICLE_MOWERED_ZOMBIE_ARM;
			}
			if (mZombiePhase == ZombiePhase.PHASE_ZOMBIE_MOWERED)
			{
				thePosX -= 40f;
				thePosY -= 50f;
			}
			TodParticleSystem todParticleSystem = mApp.AddTodParticle(thePosX, thePosY, mRenderOrder + 1, theEffect);
			OverrideParticleColor(todParticleSystem);
			OverrideParticleScale(todParticleSystem);
			if (todParticleSystem != null)
			{
				if (mZombieType == ZombieType.ZOMBIE_FOOTBALL)
				{
					todParticleSystem.OverrideImage(null, AtlasResources.IMAGE_REANIM_ZOMBIE_FOOTBALL_LEFTARM_HAND);
				}
				else if (mZombieType == ZombieType.ZOMBIE_NEWSPAPER)
				{
					todParticleSystem.OverrideImage(null, AtlasResources.IMAGE_REANIM_ZOMBIE_PAPER_LEFTARM_LOWER);
				}
				else if (mZombieType == ZombieType.ZOMBIE_DANCER)
				{
					ReanimShowPrefix("Zombie_disco_outerarm_lower", -1);
					ReanimShowPrefix("Zombie_disco_outerhand_point", -1);
					ReanimShowPrefix("Zombie_disco_outerhand", -1);
					ReanimShowPrefix("Zombie_disco_outerarm_upper", -1);
				}
				else if (mZombieType == ZombieType.ZOMBIE_BACKUP_DANCER)
				{
					ReanimShowPrefix("Zombie_disco_outerarm_lower", -1);
					ReanimShowPrefix("Zombie_disco_outerhand", -1);
				}
				else if (mZombieType == ZombieType.ZOMBIE_BOBSLED)
				{
					todParticleSystem.OverrideImage(null, AtlasResources.IMAGE_REANIM_ZOMBIE_BOBSLED_OUTERARM_HAND);
				}
				else if (mZombieType == ZombieType.ZOMBIE_IMP)
				{
					todParticleSystem.OverrideImage(null, AtlasResources.IMAGE_REANIM_ZOMBIE_IMP_ARM2);
				}
				else if (mZombieType == ZombieType.ZOMBIE_YETI)
				{
					todParticleSystem.OverrideImage(null, AtlasResources.IMAGE_REANIM_ZOMBIE_YETI_OUTERARM_HAND);
				}
				else if (mZombieType == ZombieType.ZOMBIE_JACK_IN_THE_BOX)
				{
					todParticleSystem.OverrideImage(null, AtlasResources.IMAGE_ZOMBIEJACKBOXARM);
				}
				else if (mZombieType == ZombieType.ZOMBIE_DIGGER)
				{
					todParticleSystem.OverrideImage(null, AtlasResources.IMAGE_ZOMBIEDIGGERARM);
				}
				else if (mZombieType == ZombieType.ZOMBIE_POLEVAULTER || mZombieType == ZombieType.ZOMBIE_BALLOON || mZombieType == ZombieType.ZOMBIE_DOLPHIN_RIDER || mZombieType == ZombieType.ZOMBIE_POGO || mZombieType == ZombieType.ZOMBIE_LADDER)
				{
					todParticleSystem.OverrideImage(null, AtlasResources.IMAGE_REANIM_ZOMBIE_OUTERARM_HAND);
				}
			}
		}

		public void DropArm(uint theDamageFlags)
		{
			if (CanLoseBodyParts() && mShieldType != ShieldType.SHIELDTYPE_DOOR && mShieldType != ShieldType.SHIELDTYPE_NEWSPAPER && mZombiePhase != ZombiePhase.PHASE_SNORKEL_INTO_POOL && mZombiePhase != ZombiePhase.PHASE_DOLPHIN_WALKING && mZombiePhase != ZombiePhase.PHASE_DOLPHIN_INTO_POOL && mZombiePhase != ZombiePhase.PHASE_DOLPHIN_RIDING && mZombiePhase != ZombiePhase.PHASE_DOLPHIN_IN_JUMP && mZombiePhase != ZombiePhase.PHASE_NEWSPAPER_READING && mHasArm)
			{
				mHasArm = false;
				SetupReanimForLostArm(theDamageFlags);
				mApp.PlayFoley(FoleyType.FOLEY_LIMBS_POP);
			}
		}

		public bool CanLoseBodyParts()
		{
			if (mZombieType == ZombieType.ZOMBIE_ZAMBONI || mZombieType == ZombieType.ZOMBIE_BUNGEE || mZombieType == ZombieType.ZOMBIE_CATAPULT || mZombieType == ZombieType.ZOMBIE_GARGANTUAR || mZombieType == ZombieType.ZOMBIE_REDEYE_GARGANTUAR || mZombieType == ZombieType.ZOMBIE_BOSS || mZombieHeight == ZombieHeight.HEIGHT_ZOMBIQUARIUM)
			{
				return false;
			}
			if (IsFlying())
			{
				return false;
			}
			if (IsBobsledTeamWithSled())
			{
				return false;
			}
			return true;
		}

		public void DropHelm(uint theDamageFlags)
		{
			if (mHelmType != 0)
			{
				ZombieDrawPosition theDrawPos = default(ZombieDrawPosition);
				GetDrawPos(ref theDrawPos);
				float thePosX = mPosX + theDrawPos.mImageOffsetX + (float)theDrawPos.mHeadX + 14f;
				float thePosY = mPosY + theDrawPos.mImageOffsetY + (float)theDrawPos.mHeadY + theDrawPos.mBodyY + 18f;
				ParticleEffect particleEffect = ParticleEffect.PARTICLE_NONE;
				if (mHelmType == HelmType.HELMTYPE_TRAFFIC_CONE)
				{
					GetTrackPosition(ref GlobalMembersReanimIds.ReanimTrackId_anim_cone, ref thePosX, ref thePosY);
					ReanimShowPrefix("anim_cone", -1);
					ReanimShowPrefix("anim_hair", 0);
					particleEffect = ParticleEffect.PARTICLE_ZOMBIE_TRAFFIC_CONE;
				}
				else if (mHelmType == HelmType.HELMTYPE_PAIL)
				{
					GetTrackPosition(ref GlobalMembersReanimIds.ReanimTrackId_anim_bucket, ref thePosX, ref thePosY);
					ReanimShowPrefix("anim_bucket", -1);
					ReanimShowPrefix("anim_hair", 0);
					particleEffect = ParticleEffect.PARTICLE_ZOMBIE_PAIL;
				}
				else if (mHelmType == HelmType.HELMTYPE_FOOTBALL)
				{
					GetTrackPosition(ref GlobalMembersReanimIds.ReanimTrackId_zombie_football_helmet, ref thePosX, ref thePosY);
					ReanimShowPrefix("zombie_football_helmet", -1);
					ReanimShowPrefix("anim_hair", 0);
					particleEffect = ParticleEffect.PARTICLE_ZOMBIE_HELMET;
				}
				else if (mHelmType == HelmType.HELMTYPE_DIGGER)
				{
					GetTrackPosition(ref GlobalMembersReanimIds.ReanimTrackId_zombie_digger_hardhat, ref thePosX, ref thePosY);
					ReanimShowTrack(ref GlobalMembersReanimIds.ReanimTrackId_zombie_digger_hardhat, -1);
					particleEffect = ParticleEffect.PARTICLE_ZOMBIE_HEADLIGHT;
				}
				else if (mHelmType == HelmType.HELMTYPE_BOBSLED && !TodCommon.TestBit(theDamageFlags, 4))
				{
					BobsledCrash();
				}
				if (!TodCommon.TestBit(theDamageFlags, 4) && particleEffect != ParticleEffect.PARTICLE_NONE)
				{
					TodParticleSystem aParticle = mApp.AddTodParticle(thePosX, thePosY, mRenderOrder + 1, particleEffect);
					OverrideParticleScale(aParticle);
				}
				mHasHelm = false;
				mHelmType = HelmType.HELMTYPE_NONE;
			}
		}

		public void DropShield(uint theDamageFlags)
		{
			if (mShieldType == ShieldType.SHIELDTYPE_NONE)
			{
				return;
			}
			ZombieDrawPosition theDrawPos = default(ZombieDrawPosition);
			GetDrawPos(ref theDrawPos);
			int aRenderOrder = mRenderOrder + 1;
			TodParticleSystem aParticle = null;
			if (mShieldType == ShieldType.SHIELDTYPE_DOOR)
			{
				DetachShield();
				if (!TodCommon.TestBit(theDamageFlags, 4))
				{
					float thePosX = 0f;
					float thePosY = 0f;
					GetTrackPosition(ref GlobalMembersReanimIds.ReanimTrackId_anim_screendoor, ref thePosX, ref thePosY);
					aParticle = mApp.AddTodParticle(thePosX, thePosY, aRenderOrder, ParticleEffect.PARTICLE_ZOMBIE_DOOR);
				}
			}
			else if (mShieldType == ShieldType.SHIELDTYPE_NEWSPAPER)
			{
				StopEating();
				if (mYuckyFace)
				{
					ShowYuckyFace(false);
					mYuckyFace = false;
					mYuckyFaceCounter = 0;
				}
				mZombiePhase = ZombiePhase.PHASE_NEWSPAPER_MADDENING;
				PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_gasp, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 10, 8f);
				DetachShield();
				if (!TodCommon.TestBit(theDamageFlags, 4))
				{
					float thePosX2 = 0f;
					float thePosY2 = 0f;
					GetTrackPosition(ref GlobalMembersReanimIds.ReanimTrackId_zombie_paper_paper, ref thePosX2, ref thePosY2);
					aParticle = mApp.AddTodParticle(thePosX2, thePosY2, aRenderOrder, ParticleEffect.PARTICLE_ZOMBIE_NEWSPAPER);
				}
				if (!TodCommon.TestBit(theDamageFlags, 4) && !TodCommon.TestBit(theDamageFlags, 0))
				{
					mApp.PlayFoley(FoleyType.FOLEY_NEWSPAPER_RIP);
					AddAttachedReanim(-11, 0, ReanimationType.REANIM_ZOMBIE_SURPRISE);
					mSurprised = true;
				}
			}
			else if (mShieldType == ShieldType.SHIELDTYPE_LADDER)
			{
				DetachShield();
				if (!TodCommon.TestBit(theDamageFlags, 4))
				{
					float theX = mPosX + 31f;
					float theY = mPosY + 80f;
					aParticle = mApp.AddTodParticle(theX, theY, aRenderOrder, ParticleEffect.PARTICLE_ZOMBIE_LADDER);
				}
			}
			OverrideParticleScale(aParticle);
			mHasShield = false;
			mShieldType = ShieldType.SHIELDTYPE_NONE;
		}

		public void ReanimReenableClipping()
		{
			Reanimation reanimation = mApp.ReanimationTryToGet(mBodyReanimID);
			if (reanimation != null)
			{
				for (int i = 0; i < reanimation.mDefinition.mTrackCount; i++)
				{
					ReanimatorTrackInstance reanimatorTrackInstance = reanimation.mTrackInstances[i];
					reanimatorTrackInstance.mIgnoreClipRect = false;
				}
				mUsesClipping = true;
			}
		}

		public void UpdateBoss()
		{
			Reanimation reanimation = mApp.ReanimationTryToGet(mBodyReanimID);
			if (mApp.mGameScene == GameScenes.SCENE_LEVEL_INTRO)
			{
				if (reanimation.ShouldTriggerTimedEvent(0.24f) || reanimation.ShouldTriggerTimedEvent(0.79f))
				{
					mApp.PlayFoley(FoleyType.FOLEY_THUMP);
					mBoard.ShakeBoard(1, 4);
					mApp.Vibrate();
				}
				return;
			}
			Reanimation reanimation2 = mApp.ReanimationGet(mSpecialHeadReanimID);
			UpdateBossFireball();
			if (mIceTrapCounter == 0)
			{
				if (mSummonCounter > 0)
				{
					mSummonCounter -= 3;
				}
				if (mBossBungeeCounter > 0)
				{
					mBossBungeeCounter -= 3;
				}
				if (mBossStompCounter > 0)
				{
					mBossStompCounter -= 3;
				}
				if (mBossHeadCounter > 0)
				{
					mBossHeadCounter -= 3;
				}
				if (mChilledCounter > 0)
				{
					reanimation2.mAnimRate = 6f;
				}
				else if (reanimation2.mAnimRate == 0f)
				{
					reanimation2.mAnimRate = 12f;
				}
			}
			else
			{
				reanimation2.mAnimRate = 0f;
			}
			if (mZombiePhase == ZombiePhase.PHASE_BOSS_ENTER)
			{
				BossPlayIdle();
			}
			else if (mZombiePhase == ZombiePhase.PHASE_BOSS_IDLE)
			{
				if (mBodyHealth == 1)
				{
					PlayDeathAnim(0u);
				}
				else
				{
					if (mPhaseCounter > 0)
					{
						return;
					}
					int bodyDamageIndex = GetBodyDamageIndex();
					if (bodyDamageIndex != mBossMode)
					{
						mBossMode = bodyDamageIndex;
						if (mBossMode == 1)
						{
							BossBungeeAttack();
						}
						else
						{
							BossRVAttack();
						}
					}
					else if (mBossStompCounter == 0)
					{
						BossStompAttack();
					}
					else if (mBossBungeeCounter == 0)
					{
						int ceiling = (!mApp.IsAdventureMode() && !mApp.IsQuickPlayMode()) ? 2 : 4;
						if (RandomNumbers.NextNumber(ceiling) == 0)
						{
							mBossBungeeCounter = TodCommon.RandRangeInt(4000, 5000);
							BossRVAttack();
						}
						else
						{
							BossBungeeAttack();
						}
					}
					else if (mBossHeadCounter <= 0)
					{
						BossHeadAttack();
					}
					else if (mSummonCounter <= 0)
					{
						BossSpawnAttack();
					}
					else
					{
						mPhaseCounter = TodCommon.RandRangeInt(100, 200);
					}
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_BOSS_SPAWNING)
			{
				if (reanimation.ShouldTriggerTimedEvent(0.6f))
				{
					BossSpawnContact();
				}
				if (reanimation.mLoopCount > 0)
				{
					BossPlayIdle();
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_BOSS_STOMPING)
			{
				float theEventTime = 0.5f;
				if (mTargetRow >= 2)
				{
					theEventTime = 0.55f;
				}
				if (reanimation.ShouldTriggerTimedEvent(theEventTime))
				{
					BossStompContact();
				}
				if (reanimation.mLoopCount > 0)
				{
					BossPlayIdle();
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_BOSS_BUNGEES_ENTER)
			{
				if (reanimation.ShouldTriggerTimedEvent(0.4f))
				{
					BossBungeeSpawn();
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_BOSS_BUNGEES_DROP)
			{
				if (BossAreBungeesDone())
				{
					BossBungeeLeave();
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_BOSS_BUNGEES_LEAVE)
			{
				if (reanimation.mLoopCount > 0)
				{
					BossPlayIdle();
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_BOSS_DROP_RV)
			{
				if (reanimation.ShouldTriggerTimedEvent(0.65f))
				{
					BossRVLanding();
				}
				if (reanimation.mLoopCount > 0)
				{
					BossPlayIdle();
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_BOSS_HEAD_ENTER)
			{
				if (GetBodyDamageIndex() == 2 && reanimation.ShouldTriggerTimedEvent(0.37f))
				{
					ApplyBossSmokeParticles(true);
				}
				if (reanimation.ShouldTriggerTimedEvent(0.55f))
				{
					mApp.PlayFoley(FoleyType.FOLEY_HYDRAULIC);
				}
				if (reanimation.mLoopCount > 0)
				{
					mZombiePhase = ZombiePhase.PHASE_BOSS_HEAD_IDLE_BEFORE_SPIT;
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_head_idle, ReanimLoopType.REANIM_LOOP, 0, 12f);
					mPhaseCounter = 500;
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_BOSS_HEAD_IDLE_BEFORE_SPIT)
			{
				if (mBodyHealth == 1)
				{
					BossStartDeath();
				}
				else if (mPhaseCounter <= 0)
				{
					BossHeadSpit();
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_BOSS_HEAD_SPIT)
			{
				if (reanimation.ShouldTriggerTimedEvent(0.37f))
				{
					BossHeadSpitEffect();
				}
				if (reanimation.ShouldTriggerTimedEvent(0.42f))
				{
					BossHeadSpitContact();
				}
				if (reanimation.mLoopCount > 0)
				{
					reanimation2 = mApp.ReanimationTryToGet(mSpecialHeadReanimID);
					reanimation2.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_anim_idle, ReanimLoopType.REANIM_LOOP, 20, 18f);
					mZombiePhase = ZombiePhase.PHASE_BOSS_HEAD_IDLE_AFTER_SPIT;
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_head_idle, ReanimLoopType.REANIM_LOOP, 0, 12f);
					mPhaseCounter = 300;
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_BOSS_HEAD_IDLE_AFTER_SPIT)
			{
				if (mBodyHealth == 1)
				{
					BossStartDeath();
				}
				else if (mPhaseCounter <= 0)
				{
					mZombiePhase = ZombiePhase.PHASE_BOSS_HEAD_LEAVE;
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_head_leave, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 0, 12f);
				}
			}
			else if (mZombiePhase == ZombiePhase.PHASE_BOSS_HEAD_LEAVE)
			{
				if (reanimation.ShouldTriggerTimedEvent(0.23f))
				{
					mChilledCounter = 0;
					UpdateAnimSpeed();
				}
				if (reanimation.ShouldTriggerTimedEvent(0.48f) || reanimation.ShouldTriggerTimedEvent(0.8f))
				{
					mApp.PlayFoley(FoleyType.FOLEY_THUMP);
				}
				if (reanimation.mLoopCount > 0)
				{
					ApplyBossSmokeParticles(false);
					BossPlayIdle();
				}
			}
			else
			{
				Debug.ASSERT(false);
			}
		}

		public void BossPlayIdle()
		{
			mZombiePhase = ZombiePhase.PHASE_BOSS_IDLE;
			mPhaseCounter = TodCommon.RandRangeInt(100, 200);
			PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_idle, ReanimLoopType.REANIM_LOOP, 0, 6f);
		}

		public void BossRVLanding()
		{
			int count = mBoard.mPlants.Count;
			for (int i = 0; i < count; i++)
			{
				Plant plant = mBoard.mPlants[i];
				if (!plant.mDead && plant.mRow >= mTargetRow && plant.mRow <= mTargetRow + 1 && plant.mPlantCol >= mTargetCol && plant.mPlantCol <= mTargetCol + 2)
				{
					plant.Squish();
				}
			}
			mBoard.ShakeBoard(1, 2);
			mApp.PlaySample(Resources.SOUND_RVTHROW);
			mApp.Vibrate();
			mSummonCounter = 500;
			mBossHeadCounter = 5000;
			if (mBossMode >= 1)
			{
				mBossStompCounter = 4000;
			}
			if (mBossMode >= 2)
			{
				mBossBungeeCounter = 6500;
			}
		}

		public void BossStompContact()
		{
			int count = mBoard.mPlants.Count;
			for (int i = 0; i < count; i++)
			{
				Plant plant = mBoard.mPlants[i];
				if (!plant.mDead && plant.mRow >= mTargetRow && plant.mRow <= mTargetRow + 1 && plant.mPlantCol >= 5)
				{
					plant.Squish();
				}
			}
			mBoard.ShakeBoard(1, 4);
			mApp.PlayFoley(FoleyType.FOLEY_THUMP);
			mApp.Vibrate();
		}

		public bool BossAreBungeesDone()
		{
			int num = 0;
			for (int i = 0; i < 3; i++)
			{
				Zombie zombie = mBoard.ZombieTryToGet(mFollowerZombieID[i]);
				if (zombie != null)
				{
					if (zombie.mZombiePhase == ZombiePhase.PHASE_BUNGEE_RISING)
					{
						return true;
					}
					num++;
				}
			}
			if (num == 0)
			{
				return true;
			}
			return false;
		}

		public void BossBungeeSpawn()
		{
			mZombiePhase = ZombiePhase.PHASE_BOSS_BUNGEES_DROP;
			for (int i = 0; i < 3; i++)
			{
				Zombie zombie = mBoard.AddZombieInRow(ZombieType.ZOMBIE_BUNGEE, 0, 0);
				zombie.PickBungeeZombieTarget(mTargetCol + i);
				zombie.mRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_GRAVE_STONE, zombie.mRow, 7);
				zombie.mAltitude = zombie.mPosY - 30f;
				mFollowerZombieID[i] = mBoard.ZombieGetID(zombie);
			}
		}

		public void BossSpawnAttack()
		{
			RemoveColdEffects();
			mZombiePhase = ZombiePhase.PHASE_BOSS_SPAWNING;
			if (mBossMode == 0)
			{
				mSummonCounter = TodCommon.RandRangeInt(450, 550);
			}
			else if (mBossMode == 1)
			{
				mSummonCounter = TodCommon.RandRangeInt(350, 450);
			}
			else if (mBossMode == 2)
			{
				mSummonCounter = TodCommon.RandRangeInt(150, 250);
			}
			mTargetRow = mBoard.PickRowForNewZombie(ZombieType.ZOMBIE_NORMAL);
			string theTrackName = string.Empty;
			switch (mTargetRow)
			{
			case 0:
				theTrackName = GlobalMembersReanimIds.ReanimTrackId_anim_spawn_1;
				break;
			case 1:
				theTrackName = GlobalMembersReanimIds.ReanimTrackId_anim_spawn_2;
				break;
			case 2:
				theTrackName = GlobalMembersReanimIds.ReanimTrackId_anim_spawn_3;
				break;
			case 3:
				theTrackName = GlobalMembersReanimIds.ReanimTrackId_anim_spawn_4;
				break;
			case 4:
				theTrackName = GlobalMembersReanimIds.ReanimTrackId_anim_spawn_5;
				break;
			default:
				Debug.ASSERT(false);
				break;
			}
			PlayZombieReanim(ref theTrackName, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 20, 12f);
			mApp.PlayFoley(FoleyType.FOLEY_HYDRAULIC_SHORT);
		}

		public void BossBungeeAttack()
		{
			RemoveColdEffects();
			mZombiePhase = ZombiePhase.PHASE_BOSS_BUNGEES_ENTER;
			mBossBungeeCounter = TodCommon.RandRangeInt(4000, 5000);
			mTargetCol = TodCommon.RandRangeInt(0, 2);
			PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_bungee_1_enter, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 20, 12f);
			mApp.PlayFoley(FoleyType.FOLEY_HYDRAULIC_SHORT);
			mApp.PlayFoley(FoleyType.FOLEY_BUNGEE_SCREAM);
		}

		public void BossRVAttack()
		{
			RemoveColdEffects();
			mZombiePhase = ZombiePhase.PHASE_BOSS_DROP_RV;
			mTargetRow = TodCommon.RandRangeInt(0, 3);
			mTargetCol = TodCommon.RandRangeInt(0, 2);
			PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_rv_1, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 20, 16f);
			mApp.PlayFoley(FoleyType.FOLEY_HYDRAULIC_SHORT);
		}

		public void BossSpawnContact()
		{
			ZombieType theZombieType;
			if (mZombieAge < 3500)
			{
				theZombieType = ZombieType.ZOMBIE_NORMAL;
			}
			else if (mZombieAge < 8000)
			{
				theZombieType = ZombieType.ZOMBIE_TRAFFIC_CONE;
			}
			else if (mZombieAge < 12500)
			{
				theZombieType = ZombieType.ZOMBIE_PAIL;
			}
			else
			{
				int num = GameConstants.gBossZombieList.Length;
				if (mTargetRow == 0)
				{
					Debug.ASSERT(GameConstants.gBossZombieList[num - 1] == ZombieType.ZOMBIE_GARGANTUAR);
					num--;
				}
				theZombieType = GameConstants.gBossZombieList[RandomNumbers.NextNumber(num)];
			}
			Zombie zombie = mBoard.AddZombieInRow(theZombieType, mTargetRow, 0);
			zombie.mPosX = 600f + mPosX;
		}

		public void BossBungeeLeave()
		{
			mZombiePhase = ZombiePhase.PHASE_BOSS_BUNGEES_LEAVE;
			for (int i = 0; i < 3; i++)
			{
				Zombie zombie = mBoard.ZombieTryToGet(mFollowerZombieID[i]);
				if (zombie != null && zombie.mButteredCounter > 0)
				{
					zombie.DieWithLoot();
				}
			}
			PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_bungee_1_leave, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 20, 18f);
		}

		public void BossStompAttack()
		{
			RemoveColdEffects();
			mZombiePhase = ZombiePhase.PHASE_BOSS_STOMPING;
			mBossStompCounter = TodCommon.RandRangeInt(5500, 6500);
			int num = 0;
			int[] array = new int[4];
			for (int i = 0; i < 4; i++)
			{
				if (BossCanStompRow(i))
				{
					array[num] = i;
					num++;
				}
			}
			if (num != 0)
			{
				mTargetRow = TodCommon.TodPickFromArray(array, num);
				string theTrackName = string.Empty;
				switch (mTargetRow)
				{
				case 0:
					theTrackName = GlobalMembersReanimIds.ReanimTrackId_anim_stomp_1;
					break;
				case 1:
					theTrackName = GlobalMembersReanimIds.ReanimTrackId_anim_stomp_2;
					break;
				case 2:
					theTrackName = GlobalMembersReanimIds.ReanimTrackId_anim_stomp_3;
					break;
				case 3:
					theTrackName = GlobalMembersReanimIds.ReanimTrackId_anim_stomp_4;
					break;
				default:
					Debug.ASSERT(false);
					break;
				}
				PlayZombieReanim(ref theTrackName, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 20, 12f);
				mApp.PlayFoley(FoleyType.FOLEY_HYDRAULIC_SHORT);
			}
		}

		public bool BossCanStompRow(int theRow)
		{
			int count = mBoard.mPlants.Count;
			for (int i = 0; i < count; i++)
			{
				Plant plant = mBoard.mPlants[i];
				if (!plant.mDead && !plant.NotOnGround() && plant.mRow >= theRow && plant.mRow <= theRow + 1 && plant.mPlantCol >= 5)
				{
					return true;
				}
			}
			return false;
		}

		public void BossDie()
		{
			if (!IsOnBoard())
			{
				return;
			}
			Reanimation reanimation = mApp.ReanimationTryToGet(mBossFireBallReanimID);
			if (reanimation != null)
			{
				reanimation.ReanimationDie();
				mBossFireBallReanimID = null;
				BossDestroyIceballInRow(mFireballRow);
				BossDestroyFireball();
			}
			mApp.mMusic.FadeOut(200);
			int count = mBoard.mZombies.Count;
			for (int i = 0; i < count; i++)
			{
				Zombie zombie = mBoard.mZombies[i];
				if (!zombie.mDead && zombie != this && !zombie.IsDeadOrDying())
				{
					zombie.DieWithLoot();
				}
			}
			RemoveColdEffects();
		}

		public void BossHeadAttack()
		{
			mZombiePhase = ZombiePhase.PHASE_BOSS_HEAD_ENTER;
			mBossHeadCounter = TodCommon.RandRangeInt(4000, 5000);
			PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_head_enter, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 20, 12f);
			mApp.PlayFoley(FoleyType.FOLEY_HYDRAULIC_SHORT);
		}

		public void BossHeadSpitContact()
		{
			Debug.ASSERT(mApp.ReanimationTryToGet(mBossFireBallReanimID) == null);
			float num = 550f + mPosX;
			float theY = mBoard.GetPosYBasedOnRow(num, mFireballRow) + GameConstants.BOSS_BALL_OFFSET_Y;
			int aRenderOrder = mRenderOrder + 1;
			Reanimation reanimation;
			if (mIsFireBall)
			{
				num -= 95f;
				reanimation = mApp.AddReanimation(num, theY, aRenderOrder, ReanimationType.REANIM_BOSS_FIREBALL);
				reanimation.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_anim_form, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 0, 16f);
				reanimation.mIsAttachment = true;
				reanimation.AssignRenderGroupToTrack(GlobalMembersReanimIds.ReanimTrackId_additive, GameConstants.RENDER_GROUP_BOSS_FIREBALL_ADDITIVE);
				reanimation.AssignRenderGroupToTrack(GlobalMembersReanimIds.ReanimTrackId_superglow, GameConstants.RENDER_GROUP_BOSS_FIREBALL_ADDITIVE);
			}
			else
			{
				num -= 95f;
				reanimation = mApp.AddReanimation(num, theY, aRenderOrder, ReanimationType.REANIM_BOSS_ICEBALL);
				reanimation.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_anim_form, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 0, 16f);
				reanimation.mIsAttachment = true;
				reanimation.AssignRenderGroupToTrack(GlobalMembersReanimIds.ReanimTrackId_ice_highlight, GameConstants.RENDER_GROUP_BOSS_FIREBALL_ADDITIVE);
			}
			mBossFireBallReanimID = mApp.ReanimationGetID(reanimation);
			Reanimation reanimation2 = mApp.ReanimationTryToGet(mSpecialHeadReanimID);
			reanimation2.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_anim_laugh, ReanimLoopType.REANIM_LOOP, 20, 18f);
			mApp.PlayFoley(FoleyType.FOLEY_HYDRAULIC_SHORT);
		}

		public void BossHeadSpit()
		{
			Reanimation reanimation = mApp.ReanimationTryToGet(mBossFireBallReanimID);
			if (reanimation != null)
			{
				reanimation.ReanimationDie();
				mBossFireBallReanimID = null;
			}
			mZombiePhase = ZombiePhase.PHASE_BOSS_HEAD_SPIT;
			mFireballRow = TodCommon.RandRangeInt(0, 4);
			mIsFireBall = (TodCommon.RandRangeInt(0, 1) == 0);
			string theTrackName = string.Empty;
			switch (mFireballRow)
			{
			case 0:
				theTrackName = GlobalMembersReanimIds.ReanimTrackId_anim_head_attack_1;
				break;
			case 1:
				theTrackName = GlobalMembersReanimIds.ReanimTrackId_anim_head_attack_2;
				break;
			case 2:
				theTrackName = GlobalMembersReanimIds.ReanimTrackId_anim_head_attack_3;
				break;
			case 3:
				theTrackName = GlobalMembersReanimIds.ReanimTrackId_anim_head_attack_4;
				break;
			case 4:
				theTrackName = GlobalMembersReanimIds.ReanimTrackId_anim_head_attack_5;
				break;
			default:
				Debug.ASSERT(false);
				break;
			}
			PlayZombieReanim(ref theTrackName, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 20, 12f);
			Reanimation reanimation2 = mApp.ReanimationGet(mBodyReanimID);
			Image theImage = null;
			if (mIsFireBall)
			{
				reanimation2.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_boss_eyeglow_red, theImage);
				reanimation2.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_boss_mouthglow_red, theImage);
			}
			else
			{
				reanimation2.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_boss_eyeglow_red, AtlasResources.IMAGE_REANIM_ZOMBIE_BOSS_EYEGLOW_BLUE);
				reanimation2.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_boss_mouthglow_red, AtlasResources.IMAGE_REANIM_ZOMBIE_BOSS_MOUTHGLOW_BLUE);
			}
			Reanimation reanimation3 = mApp.ReanimationTryToGet(mSpecialHeadReanimID);
			reanimation3.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_anim_drive, ReanimLoopType.REANIM_LOOP, 20, 36f);
		}

		public void UpdateBossFireball()
		{
			Reanimation reanimation = mApp.ReanimationTryToGet(mBossFireBallReanimID);
			if (reanimation == null)
			{
				return;
			}
			float trackVelocity = reanimation.GetTrackVelocity(Reanimation.ReanimTrackId__ground);
			float num = reanimation.mOverlayMatrix.mMatrix.M41 * Constants.IS;
			num -= trackVelocity;
			reanimation.mOverlayMatrix.mMatrix.M41 = num * Constants.S;
			float num2 = mBoard.GetPosYBasedOnRow(num + 75f, mFireballRow) + GameConstants.BOSS_BALL_OFFSET_Y;
			reanimation.mOverlayMatrix.mMatrix.M42 = num2 * Constants.S;
			if (num < -180f + (float)Constants.BOARD_EXTRA_ROOM)
			{
				reanimation.ReanimationDie();
				mBossFireBallReanimID = null;
			}
			int theX = mBoard.PixelToGridX((int)num + 75, (int)num2);
			SquishAllInSquare(theX, mFireballRow, ZombieAttackType.ATTACKTYPE_DRIVE_OVER);
			List<LawnMower>.Enumerator enumerator = mBoard.mLawnMowers.GetEnumerator();
			while (enumerator.MoveNext())
			{
				LawnMower current = enumerator.Current;
				if (!current.mDead && current.mMowerState != LawnMowerState.MOWER_SQUISHED && current.mRow == mFireballRow && current.mPosX > num && current.mPosX < num + 50f)
				{
					current.SquishMower();
				}
			}
			if (mIsFireBall)
			{
				if (reanimation.mLoopType == ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD && reanimation.mLoopCount > 0)
				{
					reanimation.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_anim_role, ReanimLoopType.REANIM_LOOP, 0, 2f);
					reanimation.mRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_PARTICLE, mFireballRow, 0);
				}
				if (reanimation.mLoopType == ReanimLoopType.REANIM_LOOP && RandomNumbers.NextNumber(10) == 0)
				{
					float num3 = num + 100f + TodCommon.RandRangeFloat(0f, 20f);
					float theY = mBoard.GetPosYBasedOnRow(num3 - 40f, mFireballRow) + 90f + TodCommon.RandRangeFloat(-50f, 0f);
					int aRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_GRAVE_STONE, mFireballRow, 6);
					mApp.AddTodParticle(num3, theY, aRenderOrder, ParticleEffect.PARTICLE_FIREBALL_TRAIL);
				}
			}
			else
			{
				if (reanimation.mLoopType == ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD && reanimation.mLoopCount > 0)
				{
					reanimation.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_anim_role, ReanimLoopType.REANIM_LOOP, 0, 2f);
					reanimation.mRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_PARTICLE, mFireballRow, 0);
				}
				if (reanimation.mLoopType == ReanimLoopType.REANIM_LOOP && RandomNumbers.NextNumber(10) == 0)
				{
					float num4 = num + 100f + TodCommon.RandRangeFloat(0f, 20f);
					float theY2 = mBoard.GetPosYBasedOnRow(num4 - 40f, mFireballRow) + 90f + TodCommon.RandRangeFloat(-50f, 0f);
					int aRenderOrder2 = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_GRAVE_STONE, mFireballRow, 6);
					mApp.AddTodParticle(num4, theY2, aRenderOrder2, ParticleEffect.PARTICLE_ICEBALL_TRAIL);
				}
			}
			reanimation.Update();
		}

		public void BossDestroyFireball()
		{
			Reanimation reanimation = mApp.ReanimationTryToGet(mBossFireBallReanimID);
			if (reanimation != null && mIsFireBall)
			{
				float num = reanimation.mOverlayMatrix.mMatrix.M41 * Constants.IS + 80f;
				float num2 = reanimation.mOverlayMatrix.mMatrix.M42 * Constants.IS + 40f;
				for (int i = 0; i < 6; i++)
				{
					float num3 = (float)Math.PI / 2f + (float)Math.PI * 2f * (float)i / 6f;
					float theX = num + 60f * (float)Math.Sin(num3);
					float theY = num2 + 60f * (float)Math.Cos(num3);
					Reanimation reanimation2 = mApp.AddReanimation(theX, theY, 400000, ReanimationType.REANIM_JALAPENO_FIRE);
					reanimation2.mLoopType = ReanimLoopType.REANIM_PLAY_ONCE_FULL_LAST_FRAME;
					reanimation2.mAnimTime = 0.2f;
					reanimation2.mAnimRate = TodCommon.RandRangeFloat(20f, 25f);
				}
				reanimation.ReanimationDie();
				mBossFireBallReanimID = null;
				mBoard.RemoveParticleByType(ParticleEffect.PARTICLE_FIREBALL_TRAIL);
			}
		}

		public void BossDestroyIceballInRow(int theRow)
		{
			if (theRow == mFireballRow)
			{
				Reanimation reanimation = mApp.ReanimationTryToGet(mBossFireBallReanimID);
				if (reanimation != null && !mIsFireBall)
				{
					float theX = reanimation.mOverlayMatrix.mMatrix.M41 * Constants.IS + 80f;
					float theY = reanimation.mOverlayMatrix.mMatrix.M42 * Constants.IS + 80f;
					mApp.AddTodParticle(theX, theY, 400000, ParticleEffect.PARTICLE_ICEBALL_DEATH);
					reanimation.ReanimationDie();
					mBossFireBallReanimID = null;
					mBoard.RemoveParticleByType(ParticleEffect.PARTICLE_ICEBALL_TRAIL);
				}
			}
		}

		public void DiggerLoseAxe()
		{
			if (mZombiePhase == ZombiePhase.PHASE_DIGGER_TUNNELING)
			{
				mZombiePhase = ZombiePhase.PHASE_DIGGER_TUNNELING_PAUSE_WITHOUT_AXE;
				mPhaseCounter = 200;
				SetAnimRate(0f);
				UpdateAnimSpeed();
				GlobalMembersAttachment.AttachmentDetachCrossFadeParticleType(ref mAttachmentID, ParticleEffect.PARTICLE_DIGGER_TUNNEL, null);
				StopZombieSound();
			}
			mHasObject = false;
			ReanimShowTrack(ref GlobalMembersReanimIds.ReanimTrackId_zombie_digger_pickaxe, -1);
			ReanimShowTrack(ref GlobalMembersReanimIds.ReanimTrackId_zombie_digger_dirt, -1);
		}

		public void BungeeDropZombie(Zombie theDroppedZombie, int theGridX, int theGridY)
		{
			mTargetCol = theGridX;
			SetRow(theGridY);
			mPosX = mBoard.GridToPixelX(mTargetCol, mRow);
			mPosY = GetPosYBasedOnRow(mRow);
			PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_raise, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 0, 36f);
			mRelatedZombieID = mBoard.ZombieGetID(theDroppedZombie);
			theDroppedZombie.mPosX = mPosX - 15f;
			theDroppedZombie.SetRow(theGridY);
			theDroppedZombie.mPosY = GetPosYBasedOnRow(theGridY);
			theDroppedZombie.mZombieHeight = ZombieHeight.HEIGHT_GETTING_BUNGEE_DROPPED;
			theDroppedZombie.PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_idle, ReanimLoopType.REANIM_LOOP, 0, 0f);
			theDroppedZombie.mRenderOrder = mRenderOrder + 1;
		}

		private Image GetYuckyFaceImage()
		{
			if (mZombieType == ZombieType.ZOMBIE_DANCER)
			{
				return AtlasResources.IMAGE_REANIM_ZOMBIE_DISCO_HEAD_GROSSOUT;
			}
			if (mZombieType == ZombieType.ZOMBIE_BACKUP_DANCER)
			{
				return AtlasResources.IMAGE_REANIM_ZOMBIE_DANCER_HEAD_GROSSOUT;
			}
			if (mZombieType == ZombieType.ZOMBIE_POLEVAULTER)
			{
				return AtlasResources.IMAGE_REANIM_ZOMBIE_POLEVAULTER_HEAD_GROSSOUT;
			}
			return AtlasResources.IMAGE_REANIM_ZOMBIE_HEAD_GROSSOUT;
		}

		public void ShowYuckyFace(bool theShow)
		{
			Reanimation reanimation = mApp.ReanimationTryToGet(mBodyReanimID);
			if (reanimation == null || !HasYuckyFaceImage())
			{
				return;
			}
			if (theShow)
			{
				Image yuckyFaceImage = GetYuckyFaceImage();
				reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_anim_head1, yuckyFaceImage);
				reanimation.AssignRenderGroupToTrack(GlobalMembersReanimIds.ReanimTrackId_anim_head2, -1);
				reanimation.AssignRenderGroupToTrack(GlobalMembersReanimIds.ReanimTrackId_anim_head_jaw, -1);
				reanimation.AssignRenderGroupToTrack(GlobalMembersReanimIds.ReanimTrackId_anim_tongue, -1);
			}
			else if (mHasHead)
			{
				Image theImage = null;
				reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_anim_head1, theImage);
				reanimation.AssignRenderGroupToTrack(GlobalMembersReanimIds.ReanimTrackId_anim_head2, 0);
				reanimation.AssignRenderGroupToTrack(GlobalMembersReanimIds.ReanimTrackId_anim_head_jaw, 0);
				if (mVariant)
				{
					reanimation.AssignRenderGroupToTrack(GlobalMembersReanimIds.ReanimTrackId_anim_tongue, 0);
				}
			}
		}

		public void AnimateChewSound()
		{
			if (mZombiePhase == ZombiePhase.PHASE_SNORKEL_UP_TO_EAT)
			{
				return;
			}
			Plant plant = FindPlantTarget(ZombieAttackType.ATTACKTYPE_CHEW);
			if (plant != null)
			{
				if (plant.mSeedType == SeedType.SEED_HYPNOSHROOM && !plant.mIsAsleep)
				{
					mApp.PlayFoley(FoleyType.FOLEY_FLOOP);
					plant.Die();
					StartMindControlled();
					mApp.AddTodParticle(mPosX + 60f, mPosY + 40f, mRenderOrder + 1, ParticleEffect.PARTICLE_MIND_CONTROL);
					TrySpawnLevelAward();
					mVelX = 0.17f;
					mAnimTicksPerFrame = 18;
					UpdateAnimSpeed();
				}
				else if (plant.mSeedType == SeedType.SEED_GARLIC)
				{
					if (!mYuckyFace)
					{
						mYuckyFace = true;
						mYuckyFaceCounter = 0;
						UpdateAnimSpeed();
						mApp.PlayFoley(FoleyType.FOLEY_CHOMP);
					}
				}
				else if (plant.mSeedType == SeedType.SEED_WALLNUT || plant.mSeedType == SeedType.SEED_TALLNUT || plant.mSeedType == SeedType.SEED_GARLIC || plant.mSeedType == SeedType.SEED_PUMPKINSHELL)
				{
					mApp.PlayFoley(FoleyType.FOLEY_CHOMP_SOFT);
				}
				else
				{
					mApp.PlayFoley(FoleyType.FOLEY_CHOMP);
				}
			}
			else if (mMindControlled)
			{
				mApp.PlayFoley(FoleyType.FOLEY_CHOMP_SOFT);
			}
			else
			{
				mApp.PlayFoley(FoleyType.FOLEY_CHOMP);
			}
		}

		public void AnimateChewEffect()
		{
			if (mZombiePhase == ZombiePhase.PHASE_SNORKEL_UP_TO_EAT)
			{
				return;
			}
			if (mApp.IsIZombieLevel())
			{
				GridItem gridItem = mBoard.mChallenge.IZombieGetBrainTarget(this);
				if (gridItem != null)
				{
					gridItem.mTransparentCounter = Math.Max(gridItem.mTransparentCounter, 25);
					return;
				}
			}
			Plant plant = FindPlantTarget(ZombieAttackType.ATTACKTYPE_CHEW);
			if (plant == null)
			{
				return;
			}
			if (plant.mSeedType == SeedType.SEED_WALLNUT || plant.mSeedType == SeedType.SEED_TALLNUT)
			{
				int aRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_PROJECTILE, mRow, 0);
				ZombieDrawPosition theDrawPos = default(ZombieDrawPosition);
				GetDrawPos(ref theDrawPos);
				float num = mPosX + 37f;
				float num2 = mPosY + 40f + theDrawPos.mBodyY;
				if (mZombieType == ZombieType.ZOMBIE_SNORKEL || mZombieType == ZombieType.ZOMBIE_DOLPHIN_RIDER)
				{
					num -= 7f;
					num2 += 70f;
				}
				else if (IsWalkingBackwards())
				{
					num += 47f;
				}
				else if (mZombieType == ZombieType.ZOMBIE_BALLOON)
				{
					num2 += 47f;
				}
				else if (mZombieType == ZombieType.ZOMBIE_IMP)
				{
					num += 24f;
					num2 += 40f;
				}
				mApp.AddTodParticle(num, num2, aRenderOrder, ParticleEffect.PARTICLE_WALLNUT_EAT_SMALL);
			}
			plant.mEatenFlashCountdown = Math.Max(plant.mEatenFlashCountdown, 25);
		}

		public void UpdateActions()
		{
			if (mZombieHeight == ZombieHeight.HEIGHT_UP_LADDER)
			{
				UpdateClimbingLadder();
				UpdateClimbingLadder();
				UpdateClimbingLadder();
			}
			if (mZombieHeight == ZombieHeight.HEIGHT_ZOMBIQUARIUM)
			{
				UpdateZombiquarium();
			}
			if (mZombieHeight == ZombieHeight.HEIGHT_OUT_OF_POOL || mZombieHeight == ZombieHeight.HEIGHT_IN_TO_POOL || mInPool)
			{
				UpdateZombiePool();
			}
			if (mZombieHeight == ZombieHeight.HEIGHT_UP_TO_HIGH_GROUND || mZombieHeight == ZombieHeight.HEIGHT_DOWN_OFF_HIGH_GROUND)
			{
				UpdateZombieHighGround();
			}
			if (mZombieHeight == ZombieHeight.HEIGHT_FALLING)
			{
				UpdateZombieFalling();
			}
			if (mZombieHeight == ZombieHeight.HEIGHT_IN_TO_CHIMNEY)
			{
				UpdateZombieChimney();
			}
			if (mZombieType == ZombieType.ZOMBIE_POLEVAULTER)
			{
				UpdateZombiePolevaulter();
			}
			if (mZombieType == ZombieType.ZOMBIE_CATAPULT)
			{
				UpdateZombieCatapult();
			}
			if (mZombieType == ZombieType.ZOMBIE_DOLPHIN_RIDER)
			{
				UpdateZombieDolphinRider();
			}
			if (mZombieType == ZombieType.ZOMBIE_SNORKEL)
			{
				UpdateZombieSnorkel();
			}
			if (mZombieType == ZombieType.ZOMBIE_BALLOON)
			{
				UpdateZombieFlyer();
			}
			if (mZombieType == ZombieType.ZOMBIE_NEWSPAPER)
			{
				UpdateZombieNewspaper();
			}
			if (mZombieType == ZombieType.ZOMBIE_DIGGER)
			{
				UpdateZombieDigger();
			}
			if (mZombieType == ZombieType.ZOMBIE_JACK_IN_THE_BOX)
			{
				UpdateZombieJackInTheBox();
			}
			if (mZombieType == ZombieType.ZOMBIE_GARGANTUAR || mZombieType == ZombieType.ZOMBIE_REDEYE_GARGANTUAR)
			{
				UpdateZombieGargantuar();
			}
			if (mZombieType == ZombieType.ZOMBIE_BOBSLED)
			{
				UpdateZombieBobsled();
			}
			if (mZombieType == ZombieType.ZOMBIE_ZAMBONI)
			{
				UpdateZamboni();
			}
			if (mZombieType == ZombieType.ZOMBIE_LADDER)
			{
				UpdateLadder();
			}
			if (mZombieType == ZombieType.ZOMBIE_YETI)
			{
				UpdateYeti();
			}
			if (mZombieType == ZombieType.ZOMBIE_DANCER)
			{
				UpdateZombieDancer();
				UpdateZombieDancer();
				UpdateZombieDancer();
			}
			if (mZombieType == ZombieType.ZOMBIE_BACKUP_DANCER)
			{
				UpdateZombieBackupDancer();
				UpdateZombieBackupDancer();
				UpdateZombieBackupDancer();
			}
			if (mZombieType == ZombieType.ZOMBIE_IMP)
			{
				UpdateZombieImp();
				UpdateZombieImp();
				UpdateZombieImp();
			}
			if (mZombieType == ZombieType.ZOMBIE_PEA_HEAD)
			{
				UpdateZombiePeaHead();
			}
			if (mZombieType == ZombieType.ZOMBIE_JALAPENO_HEAD)
			{
				UpdateZombieJalapenoHead();
			}
			if (mZombieType == ZombieType.ZOMBIE_GATLING_HEAD)
			{
				UpdateZombieGatlingHead();
			}
			if (mZombieType == ZombieType.ZOMBIE_SQUASH_HEAD)
			{
				UpdateZombieSquashHead();
			}
		}

		public void CheckForBoardEdge()
		{
			if (mZombieType == ZombieType.ZOMBIE_BOSS)
			{
				return;
			}
			if (IsWalkingBackwards() && mPosX > 900f)
			{
				DieNoLoot(false);
				return;
			}
			int bOARD_EDGE = Constants.BOARD_EDGE;
			if (mZombieType == ZombieType.ZOMBIE_GARGANTUAR || mZombieType == ZombieType.ZOMBIE_REDEYE_GARGANTUAR)
			{
				bOARD_EDGE = Constants.BOARD_EDGE;
			}
			else if (mZombieType == ZombieType.ZOMBIE_POLEVAULTER)
			{
				bOARD_EDGE = Constants.BOARD_EDGE;
			}
			else if (mZombieType == ZombieType.ZOMBIE_CATAPULT || mZombieType == ZombieType.ZOMBIE_FOOTBALL || mZombieType == ZombieType.ZOMBIE_ZAMBONI)
			{
				bOARD_EDGE = Constants.BOARD_EDGE;
			}
			else if (mZombieType == ZombieType.ZOMBIE_ZAMBONI)
			{
				bOARD_EDGE = Constants.BOARD_EDGE;
			}
			else if (mZombieType == ZombieType.ZOMBIE_BACKUP_DANCER || mZombieType == ZombieType.ZOMBIE_DANCER || mZombieType == ZombieType.ZOMBIE_SNORKEL)
			{
				bOARD_EDGE = Constants.BOARD_EDGE;
			}
			if (mX <= bOARD_EDGE && mHasHead)
			{
				if (mApp.IsIZombieLevel())
				{
					DieNoLoot(false);
				}
				else
				{
					mBoard.ZombiesWon(this);
				}
			}
			if (mX <= bOARD_EDGE + 70 && !mHasHead)
			{
				TakeDamage(1800, 9u);
			}
		}

		public void UpdateYeti()
		{
			if (!mMindControlled && mHasHead && !IsDeadOrDying() && mZombiePhase == ZombiePhase.PHASE_ZOMBIE_NORMAL && mPhaseCounter <= 0)
			{
				mZombiePhase = ZombiePhase.PHASE_YETI_RUNNING;
				mHasObject = false;
				PickRandomSpeed();
			}
		}

		public void DrawBossPart(Graphics g, BossPart theBossPart)
		{
			ZombieDrawPosition theDrawPos = default(ZombieDrawPosition);
			GetDrawPos(ref theDrawPos);
			switch (theBossPart)
			{
			case BossPart.BOSS_PART_BACK_LEG:
				DrawReanim(g, ref theDrawPos, GameConstants.RENDER_GROUP_BOSS_BACK_LEG);
				break;
			case BossPart.BOSS_PART_FRONT_LEG:
				DrawReanim(g, ref theDrawPos, GameConstants.RENDER_GROUP_BOSS_FRONT_LEG);
				break;
			case BossPart.BOSS_PART_MAIN:
				DrawReanim(g, ref theDrawPos, 0);
				break;
			case BossPart.BOSS_PART_BACK_ARM:
				DrawBossBackArm(g, ref theDrawPos);
				break;
			case BossPart.BOSS_PART_FIREBALL:
				DrawBossFireBall(g, ref theDrawPos);
				break;
			}
		}

		public void BossSetupReanim()
		{
			Reanimation reanimation = mApp.ReanimationGet(mBodyReanimID);
			reanimation.AssignRenderGroupToPrefix("Boss_innerleg", GameConstants.RENDER_GROUP_BOSS_BACK_LEG);
			reanimation.AssignRenderGroupToPrefix("Boss_outerleg", GameConstants.RENDER_GROUP_BOSS_FRONT_LEG);
			reanimation.AssignRenderGroupToPrefix("Boss_body2", GameConstants.RENDER_GROUP_BOSS_FRONT_LEG);
			reanimation.AssignRenderGroupToPrefix("Boss_innerarm", GameConstants.RENDER_GROUP_BOSS_BACK_ARM);
			reanimation.AssignRenderGroupToPrefix("Boss_RV", GameConstants.RENDER_GROUP_BOSS_BACK_ARM);
			Reanimation reanimation2 = mApp.AddReanimation(0f, 0f, 0, ReanimationType.REANIM_BOSS_DRIVER);
			reanimation2.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_anim_idle, ReanimLoopType.REANIM_LOOP, 0, 12f);
			mSpecialHeadReanimID = mApp.ReanimationGetID(reanimation2);
			ReanimatorTrackInstance trackInstanceByName = reanimation.GetTrackInstanceByName(GlobalMembersReanimIds.ReanimTrackId_boss_head2);
			AttachEffect attachEffect = GlobalMembersAttachment.AttachReanim(ref trackInstanceByName.mAttachmentID, reanimation2, 25f * Constants.S, -70f * Constants.S);
			reanimation.mFrameBasePose = 0;
			attachEffect.mDontDrawIfParentHidden = true;
			attachEffect.mOffset.Scale(1.2f, 1.2f);
		}

		public void MowDown()
		{
			if (mDead || mZombiePhase == ZombiePhase.PHASE_ZOMBIE_MOWERED || mZombieType == ZombieType.ZOMBIE_BOSS)
			{
				return;
			}
			if (mZombieType == ZombieType.ZOMBIE_CATAPULT)
			{
				mApp.AddTodParticle(mPosX + 80f, mPosY + 60f, mRenderOrder + 1, ParticleEffect.PARTICLE_CATAPULT_EXPLOSION);
				mApp.PlayFoley(FoleyType.FOLEY_EXPLOSION);
				DieWithLoot();
				return;
			}
			if (mZombieType == ZombieType.ZOMBIE_ZAMBONI)
			{
				mApp.AddTodParticle(mPosX + 80f, mPosY + 60f, mRenderOrder + 1, ParticleEffect.PARTICLE_ZAMBONI_EXPLOSION);
				mApp.PlayFoley(FoleyType.FOLEY_EXPLOSION);
				DieWithLoot();
				return;
			}
			if (mZombiePhase == ZombiePhase.PHASE_ZOMBIE_DYING || mZombiePhase == ZombiePhase.PHASE_POLEVAULTER_IN_VAULT || mZombiePhase == ZombiePhase.PHASE_RISING_FROM_GRAVE || mZombiePhase == ZombiePhase.PHASE_DANCER_RISING || mZombiePhase == ZombiePhase.PHASE_SNORKEL_INTO_POOL || mZombiePhase == ZombiePhase.PHASE_ZOMBIE_BURNED || mZombieType == ZombieType.ZOMBIE_GARGANTUAR || mZombieType == ZombieType.ZOMBIE_REDEYE_GARGANTUAR || mZombieType == ZombieType.ZOMBIE_BUNGEE || mZombieType == ZombieType.ZOMBIE_DIGGER || mZombieType == ZombieType.ZOMBIE_IMP || mZombieType == ZombieType.ZOMBIE_YETI || mZombieType == ZombieType.ZOMBIE_DOLPHIN_RIDER || IsBobsledTeamWithSled() || IsFlying() || mInPool)
			{
				Reanimation reanimation = mApp.AddReanimation(mPosX - 73f, mPosY - 56f, mRenderOrder + 2, ReanimationType.REANIM_PUFF);
				reanimation.SetFramesForLayer(GlobalMembersReanimIds.ReanimTrackId_anim_puff);
				mApp.AddTodParticle(mPosX + 110f, mPosY + 0f, mRenderOrder + 1, ParticleEffect.PARTICLE_MOWER_CLOUD);
				if (mBoard.mPlantRow[mRow] != PlantRowType.PLANTROW_POOL)
				{
					DropHead(0u);
					DropArm(0u);
					DropHelm(0u);
					DropShield(0u);
				}
				DieWithLoot();
				return;
			}
			if (mIceTrapCounter > 0)
			{
				RemoveIceTrap();
			}
			if (mButteredCounter > 0)
			{
				mButteredCounter = 0;
			}
			DropShield(0u);
			DropHelm(0u);
			if (mZombieType == ZombieType.ZOMBIE_FLAG)
			{
				DropFlag();
			}
			else if (mZombieType == ZombieType.ZOMBIE_POLEVAULTER)
			{
				DropPole();
			}
			else if (mZombieType == ZombieType.ZOMBIE_NEWSPAPER || mZombieType == ZombieType.ZOMBIE_BALLOON)
			{
				DropHead(0u);
			}
			else if (mZombieType == ZombieType.ZOMBIE_POGO)
			{
				DropHead(0u);
				mAltitude = 0f;
			}
			Reanimation reanimation2 = mApp.AddReanimation(0f, 0f, mRenderOrder, ReanimationType.REANIM_LAWN_MOWERED_ZOMBIE);
			reanimation2.mIsAttachment = false;
			reanimation2.mLoopType = ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD;
			reanimation2.mAnimRate = 8f;
			mMoweredReanimID = mApp.ReanimationGetID(reanimation2);
			mZombiePhase = ZombiePhase.PHASE_ZOMBIE_MOWERED;
			DropLoot();
			mBoard.AreEnemyZombiesOnScreen();
		}

		public void UpdateMowered()
		{
			Reanimation reanimation = mApp.ReanimationTryToGet(mMoweredReanimID);
			if (reanimation == null || reanimation.mLoopCount > 0)
			{
				DropHead(0u);
				DropArm(0u);
				DieWithLoot();
			}
		}

		public void DropFlag()
		{
			if (mZombieType == ZombieType.ZOMBIE_FLAG && mHasObject)
			{
				DetachFlag();
				mApp.RemoveReanimation(ref mSpecialHeadReanimID);
				mSpecialHeadReanimID = null;
				ReanimShowPrefix("anim_innerarm", 0);
				ReanimShowTrack(ref GlobalMembersReanimIds.ReanimTrackId_zombie_flaghand, -1);
				ReanimShowTrack(ref GlobalMembersReanimIds.ReanimTrackId_zombie_innerarm_screendoor, -1);
				mHasObject = false;
				float thePosX = 0f;
				float thePosY = 0f;
				GetTrackPosition(ref GlobalMembersReanimIds.ReanimTrackId_zombie_flaghand, ref thePosX, ref thePosY);
				TodParticleSystem aParticle = mApp.AddTodParticle(thePosX + 6f, thePosY - 45f, mRenderOrder + 1, ParticleEffect.PARTICLE_ZOMBIE_FLAG);
				OverrideParticleColor(aParticle);
				OverrideParticleScale(aParticle);
			}
		}

		public void DropPole()
		{
			if (mZombieType == ZombieType.ZOMBIE_POLEVAULTER)
			{
				ReanimShowPrefix("Zombie_polevaulter_innerarm", -1);
				ReanimShowPrefix("Zombie_polevaulter_innerhand", -1);
				ReanimShowPrefix("Zombie_polevaulter_pole", -1);
			}
		}

		public void DrawBossBackArm(Graphics g, ref ZombieDrawPosition theDrawPos)
		{
			float num = 0f;
			float num2 = 0f;
			if (mZombiePhase == ZombiePhase.PHASE_BOSS_DROP_RV)
			{
				num2 = (float)(mTargetRow - 1) * 85f - (float)mTargetCol * 20f;
				num = (float)mTargetCol * 80f;
				num *= Constants.S;
				num2 *= Constants.S;
			}
			else if (mZombiePhase == ZombiePhase.PHASE_BOSS_BUNGEES_ENTER || mZombiePhase == ZombiePhase.PHASE_BOSS_BUNGEES_DROP || mZombiePhase == ZombiePhase.PHASE_BOSS_BUNGEES_LEAVE)
			{
				num = (float)mTargetCol * 80f - 23f;
				num *= Constants.S;
			}
			Reanimation reanimation = mApp.ReanimationGet(mBodyReanimID);
			reanimation.mOverlayMatrix.mMatrix.M41 += num;
			reanimation.mOverlayMatrix.mMatrix.M42 += num2;
			DrawReanim(g, ref theDrawPos, GameConstants.RENDER_GROUP_BOSS_BACK_ARM);
			reanimation.mOverlayMatrix.mMatrix.M41 -= num;
			reanimation.mOverlayMatrix.mMatrix.M42 -= num2;
		}

		public static void PreloadZombieResources(ZombieType theZombieType)
		{
			ZombieDefinition zombieDefinition = GetZombieDefinition(theZombieType);
			if (zombieDefinition.mReanimationType != ReanimationType.REANIM_NONE)
			{
				ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(zombieDefinition.mReanimationType, true);
			}
			if (theZombieType == ZombieType.ZOMBIE_DIGGER)
			{
				ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_DIGGER_DIRT, true);
				ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_ZOMBIE_CHARRED_DIGGER, true);
			}
			if (theZombieType == ZombieType.ZOMBIE_BOSS)
			{
				ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_BOSS_DRIVER, true);
				ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_BOSS_FIREBALL, true);
				ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_BOSS_ICEBALL, true);
				for (int i = 0; i < GameConstants.gBossZombieList.Length; i++)
				{
					ZombieDefinition zombieDefinition2 = GetZombieDefinition(GameConstants.gBossZombieList[i]);
					ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(zombieDefinition2.mReanimationType, true);
				}
			}
			if (theZombieType == ZombieType.ZOMBIE_DANCER)
			{
				ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_BACKUP_DANCER, true);
			}
			if (theZombieType == ZombieType.ZOMBIE_GARGANTUAR || theZombieType == ZombieType.ZOMBIE_REDEYE_GARGANTUAR)
			{
				ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_IMP, true);
				ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_ZOMBIE_CHARRED_IMP, true);
				ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_ZOMBIE_CHARRED_GARGANTUAR, true);
			}
			if (theZombieType == ZombieType.ZOMBIE_ZAMBONI)
			{
				ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_IMP, true);
				ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_ZOMBIE_CHARRED_ZAMBONI, true);
			}
			if (theZombieType == ZombieType.ZOMBIE_CATAPULT)
			{
				ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_ZOMBIE_CHARRED_CATAPULT, true);
			}
			ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_PUFF, true);
			ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_ZOMBIE_CHARRED, true);
			ReanimatorXnaHelpers.ReanimatorEnsureDefinitionLoaded(ReanimationType.REANIM_LAWN_MOWERED_ZOMBIE, true);
		}

		public void BossStartDeath()
		{
			mZombiePhase = ZombiePhase.PHASE_BOSS_HEAD_LEAVE;
			PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_head_leave, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 0, 24f);
			mApp.AddTodParticle(700f, 150f, 400000, ParticleEffect.PARTICLE_BOSS_EXPLOSION);
			mApp.PlaySample(Resources.SOUND_BOSSEXPLOSION);
			mApp.PlayFoley(FoleyType.FOLEY_GARGANTUDEATH);
			BossDie();
		}

		public void RemoveColdEffects()
		{
			if (mIceTrapCounter > 0)
			{
				RemoveIceTrap();
			}
			if (mChilledCounter > 0)
			{
				mChilledCounter = 0;
				UpdateAnimSpeed();
			}
		}

		public void BossHeadSpitEffect()
		{
			int aRenderOrder = mRenderOrder + 2;
			if (mIsFireBall)
			{
				Reanimation reanimation = mApp.ReanimationGet(mBodyReanimID);
				int theTrackIndex = reanimation.FindTrackIndex(GlobalMembersReanimIds.ReanimTrackId_boss_jaw);
				ReanimatorTransform aTransformCurrent;
				reanimation.GetCurrentTransform(theTrackIndex, out aTransformCurrent, false);
				float theX = mPosX + aTransformCurrent.mTransX * Constants.IS + 100f;
				float theY = mPosY + aTransformCurrent.mTransY * Constants.IS + 50f;
				mApp.AddTodParticle(theX, theY, aRenderOrder, ParticleEffect.PARTICLE_ZOMBIE_BOSS_FIREBALL);
				aTransformCurrent.PrepareForReuse();
			}
			else
			{
				Reanimation reanimation2 = mApp.ReanimationGet(mBodyReanimID);
				int theTrackIndex2 = reanimation2.FindTrackIndex(GlobalMembersReanimIds.ReanimTrackId_boss_jaw);
				ReanimatorTransform aTransformCurrent2;
				reanimation2.GetCurrentTransform(theTrackIndex2, out aTransformCurrent2, false);
				float theX2 = mPosX + aTransformCurrent2.mTransX * Constants.IS + 100f;
				float theY2 = mPosY + aTransformCurrent2.mTransY * Constants.IS + 50f;
				TodParticleSystem todParticleSystem = mApp.AddTodParticle(theX2, theY2, aRenderOrder, ParticleEffect.PARTICLE_ZOMBIE_BOSS_FIREBALL);
				if (todParticleSystem != null)
				{
					todParticleSystem.OverrideImage(null, AtlasResources.IMAGE_ZOMBIE_BOSS_ICEBALL_PARTICLES);
				}
				aTransformCurrent2.PrepareForReuse();
			}
			mApp.PlayFoley(FoleyType.FOLEY_BOSSBOULDERATTACK);
		}

		public void DrawBossFireBall(Graphics g, ref ZombieDrawPosition theDrawPos)
		{
			MakeParentGraphicsFrame(g);
			Reanimation reanimation = mApp.ReanimationTryToGet(mBossFireBallReanimID);
			if (reanimation != null)
			{
				reanimation.DrawRenderGroup(g, 0);
				g.SetDrawMode(Graphics.DrawMode.DRAWMODE_ADDITIVE);
				reanimation.DrawRenderGroup(g, GameConstants.RENDER_GROUP_BOSS_FIREBALL_ADDITIVE);
				g.SetDrawMode(Graphics.DrawMode.DRAWMODE_NORMAL);
				reanimation.DrawRenderGroup(g, GameConstants.RENDER_GROUP_BOSS_FIREBALL_TOP);
			}
		}

		public void UpdateZombiePeaHead()
		{
			if (!mHasHead)
			{
				return;
			}
			if (mPhaseCounter >= 36 && mPhaseCounter < 39)
			{
				Reanimation reanimation = mApp.ReanimationGet(mSpecialHeadReanimID);
				if (reanimation != null)
				{
					reanimation.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_anim_shooting, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 20, 35f);
				}
			}
			else
			{
				if (mPhaseCounter > 0)
				{
					return;
				}
				Reanimation reanimation2 = mApp.ReanimationGet(mSpecialHeadReanimID);
				if (reanimation2 != null)
				{
					reanimation2.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_anim_head_idle, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 20, 15f);
				}
				mApp.PlayFoley(FoleyType.FOLEY_THROW);
				Reanimation reanimation3 = mApp.ReanimationGet(mBodyReanimID);
				if (reanimation3 != null)
				{
					int theTrackIndex = reanimation3.FindTrackIndex(GlobalMembersReanimIds.ReanimTrackId_anim_head1);
					ReanimatorTransform aTransformCurrent;
					reanimation3.GetCurrentTransform(theTrackIndex, out aTransformCurrent, false);
					float num = mPosX + aTransformCurrent.mTransX * Constants.IS - 9f;
					float num2 = mPosY + aTransformCurrent.mTransY * Constants.IS + 6f - mAltitude;
					ProjectileType projectileType = ProjectileType.PROJECTILE_ZOMBIE_PEA;
					if (mMindControlled)
					{
						num += 90f * mScaleZombie;
						projectileType = ProjectileType.PROJECTILE_ZOMBIE_PEA_MIND_CONTROL;
					}
					Projectile projectile = mBoard.AddProjectile((int)num, (int)num2, mRenderOrder, mRow, projectileType);
					if (!mMindControlled)
					{
						projectile.mMotionType = ProjectileMotion.MOTION_BACKWARDS;
					}
					mPhaseCounter = 150;
					aTransformCurrent.PrepareForReuse();
				}
			}
		}

		public void BurnRow(int theRow)
		{
			int theDamageRangeFlags = 127;
			int count = mBoard.mZombies.Count;
			for (int i = 0; i < count; i++)
			{
				Zombie zombie = mBoard.mZombies[i];
				if (zombie != this && !zombie.mDead)
				{
					int num = zombie.mRow - mRow;
					if (zombie.mZombieType == ZombieType.ZOMBIE_BOSS)
					{
						num = 0;
					}
					if (num == 0 && zombie.EffectedByDamage((uint)theDamageRangeFlags))
					{
						zombie.RemoveColdEffects();
						zombie.ApplyBurn();
					}
				}
			}
			int index = -1;
			GridItem theGridItem = null;
			while (mBoard.IterateGridItems(ref theGridItem, ref index))
			{
				if (theGridItem.mGridY == theRow && theGridItem.mGridItemType == GridItemType.GRIDITEM_LADDER)
				{
					theGridItem.GridItemDie();
				}
			}
		}

		public void UpdateZombieJalapenoHead()
		{
			if (!mHasHead || mPhaseCounter > 0)
			{
				return;
			}
			mApp.PlayFoley(FoleyType.FOLEY_JALAPENO_IGNITE);
			mApp.PlayFoley(FoleyType.FOLEY_JUICY);
			mBoard.DoFwoosh(mRow);
			mBoard.ShakeBoard(3, -4);
			if (mMindControlled)
			{
				BurnRow(mRow);
				return;
			}
			int count = mBoard.mPlants.Count;
			for (int i = 0; i < count; i++)
			{
				Plant plant = mBoard.mPlants[i];
				if (!plant.mDead)
				{
					plant.GetPlantRect();
					if (mRow == plant.mRow && !plant.NotOnGround())
					{
						mBoard.mPlantsEaten++;
						plant.Die();
					}
				}
			}
			DieNoLoot(false);
		}

		public void ApplyBossSmokeParticles(bool theEnable)
		{
			Reanimation reanimation = mApp.ReanimationTryToGet(mBodyReanimID);
			ReanimatorTrackInstance trackInstanceByName = reanimation.GetTrackInstanceByName(GlobalMembersReanimIds.ReanimTrackId_boss_head);
			GlobalMembersAttachment.AttachmentDetachCrossFadeParticleType(ref trackInstanceByName.mAttachmentID, ParticleEffect.PARTICLE_ZAMBONI_SMOKE, null);
			if (!theEnable)
			{
				return;
			}
			TodParticleSystem theParticleSystem = mApp.AddTodParticle(0f, 0f, 0, ParticleEffect.PARTICLE_ZAMBONI_SMOKE);
			TodParticleSystem theParticleSystem2 = mApp.AddTodParticle(0f, 0f, 0, ParticleEffect.PARTICLE_ZAMBONI_SMOKE);
			if (theParticleSystem != null)
			{
				AttachEffect attachEffect = reanimation.AttachParticleToTrack(GlobalMembersReanimIds.ReanimTrackId_boss_head, ref theParticleSystem, 60f, 30f);
				attachEffect.mDontDrawIfParentHidden = true;
				attachEffect.mDontPropogateColor = true;
			}
			if (theParticleSystem2 != null)
			{
				AttachEffect attachEffect2 = reanimation.AttachParticleToTrack(GlobalMembersReanimIds.ReanimTrackId_boss_head, ref theParticleSystem2, 100f, 58f);
				attachEffect2.mDontDrawIfParentHidden = true;
				attachEffect2.mDontPropogateColor = true;
			}
			if (mBodyHealth < mBodyMaxHealth / GameConstants.BOSS_FLASH_HEALTH_FRACTION)
			{
				TodParticleSystem theParticleSystem3 = mApp.AddTodParticle(0f, 0f, 0, ParticleEffect.PARTICLE_ZAMBONI_SMOKE);
				if (theParticleSystem3 != null)
				{
					AttachEffect attachEffect3 = reanimation.AttachParticleToTrack(GlobalMembersReanimIds.ReanimTrackId_boss_head, ref theParticleSystem3, 80f, 27f);
					attachEffect3.mDontDrawIfParentHidden = true;
					attachEffect3.mDontPropogateColor = true;
				}
			}
		}

		public void UpdateZombiquarium()
		{
			if (IsDeadOrDying())
			{
				return;
			}
			float num = mVelZ;
			float num2 = mVelX;
			Reanimation reanimation = mApp.ReanimationGet(mBodyReanimID);
			if (mZombiePhase == ZombiePhase.PHASE_ZOMBIQUARIUM_BITE)
			{
				if (reanimation.mLoopCount > 0)
				{
					float theAnimRate = TodCommon.RandRangeFloat(8f, 10f);
					PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_aquarium_swim, ReanimLoopType.REANIM_LOOP, 20, theAnimRate);
					mZombiePhase = ZombiePhase.PHASE_ZOMBIQUARIUM_DRIFT;
					mPhaseCounter = 100;
				}
			}
			else if (!ZombiquariumFindClosestBrain() && mPhaseCounter == 0)
			{
				int num3 = RandomNumbers.NextNumber(7);
				if (num3 <= 4)
				{
					mZombiePhase = ZombiePhase.PHASE_ZOMBIQUARIUM_ACCEL;
					num = TodCommon.RandRangeFloat(0f, (float)Math.PI * 2f);
					mPhaseCounter = TodCommon.RandRangeInt(300, 1000);
					reanimation.mAnimRate = TodCommon.RandRangeFloat(15f, 20f);
				}
				else
				{
					switch (num3)
					{
					case 4:
						mZombiePhase = ZombiePhase.PHASE_ZOMBIQUARIUM_DRIFT;
						num = 4.712389f;
						mPhaseCounter = TodCommon.RandRangeInt(300, 1000);
						reanimation.mAnimRate = TodCommon.RandRangeFloat(8f, 10f);
						break;
					case 5:
						mZombiePhase = ZombiePhase.PHASE_ZOMBIQUARIUM_BACK_AND_FORTH;
						num = 0f;
						mPhaseCounter = TodCommon.RandRangeInt(300, 1000);
						reanimation.mAnimRate = TodCommon.RandRangeFloat(15f, 20f);
						break;
					default:
						mZombiePhase = ZombiePhase.PHASE_ZOMBIQUARIUM_BACK_AND_FORTH;
						num = (float)Math.PI;
						mPhaseCounter = TodCommon.RandRangeInt(300, 1000);
						reanimation.mAnimRate = TodCommon.RandRangeFloat(15f, 20f);
						break;
					}
				}
			}
			float num4 = (float)Math.Cos(num);
			float num5 = (float)Math.Sin(num);
			bool flag = false;
			if (mPosX < 0f && num4 < 0f)
			{
				flag = true;
			}
			else if (mPosX > 680f && num4 > 0f)
			{
				flag = true;
			}
			else if (mPosY < 100f && num5 < 0f)
			{
				flag = true;
			}
			else if (mPosY > 400f && num5 > 0f)
			{
				flag = true;
			}
			float val = 0.5f;
			if (flag)
			{
				val = num2 * 0.3f;
				mPhaseCounter = Math.Min(100, mPhaseCounter);
			}
			else if (mZombiePhase == ZombiePhase.PHASE_ZOMBIQUARIUM_ACCEL)
			{
				val = 0.5f;
			}
			else if (mZombiePhase == ZombiePhase.PHASE_ZOMBIQUARIUM_BACK_AND_FORTH)
			{
				if (mPosX < 200f && num4 < 0f)
				{
					num = 0f;
				}
				if (mPosX > 550f && num4 > 0f)
				{
					num = (float)Math.PI;
				}
				val = 0.3f;
			}
			else if (mZombiePhase == ZombiePhase.PHASE_ZOMBIQUARIUM_DRIFT || mZombiePhase == ZombiePhase.PHASE_ZOMBIQUARIUM_BITE)
			{
				val = 0.05f;
			}
			num2 = Math.Min(val, num2 + 0.01f);
			num4 *= num2;
			num5 *= num2;
			mPosX += num4;
			mPosY += num5;
			if (mBoard.HasLevelAwardDropped())
			{
				return;
			}
			if (mSummonCounter > 0)
			{
				mSummonCounter--;
				if (mSummonCounter == 0)
				{
					mApp.PlayFoley(FoleyType.FOLEY_SPAWN_SUN);
					mBoard.AddCoin(mX + 50, mY + 40, CoinType.COIN_SUN, CoinMotion.COIN_MOTION_FROM_PLANT);
					mSummonCounter = TodCommon.RandRangeInt(1000, 1500);
				}
			}
			if (mZombieAge % 100 == 0)
			{
				TakeDamage(10, 8u);
				if (IsDeadOrDying())
				{
					mApp.PlaySample(Resources.SOUND_ZOMBAQUARIUM_DIE);
				}
			}
		}

		public bool ZombiquariumFindClosestBrain()
		{
			if (mBoard.HasLevelAwardDropped())
			{
				return false;
			}
			if (mBodyHealth > 150)
			{
				return false;
			}
			GridItem gridItem = null;
			float num = 0f;
			float num2 = 15f;
			float num3 = 15f;
			float num4 = 50f;
			float num5 = 40f;
			int index = -1;
			GridItem theGridItem = null;
			while (mBoard.IterateGridItems(ref theGridItem, ref index))
			{
				if (theGridItem.mGridItemType == GridItemType.GRIDITEM_BRAIN && theGridItem.mGridItemCounter >= 15)
				{
					float num6 = TodCommon.Distance2D(theGridItem.mPosX + num2, theGridItem.mPosY + num3, mPosX + num4, mPosY + num5);
					if (gridItem == null || num6 < num)
					{
						num = num6;
						gridItem = theGridItem;
					}
				}
			}
			if (gridItem != null && num < 50f)
			{
				gridItem.GridItemDie();
				mApp.PlayFoley(FoleyType.FOLEY_SLURP);
				mBodyHealth = Math.Min(mBodyMaxHealth, mBodyHealth + 200);
				PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_aquarium_bite, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 10, 24f);
				mZombiePhase = ZombiePhase.PHASE_ZOMBIQUARIUM_BITE;
				mPhaseCounter = 200;
				return false;
			}
			if (gridItem != null)
			{
				float num7 = gridItem.mPosX + num2 - (mPosX + num4);
				float num8 = gridItem.mPosY + num3 - (mPosY + num5);
				float num9 = mVelZ;
				num9 = (float)Math.Atan2(num8, num7);
				if (num9 < 0f)
				{
					num9 += (float)Math.PI * 2f;
				}
				mZombiePhase = ZombiePhase.PHASE_ZOMBIQUARIUM_ACCEL;
				return true;
			}
			return false;
		}

		public void UpdateZombieGatlingHead()
		{
			if (!mHasHead)
			{
				return;
			}
			if (mPhaseCounter >= 99 && mPhaseCounter < 102)
			{
				Reanimation reanimation = mApp.ReanimationGet(mSpecialHeadReanimID);
				reanimation.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_anim_shooting, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 20, 38f);
			}
			else if ((mPhaseCounter >= 18 && mPhaseCounter < 21) || (mPhaseCounter >= 36 && mPhaseCounter < 39) || (mPhaseCounter >= 51 && mPhaseCounter < 54) || (mPhaseCounter >= 69 && mPhaseCounter < 72))
			{
				mApp.PlayFoley(FoleyType.FOLEY_THROW);
				Reanimation reanimation2 = mApp.ReanimationGet(mBodyReanimID);
				if (reanimation2 != null)
				{
					int theTrackIndex = reanimation2.FindTrackIndex(GlobalMembersReanimIds.ReanimTrackId_anim_head1);
					ReanimatorTransform aTransformCurrent;
					reanimation2.GetCurrentTransform(theTrackIndex, out aTransformCurrent, false);
					float num = mPosX + aTransformCurrent.mTransX * Constants.IS - 9f;
					float num2 = mPosY + aTransformCurrent.mTransY * Constants.IS + 6f;
					ProjectileType projectileType = ProjectileType.PROJECTILE_ZOMBIE_PEA;
					if (mMindControlled)
					{
						num += 90f * mScaleZombie;
						projectileType = ProjectileType.PROJECTILE_ZOMBIE_PEA_MIND_CONTROL;
					}
					Projectile projectile = mBoard.AddProjectile((int)num, (int)num2, mRenderOrder, mRow, projectileType);
					if (!mMindControlled)
					{
						projectile.mMotionType = ProjectileMotion.MOTION_BACKWARDS;
					}
					aTransformCurrent.PrepareForReuse();
				}
			}
			else if (mPhaseCounter <= 0)
			{
				Reanimation reanimation3 = mApp.ReanimationGet(mSpecialHeadReanimID);
				reanimation3.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_anim_head_idle, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 20, 15f);
				mPhaseCounter = 150;
			}
		}

		public void UpdateZombieSquashHead()
		{
			float num = 6f;
			float num2 = -21f;
			if (mHasHead && mIsEating && mZombiePhase == ZombiePhase.PHASE_SQUASH_PRE_LAUNCH)
			{
				StopEating();
				PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_idle, ReanimLoopType.REANIM_LOOP, 20, 12f);
				mHasHead = false;
				Reanimation reanimation = mApp.ReanimationGet(mSpecialHeadReanimID);
				reanimation.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_anim_jumpup, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 20, 24f);
				reanimation.mRenderOrder = mRenderOrder + 1;
				reanimation.SetPosition((mPosX + num) * Constants.S, (mPosY + num2) * Constants.S);
				Reanimation reanimation2 = mApp.ReanimationGet(mBodyReanimID);
				ReanimatorTrackInstance trackInstanceByName = reanimation2.GetTrackInstanceByName(GlobalMembersReanimIds.ReanimTrackId_anim_head1);
				GlobalMembersAttachment.AttachmentDetach(ref trackInstanceByName.mAttachmentID);
				reanimation.mOverlayMatrix.mMatrix.M11 = 0.75f;
				reanimation.mOverlayMatrix.mMatrix.M12 = 0f;
				reanimation.mOverlayMatrix.mMatrix.M22 = 0.75f;
				reanimation.mOverlayMatrix.mMatrix.M21 = 0f;
				mZombiePhase = ZombiePhase.PHASE_SQUASH_RISING;
				mPhaseCounter = 95;
			}
			if (mZombiePhase == ZombiePhase.PHASE_SQUASH_RISING)
			{
				int theGridX = mBoard.PixelToGridXKeepOnBoard(mX, mY);
				int num3 = mBoard.GridToPixelX(theGridX, mRow);
				int num4 = TodCommon.TodAnimateCurve(50, 20, mPhaseCounter, 0, num3 - (int)mPosX, TodCurves.CURVE_EASE_IN_OUT);
				int num5 = TodCommon.TodAnimateCurve(50, 20, mPhaseCounter, 0, -20, TodCurves.CURVE_EASE_IN_OUT);
				Reanimation reanimation = mApp.ReanimationGet(mSpecialHeadReanimID);
				reanimation.SetPosition((mPosX + num + (float)num4) * Constants.S, (mPosY + num2 + (float)num5) * Constants.S);
				if (mPhaseCounter <= 0)
				{
					reanimation = mApp.ReanimationGet(mSpecialHeadReanimID);
					reanimation.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_anim_jumpdown, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 0, 60f);
					mZombiePhase = ZombiePhase.PHASE_SQUASH_FALLING;
					mPhaseCounter = 10;
				}
			}
			if (mZombiePhase == ZombiePhase.PHASE_SQUASH_FALLING)
			{
				int num6 = TodCommon.TodAnimateCurve(10, 0, mPhaseCounter, -20, 74, TodCurves.CURVE_LINEAR);
				int theGridX = mBoard.PixelToGridXKeepOnBoard(mX, mY);
				int num7 = mBoard.GridToPixelX(theGridX, mRow);
				Reanimation reanimation = mApp.ReanimationGet(mSpecialHeadReanimID);
				reanimation.SetPosition((mPosX + num + (float)num7 - mPosX) * Constants.S, (mPosY + num2 + (float)num6) * Constants.S);
				if (mPhaseCounter >= 4 && mPhaseCounter < 7)
				{
					theGridX = mBoard.PixelToGridXKeepOnBoard(mX, mY);
					SquishAllInSquare(theGridX, mRow, ZombieAttackType.ATTACKTYPE_CHEW);
				}
				if (mPhaseCounter <= 0)
				{
					mZombiePhase = ZombiePhase.PHASE_SQUASH_DONE_FALLING;
					mPhaseCounter = 100;
					mBoard.ShakeBoard(1, 4);
					mApp.PlayFoley(FoleyType.FOLEY_THUMP);
				}
			}
			if (mZombiePhase == ZombiePhase.PHASE_SQUASH_DONE_FALLING && mPhaseCounter <= 0)
			{
				Reanimation reanimation = mApp.ReanimationGet(mSpecialHeadReanimID);
				reanimation.ReanimationDie();
				mSpecialHeadReanimID = null;
				TakeDamage(1800, 9u);
			}
		}

		public bool IsTanglekelpTarget()
		{
			int count = mBoard.mPlants.Count;
			for (int i = 0; i < count; i++)
			{
				Plant plant = mBoard.mPlants[i];
				if (!plant.mDead && plant.mState == PlantState.STATE_TANGLEKELP_GRABBING && plant.mTargetZombieID == mBoard.ZombieGetID(this))
				{
					return true;
				}
			}
			return false;
		}

		public bool HasYuckyFaceImage()
		{
			if (mBoard.mFutureMode)
			{
				return false;
			}
			if (mZombieType == ZombieType.ZOMBIE_NORMAL || mZombieType == ZombieType.ZOMBIE_TRAFFIC_CONE || mZombieType == ZombieType.ZOMBIE_PAIL || mZombieType == ZombieType.ZOMBIE_FLAG || mZombieType == ZombieType.ZOMBIE_DOOR || mZombieType == ZombieType.ZOMBIE_DUCKY_TUBE || mZombieType == ZombieType.ZOMBIE_DANCER || mZombieType == ZombieType.ZOMBIE_BACKUP_DANCER || mZombieType == ZombieType.ZOMBIE_NEWSPAPER || mZombieType == ZombieType.ZOMBIE_POLEVAULTER)
			{
				return true;
			}
			return false;
		}

		public bool IsSquashTarget(Plant exceptMe)
		{
			int count = mBoard.mPlants.Count;
			for (int i = 0; i < count; i++)
			{
				Plant plant = mBoard.mPlants[i];
				if (!plant.mDead && plant != exceptMe && plant.mSeedType == SeedType.SEED_SQUASH && plant.mTargetZombieID == mBoard.ZombieGetID(this))
				{
					return true;
				}
			}
			return false;
		}

		public bool IsTangleKelpTarget()
		{
			if (!mApp.mBoard.StageHasPool())
			{
				return false;
			}
			if (mZombieHeight == ZombieHeight.HEIGHT_DRAGGED_UNDER)
			{
				return true;
			}
			int count = mBoard.mPlants.Count;
			for (int i = 0; i < count; i++)
			{
				Plant plant = mBoard.mPlants[i];
				if (!plant.mDead && plant.mSeedType == SeedType.SEED_TANGLEKELP && plant.mTargetZombieID == mBoard.ZombieGetID(this))
				{
					return true;
				}
			}
			return false;
		}

		public bool IsFireResistant()
		{
			if (mZombieType == ZombieType.ZOMBIE_CATAPULT || mZombieType == ZombieType.ZOMBIE_ZAMBONI)
			{
				return true;
			}
			if (mShieldType == ShieldType.SHIELDTYPE_DOOR || mShieldType == ShieldType.SHIELDTYPE_LADDER)
			{
				return true;
			}
			return false;
		}

		public void EnableMustache(bool theEnableMustache)
		{
			if (mFromWave == GameConstants.ZOMBIE_WAVE_UI || !mHasHead || mZombieType == ZombieType.ZOMBIE_PEA_HEAD || mZombieType == ZombieType.ZOMBIE_WALLNUT_HEAD || mZombieType == ZombieType.ZOMBIE_TALLNUT_HEAD || mZombieType == ZombieType.ZOMBIE_JALAPENO_HEAD || mZombieType == ZombieType.ZOMBIE_GATLING_HEAD || mZombieType == ZombieType.ZOMBIE_SQUASH_HEAD)
			{
				return;
			}
			Reanimation reanimation = mApp.ReanimationTryToGet(mBodyReanimID);
			if (reanimation == null || !reanimation.TrackExists(GlobalMembersReanimIds.ReanimTrackId_zombie_mustache))
			{
				return;
			}
			if (!theEnableMustache)
			{
				reanimation.AssignRenderGroupToPrefix("Zombie_mustache", -1);
				return;
			}
			reanimation.AssignRenderGroupToPrefix("Zombie_mustache", 0);
			switch (TodCommon.RandRangeInt(1, 3))
			{
			case 1:
			{
				Image theImage = null;
				reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_mustache, theImage);
				break;
			}
			case 2:
				reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_mustache, AtlasResources.IMAGE_REANIM_ZOMBIE_MUSTACHE2);
				break;
			case 3:
				reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_zombie_mustache, AtlasResources.IMAGE_REANIM_ZOMBIE_MUSTACHE3);
				break;
			}
		}

		public void EnableFuture(bool theEnableFuture)
		{
			if (mFromWave == GameConstants.ZOMBIE_WAVE_UI || mZombieType == ZombieType.ZOMBIE_PEA_HEAD || mZombieType == ZombieType.ZOMBIE_WALLNUT_HEAD || mZombieType == ZombieType.ZOMBIE_JALAPENO_HEAD || mZombieType == ZombieType.ZOMBIE_GATLING_HEAD || mZombieType == ZombieType.ZOMBIE_SQUASH_HEAD || mZombieType == ZombieType.ZOMBIE_TALLNUT_HEAD)
			{
				return;
			}
			Reanimation reanimation = mApp.ReanimationTryToGet(mBodyReanimID);
			if (reanimation == null || reanimation.mReanimationType != ReanimationType.REANIM_ZOMBIE)
			{
				return;
			}
			if (!theEnableFuture)
			{
				Image theImage = null;
				reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_anim_head1, theImage);
				return;
			}
			int num = RandomNumbers.NextNumber(8) % 4;
			Image theImage2 = null;
			switch (num)
			{
			case 0:
				theImage2 = AtlasResources.IMAGE_REANIM_ZOMBIE_HEAD_SUNGLASSES1;
				break;
			case 1:
				theImage2 = AtlasResources.IMAGE_REANIM_ZOMBIE_HEAD_SUNGLASSES2;
				break;
			case 2:
				theImage2 = AtlasResources.IMAGE_REANIM_ZOMBIE_HEAD_SUNGLASSES3;
				break;
			case 3:
				theImage2 = AtlasResources.IMAGE_REANIM_ZOMBIE_HEAD_SUNGLASSES4;
				break;
			default:
				Debug.ASSERT(false);
				break;
			}
			reanimation.SetImageOverride(GlobalMembersReanimIds.ReanimTrackId_anim_head1, theImage2);
		}

		public void BungeeDropPlant()
		{
			if (mZombiePhase != ZombiePhase.PHASE_BUNGEE_GRABBING)
			{
				return;
			}
			Plant plant = mBoard.mPlants[mBoard.mPlants.IndexOf(mTargetPlantID)];
			if (plant != null)
			{
				if (plant.mOnBungeeState == PlantOnBungeeState.PLANT_GETTING_GRABBED_BY_BUNGEE)
				{
					plant.mOnBungeeState = PlantOnBungeeState.PLANT_NOT_ON_BUNGEE;
				}
				else if (plant.mOnBungeeState == PlantOnBungeeState.PLANT_RISING_WITH_BUNGEE)
				{
					plant.Die();
				}
				mTargetPlantID = null;
			}
		}

		public void RemoveButter()
		{
			if (mZombieType == ZombieType.ZOMBIE_BALLOON)
			{
				BalloonPropellerHatSpin(true);
			}
			if (mZombieType == ZombieType.ZOMBIE_PEA_HEAD || mZombieType == ZombieType.ZOMBIE_WALLNUT_HEAD || mZombieType == ZombieType.ZOMBIE_TALLNUT_HEAD || mZombieType == ZombieType.ZOMBIE_JALAPENO_HEAD || mZombieType == ZombieType.ZOMBIE_GATLING_HEAD || mZombieType == ZombieType.ZOMBIE_SQUASH_HEAD)
			{
				Reanimation reanimation = mApp.ReanimationTryToGet(mSpecialHeadReanimID);
				if (reanimation != null)
				{
					if (mZombieType == ZombieType.ZOMBIE_PEA_HEAD && reanimation.IsAnimPlaying(GlobalMembersReanimIds.ReanimTrackId_anim_shooting))
					{
						reanimation.mAnimRate = 35f;
					}
					else if (mZombieType == ZombieType.ZOMBIE_GATLING_HEAD && reanimation.IsAnimPlaying(GlobalMembersReanimIds.ReanimTrackId_anim_shooting))
					{
						reanimation.mAnimRate = 38f;
					}
					else
					{
						reanimation.mAnimRate = 15f;
					}
				}
			}
			UpdateAnimSpeed();
			StartZombieSound();
		}

		public void BalloonPropellerHatSpin(bool theSpinning)
		{
			Reanimation reanimation = mApp.ReanimationGet(mBodyReanimID);
			ReanimatorTrackInstance trackInstanceByName = reanimation.GetTrackInstanceByName(GlobalMembersReanimIds.ReanimTrackId_hat);
			Reanimation reanimation2 = GlobalMembersAttachment.FindReanimAttachment(trackInstanceByName.mAttachmentID);
			if (reanimation2 != null)
			{
				if (theSpinning)
				{
					reanimation2.mAnimRate = reanimation2.mDefinition.mFPS;
				}
				else
				{
					reanimation2.mAnimRate = 0f;
				}
			}
		}

		public void DoDaisies()
		{
			if (!IsWalkingBackwards() && mBoard.mPlantRow[mRow] != PlantRowType.PLANTROW_POOL && mZombieType != ZombieType.ZOMBIE_BOBSLED && mZombieType != ZombieType.ZOMBIE_ZAMBONI && mZombieType != ZombieType.ZOMBIE_CATAPULT && !mBoard.StageHasRoof())
			{
				float num = 20f;
				float num2 = 100f;
				if (mZombieType == ZombieType.ZOMBIE_FOOTBALL || mZombieType == ZombieType.ZOMBIE_DANCER || mZombieType == ZombieType.ZOMBIE_BACKUP_DANCER)
				{
					num += 160f;
				}
				if (mZombieType == ZombieType.ZOMBIE_POGO)
				{
					num2 += 20f;
				}
				if (mZombieType == ZombieType.ZOMBIE_BALLOON)
				{
					num2 += 30f;
					num += 110f;
				}
				if (mBoard.StageHasGraveStones())
				{
					num2 += 15f;
				}
				int aRenderOrder = Board.MakeRenderOrder(RenderLayer.RENDER_LAYER_GRAVE_STONE, mRow, 5);
				mApp.AddTodParticle((float)mX + Constants.InvertAndScale(num), (float)mY + Constants.InvertAndScale(num2), aRenderOrder, ParticleEffect.PARTICLE_DAISY);
			}
		}

		public void EnableDanceMode(bool theEnableDance)
		{
			if (mFromWave != GameConstants.ZOMBIE_WAVE_UI && mFromWave != GameConstants.ZOMBIE_WAVE_CUTSCENE && !ZombieNotWalking() && !IsDeadOrDying() && (mZombieType == ZombieType.ZOMBIE_NORMAL || mZombieType == ZombieType.ZOMBIE_TRAFFIC_CONE || mZombieType == ZombieType.ZOMBIE_PAIL))
			{
				StartWalkAnim(0);
			}
		}

		public void BungeeLiftTarget()
		{
			PlayZombieReanim(ref GlobalMembersReanimIds.ReanimTrackId_anim_raise, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 0, 36f);
			if (mTargetPlantID == null)
			{
				return;
			}
			Plant plant = mTargetPlantID;
			if (plant == null)
			{
				return;
			}
			for (int i = 0; i < mBoard.mZombies.Count; i++)
			{
				Zombie zombie = mBoard.mZombies[i];
				if (zombie.mZombieType == ZombieType.ZOMBIE_BUNGEE && zombie != this && zombie.mTargetPlantID == plant)
				{
					zombie.mTargetPlantID = null;
				}
			}
			plant.mOnBungeeState = PlantOnBungeeState.PLANT_RISING_WITH_BUNGEE;
			mApp.PlayFoley(FoleyType.FOLEY_FLOOP);
			Reanimation reanimation = mApp.ReanimationTryToGet(plant.mBodyReanimID);
			if (reanimation != null)
			{
				reanimation.mAnimRate = 0.1f;
			}
			if (plant.mSeedType == SeedType.SEED_CATTAIL && mBoard.GetTopPlantAt(mTargetCol, mRow, PlantPriority.TOPPLANT_ONLY_PUMPKIN) != null)
			{
				mBoard.NewPlant(mTargetCol, mRow, SeedType.SEED_LILYPAD, SeedType.SEED_NONE);
			}
			if (mApp.IsIZombieLevel())
			{
				mBoard.mChallenge.IZombiePlantDropRemainingSun(plant);
			}
		}

		public void SetupWaterTrack(ref string theTrackName)
		{
			Reanimation reanimation = mApp.ReanimationGet(mBodyReanimID);
			ReanimatorTrackInstance trackInstanceByName = reanimation.GetTrackInstanceByName(theTrackName);
			trackInstanceByName.mIgnoreExtraAdditiveColor = true;
			trackInstanceByName.mIgnoreColorOverride = true;
			trackInstanceByName.mIgnoreClipRect = true;
		}

		public static ZombieDefinition GetZombieDefinition(ZombieType theZombieType)
		{
			Debug.ASSERT(theZombieType >= ZombieType.ZOMBIE_NORMAL && theZombieType < ZombieType.NUM_ZOMBIE_TYPES);
			Debug.ASSERT(GameConstants.gZombieDefs[(int)theZombieType].mZombieType == theZombieType);
			return GameConstants.gZombieDefs[(int)theZombieType];
		}

		public static bool ZombieTypeCanGoInPool(ZombieType theZombieType)
		{
			if (theZombieType == ZombieType.ZOMBIE_NORMAL || theZombieType == ZombieType.ZOMBIE_TRAFFIC_CONE || theZombieType == ZombieType.ZOMBIE_PAIL || theZombieType == ZombieType.ZOMBIE_FLAG || theZombieType == ZombieType.ZOMBIE_SNORKEL || theZombieType == ZombieType.ZOMBIE_DOLPHIN_RIDER || theZombieType == ZombieType.ZOMBIE_PEA_HEAD || theZombieType == ZombieType.ZOMBIE_WALLNUT_HEAD || theZombieType == ZombieType.ZOMBIE_JALAPENO_HEAD || theZombieType == ZombieType.ZOMBIE_GATLING_HEAD || theZombieType == ZombieType.ZOMBIE_TALLNUT_HEAD)
			{
				return true;
			}
			return false;
		}

		public static bool ZombieTypeCanGoOnHighGround(ZombieType theZombieType)
		{
			if (theZombieType == ZombieType.ZOMBIE_ZAMBONI || theZombieType == ZombieType.ZOMBIE_BOBSLED)
			{
				return false;
			}
			return true;
		}
	}
}
