using Build1.PostMVC.Core.MVCS.Commands;
using DG.Tweening;
using UnityEngine;

namespace Modules.App.Commands
{
    public sealed class AppDeinitializeCommand : Command
    {
        public override void Execute()
        {
            // To prevent null refs in editor.
            if (DOTween.instance != null)
                Object.Destroy(DOTween.instance.gameObject);
        }
    }
}