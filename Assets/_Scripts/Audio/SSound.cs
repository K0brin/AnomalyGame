using UnityEngine;

[System.Serializable]
public class SSound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;

    [Range(0f, 1f)]
    public float spatialBlend = 0f;



    [HideInInspector]
    public AudioSource source;

}