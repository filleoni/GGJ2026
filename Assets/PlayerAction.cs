using UnityEngine;
using System.Collections.Generic;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] List<PlayerMask> masks = null;

    [SerializeField] SpriteRenderer maskVisual;

    int currentMaskIndex = 0;

    PlayerMask CurrentMask => masks[Mathf.Clamp(currentMaskIndex, 0, masks.Count - 1)];

    void Start()
    {
        SetMask(currentMaskIndex);
    }

    private void Update()
    {
        maskVisual.transform.rotation = Quaternion.Euler(0, 0, 0);

        ProcessInput();
    }

    void ProcessInput()
    {
        bool changed = false;
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentMaskIndex++;
            if (currentMaskIndex >= masks.Count)
                currentMaskIndex = 0;

            changed = true;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentMaskIndex--;
            if (currentMaskIndex < 0)
                currentMaskIndex = masks.Count - 1;

            changed = true;
        }

        if (changed)
            SetMask(currentMaskIndex);
    }

    void SetMask(int index)
    {
        currentMaskIndex = index;

        maskVisual.sprite = CurrentMask.image;
    }
}
