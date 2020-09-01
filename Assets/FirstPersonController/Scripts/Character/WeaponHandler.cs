using System;
using System.Collections;
using System.Collections.Generic;
using FirstPersonController.Scripts.Equipment;
using FirstPersonController.Scripts.Equipment.Weapon.ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private Transform cam_Transform;
    [SerializeField] private Weapon m_CurrentWeapon = null;
    [SerializeField] private Transform m_WeaponHolder = null;

    private void Shoot()
    {
        if (Physics.Raycast(cam_Transform.position, cam_Transform.forward, out var hitInfo))
        {
            AddForceToRigidbody(hitInfo);
        }
    }

    private void AddForceToRigidbody(RaycastHit hitInfo)
    {
        if (hitInfo.rigidbody)
        {
            var force = cam_Transform.forward * m_CurrentWeapon.Damage;
            hitInfo.rigidbody.AddForceAtPosition(force, hitInfo.point, ForceMode.Impulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackStart();
        }

        if (context.performed)
        {
            AttackCancel();
        }
    }

    public void OnNextWeapon(InputAction.CallbackContext context)
    {
        
    }
    
    public void OnPreviousWeapon(InputAction.CallbackContext context)
    {
        
    }

    private void AttackStart()
    {
        var ray = new Ray(cam_Transform.position, cam_Transform.forward);
        m_CurrentWeapon.FireWeapon(ray);
    }

    private void AttackCancel()
    {
        Debug.Log("attack ended");
        Debug.DrawRay(cam_Transform.position, cam_Transform.forward * 10f, Color.red, 2f);
    }
}