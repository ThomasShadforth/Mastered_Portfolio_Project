using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[System.Serializable]
public class Stat
{
    [SerializeField]
    int _originalBaseValue;
    [SerializeField]
    int _baseValue;
    [SerializeField]
    int _scoreModifier;
    [SerializeField]
    int _previousRecordedValue;

    //Call the constructor when creating a new stat variable
    //Run the set score modifier
    public Stat(int rolledBaseValue)
    {
        this._baseValue = rolledBaseValue;
        _originalBaseValue = _baseValue;
        SetPreviousRecordedValue();
        SetScoreModifier();
    }

    public void ResetToPrevValue()
    {
        _baseValue = _previousRecordedValue;
    }

    public void ResetToOriginalBase()
    {
        _baseValue = _originalBaseValue;
    }
    
    public void SetPreviousRecordedValue()
    {
        _previousRecordedValue = _baseValue;
    }

    public int GetPreviousValue()
    {
        return _previousRecordedValue;
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
        //Subtract 10 from the base stat value, divide the result by 2 and round down.
        this._scoreModifier = (this._baseValue - 10) / 2;
    }

    public int GetScoreModifier()
    {
        return _scoreModifier;
    }
}
