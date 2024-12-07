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

        var hits = body
            .Cast(body.Velocity.normalized, body.Velocity.magnitude * Time.fixedDeltaTime * 1.25f);
        
        if (hits.Count > 0)
        {
            var hit = hits.First();
            Bounce(hit.normal);
        }
    }
}