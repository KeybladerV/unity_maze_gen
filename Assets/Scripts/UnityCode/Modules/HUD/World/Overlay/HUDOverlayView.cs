using System;
using Build1.PostMVC.Core.MVCS.Mediation;
using Build1.PostMVC.Unity.App.Mediation;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Event = Build1.PostMVC.Core.MVCS.Events.Event;

namespace Components.HUD.World.Overlay
{
    [Mediator(typeof(HUDOverlayMediator))]
    public sealed class HUDOverlayView : UnityViewDispatcher, IHUDOverlayView
    {
        public static readonly Event OnProfile       = new(typeof(HUDOverlayView), nameof(OnProfile));
        public static readonly Event OnZoom          = new(typeof(HUDOverlayView), nameof(OnZoom));

        [Header("Parts"), SerializeField] public  GameObject groupLeft;
        [SerializeField]                  public  GameObject groupRight;
        [SerializeField]                  public  Button     buttonProfile;
        [SerializeField]                  public  Button     buttonZoom;

        private Tween _fadeTween;

        [Start]
        public void OnStart()
        {
            BindUnityEvent(buttonProfile.onClick, OnProfile);
            BindUnityEvent(buttonZoom.onClick, OnZoom);

            SetRightGroupVisible(true);
        }

        [OnDestroy]
        public void OnDestroying()
        {
            UnbindAllUnityEvents();
        }

        /*
         * Public.
         */

        public void SetState(HUDOverlayState state)
        {
            switch (state)
            {
                case HUDOverlayState.Invisible:
                    groupRight.SetActive(false);
                    groupLeft.SetActive(false);
                    break;
                case HUDOverlayState.ZoomOnly:
                    buttonProfile.gameObject.SetActive(false);
                    
                    buttonZoom.gameObject.SetActive(true);
                    groupRight.gameObject.SetActive(true);
                    break;
                case HUDOverlayState.AllVisible:
                    groupLeft.gameObject.SetActive(true);
                    groupRight.gameObject.SetActive(true);
                    
                    buttonProfile.gameObject.SetActive(true);
                    buttonZoom.gameObject.SetActive(true);
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
        
        public void SetRightGroupVisible(bool visible)
        {
            groupRight.SetActive(visible);
        }
    }
}