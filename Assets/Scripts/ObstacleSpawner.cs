using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour {

	[SerializeField] private float waitTime;
    //[SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private ObstacleBehaviour obstaclePrefab;
	[SerializeField] private float minObstacleY;
	[SerializeField] private float maxObstacleY;
	[SerializeField] private float highDifference;
	[SerializeField] private int easy;
	[SerializeField] private int hard;
	private float tempTime;
	private ObstacleBehaviour pipeClone;
	private int differenceIndex;

	void Start(){
		tempTime = waitTime - Time.deltaTime;
	}

	void LateUpdate () {
		if(GameManager.Instance.GameState()){
			tempTime += Time.deltaTime;
			if(tempTime > waitTime){
				// Wait for some time, create an obstacle, then set wait time to 0 and start again
				tempTime = 0;
				float posY = GetRandomPosY();
				pipeClone = Instantiate(obstaclePrefab, transform.position + new Vector3(0f, posY), transform.rotation);

			}
		}
	}

	private float GetRandomPosY()
	{
		differenceIndex++;
		if (pipeClone == null)
		{
			return Random.Range(minObstacleY, maxObstacleY);
		}

		if (differenceIndex > easy)		//easy position: index > 0, hard position: index <= 0
        {
			differenceIndex = -hard + 1;
        }

		float pipeY = pipeClone.transform.position.y;
		if (differenceIndex > 0)
        {
			float minY = pipeY - highDifference;
			if (minY < minObstacleY)
			{
				minY = minObstacleY;
			}

			float maxY = pipeY + highDifference;
			if (maxY > maxObstacleY)
            {
				maxY = maxObstacleY;
            }
			return Random.Range(minY, maxY);
		}
		else
        {
			float min1Y = minObstacleY;
			float max1Y = pipeY - highDifference;
			float min2Y = pipeY + highDifference;
			float max2Y = maxObstacleY;

			if (max1Y < min1Y)
            {
				max1Y = min1Y;
			}
			if (min2Y > max2Y)
            {
				min2Y = max2Y;
			}
			if (min1Y == max1Y && min2Y == max2Y)
			{
				return Random.Range(minObstacleY, maxObstacleY);
			}
			else if (min1Y == max1Y)
			{
				return Random.Range(min2Y, max2Y);
			}
			else if (min2Y == max2Y)
			{
				return Random.Range(min1Y, max1Y);
			}
			return RandomWith2PairMinMax(min1Y, max1Y, min2Y, max2Y);
		}
	}

	private float RandomWith2PairMinMax(float min1, float max1, float min2, float max2)
    {
		float result = Random.Range(min1, max1 + max2 - min2);
		if (result > max1)
        {
			result = result - max1 + min2;
        }
		return result;
    }

	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.transform.parent != null){
			Destroy(col.gameObject.transform.parent.gameObject);
		}else{
			Destroy(col.gameObject);
		}
	}

}
