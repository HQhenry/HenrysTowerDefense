using UnityEngine;

namespace DefaultNamespace
{
    public class Wasp
    {
        public GameObject WaspGameObject;
        public Bee FollowingBee;
        public bool destroyed;

        public Wasp(GameObject waspGameObject)
        {
            this.WaspGameObject = waspGameObject;
            destroyed = false;
        }
    }
}