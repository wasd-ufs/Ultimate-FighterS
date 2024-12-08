using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ButtonType
{
    Attack,
    Special,
    Any
}

public class ButtonSceneTransition : MonoBehaviour
{
    [Header("Trigger")]
    [SerializeField] private ButtonType buttonType = ButtonType.Any;
    [SerializeField] private InputSystem input;
    [SerializeField] private bool requireMinimumPlayerCount;
    
    [Header("Transition")]
    [SerializeField] private int nextSceneIndex;
    
    private bool transitioning = false;

    public void Start()
    {
        SceneChangerController.FadeIn();
    }

    public void Update()
    {
        if (IsButtonPressed() && CanTransition() && !transitioning)
        {
            SceneChangerController.FadeOut(nextSceneIndex);
            transitioning = true;
        }
    }

    private bool CanTransition() => !requireMinimumPlayerCount
                                   || MatchConfiguration.Characters.Count >= 2;

    private bool IsButtonPressed() => buttonType switch
    {
        ButtonType.Attack => input.IsAttackJustPressed(),
        ButtonType.Special => input.IsSpecialJustPressed(),
        ButtonType.Any => input.IsAttackJustPressed() || input.IsSpecialJustPressed()
    };
}