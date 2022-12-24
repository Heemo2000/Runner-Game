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
    [SerializeField]private float obstacleSpawnWidth = 2f;
    [SerializeField]private Obstacle obstaclePrefab;
    [SerializeField]private float obstacleSpawnLength = 20f;
    [Range(1,3)]
    [SerializeField]private int maxObstaclesPerFloor = 3;
    [Min(0f)]
    [SerializeField]private float backwardDistance = 25f;
    
    private List<Transform> _floors;

    private void Awake() {
        _floors = new List<Transform>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
        Transform currentFloor = firstFloor.transform;
        _floors.Add(currentFloor);

        for(int i = 2; i <= 3; i++)
        {
            Transform endPoint = currentFloor.Find("End Point");
            Vector3 spawnPosition = endPoint.position;
            GameObject generatedFloor = Instantiate(floor,spawnPosition,Quaternion.identity);
            currentFloor = generatedFloor.transform;
            _floors.Add(currentFloor);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Transform secondFloor = _floors[1];
        Transform startPoint = secondFloor.Find("Start Point");
        float offset = player.transform.position.z - startPoint.position.z;
        if(offset >= spawnDistance)
        {
            player.transform.position -= Vector3.forward * backwardDistance;
            Transform initialFloor = _floors[0];
            _floors.RemoveAt(0);
            Destroy(initialFloor.gameObject);
            
            Transform lastFloor = _floors[_floors.Count-1];
            Debug.Log("Last Floor name : " + lastFloor.name);
            Transform lastFloorEndPoint = lastFloor.Find("End Point");
            GameObject generatedFloor = Instantiate(floor,lastFloorEndPoint.position,Quaternion.identity);
            _floors.Add(generatedFloor.transform);

            foreach(Transform floor in _floors)
            {
                floor.position -= Vector3.forward * backwardDistance;
            }
        }
    }


    private void GenerateObstacles(Transform floorEndPoint)
    {
        int randomObstacleCount = Random.Range(1,maxObstaclesPerFloor + 1);
        for(int i = 1; i <= randomObstacleCount; i++)
        {
            float positionX = Random.Range(floorEndPoint.position.x - obstacleSpawnWidth/2f,floorEndPoint.position.x + obstacleSpawnWidth/2f);
            float positionZ = floorEndPoint.position.z - Random.Range(0f,obstacleSpawnLength);
            float positionY = floorEndPoint.position.y;

            Vector3 spawnPosition = new Vector3(positionX,positionY,positionZ);
            Instantiate(obstaclePrefab,spawnPosition,Quaternion.identity);
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Vector3 obstacleSpawnWidthStart = transform.position - Vector3.right * obstacleSpawnWidth/2f;
        Vector3 obstacleSpawnWidthEnd = transform.position + Vector3.right * obstacleSpawnWidth/2f;
        Gizmos.DrawLine(obstacleSpawnWidthStart,obstacleSpawnWidthEnd);

        Gizmos.color = Color.green;
        Vector3 obstacleSpawnLengthStart = transform.position;
        Vector3 obstacleSpawnLengthEnd = transform.position - Vector3.forward * obstacleSpawnLength;
        Gizmos.DrawLine(obstacleSpawnLengthStart,obstacleSpawnLengthEnd);
    }
}
