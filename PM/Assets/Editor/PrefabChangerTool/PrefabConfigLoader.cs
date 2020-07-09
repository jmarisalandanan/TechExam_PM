using System.IO;
using UnityEditor;
using UnityEngine;
using PM.Common;

namespace PM.PrefabChanger
{
    public static class PrefabConfigLoader
    {
        private const string ASSET_PATH = "Assets";
        private const string CARD_DATA_PATH = "Data";
        private const string CARD_DATA_FILENAME = "data.json";

        private static PrefabConfiguration[] prefabConfigurations;

        public static PrefabConfiguration[] GetPrefabConfigData()
        {
            if (prefabConfigurations == null)
            {
                var jsonPath = Path.Combine(ASSET_PATH, CARD_DATA_PATH, CARD_DATA_FILENAME);
                var jsonString = AssetDatabase.LoadAssetAtPath<TextAsset>(jsonPath);
                prefabConfigurations = JsonHelper.FromJson<PrefabConfiguration>(jsonString.text);
            }

            return prefabConfigurations;
        }
    }
}
