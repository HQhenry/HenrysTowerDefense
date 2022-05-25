
using DefaultNamespace;
using UnityEngine;

public class Bee
{
    public GameObject BeeGameObject;
    public Wasp FollowedWasp;
    public BeeBehaviour BeeBehaviour;
    public bool onWayToHive;

    public Bee(GameObject beeGameObject, BeeBehaviour beeBehaviour)
    {
        this.BeeGameObject = beeGameObject;
        this.BeeBehaviour = beeBehaviour;
        this.onWayToHive = false;
    }
}
