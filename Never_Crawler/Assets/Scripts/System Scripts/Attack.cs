using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{
    

    public bool AttackRoll(int modifier, int enemyAC)
    {
        int roll = Random.Range(0, 21) + modifier;
        return roll >= enemyAC;
    }

    public bool AttackAdvDisadv(int modifier, int enemyAC, bool advantage)
    {
        int decidedRoll = 0;
        int firstRoll = Random.Range(0, 21) + modifier;
        int secondRoll = Random.Range(0, 21) + modifier;

        if (advantage)
        {
            decidedRoll = Mathf.Max(firstRoll, secondRoll);
        }
        else
        {
            decidedRoll = Mathf.Min(firstRoll, secondRoll);
        }


        return decidedRoll >= enemyAC;
    }

    public int DamageRoll(int numberOfDice, int maxVal, int damageBonus)
    {
        int damageVal = 0;
        
        for(int i = 0; i < numberOfDice; i++)
        {
            int rolledVal = Random.Range(1, maxVal + 1);
            damageVal += rolledVal;
        }

        damageVal += damageBonus;

        return damageVal;
    }
}
