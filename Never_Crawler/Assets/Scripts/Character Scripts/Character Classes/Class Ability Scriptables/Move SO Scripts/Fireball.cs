using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireballSO", menuName = "Scriptable Objects/Class Abilities/Fireball")]
public class Fireball : AbilitySO
{
    public override void UseAbility(int modifier, PlayerController ownerPlayer = null, AIThinker thinker = null)
    {
        if (ownerPlayer != null)
        {
            
            ownerPlayer.PrepareCombatNotify(actionType, diceNum, maxDamage, this.modifier);
        } else if(thinker != null)
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
