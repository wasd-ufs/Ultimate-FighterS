using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe responsavel por executar a acao a esquerda do gato
/// </summary>
public class CatLeft : CatBaseMoves
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _skin;

    void Awake()
    {
        _skin.SetActive(false);
    }

    /// <summary>
    /// Responsavel por executar o ataque da esquerda
    /// </summary>
    /// <return>void</return>
    /// <author>Wallisson de jesus</author>
    public override void Execute()
    {
        _skin.SetActive(true);
        _animator.Play("CatLeftPunch",-1);
    }


    /// <summary>
    /// Responsavel por desabilitar o ataque da esquerda
    /// </summary>
    /// <return>void</return>
    /// <author>Wallisson de jesus</author>
    public override void Hide()
    {
        _skin.SetActive(false);
    }
}
