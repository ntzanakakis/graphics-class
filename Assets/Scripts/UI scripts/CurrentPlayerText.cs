using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentPlayerText : MonoBehaviour
{
    StateManager manager;
    Text playerText;

    string[] numberWords = { "One", "Two" };

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindObjectOfType<StateManager>();
        playerText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        playerText.text = "Current Player: " + numberWords[manager.CurrentPlayerID];
    }
}
