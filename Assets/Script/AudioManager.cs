using System;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private void Start()
    {
        PlayMusic(SoundName.Theme);
    }

    public void PlayMusic(SoundName name)
    {
        Sound s = Array.Find(musicSounds, x => x.Name == name);

        if (s == null)
        {
            Debug.Log("sound not found");
        }
        else
        {
            musicSource.clip = s.Clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(SoundName name)
    {
        Sound s = Array.Find(sfxSounds, x => x.Name == name);

        if (s == null)
        {
            Debug.Log("sound not found");
        }
        else
        {
            sfxSource.PlayOneShot(s.Clip);
        }
    }
}
