using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{

    public AudioClip buttonEnterSound;

    public void OnClickPlay()
    {
        LevelLoader.GetInstance().Load(LevelLoader.Scene.GameScene);
    }
    public void OnClickHelp()
    {
        LevelLoader.GetInstance().Load(LevelLoader.Scene.HelpScene);
    }

    public void OnHoverPlaySound()
    {
        GameObject gameObject = new GameObject("Sound", typeof(AudioSource));
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(buttonEnterSound);
    }
}
