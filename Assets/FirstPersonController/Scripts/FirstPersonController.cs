using UnityEngine;
using UnityEngine.InputSystem;

namespace FirstPersonController.Scripts {
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class FirstPersonController : MonoBehaviour, Sjors_GD_Repo.IPlayerActions {
        [SerializeField] private float walkSpeed = 5;
        
        [SerializeField] private float mouseSensitivity = 1;
        [SerializeField] private float minPitch = -90f, maxPitch = 90f;
        
        private Vector2 _inputDirection;
        private Vector2 _lookRotation;

        private Quaternion _characterTargetRotation;
        private Quaternion _cameraTargetRotation;

        private Transform _myTransform;
        private Rigidbody _myRigidbody;
        private Camera _camera;
        private Transform _cameraTransform;

        private void Awake() {
            
            _myTransform = GetComponent<Transform>();
            _myRigidbody = GetComponent<Rigidbody>();
            
            _camera = Camera.main;
            _cameraTransform = _camera.GetComponent<Transform>();

            _characterTargetRotation = _myTransform.localRotation;
            _cameraTargetRotation = _cameraTransform.localRotation;
        }

        private void Update() {
            
            Rotate(_lookRotation);
        }

        private void FixedUpdate() {
            
            Move(_inputDirection, Time.fixedDeltaTime);
        }

        private void Move(Vector2 direction, float deltaTime) {
            
            var targetDirection = _cameraTransform.TransformDirection(direction.TransformDimension());
            targetDirection = Vector3.ProjectOnPlane(targetDirection, Vector3.up).normalized;
            targetDirection *= walkSpeed;
            
            if(_myRigidbody.velocity.sqrMagnitude < Mathf.Pow(walkSpeed, 2)) 
                _myRigidbody.AddForce(targetDirection, ForceMode.VelocityChange);
        }

        private void Rotate(Vector2 direction) {
            
            ChangeJaw(direction.x);
            ChangePitch(-direction.y);
        }

        private void ChangeJaw(float value) {
            
            var oldRotation = transform.eulerAngles.y;
            _characterTargetRotation *= Quaternion.Euler(Vector3.up * value * mouseSensitivity);
            _myTransform.localRotation = _characterTargetRotation;
            var velRotation = Quaternion.AngleAxis(_myTransform.eulerAngles.y - oldRotation, Vector3.up);
            _myRigidbody.velocity = velRotation * _myRigidbody.velocity;
            
        }

        private void ChangePitch(float value) {
            
            _cameraTargetRotation *= Quaternion.Euler(Vector3.right * value * mouseSensitivity);
            _cameraTargetRotation = ClampRotationAroundXAxis(_cameraTargetRotation, minPitch, maxPitch);
            _cameraTransform.localRotation = _cameraTargetRotation;
        }

        public void OnMove(InputAction.CallbackContext context) { _inputDirection = context.ReadValue<Vector2>(); }

        public void OnLook(InputAction.CallbackContext context) { _lookRotation = context.ReadValue<Vector2>(); }

        public void OnFire(InputAction.CallbackContext context) { }
        
        private Quaternion ClampRotationAroundXAxis(Quaternion q, float min, float max){
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);
            angleX = Mathf.Clamp (angleX, min, max);
            q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);
            
            return q;
        }
    }

    public static class VectorExtentionMethods {
        public static Vector3 TransformDimension(this Vector2 a) => new Vector3(a.x, 0, a.y);
    }
}
