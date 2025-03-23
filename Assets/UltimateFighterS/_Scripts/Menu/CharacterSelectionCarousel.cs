using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
[RequireComponent(typeof(IdComponent))]
public class CharacterSelectionCarousel : Carousel
{
    [SerializeField] private Texture noCharacter;
    private IdComponent _idComponent;
    private RawImage _image;

    private void Awake()
    {
        _idComponent = GetComponent<IdComponent>();
        _image = GetComponent<RawImage>();

        CharacterRegistryLoader.OnLoadComplete.AddListener(OnLoadComplete);
    }

    private void OnLoadComplete()
    {
        Select(IndexFromMatchConfiguration());
    }

    protected override void OnSelected(int index)
    {
        Character selectedCharacter = index == 0 ? null : CharacterRegistry.Characters[index - 1];

        WriteToMatchConfiguration(selectedCharacter);
        UpdateUI(selectedCharacter);
    }

    public int IndexFromMatchConfiguration()
    {
        if (!MatchConfiguration.Characters.ContainsKey(_idComponent.id))
            return 0;

        return CharacterRegistry.Characters
            .FindIndex(character => character.prefab == MatchConfiguration.Characters[_idComponent.id].prefab) + 1;
    }

    public void WriteToMatchConfiguration(Character selectedCharacter)
    {
        if (selectedCharacter is null)
        {
            MatchConfiguration.Characters.Remove(_idComponent.id);
            MatchConfiguration.PlayerInputTypes.Remove(_idComponent.id);
            return;
        }

        MatchConfiguration.Characters[_idComponent.id] = selectedCharacter;
        MatchConfiguration.PlayerInputTypes[_idComponent.id] = InputType.Player;
    }

    public void UpdateUI(Character selectedCharacter)
    {
        _image.texture = selectedCharacter?.icon ?? noCharacter;
    }

    protected override int GetItemCount()
    {
        return CharacterRegistry.CharacterCount + 1;
    }
}