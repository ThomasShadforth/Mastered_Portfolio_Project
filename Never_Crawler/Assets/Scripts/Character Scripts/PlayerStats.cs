using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerStats : CharacterStats, IObserver
{
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


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!CharacterData.firstLoadDone)
        {
            CharacterData.firstLoadDone = true;
            
            return;
        }
        else
        {
            
        }
    }

    public override void CalculateAdditionalValues()
    {
        maxHealth = baseHealth + constitution.GetScoreModifier();

        InitializeExpValues();

        _previousStatPoints = availableStatPoints;
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

    public int GetCurrentEXPRequirement()
    {
        return expToNextLevel[currentLevel];
    }

    public void GainExp(int ExpToGain)
    {
        ExpUI expUI = ExpTextObjectPool.instance.GetFromPool().GetComponent<ExpUI>();
        expUI.SetTextandPosition($"Gained {ExpToGain} EXP!", transform.position);

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

    public override void InitialiseBaseStats()
    {
        strength = new Stat(CreatorDataHandler.statValues[0] > 0 ? CreatorDataHandler.statValues[0] : 10);
        dexterity = new Stat(CreatorDataHandler.statValues[1] > 0 ? CreatorDataHandler.statValues[1] : 13);
        constitution = new Stat(CreatorDataHandler.statValues[2]);
        intelligence = new Stat(CreatorDataHandler.statValues[3] > 0 ? CreatorDataHandler.statValues[3] : 15);
        wisdom = new Stat(CreatorDataHandler.statValues[4]);
        charisma = new Stat(CreatorDataHandler.statValues[5]);

        carryWeight = strength.GetBaseValue() * 15;
    }

    public void ReloadStatValues()
    {
        strength = new Stat(CharacterData.statValue[0]);
        dexterity = new Stat(CharacterData.statValue[1]);
        constitution = new Stat(CharacterData.statValue[2]);
        intelligence = new Stat(CharacterData.statValue[3]);
        wisdom = new Stat(CharacterData.statValue[4]);
        charisma = new Stat(CharacterData.statValue[5]);

        currentLevel = CharacterData.currentLevel;
        currentEXP = CharacterData.currentExp;
        GetComponent<PlayerAbilities>().SetPlayerClass(CharacterData.playerClass);

        carryWeight = strength.GetBaseValue() * 15;
    }

    public void OnNotify(TutorialEnum tutorialEvent)
    {

    }

    public void OnNotify(CombatActionEnum actionType, CombatActionEnum minExp = CombatActionEnum.enemy_Died, CombatActionEnum maxExp = CombatActionEnum.enemy_Died, CombatActionEnum mod = CombatActionEnum.enemy_Died)
    {
        if(actionType == CombatActionEnum.enemy_Died)
        {
            //Increase exp
            string[] splitMinExp = minExp.ToString().Split('_');
            string[] splitMaxExp = maxExp.ToString().Split('_');

            int minExpBound = int.Parse(splitMinExp[1]);
            int maxExpBound = int.Parse(splitMaxExp[1]);

            int expGained = Random.Range(minExpBound, maxExpBound + 1);

            GainExp(expGained);

        }
    }
}
