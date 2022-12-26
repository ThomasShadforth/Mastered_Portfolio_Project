using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionSlot : MonoBehaviour
{
    //This class is used to hold assigned actions (These are assigned via the class interface when/where applicable). This is specifically for the sake of the UI (As moves will be called from the player controller)

    public ClassMoveSO assignedAction;
    public Image buttonImage;
    public int buttonValue;
    
    public void Press()
    {
        ClassMenu menu = PauseMenu.instance.GetComponent<ClassMenu>();

        menu.AssignActionToSlot(buttonValue);
    }
}
