using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveDebug : MonoBehaviour
{
    // Start is called before the first frame update
    public bool removeOnLoad;

    void Start()
    {
        if(removeOnLoad)
        {
            Destroy(gameObject);
        }
    }
}
