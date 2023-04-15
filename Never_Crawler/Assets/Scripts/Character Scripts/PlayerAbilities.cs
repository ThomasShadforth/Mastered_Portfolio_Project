using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController))]
public class PlayerAbilities : MonoBehaviour
{
    PlayerController _player;
    BaseClassSO _playerClass;

    [SerializeField]
    AbilitySO[] _assignedMoves;

    [Header("Player Weapon Prefabs")]
    [SerializeField] GameObject _playerSword;
    [SerializeField] GameObject _playerWand;

    PlayerActionMap _playerInput;

    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponent<PlayerController>();
        

        if(CreatorDataHandler.chosenClass != null)
        {
            _playerClass = CreatorDataHandler.chosenClass;
        }

        StartCoroutine(SetAbilitiesCo());
        SetClassWeapon();

        _playerInput = new PlayerActionMap();
        _playerInput.Player.Enable();
        _playerInput.Player.Look.Enable();


        _playerInput.Player.AbilitySlot1.performed += TriggerAbility1;
        _playerInput.Player.AbilitySlot2.performed += TriggerAbility2;
        _playerInput.Player.AbilitySlot3.performed += TriggerAbility3;
        _playerInput.Player.AbilitySlot4.performed += TriggerAbility4;
    }

    void TriggerAbility1(InputAction.CallbackContext context)
    {
        TriggerAbilitySlot(0);
    }

    void TriggerAbility2(InputAction.CallbackContext context)
    {
        TriggerAbilitySlot(1);
    }

    void TriggerAbility3(InputAction.CallbackContext context)
    {
        TriggerAbilitySlot(2);
    }

    void TriggerAbility4(InputAction.CallbackContext context)
    {
        TriggerAbilitySlot(3);
    }

    void TriggerAbilitySlot(int slotIndex)
    {
        if (_assignedMoves[slotIndex] != null)
        {
            _assignedMoves[slotIndex].UseAbility(_player);
        }
    }

    IEnumerator SetAbilitiesCo()
    {
        yield return new WaitForSeconds(.2f);

        _SetDefaultAbilities();
    }

    void _SetDefaultAbilities()
    {
        bool changeSlot = !CharacterData.firstLoadDone;

        for(int i = 0; i < _assignedMoves.Length; i++)
        {
            FindObjectOfType<ClassMenu>().AssignActionToSlot(i, _playerClass.GetDefaultAbility(_assignedMoves), changeSlot);
        }
    }

    public BaseClassSO GetPlayerClass()
    {
        return _playerClass;
    }

    public void SetPlayerClass(BaseClassSO playerClass)
    {
        _playerClass = playerClass;
    }

    public AbilitySO[] GetAssignedMovesList()
    {
        return _assignedMoves;
    }

    void SetClassWeapon()
    {
        if (_playerClass.className == "Fighter")
        {
            if (_playerSword != null)
            {
                _playerSword.SetActive(true);
            }
        }
        else if (_playerClass.className == "Wizard")
        {
            if (_playerWand != null)
            {
                _playerWand.SetActive(true);
            }
        }
    }
}
