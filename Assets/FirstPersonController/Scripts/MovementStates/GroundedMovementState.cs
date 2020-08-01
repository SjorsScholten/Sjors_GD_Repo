using UnityEngine;

namespace FirstPersonController.Scripts.MovementStates
{
    public class GroundedMovementState : BaseMovementState
    {
        public override void Move(Vector2 direction, float speed, float deltaTime)
        {
            throw new System.NotImplementedException();
        }

        public override void Rotate(float axis, float sensitivity)
        {
            throw new System.NotImplementedException();
        }

        public override void Jump(float jumpHeight)
        {
            throw new System.NotImplementedException();
        }
    }
}