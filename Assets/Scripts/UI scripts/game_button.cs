using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class game_button : MonoBehaviour
{
    ownerships ownershipText;
    public GameObject PlayCam1;
    public GameObject PlayCam2;
    public GameObject DiceCam;
    public GameObject RollResult;
    BuildDicescript buildRoller;
    Dicescript roller;
    StateManager manager;
    GameObject cameraButton;
    GameObject plot1Button;
    GameObject plot2Button;
    GameObject plotYes;
    GameObject plotNo;
    GameObject plotNoLeft;
    PlayerMovement[] move;
    public GameObject[] BuildButtons;
    public GameObject[] BuildAmountButtons;
    GameObject buildYes;
    GameObject buildNo;
    GameObject entranceYes;
    GameObject entranceNo;
    public GameObject BuyPlotText;
    public GameObject BuyOtherPlotText;
    public GameObject BuildHotelText;
    public GameObject HowManyText;
    public GameObject ChoosePlotText;
    public GameObject BuyEntranceText;
    GameObject info;

    bool cam1;
    public Sprite[] DiceImages;

    // Start is called before the first frame update
    void Start()
    {
        ownershipText = GameObject.FindObjectOfType<ownerships>();
        buildRoller = GameObject.FindObjectOfType<BuildDicescript>();
        cameraButton = GameObject.Find("Camera Change");
        plot1Button = GameObject.Find("Plot1");
        plot2Button = GameObject.Find("Plot2");
        plotYes = GameObject.Find("Yes");
        plotNo = GameObject.Find("No");
        buildYes = GameObject.Find("buildYes");
        buildNo = GameObject.Find("buildNo");
        plotNoLeft = GameObject.Find("No Left");
        entranceYes = GameObject.Find("entranceYes");
        entranceNo = GameObject.Find("entranceNo");
        BuyPlotText = GameObject.Find("Buy Plot?");
        BuyOtherPlotText = GameObject.Find("Buy other plot?");
        BuildHotelText = GameObject.Find("Build hotel?");
        HowManyText = GameObject.Find("How many?");
        ChoosePlotText = GameObject.Find("Choose Plot");
        BuyEntranceText = GameObject.Find("Buy Entrance?");
        BuyPlotText.SetActive(false);
        BuyOtherPlotText.SetActive(false);
        BuildHotelText.SetActive(false);
        HowManyText.SetActive(false);
        ChoosePlotText.SetActive(false);
        BuyEntranceText.SetActive(false);
        plot1Button.SetActive(false);
        plot2Button.SetActive(false);
        plotNoLeft.SetActive(false);
        plotYes.SetActive(false);
        plotNo.SetActive(false);
        buildYes.SetActive(false);
        buildNo.SetActive(false);
        entranceYes.SetActive(false);
        entranceNo.SetActive(false);
        foreach (GameObject buildButton in BuildButtons)
        {
            buildButton.SetActive(false);
        }
        foreach (GameObject buildAmount in BuildAmountButtons)
        {
            buildAmount.SetActive(false);
        }
        cam1 = true; //is main cam active?
        PlayCam1.SetActive(true); //main cam
        PlayCam2.SetActive(false); //secondary cam
        DiceCam.SetActive(false); //dice cam
        manager = GameObject.FindObjectOfType<StateManager>();
        RollResult.transform.GetChild(0).GetComponent<Image>().sprite = DiceImages[manager.value - 1];
        roller = GameObject.FindObjectOfType<Dicescript>();
        move = GameObject.FindObjectsOfType<PlayerMovement>();
        manager.doneRolling = false;
        info = GameObject.Find("Info");
    }

    public void Exit_Button()
    {
        //go back to main menu
        UnityEngine.SceneManagement.SceneManager.LoadScene("New Scene");
    }

    public void Dice_Roll() 
    {

        if (manager.doneRolling)
        {
            return;
        }
        cameraButton.SetActive(false); //remove camera change button
        manager.doneRolling = false; 
        PlayCam1.SetActive(false); //close main cam
        PlayCam2.SetActive(false); //and secondary cam
        DiceCam.SetActive(true); //open dice cam
        roller.Roll_Dice(); //roll dice
        //manager.old_value = manager.value; //save old value (not sure i still need this)
        StartCoroutine(Corout()); //start routine that waits for dice to land


    }

    IEnumerator Corout()
    {
        while (DiceCam.activeSelf)
        {
            yield return null; //if dicecam is active then dice hasn't landed yet

        }
        RollResult.transform.GetChild(0).GetComponent<Image>().sprite = DiceImages[manager.value-1]; //result image
        manager.doneRolling = true; //we are done rolling the physical dice
        cameraButton.SetActive(true); //we can change camera now
    }

    public void ChangeCamera()
    {
        if (cam1) //if we are at the main cam
        {
            //Debug.Log("plot 0 name" + this.move.getCurrentTile().AdjacentPlots[0].name); //debugging code, discard
            PlayCam1.SetActive(false); //turn off main and go to secondary
            PlayCam2.SetActive(true);
            cam1 = false; //main cam no longer active
        }
        else
        {
            PlayCam1.SetActive(true); 
            PlayCam2.SetActive(false);
            cam1 = true;
        }
    }

    public bool Get_cam1()
    {
        return cam1;
    }

    public void clickedOnPlot1Button()
    {
        //manager.doneOther = true; //if plot button has been clicked, disable buttons and flag that we are done
        Debug.Log("you bought: " + move[manager.CurrentPlayerID].getCurrentTile().AdjacentPlots[0].name);
        if (move[manager.CurrentPlayerID].getCurrentTile().AdjacentPlots[0].Owner == -1)
        {
            if (manager.playerMoney[manager.CurrentPlayerID] - move[manager.CurrentPlayerID].getCurrentTile().AdjacentPlots[0].PlotCost < 0)//prevents bug that causes a game over if the player
                                                                                                                                            //passes over a buy entrance point and tries to buy a plot
                                                                                                                                            //at the same time while not having enough money for both
            {
                plot1Button.SetActive(false);
                plotNoLeft.SetActive(true);
                plotNo.SetActive(false);
                return;
            }
            manager.playerMoney[manager.CurrentPlayerID] -= move[manager.CurrentPlayerID].getCurrentTile().AdjacentPlots[0].PlotCost;
        }
        else
        {
            if (manager.playerMoney[manager.CurrentPlayerID] - (move[manager.CurrentPlayerID].getCurrentTile().AdjacentPlots[0].PlotCost) / 2 < 0) //same as above but for an already owned plot
            {
                plot1Button.SetActive(false);
                plotNoLeft.SetActive(true);
                plotNo.SetActive(false);
                return;
            }
            manager.playerMoney[manager.CurrentPlayerID] -= move[manager.CurrentPlayerID].getCurrentTile().AdjacentPlots[0].PlotCost / 2;
        }
        move[manager.CurrentPlayerID].getCurrentTile().AdjacentPlots[0].Owner = manager.CurrentPlayerID;
        plot1Button.SetActive(false);
        plotNoLeft.SetActive(true);
        plotNo.SetActive(false);
        ownershipText.printText();
    }  
    
    public void clickedOnPlot2Button()
    {
        //manager.doneOther = true; //if plot button has been clicked, disable buttons and flag that we are done
        Debug.Log("you bought: " + move[manager.CurrentPlayerID].getCurrentTile().AdjacentPlots[1].name);
        if (move[manager.CurrentPlayerID].getCurrentTile().AdjacentPlots[1].Owner == -1)
        {
            if (manager.playerMoney[manager.CurrentPlayerID] - move[manager.CurrentPlayerID].getCurrentTile().AdjacentPlots[1].PlotCost < 0)
            {
                plot2Button.SetActive(false);
                plotNo.SetActive(true);
                plotNoLeft.SetActive(false);
                return;
            }
            manager.playerMoney[manager.CurrentPlayerID] -= move[manager.CurrentPlayerID].getCurrentTile().AdjacentPlots[1].PlotCost;
        }
        else
        {
            if (manager.playerMoney[manager.CurrentPlayerID] - (move[manager.CurrentPlayerID].getCurrentTile().AdjacentPlots[1].PlotCost) / 2 < 0)
            {
                plot2Button.SetActive(false);
                plotNo.SetActive(true);
                plotNoLeft.SetActive(false);
                return;
            }
            manager.playerMoney[manager.CurrentPlayerID] -= move[manager.CurrentPlayerID].getCurrentTile().AdjacentPlots[1].PlotCost / 2;
        }
        move[manager.CurrentPlayerID].getCurrentTile().AdjacentPlots[1].Owner = manager.CurrentPlayerID;
        plot2Button.SetActive(false);
        plotNo.SetActive(true);
        plotNoLeft.SetActive(false);
        ownershipText.printText();
    }

    public void YesNoButton()
    {
        if (EventSystem.current.currentSelectedGameObject.name == "Yes")
        {
            BuyPlotText.SetActive(false);
            plotYes.SetActive(false);
            plotNo.SetActive(false);
            manager.BuyPlotSelection();
        }
        else
        {
            BuyPlotText.SetActive(false);
            plotYes.SetActive(false);
            plotNo.SetActive(false);
            plotNoLeft.SetActive(false);
            plot1Button.SetActive(false);
            plot2Button.SetActive(false);
            manager.doneOther = true;

        }
    }

    public void BuildYesNo()
    {
        if (EventSystem.current.currentSelectedGameObject.name == "buildYes")
        {
            BuildHotelText.SetActive(false);
            buildYes.SetActive(false);
            buildNo.SetActive(false);
            manager.ActivateBuildButtons();
        }
        else
        {
            BuildHotelText.SetActive(false);
            buildYes.SetActive(false);
            buildNo.SetActive(false);
            manager.doneOther = true;
        }
    }

    public void buyEntranceYesNo()
    {
        if (EventSystem.current.currentSelectedGameObject.name == "entranceYes")
        {
            BuyEntranceText.SetActive(false);
            entranceYes.SetActive(false);
            entranceNo.SetActive(false);
            manager.buyingEntrance = true;
            manager.doneEntrance = false;
        }
    }

    public void BuildReefResort()
    {
        manager.BuildHotelSelection(manager.plots[0]);
    }    
    
    public void BuildDragonGate()
    {
        manager.BuildHotelSelection(manager.plots[1]);
    }
    public void BuildReine()
    {
        manager.BuildHotelSelection(manager.plots[2]);
    }
    public void BuildUptown()
    {
        manager.BuildHotelSelection(manager.plots[4]);
    }
    public void BuildArtika()
    {
        manager.BuildHotelSelection(manager.plots[3]);
    }
    public void BuildCoconutClub()
    {
        manager.BuildHotelSelection(manager.plots[5]);
    }
    public void BuildAlWalid()
    {
        manager.BuildHotelSelection(manager.plots[6]);
    }
    public void BuildZebra()
    {
        manager.BuildHotelSelection(manager.plots[7]);
    }

    public void BuildDone()
    {
        foreach (GameObject buildButton in BuildButtons)
        {
            buildButton.SetActive(false);
        }
        manager.doneOther = true;

    }
    public void BuildAmount()
    {
        HowManyText.SetActive(false);
        foreach (GameObject amount in BuildAmountButtons)
        {
            amount.SetActive(false);
        }
        if (move[manager.CurrentPlayerID].getCurrentTile().isFreeBuild)
        {
            manager.buildValue = 8; //free build
            manager.BuildCost(int.Parse(EventSystem.current.currentSelectedGameObject.name)); //int.Parse converts string to int.
                                                                                              //so we convert the name of the button (which is a number) to an int and pass it on to the manager
        }
        else
        {
            cameraButton.SetActive(false); //remove camera change button
            PlayCam1.SetActive(false); //close main cam
            PlayCam2.SetActive(false); //and secondary cam
            DiceCam.SetActive(true); //open dice cam
            buildRoller.Roll_Dice(); //roll dice
            StartCoroutine(Corout2()); //start routine that waits for dice to land 
        } 
         
        
                                                                                 
    }

    IEnumerator Corout2()
    {
        while (DiceCam.activeSelf)
        {
            yield return null; //if dicecam is active then dice hasn't landed yet

        }
        cameraButton.SetActive(true); //we can change camera now
        manager.BuildCost(int.Parse(EventSystem.current.currentSelectedGameObject.name)); //int.Parse converts string to int.
                                                                                      //so we convert the name of the button (which is a number) to an int and pass it on to the manager
    }

    public void musicEnable()
    {
        if (AudioListener.volume == 1)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }
    }



    public GameObject GetPlotYes()
    {
        return plotYes;
    }

    public GameObject GetPlotNo()
    {
        return plotNo;
    }

    public GameObject GetPlot1But()
    {
        return plot1Button;
    }

    public GameObject GetPlot2But()
    {
        return plot2Button;
    }

    public GameObject GetBuildYes()
    {
        return buildYes;
    }

    public GameObject GetBuildNo()
    {
        return buildNo;
    }

    public GameObject GetEntranceYes()
    {
        return entranceYes;
    }

    public GameObject GetEntranceNo()
    {
        return entranceNo;
    }
}

    