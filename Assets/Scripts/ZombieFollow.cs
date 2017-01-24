using UnityEngine;

public class ZombieFollow : MonoBehaviour
{
	[SerializeField]
	private Transform cam;
	[SerializeField]
	private ParticleSystem particles;
	[SerializeField]
	private Transform player;

	private Vector3 distCam;
	private bool dead = false;
	private float deadTimer;
	private float lastPosition = 0.0f;
	private bool started = false;

	void Start()
	{
		distCam = transform.position - cam.position;
		particles.Pause();
	}
	
	void Update()
	{
		if (started)
		{
			if (!dead)
			{
				transform.position = cam.position + distCam;
			}
			else
			{
				deadTimer += Time.deltaTime;
				if (deadTimer > 1.5f)
				{
					particles.Play(true);
				}
				float position = lastPosition + (player.position.x - lastPosition) * Mathf.Clamp(deadTimer * 1.0f, 0.0f, 1.0f);
				transform.position = new Vector3(position, transform.position.y, transform.position.z);
			}
		}
	}

	public void Play()
	{
		started = true;
	}

	public void SetDead()
	{
		dead = true;
		distCam = transform.position - player.position;
		lastPosition = transform.position.x;
		deadTimer = 0.0f;
	}

	public bool IsDead(ref float timer)
	{
		timer = deadTimer;
		return dead;
	}
}
