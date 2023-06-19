using System;
using Build1.PostMVC.Core.MVCS.Mediation;
using Build1.PostMVC.Unity.App.Mediation;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Components.HUD.World.Overlay
{
    [Mediator(typeof(HUDFadeOverlayMediator))]
    public sealed class HUDFadeOverlayView : UnityViewDispatcher, IHUDFadeOverlayView
    {
        [SerializeField] private Image fader;

        public HUDFadeOverlayState State { get; private set; }
        public bool IsInProcessOfFade { get; private set; }

        private Tween _fadeTween;

        /*
         * Public.
         */

        public void SetState(HUDFadeOverlayState state)
        {
            if (State == state)
                return;

            var fadeValue = state switch
            {
                HUDFadeOverlayState.FadeIn => 0,
                HUDFadeOverlayState.FadeOut => 1,
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };

            var color = fader.color;
            color.a = fadeValue;
            fader.color = color;

            State = state;
        }

        public void DoFade(float fadeValue, float duration, Action callback)
        {
            State = fadeValue == 0 ? HUDFadeOverlayState.FadeIn : HUDFadeOverlayState.FadeOut;

            _fadeTween?.Kill(true);
            _fadeTween = fader.DOFade(fadeValue, duration)
                .OnStart(() => IsInProcessOfFade = true)
                .OnComplete(() =>
                {
                    callback?.Invoke();
                    IsInProcessOfFade = false;
                });
        }
    }
}