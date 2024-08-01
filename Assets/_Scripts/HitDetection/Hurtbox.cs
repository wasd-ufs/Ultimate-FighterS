using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Hurtbox : MonoBehaviour
{
    [SerializeField] private GameObject owner;
    [SerializeField] private bool isInvincible;

    [SerializeField] private UnityEvent onHurtBoxDetected;
    public GameObject Owner => owner;
    public bool IsInvincible => isInvincible;

    void Start()
    {
        
    }

    //Method called of HitBox unity Event 
    public void OnHurted(GameObject o)
    {
        if (owner == o)
        {
            //Call hitlag
            onHurtBoxDetected.Invoke();
        }
        return;
    }
}