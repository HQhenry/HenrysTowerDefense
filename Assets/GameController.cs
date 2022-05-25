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
    private List<GameObject> _waspsInGame;
    private List<GameObject> _waspsFreeToFollow;
    
    public GameObject beePrefab;
    public GameObject defendingHivePrefab;
    private GameObject defendingHive;
    
    
    public GameObject[] hearts;
    public GameObject gameOverScreen;
    public GameObject hiveButton;
    private bool _hiveButtonPressed = false;
    
    // Start is called before the first frame update
    void Start()
    {
        lifes = 4;
        _waspsInGame = new List<GameObject>();
        _waspsFreeToFollow = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_hiveButtonPressed) {
            targetTime -= Time.deltaTime;

            if (targetTime <= 0.0f)
            {

                TimerEnded();

            }


            GameObject waspToDestroy = null;
            foreach (var wasp in _waspsInGame)
            {
                if (Vector3.Distance(wasp.transform.position, target.position) < 0.5f)
                {
                    Debug.Log("Here");

                    if (lifes - 1 > 0)
                    {

                        hearts[lifes - 1].SetActive(false);
                        lifes -= 1;

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
                _waspsInGame.Remove(waspToDestroy);
            }
        }
    }

    void TimerEnded()
    {
        targetTime = 5.0f;
        AddWasp();
        AddBee();
    }

    void AddBee()
    {
        var placementTransform = defendingHive.transform.position;
        GameObject newBee = Instantiate(beePrefab, placementTransform, Quaternion.identity);
        newBee.name = "DefendingBee";
        BeeBehaviour beeBehaviour = newBee.AddComponent<BeeBehaviour>();

        beeBehaviour.target = FindNearestEnemy();
        beeBehaviour.spawnedHive = defendingHive;
       
    }

    void AddWasp()
    {
        var placementTransform = new Vector3(-1.86f, 10.34f, Random.Range(16.0f, -6f));
        GameObject newWasp = Instantiate(waspPrefab, placementTransform, Quaternion.identity);
        newWasp.name = "EnemyWasp";
        newWasp.AddComponent<EnemyBehaviour>().target = target.transform;
        _waspsInGame.Add(newWasp);
        _waspsFreeToFollow.Add(newWasp);
    }
    GameObject FindNearestEnemy()
    {
        GameObject nearestWasp = null;
        float minDistance = float.MaxValue;
        foreach (var wasp in _waspsFreeToFollow)
        {
            var distance = Vector3.Distance(wasp.transform.position, defendingHive.transform.position); 
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestWasp = wasp;
            }
        }

        _waspsFreeToFollow.Remove(nearestWasp);
        return nearestWasp;
    }
    public void Restart()
    {
        foreach (var wasp in _waspsInGame)
        {
            Destroy(wasp);
        }

        foreach (var heart in hearts)
        {
          heart.SetActive(true);
        }
        lifes = 4;
        _waspsInGame = new List<GameObject>();
        gameOverScreen.SetActive(false);
    }

    public void PlaceHive()
    {
        hiveButton.SetActive(false);
        var placementTransform = new Vector3(9.31000042f,10.25f,3.52999997f);
        GameObject newHive = Instantiate(defendingHivePrefab, placementTransform, Quaternion.identity);
        newHive.name = "Defending_Hive";
        defendingHive = newHive;
        _hiveButtonPressed = true;
    }
}
