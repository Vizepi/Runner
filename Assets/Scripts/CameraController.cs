using UnityEngine;

public class CameraController : MonoBehaviour
{

	[SerializeField]
	private Transform target = null;
	[SerializeField]
	private float offset = 0.0f;
	[SerializeField]
	private float offsetChangeSpeed = 1.0f;
	[SerializeField]
	private float offsetStep = 4.0f;

	private float previousOffset;
	private float offsetTimer;
	private bool changingOffset;
	private bool dead = false;

	void Start()
	{
		Debug.Assert(target != null);
	}
	
	void Update()
	{
		if (changingOffset)
		{
			offsetTimer += Time.deltaTime;
			float x = offsetTimer / offsetChangeSpeed;
			x = 1.0f - x * x;
			transform.position = new Vector3(
				target.position.x + previousOffset + (offset - previousOffset) * (1.0f - x * x * x), 
				transform.position.y, 
				transform.position.z);
			if(offsetTimer >= offsetChangeSpeed)
			{
				changingOffset = false;
			}
		}
		else
		{
			transform.position = new Vector3(
				target.position.x + offset, 
				transform.position.y, 
				transform.position.z);
		}
	}

	public void SetOffset(float o)
	{
		previousOffset = offset;
		offset = o;
		offsetTimer = 0.0f;
		changingOffset = true;
	}

	public void DownOffset(int count = 1)
	{
		Debug.Log("A");
		previousOffset = offset;
		for (int i = 0; i < count; ++i)
		{
			Debug.Log("B");
			if (offset < 3 * offsetStep)
			{
				Debug.Log("C");
				offset += offsetStep;
				offsetTimer = 0.0f;
				changingOffset = true;
			}
			else
			{
				Debug.Log("D");
				dead = true;
				break;
			}
		}
	}

	public bool IsDead()
	{
		return dead;
	}

	public void UpOffset(int count = 1)
	{
				previousOffset = offset;
		for (int i = 0; i < count; ++i)
		{
			if (offset > 0.0f)
			{
				offset -= offsetStep;
				offsetTimer = 0.0f;
				changingOffset = true;
			}
		}
	}
}
