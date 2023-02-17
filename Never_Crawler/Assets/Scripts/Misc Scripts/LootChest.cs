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

    // Start is called before the first frame update
    void Start()
    {
        InitializeLootChestSize();
        RandomizeChestContents();
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

        Debug.Log(_storedItems.Count);

    }

    void GivePlayerItems()
    {
        _opened = true;
        GetComponent<Animator>().SetBool("Open", true);

        for(int i = 0; i < _storedItems.Count; i++)
        {
            ItemManager.instance.AddItem(_storedItems[i]);
        }
    }

    public void OpenChest(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (canOpen && !_opened)
            {
                GivePlayerItems();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            canOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            canOpen = false;
        }
    }
}
