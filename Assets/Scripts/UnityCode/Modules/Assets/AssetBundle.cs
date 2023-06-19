using Build1.PostMVC.Unity.App.Modules.Assets;

namespace Modules.Assets
{
    public static class AssetBundle
    {
        public static readonly AssetBundleInfo AppLoading = AssetBundleInfo.FromId("app_loading")
                                                                           .AddAtlasesNames("AppLoadingAtlas");

        public static readonly AssetBundleInfo UI = AssetBundleInfo.FromId("ui")
                                                                   .AddAtlasesNames("UIAtlas", "HUDAtlas", "StoreAtlas");
    }
}