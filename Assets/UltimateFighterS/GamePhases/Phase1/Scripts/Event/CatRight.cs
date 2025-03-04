using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe responsavel por executar a acao a direita do gato
/// </summary>
public class CatRight : CatBaseMoves
{

    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _skin;

    void Awake()
    {
        _skin.SetActive(false);
    }

    /// <summary>
    /// Responsavel por executar o ataque da direita
    /// </summary>
    /// <return>void</return>
    /// <author>Wallisson de jesus</author>
    public override void Execute()
    {
        _skin.SetActive(true);
        _animator.Play("CatRightPunch",-1);
    }

    /// <summary>
    /// Responsavel por desabilitar o ataque da direita
    /// </summary>
    /// <return>void</return>
    /// <author>Wallisson de jesus</author>
    public override void Hide()
    {
        _skin.SetActive(false);
    }
}
