using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{

    private int lifes;
    public float targetTime = 0 ;
    public Transform target ;

    public GameObject waspPrefab;
    private List<GameObject> waspsInGame;
    private List<GameObject> waspsFreeToFollow;
    
    public GameObject beePrefab;
    public GameObject defendingHive;
    
    
    public GameObject[] hearts;
    public GameObject gameOverScreen;
    
    // Start is called before the first frame update
    void Start()
    {
        lifes = 4;
        waspsInGame = new List<GameObject>();
        waspsFreeToFollow = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        targetTime -= Time.deltaTime;
 
        if (targetTime <= 0.0f) {
 
            timerEnded();
 
        }


        GameObject waspToDestroy = null;
        foreach (var wasp in waspsInGame)
        {
            if (Vector3.Distance(wasp.transform.position, target.position) < 0.2f)
            {

                if (lifes -1 > 0)
                {
                
                    hearts[lifes - 1].SetActive(false);
                    lifes -= 1 ;
                  
                }
                else
                {
                    hearts[0].SetActive(false);
                    gameOverScreen.SetActive(true);
                }

                waspToDestroy = wasp;
                
                
            }
        }

        if (waspToDestroy != null)
        {
            Destroy(waspToDestroy);
            waspsInGame.Remove(waspToDestroy);
        }
       
    }

    void timerEnded()
    {
        targetTime = 5.0f;
        addWasp();
       
    
        addBee();
    }

    void addBee()
    {
        var transform = defendingHive.transform.position;
        GameObject newBee = GameObject.Instantiate(beePrefab, transform, Quaternion.identity) as GameObject;
        newBee.name = "DefendingBee";
        BeeBehaviour beeBehaviour = newBee.AddComponent<BeeBehaviour>();

        beeBehaviour.target = findNearestEnemy();
        beeBehaviour.spawnedHive = defendingHive;
       
    }

    void addWasp()
    {
        var transform = new Vector3(-1.86f, 10.34f, Random.Range(16.0f, -6f));
        GameObject newWasp = GameObject.Instantiate(waspPrefab, transform, Quaternion.identity) as GameObject;
        newWasp.name = "EnemyWasp";
        newWasp.AddComponent<EnemyBehaviour>().target = target.transform;
        waspsInGame.Add(newWasp);
        waspsFreeToFollow.Add(newWasp);
    }
    GameObject findNearestEnemy()
    {
        GameObject nearestWasp = null;
        float minDistance = float.MaxValue;
        foreach (var wasp in waspsFreeToFollow)
        {
            var distance = Vector3.Distance(wasp.transform.position, defendingHive.transform.position); 
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestWasp = wasp;
            }
        }

        waspsFreeToFollow.Remove(nearestWasp);
        return nearestWasp;
    }
    public void restart()
    {
        foreach (var wasp in waspsInGame)
        {
            Destroy(wasp);
        }

        foreach (var heart in hearts)
        {
          heart.SetActive(true);
        }
        lifes = 4;
        waspsInGame = new List<GameObject>();
        gameOverScreen.SetActive(false);
    }
}
