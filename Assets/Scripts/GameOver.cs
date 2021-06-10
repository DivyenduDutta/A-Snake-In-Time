using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public AudioClip buttonEnterSound;
    public void LoadLevelForRetry(){
        LevelLoader.GetInstance().Load(LevelLoader.Scene.GameScene);
    }

    public void OnClickMainMenu()
    {
        LevelLoader.GetInstance().Load(LevelLoader.Scene.MainMenuScene);
    }
    public void OnHoverPlaySound()
    {
        GameObject gameObject = new GameObject("Sound", typeof(AudioSource));
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(buttonEnterSound);
        Destroy(gameObject, 2);
    }
}
