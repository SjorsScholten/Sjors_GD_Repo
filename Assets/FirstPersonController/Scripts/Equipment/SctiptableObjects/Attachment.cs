using UnityEngine;

namespace FirstPersonController.Scripts.Equipment
{
    [CreateAssetMenu(menuName = "Equipment/Attachment", fileName = "New Attachment", order = 1)]
    public class Attachment : Item
    {
        public override int GetCost()
        {
            return cost;
        }
    }
}