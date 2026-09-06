using System.Threading;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public string effectName;
    public Sprite Image;
    public float duration;
    [HideInInspector] public float timeLeft;
}