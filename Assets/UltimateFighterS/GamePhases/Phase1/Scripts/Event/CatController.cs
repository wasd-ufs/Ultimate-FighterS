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
          
            randomIndex = UnityEngine.Random.Range(0, CatMoves.Count);

            if (CatMoves[randomIndex] != null && TimeEvent <= 0)
            {
                CatMoves[randomIndex].Execute();
                TimeEvent = TimeStart;
            }

            //StartCoroutine(HideAnimation());
          
        }
    }
    IEnumerator HideAnimation()
    {
        yield return new WaitForSeconds (3f);
        CatMoves[randomIndex].Hide();
    }

}
    
