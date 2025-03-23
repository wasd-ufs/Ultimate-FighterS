using System;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Executa os métodos do comportamento subordinado ao Comportamento Condicional se, e somente se, o atributo publico run for true
/// </summary>
public class ConditionalBehaviour : CharacterState
{
    [FormerlySerializedAs("subordinate")] [SerializeField] private CharacterState _subordinate;
    [FormerlySerializedAs("run")] [SerializeField] private bool _run;

    /// <summary>
    /// Executa a ação action apenas se run for true
    /// Antes de executar, configura o estado subordinado, garantindo que os atributos do estado estejam sincronizados
    /// </summary>
    /// <param name="action">A ação a ser executada condicionalmente</param>
    private void TryRun(Action<CharacterState> action)
    {
        if (_run)
        {
            Configure(_subordinate);
            action.Invoke(_subordinate);
        }
    }

    /// <summary>
    /// Sincroniza os atributos do estado state com os valores atualmente nesse estado
    /// </summary>
    /// <param name="state">O estado a ser configurado</param>
    private void Configure(CharacterState state)
    {
        state.Machine = Machine;
        state.Body = Body;
        state.Input = Input;
        state.InputBuffer = InputBuffer;
        state.FlipPivotPoint = FlipPivotPoint;
        state.DustParticles = DustParticles;
    }

    public override void Enter()
    {
        TryRun(state => state.Enter());
    }

    public override void Exit()
    {
        TryRun(state => state.Exit());
    }

    public override void StateUpdate()
    {
        TryRun(state => state.StateUpdate());
    }

    public override void StateFixedUpdate()
    {
        TryRun(state => state.StateFixedUpdate());
    }

    public override void OnCeilingEnter(Vector2 normal)
    {
        TryRun(state => state.OnCeilingEnter(normal));
    }

    public override void OnCeilingExit(Vector2 normal)
    {
        TryRun(state => state.OnCeilingExit(normal));
    }

    public override void OnFloorEnter(Vector2 normal)
    {
        TryRun(state => state.OnFloorEnter(normal));
    }

    public override void OnFloorExit(Vector2 normal)
    {
        TryRun(state => state.OnFloorExit(normal));
    }

    public override void OnLeftWallEnter(Vector2 normal)
    {
        TryRun(state => state.OnLeftWallEnter(normal));
    }

    public override void OnLeftWallExit(Vector2 normal)
    {
        TryRun(state => state.OnLeftWallExit(normal));
    }

    public override void OnRightWallEnter(Vector2 normal)
    {
        TryRun(state => state.OnRightWallEnter(normal));
    }

    public override void OnRightWallExit(Vector2 normal)
    {
        TryRun(state => state.OnRightWallExit(normal));
    }
}