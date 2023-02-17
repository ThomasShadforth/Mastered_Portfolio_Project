using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Items/Weapon")]
public class WeaponItem : BaseItemSO
{
    public override void UseItem(PlayerController player)
    {
        //Equip the item
        //Check if the player already has said item equipped
        //if(weapon != null) for example (In this case, it won't be by default)
    }
}
