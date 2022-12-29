using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    [SerializeField]private PlayerMovement player;
    [SerializeField]private GameObject floor;
    [SerializeField]private LayerMask floorMask;
    [SerializeField]private float spawnDistance = 10f;

    [SerializeField]private GameObject firstFloor;
    
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
    private List<Transform> _floors;

    private void Awake() {
        _floors = new List<Transform>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
        _floors.Add(firstFloor.transform);
        Transform currentFloor = firstFloor.transform;

        for(int i = 2; i <= 3; i++)
        {
            Transform endPoint = currentFloor.Find("End Point");
            Vector3 spawnPosition = endPoint.position;
            GameObject generatedFloor = Instantiate(floor,spawnPosition,Quaternion.identity);

            if(generateObstacles)
            {
                Transform generatedFloorEndPoint = generatedFloor.transform.Find("End Point");            
                GenerateObstacles(generatedFloor.transform,generatedFloorEndPoint);
            }

            currentFloor = generatedFloor.transform;
            
            _floors.Add(currentFloor);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Transform firstFloor = _floors[0];
        Transform firstFloorEndPoint = firstFloor.Find("End Point");
        float offset = player.transform.position.z - firstFloorEndPoint.position.z;
        if(offset >= spawnDistance)
        {
            
            Transform lastFloor = _floors[_floors.Count - 1];
            Transform lastFloorEndPoint = lastFloor.Find("End Point");
            
            GameObject generatedFloor = Instantiate(floor,lastFloorEndPoint.position,Quaternion.identity);
            _floors.Add(generatedFloor.transform);
        
            Transform generatedFloorEndPoint = generatedFloor.transform.Find("End Point");
            if(generateObstacles)
            {
                GenerateObstacles(generatedFloor.transform,generatedFloorEndPoint);
            }

            if(generateCoins)
            {
                GenerateCoins(generatedFloor.transform,generatedFloorEndPoint);
            }
            
            SetToOrigin();

            _floors.Remove(firstFloor);
            Destroy(firstFloor.gameObject);       
        }
    }

    void SetToOrigin()
    {
        player.transform.position -= Vector3.forward * backwardDistance;
        foreach(Transform floor in _floors)
        {
            floor.position -= Vector3.forward * backwardDistance;
        }
    }
    private void GenerateObstacles(Transform floor,Transform floorEndPoint)
    {
        int randomObstacleCount = Random.Range(1,maxObstaclesPerFloor + 1);
        for(int i = 1; i <= randomObstacleCount; i++)
        {
            int randomSpawnPtIndex = Random.Range(0,spawnPointRefs.Length);

            float positionX = spawnPointRefs[randomSpawnPtIndex].transform.position.x;
            float positionZ = floorEndPoint.position.z - Random.Range(0f,obstacleSpawnLength);
            float positionY = spawnPointRefs[randomSpawnPtIndex].transform.position.y;

            Vector3 spawnPosition = new Vector3(positionX,positionY,positionZ);
            Obstacle obstacle = Instantiate(obstaclePrefab,spawnPosition,Quaternion.identity);
            obstacle.transform.parent = floor;
        }
    }

    private void GenerateCoins(Transform floor,Transform floorEndPoint)
    {
        int randomCoinsCount = Random.Range(1,maxCoinsToGenerate + 1);

        for(int i = 1; i <= randomCoinsCount; i++)
        {
            int randomSpawnPtIndex = Random.Range(0,spawnPointRefs.Length);
            float positionX = spawnPointRefs[randomSpawnPtIndex].transform.position.x;
            float positionZ = floorEndPoint.position.z - Random.Range(0f,obstacleSpawnLength);
            float positionY = spawnPointRefs[randomSpawnPtIndex].transform.position.y;

            Vector3 spawnPosition = new Vector3(positionX,positionY,positionZ);
            Coin coin = Instantiate(coinPrefab,spawnPosition,Quaternion.identity);
            coin.transform.parent = floor;
        }
        
            
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.green;
        Vector3 obstacleSpawnLengthStart = transform.position;
        Vector3 obstacleSpawnLengthEnd = transform.position - Vector3.forward * obstacleSpawnLength;
        Gizmos.DrawLine(obstacleSpawnLengthStart,obstacleSpawnLengthEnd);
    }
}
