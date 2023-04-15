using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixerGroup _musicGroup;
    [SerializeField] AudioMixerGroup _SFXGroup;

    //
    [SerializeField] Sound[] sounds;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {

            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.playOnAwake = false;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            if (s.name.Contains("FX"))
            {
                s.source.outputAudioMixerGroup = _SFXGroup;
            }
            else
            {
                s.source.outputAudioMixerGroup = _musicGroup;
            }
        }
    }


    public void Play(string name)
    {

        Sound s = null;

        for(int i = 0; i < sounds.Length; i++)
        {
            if(sounds[i].name == name)
            {
                s = sounds[i];
                Debug.Log(s.source.clip.name);
                i = sounds.Length;
            }
        }

        if (s == null)
        {
            return;
        }


        for(int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].source.isPlaying && !name.Contains("FX"))
            {
                sounds[i].source.Stop();
            }
        }
        
        
        s.source.Play();

    }

    public bool _IsPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) return false;

        return (s.source.isPlaying);
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            return;
        }

        s.source.Stop();
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
