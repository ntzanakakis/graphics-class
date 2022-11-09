using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{

    StateManager manager;
    Text score;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindObjectOfType<StateManager>();
        score = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.name == "Player1Score")
        {
            score.text = "Player one: " + manager.playerMoney[0];
        }
        else
        {
            score.text = "Player two: " + manager.playerMoney[1];
        }
    }
}
