using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInstance
{
    GameObject gameObject;
    Transform transform;

    bool hasPoolObjectComponent;
    PoolObject poolObject;

    int mainPrefabID;

    public PoolObject PoolObject { get => poolObject; }
    public GameObject GameObject { get => gameObject; }
    public int MainPrefabID { get => mainPrefabID; }

    public ObjectInstance(GameObject gameObject,int mainPrefabID)
    {
        this.gameObject = gameObject;
        this.transform = gameObject.transform;
        this.gameObject.SetActive(false);
        this.mainPrefabID = mainPrefabID;
        
        PoolObject poolObjectReference = gameObject.GetComponent<PoolObject>();
        if(poolObjectReference != null)
        {
            this.hasPoolObjectComponent = true;
            this.poolObject = poolObjectReference;
            this.poolObject.ObjectInstanceComp = this;
        }
    }

    
    public void Reuse(Vector3 position,Quaternion rotation)
    {
        if(this.hasPoolObjectComponent)
        {
            this.poolObject.Reuse();
        }
        this.gameObject.SetActive(true);        
        this.gameObject.transform.position = position;
        this.gameObject.transform.rotation = rotation;
    }
    

    public void SetParent(Transform parent)
    {
        this.gameObject.transform.parent = parent;
    }
}
