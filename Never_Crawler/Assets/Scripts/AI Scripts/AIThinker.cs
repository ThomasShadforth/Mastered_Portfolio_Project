using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIThinker : MonoBehaviour
{
    [Header("AI State Properties")]
    public State currentState;
    public State remainState;

    [Header("AI Radius Properties")]
    public float noiseCheckRadius;

    [Header("AI Timer Properties")]
    public float waitTime;
    [HideInInspector]
    public float waitTimer;

    //Misc Values: Noise tracking, etc.
    Vector3 _noisePosition = Vector3.zero;
    float _noiseRadius;

    private void OnEnable()
    {
        Noise.SoundEvent += OnHearNoise;
    }

    private void OnDisable()
    {
        Noise.SoundEvent -= OnHearNoise;
    }

    // Start is called before the first frame update
    void Start()
    {
        waitTimer = waitTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState(this);
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

        Invoke("ResetNoiseValues", 2f);
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

    public void ResetWaitTimer()
    {
        waitTimer = waitTime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, noiseCheckRadius);
    }
}
