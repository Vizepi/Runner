using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
	[SerializeField]
	private GameObject[] obstacles = null;
	[SerializeField]
	private Transform[] spawns = null;
	[SerializeField]
	private GameObject[] buildings = null;
	[SerializeField]
	private Transform buildingPosition = null;
	[SerializeField]
	private uint minQuantity = 10;
	[SerializeField]
	private uint maxQuantity = 30;

	private List<GameObject> instances;

	void Start()
	{
		Debug.Assert(obstacles != null && obstacles.Length > 0);
		Debug.Assert(spawns != null && spawns.Length > 0);
		Debug.Assert(buildings != null && buildings.Length > 0);
		Debug.Assert(buildingPosition != null);
	}

	int Rand(int size)
	{
		return (int)(size * Random.value * 0.999999f);
	}

	public void Initialize()
	{
		maxQuantity = (uint)Mathf.Min(maxQuantity, spawns.Length);
		minQuantity = (uint)Mathf.Min(minQuantity, maxQuantity);
		if(instances != null)
		{
			foreach(GameObject go in instances)
			{
				go.SetActive(false);
				Destroy(go);
			}
		}
		instances = new List<GameObject>();

		// Get all transform positions
		List<Transform> remainingTransforms = new List<Transform>(spawns);
		uint spawnCount = (uint)(minQuantity + Rand((int)maxQuantity - (int)minQuantity + 1));
		for(uint i = 0; i < spawnCount; ++i)
		{
			// Choose a transform and remove it from free ones
			int rand = Rand(remainingTransforms.Count);
			Transform current = remainingTransforms[rand];
			remainingTransforms.Remove(current);

			// Instantiate an obstacle at the position
			GameObject go = Instantiate(obstacles[Rand(obstacles.Length)]);
			go.transform.position = current.position;
			instances.Add(go);

		}

		// Instantiate building
		GameObject building = Instantiate(buildings[Rand(buildings.Length)]);
		building.transform.position = buildingPosition.position;
		instances.Add(building);
	}
}
