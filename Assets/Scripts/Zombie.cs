using UnityEngine;

public class Zombie : MonoBehaviour
{
	[SerializeField]
	private Animator anim = null;

	void Start()
	{
		Debug.Assert(anim != null);
		anim.SetFloat("offset", Random.value);
		float s = 0.8f + Random.value * 0.3f;
		transform.localScale = new Vector3(s, s, s);
	}
	
	void Update()
	{
		
	}
}
