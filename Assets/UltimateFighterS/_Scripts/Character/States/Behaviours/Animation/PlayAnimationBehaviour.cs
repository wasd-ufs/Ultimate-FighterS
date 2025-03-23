using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Behaviour that plays an animation on entering this state
/// </summary>
[RequireComponent(typeof(Animator))]
public class PlayAnimationBehaviour : CharacterState
{
    private const string ANIMATION_NAME_NOT_SET = "<BOTE O NOME DA ANIMAÇÃO AQUI>";

    [FormerlySerializedAs("animationName")] [SerializeField] private string _animationName = ANIMATION_NAME_NOT_SET;
    [FormerlySerializedAs("resetIfAlreadyPlaying")] [SerializeField] private bool _resetIfAlreadyPlaying;
    private Animator _animator;


    /// TODO: Check for non existing animation name instead
    public void Awake()
    {
        _animator = GetComponent<Animator>();

        if (_animationName == ANIMATION_NAME_NOT_SET)
            Debug.LogError(
                $"Tu esqueceu de por o nome da animação no PlayAnimationBehaviour do teu movimento. Ass: O PlayAnimationBehaviour do {gameObject.name}");
    }

    public void Start()
    {
        gameObject.SetActive(false);
    }

    public override void Enter()
    {
        gameObject.SetActive(true);
        _animator.Play(_animationName, -1, 0f);
    }

    public override void Exit()
    {
        gameObject.SetActive(false);
    }
}