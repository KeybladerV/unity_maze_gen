using System;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Mediation;
using Build1.PostMVC.Unity.App.Mediation;
using Build1.PostMVC.Unity.App.Modules.UI.Popups;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Components.Popups.Generic
{
    // TODO: make Generic popup stretchable

    [Mediator(typeof(GenericPopupMediator))]
    public sealed class GenericPopupView : PopupViewDispatcher
    {
        public static readonly Event<GenericPopupButton> OnButton = new(typeof(GenericPopupView), nameof(OnButton));

        [Header("Generic"), SerializeField] private TextMeshProUGUI textTitle;
        [SerializeField]                    private Image           textTitleBackground;
        [SerializeField]                    private TextMeshProUGUI textMessage;
        [SerializeField]                    private GameObject      buttonsContainer;
        [SerializeField]                    private Button          buttonYes;
        [SerializeField]                    private TextMeshProUGUI buttonYesLabel;
        [SerializeField]                    private Button          buttonNo;
        [SerializeField]                    private TextMeshProUGUI buttonNoLabel;

        [Header("Titles"), SerializeField] private Sprite spriteTitleDefault;
        [SerializeField]                   private Sprite spriteTitleCongratulation;
        [SerializeField]                   private Sprite spriteTitleWarning;
        [SerializeField]                   private Sprite spriteTitleError;

        [Start]
        private void OnStart()
        {
            BindUnityEvent(buttonYes.onClick, OnButton, GenericPopupButton.Yes);
            BindUnityEvent(buttonNo.onClick, OnButton, GenericPopupButton.No);
        }

        [OnDestroy]
        private void OnDestroying()
        {
            UnbindAllUnityEvents();
        }

        /*
         * Public.
         */

        public void ShowData(GenericPopupData data)
        {
            if (Overlay)
                Overlay.SetActive(data.OverlayVisible);

            if (string.IsNullOrWhiteSpace(data.Title))
            {
                textTitleBackground.gameObject.SetActive(false);
            }
            else
            {
                textTitle.text = data.Title;

                textTitleBackground.gameObject.SetActive(true);
                textTitleBackground.sprite = data.TitleStyle switch
                {
                    GenericPopupTitleStyle.Default        => spriteTitleDefault,
                    GenericPopupTitleStyle.Congratulation => spriteTitleCongratulation,
                    GenericPopupTitleStyle.Warning        => spriteTitleWarning,
                    GenericPopupTitleStyle.Error          => spriteTitleError,
                    _                                     => spriteTitleDefault
                };
            }

            if (string.IsNullOrWhiteSpace(data.Message))
            {
                textMessage.gameObject.SetActive(false);
            }
            else
            {
                textMessage.text = data.Message;
                textMessage.gameObject.SetActive(true);
            }

            TryShowButton(buttonYes, buttonYesLabel, data.HasOkButton, GenericPopupButton.Yes, data);
            TryShowButton(buttonNo, buttonNoLabel, data.HasNoButton, GenericPopupButton.No, data);

            buttonsContainer.SetActive(data.HasOkButton || data.HasNoButton);
        }

        private void TryShowButton(Component button, TMP_Text textLabel, bool buttonEnabled, GenericPopupButton popupButton, GenericPopupData data)
        {
            button.gameObject.SetActive(buttonEnabled);

            if (buttonEnabled)
                textLabel.text = data.GetButtonLabel(popupButton);
        }
    }
}