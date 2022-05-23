using UnityEngine;

namespace DefaultNamespace
{
    public class EnemyBehaviour : MonoBehaviour
    {
        public float speed = 2.0f; 
        public Transform target ;
   

        void Start()
        {
           // target.position = new Vector3(16.2600002f, 12.25f, 4.63000011f);
        }
        void Update()
        {
            float step = speed * Time.deltaTime;
           
            //transform.Translate(0, 0, speed * Time.deltaTime);
            transform.LookAt(target);
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
            
        }
    }
}