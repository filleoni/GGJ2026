using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    int score;

    void Start()
    {
        Star.SignalScore.AddListener(OnScore);
    }

    void OnScore()
    {
        score++;
        text.text = score.ToString();
    }
}
