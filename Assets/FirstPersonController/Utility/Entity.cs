using UnityEngine;

namespace FirstPersonController.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Entity : MonoBehaviour
    {
        [HideInInspector] public new Transform transform;
        [HideInInspector] public new Rigidbody rigidbody;

        protected virtual void Awake()
        {
            transform = GetComponent<Transform>();
            rigidbody = GetComponent<Rigidbody>();
        }
    }
}