using Lawn;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Globalization;

namespace Sexy
{
	public class Main : Game
	{
		private static SexyTransform2D orientationTransform;

		private static UI_ORIENTATION orientationUsed;

		private static bool newOrientation;

		//public static GamerServicesComponent GamerServicesComp;

		public static bool trialModeChecked = false;

		private static bool trialModeCachedValue = true;

		internal static Graphics graphics;

		private int mFrameCnt;

		private static bool startedProfiler;

		private static bool wantToSuppressDraw;

		private GamePadState previousGamepadState = default(GamePadState);

		private MouseState previousMouseState = default(MouseState);

		private KeyboardState previousKeyboardState = default(KeyboardState);

		public static bool RunWhenLocked
		{
			get
			{
                return true;//(int)PhoneApplicationService.get_Current().get_ApplicationIdleDetectionMode() == 1;
			}
			set
			{
				try
				{
					//PhoneApplicationService.get_Current().set_ApplicationIdleDetectionMode((IdleDetectionMode)(value ? 1 : 0));
				}
				catch
				{
				}
			}
		}

		public static bool LOW_MEMORY_DEVICE
		{
			get;
			private set;
		}

		public static bool DO_LOW_MEMORY_OPTIONS
		{
			get;
			private set;
		}

		public static bool IsInTrialMode
		{
			get
			{
				return trialModeCachedValue;
			}
		}

		public Main()
		{
			SetupTileSchedule();
			graphics = Graphics.GetNew(this);
			SetLowMem();
			IsMouseVisible = true;
			graphics.IsFullScreen = false;//true
			//Guide.SimulateTrialMode = false;
			graphics.PreferredBackBufferWidth = 800;
			graphics.PreferredBackBufferHeight = 480;
			GraphicsState.mGraphicsDeviceManager.SupportedOrientations = Constants.SupportedOrientations;
			GraphicsState.mGraphicsDeviceManager.DeviceCreated += graphics_DeviceCreated;
			GraphicsState.mGraphicsDeviceManager.DeviceReset += graphics_DeviceReset;
			GraphicsState.mGraphicsDeviceManager.PreparingDeviceSettings += mGraphicsDeviceManager_PreparingDeviceSettings;
			base.TargetElapsedTime = TimeSpan.FromSeconds(0.033333333333333333);
			base.Exiting += Main_Exiting;
			/*PhoneApplicationService.get_Current().set_UserIdleDetectionMode((IdleDetectionMode)0);
			PhoneApplicationService.get_Current().add_Launching((EventHandler<LaunchingEventArgs>)Game_Launching);
			PhoneApplicationService.get_Current().add_Activated((EventHandler<ActivatedEventArgs>)Game_Activated);
			PhoneApplicationService.get_Current().add_Closing((EventHandler<ClosingEventArgs>)Current_Closing);
			PhoneApplicationService.get_Current().add_Deactivated((EventHandler<DeactivatedEventArgs>)Current_Deactivated);*/
		}

		/*private void Current_Deactivated(object sender, DeactivatedEventArgs e)
		{
			GlobalStaticVars.gSexyAppBase.Tombstoned();
		}

		private void Current_Closing(object sender, ClosingEventArgs e)
		{
			PhoneApplicationService.get_Current().get_State().Clear();
		}

		private void Game_Activated(object sender, ActivatedEventArgs e)
		{
		}

		private void Game_Launching(object sender, LaunchingEventArgs e)
		{
			PhoneApplicationService.get_Current().get_State().Clear();
		}*/

		private static void SetupTileSchedule()
		{
		}

		private void mGraphicsDeviceManager_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
		{
		}

		private void graphics_DeviceReset(object sender, EventArgs e)
		{
		}

		private void graphics_DeviceCreated(object sender, EventArgs e)
		{
			base.GraphicsDevice.PresentationParameters.PresentationInterval = PresentInterval.Immediate;
		}

		private void Main_Exiting(object sender, EventArgs e)
		{
			GlobalStaticVars.gSexyAppBase.AppExit();
		}

		protected override void Initialize()
		{
			base.Window.OrientationChanged += Window_OrientationChanged;
			//GamerServicesComp = new GamerServicesComponent(this);
			ReportAchievement.Initialise();
			base.Initialize();
		}

		protected override void LoadContent()
		{
			GraphicsState.Init();
			SetupForResolution();
			GlobalStaticVars.initialize(this);
			GlobalStaticVars.mGlobalContent.LoadSplashScreen();
			GlobalStaticVars.gSexyAppBase.StartLoadingThread();
		}

