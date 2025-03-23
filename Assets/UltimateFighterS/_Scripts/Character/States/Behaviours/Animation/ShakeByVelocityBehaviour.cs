using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
/// <summary>
/// Shakes character based on its velocity and position entering this state and a scale factor, changing its position every frame
/// Resets back to Entering position when exiting this state
/// </summary>
public class ShakeByVelocityBehaviour : CharacterState
{
    [FormerlySerializedAs("scaledForce")] [SerializeField] private float _scaledForce;

    private Vector3 _enteringPosition;
    private Vector2 _shakeDirection;
    private float _speedForce;

    public override void Enter()
    {
        _enteringPosition = Body.transform.localPosition;

        _shakeDirection = Body.Velocity;
        if (_shakeDirection.sqrMagnitude < 0.01f)
            _shakeDirection = Body.Right;

        _speedForce = Mathf.Sqrt(_shakeDirection.magnitude);
        _shakeDirection.Normalize();
    }

    public override void StateFixedUpdate()
    {
        Shake();
    }

    /// <summary>
    /// Repositions the character to a random radius centered around character's original position, in the opposite direction of the previous shake
    /// </summary>
    private void Shake()
    {
        Body.transform.localPosition =
            _enteringPosition + (Vector3)_shakeDirection * (_speedForce * _scaledForce * Random.Range(0.5f, 1f));
        _shakeDirection *= -1;
    }

    public override void Exit()
    {
        Body.transform.localPosition = _enteringPosition;
    }
}