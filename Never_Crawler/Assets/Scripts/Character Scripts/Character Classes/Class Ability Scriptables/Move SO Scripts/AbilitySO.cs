using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Class Ability", menuName = "Scriptable Objects/Classes/Class Ability")]
public class AbilitySO : ScriptableObject
{
    public string abilityName;
    public string description;

    public bool defaultAbility;
    public int requiredLevel;

    public CombatActionEnum actionType;
    public CombatActionEnum diceNum;
    public CombatActionEnum maxDamage;
    public CombatActionEnum modifier;
    
    public string animName;

    public void UseAbility(PlayerController ownerPlayer = null, AIThinker thinker = null)
    {
        if(ownerPlayer != null)
        {
            ownerPlayer.PrepareCombatNotify(actionType, diceNum, maxDamage, modifier);
        } else if(thinker != null)
        {
            thinker.PrepareCombatNotify(actionType, diceNum, maxDamage, modifier);
        }

        PlayAnim(ownerPlayer, thinker);
    }

    public void PlayAnim(PlayerController ownerPlayer = null, AIThinker thinker = null)
    {
        if(ownerPlayer != null)
        {
            ownerPlayer.GetComponentInChildren<Animator>().Play(animName);
        } else if(thinker != null)
        {
            //Insert call to play enemyAnim here
            thinker.GetComponent<Animator>().Play(animName);
        }
    }

}
