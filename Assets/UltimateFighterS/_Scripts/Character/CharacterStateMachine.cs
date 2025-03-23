using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// A maquina de estados dos personagens
/// </summary>
[RequireComponent(typeof(CharacterBody2D))]
public class CharacterStateMachine : StateMachine<CharacterState>
{
    [FormerlySerializedAs("FlipPivotPoint")] [FormerlySerializedAs("flipPivotPoint")] [SerializeField] private Transform _flipPivotPoint;
    [FormerlySerializedAs("DustParticles")] [FormerlySerializedAs("dustParticles")] [SerializeField] private ParticleSystem _dustParticles;
    private CharacterBody2D _body;
    private InputSystem _input;
    private InputBuffer _inputBuffer;

    private void Awake()
    {
        _body = GetComponent<CharacterBody2D>();
        _input = GetComponent<InputSystem>();
        _inputBuffer = GetComponent<InputBuffer>();
    }

    private void Update()
    {
        Current?.StateUpdate();
    }

    private void FixedUpdate()
    {
        Current?.StateFixedUpdate();
    }
    
    protected override void OnStart()
    {
        _body.OnCeilingEnter.AddListener(OnCeilingEnter);
        _body.OnCeilingExit.AddListener(OnCeilingExit);
        _body.OnFloorEnter.AddListener(OnFloorEnter);
        _body.OnFloorExit.AddListener(OnFloorExit);
        _body.OnLeftWallEnter.AddListener(OnLeftWallEnter);
        _body.OnLeftWallExit.AddListener(OnLeftWallExit);
        _body.OnRightWallEnter.AddListener(OnRightWallEnter);
        _body.OnRightWallExit.AddListener(OnRightWallExit);
    }

    protected override void OnTransition()
    {
        if (Current is null)
            return;

        Current.Machine = this;
        Current.Body = _body;
        Current.Input = _input;
        Current.InputBuffer = _inputBuffer;
        Current.FlipPivotPoint = _flipPivotPoint;
        Current.DustParticles = _dustParticles;
    }

    private void OnCeilingEnter(Vector2 normal)
    {
        Current?.OnCeilingEnter(normal);
    }

    private void OnCeilingExit(Vector2 normal)
    {
        Current?.OnCeilingExit(normal);
    }

    private void OnFloorEnter(Vector2 normal)
    {
        Current?.OnFloorEnter(normal);
    }

    private void OnFloorExit(Vector2 normal)
    {
        Current?.OnFloorExit(normal);
    }

    private void OnLeftWallEnter(Vector2 normal)
    {
        Current?.OnLeftWallEnter(normal);
    }

    private void OnLeftWallExit(Vector2 normal)
    {
        Current?.OnLeftWallExit(normal);
    }

    private void OnRightWallEnter(Vector2 normal)
    {
        Current?.OnRightWallEnter(normal);
    }

    private void OnRightWallExit(Vector2 normal)
    {
        Current?.OnRightWallExit(normal);
    }
}