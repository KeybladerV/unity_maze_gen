#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.UI;

namespace Components.Parts.Buttons.ImageTintButton
{
    [CustomEditor(typeof(ImageTintButton))]
    public sealed class ImageTintButtonEditor : ButtonEditor
    {
        private SerializedProperty inactiveIconColor;
        private SerializedProperty activeIconColor;
        private SerializedProperty iconImage;

        protected override void OnEnable()
        {
            base.OnEnable();

            inactiveIconColor = serializedObject.FindProperty("inactiveIconColor");
            activeIconColor = serializedObject.FindProperty("activeIconColor");
            iconImage = serializedObject.FindProperty("iconImage");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            EditorGUILayout.PropertyField(inactiveIconColor);
            EditorGUILayout.PropertyField(activeIconColor);
            EditorGUILayout.LabelField("Parts", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(iconImage);
            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif