using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
	enum State
	{
		RUNNING, JUMPING, ROLLING
	};

	[SerializeField]
	private float velocity;
	[SerializeField]
	private float jumpHeight;
	[SerializeField]
	private float rollCorrection;
	[SerializeField]
	private float jumpDuration = 0.8f;
	[SerializeField]
	private float rollDuration = 1.0f;

	private Animator animator;

	private int animatorJumpTrigger;
	private int animatorRollTrigger;
	private int animatorRunTrigger;

	private State state = State.RUNNING;

	private float stateTimer;
	private bool running = false;

	void Start()
	{
		animator = GetComponent<Animator>();
		animatorJumpTrigger = Animator.StringToHash("jump");
		animatorRollTrigger = Animator.StringToHash("roll");
		transform.position = new Vector3(0.0f, -0.2f, 0.0f);
	}

	public void Play()
	{
		animator.SetTrigger("run");
		running = true;
		transform.position = new Vector3(0.0f, 0.0f, 0.0f);
	}

	public void Stop()
	{
		animator.SetTrigger("die");
		running = false;
		transform.position = new Vector3(transform.position.x, -0.2f, 0.0f);
	}

	void Update()
	{
		if (running)
		{
			transform.position += new Vector3(velocity * Time.deltaTime, 0.0f, 0.0f);
			if (state == State.RUNNING)
			{
				if (Input.GetKeyDown(KeyCode.UpArrow))
				{
					animator.SetTrigger(animatorJumpTrigger);
					state = State.JUMPING;
					stateTimer = 0.0f;
				}
				else if (Input.GetKeyDown(KeyCode.DownArrow))
				{
					animator.SetTrigger(animatorRollTrigger);
					state = State.ROLLING;
					stateTimer = 0.0f;
				}
			}
			else
			{
				stateTimer += Time.deltaTime;
				if (state == State.JUMPING)
				{
					if (stateTimer >= jumpDuration)
					{
						state = State.RUNNING;
						transform.position = new Vector3(transform.position.x, 0.0f, 0.0f);
					}
					else
					{
						float tmp = 3.0f * stateTimer - 1.0f;
						transform.position = new Vector3(
							transform.position.x,
							jumpHeight * Mathf.Clamp((1.0f - tmp * tmp), 0.0f, 1.0f),
							0.0f);
					}
				}
				else if (state == State.ROLLING)
				{
					if (stateTimer >= rollDuration)
					{
						state = State.RUNNING;
						transform.position = new Vector3(transform.position.x, 0.0f, 0.0f);
					}
					else
					{
						float tmp = 2.8f * stateTimer - 2.0f;
						transform.position = new Vector3(
							transform.position.x,
							rollCorrection * Mathf.Clamp((tmp * tmp - 1.0f), -1.0f, 0.0f),
							0.0f);
					}
				}
			}
		}
	}
}
