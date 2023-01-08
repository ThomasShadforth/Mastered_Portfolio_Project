using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStats : MonoBehaviour
{
    public int[] statBaseValues;

    public Stat strength, dexterity, constitution, charisma, intelligence, wisdom;

    public int armourClass;

    //Set level of the enemy. Will potentially be used for the purpose of EXP scaling based on player level
    public int level;
    public int expRewarded;

    // Start is called before the first frame update
    void Start()
    {
        InitialiseBaseStats();
        CalculateAC();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void InitialiseBaseStats()
    {
        strength = new Stat(statBaseValues[0]);
        dexterity = new Stat(statBaseValues[1]);
        constitution = new Stat(statBaseValues[2]);
        charisma = new Stat(statBaseValues[3]);
        intelligence = new Stat(statBaseValues[4]);
        wisdom = new Stat(statBaseValues[5]);
    }

    public void CalculateAC()
    {
        armourClass = 10 + dexterity.GetScoreModifier();
    }
}
