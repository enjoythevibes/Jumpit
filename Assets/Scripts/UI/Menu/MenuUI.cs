using enjoythevibes.Managers;
using UnityEngine;

namespace enjoythevibes.UI
{
    public class MenuUI : MonoBehaviour
    {
        [SerializeField]
        private Player.PlayerStats playerStats = default;

        [SerializeField]
        private UIField recordScoreField = default;
        [SerializeField]
        private UIField crystalsField = default;

        private void Awake() 
        {
            EventsManager.AddListener(Events.GameInitialization, OnUpdateMenuUIFields);
            EventsManager.AddListener(Events.PlayGame, OnDisableMenuUI);
            EventsManager.AddListener(Events.BackToMenu, OnEnableMenuUI);
            EventsManager.AddListener(Events.OpenSettings, OnDisableMenuUI);
            EventsManager.AddListener(Events.CloseSettings, OnEnableMenuUI);
            EventsManager.AddListener(Events.OpenShop, OnDisableMenuUI);
            EventsManager.AddListener(Events.CloseShop, OnEnableMenuUI);
            recordScoreField.SetUp();
            crystalsField.SetUp();
        }
        
        private void OnEnableMenuUI()
        {
            gameObject.SetActive(true);
            OnUpdateMenuUIFields();
        }

        private void OnDisableMenuUI()
        {
            gameObject.SetActive(false);
        }

        private void OnUpdateMenuUIFields()
        {
            recordScoreField.SetText(playerStats.RecordScores.ToString());
            crystalsField.SetText(playerStats.Crystals.ToString());
        }

        public void OnPlayGameButton()
        {
            EventsManager.CallEvent(Events.PlayGame);
        }

        public void OnOpenSettingsButton()
        {
            EventsManager.CallEvent(Events.OpenSettings);
        }

        public void OnOpenShopButton()
        {
            EventsManager.CallEvent(Events.OpenShop);
        }
    }
}