using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class ShowCharacterAtRank : MonoBehaviour
{
    [SerializeField] private int rank;
    private RawImage _image;

    private void Awake()
    {
        _image = GetComponent<RawImage>();
    }

    private void Start()
    {
        Character character = GetCharacter(rank);
        if (character is null)
        {
            _image.color = new Color(1, 1, 1, 0);
            return;
        }

        _image.texture = character.icon;
    }

    private Character GetCharacter(int rank)
    {
        if (!MatchResult.Results.ContainsKey(rank))
            return null;

        return MatchConfiguration.Characters[MatchResult.Results[rank]];
    }
}