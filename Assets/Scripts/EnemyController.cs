using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator AIAnime;
    private Transform trackPlayer;

    public float EnemyMoveSpeed;
    public float EnemyStopDistance;

    public float AttackDelay;
    public float AttackDamage;
    private float AttackTimer = 0f;
    private bool JustAttacked;

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

        float movementSpeed = 0f;

        if(trackPlayer.position.x - EnemyStopDistance > transform.position.x)
        {
            horizontalMovement = 1f;
            movementSpeed = 0.5f;
            GetComponentInChildren<SpriteRenderer>().flipX = false;
        }
        else if(trackPlayer.position.x + EnemyStopDistance < transform.position.x)
        {
            horizontalMovement = -1f;
            movementSpeed = 0.5f;
            GetComponentInChildren<SpriteRenderer>().flipX = true;
        }

        if (trackPlayer.position.z - EnemyStopDistance > transform.position.z)
        {
            verticalMovement = 1f;
            movementSpeed = 0.5f;
        }
        else if (trackPlayer.position.z + EnemyStopDistance < transform.position.z)
        {
            verticalMovement = -1f;
            movementSpeed = 0.5f;
        }

        AIAnime.SetFloat("Speed", movementSpeed);

        transform.position += transform.right * horizontalMovement * EnemyMoveSpeed * Time.deltaTime;
        transform.position += transform.forward * verticalMovement * EnemyMoveSpeed * Time.deltaTime;

        if(JustAttacked)
        {
            AttackTimer += Time.deltaTime;
            if(AttackTimer > AttackDelay)
            {
                AttackTimer = 0f;
                JustAttacked = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        switch(other.tag)
        {
            case "Player":
                if(!JustAttacked)
                {
                    AIAnime.SetTrigger("Punching");
                    JustAttacked = true;
                }
                break;
        }
    }
}
