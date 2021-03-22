using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//From https://www.youtube.com/playlist?list=PLFt_AvWsXl0cq5Umv3pMC9SPnKjfp9eGW
public class NavGrid : MonoBehaviour {

	public bool displayGridGizmos;

	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;

	Node[,] grid; //2D array
	float nodeDiameter;
	int gridSizeX, gridSizeY;

	void Awake() {
		nodeDiameter = nodeRadius*2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);

		createGrid();
	}

	//This is kinda cool that you can do this
	public int maxSize {
		get {
			return gridSizeX * gridSizeY;
		}
	}

	void createGrid() {
		grid = new Node[gridSizeX,gridSizeY];
		Vector2 worldBottomLeft = (Vector2)transform.position - Vector2.right * gridWorldSize.x/2 - Vector2.up * gridWorldSize.y/2;

		for (int x = 0; x < gridSizeX; x ++) {
			for (int y = 0; y < gridSizeY; y ++) {
				Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);
				bool walkable = (Physics2D.OverlapCircle(worldPoint,nodeRadius,unwalkableMask) == null); // if no collider2D is returned by overlap circle, then this node is walkable

				grid[x,y] = new Node(walkable,worldPoint, x,y);
			}
		}
	}
	

	public List<Node> getNeighbours(Node node, int depth = 1) {
		List<Node> neighbours = new List<Node>();

		for (int x = -depth; x <= depth; x++) {
			for (int y = -depth; y <= depth; y++) {
				if (x == 0 && y == 0)
					continue;

				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
					neighbours.Add(grid[checkX,checkY]);
				}
			}
		}

		return neighbours;
	}
	

	public Node nodeFromWorldPoint(Vector2 worldPosition) {
		float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (worldPosition.y + gridWorldSize.y/2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
		return grid[x,y];
	}

	public Node closestWalkableNode(Node node) {
		int maxRadius = Mathf.Max (gridSizeX, gridSizeY) / 2;
		for (int i = 1; i < maxRadius; i++) {
			Node n = findWalkableInRadius (node.gridX, node.gridY, i);
			if (n != null) {
				return n;

			}
		}
		return null;
	}
	Node findWalkableInRadius(int centreX, int centreY, int radius) {

		for (int i = -radius; i <= radius; i ++) {
			int verticalSearchX = i + centreX;
			int horizontalSearchY = i + centreY;

			// top
			if (inBounds(verticalSearchX, centreY + radius)) {
				if (grid[verticalSearchX, centreY + radius].walkable) {
					return grid [verticalSearchX, centreY + radius];
				}
			}

			// bottom
			if (inBounds(verticalSearchX, centreY - radius)) {
				if (grid[verticalSearchX, centreY - radius].walkable) {
					return grid [verticalSearchX, centreY - radius];
				}
			}
			// right
			if (inBounds(centreY + radius, horizontalSearchY)) {
				if (grid[centreX + radius, horizontalSearchY].walkable) {
					return grid [centreX + radius, horizontalSearchY];
				}
			}

			// left
			if (inBounds(centreY - radius, horizontalSearchY)) {
				if (grid[centreX - radius, horizontalSearchY].walkable) {
					return grid [centreX - radius, horizontalSearchY];
				}
			}

		}

		return null;

	}

	bool inBounds(int x, int y) {
		return x>=0 && x<gridSizeX && y>= 0 && y<gridSizeY;
	}
	
	void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position,new Vector2(gridWorldSize.x,gridWorldSize.y));
		if (grid != null && displayGridGizmos) {
			foreach (Node n in grid) {
				Gizmos.color = Color.red;
				if (n.walkable)
					Gizmos.color = Color.white;

				Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter-.1f));
			}
		}
	}

}