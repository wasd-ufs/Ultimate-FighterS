using UnityEngine;

public enum ButtonType
{
    Attack,
    Special,
    Any
}

public class ButtonSceneTransition : MonoBehaviour
{
    [Header("Trigger")] [SerializeField] private ButtonType buttonType = ButtonType.Any;

    [SerializeField] private InputSystem input;
    [SerializeField] private bool requireMinimumPlayerCount;

    [Header("Transition")] [SerializeField]
    private int nextSceneIndex;

    private bool _transitioning;

    public void Update()
    {
        if (IsButtonPressed() && CanTransition() && !_transitioning)
        {
            SceneChangerController.FadeOut(nextSceneIndex);
            _transitioning = true;
        }
    }

    private bool CanTransition()
    {
        return !requireMinimumPlayerCount
               || MatchConfiguration.Characters.Count >= 2;
    }

    private bool IsButtonPressed()
    {
        return buttonType switch
        {
            ButtonType.Attack => input.IsAttackJustPressed(),
            ButtonType.Special => input.IsSpecialJustPressed(),
            ButtonType.Any => input.IsAttackJustPressed() || input.IsSpecialJustPressed()
        };
    }
}