		protected override void UnloadContent()
		{
			GlobalStaticVars.mGlobalContent.cleanUp();
		}

		protected override void BeginRun()
		{
			base.BeginRun();
		}

		public void CompensateForSlowUpdate()
		{
			ResetElapsedTime();
		}

		protected override void Update(GameTime gameTime)
		{
			if (!base.IsActive)
			{
				return;
			}
			if (GlobalStaticVars.gSexyAppBase.WantsToExit)
			{
				Exit();
			}
			HandleInput(gameTime);
			GlobalStaticVars.gSexyAppBase.UpdateApp();
			if (!trialModeChecked)
			{
				trialModeChecked = true;
				bool flag = trialModeCachedValue;
				SetLowMem();
                trialModeCachedValue = false;//Guide.IsTrialMode;
				if (flag != trialModeCachedValue && flag)
				{
					LeftTrialMode();
				}
			}
			//try
			//{
				base.Update(gameTime);
			/*}
			catch (GameUpdateRequiredException)
			{
				GlobalStaticVars.gSexyAppBase.ShowUpdateRequiredMessage();
			}*/
		}

		private static void SetLowMem()
		{
			/*object obj = default(object);
			DeviceExtendedProperties.TryGetValue("DeviceTotalMemory", ref obj);
			DO_LOW_MEMORY_OPTIONS = (LOW_MEMORY_DEVICE = ((long)obj / 1024 / 1024 <= 256));*/
			LOW_MEMORY_DEVICE = false;
		}

		private void LeftTrialMode()
		{
			if (GlobalStaticVars.gSexyAppBase != null)
			{
				GlobalStaticVars.gSexyAppBase.LeftTrialMode();
			}
			Window_OrientationChanged(null, null);
		}

		public static void SuppressNextDraw()
		{
			wantToSuppressDraw = true;
		}

		/*public static SignedInGamer GetGamer()
		{
			if (Gamer.SignedInGamers.Count == 0)
			{
				return null;
			}
			return Gamer.SignedInGamers[PlayerIndex.One];
		}*/

		public static void NeedToSetUpOrientationMatrix(UI_ORIENTATION orientation)
		{
			orientationUsed = orientation;
			newOrientation = true;
		}

		private static void SetupOrientationMatrix(UI_ORIENTATION orientation)
		{
			newOrientation = false;
		}

		private void Window_OrientationChanged(object sender, EventArgs e)
		{
			SetupInterfaceOrientation();
		}

		private void SetupInterfaceOrientation()
		{
			if (GlobalStaticVars.gSexyAppBase != null)
			{
				if (base.Window.CurrentOrientation == DisplayOrientation.LandscapeLeft || base.Window.CurrentOrientation == DisplayOrientation.LandscapeRight)
				{
					GlobalStaticVars.gSexyAppBase.InterfaceOrientationChanged(UI_ORIENTATION.UI_ORIENTATION_LANDSCAPE_LEFT);
				}
				else
				{
					GlobalStaticVars.gSexyAppBase.InterfaceOrientationChanged(UI_ORIENTATION.UI_ORIENTATION_PORTRAIT);
				}
			}
		}

		protected override void Draw(GameTime gameTime)
		{
			if (newOrientation)
			{
				SetupOrientationMatrix(orientationUsed);
			}
			lock (ResourceManager.DrawLocker)
			{
				base.GraphicsDevice.Clear(Color.Black);
				GlobalStaticVars.gSexyAppBase.DrawGame(gameTime);
				base.Draw(gameTime);
			}
		}

