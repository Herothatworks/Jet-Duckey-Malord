using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public string nextLevel;

    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case "Player":
                SceneManager.LoadScene(nextLevel);
                break;
        }
    }
}
