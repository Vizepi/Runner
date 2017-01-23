using UnityEngine;

public class Obstacle : MonoBehaviour
{
	[SerializeField]
	private Rigidbody parent = null;
	[SerializeField]
	private Vector3 chockIntensity = new Vector3(500.0f, 100.0f, -300.0f);
	[SerializeField]
	private int penality = 1;

	private bool collided = false;
	private bool colliding = false;

	void Start()
	{
		Debug.Assert(parent != null);
	}
	
	void FixedUpdate()
	{
		if(colliding)
		{
			collided = true;
			colliding = false;
			parent.AddForce(chockIntensity, ForceMode.Impulse);
			parent.AddTorque(0.0f, -45.0f, 40.0f, ForceMode.Impulse);
		}
	}

	public bool Collide(ref int down)
	{
		if(!collided && !colliding)
		{
			colliding = true;
			down = penality;
			return true;
		}
		return false;
	}
}
