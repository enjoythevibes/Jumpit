using enjoythevibes.Managers;
using UnityEngine;

namespace enjoythevibes.UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField]
        private Player.PlayerStats playerStats = default;

        [SerializeField]
        private UIField scoreField = default;
        [SerializeField]
        private UIField crystalsField = default;

        private void Awake()
        {
            EventsManager.AddListener(Events.PlayGame, OnEnableGameUI);
            EventsManager.AddListener(Events.RestartGame, OnEnableGameUI);
            EventsManager.AddListener(Events.GameOver, OnDisableGameUI);   
            EventsManager.AddListener(Events.AddScore, OnUpdateScore);
            EventsManager.AddListener(Events.AddExtraScore, OnUpdateScore);
            EventsManager.AddListener(Events.AddCrystal, OnUpdateCrystals);
            scoreField.SetUp();
            crystalsField.SetUp();
            OnDisableGameUI();     
        }

        private void OnEnableGameUI()
        {
            OnUpdateScore();
            OnUpdateCrystals();
            gameObject.SetActive(true);
        }

        private void OnDisableGameUI()
        {
            gameObject.SetActive(false);
        }        

        private void OnUpdateScore()
        {
            scoreField.SetText(playerStats.Scores.ToString());
        }
        
        private void OnUpdateCrystals()
        {
            crystalsField.SetText(playerStats.Crystals.ToString());
        }
    }
}
