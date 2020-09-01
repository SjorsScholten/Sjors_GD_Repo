using UnityEngine;

namespace FirstPersonController.Scripts.Equipment.Weapon.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Equipment/Weapon/SemiAutomatic", fileName = "New Weapon", order = 0)]
    public class SemiAutomaticWeapon : Weapon
    {
        public override void FireWeapon(Ray ray)
        {
            var hitInfo = ShootRay(ray);
        }

        public override void CancelFire()
        {
            
        }
    }
}