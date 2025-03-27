public class SkipSnappingBehaviour : CharacterState
{
    public override void PhysicsProcess()
    {
        Body.SkipSnappingFrame();
    }
}