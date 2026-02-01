using UnityEngine;

public class Cursor : MonoBehaviour
{
    private void Start()
    {
        transform.SetParent(null);  
        gameObject.SetActive(false);  
    }

    void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
    }
}
