using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    // Start is called before the first frame update
 

    public bool plotSelected, nodeSelected;
    public int value, oldHotelsOwned, buildValue;
    public bool doneRolling = false, doneClicking = false, doneAnimating = false, doneOther = false, buildPermission = false, doneEntrance = true, buyingEntrance = false;

    public int[] playerMoney;
    public int CurrentPlayerID = 0;
    public int NumberOfPlayers = 2;
    public Plots[] plots;
    public int entranceCost = 400;

    GameObject info;
    GameObject gameMusic;
    GameObject overMusic;
    GameObject winner;
    int available;
    Vector3 buildSpot;
    Plots selectedPlot;
    PlayerMovement[] move;
    game_button button;
    ownerships ownershipText;
    public GameObject hotelModel;
    public Plots entryPlot;
    public Tile entryNode;

    void Start()
    {
        info = GameObject.Find("Info");
        ownershipText = GameObject.FindObjectOfType<ownerships>();
        playerMoney = new int[NumberOfPlayers];
        move = GameObject.FindObjectsOfType<PlayerMovement>();
        button = GameObject.FindObjectOfType<game_button>();
        doneEntrance = true;
        winner = GameObject.Find("Game Over Text");
        winner.SetActive(false);
        overMusic = GameObject.Find("Over Music");
        overMusic.SetActive(false);
        gameMusic = GameObject.Find("Game Music");
    }

    public void NewTurn() //a new turn means we are no longer done doing anything and we have to change the playerID
    {
        doneAnimating = false;
        doneRolling = false;
        doneClicking = false;
        doneOther = false;

        CurrentPlayerID = (CurrentPlayerID + 1) % NumberOfPlayers;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMoney[CurrentPlayerID] < 0)
        {
            GameOver();
        }
        if (doneRolling && doneClicking && doneAnimating && doneOther && doneEntrance) //if we are done with everything
        {
            //Debug.Log("turn done");
            //ownershipText.printText();
            NewTurn();
        }
    }

    public void BuildHotel() 
    {
        info.GetComponent<Text>().text = "Landed on build node";
        if (canBuild())
        {
            button.BuildHotelText.SetActive(true);
            button.GetBuildYes().SetActive(true);
            button.GetBuildNo().SetActive(true);
        }
        else
        {
            doneOther = true;
            doneEntrance = true;
            info.GetComponent<Text>().text = "No plots owned";
            Debug.Log("no plots owned"); 
        }
    }

    public void BuyPlot()
    {
        info.GetComponent<Text>().text = "Landed on buy node";
        //if player has money to buy plot 1 and plot 0 doesn't have hotels on it
        // or if there are 2 plots and player has money to buy plot 2 and plot 2 doesn't have hotels on it
        if ( canBuyPlot1() || (move[CurrentPlayerID].getCurrentTile().AdjacentPlots.Length > 1 && canBuyPlot2() ))
        {
            button.BuyPlotText.SetActive(true);
            button.GetPlotYes().SetActive(true);
            button.GetPlotNo().SetActive(true);
            //button.PlotPurchase(); //show prompt to buy plot
        }
        else
        {
            doneOther = true;
            info.GetComponent<Text>().text = "Not enough money to buy plot";
            Debug.Log("not enough money to buy plot");
        }
        
    }

    public void BuyPlotSelection()
    {
        button.BuyPlotText.SetActive(true);
        if (canBuyPlot1()) 
        {
            button.GetPlot1But().SetActive(true);
        }
        if (move[CurrentPlayerID].getCurrentTile().AdjacentPlots.Length > 1 && canBuyPlot2())
        {
            button.GetPlot2But().SetActive(true);
        }
        info.GetComponent<Text>().text = "Choose one or both plots then click 'no'. ";
    }

    public void BuildHotelSelection (Plots plot)
    {
        info.GetComponent<Text>().text = "Choose how many hotels you want to build.";
        for (int i = 0; i < plots.Length - 1; i++) //for all plots
        {
            button.BuildButtons[i].SetActive(false); //deactivate button
        }
        button.ChoosePlotText.SetActive(false);
        available = plot.Hotels.Length - plot.HotelsOwned;
        selectedPlot = plot;
        if (move[CurrentPlayerID].getCurrentTile().isFreeBuild)
        {
            available = 1;
        }
        button.HowManyText.SetActive(true);
        for (int i = 0; i < available; i++)
        {
            button.BuildAmountButtons[i].SetActive(true);
        }

    }

    public void FreeEntrance()
    {
        if (hasHotel())
        {
            info.GetComponent<Text>().text = "Choose a plot and a node.";
            buyingEntrance = true;
        }
        else
        {
            doneOther = true;
            info.GetComponent<Text>().text = "No Hotels owned.";
            Debug.Log("doesn't own any hotels!");
        }

    }
    public void FreeBuild()
    {
       
        ActivateBuildButtons();
        // doneOther = true;
        //  doneEntrance = true;
        info.GetComponent<Text>().text = "Build a free hotel.";
        Debug.Log("building a free hotel");
    }

    bool canBuyPlot1()
    {
        return (playerMoney[CurrentPlayerID] >= move[CurrentPlayerID].getCurrentTile().AdjacentPlots[0].PlotCost && //if player has enough money and
            move[CurrentPlayerID].getCurrentTile().AdjacentPlots[0].Owner == -1) || //plot owned by bank OR
            (move[CurrentPlayerID].getCurrentTile().AdjacentPlots[0].HotelsOwned == 0 && //there are no hotels and
            playerMoney[CurrentPlayerID] >= move[CurrentPlayerID].getCurrentTile().AdjacentPlots[0].PlotCost/2 && //player has more than half the cost of plot in money and
            move[CurrentPlayerID].getCurrentTile().AdjacentPlots[0].Owner == (CurrentPlayerID + 1) % NumberOfPlayers); //plot is owned by the other playerID than the one playing
    }

    bool canBuyPlot2()
    {
        return (playerMoney[CurrentPlayerID] >= move[CurrentPlayerID].getCurrentTile().AdjacentPlots[1].PlotCost && //same as above
            move[CurrentPlayerID].getCurrentTile().AdjacentPlots[1].Owner == -1) ||
            (move[CurrentPlayerID].getCurrentTile().AdjacentPlots[1].HotelsOwned == 0 &&
            playerMoney[CurrentPlayerID] >= move[CurrentPlayerID].getCurrentTile().AdjacentPlots[1].PlotCost / 2 &&
            move[CurrentPlayerID].getCurrentTile().AdjacentPlots[1].Owner == (CurrentPlayerID + 1) % NumberOfPlayers);
    }

    bool canBuild()
    {
        for (int i = 0; i < plots.Length-1; i++)
        {
            if (plots[i].Owner == CurrentPlayerID) //if player has at least 1 plot, ask to build hotel
            {
                return true;
            }
        }
        return false;
    }

    public void ActivateBuildButtons()
    {
        info.GetComponent<Text>().text = "Choose a plot";
        button.ChoosePlotText.SetActive(true);
        button.BuildButtons[button.BuildButtons.Length - 1].SetActive(true); //activate the last button which is always the "Done" button
        for (int i=0; i<plots.Length-1; i++) //for all plots
        {
            if (plots[i].Owner == CurrentPlayerID) //if owner is the current player
            {
                button.BuildButtons[i].SetActive(true); //activate button
            }
        }
    }

    public void BuildCost(int amount)
    {
        int preDiceCost = 0, finalCost = 0;
        Debug.Log("you want to build that many hotels: " + amount);
        for (int i = selectedPlot.HotelsOwned; i < selectedPlot.HotelsOwned + amount; i++)
        {
            preDiceCost += selectedPlot.HotelCost[i];
        }
        Debug.Log("pre roll cost is: " + preDiceCost);
        if (buildValue == 1)
        {
            info.GetComponent<Text>().text = "Building Denied.";
            Debug.Log("can't build");
            finalCost = 0;
            buildPermission = false;
        }
        else if(buildValue == 3)
        {
            info.GetComponent<Text>().text = "Double Cost.";
            Debug.Log("double cost");
            finalCost = preDiceCost * 2;
            buildPermission = true;
            Build(amount);
        }
        else if(buildValue == 5)
        {
            info.GetComponent<Text>().text = "Cost halved.";
            Debug.Log("half cost");
            finalCost = preDiceCost / 2;
            buildPermission = true;
            Build(amount);
        }
        else if(buildValue == 8)
        {
            info.GetComponent<Text>().text = "This one is free.";
            Debug.Log("free");
            finalCost = 0;
            buildPermission = true;
            Build(amount);
        }
        else if (buildValue != 7)
        {
            info.GetComponent<Text>().text = "Building granted.";
            Debug.Log("granted");
            finalCost = preDiceCost;
            Build(amount);
        }
        playerMoney[CurrentPlayerID] -= finalCost;
        doneOther = true;
        doneEntrance = true;
        button.BuildButtons[plots.Length].SetActive(false);

    }

    void Build(int amount)
    {
        GameObject hotel;
        oldHotelsOwned = selectedPlot.HotelsOwned;
        for (int i = oldHotelsOwned; i < oldHotelsOwned + amount; i++)
        {
            selectedPlot.HotelsOwned++;
            hotel = Instantiate(hotelModel, new Vector3(selectedPlot.Hotels[i].transform.position.x-1, selectedPlot.Hotels[i].transform.position.y, selectedPlot.Hotels[i].transform.position.z-1), Quaternion.identity);
            hotel.transform.parent = GameObject.Find(selectedPlot.name).transform;
            ownershipText.printText();
        }
    }

    public void BuyEntrance()
    {
        if (playerMoney[CurrentPlayerID] > entranceCost && hasHotel())
        {
            button.BuyEntranceText.SetActive(true);
            button.GetEntranceNo().SetActive(true);
            button.GetEntranceYes().SetActive(true);
        }

    }
    bool hasHotel()
    {
        foreach (Plots plot in plots)
        {
            if (plot.HotelsOwned != 0 && plot.Owner == CurrentPlayerID)
            {
                return true;
            }
        }
        return false;
    }

    void GameOver()
    {
        Debug.Log("game over. loser is player: " + (CurrentPlayerID + 1));
        winner.GetComponent<Text>().text = "Congratulations Player " + (((CurrentPlayerID + 1) % NumberOfPlayers) + 1) + "! You are the winner!!!";
        winner.SetActive(true);
        gameMusic.SetActive(false);
        overMusic.SetActive(true);
        playerMoney[CurrentPlayerID] = 0;
    }


}
