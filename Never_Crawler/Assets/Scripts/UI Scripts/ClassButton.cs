using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassButton : MonoBehaviour
{
    public Image buttonImage;
    public int buttonValue;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Press()
    {
        ClassMenu menu = PauseMenu.instance.GetComponent<ClassMenu>();

        if (menu != null)
        {
            Debug.Log("GETTING INFO");
            menu.DisplayAbilityInfo(menu.GetPlayerClass().knownMoves[buttonValue]);
        }
    }
}
