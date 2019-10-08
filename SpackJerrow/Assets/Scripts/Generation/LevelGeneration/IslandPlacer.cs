using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandPlacer : SaveableMonoBehaviour
{

    public Transform parent;
    
    public Vector2 seaScale;

    public float islands;

    public float islandCountVariance;

    public int islandSize;

    public int islandSizeVariance;
    
    private List<IslandPlace> placedIslands = new List<IslandPlace>();

    private float radOffset;

    public GameObject island;

    public int treasuresOnIsland;

    public int treasureVariance;

    public List<GameObject> cliffs;

    private bool wasConstructed;

    protected override void OnFirstTimeBehaviourAwakend()
    {
        wasConstructed = true;
    }

    private void Start()
    {
        if (wasConstructed)
        {
            radOffset = Random.Range(0, 2);
            int actualIslandCount = Rand.Variance(islands, islandCountVariance);
            for (int i = 0; i < actualIslandCount; i++)
            {
                GenerateIsland(i, actualIslandCount);
            }
        }
    }

    private void GenerateIsland(int count, int max)
    {
        float percentageToMiddle = Random.Range(0.2f, 0.8f);
        float radVariance = (360f / max) / 180;
        float rad = Random.Range(count * radVariance, (1 + count) * radVariance) + radOffset;

        float xDiff = Mathf.Cos(Mathf.PI * rad) * seaScale.x * percentageToMiddle;
        float zDiff = Mathf.Sin(Mathf.PI * rad) * seaScale.y * percentageToMiddle;

        IslandPlace islandPlace = new IslandPlace();
        islandPlace.position = new Vector2(xDiff, zDiff);

        int xVar = Rand.Variance(0, islandSizeVariance);

        int xSize = islandSize + xVar;
        int zSize = Rand.Variance(islandSize + xVar / 4, (int)(islandSizeVariance / 1.5));
        islandPlace.scale = new Vector2(xSize, zSize);

        Transform newIsland = Instantiate(island).transform;
        TreasureIsland treasureIsland = newIsland.GetComponent<TreasureIsland>();
        treasureIsland.terrainWidth = xSize;
        treasureIsland.terrainLength = zSize;
        treasureIsland.treasureCount = Rand.Variance(treasuresOnIsland, treasureVariance);
        newIsland.parent = parent;
        newIsland.position = new Vector3(xDiff, -0.5f, zDiff);

        placedIslands.Add(islandPlace);
    }
    
    private struct IslandPlace
    {
        public Vector2 position;
        public Vector2 scale;
    }

}
