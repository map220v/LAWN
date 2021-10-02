using Sexy;
using Sexy.TodLib;
using System;

namespace Lawn
{
	internal static class GlobalMembersSaveGame
	{
		internal struct SaveFileHeader
		{
			public uint mMagicNumber;

			public uint mBuildVersion;

			public uint mBuildDate;
		}

		internal class SaveGameContext
		{
			public Sexy.Buffer mBuffer = new Sexy.Buffer();

			public bool mFailed;

			public bool mReading;

			public void SyncInt(ref int theInt)
			{
			}

			public void SyncUint(ref uint theUint)
			{
			}

			public void SyncReanimationDef(ref ReanimatorDefinition theDefinition)
			{
			}

			public void SyncParticleDef(ref TodParticleDefinition theDefinition)
			{
			}

			public void SyncTrailDef(ref TrailDefinition theDefinition)
			{
			}

			public void SyncImage(ref Image theImage)
			{
			}
		}

		public const uint SAVE_FILE_VERSION = 2u;

		public static bool LawnLoadGame(Board mBoard, string theFilePath)
		{
			try
			{
				Sexy.Buffer theBuffer = new Sexy.Buffer();
				GlobalStaticVars.gSexyAppBase.ReadBufferFromFile(theFilePath, ref theBuffer, false);
				mBoard.LoadFromFile(theBuffer);
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				EffectSystem.gEffectSystem.EffectSystemFreeAll();
				return false;
			}
			return true;
		}

		public static bool LawnSaveGame(Board mBoard, string theFilePath)
		{
			LawnApp mApp = mBoard.mApp;
			SaveGameContext theContext = new SaveGameContext();
			theContext.mFailed = false;
			theContext.mReading = false;
			SyncBoard(ref theContext, mBoard);
			if (!mApp.WriteBufferToFile(theFilePath, theContext.mBuffer))
			{
				return false;
			}
			try
			{
				Sexy.Buffer buffer = new Sexy.Buffer();
				mBoard.SaveToFile(buffer);
				GlobalStaticVars.gSexyAppBase.WriteBufferToFile(theFilePath, buffer);
			}
			catch (Exception ex)
			{
				string message = ex.Message;
			}
			return true;
		}

		public static void SyncParticleEmitter(TodParticleSystem theParticleSystem, ref TodParticleEmitter theParticleEmitter, ref SaveGameContext theContext)
		{
		}

		public static void SyncParticleSystem(Board theBoard, TodParticleSystem theParticleSystem, ref SaveGameContext theContext)
		{
		}

		public static void SyncReanimation(Board theBoard, Reanimation theReanimation, ref SaveGameContext theContext)
		{
		}

		public static void SyncBoard(ref SaveGameContext theContext, Board mBoard)
		{
		}

		public static void FixBoardAfterLoad(Board mBoard)
		{
		}
	}
}
