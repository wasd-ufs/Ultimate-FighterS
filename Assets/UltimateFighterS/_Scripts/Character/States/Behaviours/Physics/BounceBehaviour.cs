using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BounceBehaviour : CharacterState
{
    [Range(0f, 1f)] [SerializeField] public float reductionPerBounce = 0.5f;
    
    void Bounce(Vector2 normal)
    {
        if (body.Velocity.sqrMagnitude < 0.01f)
            return;
        
        Debug.Log($"original: {body.Velocity}");
        body.SetVelocity(VectorUtils.Reflected(body.Velocity, normal) * (1f - reductionPerBounce));
        body.UpdateCurrentContacts();
    }

    public override void Enter()
    {
        CheckAndBounce();   
    }

    public void CheckAndBounce()
    {
        if (body.IsOnFloor())
            Bounce(body.FloorNormal);
        
        if (body.IsOnLeftWall())
            Bounce(body.LeftWallNormal);
        
        if (body.IsOnRightWall())
            Bounce(body.RightWallNormal);
        
        if (body.IsOnCeiling())
            Bounce(body.CeilingNormal);
    }
    
    public override void PhysicsProcess()
    {
        if (body.Velocity.sqrMagnitude < 0.01f)
            return;

        var hits = Physics2D.RaycastAll(body.transform.position, body.Velocity.normalized, body.Velocity.magnitude * Time.fixedDeltaTime).ToList()
            .Where(hit => !hit.collider.gameObject.CompareTag("Player") && !hit.collider.CompareTag("Hitbox") && !hit.collider.CompareTag("Hurtbox") && !hit.collider.isTrigger)
            .Where(hit => hit.distance >= 0.01f)
            .ToList();
        
        if (hits.Count > 0)
        {
            var hit = hits.First();
            Bounce(hit.normal);
            Debug.Log($"Hit name: {hit.collider.name}");
        }
    }
}