using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EndMatchMenu : MonoBehaviour
{
    private static Sprite _winnerImage;

    [FormerlySerializedAs("Rematch")] public Button rematch;
    [FormerlySerializedAs("MenuSelectPlayer")] public Button menuSelectPlayer;
    [FormerlySerializedAs("Back")] public Button back;
    [FormerlySerializedAs("PlayerWinner")] public TextMeshProUGUI playerWinner;
    [FormerlySerializedAs("WinnerDisplayImage")] public Image winnerDisplayImage;
    private bool _player1Winner;

    private void Awake()
    {
        rematch.onClick.AddListener(RematchAction);
        menuSelectPlayer.onClick.AddListener(MenuSelectPlayerAction);
        back.onClick.AddListener(BackAction);
        SetPlayerWinner();
        SetWinnerImage();
    }

    private void SetPlayerWinner()
    {
        if (_player1Winner)
            playerWinner.text = "PLAYER 1 WINNER";
        else
            playerWinner.text = "PLAYER 2 WINNER";
    }

    private void SetWinnerImage()
    {
        if (_winnerImage != null) winnerDisplayImage.sprite = _winnerImage;
    }

    private void RematchAction()
    {
        SceneManager.LoadScene("Cena da Tela de Luta");
    }

    private void MenuSelectPlayerAction()
    {
        SceneManager.LoadScene("Cena do Menu de Seleção de Personagem");
    }

    private void BackAction()
    {
        SceneManager.LoadScene("Cena de tela Inicial");
    }
}