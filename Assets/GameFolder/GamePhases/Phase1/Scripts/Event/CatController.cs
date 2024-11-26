using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{
    [Header("Tempo para o ovento")]
    [SerializeField] private float TimeStart;
    [SerializeField] private float TimeSpeed;

    [Header("Conometro")]
    [SerializeField]private float TimeEvent;

    [SerializeField] private List<CatBaseMoves> CatMoves = new List<CatBaseMoves>();

    public int randomIndex;


    void Start()
    {
        TimeEvent = TimeStart;        
    }


    void Update()
    {
        TimeEvent -= Time.deltaTime * TimeSpeed;

        if (TimeEvent <= 0)
        {
            TimeEvent = TimeStart;
           
            if (CatMoves.Count > 0)
            {
                randomIndex = UnityEngine.Random.Range(0, CatMoves.Count);
                CatMoves[randomIndex].Execute();

                if (CatMoves[randomIndex].ShowAnimator())
                {
                    CatMoves[randomIndex].Hide();
                }
            }
        }
    }

}
