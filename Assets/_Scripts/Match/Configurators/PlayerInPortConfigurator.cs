using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInPortConfigurator : MatchConfigurator
{
    [SerializeField] public int port;
    [SerializeField] public GameObject[] prefabs;
    private GameObject selectedCharacter;

    public void SelectCharacter()
    {
        Debug.Log("Port "+port+" selected "+selectedCharacter.name);
    }

    public void RemoveCharacter()
    {
        selectedCharacter = null;
        Debug.Log("Port "+port+" removed");
    }

    public override void Configure()
    {
        if (selectedCharacter == null)
        {
            MatchConfiguration.PlayersPrefabs.Remove(port);
            MatchConfiguration.PlayerInputTypes.Remove(port);
            return;
        }

        MatchConfiguration.PlayersPrefabs[port] = selectedCharacter;
        MatchConfiguration.PlayerInputTypes[port] =
            MatchConfiguration.PlayerInputTypes.GetValueOrDefault(port, InputType.Player);
    }
}
