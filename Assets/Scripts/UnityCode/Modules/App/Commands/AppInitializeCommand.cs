using System.Globalization;
using Build1.PostMVC.Core.MVCS.Commands;
using DG.Tweening;
using UnityEngine;

namespace Modules.App.Commands
{
    public sealed class AppInitializeCommand : Command
    {
        public override void Execute()
        {
            Application.targetFrameRate = 144; // 120 FPS enabled Apple Pro Motion on supported devices.
            
            // Setting default culture. Otherwise users from Turkey can't use the app.
            var culture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            
            DOTween.useSmoothDeltaTime = true;
            DOTween.Init(true, false, LogBehaviour.Default).SetCapacity(20, 5);
        }
    }
}