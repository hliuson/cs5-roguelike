using UnityEngine;
using System.Collections;

//From https://www.youtube.com/playlist?list=PLFt_AvWsXl0cq5Umv3pMC9SPnKjfp9eGW
public class Node : IHeapItem<Node> {

	public bool walkable;
	public Vector2 worldPosition;
	public int gridX;
	public int gridY;
	
	//General Cost
	public int gCost;
	//Heuristic Cost
	public int hCost;
	public Node parent;
	int heapIndex;


	//Constructor
	public Node(bool isWalkable, Vector2 newWorldPos, int gridPosX, int gridPosY) {
		walkable = isWalkable;
		worldPosition = newWorldPos;
		gridX = gridPosX;
		gridY = gridPosY;
	}

	public int fCost {
		get {
			return gCost + hCost;
		}
	}

	public int HeapIndex {
		get {
			return heapIndex;
		}
		set {
			heapIndex = value;
		}
	}

	public int CompareTo(Node nodeToCompare) {
		int compare = fCost.CompareTo(nodeToCompare.fCost);
		if (compare == 0) {
			compare = hCost.CompareTo(nodeToCompare.hCost);
		}
		return -compare;
	}
}
