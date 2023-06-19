using Build1.PostMVC.Core.MVCS.Mediation;
using Build1.PostMVC.Unity.App.Mediation;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Event = Build1.PostMVC.Core.MVCS.Events.Event;

namespace Components.Screens.AppLoading
{
    [Mediator(typeof(AppLoadingScreenMediator))]
    public sealed class AppLoadingScreenView : UnityViewDispatcher
    {
        public readonly Event OnComplete = new();
        
        [Header("Components"), SerializeField] private Slider          progressBar;
        [SerializeField]                       private TextMeshProUGUI textProgress;
        [SerializeField]                       private TextMeshProUGUI textTip01;
        [SerializeField]                       private TextMeshProUGUI textTip02;

        [Header("Progress"), SerializeField, Range(0.1f, 10f)] private float  progressAnimationDurationSeconds          = 1.0F;
        [SerializeField, Range(0.1f, 10f)]                     private float  progressAnimationBufferingDurationSeconds = 2.0F;
        [SerializeField, Range(0.01f, 0.25f)]                  private float  progressAnimationBuffer                   = 0.10F;
        [SerializeField]                                       private string progressFormatString                      = "Loading {0}%";

        private Tween _tweenProgress;
        private Tween _tweenProgressBuffer;

        [OnEnable]
        private void OnEnabled()
        {
            textTip01.alpha = 0;
            textTip02.alpha = 0;

            progressBar.value = 0;
            UpdateProgressImpl();
        }

        /*
         * Progress.
         */

        public void SetProgress(float progress)
        {
            if (progress <= progressBar.value)
                return;

            if (_tweenProgress != null)
            {
                _tweenProgress.Kill();
                _tweenProgress = null;
            }

            if (_tweenProgressBuffer != null)
            {
                _tweenProgressBuffer.Kill();
                _tweenProgressBuffer = null;
            }

            _tweenProgress = progressBar.DOValue(progress, progressAnimationDurationSeconds)
                                        .OnUpdate(UpdateProgressImpl)
                                        .OnComplete(OnProgressComplete);
        }

        private void UpdateProgressImpl()
        {
            if (textProgress == null)
                return;

            var value = Mathf.CeilToInt(progressBar.value * 100);
            if (value > 0 && value < 99)
                value -= 1;
            textProgress.text = string.Format(progressFormatString, value);
        }

        private void OnProgressComplete()
        {
            if (progressBar.value > 0.99)
            {
                Dispatch(OnComplete);
                return;
            }

            var value = Mathf.Min(progressBar.value + progressAnimationBuffer, 0.99F);
            _tweenProgressBuffer = progressBar.DOValue(value, progressAnimationBufferingDurationSeconds)
                                              .OnUpdate(UpdateProgressImpl);
        }
    }
}