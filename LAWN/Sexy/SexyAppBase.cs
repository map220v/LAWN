using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Sexy.TodLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading;

namespace Sexy
{
	internal class SexyAppBase : SexyAppBaseInterface, ButtonListener, DialogListener, IDisposable
	{
		private const double VIBRATION_DURATION = 500.0;

		public static Main XnaGame;

		protected UI_ORIENTATION? nextOrientation;

		public static object SplashScreenDrawLock = new object();

		public static bool FirstRun;

		public SexyAppBase gSexyAppBase;

		public string mCompanyName;

		public string mFullCompanyName;

		public string mProdName;

		public string mTitle;

		public int mWidth;

		public int mHeight;

		public UI_ORIENTATION mInterfaceOrientation;

		public bool mMusicEnabled;

		public double mSfxVolume;

		public bool mShutdown;

		public bool mInitialized;

		public MusicInterface mMusicInterface;

		public WidgetManager mWidgetManager;

		public ResourceManager mResourceManager;

		public ContentManager mContentManager;

		public SoundManager mSoundManager;

		public bool mPaused;

		public bool mAutoStartLoadingThread;

		public bool mLoadingThreadStarted;

		public bool mLoadingThreadCompleted;

		public bool mLoaded;

		public bool mLoadingFailed;

		public bool mIsOrientationLocked;

		public int mUpdateCount;

		public Dictionary<int, Dialog> mDialogMap = new Dictionary<int, Dialog>();

		public int mUpdateAppDepth;

		public int mNumLoadingThreadTasks;

		public int mMuteCount;

		private Dictionary<string, Image> mRegisteredImages = new Dictionary<string, Image>();

		public Dictionary<string, string> mStringProperties = new Dictionary<string, string>();

		public Dictionary<string, List<string>> mStringVectorProperties = new Dictionary<string, List<string>>();

		public Dictionary<string, bool> mBoolProperties = new Dictionary<string, bool>();

		public Dictionary<string, int> mIntProperties = new Dictionary<string, int>();

		public Dictionary<string, double> mDoubleProperties = new Dictionary<string, double>();

		public bool mReadFromRegistry;

		public LinkedList<Dialog> mDialogList = new LinkedList<Dialog>();

		public int mCompletedLoadingThreadTasks;

		private static Random rand = new Random(DateTime.UtcNow.Millisecond);

		//public static MediaPlayerLauncher VideoPlayer = (MediaPlayerLauncher)(object)new MediaPlayerLauncher();

		protected bool wantToShowUpdateMessage;

		public static bool UseLiveServers
		{
			get;
			protected set;
		}

		public static string mLanguage
		{
			get
			{
				return Constants.Language.ToString();
			}
		}

		public bool WantsToExit
		{
			get;
			set;
		}

		public double mMusicVolume
		{
			get
			{
				return mMusicInterface.GetVolume();
			}
			set
			{
				mMusicInterface.SetVolume((float)value);
			}
		}

		public static bool IsInTrialMode
		{
			get
			{
				return Main.IsInTrialMode;
			}
		}

		public virtual void GotoInterfaceState(int state)
		{
		}

		internal virtual void NewGame()
		{
		}

		public virtual void PlaySample(int theSoundNum)
		{
			PlaySample(theSoundNum, 0);
		}

		public virtual void PlaySample(int theSoundNum, int thePan)
		{
			PlaySample(theSoundNum, thePan, false);
		}

		public virtual SoundInstance PlaySample(int theSoundNum, int thePan, bool looping)
		{
			if (mSoundManager == null)
			{
				return null;
			}
			SoundInstance soundInstance = mSoundManager.GetSoundInstance((uint)theSoundNum);
			if (soundInstance != null)
			{
				if (thePan != 0)
				{
					soundInstance.SetPan(thePan);
				}
				soundInstance.Play(looping);
			}
			return soundInstance;
		}

