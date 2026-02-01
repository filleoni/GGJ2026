using UnityEngine;

public class cameraManager : MonoBehaviour
{

    [SerializeField] GameObject gameManager;

    void Started()
    {
        gameManager.GetComponent<GameManager>().StartGame();
    }
}
