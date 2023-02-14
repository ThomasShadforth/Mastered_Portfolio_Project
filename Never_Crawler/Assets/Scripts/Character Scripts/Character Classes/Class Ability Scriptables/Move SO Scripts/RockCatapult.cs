using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RockCatapultSO", menuName = "Scriptable Objects/Class Abilities/Rock Catapult")]
public class RockCatapult : AbilitySO
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
            //thinker.PrepareCombatNotify(actionType, diceNum, maxDamage);
        }

        PlayAnim(ownerPlayer, thinker);
    }

    public override void PlayAnim(PlayerController ownerPlayer = null, AIThinker thinker = null)
    {
        base.PlayAnim(ownerPlayer, thinker);
    }
}
