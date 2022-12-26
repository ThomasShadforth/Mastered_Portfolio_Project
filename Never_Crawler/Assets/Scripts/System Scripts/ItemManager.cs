using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemManager : MonoBehaviour
{
    public BaseItemSO[] items;
    public int[] itemCount;
    public BaseItemSO[] referenceItems;

    public int maxItemCount;

    public static ItemManager instance;

    PlayerActionMap _input;

    public float testItemWeight;

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

        
        SortItems();
        testItemWeight = GetInventoryWeight();


        _input = new PlayerActionMap();
        _input.Player.Enable();
        
        
    }

    // Update is called once per frame
    

    public void AddItem(BaseItemSO itemToAdd)
    {
        int itemPos = 0;
        bool spaceFound = false;

        Debug.Log("STARTING");

        for(int i = 0; i < items.Length; i++)
        {
            if(items[i] == null || items[i] == itemToAdd)
            {
                Debug.Log("SPACE FOUND");
                if(items[i] == itemToAdd && itemCount[i] == maxItemCount)
                {
                    continue;
                }

                itemPos = i;
                i = items.Length;
                spaceFound = true;
            }
        }

        if (spaceFound)
        {
            bool exists = false;

            for(int i = 0; i < referenceItems.Length; i++)
            {
                if(referenceItems[i] == itemToAdd)
                {
                    exists = true;
                    i = referenceItems.Length;
                }
            }

            if (exists)
            {
                items[itemPos] = itemToAdd;
                itemCount[itemPos]++;
            }
            else
            {
                //Flag an error regarding how the item doesn't exist
            }

        }

        
    }

    public void QueryToRemove(string itemName)
    {
        //Used to check if the item's name exists in reference items
        for(int i = 0; i < items.Length; i++)
        {
            if(items[i].itemName == itemName)
            {
                RemoveItems(items[i]);
                i = items.Length;
            }
        }
    }

    public BaseItemSO GetItemDetails(int indexToCheck)
    {
        for(int i = 0; i < items.Length; i++)
        {
            if(indexToCheck == i)
            {
                if(items[indexToCheck] != null)
                {
                    return items[indexToCheck];
                }
            }
        }

        return null;
    }

    public void RemoveItems(BaseItemSO itemToRemove)
    {
        bool itemFound = false;
        int itemPos = 0;

        for(int i = 0; i < items.Length; i++)
        {
            if(items[i] == itemToRemove)
            {
                itemFound = true;
                itemPos = i;

                i = items.Length;
            }
        }

        if (itemFound)
        {
            itemCount[itemPos]--;
            if (itemCount[itemPos] == 0)
            {
                items[itemPos] = null;
                SortItems();
            }
        }
    }

    public void SortItems()
    {
        bool itemAfterSpace = true;

        while (itemAfterSpace)
        {
            itemAfterSpace = false;
            for(int i = 0; i < items.Length - 1; i++)
            {
                if(items[i] == null)
                {
                    items[i] = items[i + 1];
                    items[i + 1] = null;

                    itemCount[i] = itemCount[i + 1];
                    itemCount[i + 1] = 0;

                    if(items[i] != null)
                    {
                        itemAfterSpace = true;
                    }

                }
            }
        }
    }

    public float GetInventoryWeight()
    {
        float totalInventoryWeight = 0;

        for(int i = 0; i < items.Length; i++)
        {
            if(items[i] == null)
            {
                continue;
            }

            totalInventoryWeight += items[i].itemWeight * itemCount[i];
        }

        return totalInventoryWeight;
    }

    void TestAddItem(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        AddItem(referenceItems[0]);
    }

    void TestRemoveItem(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        QueryToRemove("Potion");
    }
}