		internal static void GetWidthHeightForOrientation(UI_ORIENTATION theOrientation, ref int theWidth, ref int theHeight)
		{
			theOrientation = UI_ORIENTATION.UI_ORIENTATION_LANDSCAPE_LEFT;
			if (theOrientation == UI_ORIENTATION.UI_ORIENTATION_PORTRAIT || theOrientation == UI_ORIENTATION.UI_ORIENTATION_PORTRAIT_UPSIDE_DOWN)
			{
				if (Constants.Loaded)
				{
					theWidth = Constants.BackBufferSize.X;
					theHeight = Constants.BackBufferSize.Y;
				}
				else
				{
					theWidth = GlobalStaticVars.g.GraphicsDevice.PresentationParameters.BackBufferWidth;
					theHeight = GlobalStaticVars.g.GraphicsDevice.PresentationParameters.BackBufferHeight;
				}
			}
			else if (Constants.Loaded)
			{
				theWidth = Constants.BackBufferSize.Y;
				theHeight = Constants.BackBufferSize.X;
			}
			else
			{
				theWidth = GlobalStaticVars.g.GraphicsDevice.PresentationParameters.BackBufferWidth;
				theHeight = GlobalStaticVars.g.GraphicsDevice.PresentationParameters.BackBufferHeight;
			}
		}

		public SexyAppBase(Main m)
		{
			UseLiveServers = true;
			XnaGame = m;
			gSexyAppBase = this;
			mProdName = "ProductName";
			mShutdown = false;
			mInterfaceOrientation = UI_ORIENTATION.UI_ORIENTATION_LANDSCAPE_LEFT;
			mPaused = false;
			mLoaded = false;
			mLoadingFailed = false;
			mAutoStartLoadingThread = true;
			mLoadingThreadStarted = false;
			mLoadingThreadCompleted = false;
			mInitialized = false;
			mMusicEnabled = true;
			mContentManager = m.Content;
			GetWidthHeightForOrientation(mInterfaceOrientation, ref mWidth, ref mHeight);
			mWidgetManager = new WidgetManager(this);
			mResourceManager = new ResourceManager(this);
			mSoundManager = new XNASoundManager(this);
			mMusicInterface = new XNAMusicInterface(this);
			mMusicInterface.SetDefaultFadeIn(0f);
			mMusicInterface.SetDefaultFadeOut(0.006f);
			mMusicVolume = 0.85;
			mSfxVolume = 0.85;
		}

		~SexyAppBase()
		{
			Dispose(false);
		}

		public virtual void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			gSexyAppBase = null;
		}

		public bool EraseFile(string theFileName)
		{
			try
			{
				if (!File.Exists(theFileName))
				{
					return true;
				}
				File.Delete(theFileName);
				if (File.Exists(theFileName))
				{
					return false;
				}
				return true;
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				return false;
			}
		}

		public void SetBoolean(string theId, bool theValue)
		{
			if (mBoolProperties.ContainsKey(theId))
			{
				mBoolProperties[theId] = theValue;
			}
			else
			{
				mBoolProperties.Add(theId, theValue);
			}
		}

		public void SetInteger(string theId, int theValue)
		{
			if (mIntProperties.ContainsKey(theId))
			{
				mIntProperties[theId] = theValue;
			}
			else
			{
				mIntProperties.Add(theId, theValue);
			}
		}

		public int GetInteger(string theId, int theDefault)
		{
			if (mIntProperties.ContainsKey(theId))
			{
				return mIntProperties[theId];
			}
			return theDefault;
		}

		public void SetDouble(string theId, double theValue)
		{
			if (mDoubleProperties.ContainsKey(theId))
			{
				mDoubleProperties[theId] = theValue;
			}
			else
			{
				mDoubleProperties.Add(theId, theValue);
			}
		}

		public virtual void Init()
		{
			mWidgetManager.Resize(mWidth, mHeight);
			InitHook();
		}

		public virtual void InitHook()
		{
		}

		public virtual void SaveGame()
		{
		}

		public void StartLoadingThread()
		{
			if (!mLoadingThreadStarted)
			{
				Thread thread = new Thread(LoadingThreadProcStub);
				thread.IsBackground = true;
				thread.Start();
				mLoadingThreadStarted = true;
			}
		}

		public void LoadingThreadProcStub()
		{
			LoadingThreadProc();
			mLoadingThreadCompleted = true;
		}

		public virtual void LoadingThreadProc()
		{
		}

		public virtual void LoadingThreadCompleted()
		{
		}

		public bool FileExists(string filename)
		{
			return File.Exists(filename);
		}

		public void UpdateInput()
		{
			InputController.HandleTouchInput();
		}

		public void UpdateAudio()
		{
			mSoundManager.Update();
			mMusicInterface.Update();
		}

