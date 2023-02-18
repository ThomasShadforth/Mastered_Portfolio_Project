using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObject : Subject
{
    public bool isStart;
    public bool triggered;

    public bool triggerOnEnter;

    public TutorialEnum tutorialEvent;

    public void Start()
    {
        if (isStart)
        {
            triggered = true;
            NotifyObservers(tutorialEvent);
        }
    }

    private void OnDisable()
    {
        //Debug.Log("OBJECT DISABLED/TRIGGERED");
        if (Application.isPlaying)
        {
            NotifyObservers(tutorialEvent);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            if (!triggered && triggerOnEnter)
            {
                triggered = true;
                
                NotifyObservers(tutorialEvent);
            }
        }
    }
}
