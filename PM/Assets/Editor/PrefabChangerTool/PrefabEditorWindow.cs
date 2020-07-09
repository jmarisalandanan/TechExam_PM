using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PM.PrefabChanger
{
    public class PrefabEditorWindow : EditorWindow
    {
        private List<Editor> cards = new List<Editor>();
        private PrefabConfiguration[] cardConfigurations;
        private string currentPath = "Assets";

        [MenuItem("Product Madness/Prefab Editor")]
        private static void Init()
        {
            var window = GetWindow<PrefabEditorWindow>();
            window.titleContent = new GUIContent("Prefab Editor");
            window.Show();
        }

        private void OnGUI()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Path to load:");
                currentPath = EditorGUILayout.TextField(currentPath);
                if (GUILayout.Button("Load"))
                {
                    FindCards();
                }
            }

            DrawPrefabs();
        }

        private void FindCards()
        {
            cards.Clear();

            var cardGuids = AssetDatabase.FindAssets("t:" + nameof(GameObject), new[] { currentPath });
            foreach (var cardGuid in cardGuids)
            {
                var cardPath = AssetDatabase.GUIDToAssetPath(cardGuid);
                var card = AssetDatabase.LoadAssetAtPath<Card>(cardPath);
                if (card != null)
                {
                    cards.Add(Editor.CreateEditor(card));
                }
            }

            DrawPrefabs();
        }

        private void DrawPrefabs()
        {
            if (cards.Count > 0)
            {
                EditorGUILayout.LabelField("Prefabs");
                using (new EditorGUI.IndentLevelScope())
                {
                    foreach (var cardEditor in cards)
                    {
                        DrawPrefab(cardEditor);
                    }
                }
            }
        }

        private void DrawPrefab(Editor cardEditor)
        {
            cardEditor.OnInspectorGUI();
            if (GUILayout.Button("Select Prefab"))
            {
                AssetDatabase.OpenAsset(cardEditor.target);
            }
        }
    }
}
