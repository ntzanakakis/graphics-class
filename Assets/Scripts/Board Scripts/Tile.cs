using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{


    public Tile nextTile;
    public bool isBuy;
    public bool isBuild;
    public bool isFreeBuild;
    public bool isFreeEntrance;
    public Plots[] AdjacentPlots;
    public bool hasEntrance;
    public Plots EntranceToPlotX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
