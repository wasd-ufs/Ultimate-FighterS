using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesactivateObj : MonoBehaviour
{
    [SerializeField] public GameObject objectToSet;
    [SerializeField] public bool set;
    public void Desactivate()
    {
        objectToSet.SetActive(set);
    }
    
    
    
}
