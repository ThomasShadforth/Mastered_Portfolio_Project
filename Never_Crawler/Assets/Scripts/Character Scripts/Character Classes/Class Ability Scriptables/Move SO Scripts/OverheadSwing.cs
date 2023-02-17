using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OverheadSwingSO", menuName = "Scriptable Objects/Class Abilities/Overhead Swing")]
public class OverheadSwing : AbilitySO
{
    public override void UseAbility(int modifier, PlayerController ownerPlayer = null, AIThinker thinker = null)
    {
        if (ownerPlayer != null)
        {
            ownerPlayer.PrepareCombatNotify(actionType, diceNum, maxDamage, this.modifier);
        }
        else
        {
            thinker.PrepareCombatNotify(actionType, diceNum, maxDamage, this.modifier);
        }

        PlayAnim(ownerPlayer, thinker);
    }

    public override void PlayAnim(PlayerController ownerPlayer = null, AIThinker thinker = null)
    {
        base.PlayAnim(ownerPlayer, thinker);
    }
}
