using UnityEngine;

namespace enjoythevibes.Camera
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField]
        private Transform playerTransform = default;
        [SerializeField]
        private float zoom = 7f;

        private void LateUpdate()
        {
            var playerPosition = new Vector3(0f, 0.25f, playerTransform.position.z);
            var position = playerPosition - transform.forward * zoom;
            transform.position = position;
        }
    }
}