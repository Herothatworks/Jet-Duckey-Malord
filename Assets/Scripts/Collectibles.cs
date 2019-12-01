using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
    public bool givesHealth;
    public bool givesPower;
    public float HealthAmount;
    public float PowerAmount;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(givesHealth)
            {
                other.GetComponent<PlayerControl>().RestoreHealth(HealthAmount);
            }
            if(givesPower)
            {
                other.GetComponent<PlayerControl>().RestorePower(PowerAmount);
            }
            Destroy(gameObject);
        }
    }
}
