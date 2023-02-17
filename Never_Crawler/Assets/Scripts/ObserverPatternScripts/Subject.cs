using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subject : MonoBehaviour
{
    List<IObserver> _observers = new List<IObserver>();

    
    protected void NotifyObservers(CombatActionEnum combatAction)
    {
        if(_observers.Count != 0)
        {
            _observers.ForEach((observer) =>
            {
                observer.OnNotify(combatAction);
            });
        }
    }

    protected void NotifyObservers(CombatActionEnum actionType, CombatActionEnum diceNum, CombatActionEnum maxDamage, CombatActionEnum modifier)
    {
        if(_observers.Count != 0)
        {
            _observers.ForEach((observer) =>
            {
                observer.OnNotify(actionType, diceNum, maxDamage, modifier);
            });
        }
    }

    protected void NotifyObservers(TutorialEnum tutorialEvent)
    {
        if(_observers.Count != 0)
        {
            _observers.ForEach((observer) =>
            {
                observer.OnNotify(tutorialEvent);
            });
        }
    }

    public void AddObserver(IObserver observer)
    {
        if(observer == null)
        {
            Debug.LogError("NOT FOUND");
        }
        _observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        if(observer == null)
        {
            Debug.Log("OBSERVER DOESNT EXIST");
        }
        _observers.Remove(observer);
    }

    public int GetObserverCount()
    {
        return _observers.Count;
    }


}
