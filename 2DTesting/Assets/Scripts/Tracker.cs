using UnityEngine;
using System.Collections;

//From https://www.youtube.com/playlist?list=PLFt_AvWsXl0cq5Umv3pMC9SPnKjfp9eGW
public class Tracker : MonoBehaviour {
	
	public Transform target;
	private float speed = 20;

	public bool running = false;

	private Vector2[] path;
	private int targetIndex;
	private int buffer = 10;

	public void setSpeed(float speed)
    {
		this.speed = speed;
    }

	public void setBuffer(int buffer)
	{
		this.buffer = buffer;
	}

	void Start() {
		//StartCoroutine(refreshPath());
	}

	public void startFollowing()
    {
		running = true;
		StartCoroutine(refreshPath());
	}

	public void stopFollowing()
	{
		running = false;
		StopCoroutine(refreshPath());
	}

	public IEnumerator refreshPath() {
		Vector2 targetPositionOld = (Vector2)target.position + Vector2.up; // ensure not target.position initially
		while (true) {
			if (targetPositionOld != (Vector2)target.position) {
				targetPositionOld = (Vector2)target.position;

				path = Pathfinding.requestPath(transform.position, target.position, buffer);
				StopCoroutine ("followPath");
				StartCoroutine ("followPath");
			}
			//Yeild is used for iterators 
			yield return new WaitForSeconds (.25f);
		}
	}
		
	public IEnumerator followPath() {
		if (path.Length > 0) {
			targetIndex = 0;
			Vector2 currentWaypoint = path [0];

			while (true) {
				if ((Vector2)transform.position == currentWaypoint) {
					targetIndex++;
					if (targetIndex >= path.Length) {
						yield break;
					}
					currentWaypoint = path [targetIndex];
				}

				transform.position = Vector2.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
				yield return null;

			}
		}
	}

	public void OnDrawGizmos() {
		if (path != null) {
			for (int i = targetIndex; i < path.Length; i ++) {
				Gizmos.color = Color.black;
				//Gizmos.DrawCube((Vector3)path[i], Vector3.one *.5f);

				if (i == targetIndex) {
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else {
					Gizmos.DrawLine(path[i-1],path[i]);
				}
			}
		}
	}
}
