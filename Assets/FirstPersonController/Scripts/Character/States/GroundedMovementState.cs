using System;
using UnityEngine;

namespace FirstPersonController.Scripts.MovementStates
{
    public class GroundedMovementState : BaseMovementState
    {
        public GroundedMovementState(Entity entity) : base(entity) { }

        public override void Jump(Vector2 direction, float jumpDistance)
        {
            var targetDirection = Vector3.up + direction.TransformDimension();
            var force = Mathf.Sqrt(-2 * Physics.gravity.y * jumpDistance);
            m_Entity.rigidbody.AddForce(targetDirection.normalized * force, ForceMode.VelocityChange);
        }
    }
}