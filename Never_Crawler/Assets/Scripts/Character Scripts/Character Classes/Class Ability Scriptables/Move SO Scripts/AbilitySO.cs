using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilitySO : ScriptableObject
{
    public string abilityName;
    public string description;

    public int requiredLevel;
    public int effectVal;

    public int diceNum;
    public int maxDamageVal;

    public bool isAttack;
    public bool healsAlly;
    public bool givesBuff;

    public HitUI hitTextPrefab;

    public AttackType type;

    public abstract void UseAbility(int modifier, PlayerController ownerPlayer = null, AIThinker thinker = null);



}
