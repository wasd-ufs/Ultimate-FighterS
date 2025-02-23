using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class EndMatchMenu : MonoBehaviour
{

    public Button Rematch;
    public Button MenuSelectPlayer;
    public Button Back;
    public TextMeshProUGUI PlayerWinner;
    private bool Player1Winner;
    public Image WinnerDisplayImage;
    private static Sprite WinnerImage;

    private void Awake()
    {
        Rematch.onClick.AddListener(RematchAction);
        MenuSelectPlayer.onClick.AddListener(MenuSelectPlayerAction);
        Back.onClick.AddListener(BackAction);
        SetPlayerWinner();
        SetWinnerImage();
    }

    void SetPlayerWinner() {
        if (Player1Winner) {
            PlayerWinner.text = "PLAYER 1 WINNER";
        }
        else 
         PlayerWinner.text = "PLAYER 2 WINNER";
    }

    void SetWinnerImage() {
        if (WinnerImage != null) {
            WinnerDisplayImage.sprite = WinnerImage;
        }
    }

    void RematchAction()
    {
        SceneManager.LoadScene("Cena da Tela de Luta");
    }

     void MenuSelectPlayerAction()
    {
        SceneManager.LoadScene("Cena do Menu de Seleção de Personagem");
    }

     void BackAction()
    {
        SceneManager.LoadScene("Cena de tela Inicial");
    }
}
