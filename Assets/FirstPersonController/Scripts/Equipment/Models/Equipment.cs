using System;
using System.Collections.Generic;

namespace FirstPersonController.Scripts.Equipment.Models
{
    [Serializable]
    public class Equipment
    {
        public List<Weapon.ScriptableObjects.Weapon> weapons = new List<Weapon.ScriptableObjects.Weapon>();
        public List<CharacterModifier> characterModifiers = new List<CharacterModifier>();
        
        public int GetCost()
        {
            var cost = 0;
            
            foreach (Item item in weapons)
            {
                cost += item.GetCost();
            }

            foreach (Item item in characterModifiers)
            {
                cost += item.GetCost();
            }

            return cost;
        }
    }
}