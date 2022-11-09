using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    GameObject gameMusic;
    GameObject overMusic;
    Text winner;
    StateManager manager;
    // Start is called before the first frame update
    void Start()
    {
        overMusic = GameObject.Find("Game Over Music");
        overMusic.SetActive(false);
        gameMusic = GameObject.Find("Game Music");
        winner = GetComponent<Text>();
        manager = GameObject.FindObjectOfType<StateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        winner.text = "Congratulations player " + (manager.CurrentPlayerID + 1) + "! You are the winner!!!";
    }
}
