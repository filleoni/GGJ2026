using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "newMask", menuName = "SO/PlayerMask", order = 0)]
public class PlayerMask : ScriptableObject
{
    public string name;
    public Sprite image;
    public UnityEvent attack;
}
