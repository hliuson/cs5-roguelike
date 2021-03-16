using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    public GameObject[] roomBlockPrefabs;


    [SerializeField]
    public float[] weights;

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
            Instantiate(roomBlockPrefabs[0], new Vector3(roomPos.x * blockSizing, roomPos.y * blockSizing, 0), Quaternion.identity);
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
                terrainSet.Add((i, j, terrainType.traversable));
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
