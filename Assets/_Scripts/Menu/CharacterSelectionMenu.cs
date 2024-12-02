using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerInputSystem))]
[RequireComponent(typeof(IdComponent))]
[RequireComponent(typeof(RawImage))]
public class CharacterSelectionMenu : MonoBehaviour
{
    private static readonly List<Character> registeredCharacters = new();
    public List<Character> charactersToGloballyRegister = new();
    
    private Character SelectedCharacter => registeredCharacters[selectedCharacterIndex];
    private int selectedCharacterIndex = 0;
    
    private InputSystem inputSystem;
    private IdComponent idComponent;
    private RawImage sprite;

    private int lastDirection = 0;
    
    private void Awake()
    {
        registeredCharacters.AddRange(charactersToGloballyRegister);
        
        inputSystem = GetComponent<PlayerInputSystem>();
        idComponent = GetComponent<IdComponent>();
        sprite = GetComponent<RawImage>();
    }

    private void Start()
    {
        ReadFromMatchConfiguration();
        Select(selectedCharacterIndex);
    }

    private void Update()
    {
        var input = Mathf.Clamp(
            inputSystem.GetDirection().x, 
            -1, 
            1
        );
        
        var direction = input > 0 ? 1 : input < 0 ? -1 : 0;
        
        if (direction != lastDirection)
            Select(selectedCharacterIndex + direction + registeredCharacters.Count);
        
        lastDirection = direction;
    }

    private void Select(int index)
    {
        selectedCharacterIndex = index % registeredCharacters.Count;
        
        UpdateUI();
        WriteToMatchConfiguration();
    }
    
    public void UpdateUI()
    {
        sprite.texture = SelectedCharacter.characterPreviewImage;
    }
    
    private void WriteToMatchConfiguration()
    {
        if (SelectedCharacter.prefab == null)
        {
            MatchConfiguration.PlayersPrefabs.Remove(idComponent.id);
            MatchConfiguration.PlayerInputTypes.Remove(idComponent.id);
            return;
        }

        MatchConfiguration.PlayersPrefabs[idComponent.id] = SelectedCharacter.prefab;
        MatchConfiguration.PlayerInputTypes[idComponent.id] =
            MatchConfiguration.PlayerInputTypes.GetValueOrDefault(idComponent.id, InputType.Player);
    }

    private void ReadFromMatchConfiguration()
    {
        if (!MatchConfiguration.PlayersPrefabs.ContainsKey(idComponent.id))
        {
            selectedCharacterIndex = 0;
            return;
        }

        selectedCharacterIndex =
            registeredCharacters.FindIndex(c => c.prefab == MatchConfiguration.PlayersPrefabs[idComponent.id]);
    }
}

[Serializable]
public class Character
{
    public Texture characterPreviewImage;
    public GameObject prefab;
}