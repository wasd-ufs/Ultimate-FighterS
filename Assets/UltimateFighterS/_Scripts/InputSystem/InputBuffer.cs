using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

///<summary>
/// Armazena comandos do jogador por um curto período de tempo, permitindo que sejam processados antes de expirarem.
///</summary>
[RequireComponent(typeof(Timer))]
public class InputBuffer : MonoBehaviour
{
    [SerializeField] private InputSystem _input;
    public InputSystemMemento Current { get; private set; }
    private Timer _timer;

    void Awake()
    {
        _timer = gameObject.GetComponent<Timer>();
        _timer.onTimerFinish.AddListener(Consume);
    }

    void Update()
    {
        if (_input.IsAttackJustPressed())
            Register();
        
        if (_input.IsSpecialJustPressed())
            Register();
    }

    ///<summary>
    /// Marca o timer de duração da entrada e captura (registra) o estado atual da entrada.
    ///</summary>
    ///<author>Davi Fontes</author>
    public void Register()
    {
        _timer.Init();
        Current = _input.GetMemento();
    }

    ///<summary>
    /// Encerra o timer e descarta a entrada armazenada.
    ///</summary>
    ///<author>Davi Fontes</author>
    public void Consume()
    {
        _timer.Finish();
        Current = null;
    }

    ///<summary>
    /// Verifica se há uma entrada armazenada e se ela satisfaz uma condição para executar uma ação antes de consumir a entrada.
    ///<param name="condition"> Uma função condicional que retorna um booleano.
    ///<param name="condition"> Uma ação a ser executada.
    ///<param name="durationHitlag"> A duração do Hitlag
    ///</summary>
    ///<author>Davi Fontes</author>
    public void ConsumeIf(Func<InputSystemMemento, bool> condition, Action<InputSystemMemento> beforeConsumption)
    {
        if (Current == null || !condition(Current))
            return;
        
        beforeConsumption(Current);
        Consume();
    }
}
