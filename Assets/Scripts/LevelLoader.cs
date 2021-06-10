using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    //public GameObject loadingScreen;
    //public Slider slider;
    public Animator transition;
    public float transitionTime = 0.5f;

    public static LevelLoader instance;

    public static LevelLoader GetInstance()
    {
        return instance;
    }
    private void Awake()
    {
        instance = this;
    }
    public enum Scene
    {
        GameScene,
        HelpScene,
        MainMenuScene
    }
    public void Load(Scene scene)
    {
        StartCoroutine(LoadSynchronously(scene));
    }

    /*IEnumerator LoadAsynchronously(Scene scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene.ToString());
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            yield return null; //waits for a frame
        }
        transition.SetTrigger("Start");
    }*/

    IEnumerator LoadSynchronously(Scene scene)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(scene.ToString());
    }
}
