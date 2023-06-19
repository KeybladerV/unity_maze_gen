using System;
using Build1.PostMVC.Core.Modules;
using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Unity.App.Modules.Assets;

namespace Modules.Assets
{
    public sealed class AssetsModule : Module
    {
        [Inject] public IAssetsController AssetsController { get; set; }

        [PostConstruct]
        public void PostConstruct()
        {
            AssetsController.OnBundleStringIdentifier += GetAssetBundleIdentifier;
            AssetsController.OnBundleInfoRequest += GetAssetBundleInfo;
        }

        [PreDestroy]
        public void PreDestroy()
        {
            AssetsController.OnBundleStringIdentifier -= GetAssetBundleIdentifier;
            AssetsController.OnBundleInfoRequest -= GetAssetBundleInfo;
        }

        private string GetAssetBundleIdentifier(Enum identifier)
        {
            return (AssetBundleId)identifier switch
            {
                AssetBundleId.BiomeOcean          => "biome_ocean",
                AssetBundleId.BiomeGrasslands     => "biome_grasslands",
                AssetBundleId.BiomeDesert         => "biome_desert",
                AssetBundleId.ContentGroupDefault => "content_group_default",
                _                                 => throw new ArgumentOutOfRangeException(nameof(identifier), identifier, null)
            };
        }

        private AssetBundleInfo GetAssetBundleInfo(string bundleName)
        {
            return bundleName switch
            {
                "biome_desert"     => AssetBundleInfo.FromId("biome_desert").AddAtlasesNames("DesertAtlas"),
                "biome_grasslands" => AssetBundleInfo.FromId("biome_grasslands").AddAtlasesNames("GrasslandsAtlas"),
                "biome_ocean"      => AssetBundleInfo.FromId("biome_ocean").AddAtlasesNames("OceanAtlas"),

                "content_group_default" => AssetBundleInfo.FromId("content_group_default")
                                                          .AddAtlasesNames("DefaultInsertsAtlas",
                                                                           "DefaultStructuresAtlas",
                                                                           "DefaultStaticsAtlas",
                                                                           "DefaultResearchAtlas",
                                                                           "DefaultThumbnailsAtlas",
                                                                           "DefaultCloudIdleAtlas",
                                                                           "DefaultCloudFadeAtlas"),
                _ => null
            };
        }
    }
}