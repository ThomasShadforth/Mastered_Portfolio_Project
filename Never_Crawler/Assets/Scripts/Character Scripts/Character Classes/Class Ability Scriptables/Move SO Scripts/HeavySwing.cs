using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeavySwingSO", menuName = "Scriptable Objects/Class Abilities/Heavy Swing")]
public class HeavySwing : AbilitySO
{
    public override void UseAbility(int modifier, PlayerController ownerPlayer = null, AIThinker thinker = null)
    {
        if(ownerPlayer != null)
        {
            ownerPlayer.PrepareCombatNotify(actionType, diceNum, maxDamage, this.modifier);
        }

        PlayAnim(ownerPlayer, thinker);

    }

    public override void PlayAnim(PlayerController ownerPlayer = null, AIThinker thinker = null)
    {
        base.PlayAnim(ownerPlayer, thinker);
    }
}
