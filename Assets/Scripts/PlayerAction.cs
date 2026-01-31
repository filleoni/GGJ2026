using UnityEngine;
using System.Collections.Generic;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] List<PlayerMask> masks = null;

    [SerializeField] SpriteRenderer maskVisual;

    public Cursor Cursor;
    int currentMaskIndex = 0;

    PlayerMask CurrentMask = null;

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
        bool changedMask = false;
        if (Input.GetButtonDown("NextMask") || Input.GetAxisRaw("ScrollMask") > 0)
        {
            currentMaskIndex++;
            if (currentMaskIndex >= masks.Count)
                currentMaskIndex = 0;

            changedMask = true;
        }
        if (Input.GetButtonDown("PreviousMask") || Input.GetAxisRaw("ScrollMask") < 0)
        {
            currentMaskIndex--;
            if (currentMaskIndex < 0)
                currentMaskIndex = masks.Count - 1;

            changedMask = true;
        }
        if (changedMask)
            SetMask(currentMaskIndex);

        if (CurrentMask)
            CurrentMask.Process?.Invoke(this);
    }

    void SetMask(int index)
    {
        currentMaskIndex = index;

        if (CurrentMask)
            CurrentMask.Unequip(this);

        CurrentMask = masks[currentMaskIndex];
        CurrentMask.Equip(this);

        maskVisual.sprite = CurrentMask.image;
    }
}
