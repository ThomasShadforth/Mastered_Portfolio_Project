using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParticle : MonoBehaviour
{
    [SerializeField]
    ParticleSystem _weaponTrail;
    [SerializeField]
    ParticleSystem _weaponGlow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayParticleSystem()
    {
        if (_weaponTrail != null)
        {
            _weaponTrail.Play();
        }
    }
}
