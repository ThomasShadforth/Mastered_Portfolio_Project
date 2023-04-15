using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack Action", menuName = "Scriptable Objects/Pluggable AI/Actions/Attack Action")]
public class AttackAction : Action
{
    //public AbilitySO[] attacks;

    public override void Act(AIThinker thinker)
    {
        if (!thinker.attacking)
        {
            Attack(thinker);
        }
    }

    void Attack(AIThinker thinker)
    {
        AbilitySO[] attacks = thinker.attacks;
        thinker.attacking = true;

        if(attacks.Length > 1)
        {
            //Randomise attacks if more than one exists
        }
        else
        {
            attacks[0].UseAbility(null, thinker);
        }
    }
}
