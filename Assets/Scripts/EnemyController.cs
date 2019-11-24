﻿using System.Collections;
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

    public float Hitpoints;
    public float currentHP;

    private bool WaveSpawn;

    // Start is called before the first frame update
    void Start()
    {
        AIAnime = GetComponent<Animator>();
        trackPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        currentHP = Hitpoints;
    }

    public void SetStats(int HP, int Dmg)
    {
        Hitpoints = HP;
        currentHP = Hitpoints;
        AttackDamage = Dmg;
    }

    public void TakeHit(float damageTaken)
    {
        currentHP -= damageTaken;

        AIAnime.SetTrigger("TakeDamage");
        JustAttacked = true;
        
        if (currentHP > Hitpoints) currentHP = Hitpoints;

        if (currentHP <= 0) gameObject.SetActive(false);
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

        if (trackPlayer.position.z - 0.01f > transform.position.z)
        {
            verticalMovement = 1f;
            movementSpeed = 0.5f;
        }
        else if (trackPlayer.position.z + 0.01f < transform.position.z)
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
                    other.GetComponent<PlayerControl>().TakeDamage(AttackDamage);
                    JustAttacked = true;
                }
                break;
        }
    }
}
