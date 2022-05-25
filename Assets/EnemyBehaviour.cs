using UnityEngine;

namespace DefaultNamespace
{
    public class EnemyBehaviour : MonoBehaviour
    {
        public float speed = 2.0f; 
        public Transform target ;
        
        void Update()
        {
            float step = speed * Time.deltaTime;
           
            //transform.Translate(0, 0, speed * Time.deltaTime);
            transform.LookAt(target);
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
            
        }
    }
}