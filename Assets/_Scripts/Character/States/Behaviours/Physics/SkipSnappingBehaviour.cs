public class SkipSnappingBehaviour : CharacterState
{
    public override void PhysicsProcess()
    {
        body.SkipSnappingFrame();
    }
}