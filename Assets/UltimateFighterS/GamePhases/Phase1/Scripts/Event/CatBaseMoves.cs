using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe base do gato
/// </summary>
public abstract class CatBaseMoves : MonoBehaviour
{
    /// <summary>
    /// Responsavel por executar uma acao da animacao
    /// </summary>
    /// <return>void</return>
    /// <author>Wallisson de jesus</author>
    public virtual void Execute(){}


    /// <summary>
    /// Responsavel por desabilitar uma acao da animacao
    /// </summary>
    /// <return>void</return>
    /// <author>Wallisson de jesus</author>
    public virtual void Hide(){}
}
