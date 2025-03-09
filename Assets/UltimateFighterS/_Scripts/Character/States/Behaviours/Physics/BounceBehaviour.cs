using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BounceBehaviour : CharacterState
{
    [Range(0f, 1f)] [SerializeField] public float reductionPerBounce = 0.5f;
    
    void Bounce(Vector2 normal)
    {
        if (Body.Velocity.sqrMagnitude < 0.01f)
            return;
        
        Debug.Log($"original: {Body.Velocity}");
        Body.SetVelocity(VectorUtils.Reflected(Body.Velocity, normal) * (1f - reductionPerBounce));
        Body.UpdateCurrentContacts();
    }

    public override void Enter()
    {
        CheckAndBounce();   
    }

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
    
    public override void PhysicsProcess()
    {
        if (Body.Velocity.sqrMagnitude < 0.01f)
            return;

        var hits = Physics2D.RaycastAll(Body.transform.position, Body.Velocity.normalized, Body.Velocity.magnitude * Time.fixedDeltaTime).ToList()
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