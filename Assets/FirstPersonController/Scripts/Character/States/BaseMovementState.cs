using System;
using UnityEngine;

namespace FirstPersonController.Scripts.MovementStates
{
    public abstract class BaseMovementState
    {
        protected Entity m_Entity;

        public BaseMovementState(Entity entity)
        {
            m_Entity = entity;
        }

        public virtual void Move(Vector2 direction, float speed, float deltaTime)
        {
            //TODO: Fix movement bug
            
            if ((Mathf.Abs(direction.x) > float.Epsilon || Mathf.Abs(direction.y) > float.Epsilon) &&
                Mathf.Abs(speed) > float.Epsilon)
            {
                var targetDirection = m_Entity.transform.TransformDirection(direction.TransformDimension());
                targetDirection = Vector3.ProjectOnPlane(targetDirection, Vector3.up).normalized;

                float targetSpeed = 0;
                try
                {
                    targetSpeed = ((speed - m_Entity.rigidbody.velocity.HorizontalPlane().magnitude) / deltaTime) *
                                  m_Entity.rigidbody.mass;
                }
                catch (DivideByZeroException dbze)
                {
                    Debug.LogError(dbze.Message);
                }

                //if (m_Entity.rigidbody.velocity.sqrMagnitude < speed * speed)
                m_Entity.rigidbody.AddForce(targetDirection * targetSpeed, ForceMode.Force);
            }
        }

        public virtual void Rotate(float axis, float sensitivity, ref Quaternion targetRotation)
        {
            var oldRotation = m_Entity.transform.eulerAngles.y;
            targetRotation *= Quaternion.Euler(Vector3.up * (axis * sensitivity));
            m_Entity.transform.localRotation = targetRotation;

            //TODO: find another way to rotate the players velocity
            //var velRotation = Quaternion.AngleAxis(m_Entity.transform.eulerAngles.y - oldRotation, Vector3.up);
            //m_Entity.rigidbody.velocity = velRotation * m_Entity.rigidbody.velocity;
        }

        public abstract void Jump(Vector2 direction, float jumpDistance);
    }
}