using UnityEngine;
using System;

public class SAudioManager : MonoBehaviour
{

    // MG added floats for audio settings
    public float masterVolume = 1f;
    public float musicVolume = 1f;


    public static SAudioManager Instance;


    public SSound[] sounds;
    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (SSound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = s.spatialBlend;
        }

    }
    void Start()
    {
        Play("Level_01");
    }

    public void Play(string name)
    {
        SSound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.Log("Sound Ain't Working");
            return;
        }

        s.source.Play();
    }

    public void Stop(string name)
    {
        SSound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Stopping");
            return;
        }

        s.source.Stop();
    }
    public bool IsPlaying(string name)
    {
        SSound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
            return false;
        }

        return s.source.isPlaying;
    }


    public void SetVolume(string name, float volume)
    {
        //MG added master and music for the audio settings
        if (name == "Master")
        {
            masterVolume = volume;
            ApplyVolumes();
            return;
        }

        if (name == "Music")
        {
            musicVolume = volume;
            ApplyVolumes();
            return;
        }

        SSound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.volume = volume * masterVolume;
        }
    }

    public float GetVolume(string name)
    {
        SSound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            return s.source.volume;
        }
        return 0f;
    }

    //MG added method for audio settings
    private void ApplyVolumes()
    {
        foreach (SSound s in sounds)
        {
            float category = 1f;

            if (s.name.Contains("Music") || s.name == "Level_01") // adjust to fit your naming
                category = musicVolume;

            s.source.volume = s.volume * masterVolume * category;
        }
    }

    public void Pause(string name)
    {
        SSound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found to pause");
            return;
        }

        s.source.Pause();
    }

    public void Resume(string name)
    {
        SSound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found to resume");
            return;
        }

        s.source.UnPause();
    }

}