using Sexy;
using System;

namespace Lawn
{
	internal class Music
	{
		public LawnApp mApp;

		public MusicInterface mMusicInterface;

		public MusicTune mCurMusicTune;

		private bool mPaused;

		private bool mMusicDisabled;

		public Music()
		{
			mApp = (LawnApp)GlobalStaticVars.gSexyAppBase;
			mMusicInterface = mApp.mMusicInterface;
			mPaused = false;
			mMusicDisabled = false;
			mCurMusicTune = MusicTune.MUSIC_TUNE_NONE;
		}

		public void MusicTitleScreenInit()
		{
			LoadSong(MusicTune.MUSIC_TUNE_TITLE_CRAZY_DAVE_MAIN_THEME, "crazydave");
			PlayMusic(MusicTune.MUSIC_TUNE_TITLE_CRAZY_DAVE_MAIN_THEME);
		}

		public void MusicInit()
		{
			LoadSong(MusicTune.MUSIC_TUNE_DAY_GRASSWALK, "day");
			mApp.mCompletedLoadingThreadTasks += 3500;
			LoadSong(MusicTune.MUSIC_TUNE_NIGHT_MOONGRAINS, "night");
			mApp.mCompletedLoadingThreadTasks += 3500;
			LoadSong(MusicTune.MUSIC_TUNE_POOL_WATERYGRAVES, "pool");
			mApp.mCompletedLoadingThreadTasks += 3500;
			LoadSong(MusicTune.MUSIC_TUNE_FOG_RIGORMORMIST, "fog");
			mApp.mCompletedLoadingThreadTasks += 3500;
			LoadSong(MusicTune.MUSIC_TUNE_ROOF_GRAZETHEROOF, "roof");
			mApp.mCompletedLoadingThreadTasks += 3500;
			LoadSong(MusicTune.MUSIC_TUNE_CHOOSE_YOUR_SEEDS, "chooseyourseeds");
			mApp.mCompletedLoadingThreadTasks += 3500;
			LoadSong(MusicTune.MUSIC_TUNE_FINAL_BOSS_BRAINIAC_MANIAC, "boss");
			mApp.mCompletedLoadingThreadTasks += 3500;
			LoadSong(MusicTune.MUSIC_TUNE_PUZZLE_CEREBRAWL, "cerebrawl");
			mApp.mCompletedLoadingThreadTasks += 3500;
			LoadSong(MusicTune.MUSIC_TUNE_MINIGAME_LOONBOON, "loonboon");
			mApp.mCompletedLoadingThreadTasks += 3500;
			LoadSong(MusicTune.MUSIC_TUNE_CONVEYER, "conveyor");
			mApp.mCompletedLoadingThreadTasks += 3500;
			LoadSong(MusicTune.MUSIC_TUNE_ZEN_GARDEN, "zengarden");
			mApp.mCompletedLoadingThreadTasks += 3500;
		}

		public void StopAllMusic()
		{
			mMusicInterface.StopMusic(0f);
			mCurMusicTune = MusicTune.MUSIC_TUNE_NONE;
			mPaused = false;
		}

