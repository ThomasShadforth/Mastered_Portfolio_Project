using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioComponent : MonoBehaviour
{

    [SerializeField] string _audioName;

    private void Start()
    {
        _PlayAudio();
    }

    void _PlayAudio()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.Play(_audioName);
        }
    }
}
