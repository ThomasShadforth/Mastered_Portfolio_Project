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
    public string animName;


    public AttackType type;

    public abstract void UseAbility(int modifier, PlayerController ownerPlayer = null, AIThinker thinker = null);

    public virtual void PlayAnim(PlayerController ownerPlayer = null, AIThinker thinker = null)
    {
        if(ownerPlayer != null)
        {
            ownerPlayer.GetComponentInChildren<Animator>().Play(animName);
        } else if(thinker != null)
        {
            //Insert call to play enemyAnim here
        }
    }

}