		public bool UpdateApp()
		{
			UpdateAudio();
			UpdateInput();
			if (FirstRun && ShowRunWhenLockedMessage())
			{
				FirstRun = false;
			}
			DoUpdateFrames();
			return true;
		}

		protected virtual bool ShowRunWhenLockedMessage()
		{
			return true;
		}

		public virtual void AppEnteredBackground()
		{
		}

		public virtual bool DoUpdateFrames()
		{
			mUpdateCount++;
			if (mLoadingThreadCompleted && !mLoaded)
			{
				mLoaded = true;
				LoadingThreadCompleted();
			}
			UpdateFrames();
			return true;
		}

		protected virtual void ShowUpdateMessage()
		{
		}

		public virtual void UpdateFrames()
		{
			if (wantToShowUpdateMessage)
			{
				ShowUpdateMessage();
			}
			mWidgetManager.UpdateFrame();
		}

		public virtual void DrawGame(GameTime gameTime)
		{
			GlobalStaticVars.g.BeginFrame();
			mWidgetManager.DrawScreen();
			GlobalStaticVars.g.EndFrame();
		}

		public void DeviceOrientationChanged(UI_ORIENTATION toOrientation)
		{
		}

		public virtual void InterfaceOrientationChanged(UI_ORIENTATION toOrientation)
		{
			nextOrientation = toOrientation;
			if (!mIsOrientationLocked)
			{
				GetWidthHeightForOrientation(toOrientation, ref mWidth, ref mHeight);
				mInterfaceOrientation = toOrientation;
				mWidgetManager.InterfaceOrientationChanged(toOrientation);
				Main.NeedToSetUpOrientationMatrix(toOrientation);
				Graphics.OrientationChanged();
			}
		}

		public bool OrientationIsLandscape(UI_ORIENTATION orientation)
		{
			if (orientation != UI_ORIENTATION.UI_ORIENTATION_LANDSCAPE_LEFT)
			{
				return orientation == UI_ORIENTATION.UI_ORIENTATION_LANDSCAPE_RIGHT;
			}
			return true;
		}

		public void AccelerometerDidAccelerate(double timestamp, double ax, double ay, double az)
		{
		}

		internal bool Is3DAccelerated()
		{
			return true;
		}

		public double GetMusicVolume()
		{
			return mMusicVolume;
		}

		public virtual void SetMusicVolume(double theVolume)
		{
			mMusicVolume = theVolume;
		}

		public double GetSfxVolume()
		{
			return mSfxVolume;
		}

		public virtual void SetSfxVolume(double theVolume)
		{
			mSfxVolume = theVolume;
			mSoundManager.SetVolume(theVolume);
		}

		public void EnableMusic(bool enable)
		{
			mMusicEnabled = enable;
		}

		public virtual void ModalOpen()
		{
		}

		public virtual void ModalClose()
		{
		}

		public void SafeDeleteWidget(Widget theWidget)
		{
			theWidget.Dispose();
		}

		public bool KillDialog(int theDialogId, bool removeWidget, bool deleteWidget)
		{
			if (mDialogMap.ContainsKey(theDialogId))
			{
				Dialog dialog = mDialogMap[theDialogId];
				mDialogList.Remove(dialog);
				mDialogMap.Remove(theDialogId);
				if (removeWidget || deleteWidget)
				{
					mWidgetManager.RemoveWidget(dialog);
				}
				if (dialog.IsModal())
				{
					ModalClose();
					mWidgetManager.RemoveBaseModal(dialog);
				}
				if (deleteWidget)
				{
					SafeDeleteWidget(dialog);
				}
				return true;
			}
			return false;
		}

		public virtual void ShowResourceError(bool boolean)
		{
		}

		public virtual void ShowAchievementMessage(TrialAchievementAlert alert)
		{
		}

		public virtual void Start()
		{
			if (mAutoStartLoadingThread)
			{
				StartLoadingThread();
			}
		}

		public virtual bool KillDialog(int theDialogId)
		{
			return KillDialog(theDialogId, true, true);
		}

		public virtual bool KillDialog(Dialogs theDialogId)
		{
			return KillDialog((int)theDialogId);
		}

		public virtual bool KillDialog(Dialog theDialog)
		{
			return KillDialog(theDialog.mId);
		}

