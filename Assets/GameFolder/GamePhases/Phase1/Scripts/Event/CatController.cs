using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CatController : MonoBehaviour
{
    [Header("Tempo para o ovento")]
    [SerializeField]private float Timee;
    [SerializeField] private float TimeSpeed;

    [Header("Conometro")]
    [SerializeField]private float TimeEvent;

    public UnityEvent cat;

    void Start()
    {
        TimeEvent = Timee;        
    }


    void Update()
    {
        TimeEvent -= Time.deltaTime * TimeSpeed;

        if (TimeEvent <= 0)
        {
            TimeEvent = Timee;

            //Chama uma animacao
        }
    }

}
