using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Base Class", menuName = "Scriptable Objects/Classes/Base Class")]
public class BaseClassSO : ScriptableObject
{
    public string className;
    public string classDescription;

    public AbilitySO[] knownMoves;
    //public PlayerLevel classLevel = new PlayerLevel(5000, 20);

    public void TestAttack(GameObject parent)
    {
        Debug.Log(parent.name + " used an attack!");
    }

    public void UseAttack(string attackName)
    {

    }
}
