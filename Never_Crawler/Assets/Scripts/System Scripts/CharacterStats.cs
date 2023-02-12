using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat strength, dexterity, constitution, charisma, intelligence, wisdom;

    public int armourClass { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        InitialiseBaseStats();

        //Have a method that performs other calculations where necessary (e.g. for the player)
        CalculateAdditionalValues();

        CalculateAC();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void CalculateAdditionalValues()
    {

    }

    public virtual void InitialiseBaseStats()
    {

    }

    public void CalculateAC()
    {
        armourClass = 10 + dexterity.GetScoreModifier();
    }
}
