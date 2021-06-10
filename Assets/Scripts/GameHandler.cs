using UnityEngine;
using TMPro;

public class GameHandler : MonoBehaviour
{
    public GameObject gameOverWindow;

    private void Start()
    {
        AudioHandler.PlayAudio(AudioHandler.Sound.MainGame, true);
    }

    private void Update(){

        gameOverWindow.GetComponentInChildren<TextMeshProUGUI>().SetText("Score:" + Level.finalScore);
        gameOverWindow.SetActive(Snake.GetInstance().gameOver);
    }
}
