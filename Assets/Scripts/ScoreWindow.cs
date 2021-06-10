using UnityEngine;
using UnityEngine.UI;

public class ScoreWindow : MonoBehaviour
{
    private Text scoreText;

    private void Awake(){
        scoreText = transform.Find("ScoreText").GetComponent<Text>();
    }

    private void Update(){
        scoreText.text = Level.finalScore.ToString() + " | " + Level.score.ToString();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}
