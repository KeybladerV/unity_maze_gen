using System.Collections.Generic;
using Build1.PostMVC.Core.MVCS.Mediation;
using Build1.PostMVC.Unity.App.Mediation;
using TMPro;
using UnityEngine;

namespace Components.HUD.Common.Stats
{
    [Mediator(typeof(HUDStatsMediator))]
    public sealed class HUDStatsView : UnityViewDispatcher
    {
        [Header("Parts"), SerializeField] private TextMeshProUGUI textFPS;
        [SerializeField]                  private TextMeshProUGUI textMemory;

        private readonly Dictionary<int, string> _fpsStrings = new();
        
        /*
         * Public.
         */

        public void SetFPS(int fps)
        {
            if (!_fpsStrings.TryGetValue(fps, out var str))
            {
                str = $"FPS: {fps}";
                _fpsStrings.Add(fps, str);
            }
            
            textFPS.text = str;
        }
        
        public void SetMemory(long totalUsedMemory, long totalReservedMemory)
        {
            textMemory.text = $"RAM: {ToMegaBytes(totalUsedMemory):n00}/{ToMegaBytes(totalReservedMemory):n00} MB";
        }
        
        /*
         * Private.
         */
        
        private static float ToMegaBytes(long bytes)
        {
            return bytes / 1024f / 1024f;
        }
    }
}