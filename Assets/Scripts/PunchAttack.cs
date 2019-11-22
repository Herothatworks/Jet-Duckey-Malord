using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchAttack : MonoBehaviour
{
    public float Damage;
    public bool hurtEnemy;

    private float LiveTime = 0.3f;
    private float AliveTime = 0f;

    void Update()
    {
        AliveTime += Time.deltaTime;
        if(AliveTime >= LiveTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case "Enemy":
                if (hurtEnemy)
                {
                    other.transform.GetComponent<EnemyController>().TakeHit(Damage);
                }
                break;
        }
    }
}
