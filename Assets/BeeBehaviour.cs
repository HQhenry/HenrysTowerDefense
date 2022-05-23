using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BeeBehaviour : MonoBehaviour
{
    
    public float speed = 2.0f; 
    public GameObject target ;
    private bool destroyedEnemy = false;
    public GameObject spawnedHive = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
           
        //transform.Translate(0, 0, speed * Time.deltaTime);
        transform.LookAt(target.transform);
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);

        if (!destroyedEnemy)
        {
            if (target.IsUnityNull())
            {
                
                    Destroy(gameObject);
            
                
            }
            if (Vector3.Distance(transform.position, target.transform.position) < 0.5f)
            {
                destroyedEnemy = true;
                Destroy(target);
            
            }
        }
        if (destroyedEnemy)
        {
            target = spawnedHive;
            if (Vector3.Distance(transform.position, target.transform.position) < 0.5f)
            {
                Destroy(gameObject);
            
            }
        }
        

        
    }
}
