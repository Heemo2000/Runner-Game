using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PathGenerator : MonoBehaviour
{
    [SerializeField]private CinemachineVirtualCamera playerCamera;
    [SerializeField]private PlayerMovement player;

    [Min(0)]
    [SerializeField]private int maxFloors = 100;
    [SerializeField]private Floor floor;

    [SerializeField]private int initialFloorGenCount = 5;
    [SerializeField]private int floorsGenerationCount = 5;
    [SerializeField]private LayerMask floorMask;
    [SerializeField]private float spawnDistance = 10f;

    [SerializeField]private Floor firstFloor;

    [Min(0f)]
    [SerializeField]private float floorDestroyTime = 1.0f;
    
    [SerializeField]private GameObject[] spawnPointRefs;
    [SerializeField]private Obstacle obstaclePrefab;
    [SerializeField]private float obstacleSpawnLength = 20f;
    [Range(1,3)]
    [SerializeField]private int maxObstaclesPerFloor = 3;
    [SerializeField]private bool generateObstacles = true;
    
    [Min(0f)]
    [SerializeField]private float backwardDistance = 25f;
    [SerializeField]private Coin coinPrefab;
    
    [SerializeField]private int maxCoinsToGenerate = 5;
    
    [SerializeField]private bool generateCoins = true;
    private List<Floor> _floors;

    private void Awake() {
        _floors = new List<Floor>();
    }
    // Start is called before the first frame update
    void Start()
    {
        PoolManager.Instance.CreatePool(floor.gameObject,maxFloors);
        PoolManager.Instance.CreatePool(obstaclePrefab.gameObject,maxFloors * maxObstaclesPerFloor);
        PoolManager.Instance.CreatePool(coinPrefab.gameObject,maxFloors * maxCoinsToGenerate);
        _floors.Add(firstFloor);
        Floor currentFloor = firstFloor;

        for(int i = 2; i <= initialFloorGenCount; i++)
        {
            Transform endPoint = currentFloor.EndPoint;
            Vector3 spawnPosition = endPoint.position;
            Floor generatedFloor = (Floor)PoolManager.Instance.ReuseObject(floor.gameObject,spawnPosition,Quaternion.identity).PoolObject;

            Transform generatedFloorEndPoint = generatedFloor.EndPoint;
            if(generateObstacles)
            {
                GenerateObstacles(generatedFloor,generatedFloorEndPoint);
            }

            if(generateCoins)
            {
                GenerateCoins(generatedFloor,generatedFloorEndPoint);
            }
            currentFloor = generatedFloor;
            
            _floors.Add(currentFloor);
        }

        _floors.Remove(firstFloor);
        //Destroy(firstFloor.gameObject,floorDestroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        Floor firstFloor = _floors[0];
        Transform firstFloorEndPoint = firstFloor.EndPoint;
        float offset = player.transform.position.z - firstFloorEndPoint.position.z;
        if(offset >= spawnDistance)
        {
            
            for(int i = 1; i <= floorsGenerationCount; i++)
            {
                Floor lastFloor = _floors[_floors.Count - 1];
                Transform lastFloorEndPoint = lastFloor.EndPoint;
                
                Floor generatedFloor = PoolManager.Instance.ReuseObject(floor.gameObject,lastFloorEndPoint.position,Quaternion.identity).PoolObject as Floor;
                _floors.Add(generatedFloor);
            
                Transform generatedFloorEndPoint = generatedFloor.EndPoint;
                if(generateObstacles)
                {
                    GenerateObstacles(generatedFloor,generatedFloorEndPoint);
                }

                if(generateCoins)
                {
                    GenerateCoins(generatedFloor,generatedFloorEndPoint);
                }
            }
            
            SetToOrigin();
            _floors.Remove(firstFloor);
            StartCoroutine(DestroyFloor(firstFloor,floorDestroyTime));       
        }
    }

    void SetToOrigin()
    {
        playerCamera.enabled = false;
        player.transform.position -= Vector3.forward * backwardDistance;
        playerCamera.enabled = true;
        foreach(Floor floor in _floors)
        {
            floor.transform.position -= Vector3.forward * backwardDistance;
            List<PoolObject> things = floor.ThingsOnFloor;
            foreach(PoolObject thing in things)
            {
                if(thing != null)
                {
                    thing.transform.position -= Vector3.forward * backwardDistance;
                }
            }
        }
    }
    private void GenerateObstacles(Floor floor,Transform floorEndPoint)
    {
        int randomObstacleCount = Random.Range(1,maxObstaclesPerFloor + 1);
        for(int i = 1; i <= randomObstacleCount; i++)
        {
            int randomSpawnPtIndex = Random.Range(0,spawnPointRefs.Length);

            float positionX = spawnPointRefs[randomSpawnPtIndex].transform.position.x;
            float positionZ = floorEndPoint.position.z - Random.Range(0f,obstacleSpawnLength);
            float positionY = spawnPointRefs[randomSpawnPtIndex].transform.position.y;

            Vector3 spawnPosition = new Vector3(positionX,positionY,positionZ);
            Obstacle obstacle = PoolManager.Instance.ReuseObject(obstaclePrefab.gameObject,spawnPosition,Quaternion.identity).PoolObject as Obstacle;
            floor.AddThing(obstacle);
        }
    }

    private void GenerateCoins(Floor floor,Transform floorEndPoint)
    {
        int randomCoinsCount = Random.Range(1,maxCoinsToGenerate + 1);

        for(int i = 1; i <= randomCoinsCount; i++)
        {
            int randomSpawnPtIndex = Random.Range(0,spawnPointRefs.Length);
            float positionX = spawnPointRefs[randomSpawnPtIndex].transform.position.x;
            float positionZ = floorEndPoint.position.z - Random.Range(0f,obstacleSpawnLength);
            float positionY = spawnPointRefs[randomSpawnPtIndex].transform.position.y;

            Vector3 spawnPosition = new Vector3(positionX,positionY,positionZ);
            Coin coin = PoolManager.Instance.ReuseObject(coinPrefab.gameObject,spawnPosition,Quaternion.identity).PoolObject as Coin;
            floor.AddThing(coin);
        }
        
            
    }

    private IEnumerator DestroyFloor(Floor floor,float destroyTime)
    {
        yield return new WaitForSeconds(destroyTime);
        floor.Destroy();
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.green;
        Vector3 obstacleSpawnLengthStart = transform.position;
        Vector3 obstacleSpawnLengthEnd = transform.position - Vector3.forward * obstacleSpawnLength;
        Gizmos.DrawLine(obstacleSpawnLengthStart,obstacleSpawnLengthEnd);
    }
}
