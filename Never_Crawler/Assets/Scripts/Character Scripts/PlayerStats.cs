using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public Stat strength, dexterity, constitution, charisma, intelligence, wisdom;

    public int baseHealth;
    public int maxHealth;

    public PlayerStats()
    {
        //Call the constructor when initialising the player's character and adding this script
    }

    void Start()
    {
        InitialiseBaseStats();
        maxHealth = baseHealth + constitution.GetScoreModifier();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int RollBaseStat()
    {
        int rolledStat = 0;
        List<int> statRolls = new List<int>();

        for (int i = 0; i < 4; i++)
        {
            int roll = Random.Range(1, 7);
            statRolls.Add(roll);
        }

        statRolls.Sort();
        statRolls.Reverse();

        statRolls.RemoveAt(statRolls.Count - 1);

        Debug.Log(statRolls.Count);

        for(int i = 0; i < statRolls.Count; i++)
        {
            //Debug.Log(rolledStat);
            //Debug.Log(statRolls[i]);

            rolledStat += statRolls[i];
        }

        

        return rolledStat;
    }

    void InitialiseBaseStats()
    {
        strength = new Stat(RollBaseStat());
        dexterity = new Stat(RollBaseStat());
        constitution = new Stat(RollBaseStat());
        charisma = new Stat(RollBaseStat());
        intelligence = new Stat(RollBaseStat());
        wisdom = new Stat(RollBaseStat());
    }
}
