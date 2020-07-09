using System;
using UnityEngine;

namespace PM.PrefabChanger
{
    [Serializable]
    public class PrefabConfiguration
    {
        [SerializeField]
        private string text;
        [SerializeField]
        private string color;
        [SerializeField]
        private string image;

        public string Text { get { return text; } }
        public string Color { get { return color; } }
        public string Image { get { return image; } }
    }
}
