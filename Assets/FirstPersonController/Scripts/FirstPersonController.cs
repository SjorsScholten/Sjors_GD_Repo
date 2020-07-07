using UnityEngine;
using UnityEngine.InputSystem;

namespace FirstPersonController.Scripts {
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class FirstPersonController : MonoBehaviour, Sjors_GD_Repo.IPlayerActions {
        [SerializeField] private float walkSpeed = 5;

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
            _myRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            
            _camera = Camera.main;
            _cameraTransform = _camera.GetComponent<Transform>();

            _characterTargetRotation = _myTransform.localRotation;
            _cameraTargetRotation = _cameraTransform.localRotation;
        }

        private void Update() {
            Rotate();
        }

        private void FixedUpdate() {
            Move(_inputDirection, Time.fixedDeltaTime);
        }

        private void Move(Vector2 direction, float deltaTime) {
            var targetDirection = _cameraTransform.TransformDirection(direction.TransformDimension() * walkSpeed);
            targetDirection = Vector3.ProjectOnPlane(targetDirection, Vector3.up);
            if(_myRigidbody.velocity.sqrMagnitude < (walkSpeed*walkSpeed)) _myRigidbody.AddForce(targetDirection, ForceMode.VelocityChange);
        }

        private void Rotate() {
            var oldRotation = transform.eulerAngles.y;
            
            _characterTargetRotation *= Quaternion.Euler(Vector3.up * _lookRotation.x);
            _cameraTargetRotation *= Quaternion.Euler(Vector3.right * _lookRotation.y);

            _myTransform.localRotation = _characterTargetRotation;
            _cameraTransform.localRotation = _cameraTargetRotation;
            
            var velRotation = Quaternion.AngleAxis(_myTransform.eulerAngles.y - oldRotation, Vector3.up);
            _myRigidbody.velocity = velRotation * _myRigidbody.velocity;
        }

        public void OnMove(InputAction.CallbackContext context) { _inputDirection = context.ReadValue<Vector2>(); }

        public void OnLook(InputAction.CallbackContext context) { _lookRotation = context.ReadValue<Vector2>(); }

        public void OnFire(InputAction.CallbackContext context) { }
    }

    public static class VectorExtentionMethods {
        public static Vector3 TransformDimension(this Vector2 a) => new Vector3(a.x, 0, a.y);
    }
}
