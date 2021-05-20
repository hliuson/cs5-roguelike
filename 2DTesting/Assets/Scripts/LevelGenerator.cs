using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    public GameObject[] traversableBlocks;

    [SerializeField]
    public GameObject[] nontraversibleBlocks;

    public List<Enemy> enemiesList;

    [SerializeField]
    public GameObject startBlock;

    [SerializeField]
    public GameObject endBlock;

    [SerializeField]
    public GameObject borderWall;

    [SerializeField]
    public GameObject debugFlagTemp;

    [SerializeField]
    public int verticalBlocks;

    [SerializeField]
    public int horizontalBlocks;

    [SerializeField]
    public float nontraversableRatio;

    [SerializeField]
    public float blockSizing;

    [SerializeField]
    public float maxFailedAttempts;

    [SerializeField]
    public float difficulty;

    private System.Random rand = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        //EnemyCounter.clear();
        GameObject[] enemyGameObjects = Resources.LoadAll<GameObject>("Prefabs/Combatables");
        enemiesList = new List<Enemy>();
        for(int i = 0; i < enemyGameObjects.Length; i++)
        {
            enemiesList.Add(enemyGameObjects[i].GetComponent<Enemy>());
        }
        Dictionary<(int, int), terrainType> roomPositions = getRoomTemplate();
        generateRoom(roomPositions);
        spawnEnemies(roomPositions, 1);
        GetComponent<NavGrid>().wakeUp();
    }

    void generateRoom(Dictionary<(int, int), terrainType> roomPositions)
    {
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
                int index = rand.Next(traversableBlocks.Length);
                prefab = traversableBlocks[index];
            }
            if (tt == terrainType.nontraversable)
            {
                int index = rand.Next(nontraversibleBlocks.Length);
                prefab = nontraversibleBlocks[index];
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

    void spawnEnemies(Dictionary<(int, int), terrainType> roomPositions, int levelCount)
    {
        //this formula might be tweaked but tbh balance isn't a huge concern.
        float enemyPoints = levelCount * difficulty;
        while (true)
        {
            List<Enemy> toRemove = new List<Enemy>();
            foreach (Enemy e in enemiesList)
            {
                Debug.Log(e);
                if (enemyPoints < e.difficulty())
                {
                    toRemove.Add(e);
                }
            }
            foreach (Enemy e in toRemove)
            {
                enemiesList.Remove(e);
            }
            if(enemiesList.Count == 0)
            {
                return;
            }
            int index = rand.Next(enemiesList.Count);
            GameObject toSpawn = enemiesList[index].gameObject;

            enemyPoints -= toSpawn.GetComponent<Enemy>().difficulty();

            List<(int, int)> passableTiles = new List<(int, int)>();
            foreach (KeyValuePair<(int, int), terrainType> kvp in roomPositions)
            {
                (int x, int y) location = kvp.Key;
                terrainType tt = kvp.Value;
                if (tt == terrainType.traversable)
                {
                    passableTiles.Add(location);
                }
            }
            (int x, int y) tile = passableTiles[rand.Next(passableTiles.Count)];
            Vector3 roomLocation = new Vector3(tile.x * blockSizing, tile.y * blockSizing, 0);
            Instantiate(toSpawn, roomLocation, Quaternion.Euler(0, 0, 0));
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
        int blockedTiles = 0;
        int failedAttempts = 0;
        while (blockedTiles < horizontalBlocks * verticalBlocks * nontraversableRatio){
            int x = rand.Next(horizontalBlocks);
            int y = rand.Next(verticalBlocks);
            (int, int) coords = (x, y);
            if (roomTiles[coords] != terrainType.end && roomTiles[coords] != terrainType.start)
            {
                roomTiles.Remove((x, y));
                roomTiles.Add((x, y), terrainType.nontraversable);
                if (!checkPassability(roomTiles, (0, 0)))
                {
                    failedAttempts++;
                    roomTiles.Remove((x, y));
                    roomTiles.Add((x, y), terrainType.traversable);
                }
                else
                {
                    failedAttempts = 0;
                    blockedTiles++;
                }
                if (failedAttempts > maxFailedAttempts)
                {
                    break;
                }
            }
        }
        Debug.Assert(checkPassability(roomTiles, (0,0)) == true);
        return roomTiles;
    }

    bool checkPassability(Dictionary<(int, int), terrainType> roomTiles, (int, int) start)
    {
        bool didAddNewTiles = true;
        HashSet<(int,int)> accessibleTiles = new HashSet<(int, int)>();
        HashSet<(int, int)> newTiles = new HashSet<(int, int)>();
        HashSet<(int, int)> newTilesBuffer = new HashSet<(int, int)>();
        newTiles.Add(start);

        Debug.Assert(roomTiles[start] == terrainType.start, "Passability check must begin at start tile");

        while (didAddNewTiles)
        {
            didAddNewTiles = false;
            foreach((int x, int y) coords in newTiles)
            {
                
                int x = coords.x;
                int y = coords.y;
                (int,int)[] neighbors = { (x + 1, y), (x - 1, y), (x, y + 1), (x, y - 1)};
                foreach((int, int) neighbor in neighbors)
                {
                    if (!accessibleTiles.Contains(neighbor) && !newTiles.Contains(neighbor))
                    {
                        terrainType output;
                        if (roomTiles.TryGetValue(neighbor, out output))
                        {
                            if (output == terrainType.traversable)
                            {
                                didAddNewTiles = true;
                                newTilesBuffer.Add(neighbor);
                            }
                            if (output == terrainType.end)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            accessibleTiles.UnionWith(newTiles);
            newTiles.Clear();
            newTiles.UnionWith(newTilesBuffer);
            newTilesBuffer.Clear();
        }
        return false;
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