using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public static ObstacleManager Instance;

    [SerializeField] List<GameObject> obstaclePrefabs;

    [SerializeField] float minimumInterval;
    [SerializeField] float maximumInterval;
    private float interval;
    private float intervalGap;

    [Space]
    [SerializeField] float obstacleXOrigin;
    [SerializeField] Vector2 obstacleYOrigin;
    [SerializeField] float obstacleZOrigin;

    private float xPos;
    public float XPos { set { xPos = value; } }

    private bool isCheckingInterval = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //SetInterval();
    }

    private void Update()
    {
        if(isCheckingInterval)
        {
            CheckInterval();
        }
    }

    private void SpawnRandomObstacle()
    {
        if(obstaclePrefabs.Count > 0)
        {
            int _index = Random.Range(0, obstaclePrefabs.Count);
            Instantiate(obstaclePrefabs[_index], new Vector3(obstacleXOrigin, Random.Range(obstacleYOrigin.x, obstacleYOrigin.y), obstacleZOrigin), obstaclePrefabs[_index].transform.rotation, transform);
        }
    }

    private void CheckInterval()
    {
        if(xPos >= intervalGap)
        {
            SpawnRandomObstacle();
            SetInterval();
        }
    }

    private void SetInterval()
    {
        interval = Random.Range(minimumInterval, maximumInterval);
        intervalGap = xPos + interval;

        isCheckingInterval = true;
    }

    public void StartSpawning()
    {
        SetInterval();
    }

    public void SpawnObstacle(GameObject _obstacle, float _yPos)
    {
        Instantiate(_obstacle, new Vector3(obstacleXOrigin, _yPos, obstacleZOrigin), _obstacle.transform.rotation, transform);
    }

    public void StopSpawning()
    {
        foreach (Transform _child in transform)
        {
            Destroy(_child.gameObject);
        }

        isCheckingInterval = false;
    }
}
