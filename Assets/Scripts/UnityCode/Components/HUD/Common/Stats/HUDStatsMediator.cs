using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Core.MVCS.Mediation;
using Build1.PostMVC.Unity.App.Mediation;
using Build1.PostMVC.Unity.App.Modules.Async;
using Build1.PostMVC.Unity.App.Modules.Update;
using Unity.Profiling;
using UnityEngine;

namespace Components.HUD.Common.Stats
{
    public sealed class HUDStatsMediator : Mediator
    {
        [Inject] public HUDStatsView      View             { get; set; }
        [Inject] public IAsyncResolver    AsyncResolver    { get; set; }
        [Inject] public IUpdateController UpdateController { get; set; }

        private int              _callId;
        private ProfilerRecorder _totalUsedMemoryRecorder;
        private ProfilerRecorder _totalReservedMemoryRecorder;
        private int              _fps;

        [OnEnable]
        public void OnEnabled()
        {
            _totalUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Used Memory");
            _totalReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Reserved Memory");

            _callId = AsyncResolver.IntervalCall(UpdateStats, 0.5F);
            
            UpdateController.SubscribeForUpdate(OnUpdate);
        }

        [OnDisable]
        public void OnDisabled()
        {
            _totalUsedMemoryRecorder.Dispose();
            _totalReservedMemoryRecorder.Dispose();

            AsyncResolver.CancelCall(ref _callId);
            
            UpdateController.Unsubscribe(OnUpdate);
        }

        /*
         * Stats.
         */

        private void OnUpdate(float deltaTime)
        {
            _fps = Mathf.RoundToInt(1 / deltaTime);
        }
        
        private void UpdateStats()
        {
            View.SetFPS(_fps);
            
            if (_totalUsedMemoryRecorder.Valid && _totalReservedMemoryRecorder.Valid)
                View.SetMemory(_totalUsedMemoryRecorder.CurrentValue, _totalReservedMemoryRecorder.CurrentValue);
        }
    }
}