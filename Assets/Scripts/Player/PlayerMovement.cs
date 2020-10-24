using enjoythevibes.Managers;
using enjoythevibes.Platforms;
using UnityEngine;

namespace enjoythevibes.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField]
        private AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f, 2f, 2f), new Keyframe(0.5f, 1f, 0f, 0f), new Keyframe(1f, 0f, -2f, -2f));
        [SerializeField]
        private LayerMask platformLayer = default;

        private float playTimer;
        private Vector3 gravityFallSpeed;

        private void Awake() 
        {
            EventsManager.AddListener(Events.PlayGame, OnResetTimer);
            EventsManager.AddListener(Events.RestartGame, OnResetTimer);
            EventsManager.AddListener(Events.GameOver, OnResetGravityFallSpeed);
        }

        private void OnResetGravityFallSpeed()
        {
            gravityFallSpeed = new Vector3(0f, -10f, 0f);
        }
        
        private void OnResetTimer()
        {
            playTimer = 0f;
        }

        public void Move(float xAxis)
        {
            playTimer += Time.deltaTime * GameManager.TimeScale;
            if (playTimer >= 1f)
            {
                if (CheckGround())
                {
                    playTimer -= 1f;
                    EventsManager.CallEvent(Events.GenerateNextPlatform);
                }
            }
            transform.position = new Vector3(transform.position.x, curve.Evaluate(playTimer), transform.position.z);
            transform.Translate(new Vector3(xAxis, 0f, EngineSettings.Platforms.SpawnEachZ * GameManager.TimeScale) * Time.deltaTime);
        }

        public void Fall()
        {
            gravityFallSpeed += new Vector3(0f, -100f, 0f) * Time.deltaTime * 3f;
            transform.Translate(gravityFallSpeed * Time.deltaTime);
        }

        private bool CheckGround()
        {
            var ray = new Ray(transform.position + new Vector3(0f, 0.26f, 0f), Vector3.down);
            var raycastHit = default(RaycastHit);
            var result = Physics.SphereCast(ray, 0.25f, out raycastHit, 0.261f, platformLayer);
            if (result)
            {
                var platformComponent = raycastHit.transform.GetComponent<Platform>();
                var distanceFromCenter = Vector3.ProjectOnPlane(raycastHit.point - (raycastHit.transform.position + platformComponent.CenterPosition), Vector3.up).sqrMagnitude;
                if (distanceFromCenter <= platformComponent.СenterRadiusDetection * platformComponent.СenterRadiusDetection)
                {
                    EventsManager.CallEvent(Events.AddExtraScore);
                    platformComponent.TakeCrystal();
                }
                else
                {
                    EventsManager.CallEvent(Events.AddScore);
                }
                var note = Mathf.RoundToInt(Functions.Lerp(-EngineSettings.Platforms.MaxMinXPosition, EngineSettings.Platforms.MaxMinXPosition, 0, 6, platformComponent.transform.position.x));
                var noteArgument = new AudioManager.NoteArgument(note);
                EventsManager.CallEvent(Events.PlayNote, noteArgument);
                return true;
            }
            else
            {
                EventsManager.CallEvent(Events.GameOver);
                return false;
            }
        }
    }
}