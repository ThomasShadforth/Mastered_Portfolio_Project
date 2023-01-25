using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIThinker : MonoBehaviour
{
    [Header("General Enemy Properties")]
    public int enemyMaxHealth;
    public float chaseSpeed;
    HealthBar _enemyHealthBar;
    public HealthSystem healthSystem;
    [HideInInspector]
    public AIStats stats;

    [Header("AI State Properties")]
    public State currentState;
    public State remainState;

    [Header("AI Radius Properties")]
    public float noiseCheckRadius;
    public float minAttackDist;

    [Header("AI Patrol Properties")]
    public Transform[] patrolPoints;
    public float minDistFromPoint = .5f;
    [HideInInspector] public int currentPatrolIndex = 0;
    [HideInInspector] public bool initialDestinationSet = false;

    [Header("AI Timer Properties")]
    public float waitTime;
    [HideInInspector]
    public float waitTimer;
    public float investigateTime;
    [HideInInspector]
    public float investigateTimer;

    

    public float attackCoolTime;
    [HideInInspector]
    public float attackCoolTimer;

    [Header("Additional Config Values")]
    public NavMeshAgent agent;
    public LayerMask playerLayer;
    public float rotationSmooth;
    public float currSmoothVelocity;

    [Header("Test prefab values")]
    //Test projectile prefab reference
    public GameObject testProjectile;

    [HideInInspector] public Transform playerTarget;

    //Misc Values: Noise tracking, etc.
    
    [HideInInspector] public Rigidbody _rb;
    Vector3 _noisePosition = Vector3.zero;
    bool _investigatingNoise;
    float _noiseRadius;

    LineOfSight enemyLOS;
    [HideInInspector]
    public bool canSeePlayer;

    private void OnEnable()
    {
        Noise.SoundEvent += OnHearNoise;
    }

    private void OnDisable()
    {
        Noise.SoundEvent -= OnHearNoise;
    }

    private void Awake()
    {
        healthSystem = new HealthSystem(enemyMaxHealth);
    }

    // Start is called before the first frame update
    void Start()
    {
        waitTimer = waitTime;
        investigateTimer = investigateTime;
        _rb = GetComponent<Rigidbody>();
        enemyLOS = GetComponent<LineOfSight>();
        stats = GetComponent<AIStats>();
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        _enemyHealthBar = GetComponentInChildren<HealthBar>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState(this);

            canSeePlayer = enemyLOS.canSeePlayer;
        }
    }

    void HealthSystem_OnHealthChanged(object obj, System.EventArgs e)
    {
        //Reference for health bar needs to be added
        if (_enemyHealthBar != null)
        {
            _enemyHealthBar.UpdateHealthFillAmount(healthSystem.GetHealthPercent());
        }


        if(healthSystem.GetHealth() <= 0)
        {
            //Destroy(gameObject);
        }
    }

    public void TransitionToState(State nextState)
    {
        if(nextState != remainState)
        {
            currentState = nextState;
        }
    }

    public void OnHearNoise(Vector3 position, float radius)
    {
        _noisePosition = position;
        _noiseRadius = radius;


        
    }

    void ResetNoiseValues()
    {
        _noisePosition = Vector3.zero;
        _noiseRadius = 0f;
    }

    public Vector3 GetNoisePosition()
    {
        return _noisePosition;
    }

    public float GetNoiseRadius()
    {
        return _noiseRadius;
    }

    public void SetNoiseInvestigate()
    {
        _investigatingNoise = true;
    }

    public void ResetWaitTimer()
    {
        waitTimer = waitTime;
        investigateTimer = investigateTime;
    }

    public void SetAgentDestination()
    {
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    public void SetAgentNoiseDestination()
    {
        agent.SetDestination(_noisePosition);
    }

    public void SetCooldownTimer()
    {
        attackCoolTimer = attackCoolTime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, noiseCheckRadius);
    }
}
