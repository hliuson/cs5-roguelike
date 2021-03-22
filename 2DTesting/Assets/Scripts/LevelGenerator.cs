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
    public int verticalBlocks;
    
    [SerializeField]
    public int horizontalBlocks;

    [SerializeField]
    public float blockSizing;

    private System.Random rand = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        HashSet<(int, int, terrainType)> roomPositions = getRoomTemplate();
        foreach((int x, int y, terrainType tt) roomPos in roomPositions)
        {
            GameObject prefab = null;
            if(roomPos.tt == terrainType.start)
            {
                prefab = startBlock;
            }
            if(roomPos.tt == terrainType.end)
            {
                prefab = endBlock;
            }
            if(roomPos.tt == terrainType.traversable)
            {
                prefab = traversableBlocks[0];//TODO: make this actually random
            }
            if(roomPos.tt == terrainType.nontraversable)
            {
                prefab = nontraversibleBlocks[0];
            }
            Instantiate(prefab, new Vector3(roomPos.x * blockSizing, roomPos.y * blockSizing, 0), Quaternion.identity);
        }
    }

    //Maybe i should just make this a class instead of a tuple lol
    HashSet<(int,int,terrainType)> getRoomTemplate()
    {
        HashSet<(int, int, terrainType)> terrainSet = new HashSet<(int, int, terrainType)>();
        for(int i = 0; i < horizontalBlocks; i++)
        {
            for(int j = 0; j < verticalBlocks; j++)
            {
                if(i == 0 && j == 0)
                {
                    terrainSet.Add((i, j, terrainType.start));
                }
                else if(i == horizontalBlocks-1 && j == verticalBlocks-1)
                {
                    terrainSet.Add((i, j, terrainType.end));
                }
                else
                {
                    terrainSet.Add((i, j, terrainType.traversable));
                }
            }
        }

        return terrainSet;
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
