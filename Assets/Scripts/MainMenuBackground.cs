using UnityEngine;

public class MainMenuBackground : MonoBehaviour
{

    Animator mainMenuAnimator;

    private void Awake()
    {
        mainMenuAnimator = this.gameObject.GetComponent<Animator>();
        InvokeRepeating("RunMainMenuBGAnimation", 5, 15);
    }

    private void RunMainMenuBGAnimation()
    {
        mainMenuAnimator.SetTrigger("StartMainMenu");
    }
}
