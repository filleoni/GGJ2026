using UnityEngine;

public class Cursor : MonoBehaviour
{
    private void Start()
    {
        transform.SetParent(null);    
    }

    void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
    }
}
