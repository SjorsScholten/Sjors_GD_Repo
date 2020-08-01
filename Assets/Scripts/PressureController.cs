using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressureController : MonoBehaviour
{
    public float threshold;
    public UnityEvent onThresholdMet = new UnityEvent();
    
    private float c_Mass;
    private readonly HashSet<Entity> m_EntitySet = new HashSet<Entity>();
    public float CurrentMass => c_Mass;

    private void OnCollisionEnter(Collision other)
    {
        var entity = other.gameObject.GetComponent<Entity>();
        if (entity)
        {
            AddEntity(entity);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        var entity = other.gameObject.GetComponent<Entity>();
        if (entity)
        {
            RemoveEntity(entity);
        }
    }

    private void AddEntity(Entity entity)
    {
        if (m_EntitySet.Add(entity))
        {
            UpdateMass();
        }
    }

    private void RemoveEntity(Entity entity)
    {
        if (m_EntitySet.Remove(entity))
        {
            UpdateMass();
        }
    }

    private void UpdateMass()
    {
        c_Mass = 0;
        foreach (var entity in m_EntitySet)
        {
            c_Mass += entity.m_rigidbody.mass;
        }

        if (c_Mass >= threshold)
        {
            onThresholdMet.Invoke();
        }
    }
}
