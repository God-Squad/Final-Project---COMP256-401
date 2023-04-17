using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject wallPrefab;
    public GameObject gapPrefab;
    public GameObject obstacleParent;
    public float spawnTimer = 3;
    public Vector3 spawnLocation;
    public float maxGap = 2.5f;
    public float minGap = 1.5f;
    public float maxHeight = 4;
    public float minHeight = 1;
    public float levelHeight = 10;

    private void Start()
    {
        spawnLocation = new Vector3(0, 0f, 15);


    }

    public void GenerateObstacles()
    {
        for (int i = 0; i < obstacleParent.transform.childCount; i++)
        {
            Destroy(obstacleParent.transform.GetChild(i).gameObject);
        }

        SpawnWall(8);
    }


    public void SpawnWall(float zPos)
    {
        float height = CalculateHeight();
        float gap = CalculateGap();
        // y pos = y scale / 2
        GameObject bottomWall = Instantiate(wallPrefab, spawnLocation, wallPrefab.transform.rotation);
        bottomWall.transform.SetParent(obstacleParent.transform, false);
        bottomWall.transform.localScale = new Vector3(10f, height, 0.5f);
        bottomWall.transform.position = new Vector3(bottomWall.transform.position.x, height / 2, zPos);

        // y pos = 10 - y scale / 2
        GameObject topWall = Instantiate(wallPrefab, spawnLocation, wallPrefab.transform.rotation);
        float yScale = levelHeight - (height + gap);
        topWall.transform.SetParent(obstacleParent.transform, false);
        topWall.transform.localScale = new Vector3(10f, yScale, 0.5f);
        topWall.transform.position = new Vector3(topWall.transform.position.x, 10 - yScale / 2, zPos);

        // y pos = (bottom wall height + gap / 2) + bottom wall y pos

        GameObject gapObj = Instantiate(gapPrefab, spawnLocation, gapPrefab.transform.rotation);
        gapObj.transform.SetParent(obstacleParent.transform, false);
        gapObj.transform.localScale = new Vector3(10f, gap, 0.5f);
        gapObj.transform.position = new Vector3(gapObj.transform.position.x, height + gap / 2, zPos);

    }

    public float CalculateGap()
    {
        float gap = Random.Range(minGap, maxGap);
        return gap;
    }

    public float CalculateHeight()
    {
        float height = Random.Range(minHeight, maxHeight);
        return height;
    }
}
