using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Items/Item")]
public abstract class BaseItemSO : ScriptableObject
{
    public string itemName;
    public int itemCost;
    public float itemWeight;

    public bool isArmour;
    public bool isWeapon;
    public bool isItem;

    public abstract void UseItem(PlayerController player);
}
