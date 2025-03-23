using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Cada uma das direções possíveis de serem usadas
/// </summary>
/// TODO: Mover essa parte para a pasta de inputs
public enum InputDirection
{
    Neutral,
    Up,
    Down,
    Backward,
    Forward,
    UpBackward,
    UpForward,
    DownBackward,
    DownForward,
    None
}

/// <summary>
/// Cada um dos botões possíveis de serem usados
/// </summary>
/// TODO: Mover essa parte para a pasta de inputs
public enum InputButton
{
    None,
    Attack,
    Special,
    Any
}

/// <summary>
/// Realiza a transição assim que o combo de direção e input é performado pelo jogador
/// </summary>
/// TODO: Modificar esse comportamento para checar por movesets completos ao invés de um unico combo
public class InputTransitionBehaviour : CharacterState
{
    [FormerlySerializedAs("useBuffer")]
    [Header("Buffer")] 
    [SerializeField] private bool _useBuffer = true;

    [FormerlySerializedAs("requiredDirection")]
    [Header("Direction")] 
    [SerializeField] private InputDirection _requiredDirection;

    [FormerlySerializedAs("expandedTriggerAngle")] [SerializeField] private bool _expandedTriggerAngle;
    [FormerlySerializedAs("requireTap")] [SerializeField] private bool _requireTap;

    [FormerlySerializedAs("requiredButton")] [Header("Button")] [SerializeField] private InputButton _requiredButton;

    [FormerlySerializedAs("next")] [Header("Transition")] [SerializeField]
    private CharacterState _next;

    private (bool, Vector2) _lastTriggerTest = (false, Vector2.zero);

    public override void Enter()
    {
        _lastTriggerTest = (CurrentInputTriggers(), Input.GetDirection().normalized);
    }

    public override void StateUpdate()
    {
        if (_useBuffer && InputBufferTriggers())
        {
            InputBuffer.Consume();
            Machine.TransitionTo(_next);

            _lastTriggerTest = (true, Input.GetDirection().normalized);
            return;
        }

        if (CurrentInputTriggers())
        {
            _lastTriggerTest = (true, Input.GetDirection().normalized);
            Machine.TransitionTo(_next);
            return;
        }

        _lastTriggerTest = (false, Input.GetDirection().normalized);
    }

    /// <summary>
    /// Observa se o input no frame atual faz parte do combo que causa transição
    /// </summary>
    /// <returns>se o input no frame atual faz parte do combo que causa transição</returns>
    private bool CurrentInputTriggers()
    {
        return DirectionTriggers(Input.GetDirection().normalized) && (!_requireTap || !_lastTriggerTest.Item1 ||
                                                                      Vector2.Dot(Input.GetDirection().normalized,
                                                                          _lastTriggerTest.Item2.normalized) >= 0.01f)
                                                                  && ButtonCombinationTriggers(
                                                                      Input.IsAttackJustPressed(),
                                                                      Input.IsSpecialJustPressed());
    }

    /// <summary>
    /// Observa se o conteúdo do buffer de input possui o combo que causa transição
    /// </summary>
    /// <returns>se o conteúdo do buffer de input possui o combo que causa transição</returns>
    private bool InputBufferTriggers()
    {
        return InputBuffer.Current is not null
               && DirectionTriggers(InputBuffer.Current.direction)
               && ButtonCombinationTriggers(InputBuffer.Current.isAttackJustPressed,
                   InputBuffer.Current.isSpecialJustPressed);
    }

    /// <summary>
    /// Indica se a combinação de botões faz parte do combo que gatilha causa a transição
    /// </summary>
    /// <param name="attack">se o botão de ataque está apertado</param>
    /// <param name="special">se o botão especial está apertado</param>
    /// <returns>Se o combo de botões faz parte do combo que causa transição</returns>
    private bool ButtonCombinationTriggers(bool attack, bool special)
    {
        return _requiredButton switch
        {
            InputButton.None => true,
            InputButton.Attack => attack,
            InputButton.Special => special,
            InputButton.Any => attack || special
        };
    }

    /// <summary>
    /// Indica se a direção passada faz parte do combo que gatilha causa a transição
    /// </summary>
    /// <param name="direction">Direção a ser checada</param>
    /// <returns>Se a direção faz parte do combo que causa transição</returns>
    private bool DirectionTriggers(Vector2 direction)
    {
        return _requiredDirection switch
        {
            InputDirection.None => true,
            InputDirection.Neutral => direction.sqrMagnitude < 0.001f,
            _ => Mathf.Abs(AngleOfDirection(_requiredDirection) - Mathf.Atan2(direction.y, direction.x)) <=
                 (_expandedTriggerAngle ? 0.9 : 0.45)
        };
    }

    /// <summary>
    /// Retorna, em radianos, o angulo que gera a direção no círculo trigonométrico
    /// </summary>
    /// <param name="direction">A direção a ser convertida</param>
    /// <returns>O angulo no circulo trigonométrico que gera a direção</returns>
    private float AngleOfDirection(InputDirection direction)
    {
        return Mathf.Deg2Rad * direction switch
        {
            InputDirection.Up => 90f,
            InputDirection.Down => -90f,
            InputDirection.Forward => 180f - 180f * IsLookingForward(),
            InputDirection.Backward => 180f * IsLookingForward(),
            InputDirection.UpForward => 135f - 90f * IsLookingForward(),
            InputDirection.UpBackward => 45f + 90f * IsLookingForward(),
            InputDirection.DownForward => -135f + 90f * IsLookingForward(),
            InputDirection.DownBackward => -45f - 90f * IsLookingForward(),
            _ => 0f
        };
    }

    /// <summary>
    /// Indica se o jogador está olhando para frente ou para trás
    /// </summary>
    /// <returns>0 se olha para a esquerda, 1 se olha para a direita</returns>
    private float IsLookingForward()
    {
        return (Mathf.Sign(transform.lossyScale.x) + 1) / 2;
    }
}