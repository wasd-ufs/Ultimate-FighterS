using System.Collections.Generic;
using UnityEngine;

public class PlayerInPortConfigurator : MatchConfigurator
{
    [SerializeField] public int port;
    [SerializeField] public GameObject[] prefabs;
    private Character _selectedCharacter;

    public void SelectCharacter()
    {
        Debug.Log("Port " + port + " selected " + _selectedCharacter.name);
    }

    public void RemoveCharacter()
    {
        _selectedCharacter = null;
        Debug.Log("Port " + port + " removed");
    }

    public override void Configure()
    {
        if (_selectedCharacter == null)
        {
            MatchConfiguration.Characters.Remove(port);
            MatchConfiguration.PlayerInputTypes.Remove(port);
            return;
        }

        MatchConfiguration.Characters[port] = _selectedCharacter;
        MatchConfiguration.PlayerInputTypes[port] =
            MatchConfiguration.PlayerInputTypes.GetValueOrDefault(port, InputType.Player);
    }
}