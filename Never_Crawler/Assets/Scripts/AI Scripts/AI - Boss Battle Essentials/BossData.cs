using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossData : MonoBehaviour
{
    public State phase1Sstate;
    public State phase2Sstate;
    public State phase3Sstate;

    public GameObject bossEnemy;
    public GameObject bossBattle;
    public GameObject bossTrigger;

    public AbilitySO[] phase1Attacks;
    public AbilitySO[] phase2Attacks;
    public AbilitySO[] phase3Attacks;
    
}
