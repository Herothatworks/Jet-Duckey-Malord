//Do not modify these, they are just structures I will be using to make life a little easier.
//thank you for your time, and feel free to ask any questions you may have.
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MoveLimits
{
    public float Upper;
    public float Lower;
    public float LeftBound;
    public float RightBound;
}

public class ComboMoves
{
    public string ComboName;
    public List<KeyCode> MoveList;
    private int currentMove = 0;

    public float ComboKeyTimer = 0.3f;
    private float LastButtonPressed;

    public ComboMoves(List<KeyCode> movesToMakeCombo, float timer)
    {
        MoveList = new List<KeyCode>();

        foreach(KeyCode move in movesToMakeCombo)
        {
            MoveList.Add(move);
        }

        ComboKeyTimer = timer;
    }

    public bool CheckCombo()
    {
        if (Time.time > LastButtonPressed + ComboKeyTimer) currentMove = 0;
        {
            if (currentMove < MoveList.Count)
            {
                if (Input.GetKeyDown(MoveList[currentMove]))
                {
                    LastButtonPressed = Time.time;
                    ++currentMove;
                }

                if (currentMove >= MoveList.Count)
                {
                    currentMove = 0;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        return false;
    }
}
