using Unity.VisualScripting;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    [SerializeField] private GameObject owner;
    [SerializeField] private bool isInvincible;
    
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
            //Call damage and hitlag
            Debug.Log("Danou");


        }
        Debug.Log("Nao sou eu");
        return;
    }
}