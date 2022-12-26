using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{

    public GameObject menuWindow;
    public GameObject[] windows;

    public ItemButton[] itemButtons;

    BaseItemSO _activeItem;
    int _activeIndex;

    public static PauseMenu instance;

    PlayerActionMap _input;

    [Header("Inventory Menu Elements")]
    [SerializeField] TextMeshProUGUI _itemNameText;
    [SerializeField] TextMeshProUGUI _itemDescriptionText;
    [SerializeField] TextMeshProUGUI _weightText;
    [SerializeField] TextMeshProUGUI _useText;

    // Start is called before the first frame update
    void Start()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        _input = new PlayerActionMap();
        _input.Player.Enable();
        _input.Player.Pause.performed += PauseButton;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PauseButton(InputAction.CallbackContext context)
    {
        OpenMenu();
    }

    public void OpenMenu()
    {
        if (menuWindow.activeInHierarchy)
        {
            //Close the pause menu
            menuWindow.SetActive(false);

            for(int i = 0; i < windows.Length; i++)
            {
                windows[i].SetActive(false);
            }

            //Create set of action bindings for the menu
            FindObjectOfType<CinemachineFreeLook>().GetComponent<CinemachineInputProvider>().XYAxis.action.Enable();

            FindObjectOfType<PlayerController>()._playerInput.Player.Enable();

        }
        else
        {
            //Create bindings for menu

            FindObjectOfType<CinemachineFreeLook>().GetComponent<CinemachineInputProvider>().XYAxis.action.Disable();

            FindObjectOfType<PlayerController>()._playerInput.Player.Disable();

            menuWindow.SetActive(true);
            OpenWindow(0);
        }
    }

    public void OpenWindow(int windowIndex)
    {
        for(int i = 0; i < windows.Length; i++)
        {
            if(i != windowIndex)
            {
                windows[i].SetActive(false);
            }
            else
            {
                windows[i].SetActive(true);
            }
        }
    }

    public void OpenInventory()
    {
        ItemManager.instance.SortItems();

        for(int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].ButtonValue = i;

            if(ItemManager.instance.items[i] != null)
            {
                //Set the image to an item icon when these have been made
                itemButtons[i].amountText.text = ItemManager.instance.itemCount[i].ToString();
            }
            else
            {
                itemButtons[i].ButtonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
            }
        }
    }

    public void DisplayItemDetails(BaseItemSO itemToDisplay, int index)
    {
        if(itemToDisplay == null)
        {
            return;
        }
        //Set the active item here
        _activeItem = itemToDisplay;
        _activeIndex = index;
        _itemNameText.text = _activeItem.itemName;
        _itemDescriptionText.text = "";
        _weightText.text = _activeItem.itemWeight + " KGs";

        if(_activeItem.isArmour || _activeItem.isWeapon)
        {
            _useText.text = "Equip";
        }
        else
        {
            _useText.text = "Use";
        }

    }

    public void UseItem()
    {
        if (_activeItem != null)
        {
            //Use item, pass the active item as a parameter
            _activeItem.UseItem();
            ItemManager.instance.QueryToRemove(_activeItem.itemName);
        }

        ResetActiveItem();
    }

    public void DiscardItem()
    {
        if (_activeItem != null)
        {
            ItemManager.instance.QueryToRemove(_activeItem.itemName);
        }

        ResetActiveItem();
    }

    void ResetActiveItem()
    {
        if (ItemManager.instance.itemCount[_activeIndex] == 0)
        {
            _activeItem = null;
            _activeIndex = 0;
            //Write code for setting the button, get the item details
            _itemNameText.text = "";
            _itemDescriptionText.text = "";
            _weightText.text = "";

            OpenInventory();

            ItemManager.instance.testItemWeight = ItemManager.instance.GetInventoryWeight();

        }
        else
        {
            OpenInventory();

            ItemManager.instance.testItemWeight = ItemManager.instance.GetInventoryWeight();
        }
    }
}
