using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    [SerializeField]private PlayerMovement player;
    [SerializeField]private GameObject floor;
    [SerializeField]private float minDistance = 10f;

    [SerializeField]private GameObject firstFloor;
    [SerializeField]private float obstacleSpawnWidth = 2f;
    [SerializeField]private Obstacle obstaclePrefab;
    [SerializeField]private float obstacleSpawnLength = 20f;

    [Range(1,3)]
    [SerializeField]private int maxObstaclesPerFloor = 3;
    private Transform _previousFloor;

    // Start is called before the first frame update
    void Start()
    {
        _previousFloor = firstFloor.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Transform previousFloorStartPoint = _previousFloor.Find("Start Point");
        float distance = player.transform.position.z - previousFloorStartPoint.position.z;
        if(distance >= minDistance)
        {
            Transform endPoint = _previousFloor.Find("End Point");
            Vector3 spawnPosition = endPoint.position;

            GameObject generatedFloor = Instantiate(floor,spawnPosition,Quaternion.identity);
            GenerateObstacles(generatedFloor.transform.Find("End Point"));
            _previousFloor = generatedFloor.transform;
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
