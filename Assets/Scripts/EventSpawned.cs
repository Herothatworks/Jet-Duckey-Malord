using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSpawned : MonoBehaviour
{
    public bool WaveEvent;
    public int WaveCount;
    public List<SpawnMob> waveMobs;

    //Return these to private once you are done please.
    public List<GameObject> currentEnemies; //
    public int currentWave = 0; //
    public bool EventStarted = false; //
    public bool EventConcluded = false; //
    


    // Start is called before the first frame update
    void Start()
    {
        currentEnemies = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(EventStarted && !EventConcluded)
        {
            //If the enemy list is empty, the event has been started and is not finished
            if(currentEnemies.Count < 1)
            {
                //We move the wave counter up
                ++currentWave;

                //If we have completed all waves (so, at 2 out of 1), it means we are done. Regardless, we want to make sure to unlock stuff.
                //Non-Waves should still only have 1 wave, to have enemies spawn as the player progresses without overwhelming them from the start
                if (currentWave > WaveCount)
                {
                    EventConcluded = true;
                    GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMove>().CamLock = false;
                }
                else
                {
                    //Add all the mobs you added to the waves in the proper order.
                    foreach (SpawnMob spawnMob in waveMobs)
                    {
                        if (spawnMob.WaveCount == currentWave)
                        {
                            GameObject newMob;
                            newMob = Instantiate(spawnMob.EnemyMob, spawnMob.SpawnLocation.position, Quaternion.identity, transform) as GameObject;
                            newMob.GetComponent<EnemyController>().SetStats(spawnMob.HitPoints, spawnMob.DamageDone);
                            currentEnemies.Add(newMob);
                        }
                    }
                }
            }

            //Goes backward to check enemies to make sure they are still alive. If not, remove them from the list. If you go forward, it will always cause you to skip an index, or
            //create an out of index error, because 3-1 = 2, but if you still count to 3, 3 doesn't exist anymore. If that makes sense. It did to me.
            for (int i = currentEnemies.Count - 1; i >= 0; --i)
            {
                if (currentEnemies[i].GetComponent<EnemyController>().currentHP <= 0)
                {
                    currentEnemies.RemoveAt(i);
                }
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case "Player":
                if(!EventStarted)
                {
                    if (WaveEvent)
                    {
                        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMove>().CamLock = true;
                    }

                    EventStarted = true;                    
                }
                break;
        }
    }
}
