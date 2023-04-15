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

public class PlayerController : Subject
{
    //Speed at which player orients to face current movement direction
    [Header("General Movement Values")]
    public float movementSpeed;
    public float rotationSmooth;
    [SerializeField] float _weightedSpeedModifier;

    public bool attacking;

    HealthBar _playerHealthBar;
    PlayerStats _stats;
    float _currSmoothVelocity;
    Rigidbody _rb;
    
    [HideInInspector]
    public Vector2 _moveDir;

    PlayerAbilities _playerAbilities;
    public PlayerActionMap _playerInput;
    public HealthSystem _healthSystem;

    [HideInInspector] public encumbranceStates encumbranceState = encumbranceStates.normal;
    // Start is called before the first frame update
    void Start()
    {
        attacking = false;

        _rb = GetComponent<Rigidbody>();
        _stats = GetComponent<PlayerStats>();
        
        _playerInput = new PlayerActionMap();
        _playerInput.Player.Enable();
        _playerInput.Player.Look.Enable();
        _playerInput.Player.TestNoiseAction.performed += NoiseTest;
        _healthSystem = new HealthSystem(_stats.maxHealth);
        
        _healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        _playerHealthBar = GameObject.Find("PlayerHealthBar").GetComponent<HealthBar>();
        _playerHealthBar.UpdateHealthFillAmount(_healthSystem.GetHealthPercent());
        

        //StartCoroutine(SetAbilities());
        
        ResetCameraOrientation();

        
    }

    // Update is called once per frame
    void Update()
    {
        _moveDir = _playerInput.Player.Movement.ReadValue<Vector2>();
        _moveDir = _moveDir.normalized;
    }

    private void FixedUpdate()
    {
        
        if (attacking)
        {
            AttackCameraMovement();
        }
        else
        {
            RelativeCameraMovement();
        }   
    }

    void AttackCameraMovement()
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.eulerAngles.y, transform.localEulerAngles.z);

        _rb.velocity = (transform.forward * _moveDir.y *movementSpeed) + (transform.right * _moveDir.x * movementSpeed);
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


    public void ResetCameraOrientation()
    {
        Camera.main.transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    void NoiseTest(InputAction.CallbackContext context)
    {
        Noise.MakeNoise(transform.position, 3f);
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

    public void PrepareCombatNotify(CombatActionEnum combatAction)
    {
        NotifyObservers(combatAction);
    }

    public void PrepareCombatNotify(CombatActionEnum actionType, CombatActionEnum diceNum, CombatActionEnum maxDamage, CombatActionEnum modifier)
    {
        NotifyObservers(actionType, diceNum, maxDamage, modifier);
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

    void HealthSystem_OnHealthChanged(object obj, System.EventArgs e)
    {
        if(_playerHealthBar != null)
        {
            _playerHealthBar.UpdateHealthFillAmount(_healthSystem.GetHealthPercent());
        }

        if (_healthSystem.CheckIsDead())
        {
            
            NotifyObservers(CombatActionEnum.player_Dead, CombatActionEnum.enemy_Died, CombatActionEnum.enemy_Died, CombatActionEnum.enemy_Died);
        }
    }
}
