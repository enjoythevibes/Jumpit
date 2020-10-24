using enjoythevibes.Data;
using enjoythevibes.Managers;
using UnityEngine;

namespace enjoythevibes.Player
{
    public class PlayerStats : MonoBehaviour
    {
        private int scores;
        public int Scores
        {
            private set
            {
                scores = value;
                if (scores > RecordScores)
                {
                    RecordScores = scores;
                }
            }
            get => scores;
        }
        public int Crystals { private set; get; }
        public int RecordScores { private set; get; }

        private void Awake()
        {
            EventsManager.AddListener(Events.GameInitialization, OnSetPlayerData);
            EventsManager.AddListener(Events.PlayGame, OnResetScores);
            EventsManager.AddListener(Events.RestartGame, OnResetScores);
            EventsManager.AddListener(Events.GameOver, OnRecordPlayerData);
            EventsManager.AddListener(Events.AddScore, OnAddScore);
            EventsManager.AddListener(Events.AddExtraScore, OnAddExtraScores);
            EventsManager.AddListener(Events.AddCrystal, OnAddCrystal);
            EventsManager.AddListener(Events.SavePlayerData, OnSetPlayerData);
        }

        private void OnSetPlayerData()
        {
            RecordScores = DataSaver.playerData.recordScore;
            Crystals = DataSaver.playerData.crystals;
        }

        private void OnRecordPlayerData()
        {
            var playerData = DataSaver.playerData;
            playerData.recordScore = RecordScores;
            playerData.crystals = Crystals;
            EventsManager.CallEvent(Events.SavePlayerData);
        }

        private void OnAddCrystal()
        {
            Crystals++;
        }
        
        private void OnAddScore()
        {
            Scores++;
        }

        private void OnAddExtraScores()
        {
            Scores += 2;
        }

        private void OnResetScores()
        {
            Scores = 0;
        }
    }
}