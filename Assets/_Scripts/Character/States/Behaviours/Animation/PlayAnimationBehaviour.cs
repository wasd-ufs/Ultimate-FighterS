using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayAnimationBehaviour : CharacterState
{
    const string ANIMATION_NAME_NOT_SET = "<BOTE O NOME DA ANIMAÇÃO AQUI>";
    
    [SerializeField] private string animationName = ANIMATION_NAME_NOT_SET;
    private Animator animator;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        
        if (animationName == ANIMATION_NAME_NOT_SET)
            Debug.LogError($"Tu esqueceu de por o nome da animação no PlayAnimationBehaviour do teu movimento. Ass: O PlayAnimationBehaviour do {gameObject.name}");
    }

    public void Start()
    {
        gameObject.SetActive(false);
    }

    public override void Enter()
    {
        gameObject.SetActive(true);
        animator.Play(animationName, -1, 0f);
    }

    public override void Exit()
    {
        gameObject.SetActive(false);
    }
}