using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    public float noiseRadius;
    public float smoothTime;

    float _currSmoothVelocity;

    // Start is called before the first frame update
    void Start()
    {
        Noise.SoundEvent += OnHearNoise;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnHearNoise(Vector3 position, float radius) {
        
        if (Vector3.Distance(position, transform.position) <= noiseRadius + radius)
        {
            Debug.Log("NOISE DETECTED");
            transform.LookAt(position, Vector3.up);

        }
    }

    
}
