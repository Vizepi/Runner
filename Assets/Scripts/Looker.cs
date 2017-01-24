using UnityEngine;

public class Looker : MonoBehaviour
{
	private Vector3 alpha = new Vector3(1.0f, 2.0f, 3.0f);
	private Vector3 frequency = new Vector3(1.2f, 1.0f, 0.8f);
	private Vector3 forward;

	[SerializeField]
	private float amplitude;
	
	void Start()
	{
		forward = transform.forward;
	}
	
	void Update()
	{
		alpha.x += frequency.x * Time.deltaTime;
		alpha.y += frequency.y * Time.deltaTime;
		alpha.z += frequency.z * Time.deltaTime;

		Vector3 sins = new Vector3(Mathf.Sin(alpha.x) * amplitude, Mathf.Sin(alpha.y) * amplitude, Mathf.Sin(alpha.z) * amplitude);

		transform.LookAt(transform.position + forward + sins);
	}
}
