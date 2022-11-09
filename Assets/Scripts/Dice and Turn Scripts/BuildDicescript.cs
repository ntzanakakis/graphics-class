using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildDicescript : MonoBehaviour
{

    StateManager manager;
    public MeshRenderer PLS;
    Rigidbody rb;
    bool hasLanded;
    bool thrown;

    Vector3 initial_pos;

    int value;

    public DiceSides[] sides;
    game_button button;

    // Start is called before the first frame update
    void Start()
    {
        PLS.enabled = false;
        button = GameObject.FindObjectOfType<game_button>();
        manager = GameObject.FindObjectOfType<StateManager>();
        rb = GetComponent<Rigidbody>();
        initial_pos = transform.position; //save our initial position
        rb.useGravity = false; //disable gravity because we don't need it yet
        value = 7; //a value the dice will never reach, used for the placeholder image
        manager.buildValue = value; //update the manager
    }

    // Update is called once per frame
    void Update()
    {
        if (value != 7) //if we have a value
        {
            if (button.Get_cam1()) //if cam1 was active
            {
                button.PlayCam1.SetActive(true); //return to cam1
            }
            else
            {
                button.PlayCam2.SetActive(true); //otherwise, cam2
            }
            button.DiceCam.SetActive(false); //disable dicecam
            Reset(); //reset the dice to floating position
        }

        if (rb.IsSleeping() && !hasLanded && thrown) //if it has been thrown and it's sleeping (not moving), it has landed
        {
            hasLanded = true;
            rb.useGravity = false; //no longer need gravity
            Value_Check(); //what value do we have
        }
        else if (rb.IsSleeping() && hasLanded && value == 7) //if it has landed, check value
        {
            Value_Check();
            if (value == 7) //if we still have no value, it's stuck
            {
                Unstuck();
            }
        }
    }

    public void Roll_Dice()
    {
        if (!thrown && !hasLanded) //if it hasn't been thrown and it hasn't landed (so far) 
        {
            PLS.enabled = true;
            thrown = true;
            rb.useGravity = true;
            rb.AddTorque(Random.Range(0, 350), Random.Range(0, 350), Random.Range(0, 350)); //enable gravity while adding a random spin to it
        }
        else if (thrown && hasLanded) //if it has landed after being thrown, we're done, reset
        {
            PLS.enabled = false;
            Reset();
        }
    }

    void Unstuck() //if stuck
    {
        rb.useGravity = true; //re-enable gravity
        rb.AddTorque(Random.Range(0, 600), Random.Range(0, 600), Random.Range(0, 600)); //give it a spin
        rb.AddForce(Random.Range(0, 600), Random.Range(0, 600), Random.Range(0, 600)); //and a hard nudge
    }

    public void Reset() //take the dice back to original position and reset all flags
    {
        PLS.enabled = false;
        transform.position = initial_pos;
        thrown = false;
        hasLanded = false;
        rb.useGravity = false;
        value = 7;
    }

    void Value_Check()
    {
        foreach (DiceSides side in sides) //check all sides of the dice
        {
            if (side.OnGround())
            {
                value = side.sideValue;
                manager.buildValue = value; //our dice roll result
                //Debug.Log(value + " has been rolled.");
            }
        }
    }
}
