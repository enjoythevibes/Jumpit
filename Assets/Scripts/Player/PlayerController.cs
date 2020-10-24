using enjoythevibes.Managers;
using UnityEngine;

namespace enjoythevibes.Player
{
    [RequireComponent(typeof(PlayerMovement), typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerMovement playerMovement;
        private PlayerInput playerInput;
        private PlayerState currentState;

        private void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
            playerInput = GetComponent<PlayerInput>();
            EventsManager.AddListener(Events.PlayGame, OnPlayGame);
            EventsManager.AddListener(Events.RestartGame, OnRestartGame);
            EventsManager.AddListener(Events.GameOver, OnGameOver);
            EventsManager.AddListener(Events.BackToMenu, OnBackToMenu);
        }

        private void Update() 
        {
            switch (currentState)
            {
                case PlayerState.Move:
                    MoveState();
                    break;
                case PlayerState.GameOver:
                    GameOverState();
                    break;
            }
        }

        private void MoveState()
        {
            playerMovement.Move(playerInput.xAxis);
            if (GameManager.TimeScale < EngineSettings.GameManager.MaxTimeScale)
                GameManager.TimeScale += Time.deltaTime * EngineSettings.GameManager.TimeScaleMultiplier;
            else
                GameManager.TimeScale = EngineSettings.GameManager.MaxTimeScale;
        }

        private void GameOverState()
        {
            if (transform.position.y > -1000f)
            {
                playerMovement.Fall();
            }
        }
 
        private void OnPlayGame()
        {
            currentState = PlayerState.Move;
            #if UNITY_STANDALONE
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            #endif
        }

        private void OnGameOver()
        {
            currentState = PlayerState.GameOver;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private void OnRestartGame()
        {
            GameManager.TimeScale = 1f;
            currentState = PlayerState.Move;
            transform.position = Vector3.zero;
            #if UNITY_STANDALONE
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            #endif
        }

        private void OnBackToMenu()
        {
            GameManager.TimeScale = 1f;
            currentState = PlayerState.Idle;
            transform.position = Vector3.zero;
        }
    }
}