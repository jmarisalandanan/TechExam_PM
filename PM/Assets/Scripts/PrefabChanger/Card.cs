using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PM.PrefabChanger
{
    public class Card : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI cardText;
        [SerializeField]
        private Image cardImage;
        [SerializeField]
        private Color cardColor;

#if UNITY_EDITOR
        [SerializeField]
        private PrefabConfiguration currentConfiguration;

        public void ApplyConfig(PrefabConfiguration config)
        {
            currentConfiguration = config;
            cardText.SetText(config.Text);

            Color newColor;
            if (ColorUtility.TryParseHtmlString(config.Color, out newColor))
            {
                cardColor = newColor;
            }
            else
            {
                cardColor = Color.magenta;
                Debug.LogErrorFormat("Error in parsing html string with color value: {0}", config.Color);
            }

            // Ideally we want this to be combined by `Path.Combine` but as json data seems to show a path
            // I decided to just hack the `Assets/` directory into the path as I try to avoid manual changes
            // to the json.
            var loadedSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/" + config.Image);
            if (loadedSprite == null)
            {
                Debug.LogErrorFormat("Cannot find sprite {0}", config.Image);
            }
            else
            {
                cardImage.sprite = loadedSprite;
            }
        }
#endif
    }
}
