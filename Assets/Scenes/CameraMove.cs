using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Transform playerLocation;
    private Vector3 CameraOffset;
    public bool CamLock;
    //public float MinX;
    //public float MaxX;

    // Start is called before the first frame update
    void Start()
    {
        playerLocation = GameObject.FindGameObjectWithTag("Player").transform;
        CameraOffset = transform.position - playerLocation.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(!CamLock)
        {
            Vector3 newLocation = transform.position;
            newLocation.x = playerLocation.position.x + CameraOffset.x;

            transform.position = newLocation;
        }
    }
}
