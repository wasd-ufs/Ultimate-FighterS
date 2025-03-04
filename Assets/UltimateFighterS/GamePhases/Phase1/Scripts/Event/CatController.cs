using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe responsavel por gerenciar qual acao vai ser executada                                                                                                                                                                                                                
/// </summary>
public class CatController : MonoBehaviour
{
    [Header("Tempo para o ovento")]
    [SerializeField] private float _timeStart;
    [SerializeField] private float _timeSpeed;

    [Header("Conometro")]
    [SerializeField]private float _timeEvent;

    [SerializeField] private List<CatBaseMoves> _catMoves = new List<CatBaseMoves>();

    private int _randomIndex;


    void Start()
    {
       _timeEvent = _timeStart;        
    }


    void Update()
    {
        _timeEvent -= Time.deltaTime * _timeSpeed;

        if (_timeEvent <= 0)
        {
            _randomIndex = UnityEngine.Random.Range(0, _catMoves.Count);

            if (_catMoves[_randomIndex] != null && _timeEvent <= 0)
            {
                _catMoves[_randomIndex].Execute();
                _timeEvent = _timeStart;
            }
        }
    }
}
    
