using UnityEngine;
using UnityEngine.UI;
using TMPro;
using enjoythevibes.Data;

namespace enjoythevibes.UI
{
    public class ShopElement : MonoBehaviour
    {
        [SerializeField]
        private Image elementImage = default;
        [SerializeField]
        private TextMeshProUGUI priceField = default;
        
        public GradientSchemeScriptableObject gradientScheme { private set; get; }
        private Color deselectedColor = EngineSettings.ShopUI.DeselectedColor;

        public void SetImageAndPrice(Sprite sprite, GradientSchemeScriptableObject gradientScheme, UnityEngine.Events.UnityAction actionOnClick)
        {
            this.gradientScheme = gradientScheme;
            elementImage.sprite = sprite;
            priceField.text = gradientScheme.Price.ToString();
            elementImage.GetComponent<Button>().onClick.AddListener(actionOnClick);
            Deselect();
        }

        public void SetPurchased()
        {
            deselectedColor = EngineSettings.ShopUI.PurchasedDeselectedColor;
        }

        public void Select()
        {
            elementImage.color = EngineSettings.ShopUI.SelectedColor;
        }

        public void Deselect()
        {
            elementImage.color = deselectedColor;
        }        
    }
}