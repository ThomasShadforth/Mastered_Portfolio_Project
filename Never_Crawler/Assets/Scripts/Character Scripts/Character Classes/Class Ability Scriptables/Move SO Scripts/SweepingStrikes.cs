using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SweepingStrikesSO", menuName = "Scriptable Objects/Class Abilities/Sweeping Strikes")]
public class SweepingStrikes : AbilitySO
{
    public override void UseAbility(int modifier, PlayerController ownerPlayer = null, AIThinker thinker = null)
    {
        if(ownerPlayer != null)
        {
            ownerPlayer.PrepareCombatNotify(actionType, diceNum, maxDamage, this.modifier);
        }
        else
        {

        }

        PlayAnim(ownerPlayer, thinker);
    }

    public override void PlayAnim(PlayerController ownerPlayer = null, AIThinker thinker = null)
    {
        base.PlayAnim(ownerPlayer, thinker);
    }
}
