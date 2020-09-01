using UnityEngine;

namespace FirstPersonController.Scripts.Equipment
{
    [CreateAssetMenu(menuName = "Equipment/CharacterModifier", fileName = "New CharacterModifier", order = 0)]
    public class CharacterModifier : Item
    {
        public override int GetCost()
        {
            return cost;
        }
    }
}