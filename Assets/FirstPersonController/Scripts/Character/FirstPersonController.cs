using UnityEngine;
using UnityEngine.InputSystem;

namespace FirstPersonController.Scripts.Character
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class FirstPersonController : MonoBehaviour
    {
        [SerializeField] private float forwardMoveSpeed = 8.0f, strafeMoveSpeed = 5.0f, backwardMoveSpeed = 4.0f;
        [SerializeField] private float jumpHeight = 2.0f;
        [SerializeField] private float groundCheckRadius = 0.1f;
        [SerializeField] private Transform m_CameraTransform = null;

        private Vector2 m_InputDirection;
        private bool m_JumpInput = false;
        private bool m_Grounded = true;
        private bool m_GroundedPreviously = true;
        private Vector3 m_GroundContactNormal = Vector3.up;
        private Transform m_Transform;
        private Rigidbody m_Rigidbody;

        protected void Awake()
        {
            m_Transform = GetComponent<Transform>();
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            //TODO: Physics check on the surrounding conditions
            GroundCheck();
            ProcessRotation(m_CameraTransform.rotation);
            ProcessMove(m_InputDirection);
            if (m_JumpInput)
            {
                ProcessJump(m_InputDirection);
            }
        }

        private void ProcessRotation(Quaternion rotation)
        {
            m_Rigidbody.MoveRotation(Quaternion.Euler(Vector3.up * rotation.eulerAngles.y));
        }

        private void ProcessMove(Vector2 inputDirection)
        {
            var targetDirection = CalculateMoveDirection(inputDirection, Vector3.up);
            var targetAcceleration = CalculateAcceleration(targetDirection);
            m_Rigidbody.AddForce(targetDirection * targetAcceleration, ForceMode.Acceleration);
        }

        private Vector3 CalculateMoveDirection(Vector2 input, Vector3 groundNormal)
        {
            var direction = input.TransformDimension();
            direction = m_CameraTransform.TransformDirection(direction);
            var projection = Vector3.ProjectOnPlane(direction, groundNormal);
            return projection;
        }

        private float CalculateAcceleration(Vector3 moveDirection)
        {
            var targetSpeed = GetTargetSpeedBasedOnDirection(moveDirection);
            var currentSpeed = m_Rigidbody.velocity.HorizontalPlane().magnitude;
            var acceleration = (targetSpeed - currentSpeed) / Time.fixedDeltaTime;
            return acceleration;
        }

        private float GetTargetSpeedBasedOnDirection(Vector3 direction)
        {
            //TODO: set target speed based on direction
            return 5f;
        }

        private void ProcessJump(Vector2 direction)
        {
            if (m_Grounded)
            {
                var targetDirection = Vector3.up + direction.TransformDimension();
                var force = Mathf.Sqrt(-2 * Physics.gravity.y * jumpHeight);
                m_Rigidbody.AddRelativeForce(targetDirection.normalized * force, ForceMode.VelocityChange);
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            m_InputDirection = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            m_JumpInput = context.performed;
        }

        private void GroundCheck()
        {
            //TODO: fix groundcheck
            RaycastHit hitInfo;
            m_Grounded = SphereCastToGround(out hitInfo);
            if (m_Grounded)
            {
                m_GroundContactNormal = hitInfo.normal;
                if (!m_GroundedPreviously)
                {
                    m_Rigidbody.drag = 1f;
                    m_GroundedPreviously = true;
                }
            }
            else
            {
                m_GroundContactNormal = Vector3.up;
                if (m_GroundedPreviously)
                {
                    m_Rigidbody.drag = 0f;
                    m_GroundedPreviously = false;
                }
            }
        }

        private bool SphereCastToGround(out RaycastHit hitInfo)
        {
            var result = Physics.SphereCast(m_Transform.position, groundCheckRadius, Vector3.down, out hitInfo);
            return result;
        }
    }
}