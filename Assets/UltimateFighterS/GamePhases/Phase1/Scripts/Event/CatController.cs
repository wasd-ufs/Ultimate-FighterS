using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CatController : MonoBehaviour
{
    [FormerlySerializedAs("TimeStart")] [Header("Tempo para o ovento")] [SerializeField]
    private float timeStart;

    [FormerlySerializedAs("TimeSpeed")] [SerializeField] private float timeSpeed;

    [FormerlySerializedAs("TimeEvent")] [Header("Conometro")] [SerializeField] private float timeEvent;

    [FormerlySerializedAs("CatMoves")] [SerializeField] private List<CatBaseMoves> catMoves = new();

    public int randomIndex;


    private void Start()
    {
        timeEvent = timeStart;
    }


    private void Update()
    {
        timeEvent -= Time.deltaTime * timeSpeed;

        if (timeEvent <= 0)
        {
            randomIndex = Random.Range(0, catMoves.Count);

            if (catMoves[randomIndex] != null && timeEvent <= 0)
            {
                catMoves[randomIndex].Execute();
                timeEvent = timeStart;
            }

            //StartCoroutine(HideAnimation());
        }
    }

    private IEnumerator HideAnimation()
    {
        yield return new WaitForSeconds(3f);
        catMoves[randomIndex].Hide();
    }
}