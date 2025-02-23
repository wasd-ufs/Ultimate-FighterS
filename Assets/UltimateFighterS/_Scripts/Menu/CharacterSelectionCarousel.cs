using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(RawImage))]
[RequireComponent(typeof(IdComponent))]
public class CharacterSelectionCarousel : Carousel
{
    private IdComponent idComponent;
    private RawImage image;
    [SerializeField] private Texture noCharacter;

    private void Awake()
    {
        idComponent = GetComponent<IdComponent>();
        image = GetComponent<RawImage>();
        
        CharacterRegistryLoader.OnLoadComplete.AddListener(OnLoadComplete);
    }

    private void OnLoadComplete()
    {
        Select(IndexFromMatchConfiguration());
    }

    protected override void OnSelected(int index)
    {
        var selectedCharacter = index == 0 ? null : CharacterRegistry.Characters[index - 1];
        
        WriteToMatchConfiguration(selectedCharacter);
        UpdateUI(selectedCharacter);
    }

    public int IndexFromMatchConfiguration()
    {
        if (!MatchConfiguration.Characters.ContainsKey(idComponent.id))
            return 0;
        
        return CharacterRegistry.Characters
            .FindIndex(character => character.prefab == MatchConfiguration.Characters[idComponent.id].prefab) + 1;
    }

    public void WriteToMatchConfiguration(Character selectedCharacter)
    {
        if (selectedCharacter is null)
        {
            MatchConfiguration.Characters.Remove(idComponent.id);
            MatchConfiguration.PlayerInputTypes.Remove(idComponent.id);
            return;
        }

        MatchConfiguration.Characters[idComponent.id] = selectedCharacter;
        MatchConfiguration.PlayerInputTypes[idComponent.id] = InputType.Player;
    }

    public void UpdateUI(Character selectedCharacter)
    {
        image.texture = selectedCharacter?.icon ?? noCharacter;
    }

    protected override int GetItemCount() => CharacterRegistry.CharacterCount + 1;
}
