using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
[RequireComponent(typeof(IdComponent))]
public class CharacterSelectionCarousel : Carousel
{
    private IdComponent idComponent;
    private RawImage image;

    private void Awake()
    {
        idComponent = GetComponent<IdComponent>();
        image = GetComponent<RawImage>();
    }

    private void Start()
    {
        Select(IndexFromMatchConfiguration());
    }

    protected override void OnSelected(int index)
    {
        var selectedCharacter = CharacterRegistry.Characters[index];
        
        WriteToMatchConfiguration(selectedCharacter);
        UpdateUI(selectedCharacter);
    }

    public int IndexFromMatchConfiguration()
    {
        if (!MatchConfiguration.Characters.ContainsKey(idComponent.id))
            return 0;
        
        return CharacterRegistry.Characters
            .FindIndex(character => character.prefab == MatchConfiguration.Characters[idComponent.id].prefab);
    }

    public void WriteToMatchConfiguration(Character selectedCharacter)
    {
        
        if (selectedCharacter.prefab == null)
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
        image.texture = selectedCharacter.icon;
    }

    protected override int GetItemCount() => CharacterRegistry.CharacterCount;
}
