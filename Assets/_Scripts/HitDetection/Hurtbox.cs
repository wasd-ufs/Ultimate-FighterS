using Unity.VisualScripting;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    [SerializeField] private GameObject owner;
    [SerializeField] private bool isInvincible;
    
    public GameObject Owner => owner;
    public bool IsInvincible => isInvincible;
}