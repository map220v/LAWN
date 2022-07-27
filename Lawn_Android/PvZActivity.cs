using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Lawn_Android
{
    [Activity(
        Label = "@string/app_name",
        MainLauncher = true,
        Icon = "@drawable/icon",
        AlwaysRetainTaskState = true,
        LaunchMode = LaunchMode.SingleInstance,
        Immersive = true,
        MultiProcess = true,
        ScreenOrientation = ScreenOrientation.SensorLandscape,
        ConfigurationChanges = ConfigChanges.Orientation
        | ConfigChanges.Keyboard
        | ConfigChanges.KeyboardHidden
        | ConfigChanges.ScreenSize
        // Configs NOT ACTUALLY handled, but since Destroying the activity leads to crash, here to pretend handling them
        | ConfigChanges.Mnc
        | ConfigChanges.Mcc
        | ConfigChanges.Locale
        | ConfigChanges.UiMode
        | ConfigChanges.Navigation
        | ConfigChanges.Orientation
        | ConfigChanges.ScreenLayout
        | ConfigChanges.SmallestScreenSize
        | ConfigChanges.Density
        | ConfigChanges.FontScale
        | ConfigChanges.ColorMode
        | ConfigChanges.Touchscreen
        | ConfigChanges.LayoutDirection
    )]
    public class PvZActivity : MonoGame.IMEHelper.AndroidGameActivityIME//AndroidGameActivity
    {
        private Sexy.Main _game;
        private View _view;
        public static Action<Exception> ExceptionReporter = (e) => { };

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Sexy.GlobalStaticVars.gPvZActivity = this;
            //#if !DEBUG

            try
            {
                //#endif
                _game = new Sexy.Main();
                _view = _game.Services.GetService(typeof(View)) as View;
                _view.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.Fullscreen;//| (StatusBarVisibility)SystemUiFlags.HideNavigation | (StatusBarVisibility)SystemUiFlags.ImmersiveSticky;

                SetContentView(_view);
                _game.Run();
                //#if !DEBUG
            }
            catch (Exception err)
            {
                OnException(this, err);
            }
            //#endif

        }

        protected override void OnDestroy()
        {
            //(_view.Parent as ViewGroup).RemoveView(_view);  // 解绑_view以便重新使用
            Sexy.Debug.Log("Destroy...");
            base.OnDestroy();
            Sexy.GlobalStaticVars.gSexyAppBase.AppExit();
            _game.Exit();
        }

        public void OnException(object sender, Exception err)
        {
#if !DEBUG
            ExceptionReporter(err);
#endif
            var aDialog = new AlertDialog.Builder(this);
            aDialog.SetTitle(err.GetType().ToString());
            aDialog.SetMessage($"{err.Message}\n{err.StackTrace}");
            aDialog.SetPositiveButton("Close", delegate { });
            aDialog.Show();
        }

        internal void ConfigureWorkDirAsLocalData()
        {
            Directory.SetCurrentDirectory(GetExternalFilesDir("").AbsolutePath);
        }
    }
}
