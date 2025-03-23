using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Faz o corpo saltar ao entrar em contato com qualquer superfície quanto esse estado estiver em execução 
/// </summary>
public class BounceBehaviour : CharacterState
{
    [FormerlySerializedAs("reductionPerBounce")] [Range(0f, 1f)] [SerializeField] public float ReductionPerBounce = 0.5f;

    /// <summary>
    /// Altera a velocidade do corpo para saltar para fora da superfície
    /// </summary>
    /// <param name="normal">A normal da superfície a qual o corpo deve saltar fora</param>
    private void Bounce(Vector2 normal)
    {
        if (Body.Velocity.sqrMagnitude < 0.01f)
            return;

        Debug.Log($"original: {Body.Velocity}");
        Body.SetVelocity(VectorUtils.Reflected(Body.Velocity, normal) * (1f - ReductionPerBounce));
        Body.UpdateCurrentContacts();
    }

    public override void Enter()
    {
        CheckAndBounce();
    }

    /// <summary>
    /// Para cada um dos 4 tipos de superfície, observa se o corpo está em contato e o faz saltar caso positivo
    /// </summary>
    public void CheckAndBounce()
    {
        if (Body.IsOnFloor())
            Bounce(Body.FloorNormal);

        if (Body.IsOnLeftWall())
            Bounce(Body.LeftWallNormal);

        if (Body.IsOnRightWall())
            Bounce(Body.RightWallNormal);

        if (Body.IsOnCeiling())
            Bounce(Body.CeilingNormal);
    }

    public override void StateFixedUpdate()
    {
        if (Body.Velocity.sqrMagnitude < 0.01f)
            return;

        List<RaycastHit2D> hits = Physics2D
            .RaycastAll(Body.transform.position, Body.Velocity.normalized,
                Body.Velocity.magnitude * Time.fixedDeltaTime).ToList()
            .Where(hit => !hit.collider.gameObject.CompareTag("Player") && !hit.collider.CompareTag("Hitbox") &&
                          !hit.collider.CompareTag("Hurtbox") && !hit.collider.isTrigger)
            .Where(hit => hit.distance >= 0.01f)
            .ToList();

        if (hits.Count > 0)
        {
            RaycastHit2D hit = hits.First();
            Bounce(hit.normal);
            Debug.Log($"Hit name: {hit.collider.name}");
        }
    }
}