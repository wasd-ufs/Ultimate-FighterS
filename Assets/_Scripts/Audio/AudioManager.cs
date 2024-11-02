using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;
    [Range(0f, 1f)]
    public float overallVolume = 1f;

    void Awake() {

        if (instance == null){ 
            instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume * overallVolume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }


    public void Play(string name) {
        Sound s = Array.Find(sounds,sound => sound.name == name);
        if (s == null) {
            return;
        }

        s.source.Play();
    }

    public void SetGlobalVolume(float volume) {
        overallVolume = Mathf.Clamp01(volume); 
        foreach (Sound s in sounds)
        {
            s.source.volume = s.volume * overallVolume;
        }
    }
}

