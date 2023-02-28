using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss Attack Action", menuName = "Scriptable Objects/Pluggable AI/Actions/Boss Attack Action")]
public class BossAttackAction : Action
{
    public override void Act(AIThinker thinker)
    {
        AttackCooldown(thinker);
    }

    void AttackCooldown(AIThinker thinker)
    {
        if (!thinker.attacking)
        {
            Debug.Log(thinker.attackCoolTimer);
            if (thinker.attackCoolTimer > 0)
            {
                thinker.attackCoolTimer -= GamePause.deltaTime;
            }
            else
            {
                Attack(thinker);
                
            }
        }
        else
        {
            AnimatorStateInfo stateInfo = thinker.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
            float nTime = stateInfo.normalizedTime;

            if(nTime >= 1.0f)
            {
                Debug.Log("NOT ATTACKING");
                thinker.attacking = false;
                thinker.attackCoolTimer = thinker.attackCoolTime;
            }
        }
    }

    void Attack(AIThinker thinker)
    {
        AbilitySO[] attacks = thinker.attacks;

        thinker.attacking = true;

        if(attacks.Length > 1)
        {
            int attackNum = Random.Range(0, attacks.Length);

            attacks[attackNum].UseAbility(0, null, thinker);
        }
        else
        {
            //Debug.Log("ATTACKING");
            attacks[0].UseAbility(0, null, thinker);
        }
    }
}
