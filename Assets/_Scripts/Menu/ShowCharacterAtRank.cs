using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class ShowCharacterAtRank : MonoBehaviour
{
    [SerializeField] private int rank;
    private RawImage image;

    private void Awake()
    {
        image = GetComponent<RawImage>();
    }
    
    private void Start()
    {
        var character = GetCharacter(rank);
        if (character is null)
            return;

        image.texture = character.icon;
    }

    private Character GetCharacter(int rank)
    {
        if (!MatchResult.Results.ContainsKey(rank))
            return null;

        return MatchConfiguration.Characters[MatchResult.Results[rank]];
    }
}