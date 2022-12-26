using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    strength,
    dexterity,
    intelligence,
    wisdom,
    charisma,
    constitution
}

[CreateAssetMenu(fileName = "Class Move", menuName = "Scriptable Objects/Classes/Class Moves/Class Move")]
public class ClassMoveSO : ScriptableObject
{
    public string moveName;
    public string description;
    public int requiredLevel;
    public int effectValue;

    public bool isAttack;
    public bool healsSelf;
    public bool healsAlly;
    public bool givesBuff;


    public AttackType type;

    public void TriggerAbility(GameObject parent, int moveModifier)
    {
        if (isAttack)
        {
            UseAttack(moveModifier, parent);
        } else if (healsSelf)
        {
            
        } else if (healsAlly)
        {

        } else if (givesBuff)
        {

        }
    }

    //Takes in the ability modifier depending on the attack's type
    public void UseAttack(int attackModifier, GameObject parent)
    {
        int naturalRoll = Random.Range(1, 21);
        int totalRoll = naturalRoll + attackModifier;

        //Get AC, then check to see if the attack succeeds
        int AC = 15;

        if(totalRoll >= AC)
        {
            Debug.Log(parent.name + " succeeded using " + moveName);
        }
        else
        {
            Debug.Log(parent.name + " failed using " + moveName);
        }
    }

    public void UseHeal()
    {
        //Write logic for heal
    }

}
