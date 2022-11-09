using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EntranceSelector : MonoBehaviour
{
    StateManager manager;
    PlayerMovement[] move;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindObjectOfType<StateManager>();
        move = GameObject.FindObjectsOfType<PlayerMovement>();
    }

    void Update()
    {
        if (manager.plotSelected && manager.nodeSelected)
        {
            if (manager.playerMoney[manager.CurrentPlayerID] < manager.entranceCost)
            {
                manager.plotSelected = false;
                manager.nodeSelected = false;
                manager.buyingEntrance = false;
                manager.doneEntrance = true;
            }
            Debug.Log("clicked both");
            manager.plotSelected = false;
            manager.nodeSelected = false;
            manager.buyingEntrance = false;
            manager.doneEntrance = true;
            manager.entryNode.hasEntrance = true;
            manager.entryNode.EntranceToPlotX = manager.entryPlot;
            if (!move[manager.CurrentPlayerID].getCurrentTile().isFreeEntrance)
            {
                manager.playerMoney[manager.CurrentPlayerID] -= manager.entranceCost;
            }
            Debug.Log("bought entrance on node " + manager.entryNode.name + " for plot " + manager.entryPlot.name);

        }
    }

    private void OnMouseUp()
    {
        if (!manager.buyingEntrance)
        {
            return;
        }
        if (this.gameObject.tag == "Plot")
        {
            if (manager.plotSelected)
            {
                return;
            }
            manager.entryPlot = GameObject.Find(this.gameObject.name).GetComponent<Plots>(); //will not work with local variables. this workaround will have to do
            if (manager.entryPlot.Owner != manager.CurrentPlayerID)
            {
                return;
            }
            Debug.Log("clicked plot");
            manager.plotSelected = true;


            
        }
        if (this.gameObject.tag == "Node")
        {
            if (!manager.plotSelected)
            {
                return;
            }
            if (manager.nodeSelected)
            {
                return;
            }
            manager.entryNode = GameObject.Find(this.gameObject.name).GetComponent<Tile>(); //will not work with local variables. this workaround will have to do.
                                                                                            //this particular variable becomes null for no reason
            if (manager.entryNode.hasEntrance)
            {
                return;
            }
               for (int i=0; i<manager.entryNode.AdjacentPlots.Length; i++)
            {
                if (manager.entryPlot.name == manager.entryNode.AdjacentPlots[i].name)
                {
                    Debug.Log("clicked node");
                    manager.nodeSelected = true;
                }
            }
        }


    }


}
