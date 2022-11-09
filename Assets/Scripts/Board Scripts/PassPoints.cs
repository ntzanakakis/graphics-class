using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassPoints : MonoBehaviour
{
    StateManager manager;
    public bool isMoney, isBuyEntrance;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindObjectOfType<StateManager>();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (this.isMoney)
        {
            manager.playerMoney[manager.CurrentPlayerID] += 2000;
        }
        else if(this.isBuyEntrance)
        {
            manager.BuyEntrance();
            Debug.Log("buy entrance!");
        }
    }
}
