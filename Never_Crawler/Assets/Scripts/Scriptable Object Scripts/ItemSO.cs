using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Items/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public int itemCost;
    public float itemWeight;

    public virtual void UseItem()
    {

    }
}
