using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    public GameObject[] traversableBlocks;

    [SerializeField]
    public GameObject[] nontraversibleBlocks;

    [SerializeField]
    public GameObject startBlock;

    [SerializeField]
    public GameObject endBlock;

    [SerializeField]
    public GameObject borderWall;

    [SerializeField]
    public int verticalBlocks;

    [SerializeField]
    public int horizontalBlocks;

    [SerializeField]
    public float blockSizing;

    private System.Random rand = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        Dictionary<(int, int), terrainType> roomPositions = getRoomTemplate();
        foreach (KeyValuePair<(int, int), terrainType> kvp in roomPositions)
        {
            (int x, int y) location = kvp.Key;
            terrainType tt = kvp.Value;

            GameObject prefab = null;
            //Instantiate Block
            if (tt == terrainType.start)
            {
                prefab = startBlock;
            }
            if (tt == terrainType.end)
            {
                prefab = endBlock;
            }
            if (tt == terrainType.traversable)
            {
                prefab = traversableBlocks[0];//TODO: make this actually random
            }
            if (tt == terrainType.nontraversable)
            {
                prefab = nontraversibleBlocks[0];
            }
            Vector3 roomLocation = new Vector3(location.x * blockSizing, location.y * blockSizing, 0);
            Instantiate(prefab, roomLocation, Quaternion.identity);

            //Check if any of the four directions is on the perimeter
            bool topWall = !roomPositions.ContainsKey((location.x, location.y + 1));
            bool bottomWall = !roomPositions.ContainsKey((location.x, location.y - 1));
            bool leftWall = !roomPositions.ContainsKey((location.x + 1, location.y));
            bool rightWall = !roomPositions.ContainsKey((location.x - 1, location.y));

            if (topWall)
            {
                Instantiate(borderWall, roomLocation + new Vector3(0, blockSizing / 2, 0), Quaternion.Euler(0, 0, 0));
            }
            if (bottomWall)
            {
                Instantiate(borderWall, roomLocation + new Vector3(0, -blockSizing / 2, 0), Quaternion.Euler(0, 0, 180));
            }
            if (rightWall)
            {
                Instantiate(borderWall, roomLocation + new Vector3(-blockSizing / 2, 0, 0), Quaternion.Euler(0, 0, 90));
            }
            if (leftWall)
            {
                Instantiate(borderWall, roomLocation + new Vector3(blockSizing / 2, 0, 0), Quaternion.Euler(0, 0, 270));
            }
        }
    }

    //Maybe i should just make this a class instead of a tuple lol
    Dictionary<(int, int), terrainType> getRoomTemplate()
    {
        Dictionary<(int, int), terrainType> roomTiles = new Dictionary<(int, int), terrainType>();
        for (int i = 0; i < horizontalBlocks; i++)
        {
            for (int j = 0; j < verticalBlocks; j++)
            {
                if (i == 0 && j == 0)
                {
                    roomTiles.Add((i, j), terrainType.start);
                }
                else if (i == horizontalBlocks - 1 && j == verticalBlocks - 1)
                {
                    roomTiles.Add((i, j), terrainType.end);
                }
                else
                {
                    roomTiles.Add((i, j), terrainType.traversable);
                }
            }
        }

        return roomTiles;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

enum terrainType
{
    start,
    end,
    traversable,
    nontraversable
}