		public void StartGameMusic()
		{
			Debug.ASSERT(mApp.mBoard != null);
			if (mApp.mGameMode == GameMode.GAMEMODE_CHALLENGE_ZEN_GARDEN || mApp.mGameMode == GameMode.GAMEMODE_TREE_OF_WISDOM)
			{
				MakeSureMusicIsPlaying(MusicTune.MUSIC_TUNE_ZEN_GARDEN);
			}
			else if (mApp.IsFinalBossLevel())
			{
				MakeSureMusicIsPlaying(MusicTune.MUSIC_TUNE_FINAL_BOSS_BRAINIAC_MANIAC);
			}
			else if (mApp.IsWallnutBowlingLevel() || mApp.IsWhackAZombieLevel() || mApp.IsLittleTroubleLevel() || mApp.IsBungeeBlitzLevel() || mApp.mGameMode == GameMode.GAMEMODE_CHALLENGE_SPEED)
			{
				MakeSureMusicIsPlaying(MusicTune.MUSIC_TUNE_MINIGAME_LOONBOON);
			}
			else if ((mApp.IsAdventureMode() || mApp.IsQuickPlayMode()) && (mApp.mPlayerInfo.mLevel == 10 || mApp.mPlayerInfo.mLevel == 20 || mApp.mPlayerInfo.mLevel == 30))
			{
				MakeSureMusicIsPlaying(MusicTune.MUSIC_TUNE_CONVEYER);
			}
			else if (mApp.mGameMode == GameMode.GAMEMODE_CHALLENGE_COLUMN)
			{
				MakeSureMusicIsPlaying(MusicTune.MUSIC_TUNE_CONVEYER);
			}
			else if (mApp.IsStormyNightLevel())
			{
				StopAllMusic();
			}
			else if (mApp.IsScaryPotterLevel() || mApp.IsIZombieLevel())
			{
				MakeSureMusicIsPlaying(MusicTune.MUSIC_TUNE_PUZZLE_CEREBRAWL);
			}
			else if (mApp.mBoard.mBackground == BackgroundType.BACKGROUND_4_FOG)
			{
				MakeSureMusicIsPlaying(MusicTune.MUSIC_TUNE_FOG_RIGORMORMIST);
			}
			else if (mApp.mBoard.StageIsNight())
			{
				MakeSureMusicIsPlaying(MusicTune.MUSIC_TUNE_NIGHT_MOONGRAINS);
			}
			else if (mApp.mBoard.StageHasPool())
			{
				MakeSureMusicIsPlaying(MusicTune.MUSIC_TUNE_POOL_WATERYGRAVES);
			}
			else if (mApp.mBoard.StageHasRoof())
			{
				MakeSureMusicIsPlaying(MusicTune.MUSIC_TUNE_ROOF_GRAZETHEROOF);
			}
			else
			{
				MakeSureMusicIsPlaying(MusicTune.MUSIC_TUNE_DAY_GRASSWALK);
			}
		}

		public void GameMusicPause(bool thePause)
		{
			if (thePause && !mPaused && mCurMusicTune != MusicTune.MUSIC_TUNE_NONE)
			{
				if (mMusicInterface.IsPlaying((int)mCurMusicTune))
				{
					mMusicInterface.PauseMusic();
				}
				mPaused = true;
			}
			else
			{
				if (thePause || !mPaused)
				{
					return;
				}
				if (mCurMusicTune != MusicTune.MUSIC_TUNE_NONE)
				{
					if (mMusicInterface.IsPlaying((int)mCurMusicTune))
					{
						mMusicInterface.ResumeMusic();
					}
					else
					{
						PlayMusic(mCurMusicTune);
					}
				}
				mPaused = false;
			}
		}

		public void MakeSureMusicIsPlaying(MusicTune theMusicTune)
		{
			if (mCurMusicTune != theMusicTune)
			{
				StopAllMusic();
				PlayMusic(theMusicTune);
			}
		}

		public void FadeOut(int aFadeOutDuration)
		{
			if (mCurMusicTune != MusicTune.MUSIC_TUNE_NONE)
			{
				mMusicInterface.StopMusic((float)aFadeOutDuration / 100f);
				mCurMusicTune = MusicTune.MUSIC_TUNE_NONE;
				mPaused = false;
			}
		}

		public int GetNumLoadingTasks()
		{
			return 35000;
		}

		private void LoadSong(MusicTune theMusicTune, string theFileName)
		{
			if (!mMusicInterface.LoadMusic((int)theMusicTune, GlobalStaticVars.GetResourceDir() + "music/" + theFileName))
			{
				Console.Write("Failed to load music file {0}\n", theFileName);
			}
		}

		private void MusicDispose()
		{
			StopAllMusic();
			mMusicInterface.UnloadAllMusic();
		}

		private void PlayMusic(MusicTune theMusicTune)
		{
			if (!mMusicDisabled)
			{
				mCurMusicTune = theMusicTune;
				mMusicInterface.PlayMusic((int)theMusicTune, true);
			}
		}
	}
}
