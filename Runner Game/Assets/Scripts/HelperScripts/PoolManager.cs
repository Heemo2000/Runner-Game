using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PoolManager : GenericSingleton<PoolManager>
{
    private Dictionary<int,Queue<ObjectInstance>> _poolDictionary;

    protected override void Awake() {
        base.Awake();
        _poolDictionary = new Dictionary<int, Queue<ObjectInstance>>();
    }

    private void Start() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void CreatePool(GameObject prefab,int poolSize)
    {
        int id = prefab.GetInstanceID();

        if(_poolDictionary.ContainsKey(id) == false)
        {
            _poolDictionary.Add(id,new Queue<ObjectInstance>());
            GameObject poolObjectsContainer = new GameObject(prefab.name + " pool");
            poolObjectsContainer.transform.parent = transform;
            
            for(int i = 0; i < poolSize; i++)
            {
                ObjectInstance objectInPool = new ObjectInstance(Instantiate(prefab) as GameObject,id);
                objectInPool.SetParent(poolObjectsContainer.transform);
                _poolDictionary[id].Enqueue(objectInPool);
                
            }
        }
    }

    /*
    public void ReuseObject(GameObject prefab,Vector3 position,Quaternion rotation)
    {
        int id = prefab.GetInstanceID();

        if(!_poolDictionary.ContainsKey(id))
        {
            Debug.LogError(prefab.name + " Pool not found!!");
            return;
        }


        ObjectInstance objectInPool = _poolDictionary[id].Dequeue();
        objectInPool.Reuse(position,rotation);
        _poolDictionary[id].Enqueue(objectInPool);
    }
    */
    public ObjectInstance ReuseObject(GameObject prefab,Vector3 position,Quaternion rotation)
    {
        int id = prefab.GetInstanceID();

        if(!_poolDictionary.ContainsKey(id))
        {
            Debug.LogError(prefab.name + " pool not found!!");
            return null;
        }

        if(_poolDictionary[id].Count == 0)
        {
            return null;
        }
        ObjectInstance objectInPool = _poolDictionary[id].Dequeue();
        objectInPool.Reuse(position,rotation);
        //_poolDictionary[id].Enqueue(objectInPool);

        return objectInPool;
    }

    public void ReturnObjectToPool(ObjectInstance instance)
    {
        

        if(!_poolDictionary.ContainsKey(instance.MainPrefabID))
        {
            Debug.LogError(instance.GameObject.name + " Pool not found!!");
            return;
        }

        _poolDictionary[instance.MainPrefabID].Enqueue(instance);
    }
    

    public void ClearAllPools()
    {
        foreach(var pair in _poolDictionary)
        {
            Queue<ObjectInstance> poolObjects = pair.Value;

            while(poolObjects.Count > 0)
            {
                poolObjects.Dequeue();
            }
        }

        _poolDictionary.Clear();
        
        GameObject[] allChildren = new GameObject[transform.childCount];

        int i = 0;
        foreach(Transform child in transform)
        {
            allChildren[i] = child.gameObject;
            i++;
        }

        foreach(GameObject child in allChildren)
        {
            Destroy(child);
        }
    }

    private void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        ClearAllPools();
    }
    private void OnDestroy() 
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
