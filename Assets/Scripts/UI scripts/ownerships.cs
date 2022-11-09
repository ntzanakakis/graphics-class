using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ownerships : MonoBehaviour
{
    StateManager manager;
    Plots[] plot;
    Text ownership;
    string[] lines;
    string temp = "";
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindObjectOfType<StateManager>();
        plot = GameObject.FindObjectsOfType<Plots>();
        ownership = GetComponent<Text>();
        lines = ownership.text.Split('\n');
        Debug.Log("that many lines" + lines.Length);
        printText();
    }

    // Update is called once per frame
    void Update()
    {
        //printText();
    }

    public void printText()
    {
        temp = ""; //reset text. otherwise the updated ownerships go underneath
        lines = ownership.text.Split('\n');
        for (int i = 0; i < manager.plots.Length; i++)
        {
            lines[i + 1] = manager.plots[i].name + " " + (manager.plots[i].Owner + 1) + " " + manager.plots[i].HotelsOwned;
        }
        foreach (string line in lines)
        {
            temp = temp + line + "\n";
        }
        ownership.text = temp;
    }
}
