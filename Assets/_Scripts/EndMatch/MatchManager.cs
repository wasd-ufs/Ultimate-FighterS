using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{

    public void EndMatch() {
        Debug.Log("Partida Encerrada");
    }

    public void Respawn(GameObject gameObject) {
        GameObject clonedPlayer = Instantiate(gameObject);
        
        GameObject respawnPoint = GameObject.FindWithTag("respawn");

        if (respawnPoint) {
            clonedPlayer.transform.position = respawnPoint.transform.position;
            clonedPlayer.transform.rotation = respawnPoint.transform.rotation;

        }
    }
}
