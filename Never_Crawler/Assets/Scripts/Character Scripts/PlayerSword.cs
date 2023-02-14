using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : SwordBase
{
    // Start is called before the first frame update
    void Start()
    {
        combatAnimator = GetComponentInParent<CombatAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<AIThinker>())
        {
            combatAnimator.TriggerAttack(null, other.GetComponent<AIThinker>());
        }
    }
}