		public void HandleInput(GameTime gameTime)
		{
			if (!LoadingScreen.IsLoading)
			{
				GamePadState state = GamePad.GetState(PlayerIndex.One);
				if (state.Buttons.Back == ButtonState.Pressed && previousGamepadState.Buttons.Back == ButtonState.Released)
				{
					GlobalStaticVars.gSexyAppBase.BackButtonPress();
				}
				TouchCollection state2 = TouchPanel.GetState();
				bool flag = false;
				foreach (TouchLocation item in state2)
				{
					_Touch touch = default(_Touch);
					touch.location.mX = item.Position.X;
					touch.location.mY = item.Position.Y;
					TouchLocation previousLocation;
					if (item.TryGetPreviousLocation(out previousLocation))
					{
						touch.previousLocation = new CGPoint(previousLocation.Position.X, previousLocation.Position.Y);
					}
					else
					{
						touch.previousLocation = touch.location;
					}
					touch.timestamp = gameTime.TotalGameTime.TotalSeconds;
					if (item.State == TouchLocationState.Pressed && !flag)
					{
						GlobalStaticVars.gSexyAppBase.TouchBegan(touch);
						flag = true;
					}
					else if (item.State == TouchLocationState.Moved)
					{
						GlobalStaticVars.gSexyAppBase.TouchMoved(touch);
					}
					else if (item.State == TouchLocationState.Released)
					{
						GlobalStaticVars.gSexyAppBase.TouchEnded(touch);
					}
					else if (item.State == TouchLocationState.Invalid)
					{
						GlobalStaticVars.gSexyAppBase.TouchesCanceled();
					}
				}

				/* Mouse Handle */
				MouseState state3 = Mouse.GetState();
				_Touch touch2 = default(_Touch);
				touch2.location.mX = state3.X;
				touch2.location.mY = state3.Y;
				if (state3.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
				{
					GlobalStaticVars.gSexyAppBase.TouchBegan(touch2);
				}
				else if (state3.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Pressed)
				{
					GlobalStaticVars.gSexyAppBase.TouchMoved(touch2);
				}
				else if (state3.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
				{
					GlobalStaticVars.gSexyAppBase.TouchEnded(touch2);
				}
				/* Cheat Keyboard Handle */
				KeyboardState state4 = Keyboard.GetState();
				bool Shift = state4.IsKeyDown(Keys.LeftShift) || state4.IsKeyDown(Keys.RightShift);
				foreach (Keys key in state4.GetPressedKeys())
				{
					if (state4.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key))
					{
#if DEBUG
						GlobalStaticVars.gSexyAppBase.KeyChar(new SexyChar(TUtils.KeyToChar(key, Shift)));
#endif
					}
				}

				previousMouseState = state3;
				previousKeyboardState = state4;
				previousGamepadState = state;
			}
		}

		protected override void OnActivated(object sender, EventArgs args)
		{
			trialModeChecked = false;
			if (GlobalStaticVars.gSexyAppBase != null)
			{
				GlobalStaticVars.gSexyAppBase.GotFocus();
				if (!GlobalStaticVars.gSexyAppBase.mMusicInterface.isStopped)
				{
					GlobalStaticVars.gSexyAppBase.mMusicInterface.ResumeMusic();
				}
			}
			base.OnActivated(sender, args);
		}

		protected override void OnDeactivated(object sender, EventArgs args)
		{
			GlobalStaticVars.gSexyAppBase.LostFocus();
			if (!GlobalStaticVars.gSexyAppBase.mMusicInterface.isStopped)
			{
				GlobalStaticVars.gSexyAppBase.mMusicInterface.PauseMusic();
			}
			GlobalStaticVars.gSexyAppBase.AppEnteredBackground();
			base.OnDeactivated(sender, args);
		}

		private void GameSpecificCheatInputCheck()
		{
		}

		private static void SetupForResolution()
		{
			Strings.Culture = CultureInfo.CurrentCulture;
			if (Strings.Culture.TwoLetterISOLanguageName == "fr")
			{
				Constants.Language = Constants.LanguageIndex.fr;
			}
			else if (Strings.Culture.TwoLetterISOLanguageName == "de")
			{
				Constants.Language = Constants.LanguageIndex.de;
			}
			else if (Strings.Culture.TwoLetterISOLanguageName == "es")
			{
				Constants.Language = Constants.LanguageIndex.es;
			}
			else if (Strings.Culture.TwoLetterISOLanguageName == "it")
			{
				Constants.Language = Constants.LanguageIndex.it;
			}
			else
			{
				Constants.Language = Constants.LanguageIndex.en;
			}
			if ((graphics.GraphicsDevice.PresentationParameters.BackBufferWidth == 480 && graphics.GraphicsDevice.PresentationParameters.BackBufferHeight == 800) || (graphics.GraphicsDevice.PresentationParameters.BackBufferWidth == 800 && graphics.GraphicsDevice.PresentationParameters.BackBufferHeight == 480))
			{
				AtlasResources.mAtlasResources = new AtlasResources_480x800();
				Constants.Load480x800();
				return;
			}
			throw new Exception("Unsupported Resolution");
		}
	}
}
