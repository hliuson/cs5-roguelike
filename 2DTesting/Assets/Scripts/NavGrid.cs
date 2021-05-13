using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//From https://www.youtube.com/playlist?list=PLFt_AvWsXl0cq5Umv3pMC9SPnKjfp9eGW
public class NavGrid : MonoBehaviour
{

	public bool displayGridGizmos;

	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public Vector2 offSet;
	public float nodeRadius;
	public float seekerRadius;

	Node[,] grid; //2D array
	float nodeDiameter;
	int gridSizeX, gridSizeY;
	float nodeBuffer; //To stop colliders from colliding 

	//Used for creating a buffer so colliders don't interfere with pathfinding
	private enum falseBoolean
	{
		None,
		NoDuplicate,
		Duplicate
	}
	
	//Here to be called after the level is generated
	public void wakeUp()
    {
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

		nodeBuffer = seekerRadius / nodeDiameter;
		createGrid();
	}
	/*
	void Awake()
	{
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

		nodeBuffer = seekerRadius / nodeDiameter;
		createGrid();
	}
	*/
	public int maxSize
	{
		get
		{
			return gridSizeX * gridSizeY;
		}
	}

	void createGrid()
	{
		
		grid = new Node[gridSizeX, gridSizeY];
		Vector2 worldBottomLeft = ((Vector2)transform.position + offSet);
		for (int x = 0; x < gridSizeX; x++)
		{
			for (int y = 0; y < gridSizeY; y++)
			{
				Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius); //+ new Vector2((gridWorldSize.x+offSet.x)/2, (gridWorldSize.y + offSet.y) / 2) - gridWorldSize/2;
				bool walkable = (Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask) == null); // if no collider2D is returned by overlap circle, then this node is walkable

				grid[x, y] = new Node(walkable, worldPoint, x, y);

			}
		}
		createBuffer((int)Math.Round(nodeBuffer / 2));
	}

	//Actually makes the buffer
	private void createBuffer(int depth)
	{
		falseBoolean[,] tempArray = new falseBoolean[gridSizeX, gridSizeY];
		foreach (Node node in grid)
		{
			if (node.walkable)
			{
				tempArray[node.gridX, node.gridY] = falseBoolean.None;
			}
			else
			{
				tempArray[node.gridX, node.gridY] = falseBoolean.Duplicate;
			}
		}

		//PROBLEM CHILD: DOES THE OPPOSITE OF WHAT IT SHOULD
		for (int x = 0; x < gridSizeX; x++)
		{
			for (int y = 0; y < gridSizeY; y++)
			{
				if (tempArray[x, y] == falseBoolean.Duplicate)
				{
					for (int i = -depth; i <= depth; i++)
					{
						if ((i + x) < 0 || (i + x) >= gridSizeX)
						{
							continue;
						}
						for (int j = -depth; j <= depth; j++)
						{
							if ((j + y) < 0 || (j + y) >= gridSizeY)
							{
								continue;
							}
							if (i == 0 && j == 0)
							{
								continue;
							}
							if (tempArray[(x + i), (y + j)] == falseBoolean.None)
							{
								tempArray[(x + i), (y + j)] = falseBoolean.NoDuplicate;
							}
						}
					}
					//tempArray[x, y] = falseBoolean.Duplicate;
				}
			}
		}

		for (int x = 0; x < gridSizeX; x++)
		{
			for (int y = 0; y < gridSizeY; y++)
			{
				if (tempArray[x, y] == falseBoolean.None)
				{
					grid[x, y].walkable = true;
					//print(grid[x, y].walkable);
				}
				else
				{
					grid[x, y].walkable = false;
				}
			}
		}
	}

	public List<Node> getNeighbours(Node node, int depth = 1)
	{
		List<Node> neighbours = new List<Node>();

		for (int x = -depth; x <= depth; x++)
		{
			for (int y = -depth; y <= depth; y++)
			{
				if (x == 0 && y == 0)
					continue;

				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
				{
					neighbours.Add(grid[checkX, checkY]);
				}
			}
		}

		return neighbours;
	}


	public Node nodeFromWorldPoint(Vector2 worldPosition)
	{
		float percentX = (worldPosition.x + gridWorldSize.x / 4.0f + 4.0f/3 * offSet.x) / gridWorldSize.x;
		float percentY = (worldPosition.y + gridWorldSize.y / 4.0f + 4.0f/3 * offSet.y) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
		//print("X: " + x.ToString() + " Y: " + y.ToString());
		return grid[x, y];
	}

	public Node closestWalkableNode(Node node)
	{
		int maxRadius = Mathf.Max(gridSizeX, gridSizeY) / 2;
		for (int i = 1; i < maxRadius; i++)
		{
			Node n = findWalkableInRadius(node.gridX, node.gridY, i);
			if (n != null)
			{
				return n;

			}
		}
		return null;
	}
	Node findWalkableInRadius(int  centerX, int  centerY, int radius)
	{

		for (int i = -radius; i <= radius; i++)
		{
			int verticalSearchX = i +  centerX;
			int horizontalSearchY = i +  centerY;

			// top
			if (inBounds(verticalSearchX,  centerY + radius))
			{
				if (grid[verticalSearchX,  centerY + radius].walkable)
				{
					return grid[verticalSearchX,  centerY + radius];
				}
			}

			// bottom
			if (inBounds(verticalSearchX,  centerY - radius))
			{
				if (grid[verticalSearchX,  centerY - radius].walkable)
				{
					return grid[verticalSearchX,  centerY - radius];
				}
			}
			// right
			if (inBounds( centerY + radius, horizontalSearchY))
			{
				if (grid[ centerX + radius, horizontalSearchY].walkable)
				{
					return grid[ centerX + radius, horizontalSearchY];
				}
			}

			// left
			if (inBounds( centerY - radius, horizontalSearchY))
			{
				if (grid[ centerX - radius, horizontalSearchY].walkable)
				{
					return grid[ centerX - radius, horizontalSearchY];
				}
			}

		}

		return null;

	}

	bool inBounds(int x, int y)
	{
		return x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY;
	}

	//Used for debugging reasons
	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, new Vector2(gridWorldSize.x, gridWorldSize.y));
		if (grid != null && displayGridGizmos)
		{
			foreach (Node n in grid)
			{
				Gizmos.color = Color.red;
				if (n.walkable)
					Gizmos.color = Color.white;

				Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
			}
		}
	}
}