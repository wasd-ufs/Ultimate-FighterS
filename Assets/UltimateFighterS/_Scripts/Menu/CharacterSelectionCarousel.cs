using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

///<summary>
/// Gerencia a sele��o de personagens em um carrossel de sele��o.
/// Atualiza a interface e sincroniza a sele��o com a configura��o da partida.
///</summary>
[RequireComponent(typeof(RawImage))]
[RequireComponent(typeof(IdComponent))]
public class CharacterSelectionCarousel : Carousel
{
    private IdComponent _idComponent;
    private RawImage _image;
    [SerializeField] private Texture _noCharacter;

    private void Awake()
    {
        _idComponent = GetComponent<IdComponent>();
        _image = GetComponent<RawImage>();
        
        CharacterRegistryLoader.OnLoadComplete.AddListener(OnLoadComplete);
    }

    ///<summary>
    /// Chamada quando o carregamento do registro de personagens � conclu�do.
    /// Seleciona o personagem correspondente � configura��o da partida.
    ///</summary>
    ///<author>Davi Fontes</author>
    private void OnLoadComplete()
    {
        Select(IndexFromMatchConfiguration());
    }

    ///<summary>
    /// Atualiza a sele��o do personagem e, em seguida, a configura��o da partida e a UI com base no personagem selecionado.
    ///</summary>
    ///<param name="index">�ndice do personagem selecionado.</param>
    ///<author>Davi Fontes</author>
    protected override void OnSelected(int index)
    {
        var selectedCharacter = index == 0 ? null : CharacterRegistry.Characters[index - 1];
        
        WriteToMatchConfiguration(selectedCharacter);
        UpdateUI(selectedCharacter);
    }

    ///<summary>
    /// Retorna o �ndice do personagem atualmente configurado na partida.
    ///</summary>
    ///<returns>�ndice do personagem ou 0 se nenhum personagem estiver configurado.</returns>
    ///<author>Davi Fontes</author>
    public int IndexFromMatchConfiguration()
    {
        if (!MatchConfiguration.Characters.ContainsKey(_idComponent.id))
            return 0;
        
        return CharacterRegistry.Characters
            .FindIndex(character => character.prefab == MatchConfiguration.Characters[_idComponent.id].prefab) + 1;
    }

    ///<summary>
    /// Atualiza a configura��o da partida com o personagem selecionado.
    ///</summary>
    ///<param name="selectedCharacter">Personagem selecionado ou null para remover.</param>
    ///<author>Davi Fontes</author>
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

    ///<summary>
    /// Atualiza a interface de usu�rio com o �cone do personagem selecionado.
    ///</summary>
    ///<param name="selectedCharacter">Personagem selecionado ou null para exibir a textura padr�o.</param>
    ///<author>Davi Fontes</author>
    public void UpdateUI(Character selectedCharacter)
    {
        _image.texture = selectedCharacter?.icon ?? _noCharacter;
    }

    ///<summary>
    /// Retorna a quantidade total de itens no carrossel.
    ///</summary>
    protected override int GetItemCount() => CharacterRegistry.CharacterCount + 1;
}