		public void AddDialog(int theDialogId, Dialog theDialog)
		{
			KillDialog(theDialogId);
			if (theDialog.mWidth == 0)
			{
				int num = mWidth / 2;
				theDialog.Resize((mWidth - num) / 2, mHeight / 5, num, theDialog.GetPreferredHeight(num));
			}
			mDialogMap.Add(theDialogId, theDialog);
			mDialogList.AddLast(theDialog);
			mWidgetManager.AddWidget(theDialog);
			if (theDialog.IsModal())
			{
				mWidgetManager.AddBaseModal(theDialog);
				ModalOpen();
			}
		}

		public void AddDialog(Dialog theDialog)
		{
			AddDialog(theDialog.mId, theDialog);
		}

		public virtual Dialog DoDialog(Dialog theDialog, int theDialogId)
		{
			KillDialog(theDialogId);
			AddDialog(theDialogId, theDialog);
			return theDialog;
		}

		public int GetDialogCount()
		{
			return mDialogMap.Count();
		}

		public Dialog GetDialog(int theDialogId)
		{
			foreach (KeyValuePair<int, Dialog> item in mDialogMap)
			{
				if (item.Key == theDialogId)
				{
					return item.Value;
				}
			}
			return null;
		}

		public Dialog GetDialog(Dialogs theDialogId)
		{
			return GetDialog((int)theDialogId);
		}

		public void SetString(string theId, string theValue)
		{
			if (mStringProperties.ContainsKey(theId))
			{
				mStringProperties[theId] = theValue;
			}
			else
			{
				mStringProperties.Add(theId, theValue);
			}
		}

		public string GetString(string theId)
		{
			if (mStringProperties.ContainsKey(theId))
			{
				return mStringProperties[theId];
			}
			return "";
		}

		public double GetLoadingThreadProgress()
		{
			if (mLoaded)
			{
				return 1.0;
			}
			if (!mLoadingThreadStarted)
			{
				return 0.0;
			}
			return ((double)mResourceManager.mLoadedCount + (double)ReanimatorXnaHelpers.mLoadedResources) / (double)(mResourceManager.mTotalResources + ReanimatorXnaHelpers.mTotalResources);
		}

		public SexyColor HSLToRGB(int h, int s, int l)
		{
			return new SexyColor(h, s, l);
		}

		public virtual void LeftTrialMode()
		{
		}

		public bool WriteBufferToFile(string theFileName, Buffer theBuffer)
		{
			try
			{
				string directoryName = Path.GetDirectoryName(theFileName);
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				File.WriteAllBytes(theFileName, theBuffer.Data);
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				return false;
			}
			return true;
		}

		public bool ReadBufferFromFile(string theFileName, ref Buffer theBuffer, bool dontWriteToDemo)
		{
			try
			{
				if (!File.Exists(theFileName))
				{
					return false;
				}
				byte[] array = new byte[File.OpenRead(theFileName).Length];
				theBuffer.Data = File.ReadAllBytes(theFileName);
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				return false;
			}
			return true;
		}

		private void TransformTouch(_Touch touch)
		{
		}

		public void MousePosition(int x, int y)
        {
			mWidgetManager.MousePosition(x, y);
		}

		public void TouchBegan(_Touch touch)
		{
			mWidgetManager.TouchBegan(touch);
		}

		public void TouchMoved(_Touch touch)
		{
			mWidgetManager.TouchMoved(touch);
		}

		public void TouchEnded(_Touch touch)
		{
			mWidgetManager.TouchEnded(touch);
		}

		public void TouchesCanceled()
		{
			mWidgetManager.TouchesCanceled();
		}

		public bool KeyChar(SexyChar theChar)
		{
			return mWidgetManager.KeyChar(theChar);
		}

		public void ShakeBegan(double timestamp)
		{
		}

		public void ShakeEnded(double timestamp)
		{
		}

		public void ShakeCancelled(double timestamp)
		{
		}

		public virtual void BuyGame()
		{
			//Guide.ShowMarketplace(PlayerIndex.One);
		}

		public void ShowUpdateRequiredMessage()
		{
			//wantToShowUpdateMessage = true;
			//Main.GamerServicesComp.Enabled = false;
		}

		public virtual void DialogButtonPress(int theDialogId, int theButtonId)
		{
			switch (theButtonId)
			{
			case 1000:
				ButtonPress(2000 + theDialogId);
				break;
			case 1001:
				ButtonPress(3000 + theDialogId);
				break;
			}
		}

