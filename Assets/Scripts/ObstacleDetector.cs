using UnityEngine;

public class ObstacleDetector : MonoBehaviour
{
	[SerializeField]
	private CameraController cam = null;
	[SerializeField]
	private float rescueTime = 10.0f;

	private float rescueTimer;

	void Start()
	{
		Debug.Assert(cam != null);
		rescueTimer = 0.0f;
	}
	
	void Update()
	{
		rescueTimer += Time.deltaTime;
		Collider2D collider = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y), 2.5f);
		if(collider != null && collider.gameObject.tag == "Obstacle")
		{
			int penality = 1;
			if(collider.GetComponent<Obstacle>().Collide(ref penality))
			{
				cam.DownOffset(penality);
				rescueTimer = 0.0f;
				collider.enabled = false;
			}
		}
		if(rescueTimer >= rescueTime)
		{
			cam.UpOffset();
			rescueTimer = 0.0f;
		}
	}
}
