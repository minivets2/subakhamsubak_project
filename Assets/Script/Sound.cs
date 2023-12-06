using UnityEngine;

public enum SoundName
{
    Theme,
    Create,
    Merge,
    Drop,
    Click,
}

[System.Serializable]
public class Sound
{
    public SoundName Name;
    public AudioClip Clip;
}
