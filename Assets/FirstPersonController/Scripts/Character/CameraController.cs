using UnityEngine;
using UnityEngine.InputSystem;

namespace FirstPersonController.Scripts
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float verticalCameraSpeed = 5.0f, horizontalCameraSpeed = 5.0f;
        [SerializeField] private float minPitch = -90f, maxPitch = 90f;
        [SerializeField] private Transform cameraTransform = null;
        
        private Vector2 m_LookVectorInput;
        private Quaternion m_CameraTargetRotation;

        private float m_Pitch = 0f, m_Jaw = 0f;

        private void Start()
        {
            m_CameraTargetRotation = cameraTransform.localRotation;
        }

        private void Update()
        {
            ProcessRotate(m_LookVectorInput);
        }

        private void ProcessRotate(Vector2 direction)
        {
            //TODO: rotate pov camera using quaternions for more robust code
            //RotateUsingQuaternion(direction); 
            RotateUsingEulerAngles(direction);
        }

        private void RotateUsingEulerAngles(Vector2 direction)
        {
            m_Jaw += direction.x * horizontalCameraSpeed * Time.deltaTime;
            m_Pitch -= direction.y * verticalCameraSpeed * Time.deltaTime;
            m_Pitch = Mathf.Clamp(m_Pitch, minPitch, maxPitch);
            cameraTransform.localRotation = Quaternion.Euler(m_Pitch, m_Jaw, 0f);
        }
        
        /*
        private void RotateUsingQuaternion(Vector2 direction)
        {
            m_Jaw = Vector3.up * (direction.x * horizontalCameraSpeed * Time.deltaTime);
            m_pitch = Vector3.left * (direction.y * verticalCameraSpeed * Time.deltaTime);
            m_CameraTargetRotation *= Quaternion.Euler(m_pitch + m_jaw);
            m_CameraTargetRotation = ClampRotationAroundXAxis(m_CameraTargetRotation);
            cameraTransform.localRotation = m_CameraTargetRotation;
        }

        private Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            var angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);
            angleX = Mathf.Clamp (angleX, minPitch, maxPitch);
            q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);
            
            return q;
        }
        */

        public void OnChangePitch(InputAction.CallbackContext context)
        {
            m_LookVectorInput = context.ReadValue<Vector2>();
        }
    }
}