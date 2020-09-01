using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FirstPersonController.Scripts.Equipment.UIElements
{
    public class ItemSlot
    {
        [SerializeField] private Image imageSlot = null;
        [SerializeField] private TextMeshProUGUI textSlot = null;
        
        private Item m_Item;

        private void UpdateInterface(Item itemToDisplay)
        {
            imageSlot.sprite = itemToDisplay.Sprite;
            textSlot.text = itemToDisplay.Name;
        }

        public Item Item
        {
            get => m_Item;
            set
            {
                m_Item = value;
                UpdateInterface(m_Item);
            }
        }
    }
}