		public virtual void DialogButtonDepress(int theDialogId, int theButtonId)
		{
			switch (theButtonId)
			{
			case 1000:
				ButtonDepress(2000 + theDialogId);
				break;
			case 1001:
				ButtonDepress(3000 + theDialogId);
				break;
			}
		}

		public virtual void ButtonPress(int theId)
		{
		}

		public virtual void ButtonPress(int theId, int theClickCount)
		{
		}

		public virtual void ButtonDepress(int theId)
		{
		}

		public void ButtonDownTick(int theId)
		{
		}

		public void ButtonMouseEnter(int theId)
		{
		}

		public void ButtonMouseLeave(int theId)
		{
		}

		public void ButtonMouseMove(int theId, int theX, int theY)
		{
		}

		public virtual void GotFocus()
		{
		}

		public virtual void LostFocus()
		{
		}

		public virtual void Tombstoned()
		{
		}

		public UI_ORIENTATION GetOrientation()
		{
			return mInterfaceOrientation;
		}

		public virtual bool ShouldAutorotateToInterfaceOrientation(UI_ORIENTATION theOrientation)
		{
			return !mIsOrientationLocked;
		}

		public virtual void WriteToRegistry()
		{
		}

		public bool RegistryWriteString(string theValueName, string theString)
		{
			return true;
		}

		public int RegistryReadInteger(string theValueName)
		{
			return 0;
		}

		public void RegistryWriteInteger(string theValueName, int theValue)
		{
		}

		public void DoVibration()
		{
			//VibrateController @default = VibrateController.get_Default();
			//@default.Start(TimeSpan.FromMilliseconds(500.0));
		}

		public virtual void Shutdown()
		{
			mShutdown = true;
		}

		public void LockOrientation(bool theFlag)
		{
			if (theFlag)
			{
				GraphicsState.mGraphicsDeviceManager.SupportedOrientations = XnaGame.Window.CurrentOrientation;
			}
			else
			{
				GraphicsState.mGraphicsDeviceManager.SupportedOrientations = Constants.SupportedOrientations;
			}
			if (XnaGame.Window.CurrentOrientation == DisplayOrientation.Portrait)
			{
				GraphicsState.mGraphicsDeviceManager.PreferredBackBufferWidth = Constants.BackBufferSize.X;
				GraphicsState.mGraphicsDeviceManager.PreferredBackBufferHeight = Constants.BackBufferSize.Y;
			}
			else
			{
				GraphicsState.mGraphicsDeviceManager.PreferredBackBufferWidth = Constants.BackBufferSize.Y;
				GraphicsState.mGraphicsDeviceManager.PreferredBackBufferHeight = Constants.BackBufferSize.X;
			}
			GraphicsState.mGraphicsDeviceManager.ApplyChanges();
		}

		protected void ProcessSafeDeleteList()
		{
		}

		public virtual void MoviePlayerContentPreloadDidFinish(bool succeeded)
		{
		}

		public virtual void MoviePlayerPlaybackDidFinish()
		{
		}

		public bool PlayMovie(VideoType video, MOVIESCALINGMODE mOVIESCALINGMODE, MOVIECONTROLMODE mOVIECONTROLMODE, SexyColor sexyColor)
		{
			bool flag = true;
			try
			{
				if (video == VideoType.Credits)
				{
					/*VideoPlayer.set_Media(new Uri("Content/video/credits.wmv", UriKind.Relative));
					VideoPlayer.set_Location((MediaLocationType)1);
					VideoPlayer.set_Controls((MediaPlaybackControls)31);
					VideoPlayer.Show();*/
				}
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				flag = false;
				GotFocus();
			}
			MoviePlayerContentPreloadDidFinish(flag);
			return flag;
		}

		public bool IsLandscape()
		{
			if (mInterfaceOrientation != UI_ORIENTATION.UI_ORIENTATION_LANDSCAPE_LEFT)
			{
				return mInterfaceOrientation == UI_ORIENTATION.UI_ORIENTATION_LANDSCAPE_RIGHT;
			}
			return true;
		}

		public bool IsPortrait()
		{
			return !IsLandscape();
		}

		public virtual bool BackButtonPress()
		{
			if (!mLoadingThreadCompleted)
			{
				WantsToExit = true;
			}
			return mWidgetManager.BackButtonPress();
		}

		public virtual void AppExit()
		{
			WantsToExit = true;
			Shutdown();
		}
	}
}
