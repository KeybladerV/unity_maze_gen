using Build1.PostMVC.Unity.App.Modules.Device;
using Build1.PostMVC.Unity.App.Modules.UI;
using Build1.PostMVC.Unity.App.Modules.UI.Screens;
using Modules.Assets;
using ScreenInfo = Build1.PostMVC.Unity.App.Modules.UI.Screens.Screen;

namespace Modules.Screens
{
    public static class Screen
    {
        public static readonly ScreenInfo Loading = new("Loading", UIBehavior.SingleInstance | UIBehavior.DestroyOnDeactivation)
        {
            new ScreenConfig("LoadingScreen.prefab", RootLayer.SystemScreensCanvas, AssetBundle.AppLoading).SetDeviceType(DeviceType.Desktop)
        };
        
        public static readonly ScreenInfo MainScreen = new("MainMenu", UIBehavior.SingleInstance | UIBehavior.DestroyOnDeactivation)
        {
            new ScreenConfig("MainScreen.prefab", RootLayer.OverlayScreensCanvas, AssetBundle.UI).SetDeviceType(DeviceType.Desktop)
        };
    }
}