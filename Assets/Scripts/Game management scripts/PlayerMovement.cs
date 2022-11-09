using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    GameObject info;
    int spotsToMove;
    int moveQueueIndex;
    StateManager manager;
    public Tile StartingTile;
    Tile currentTile;
    Tile finalTile;
    Tile[] moveQueue;
    bool isAnimating = false;

    Vector3 targetPos;
    Vector3 velocity = Vector3.zero;
    float smoothTime = 0.1f;


    public int PlayerID;

    // Start is called before the first frame update
    void Start()
    {
        info = GameObject.Find("Info");
        manager = GameObject.FindObjectOfType<StateManager>();
        currentTile = StartingTile; //at the start, the current tile is always the starting tile
        targetPos = this.transform.position; //same as above
    }

    // Update is called once per frame
    //animation is done tile by tile. instead of moving towards the final destination, it moves from tile to tile stopping very briefly at each tile and finally stops at the final destination
    void Update()
    {
        
        if (manager.doneRolling && !manager.doneClicking)
        {
            info.GetComponent<Text>().text = "Click on your pawn.";
        }
        if (!isAnimating) //if it isn't animating, we don't have to do anything
        {
            return;
        }
        if (Vector3.Distance(this.transform.position, targetPos) > 0.01f) //if it's not close to our target
        {
            this.transform.position = Vector3.SmoothDamp(this.transform.position, targetPos, ref velocity, smoothTime); //keep moving gradually
        }
        else
        {
            if (moveQueue != null && moveQueueIndex < moveQueue.Length)  //if we still haven't reached the final destination
            {
                SetNewTargetPosition(moveQueue[moveQueueIndex].transform.position); //add the next tile to move to
                moveQueueIndex++;
            }
            else //if we finally reached the final destination
            {
                this.isAnimating = false; //we are no longer animating
                manager.doneAnimating = true;
                if (currentTile.hasEntrance)
                {
                    if (currentTile.EntranceToPlotX.Owner != manager.CurrentPlayerID)
                    {
                        manager.playerMoney[manager.CurrentPlayerID] -= currentTile.EntranceToPlotX.PaymentsPerHotel[currentTile.EntranceToPlotX.HotelsOwned-1];
                    }
                }
                if (currentTile.isBuild) //if it's a build tile
                {
                    manager.BuildHotel();
                }
                else if (currentTile.isBuy) //if current tile is a buy plot tile
                {
                    manager.BuyPlot(); //call the manager to start the buy procedure
                }
                else if (currentTile.isFreeBuild)
                {
                    manager.FreeBuild();
                }
                else if (currentTile.isFreeEntrance)
                {
                    manager.FreeEntrance();
                }
                else                            //start node is only expected to pass through here
                {
                    manager.doneOther = true;
                }


            }
        }

    }

    void SetNewTargetPosition(Vector3 pos)
    {
        targetPos = pos;
        velocity = Vector3.zero;
    }

    private void OnMouseUp() //when a player is clicked
    {
        //Debug.Log("clicked");

        if (manager.CurrentPlayerID != PlayerID) //if it's not "our" turn, skip that click
        {
            return;
        }

        if (!manager.doneRolling) //if we haven't rolled, there's no reason to click on players
        {
            return;
        }

        if (manager.doneClicking) //if we have already moved
        {
            return;
        }

        spotsToMove = manager.value; //basically, the dice roll result
        moveQueue = new Tile[spotsToMove];
        finalTile = currentTile;

        for (int i = 0; i < spotsToMove; i++) //fills up the queue with all the tiles we'll go through
        {
            finalTile = finalTile.nextTile;
            moveQueue[i] = finalTile;
        }

        //this.transform.position = finalTile.transform.position; //this is old code. it teleports to the destination
        //SetNewTargetPosition(finalTile.transform.position);

        moveQueueIndex = 0; //no longer have tiles to move to
        currentTile = finalTile;
        manager.doneClicking = true; //we are done clicking and we made our queue
        isAnimating = true; //it's time to animate
    }

    public Tile getCurrentTile()
    {
        return currentTile;
    }
}
