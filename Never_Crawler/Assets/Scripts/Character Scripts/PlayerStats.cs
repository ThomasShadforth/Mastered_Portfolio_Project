using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStats : MonoBehaviour
{
    public Stat strength, dexterity, constitution, charisma, intelligence, wisdom;

    public int armourClass;

    public int baseHealth;
    public int maxHealth;

    [Header("Level System Values")]
    public int currentLevel = 1;
    public int currentEXP;
    public int maxLevel = 20;
    public int baseEXP = 3000;
    public float expMultiplier;
    public int[] expToNextLevel;
    public int[] statPointLevels;

    public float carryWeight;



    public int availableStatPoints = 0;
    int _previousStatPoints;

    PlayerActionMap _input;

    public PlayerStats()
    {
        //Call the constructor when initialising the player's character and adding this script
    }

    void Start()
    {
        InitialiseBaseStats();

        maxHealth = baseHealth + constitution.GetScoreModifier();
        InitializeExpValues();

        _input = new PlayerActionMap();
        _input.Player.Enable();
        _input.Player.TestAddExp.performed += AddExp;

        _previousStatPoints = availableStatPoints;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeExpValues()
    {
        expToNextLevel = new int[maxLevel];
        expToNextLevel[currentLevel] = baseEXP;

        for(int i = 2; i < expToNextLevel.Length; i++)
        {
            expToNextLevel[i] = Mathf.FloorToInt(expToNextLevel[i - 1] * expMultiplier);
        }
    }

    void AddExp(InputAction.CallbackContext context)
    {
        GainExp(500);
    }

    public void GainExp(int ExpToGain)
    {
        currentEXP += ExpToGain;
        if (currentLevel < maxLevel)
        {
            if (currentEXP >= expToNextLevel[currentLevel])
            {
                currentEXP -= expToNextLevel[currentLevel];

                currentLevel++;

                Debug.Log("LEVEL UP");

                CheckForStatPoints();

            }
        }

        if(currentLevel >= maxLevel)
        {
            currentEXP = 0;
        }
    }

    void CheckForStatPoints()
    {
        for(int i = 0; i < statPointLevels.Length; i++)
        {
            if(currentLevel % statPointLevels[i] == 0)
            {
                if (currentLevel == statPointLevels[i])
                {
                    //Award Stat Points (These can be allocated manually)
                    Debug.Log("AWARDING STAT POINTS");
                }
            }
        }
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

        //Debug.Log(statRolls.Count);

        for(int i = 0; i < statRolls.Count; i++)
        {
            //Debug.Log(rolledStat);
            //Debug.Log(statRolls[i]);

            rolledStat += statRolls[i];
        }

        

        return rolledStat;
    }

    public int GetPreviousStatPoints()
    {
        return _previousStatPoints;
    }

    public void SetPreviousStatPoints()
    {
        _previousStatPoints = availableStatPoints;
    }

    public void AddStatPoints(int numOfPoints)
    {
        availableStatPoints += numOfPoints;
        _previousStatPoints = availableStatPoints;
    }

    public void CalculateAC()
    {
        armourClass = 10 + dexterity.GetScoreModifier();
    }

    void InitialiseBaseStats()
    {
        /*
        strength = new Stat(RollBaseStat());
        dexterity = new Stat(RollBaseStat());
        constitution = new Stat(RollBaseStat());
        charisma = new Stat(RollBaseStat());
        intelligence = new Stat(RollBaseStat());
        wisdom = new Stat(RollBaseStat());*/

        strength = new Stat(CreatorDataHandler.statValues[0] > 0 ? CreatorDataHandler.statValues[0] : 10);
        dexterity = new Stat(CreatorDataHandler.statValues[1]);
        constitution = new Stat(CreatorDataHandler.statValues[2]);
        intelligence = new Stat(CreatorDataHandler.statValues[3]);
        wisdom = new Stat(CreatorDataHandler.statValues[4]);
        charisma = new Stat(CreatorDataHandler.statValues[5]);

        carryWeight = strength.GetBaseValue() * 15;
    }
}
