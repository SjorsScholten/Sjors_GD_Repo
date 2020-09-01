using UnityEngine;

namespace FirstPersonController.Scripts.Equipment.Weapon.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Equipment/Weapon/Automatic", fileName = "New Weapon", order = 0)]
    public class AutomaticWeapon : Weapon
    {
        public override void FireWeapon(Ray ray)
        {
            
        }

        public override void CancelFire()
        {
            
        }
    }
}