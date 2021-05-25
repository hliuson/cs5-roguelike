using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;

//From https://www.youtube.com/playlist?list=PLFt_AvWsXl0cq5Umv3pMC9SPnKjfp9eGW
public class Pathfinding : MonoBehaviour {

	NavGrid grid;
	static Pathfinding instance;
	
	void Awake() {
		grid = GetComponent<NavGrid>();
		instance = this;
	}

	public static Vector2[] requestPath(Vector2 from, Vector2 to, int buffer) {
		return instance.findPath (from, to, buffer);
	}
	
	Vector2[] findPath(Vector2 from, Vector2 to, int buffer) {
		
		
		Vector2[] waypoints = new Vector2[0];
		bool pathSuccess = false;
		
		Node startNode = grid.nodeFromWorldPoint(from);
		Node targetNode = grid.nodeFromWorldPoint(to);
		List<Node> targetNodeArea = grid.getNeighbours(targetNode, buffer);
		targetNodeArea.Add(targetNode);
		targetNode = nodeInList(startNode, targetNodeArea);
		startNode.parent = startNode;

		if (!startNode.walkable) {
			startNode = grid.closestWalkableNode (startNode);
		}
		if (!targetNode.walkable) {
			targetNode = grid.closestWalkableNode(targetNode);
		}
		
		if (startNode.walkable && targetNode.walkable) {
			
			Heap<Node> openSet = new Heap<Node>(grid.maxSize);
			HashSet<Node> closedSet = new HashSet<Node>();
			openSet.Add(startNode);
			
			while (openSet.Count > 0) {
				Node currentNode = openSet.RemoveFirst();
				closedSet.Add(currentNode);

				if (currentNode == targetNode) {
					pathSuccess = true;
					break;
				}
				
				foreach (Node neighbour in grid.getNeighbours(currentNode)) {
					if (!neighbour.walkable || closedSet.Contains(neighbour)) {
						continue;
					}
					
					int newMovementCostToNeighbour = currentNode.gCost + getDistance(currentNode, neighbour)+turningCost(currentNode,neighbour);
					if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
						neighbour.gCost = newMovementCostToNeighbour;
						neighbour.hCost = getDistance(neighbour, targetNode);
						neighbour.parent = currentNode;
						
						if (!openSet.Contains(neighbour))
							openSet.Add(neighbour);
						else 
							openSet.UpdateItem(neighbour);
					}
				}
			}
		}

		if (pathSuccess) {
			waypoints = retracePath(startNode,targetNode);
		}

		return waypoints;
		
	}

	Node nodeInList(Node currentNode, List<Node> targetNodes)
    {
		Node closestNode = null;
		int minDistance = int.MaxValue;
		foreach(Node n in targetNodes)
        {
			int distance = getDistance(currentNode, n);
			if (distance < minDistance)
			{
				closestNode = n;
				minDistance = distance;
			}
		}
		return closestNode;
    }


	int turningCost(Node from, Node to) {
		//Don't think this is needed
		/*
		Vector2 dirOld = new Vector2(from.gridX - from.parent.gridX, from.gridY - from.parent.gridY);
		Vector2 dirNew = new Vector2(to.gridX - from.gridX, to.gridY - from.gridY);
		if (dirNew == dirOld)
			return 0;
		else if (dirOld.x != 0 && dirOld.y != 0 && dirNew.x != 0 && dirNew.y != 0) {
			return 5;
		}
		else {
			return 10;
		}
		*/

		return 0;
	}
	
	Vector2[] retracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node>();
		Node currentNode = endNode;
		
		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		Vector2[] waypoints = simplifyPath(path);
		Array.Reverse(waypoints);
		return waypoints;
		
	}
	
	Vector2[] simplifyPath(List<Node> path) {
		List<Vector2> waypoints = new List<Vector2>();
		Vector2 directionOld = Vector2.zero;
		
		for (int i = 1; i < path.Count; i ++) {
			Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX,path[i-1].gridY - path[i].gridY);
			if (directionNew != directionOld) {
				waypoints.Add(path[i-1].worldPosition);
			}
			directionOld = directionNew;
		}
		return waypoints.ToArray();
	}
	
	int getDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
		
		if (dstX > dstY)
			return 14*dstY + 10* (dstX-dstY);
		return 14*dstX + 10 * (dstY-dstX);
	}
}
