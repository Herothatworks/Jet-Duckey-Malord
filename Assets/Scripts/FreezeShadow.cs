using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeShadow : MonoBehaviour
{
    private Vector3 lockY;
    // Start is called before the first frame update
    void Start()
    {
        lockY = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newLoc = transform.position;
        newLoc.y = lockY.y;
        transform.position = newLoc;

        
    }
}
