using UnityEngine;

namespace FirstPersonController.Scripts.MovementStates
{
    public abstract class BaseMovementState
    {
        public abstract void Move(Vector2 direction, float speed, float deltaTime);
        public abstract void Rotate(float axis, float sensitivity);
        public abstract void Jump(float jumpHeight);
    }
}