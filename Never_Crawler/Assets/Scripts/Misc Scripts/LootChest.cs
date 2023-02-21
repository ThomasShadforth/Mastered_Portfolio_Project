using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LootChest : MonoBehaviour
{
    public ConsumableItem[] referenceItems;

    bool canOpen;

    bool _opened;

    public int minItems = 1;
    public int maxItems = 5;

    int numberOfItems;

    List<ConsumableItem> _storedItems = new List<ConsumableItem>();

    public GameObject interactUI;

    PlayerActionMap _playerInput;

    // Start is called before the first frame update
    void Start()
    {
        InitializeLootChestSize();
        RandomizeChestContents();

        _playerInput = new PlayerActionMap();
        _playerInput.Player.Interact.Enable();
        _playerInput.Player.Interact.started += OpenChest;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeLootChestSize()
    {
        numberOfItems = Random.Range(minItems, maxItems);
    }

    void RandomizeChestContents()
    {
        for(int i = 0; i < numberOfItems; i++)
        {
            ConsumableItem itemToAdd = referenceItems[Random.Range(0, referenceItems.Length)];
            _storedItems.Add(itemToAdd);
        }

        //Debug.Log(_storedItems.Count);

    }

    void GivePlayerItems()
    {
        _opened = true;
        canOpen = false;
        interactUI.SetActive(false);
        GetComponent<Animator>().SetBool("Open", true);

        ItemTextUI itemText = ItemTextObjectPool.instance.GetFromPool().GetComponent<ItemTextUI>();
        string textToAdd = "Items received:\n";

        for(int i = 0; i < _storedItems.Count; i++)
        {
            ItemManager.instance.AddItem(_storedItems[i]);
            textToAdd += _storedItems[i].itemName + " x 1 \n";
        }

        itemText.SetTextAndPos(textToAdd, transform.position);
    }

    public void OpenChest(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            
            if (canOpen && !_opened)
            {

                Debug.Log("OPENING");
                GivePlayerItems();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() && !_opened)
        {
            interactUI.SetActive(true);
            canOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() && !_opened)
        {
            interactUI.SetActive(false);
            canOpen = false;
        }
    }
}
