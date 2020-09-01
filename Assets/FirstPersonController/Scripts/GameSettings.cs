using System;
using UnityEngine;

namespace UnityStandardAssets.Characters.FirstPerson.FirstPersonController.Scripts
{
    public class GameSettings : MonoBehaviour
    {
        [SerializeField] private bool lockCursor = true;
        
        private void Awake()
        {
            UpdateCursor(lockCursor);
        }

        private void UpdateCursor(bool value)
        {
            if (value)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}