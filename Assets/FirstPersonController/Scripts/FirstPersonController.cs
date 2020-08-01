using System;
using FirstPersonController.Scripts.MovementStates;
using FirstPersonController.Utility;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FirstPersonController.Scripts {
    [RequireComponent(typeof(CapsuleCollider))]
    public class FirstPersonController : Entity, Sjors_GD_Repo.IPlayerActions {
        [Serializable]
        public class MovementSetting
        {
            [SerializeField] private float forwardMoveSpeed = 8.0f, strafeMoveSpeed = 5.0f, backwardMoveSpeed = 4.0f;
            [HideInInspector] public float CurrentMoveSpeed = 0.0f;

            public void UpdateCurrentMoveSpeed(Vector2 direction)
            {
                if (direction.x < 0 || direction.x > 0) CurrentMoveSpeed = strafeMoveSpeed;
                if (direction.y < 0) CurrentMoveSpeed = backwardMoveSpeed;
                if (direction.y > 0) CurrentMoveSpeed = forwardMoveSpeed;
            }
        }

        [Serializable]
        public class CameraSettings
        {
            public float horizontalCameraSpeed = 5.0f, verticalCameraSpeed = 4.0f;
            
            private Quaternion ClampRotationAroundXAxis(Quaternion q, float min, float max)
            {
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

        [Serializable]
        public class MovementBehaviour
        {
            private Quaternion m_TargetRotation;
            private Entity m_Entity;
            
            private readonly FiniteStateMachine<BaseMovementState> m_StateMachine = new FiniteStateMachine<BaseMovementState>();

            public MovementBehaviour(Entity entity)
            {
                m_Entity = entity;
                m_TargetRotation = entity.transform.rotation;
            }

            public void Move(Vector2 direction, float moveSpeed, float deltaTime)
            {
                var targetDirection = m_Entity.transform.TransformDirection(direction.TransformDimension());
                targetDirection = Vector3.ProjectOnPlane(targetDirection, Vector3.up).normalized;
                targetDirection *= moveSpeed;

                //TODO: change to constant force instead of instant force
                if (m_Entity.rigidbody.velocity.sqrMagnitude < moveSpeed * moveSpeed)
                {
                    m_Entity.rigidbody.AddForce(targetDirection, ForceMode.VelocityChange);
                }
            }

            public void Rotate(float axis, float sensitivity)
            {
                var oldRotation = m_Entity.transform.eulerAngles.y;
                m_TargetRotation *= Quaternion.Euler(Vector3.up * (axis * sensitivity));
                m_Entity.transform.localRotation = m_TargetRotation;
                var velRotation = Quaternion.AngleAxis(m_Entity.transform.eulerAngles.y - oldRotation, Vector3.up);
                m_Entity.rigidbody.velocity = velRotation * m_Entity.rigidbody.velocity;
            }

            public void Jump(float jumpForce)
            {
                //TODO: implement jumping
                //TODO: implement Thrusting
            }
        }

        [Serializable]
        public class CameraBehaviour
        {
            private Quaternion _cameraTargetRotation;
            private FirstPersonController m_FPSController;

            public CameraBehaviour(FirstPersonController firstPersonController)
            {
                m_FPSController = firstPersonController;
            }

            public void Rotate(float axis, float sensitivity)
            {
                _cameraTargetRotation *= Quaternion.Euler(Vector3.right * (axis * sensitivity));
                _cameraTargetRotation = ClampRotationAroundXAxis(_cameraTargetRotation, minPitch, maxPitch);
                m_FPSController.cameraTransform.localRotation = _cameraTargetRotation;
            }
        }

        [Serializable]
        public class CombatBehaviour
        {
            
        }

        [SerializeField] private float mouseSensitivity = 1;
        [SerializeField] private float minPitch = -90f, maxPitch = 90f;
        
        public MovementSetting movementSettings = new MovementSetting();
        public CameraSettings cameraSettings = new CameraSettings();
        public Transform cameraTransform;
        
        private MovementBehaviour m_MovementBehaviour;
        private CameraBehaviour m_CameraBehaviour;
        private CombatBehaviour m_CombatBehaviour;
        
        private Vector2 _inputDirection;
        private Vector2 _lookRotation;
        
        private Camera _camera;

        protected override void Awake() {
            base.Awake();
            
            m_MovementBehaviour = new MovementBehaviour(this);
            m_CameraBehaviour = new CameraBehaviour(this);
            m_CombatBehaviour = new CombatBehaviour();
            
            _camera = Camera.main;
            cameraTransform = _camera.GetComponent<Transform>();
        }

        private void Update() 
        {
            m_MovementBehaviour.Rotate(_lookRotation.x, cameraSettings.horizontalCameraSpeed);
            m_CameraBehaviour.Rotate(-_lookRotation.y, cameraSettings.verticalCameraSpeed);
        }

        private void FixedUpdate() 
        {
            movementSettings.UpdateCurrentMoveSpeed(_inputDirection);
            m_MovementBehaviour.Move(_inputDirection, movementSettings.CurrentMoveSpeed, Time.fixedDeltaTime);
        }

        public void OnMove(InputAction.CallbackContext context) { _inputDirection = context.ReadValue<Vector2>(); }

        public void OnLook(InputAction.CallbackContext context) { _lookRotation = context.ReadValue<Vector2>(); }

        public void OnFire(InputAction.CallbackContext context) { }
        
        
    }

    [RequireComponent(typeof(Rigidbody))]
    public abstract class Entity : MonoBehaviour
    {
        public new Transform transform;
        public new Rigidbody rigidbody;

        protected virtual void Awake()
        {
            transform = GetComponent<Transform>();
            rigidbody = GetComponent<Rigidbody>();
        }
    }

    public static class VectorExtentionMethods 
    {
        public static Vector3 TransformDimension(this Vector2 a) => new Vector3(a.x, 0, a.y);
    }
}
