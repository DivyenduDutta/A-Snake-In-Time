using System;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;

    public Transform snakeSprite;
    public Transform snakeHeadLight;
    public Transform point1;
    public Transform point2;
    public Transform timeSlow;

    public GameObject pointDestroyParticleSys;
    public GameObject plusTwoParticleSys;
    public GameObject plusFourParticleSys;
    public GameObject timeSlowParticleSys;

    public SoundAudioClip[] soundAudioClipArray;

    [Serializable]
    public class SoundAudioClip
    {
        public AudioHandler.Sound sound;
        public AudioClip audioClip;
    }

    public static GameAssets GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }
}
