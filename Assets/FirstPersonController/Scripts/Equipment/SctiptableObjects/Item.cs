using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    [Header("Item Properties")]
    [SerializeField] private string itemName = "";
    [SerializeField] private Sprite sprite = null;
    [SerializeField] private string description = "";
    [Space]
    [SerializeField] protected int cost = 1;

    public abstract int GetCost();
    
    public string Name => itemName;
    public Sprite Sprite => sprite;
    public string Description => description;
}
