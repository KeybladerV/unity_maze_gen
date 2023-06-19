using System;
using System.Collections.Generic;
using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Unity.App.Modules.Assets;
using Build1.PostMVC.Unity.App.Modules.Logging;

namespace Modules.Assets.Commands
{
    public sealed class AssetsLoadContentGroupsCommand : Command
    {
        [Log(LogLevel.Warning)] public ILog Log { get; set; }

        [Inject] public IAssetsController    AssetsController    { get; set; }

        private int _totalBundles;
        private int _completedBundles;
        private int _failedBundles;

        public override void Execute()
        {
            Retain();

            var bundleIds = GetContentGroupsBundleIds();

            _totalBundles = bundleIds.Count;

            ReleaseIfAllProcessed();

            foreach (var bundleId in bundleIds)
            {
                if (AssetsController.CheckBundleLoaded(bundleId))
                {
                    _completedBundles++;

                    Log.Debug(b => $"Bundle already loaded: {b}", bundleId);

                    ReleaseIfAllProcessed();
                    continue;
                }

                Log.Debug(i => $"Loading asset bundle: {i}", bundleId);

                AssetsController.LoadEmbedBundle(bundleId, OnComplete, OnError);
            }
        }

        private IList<AssetBundleId> GetContentGroupsBundleIds()
        {
            var ids = new List<AssetBundleId>();

            return ids;
        }

        private void OnComplete(AssetBundleInfo info)
        {
            _completedBundles++;

            Log.Debug(b => $"Loaded asset bundle: {b}", info);

            ReleaseIfAllProcessed();
        }

        private void OnError(AssetsException exception)
        {
            _failedBundles++;

            Log.Error(e => $"Failed to load asset bundle: {e}", exception);

            ReleaseIfAllProcessed();
        }

        private void ReleaseIfAllProcessed()
        {
            if (_completedBundles + _failedBundles < _totalBundles)
                return;

            if (_failedBundles > 0)
            {
                Fail(new Exception("Failed to load one or more asset bundles"));
            }
            else
            {
                Log.Debug(c => $"Loaded all asset bundles ({c} total)", _totalBundles);

                Release();
            }
        }
    }
}