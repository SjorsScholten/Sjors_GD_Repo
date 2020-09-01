using System.Collections.Generic;
using UnityEngine;

namespace FirstPersonController.Scripts.Equipment.Weapon.ScriptableObjects
{
    public abstract class Weapon : Item
    {
        [Header("Weapon Properties")]
        public float Damage = 10f;
        
        public GameObject WeaponPrefab;
        public List<Attachment> Attachments = new List<Attachment>();
        
        public override int GetCost()
        {
            return cost;
        }

        public abstract void FireWeapon(Ray ray);
        public abstract void CancelFire();

        protected RaycastHit ShootRay(Ray ray)
        {
            Debug.DrawRay(ray.origin, ray.direction * 10f, Color.green, 2f);
            Physics.Raycast(ray, out var hitInfo);
            return hitInfo;
        }
    }
}