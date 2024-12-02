using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToNextScreen : MonoBehaviour
{
    [SerializeField] private InputSystem input;
    [SerializeField] private bool requireEnoughPlayers;
    
    public void Update()
    {
        if (input.IsAttackJustPressed() && CanAdvance())
        {
            GoForward();
            return;
        }

        if (input.IsSpecialJustPressed())
        {
            GoBack();
            return;
        }
    }
    
    public bool CanAdvance() => !requireEnoughPlayers 
        || (MatchConfiguration.PlayersPrefabs.Count >= 2);

    public void GoTo(int index)
    {
        index = (index + SceneManager.sceneCount) % SceneManager.sceneCount;
        SceneManager.LoadScene(index);
    }
    
    public void GoForward() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    public void GoBack() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
}