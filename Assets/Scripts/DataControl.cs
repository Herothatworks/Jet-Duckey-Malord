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
}

public class ComboMoves
{
    public string ComboName;
    public string[] MoveList;
    private int currentMove = 0;

    public float ComboKeyTimer = 0.3f;
    private float LastButtonPressed;

    public ComboMoves(string[] movesToMakeCombo, float timer)
    {
        MoveList = movesToMakeCombo;
        ComboKeyTimer = timer;
    }

    public bool CheckCombo()
    {
        if (Time.time > LastButtonPressed + ComboKeyTimer) currentMove = 0;
        {
            if (currentMove < MoveList.Length)
            {
                if ((MoveList[currentMove] == "down" && Input.GetKeyDown(KeyCode.S)) ||
                    (MoveList[currentMove] == "up" && Input.GetKeyDown(KeyCode.W)) ||
                    (MoveList[currentMove] == "left" && Input.GetKeyDown(KeyCode.A)) ||
                    (MoveList[currentMove] == "right" && Input.GetKeyDown(KeyCode.D)) ||
                    (MoveList[currentMove] == "punch" && Input.GetKeyDown(KeyCode.O)) ||
                    (MoveList[currentMove] == "kick" && Input.GetKeyDown(KeyCode.P)) ||
                    (MoveList[currentMove] == "grab" && Input.GetKeyDown(KeyCode.Semicolon)) ||
                    (MoveList[currentMove] == "block" && Input.GetKeyDown(KeyCode.L)) ||
                    (MoveList[currentMove] == "ability" && Input.GetKeyDown(KeyCode.K)))
                {
                    LastButtonPressed = Time.time;
                    ++currentMove;
                }

                if (currentMove >= MoveList.Length)
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
