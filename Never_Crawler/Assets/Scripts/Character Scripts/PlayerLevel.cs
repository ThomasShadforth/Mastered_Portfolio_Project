using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerLevel
{
    [SerializeField] int[] expToNextLevel;
    [SerializeField] int currentLevel;
    [SerializeField] int currentEXP;
    [SerializeField] float expMult = 1.08f;


    public PlayerLevel(int baseEXP, int maxLevel)
    {
        expToNextLevel = new int[maxLevel];

        expToNextLevel[1] = baseEXP;

        for(int i = 2; i < expToNextLevel.Length; i++)
        {
            expToNextLevel[i] = Mathf.FloorToInt(expToNextLevel[i - 1] * expMult);
        }
    }
}
