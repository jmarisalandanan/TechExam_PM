using UnityEngine;
using TMPro;

namespace PM.WheelMiniGame
{
    public class WheelItem : MonoBehaviour
    {
        [SerializeField]
        private int wheelValue;
        [SerializeField]
        private TextMeshProUGUI wheelValueText;

        public int WheelValue { get { return wheelValue; } }

        private void Awake()
        {
            wheelValueText.text = string.Format("{0}x", wheelValue);
        }
    }
}
