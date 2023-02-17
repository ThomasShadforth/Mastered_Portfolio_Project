using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "Scriptable Objects/Items/Consumable")]
public class ConsumableItem : BaseItemSO
{
    public int valChange;
    public bool heals;
    public bool buffs;

    //Post deadline stretch goal - add buffs & buff types

    public override void UseItem(PlayerController player)
    {
        if (heals)
        {
            Heal(player);
        }
    }

    public void Heal(PlayerController player)
    {
        
        player._healthSystem.Heal(valChange);
    }


}
