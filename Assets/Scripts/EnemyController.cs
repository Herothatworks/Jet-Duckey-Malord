using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator AIAnime;
    private Transform trackPlayer;

    public float EnemyMoveSpeed;
    public float EnemyStopDistance;

    // Start is called before the first frame update
    void Start()
    {
        AIAnime = GetComponent<Animator>();
        trackPlayer = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalMovement = 0f;
        float verticalMovement = 0f;

        if(trackPlayer.position.x - EnemyStopDistance > transform.position.x)
        {
            horizontalMovement = 1f;
        }
        else if(trackPlayer.position.x + EnemyStopDistance < transform.position.x)
        {
            horizontalMovement = -1f;
        }

        if (trackPlayer.position.z > transform.position.z)
        {
            verticalMovement = 1f;
        }
        else if (trackPlayer.position.z< transform.position.z)
        {
            verticalMovement = -1f;
        }

        transform.position += transform.right * horizontalMovement * EnemyMoveSpeed * Time.deltaTime;
        transform.position += transform.forward * verticalMovement * EnemyMoveSpeed * Time.deltaTime;
    }
}
