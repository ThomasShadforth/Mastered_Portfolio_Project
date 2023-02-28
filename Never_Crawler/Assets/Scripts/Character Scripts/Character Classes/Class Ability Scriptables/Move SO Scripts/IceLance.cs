using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IceLanceSO", menuName = "Scriptable Objects/Class Abilities/Ice Lance")]
public class IceLance : AbilitySO
{ 
    public override void UseAbility(int modifier, PlayerController ownerPlayer = null, AIThinker thinker = null)
    {
        if (ownerPlayer != null)
        {
            //SpawnProjectile(ownerPlayer.testProjectile, modifier, ownerPlayer);
            ownerPlayer.PrepareCombatNotify(actionType, diceNum, maxDamage, this.modifier);
        }
        else if (thinker != null)
        {
            //Insert AI projectile method here
            //SpawnProjectile(thinker.testProjectile, modifier, null, thinker);
            thinker.PrepareCombatNotify(actionType, diceNum, maxDamage, this.modifier);
        }

        PlayAnim(ownerPlayer, thinker);
    }

    public override void PlayAnim(PlayerController ownerPlayer = null, AIThinker thinker = null)
    {
        base.PlayAnim(ownerPlayer, thinker);
    }
}
