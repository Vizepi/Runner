using UnityEngine;

public class ZombieFollow : MonoBehaviour
{
	[SerializeField]
	private Transform cam;

	private Vector3 distCam;
	private bool dead = false;
	private float deadTimer;

	void Start()
	{
		distCam = transform.position - cam.position;
	}
	
	void Update()
	{
		if (!dead)
		{
			transform.position = cam.position + distCam;
		}
		else
		{
			deadTimer += Time.deltaTime;
			float position = cam.position.x + distCam.x + Mathf.Clamp(deadTimer * 12.0f, 0.0f, Mathf.Abs(distCam.x));
			transform.position = new Vector3(position, transform.position.y, transform.position.z);
		}
	}

	public void SetDead()
	{
		dead = true;
		deadTimer = 0.0f;
	}
}
