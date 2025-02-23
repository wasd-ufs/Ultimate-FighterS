using UnityEngine;

[RequireComponent(typeof(Timer))]
public class PlayDustParticlesBehaviour : CharacterState
{
    [SerializeField] private float speedThreshold;
    [SerializeField] private float speedPerCooldownSeconds = 10;
    
    [SerializeField] private Vector2 axis;
    [SerializeField] private CharacterBasis basis;

    private Timer cooldown;

    public void Awake()
    {
        cooldown = GetComponent<Timer>();
    }

    public override void PhysicsProcess()
    {
        var (forward, up) = GetBasis(basis);
        var finalAxis = forward * axis.x + up * axis.y;

        if (Mathf.Abs(body.GetSpeedOnAxis(finalAxis)) >= speedThreshold && cooldown.IsFinished())
        {
            DustParticles.Play();
            
            cooldown.waitTime = body.GetSpeedOnAxis(finalAxis) / speedPerCooldownSeconds;
            cooldown.Init();
        }
    }
}