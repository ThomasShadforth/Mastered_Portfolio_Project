using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[System.Serializable]
public class Stat
{

    [SerializeField]
    int _baseValue;
    [SerializeField]
    int _scoreModifier;

    //Call the constructor when creating a new stat variable
    public Stat(int rolledBaseValue)
    {
        this._baseValue = rolledBaseValue;
        SetScoreModifier();
    }

    

    public int GetBaseValue()
    {
        return _baseValue;
    }

    public void ChangeBaseValue(int valueIncrease)
    {
        _baseValue += valueIncrease;
        SetScoreModifier();
    }

    public void SetScoreModifier()
    {
        this._scoreModifier = (this._baseValue - 10) / 2;
    }

    public int GetScoreModifier()
    {
        return _scoreModifier;
    }
}
