using UnityEngine;

public class PortInputTypeConfigurator : MatchConfigurator
{
    [SerializeField] private int port;
    [SerializeField] private InputType inputType;
    
    public override void Configure()
    {
        if (!MatchConfiguration.PlayersPrefabs.ContainsKey(port))
            return;
        
        MatchConfiguration.PlayerInputTypes[port] = inputType;
    }
}