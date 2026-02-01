using UnityEngine;
using Sonity;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject cameraParent;
    [SerializeField] GameObject cam;
    [SerializeField] GameObject enemySpawner;
    [SerializeField] GameObject player;
    [SerializeField] GameObject cursor;
    [SerializeField] GameObject healthBar;
    [SerializeField] SoundEvent mainMusic;
    [SerializeField] SoundEvent startAmbiance;

    void Start()
    {
        cameraParent.GetComponent<Animation>().Play("CameraStart");
        enemySpawner.SetActive(false);
        startAmbiance.Play(transform);
    }
    public void StartPan()
    {
        cameraParent.GetComponent<Animation>().Play("CameraPan");
    }
    public void StartGame()
    {
        cam.GetComponent<CameraMovement>().canMove = true;
        cursor.SetActive(true);
        enemySpawner.SetActive(true);
        player.SetActive(true);
        healthBar.SetActive(true);
        startAmbiance.Stop(transform,true);
        mainMusic.Play(transform);
        healthBar.SetActive(true);
    }
}
