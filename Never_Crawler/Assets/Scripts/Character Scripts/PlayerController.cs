using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum encumbranceStates
{
    normal,
    slight,
    heavy,
    tooHeavy
}

public class PlayerController : MonoBehaviour
{
    [Header("Character Class Brain")]
    public BaseClassSO classBrain;

    [Header("Assigned Class Moves")]
    public AbilitySO[] _assignedMoves;

    //Speed at which player orients to face current movement direction
    [Header("General Movement Values")]
    public float movementSpeed;
    public float rotationSmooth;
    [SerializeField] float _weightedSpeedModifier;

    //Test Values/References
    public GameObject testProjectile;

    PlayerStats _stats;
    float _currSmoothVelocity;
    Rigidbody _rb;
    Vector2 _moveDir;

    GameObject testSphere;

    public PlayerActionMap _playerInput;
    public HealthSystem _healthSystem;

    [HideInInspector] public encumbranceStates encumbranceState = encumbranceStates.normal;

    // Start is called before the first frame update
    void Start()
    {
        if(CreatorDataHandler.chosenClass != null)
        {
            classBrain = CreatorDataHandler.chosenClass;
        }

        _rb = GetComponent<Rigidbody>();
        _stats = GetComponent<PlayerStats>();
        
        _playerInput = new PlayerActionMap();
        _playerInput.Player.Enable();
        _playerInput.Player.Look.Enable();
        _playerInput.Player.AbilitySlot1.performed += TriggerAbility1;
        _playerInput.Player.TestNoiseAction.performed += NoiseTest;
        _healthSystem = new HealthSystem(_stats.maxHealth);
        _healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }

    // Update is called once per frame
    void Update()
    {
        _moveDir = _playerInput.Player.Movement.ReadValue<Vector2>();
        _moveDir = _moveDir.normalized;
    }

    private void FixedUpdate()
    {
        RelativeCameraMovement();
    }

    void RelativeCameraMovement()
    {
        //Get the camera's current forward and right vectors relative to the the world space
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        //Set forward and right's y axis values to 0, prevents movement from being slow when the camera is angled downwards
        forward.y = 0;
        right.y = 0;

        forward = forward.normalized;
        right = right.normalized;

        //Get the rotated vectors by multiplying the forward and right vectors by the respective input values
        Vector3 forwardRelativeInput = forward * _moveDir.y;
        Vector3 rightRelativeInput = right * _moveDir.x;

        //Add the two together to get the appropriate direction to move in
        Vector3 camRelativeMovement = forwardRelativeInput + rightRelativeInput;

        if(camRelativeMovement.magnitude != 0)
        {
            
            float targetAngle = Mathf.Atan2(camRelativeMovement.x, camRelativeMovement.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currSmoothVelocity, rotationSmooth);
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
        

        //Set velocity to the relative movement axes multiplied by speed (And any additional factors that will be calculated later on
        _rb.velocity = new Vector3(camRelativeMovement.x * movementSpeed * _weightedSpeedModifier, _rb.velocity.y, camRelativeMovement.z * movementSpeed * _weightedSpeedModifier);
    }


    #region Ability Triggers

    void TriggerAbility1(InputAction.CallbackContext context)
    {
        TriggerAbilitySlot(0);
    }

    void TriggerAbilitySlot(int slotIndex)
    {
        if (_assignedMoves[slotIndex].type == AttackType.strength)
        {
            _assignedMoves[slotIndex].UseAbility(_stats.strength.GetScoreModifier(), this);
        } else if(_assignedMoves[slotIndex].type == AttackType.dexterity)
        {
            _assignedMoves[slotIndex].UseAbility(_stats.dexterity.GetScoreModifier(), this);
        } else if(_assignedMoves[slotIndex].type == AttackType.intelligence)
        {
            _assignedMoves[slotIndex].UseAbility(_stats.intelligence.GetScoreModifier(), this);
        }
    }

    #endregion

    void ResetPrimitiveObject()
    {
        Destroy(testSphere);
    }

    void NoiseTest(InputAction.CallbackContext context)
    {
        Noise.MakeNoise(transform.position, 3f);

        if(testSphere != null)
        {
            ResetPrimitiveObject();
            //CancelInvoke("ResetPrimitiveObject");
        }

        /*
        testSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        testSphere.transform.position = transform.position;
        testSphere.transform.localScale = testSphere.transform.localScale * 3f;
        Invoke("ResetPrimitiveObject", 2f);*/
        

    }

    public void CheckCarryWeight()
    {
        if(ItemManager.instance.testItemWeight < _stats.strength.GetBaseValue() * 5)
        {
            encumbranceState = encumbranceStates.normal;
            _weightedSpeedModifier = 1f;
        } else if(ItemManager.instance.testItemWeight >= _stats.strength.GetBaseValue() * 5 && ItemManager.instance.testItemWeight < _stats.strength.GetBaseValue() * 10)
        {
            encumbranceState = encumbranceStates.slight;
            _weightedSpeedModifier = .67f;
        }
        else if (ItemManager.instance.testItemWeight >= _stats.strength.GetBaseValue() * 10 && ItemManager.instance.testItemWeight < _stats.strength.GetBaseValue() * 15)
        {
            encumbranceState = encumbranceStates.heavy;
            _weightedSpeedModifier = .33f;
        }
        else if (ItemManager.instance.testItemWeight >= _stats.strength.GetBaseValue() * 15)
        {
            encumbranceState = encumbranceStates.tooHeavy;
            _weightedSpeedModifier = 0f;
        }
    }

    public PlayerStats GetPlayerStats()
    {
        if (GetComponent<PlayerStats>())
        {
            return GetComponent<PlayerStats>();
        }
        else
        {
            return null;
        }
    }

    public GameObject GetProjectile(string projectileName)
    {
        return null;
    }

    void HealthSystem_OnHealthChanged(object obj, System.EventArgs e)
    {

    }

}
