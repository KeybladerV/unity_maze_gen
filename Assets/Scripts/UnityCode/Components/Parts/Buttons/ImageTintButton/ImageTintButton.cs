using UnityEngine;
using UnityEngine.UI;

namespace Components.Parts.Buttons.ImageTintButton
{
    // TODO: remove
    public sealed class ImageTintButton : Button
    {
        [SerializeField] private Color inactiveIconColor;
        [SerializeField] private Color activeIconColor;

        [Header("Parts")]
        [SerializeField] private Image iconImage;

        public void SetSelected()
        {
            interactable = false;
            iconImage.color = activeIconColor;
        }

        public void SetNotSelected()
        {
            interactable = true;
            iconImage.color = inactiveIconColor;
        }
    }
}