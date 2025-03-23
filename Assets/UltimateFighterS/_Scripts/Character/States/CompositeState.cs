using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Um estado compatible com a máquina de estados do personagem que funciona como a junção de múltiplos estados.
/// Para cada evento do Estado do personagem, é executado, seguindo a sequência de adição, o evento equivalente de cada estado subordinado.
/// Casa ocorra uma transição em um estado subordinado, a execução é parada imediatamente  
/// </summary>
public class CompositeState : CharacterState
{
    [FormerlySerializedAs("foreignStates")]
    [Header("States outside of this object that should be included. Others Are Automatically included.")]
    [SerializeField]
    private List<CharacterState> _foreignStates = new();

    [FormerlySerializedAs("automaticallyIncludeChildren")] [SerializeField] private bool _automaticallyIncludeChildren;
    private bool _skip;

    // All States used by the composite
    private readonly HashSet<CharacterState> _states = new();

    private void Awake()
    {
        if (GetComponent<CharacterState>() != this)
            Destroy(this);

        LoadStates();
    }

    /// <summary>
    /// Recria a lista de estados subordinados que estão na composição
    /// </summary>
    private void LoadStates()
    {
        _states.Clear();

        List<CharacterState> local = GetComponents<CharacterState>().ToList();
        local.Remove(this);

        _states.AddRange(local);
        _states.AddRange(_foreignStates);

        if (_automaticallyIncludeChildren)
            for (int i = 0; i < transform.childCount; i++)
                if (transform.GetChild(i).TryGetComponent(out CharacterState child))
                    _states.Add(child);
    }

    /// <summary>
    /// Carrega os atributos de um estado com os valores do Estado Composto.
    /// Usado para passar os atributos da máquina para os estados subordinados
    /// </summary>
    /// <param name="state">O estado a ser configurado</param>
    private void Configure(CharacterState state)
    {
        state.Body = Body;
        state.Input = Input;
        state.Machine = Machine;
        state.InputBuffer = InputBuffer;
        state.FlipPivotPoint = FlipPivotPoint;
        state.DustParticles = DustParticles;
    }

    /// <summary>
    /// Executa uma Ação para cada estado participante da composição
    /// </summary>
    /// <param name="action">A ação a ser executada</param>
    private void ForEachState(Action<CharacterState> action)
    {
        foreach (CharacterState state in _states)
        {
            Configure(state);
            action(state);

            if (_skip)
                break;
        }
    }

    public override void Enter()
    {
        _skip = false;
        ForEachState(Configure);
        ForEachState(state => state.Enter());
    }

    public override void Exit()
    {
        ForEachState(state => state.Exit());
        _skip = true;
    }

    public override void StateUpdate()
    {
        ForEachState(state => state.StateUpdate());
    }

    public override void StateFixedUpdate()
    {
        ForEachState(state => state.StateFixedUpdate());
    }

    public override void OnCeilingEnter(Vector2 normal)
    {
        ForEachState(state => state.OnCeilingEnter(normal));
    }

    public override void OnCeilingExit(Vector2 normal)
    {
        ForEachState(state => state.OnCeilingExit(normal));
    }

    public override void OnFloorEnter(Vector2 normal)
    {
        ForEachState(state => state.OnFloorEnter(normal));
    }

    public override void OnFloorExit(Vector2 normal)
    {
        ForEachState(state => state.OnFloorExit(normal));
    }

    public override void OnLeftWallEnter(Vector2 normal)
    {
        ForEachState(state => state.OnLeftWallEnter(normal));
    }

    public override void OnLeftWallExit(Vector2 normal)
    {
        ForEachState(state => state.OnLeftWallExit(normal));
    }

    public override void OnRightWallEnter(Vector2 normal)
    {
        ForEachState(state => state.OnRightWallEnter(normal));
    }

    public override void OnRightWallExit(Vector2 normal)
    {
        ForEachState(state => state.OnRightWallExit(normal));
    }
}