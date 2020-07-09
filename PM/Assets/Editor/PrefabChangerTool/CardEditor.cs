using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;

namespace PM.PrefabChanger
{
    [CustomEditor(typeof(Card))]
    public class CardEditor : Editor
    {
        [SerializeField]
        private PrefabConfiguration selectedConfig;

        private SerializedProperty cardTextProperty;
        private SerializedProperty cardImageProperty;
        private SerializedProperty cardColorProperty;
        private bool showFields = false;
        private PrefabStage prefabStage;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.LabelField(string.Format("Prefab: {0}", target.name));

            showFields = EditorGUILayout.Foldout(showFields, "Fields");
            if (showFields)
            {
                EditorGUILayout.PropertyField(cardTextProperty);
                EditorGUILayout.PropertyField(cardImageProperty);
                EditorGUILayout.PropertyField(cardColorProperty);
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                var selectionText = selectedConfig != null ? selectedConfig.Text : "Select Config";
                if (GUILayout.Button(selectionText))
                {
                    GenericMenu menu = new GenericMenu();
                    foreach (var config in PrefabConfigLoader.GetPrefabConfigData())
                    {
                        menu.AddItem(new GUIContent(config.Text), false, OnConfigSelected, config);
                    }
                    menu.ShowAsContext();
                }

                using (new EditorGUI.DisabledScope(selectedConfig == null))
                {
                    if (GUILayout.Button("Apply"))
                    {
                        OnApplyClicked();
                    }

                    if (GUILayout.Button("Revert"))
                    {
                        OnRevertClicked();
                    }
                }
            }
            serializedObject.ApplyModifiedProperties();
        }

        private void OnApplyClicked()
        {
            PrefabUtility.SaveAsPrefabAsset(prefabStage.prefabContentsRoot, prefabStage.prefabAssetPath);
            selectedConfig = null;
        }

        private void OnRevertClicked()
        {
            prefabStage.ClearDirtiness();
            AssetDatabase.OpenAsset(target);
            selectedConfig = null;
        }

        private void OnConfigSelected(object value)
        {
            selectedConfig = (PrefabConfiguration)value;
            AssetDatabase.OpenAsset(target);
            prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            prefabStage.prefabContentsRoot.GetComponent<Card>().ApplyConfig(selectedConfig);
        }

        private void OnEnable()
        {
            cardTextProperty = serializedObject.FindProperty("cardText");
            cardImageProperty = serializedObject.FindProperty("cardImage");
            cardColorProperty = serializedObject.FindProperty("cardColor");
        }
    }
}
