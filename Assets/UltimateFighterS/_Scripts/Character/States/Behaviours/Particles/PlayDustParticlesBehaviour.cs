using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Roda a animação de partículas quando a velocidade num eixo excede um limite e o timer de cooldown estiver livre.
/// </summary>
[RequireComponent(typeof(Timer))]
public class PlayDustParticlesBehaviour : CharacterState
{
    [FormerlySerializedAs("speedThreshold")] [SerializeField] private float _speedThreshold;
    [FormerlySerializedAs("speedPerCooldownSeconds")] [SerializeField] private float _speedPerCooldownSeconds = 10;

    [FormerlySerializedAs("axis")] [SerializeField] private Vector2 _axis;
    // TODO: Encapsular a ideia de base numa classe ou record próprio
    [FormerlySerializedAs("basis")] [SerializeField] private CharacterBasis _basis;

    private Timer _cooldown;

    public void Awake()
    {
        _cooldown = GetComponent<Timer>();
    }

    public override void StateFixedUpdate()
    {
        (Vector2 forward, Vector2 up) = GetBasis(_basis);
        Vector2 finalAxis = forward * _axis.x + up * _axis.y;

        if (Mathf.Abs(Body.GetSpeedOnAxis(finalAxis)) >= _speedThreshold && _cooldown.IsFinished())
        {
            DustParticles.Play();

            _cooldown.waitTime = Body.GetSpeedOnAxis(finalAxis) / _speedPerCooldownSeconds;
            _cooldown.Init();
        }
    }
}