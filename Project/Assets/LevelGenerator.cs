using System.Collections;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject wallPrefab;
    public GameObject gapPrefab;
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
        StartCoroutine(StartLevelGeneration());

    }

    IEnumerator StartLevelGeneration()
    {
        while (true)
        {
            SpawnWall();
            yield return new WaitForSeconds(spawnTimer);
        }
    }

    public void SpawnWall()
    {
        float height = CalculateHeight();
        float gap = CalculateGap();
        // y pos = y scale / 2
        GameObject bottomWall = Instantiate(wallPrefab, spawnLocation, wallPrefab.transform.rotation);
        bottomWall.transform.localScale = new Vector3(10f, height, 0.5f);
        bottomWall.transform.position = new Vector3(bottomWall.transform.position.x, height / 2, bottomWall.transform.position.z);
        // y pos = 10 - y scale / 2
        GameObject topWall = Instantiate(wallPrefab, spawnLocation, wallPrefab.transform.rotation);
        float yScale = levelHeight - (height + gap);
        topWall.transform.localScale = new Vector3(10f, yScale, 0.5f);
        topWall.transform.position = new Vector3(topWall.transform.position.x, 10 - yScale / 2, topWall.transform.position.z);

        // y pos = (bottom wall height + gap / 2) + bottom wall y pos

        GameObject gapObj = Instantiate(gapPrefab, spawnLocation, gapPrefab.transform.rotation);
        gapObj.transform.localScale = new Vector3(10f, gap, 0.5f);
        gapObj.transform.position = new Vector3(gapObj.transform.position.x, height + gap / 2, gapObj.transform.position.z);

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
