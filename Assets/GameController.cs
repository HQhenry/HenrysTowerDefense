using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    private int lifes;
    public float targetTime = 0 ;
    public Transform target ;

    public GameObject waspPrefab;
    private List<Wasp> _waspsInGame;
    private List<Wasp> _waspsFreeToFollow;
    
    public GameObject beePrefab;
    public GameObject defendingHivePrefab;
    private GameObject defendingHive;
    
    private List<Bee> _beesInGame;
    
    public GameObject[] hearts;
    public GameObject gameOverScreen;
    public GameObject hiveButton;
    private bool _hiveButtonPressed = false;
    
    // Start is called before the first frame update
    void Start()
    {
        lifes = 4;
        _waspsInGame = new List<Wasp>();
        _waspsFreeToFollow = new List<Wasp>();
        
        _beesInGame = new List<Bee>();
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
            CheckWasps();
            CheckBees();
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
        GameObject newBeeGameObject = Instantiate(beePrefab, placementTransform, Quaternion.identity);
        newBeeGameObject.name = "DefendingBee";
        BeeBehaviour beeBehaviour = newBeeGameObject.AddComponent<BeeBehaviour>();

        
        
        Wasp followedWasp = FindNearestEnemy();
        beeBehaviour.target = followedWasp.WaspGameObject;
        Bee newBee = new Bee(newBeeGameObject, beeBehaviour);
        newBee.FollowedWasp = followedWasp;
        followedWasp.FollowingBee = newBee;
        _beesInGame.Add(newBee);
       
    }

    void AddWasp()
    {
        var placementTransform = new Vector3(-1.86f, 10.34f, Random.Range(16.0f, -6f));
        GameObject newWaspGameObject = Instantiate(waspPrefab, placementTransform, Quaternion.identity);
        newWaspGameObject.name = "EnemyWasp";
        newWaspGameObject.AddComponent<EnemyBehaviour>().target = target.transform;
        Wasp newWasp = new Wasp(newWaspGameObject);
        _waspsInGame.Add(newWasp);
        _waspsFreeToFollow.Add(newWasp);
    }
    Wasp FindNearestEnemy()
    {
        Wasp nearestWasp = null;
        float minDistance = float.MaxValue;
        foreach (var wasp in _waspsFreeToFollow)
        {
            var distance = Vector3.Distance(wasp.WaspGameObject.transform.position, defendingHive.transform.position); 
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
            Destroy(wasp.WaspGameObject);
        }
        
        foreach (var bee in _beesInGame)
        {
            Destroy(bee.BeeGameObject);
        }

        Destroy(defendingHive);
        foreach (var heart in hearts)
        {
          heart.SetActive(true);
        }
        lifes = 4;
        _waspsInGame = new List<Wasp>();
        _beesInGame = new List<Bee>();
        
        gameOverScreen.SetActive(false);
        hiveButton.SetActive(true);
        
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


    void CheckWasps()
    {
        Wasp waspToDestroy = null;
        foreach (var wasp in _waspsInGame)
        {
            if (Vector3.Distance(wasp.WaspGameObject.transform.position, target.position) < 0.5f)
            {
                
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
            _waspsInGame.Remove(waspToDestroy);
            waspToDestroy.FollowingBee.BeeBehaviour.target = defendingHive.gameObject;
            waspToDestroy.FollowingBee.FollowedWasp = null;
            waspToDestroy.FollowingBee.onWayToHive = true;
            Destroy(waspToDestroy.WaspGameObject);
           
        }
    }

    public void CheckBees()
    {
        foreach (var bee in _beesInGame)
        {
            Wasp followedWasp = bee.FollowedWasp;
            if (followedWasp != null)
            {
                if (Vector3.Distance(bee.BeeGameObject.transform.position,
                        followedWasp.WaspGameObject.transform.position) < 0.5f)
                {
                    _waspsInGame.Remove(followedWasp);
                    bee.FollowedWasp = null;
                    bee.BeeBehaviour.target = defendingHive.gameObject;
                    bee.onWayToHive = true;
                    Destroy(followedWasp.WaspGameObject);
                    
                   
                }
                
            }

            if (bee.onWayToHive)
            {
                if (Vector3.Distance(bee.BeeGameObject.transform.position,
                        defendingHive.transform.position) < 0.5f)
                {
                    _beesInGame.Remove(bee);
                    Destroy(bee.BeeGameObject);
                }
            }
        }
    }
}
