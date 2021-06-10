using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class AudioHandler
{
    public enum Sound
    {
        PointOne,
        PointTwo,
        Death,
        TimeSlowPoint,
        MainGame,
        RewindSound,
        GameOver
    }

    public static void PlayAudio(Sound sound, bool isLoop)
    {
        GameObject gameObject = new GameObject(sound.ToString(), typeof(AudioSource));
        
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        if (sound == Sound.RewindSound)
        {
            audioSource.volume = 0.2f;
        }
        if (sound == Sound.RewindSound || sound == Sound.MainGame)
        {
            gameObject.tag = sound.ToString();
        }

        if (isLoop)
        {
            audioSource.loop = true;
            audioSource.volume = 0.2f;
            audioSource.clip = GetAudioClip(sound);
            audioSource.Play();
        }
        else
        {
            audioSource.PlayOneShot(GetAudioClip(sound));
        }
    }

    public static void StopAudioForGameOver()
    {
        //GameObject.FindGameObjectWithTag(Sound.MainGame.ToString())?.SetActive(false);
        GameObject.FindGameObjectWithTag(Sound.RewindSound.ToString())?.SetActive(false);
    }


    public static AudioClip GetAudioClip(Sound sound)
    {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.GetInstance().soundAudioClipArray)
        {
            if(soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound: " + sound + " not found!");
        return null;
    }
}
