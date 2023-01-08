using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public int buttonValue;
    public int valueChange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Called when the button is pressed. What it will do is dependent on which of the menus is active
    public void Press()
    {
        if(CharacterCreatorMenu.instance != null)
        {
            //Debug.Log("MENU HERE!");
            if (CharacterCreatorMenu.instance.GetPointBuyMenuStatus().activeInHierarchy)
            {
                CharacterCreatorMenu.instance.ChangeAbilityValue(buttonValue, valueChange);
            } else if (CharacterCreatorMenu.instance.GetRandomMenuStatus().activeInHierarchy)
            {
                CharacterCreatorMenu.instance.RandomizeValue(buttonValue);
            } else if (CharacterCreatorMenu.instance.GetClassMenuStatus().activeInHierarchy)
            {
                CharacterCreatorMenu.instance.SelectClass(buttonValue);
            }
        }
    }
}
