using UnityEngine;

public class Zombie : MonoBehaviour
{
	[SerializeField]
	private Animator anim = null;
	[SerializeField]
	private ZombieFollow group = null;
	[SerializeField]
	private Transform player = null;

	private float deadTimer = 0.0f;

	void Start()
	{
		Debug.Assert(anim != null);
		Debug.Assert(group != null);
		Debug.Assert(player != null);
		anim.SetFloat("offset", Random.value);
		float s = 0.8f + Random.value * 0.3f;
		transform.localScale = new Vector3(s, s, s);
	}
	
	void Update()
	{
		if(group.IsDead(ref deadTimer))
		{
			transform.LookAt(player.position);
			Vector3 r = transform.rotation.eulerAngles;
			r.x = 0.0f;
			r.z = 10.348f;
			transform.rotation = Quaternion.Euler(r);
			if (deadTimer >= 0.5f)
			{
				anim.SetTrigger("eat");
				transform.position = new Vector3(transform.position.x, -1.62f * Mathf.Clamp((deadTimer - 0.5f) * 2.0f, 0.0f, 1.0f), transform.position.z);
			}
		}
	}
}
