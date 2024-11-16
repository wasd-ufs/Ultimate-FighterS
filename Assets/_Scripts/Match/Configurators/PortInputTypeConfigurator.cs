using UnityEngine;

public class PortInputTypeConfigurator : MatchConfigurator
{
    [SerializeField] private int port;
    [SerializeField] private InputType inputType;

    public void SwitchInput()
    {
        if (inputType == InputType.Player)
        {
            inputType = InputType.NoInput;
            Debug.Log("Port " + port + " " + inputType.ToString());
        }
        else
        {
            inputType = InputType.Player;
            Debug.Log("Port " + port + " " + inputType.ToString());
        }
    }

    public override void Configure()
    {
        if (!MatchConfiguration.PlayersPrefabs.ContainsKey(port))
            return;
        
        MatchConfiguration.PlayerInputTypes[port] = inputType;
    }
}