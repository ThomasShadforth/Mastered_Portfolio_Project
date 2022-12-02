using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise
{
    public delegate void OnNoise(Vector3 position, float distance);

    public static event OnNoise SoundEvent;

    public static void MakeNoise(Vector3 position, float radius)
    {
        if(SoundEvent != null)
        {
            SoundEvent.Invoke(position, radius);
        }
    }

}
