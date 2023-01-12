using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Test Attack Action", menuName = "Scriptable Objects/Pluggable AI/Actions/Test Attack Action")]
public class TestAttackAction : Action
{
    public override void Act(AIThinker thinker)
    {
        TestAttack(thinker);
    }

    public void TestAttack(AIThinker thinker)
    {
        //Execute attack, set some form of cool down to revert to the pursuit state

        Debug.Log("Starting Attack");
        Collider[] hitObjects = Physics.OverlapSphere(thinker.transform.position, thinker.minAttackDist + 1, thinker.playerLayer);

        if(hitObjects.Length != 0)
        {
            
            //Get the player controller/stats, check if they can be damaged
            PlayerController player = hitObjects[0].GetComponent<PlayerController>();

            if(player != null)
            {
                
                player._healthSystem.Damage(2);
                Debug.Log(player._healthSystem.GetHealth());
            }
        }

        thinker.SetCooldownTimer();
    }
}
