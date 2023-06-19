using TMPro;
using UnityEngine;

namespace Components.Parts.Title
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    public sealed class TitleComponent : MonoBehaviour
    {
        [Header("Parts"), SerializeField] private TextMeshProUGUI title;

        public string Title
        {
            get => title.text;
            set => title.text = value;
        }

        #if UNITY_EDITOR

        private void Awake()
        {
            title ??= GetComponentInChildren<TextMeshProUGUI>(true);
        }
        
        #endif
    }
}