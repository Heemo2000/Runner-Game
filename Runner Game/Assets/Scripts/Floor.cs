using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : PoolObject
{
    [SerializeField]private Transform endPoint;
    private List<PoolObject> _thingsOnFloor;
    public Transform EndPoint { get => endPoint; }
    public List<PoolObject> ThingsOnFloor { get => _thingsOnFloor; }

    private void Awake() {
        _thingsOnFloor = new List<PoolObject>();
    }
    

    public override void Destroy()
    {
        //Debug.Log("Destroying floor...");
        for(int i = 0; i < _thingsOnFloor.Count; i++)
        {
            _thingsOnFloor[i].Destroy();
        }
        _thingsOnFloor.Clear();
        base.Destroy();

    }
    

    public void AddThing(PoolObject thing)
    {
        _thingsOnFloor.Add(thing);
    }
    
}
