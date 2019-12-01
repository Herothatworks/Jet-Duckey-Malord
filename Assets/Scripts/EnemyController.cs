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

    public float Hitpoints;
    public float currentHP;

    private bool WaveSpawn;

    private bool deathStart = false;
    private float deathDelay = 0.5f;
    private float deathCount = 0f;

    public List<GameObject> dropItems;

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

        float FlyModifier;

        if(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().MetalOn)
        {
            FlyModifier = 10f;
        }
        else
        {
            FlyModifier = 1f;
        }

        if(trackPlayer.position.x > transform.position.x)
            transform.position += -transform.right * 0.1f * FlyModifier;
        else
            transform.position += transform.right * 0.1f * FlyModifier;

        if (currentHP > Hitpoints) currentHP = Hitpoints;

        if (currentHP <= 0) deathStart = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!deathStart)
        {
            float horizontalMovement = 0f;
            float verticalMovement = 0f;

            float movementSpeed = 0f;

            if (trackPlayer.position.x - EnemyStopDistance > transform.position.x)
            {
                horizontalMovement = 1f;
                movementSpeed = 0.5f;
                GetComponentInChildren<SpriteRenderer>().flipX = false;
            }
            else if (trackPlayer.position.x + EnemyStopDistance < transform.position.x)
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

            if (JustAttacked)
            {
                AttackTimer += Time.deltaTime;
                if (AttackTimer > AttackDelay)
                {
                    AttackTimer = 0f;
                    JustAttacked = false;
                }
            }
        }
        else
        {
            deathCount += Time.deltaTime;
            if(deathCount >= deathDelay)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                JustAttacked = true;
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        switch(other.tag)
        {
            case "Player":
                if(!JustAttacked && !deathStart)
                {
                    AIAnime.SetTrigger("Punching");
                    other.GetComponent<PlayerControl>().TakeDamage(AttackDamage);
                    JustAttacked = true;
                }
                break;
        }
    }

    private void OnDestroy()
    {
        if (dropItems.Count > 0)
        {
            int dropMe = Random.Range(0, dropItems.Count);
            Instantiate(dropItems[dropMe], transform.position, Quaternion.identity);
        }
    }
}
