using System.Collections.Generic;
using enjoythevibes.Data;
using enjoythevibes.Managers;
using UnityEngine;

namespace enjoythevibes.UI
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField]
        private Transform elementsTransform = default;
        [SerializeField]
        private GameObject elementPrefab = default;

        [SerializeField]
        private UIField crystalsField = default;

        [SerializeField]
        private GameObject buyButton = default;

        private Dictionary<int, ShopElement> shopElements = new Dictionary<int, ShopElement>();
        private int selectedElementID;

        private void Awake()
        {
            EventsManager.AddListener(Events.GameInitialization, Initialize);
            EventsManager.AddListener(Events.OpenShop, OnEnableShopUI);
            EventsManager.AddListener(Events.CloseShop, OnDisableShopUI);
            crystalsField.SetUp();
        }

        private void Initialize()
        {
            var gradientSchemes = GameManager.GradientSchemes;
            for (int i = 0; i < gradientSchemes.Length; i++)
            {
                CreateShopElement(gradientSchemes[i]);
            }
            var playerData = DataSaver.playerData;
            for (int i = 0; i < playerData.gradientSchemesBought.Count; i++)
            {
                var id = playerData.gradientSchemesBought[i];
                shopElements[id].SetPurchased();
            }
            SelectShopElement(playerData.currentGradientSchemeID);
            OnDisableShopUI();
        }

        private void CreateShopElement(GradientSchemeScriptableObject gradientScheme)
        {
            var shopElementGameObject = Instantiate(elementPrefab, Vector2.zero, Quaternion.identity, elementsTransform);
            var shopElementComponent = shopElementGameObject.GetComponent<ShopElement>();
            var sprite = GenerateGradientSprite(gradientScheme);
            var buttonAction = new UnityEngine.Events.UnityAction(() => SelectShopElement(gradientScheme.ID));
            shopElementComponent.SetImageAndPrice(sprite, gradientScheme, buttonAction);
            shopElements.Add(gradientScheme.ID, shopElementComponent);
        }

        private Sprite GenerateGradientSprite(GradientSchemeScriptableObject colorScheme, int width = 128, int height = 128)
        {
            var gradient = new Gradient();
            var colorKeys = new GradientColorKey[2];
            colorKeys[0] = new GradientColorKey(colorScheme.SkyboxColor2, 1f);
            colorKeys[1] = new GradientColorKey(colorScheme.SkyboxColor1, 0f);
            gradient.colorKeys = colorKeys;
            
            var gradientTex = new Texture2D(width, height, TextureFormat.ARGB32, false);
            gradientTex.filterMode = FilterMode.Bilinear;
            
            var inv = 1f / (width - 1);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var t = x * inv;
                    var color = gradient.Evaluate(t);
                    gradientTex.SetPixel(y, x, color);
                }
            }
            gradientTex.Apply();
            var sprite = Sprite.Create(gradientTex, new Rect(0f, 0f, width, height), new Vector2(0.5f, 0.5f), 100f);
            return sprite;
        }

        private void OnDisableShopUI()
        {
            gameObject.SetActive(false);
            var playerData = DataSaver.playerData;
            if (!playerData.gradientSchemesBought.Contains(selectedElementID))
            {
                SelectShopElement(playerData.currentGradientSchemeID);
            }
        }

        private void OnEnableShopUI()
        {
            gameObject.SetActive(true);
            UpdateCrystalsField();
        }

        private void UpdateCrystalsField()
        {
            var playerData = DataSaver.playerData;
            crystalsField.SetText(playerData.crystals.ToString());
            crystalsField.SetPreferredWidth();
        }
        
        private void SelectShopElement(int schemeID)
        {
            if (selectedElementID > 0)
                shopElements[selectedElementID].Deselect();
            shopElements[schemeID].Select();
            selectedElementID = schemeID;
            var playerData = Data.DataSaver.playerData;
            if (playerData.gradientSchemesBought.Contains(schemeID))
            {
                playerData.currentGradientSchemeID = schemeID;
                buyButton.SetActive(false);
            }
            else
            {
                buyButton.SetActive(true);
            }
            GameManager.SetColorScehem(shopElements[schemeID].gradientScheme);
        }

        public void OnBuyButton()
        {
            var playerData = DataSaver.playerData;
            if (playerData.crystals >= shopElements[selectedElementID].gradientScheme.Price)
            {
                playerData.crystals -= shopElements[selectedElementID].gradientScheme.Price;
                shopElements[selectedElementID].SetPurchased();
                buyButton.SetActive(false);
                playerData.gradientSchemesBought.Add(selectedElementID);
                playerData.currentGradientSchemeID = selectedElementID;
                EventsManager.CallEvent(Events.SavePlayerData);
                UpdateCrystalsField();
            }
        }

        public void OnBackToMenuButton()
        {
            EventsManager.CallEvent(Events.CloseShop);
            EventsManager.CallEvent(Events.SavePlayerData);
        }
    }
}