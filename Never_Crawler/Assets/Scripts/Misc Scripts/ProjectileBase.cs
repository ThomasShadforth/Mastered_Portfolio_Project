using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    int modifier;
    public int diceNum;
    public int maxDamageVal;
    
    public LayerMask groundLayer;
    Attack _attack;
    HitUI damagePrefab;
    // Start is called before the first frame update
    void Start()
    {
        _attack = new Attack();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void OnTriggerEnter(Collider other)
    {
        AIThinker AI = other.GetComponent<AIThinker>();
        PlayerController player = other.GetComponent<PlayerController>();

        if (AI != null)
        {
            HitUI hitText = Instantiate(damagePrefab);
            if (_attack.AttackRoll(modifier, AI.stats.armourClass))
            {
                int damage = _attack.DamageRoll(diceNum, maxDamageVal, modifier);
                Debug.Log(damage);
                AI.healthSystem.Damage(damage);
                hitText.SetUITextAndPos($"Hit! \n {damage} damage!", other.transform.position);
            }
            else
            {
                hitText.SetUITextAndPos("Missed!", other.transform.position);
            }
            Destroy(this);
        }
        else if (player != null)
        { 
            //Damage the player
        }
        else if (other.gameObject.layer == groundLayer)
        {
            Destroy(this);
        }
    }

    public void SetModifierAndRollValues(int modifierVal, int diceNum, int maxDamageVal, HitUI damageUI)
    {
        damagePrefab = damageUI;
        modifier = modifierVal;
        this.diceNum = diceNum;
        this.maxDamageVal = maxDamageVal;
    }

    
}
