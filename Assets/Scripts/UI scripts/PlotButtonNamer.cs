using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlotButtonNamer : MonoBehaviour
{
    PlayerMovement[] move;
    Text butName;
    StateManager manager;

    // Start is called before the first frame update
    void Start()
    {
        move = GameObject.FindObjectsOfType<PlayerMovement>();
        butName = GetComponent<Text>();
        manager = GameObject.FindObjectOfType<StateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("button namer reports adjacent 0 is " + move.getCurrentTile().AdjacentPlots[0].name);
        if (this.gameObject.name == "Plot1")
        {
            butName.text = "Buy: " + move[manager.CurrentPlayerID].getCurrentTile().AdjacentPlots[0].name;
        }
        if (move[manager.CurrentPlayerID].getCurrentTile().AdjacentPlots.Length > 1 && this.gameObject.name == "Plot2")
        {
            butName.text = "Buy: " + move[manager.CurrentPlayerID].getCurrentTile().AdjacentPlots[1].name;
        }
    }
}
