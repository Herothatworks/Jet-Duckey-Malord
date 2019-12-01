using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchAttack : MonoBehaviour
{
    public float Damage;
    public bool hurtEnemy;
    public bool EndThis;


    void FixedUpdate()
    {
        if(EndThis)
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
