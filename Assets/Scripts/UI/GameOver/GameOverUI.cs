using enjoythevibes.Managers;
using UnityEngine;

namespace enjoythevibes.UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField]
        private Player.PlayerStats playerStats = default;

        [SerializeField]
        private UIField recordScoreField = default;
        [SerializeField]
        private UIField crystalsField = default;
        [SerializeField]
        private UIField lastScoreAttemptField = default;

        private void Awake() 
        {
            EventsManager.AddListener(Events.RestartGame, OnDisableGameOverUI);
            EventsManager.AddListener(Events.BackToMenu, OnDisableGameOverUI);
            EventsManager.AddListener(Events.GameOver, OnEnableGameOverUI);
            recordScoreField.SetUp();
            crystalsField.SetUp();
            lastScoreAttemptField.SetUp();
            OnDisableGameOverUI();
        }

        public void OnRestartGameButton()
        {
            EventsManager.CallEvent(Events.RestartGame);
        }

        public void OnBackToMenuButton()
        {
            EventsManager.CallEvent(Events.BackToMenu);
        }

        private void OnEnableGameOverUI()
        {
            gameObject.SetActive(true);
            UpdateGameOverInfo();
        }

        private void OnDisableGameOverUI()
        {
            gameObject.SetActive(false);
        }

        private void UpdateGameOverInfo()
        {
            recordScoreField.SetText(playerStats.RecordScores.ToString());
            recordScoreField.SetPreferredWidth();
            crystalsField.SetText(playerStats.Crystals.ToString());
            crystalsField.SetPreferredWidth();
            lastScoreAttemptField.SetText(playerStats.Scores.ToString());
        }
    }
}