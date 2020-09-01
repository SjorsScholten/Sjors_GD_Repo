using UnityEngine;

namespace FirstPersonController.Scripts.MovementStates
{
    public class AirMovementState : BaseMovementState
    {
        public AirMovementState(Entity entity) : base(entity)
        {
        }

        public override void Move(Vector2 direction, float speed, float deltaTime)
        {
            
        }

        public override void Jump(Vector2 direction, float jumpDistance)
        {
            //TODO: implement Thrusting
            var targetDirection = Vector3.up + direction.TransformDimension();
            var force = (jumpDistance - m_Entity.rigidbody.velocity.magnitude) / Time.fixedDeltaTime;
            m_Entity.rigidbody.AddForce(targetDirection.normalized * force, ForceMode.Acceleration);
        }
    }
}