using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ClassMenu : MonoBehaviour
{
    public ClassButton[] classButtons;
    public ActionSlot[] actionSlots;

    [SerializeField] GameObject _classScreen;

    int activeAbilityIndex;
    ClassMoveSO activeAbility;
    [HideInInspector]
    public AbilitySO activeAbilitySO;


    BaseClassSO _playerClass;

    PlayerController _player;
    PlayerAbilities _playerAbilities;

    [Header("Class Menu Panels")]
    [SerializeField] GameObject abilitySlotsPanel;

    [Header("UI Fields")]
    [SerializeField] TextMeshProUGUI _abilityNameText;
    [SerializeField] TextMeshProUGUI _abilityDescriptionText;
    [SerializeField] TextMeshProUGUI _levelRequirementText;
    [SerializeField] TextMeshProUGUI _buttonEquipText;
    [SerializeField] GameObject _assignButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        

        _player = FindObjectOfType<PlayerController>();

        if (_player != null)
        {
            
            _playerAbilities = _player.GetComponent<PlayerAbilities>();
            _playerClass = _playerAbilities.GetPlayerClass();
        }
    }

    public void OpenClassMenu()
    {
        for (int i = 0; i < classButtons.Length; i++)
        {
            
            if (i < GetPlayerClass().knownMoves.Length)
            {
                classButtons[i].buttonValue = i;
                classButtons[i].buttonText.text = GetPlayerClass().knownMoves[i].abilityName;
            }
            else
            {
                classButtons[i].gameObject.SetActive(false);
            }
            //Get the icon for the move in the array position, set it as the button's icon
            //Rather than make a separate UI for each individual class, streamlines the system and simplifies it down to one class menu
        }

        classButtons[0].Press();
    }

    public void DisplayAbilityInfo(AbilitySO abilityToDisplay)
    {
        if(abilityToDisplay == null)
        {
            return;
        }

        //Set the active move to the pass parameter
        activeAbilitySO = abilityToDisplay;
        _abilityNameText.text = activeAbilitySO.abilityName;
        _abilityDescriptionText.text = activeAbilitySO.description;

        _levelRequirementText.text = "Required Level: " + activeAbilitySO.requiredLevel;


        if(_player.GetComponent<PlayerStats>().currentLevel >= activeAbilitySO.requiredLevel)
        {
            _buttonEquipText.text = "Equip";
            //Set button based on whether the player meets the requirement or not
            _assignButton.GetComponent<Button>().interactable = true;
            _assignButton.GetComponent<Button>().image.color = Color.white;
        }
        else
        {
            _buttonEquipText.text = "";
            _assignButton.GetComponent<Button>().interactable = false;
            _assignButton.GetComponent<Button>().image.color = Color.gray;
        }
    }

    public void DisplaySlotChoiceMenu()
    {
        if (!abilitySlotsPanel.activeInHierarchy)
        {
            abilitySlotsPanel.SetActive(true);

            for (int i = 0; i < actionSlots.Length; i++)
            {
                actionSlots[i].buttonValue = i;

                //Temporarily set text (This is being used while the actions do not have icons

                if(actionSlots[i].assignedAction == null)
                {
                    actionSlots[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
                }
                else
                {
                    actionSlots[i].GetComponentInChildren<TextMeshProUGUI>().text = actionSlots[i].assignedAction.abilityName;
                }
                //Get the icon for the move and set it as a button image at a later point in time
            }
        }
        else
        {
            abilitySlotsPanel.SetActive(false);
        }
    }

    public void AssignActionToSlot(int slotNumber, AbilitySO abilityToAssign = null, bool changeSlot = true)
    {
        if(_player == null)
        {
            
            return;
        }

        //Assign the activeAbility to the slot (If applicable)
        int secondarySlotNum = 0;
        bool alreadyAssigned = false;

        

        for(int i = 0; i < actionSlots.Length; i++)
        {
            //If already assigned to another slot
            if(actionSlots[i].assignedAction == abilityToAssign)
            {
                if(i == slotNumber)
                {
                    //Return a message saying that you've already assigned this action to this slot
                    i = actionSlots.Length;
                    
                    break;
                }

                secondarySlotNum = i;
                alreadyAssigned = true;
                i = actionSlots.Length;
            }
        }

        if (alreadyAssigned)
        {
            //If already assigned to another slot:

            if (changeSlot)
            {
                AbilitySO actionToSwap = actionSlots[slotNumber].assignedAction;


                actionSlots[slotNumber].assignedAction = abilityToAssign;
                _playerAbilities.GetAssignedMovesList()[slotNumber] = abilityToAssign;//_assignedMoves[slotNumber] = abilityToAssign;

                actionSlots[secondarySlotNum].assignedAction = actionToSwap;
                _playerAbilities.GetAssignedMovesList()[secondarySlotNum] = actionToSwap; //_assignedMoves[secondarySlotNum] = actionToSwap;
            }
            else
            {
                _playerAbilities.GetAssignedMovesList()[slotNumber] = abilityToAssign;
            }
        }
        else
        {
            actionSlots[slotNumber].assignedAction = abilityToAssign;
            _playerAbilities.GetAssignedMovesList()[slotNumber] = abilityToAssign;//._assignedMoves[slotNumber] = abilityToAssign;
        }

        //Set the buttonImage, then close the panel
        if (_classScreen.activeInHierarchy)
        {
            DisplaySlotChoiceMenu();
        }
    }

    public BaseClassSO GetPlayerClass()
    {
        return _playerClass;
    }
}
