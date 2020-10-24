using enjoythevibes.Managers;
using UnityEngine;

namespace enjoythevibes.Player
{
    public class PlayerInput : MonoBehaviour
    {
        public float xAxis { private set; get; }
        private float sensitivity;

        #if (UNITY_ANDROID && !UNITY_EDITOR)
        private Vector2 lastTouchPosition;
        private float xTouchVelocity;
        private float smoothTime = 0.01f;
        #endif

        private void Awake() 
        {
            EventsManager.AddListener(Events.PlayGame, OnSetSensitivity);    
        }

        private void OnSetSensitivity()
        {
            sensitivity = Data.DataSaver.playerData.sensitivity;
        }

        private void Update()
        {
            #if (UNITY_STANDALONE || UNITY_EDITOR)
            xAxis = Input.GetAxis("Mouse X") * sensitivity;               
            #endif
            
            #if (UNITY_ANDROID && !UNITY_EDITOR)
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                var currentTouchPosition = Input.GetTouch(0).position;
                lastTouchPosition = currentTouchPosition;
            }
            else
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                var currentTouchPosition = Input.GetTouch(0).position;
                var deltaPosition = (currentTouchPosition - lastTouchPosition);
                xAxis = Mathf.SmoothDamp(xAxis, deltaPosition.x * 0.075f * sensitivity, ref xTouchVelocity, smoothTime);
                lastTouchPosition = currentTouchPosition;
            }
            else
            if (Input.touchCount == 0)
            {
                xAxis = 0f;
            }
            #endif
        }
    }
}