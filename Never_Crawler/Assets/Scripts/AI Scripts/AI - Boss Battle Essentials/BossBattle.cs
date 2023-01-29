using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattle : MonoBehaviour
{
    public enum Boss_Phases
    {
        WaitingToStart,
        Phase_1,
        Phase_2,
        Phase_3
    }

    [SerializeField] ColliderTrigger trigger;
    [SerializeField] AIThinker bossAI;

    [SerializeField] State phase1State;
    [SerializeField] State phase2State;
    [SerializeField] State phase3State;

    Boss_Phases phase;

    List<GameObject> enemiesPresent;

    //Get a reference to health System when it is implemented

    private void Awake()
    {
        phase = Boss_Phases.WaitingToStart;
    }

    // Start is called before the first frame update
    void Start()
    {
        trigger.OnPlayerEnterTrigger += ColliderTrigger_OnPlayerEnterTrigger;
        bossAI.healthSystem.OnHealthChanged += Boss_OnDamaged;
        //Get a reference to health System when it is implemented
    }

    void ColliderTrigger_OnPlayerEnterTrigger(object sender, System.EventArgs e)
    {
        StartBossBattle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartBossBattle()
    {
        Debug.Log("STARTING");
        StartNextPhase();
        
    }

    private void StartNextPhase()
    {
        switch (phase)
        {
            case Boss_Phases.WaitingToStart:
                phase = Boss_Phases.Phase_1;
                bossAI.SetCooldownTimer();
                bossAI.TransitionToState(phase1State);
                bossAI.playerTarget = FindObjectOfType<PlayerController>().transform;
                
                break;
            case Boss_Phases.Phase_1:
                phase = Boss_Phases.Phase_2;
                bossAI.TransitionToState(phase2State);
                break;
            case Boss_Phases.Phase_2:
                bossAI.TransitionToState(phase3State);
                phase = Boss_Phases.Phase_3;
                break;
        }
    }

    void Boss_OnDamaged(object sender, System.EventArgs e)
    {
        //For each time the boss is damaged, check to see if it passes a certain threshold.

        switch (phase)
        {
            default:
                break;
            case Boss_Phases.Phase_1:
                if(bossAI.healthSystem.GetHealthPercent() < .7f)
                {
                    Debug.Log("ENTERING PHASE 2");
                }
                break;
            case Boss_Phases.Phase_2:
                if(bossAI.healthSystem.GetHealthPercent() < .4f)
                {

                }
                break;
        }
    }

    public void SetBossAI(AIThinker bossAI)
    {
        this.bossAI = bossAI;
    }

    public void SetBossTrigger(ColliderTrigger trigger)
    {
        this.trigger = trigger;
    }

    public void SetBossPhases(State phase_1, State phase_2, State phase_3)
    {
        phase1State = phase_1;
        phase2State = phase_2;
        phase3State = phase_3;
    }

    void DestroyAllEnemies()
    {
        foreach(GameObject enemy in enemiesPresent)
        {
            //Destroy enemy if they are still alive
        }
    }
}
