using TMPro;
using UnityEngine;
#pragma warning disable 0649

namespace enjoythevibes.UI
{
    [System.Serializable]
    public struct UIField
    {
        [SerializeField]
        private TextMeshProUGUI field;
        [SerializeField]
        private string fieldFormat;

        public void SetUp()
        {
            fieldFormat = field.text;
        }

        public void SetText(string text)
        {
            field.text = string.Format(fieldFormat, text);
        }

        public void SetPreferredWidth()
        {
            var size = field.GetPreferredValues(field.text);
            field.rectTransform.sizeDelta = new Vector2(size.x, field.rectTransform.sizeDelta.y);
        }
    }
}