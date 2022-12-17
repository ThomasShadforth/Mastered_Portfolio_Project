using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemButton : MonoBehaviour
{
    public Image ButtonImage;
    public TextMeshProUGUI amountText;



    public int ButtonValue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Press()
    {
        //Trigger Method based on the open menu (If applicable)
        PauseMenu.instance.DisplayItemDetails(ItemManager.instance.GetItemDetails(ButtonValue));
    }
}
