using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	// UI
	private Text textTitle = null;
	private Text textHighscore = null;
	private Text textHigh = null;
	private Text textScore = null;
	private Text textPlay = null;
	private Text textSession = null;
	private Text textGameover = null;
	private Text textReset = null;
	[SerializeField]
	private Texture fader = null;

	// Player
	private PlayerController player = null;
	private CameraController cam = null;
	private ZombieFollow zombies = null;
	private Transform camStart = null;
	private Transform camGame = null;

	// Streets
	private ObstacleSpawner[] streets;
	private int currentStreet = 0;

	// Score
	private uint score = 0;
	private uint highscore = 0;
	private uint sessionscore = 0;

	// State
	enum State
	{
		SPLASHS,
		TRANSITION_SPLASHS_MAINMENU,
		MAINMENU,
		TRANSITION_MAINMENU_INGAME,
		INGAME,
		TRANSITION_INGAME_GAMEOVER,
		GAMEOVER,
		TRANSITION_GAMEOVER_MAINMENU,
		CREDITS,
		TRANSITION_MAINMENU_CREDITS,
		TRANSITION_CREDITS_MAINMENU
	};
	private State state = State.SPLASHS;
	private float stateTimer;

	// Instance
	static GameController instance = null;

	void Start()
	{
		if (instance != null)
		{
			DestroyImmediate(gameObject);
			instance.Initialize();
			return;
		}
		instance = this;
		DontDestroyOnLoad(this);
		FirstInitialize();
	}

	void FirstInitialize()
	{
		highscore = (uint)PlayerPrefs.GetInt("highscore", 0);
		Initialize();
		SetState(State.SPLASHS);
	}

	void SetState(State s)
	{
		stateTimer = 0.0f;
		state = s;
	}

	void Initialize()
	{
		// Get objects
		GameObject tmp = GameObject.Find("Title");
		if (tmp != null)
		{
			textTitle = tmp.GetComponent<Text>();
		}
		tmp = GameObject.Find("Highscore");
		if (tmp != null)
		{
			textHighscore = tmp.GetComponent<Text>();
		}
		tmp = GameObject.Find("High");
		if (tmp != null)
		{
			textHigh = tmp.GetComponent<Text>();
		}
		tmp = GameObject.Find("Score");
		if (tmp != null)
		{
			textScore = tmp.GetComponent<Text>();
		}
		tmp = GameObject.Find("Play");
		if (tmp != null)
		{
			textPlay = tmp.GetComponent<Text>();
		}
		tmp = GameObject.Find("Session");
		if (tmp != null)
		{
			textSession = tmp.GetComponent<Text>();
		}
		tmp = GameObject.Find("Gameover");
		if (tmp != null)
		{
			textGameover = tmp.GetComponent<Text>();
		}
		tmp = GameObject.Find("Reset");
		if (tmp != null)
		{
			textReset = tmp.GetComponent<Text>();
		}
		tmp = GameObject.Find("Player");
		if (tmp != null)
		{
			player = tmp.GetComponent<PlayerController>();
		}
		tmp = GameObject.Find("Camera");
		if (tmp != null)
		{
			cam = tmp.GetComponent<CameraController>();
		}
		tmp = GameObject.Find("Zombies");
		if (tmp != null)
		{
			zombies = tmp.GetComponent<ZombieFollow>();
		}
		tmp = GameObject.Find("CameraSpawn");
		if (tmp != null)
		{
			camGame = tmp.GetComponent<Transform>();
		}
		tmp = GameObject.Find("CameraMenu");
		if (tmp != null)
		{
			camStart = tmp.GetComponent<Transform>();
		}
		streets = new ObstacleSpawner[2];
		tmp = GameObject.Find("StreetA");
		if (tmp != null)
		{
			streets[0] = tmp.GetComponent<ObstacleSpawner>();
		}
		tmp = GameObject.Find("StreetB");
		if (tmp != null)
		{
			streets[1] = tmp.GetComponent<ObstacleSpawner>();
		}

		// Check objects found
		Debug.Assert(textTitle != null);
		Debug.Assert(textHighscore != null);
		Debug.Assert(textHigh != null);
		Debug.Assert(textScore != null);
		Debug.Assert(textPlay != null);
		Debug.Assert(textSession != null);
		Debug.Assert(textGameover != null);
		Debug.Assert(textReset != null);
		Debug.Assert(player != null);
		Debug.Assert(cam != null);
		Debug.Assert(zombies != null);
		Debug.Assert(camStart != null);
		Debug.Assert(camGame != null);
		Debug.Assert(streets[0] != null);
		Debug.Assert(streets[1] != null);
		Debug.Assert(fader != null);

		// Initialize Canvas
		textTitle.color = new Color(161.0f / 255.0f, 0.0f, 0.0f, 1.0f);
		textHighscore.color =
		textSession.color =
		textPlay.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

		textScore.color = 
		textHigh.color = 
		textGameover.color = 
		textReset.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

		// Initialize UI
		textHigh.text = "HIGH:   " + IntToString(highscore);
		textHighscore.text = "HIGHSCORE: " + IntToString(highscore);
		textSession.text = "SESSION BEST: " + IntToString(sessionscore);
		score = 0;

		// Initialize streets
		streets[0].Initialize();
		cam.transform.position = camStart.position;
		cam.transform.rotation  = camStart.rotation;

		// Initialize state
		SetState(State.MAINMENU);
	}

	string IntToString(uint i)
	{
		string s = "";
		int count = 0;
		do
		{
			s = "" + (i % 10) + s;
			i /= 10;
			count++;
			if (count % 3 == 0 && i != 0)
			{
				s = ' ' + s;
			}
		} while (i != 0);
		return s;
	}

	void OnGUI()
	{
		switch(state)
		{
			case State.SPLASHS:
				break;
			case State.TRANSITION_SPLASHS_MAINMENU:
				GUI.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Clamp(2.0f - stateTimer, 0.0f, 1.0f));
				GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fader);
				break;
		}
	}

	void Update()
	{
		switch (state)
		{
			case State.SPLASHS:
				{
					SetState(State.TRANSITION_SPLASHS_MAINMENU);
				}
				break;
			case State.MAINMENU:
				{
					textPlay.color = new Color(1.0f, 1.0f, 1.0f, 0.6f + 0.4f * Mathf.Sin(Time.time * 5.0f));
					cam.transform.position = camStart.position;
					cam.transform.rotation = camStart.rotation;
					if (Input.GetKeyDown(KeyCode.Return))
					{
						SetState(State.TRANSITION_MAINMENU_INGAME);
					}
					else if(Input.GetKeyDown(KeyCode.Escape))
					{
						Application.Quit();
					}
				}
				break;
			case State.CREDITS:
				{
					SetState(State.TRANSITION_CREDITS_MAINMENU);
				}
				break;
			case State.INGAME:
				{
					score = (uint)Mathf.FloorToInt(Mathf.Max(0.0f, player.transform.position.x));
					textScore.text = "SCORE: " + IntToString(score);
					if (score > highscore)
					{
						highscore = score;
						textHigh.text = "HIGH:   " + IntToString(score);
					}
					if(score > streets[currentStreet].transform.position.x + 45.0f)
					{
						int newStreet = (currentStreet + 1) % 2;
						streets[newStreet].transform.position =
							streets[currentStreet].transform.position +
							new Vector3(180.0f, 0.0f, 0.0f);
						streets[newStreet].Initialize();
						currentStreet = newStreet;
					}
					if (cam.IsDead())
					{
						SetState(State.TRANSITION_INGAME_GAMEOVER);
					}
				}
				break;
			case State.GAMEOVER:
				{
					textReset.color = new Color(1.0f, 1.0f, 1.0f, 0.6f + 0.4f * Mathf.Sin(Time.time * 5.0f));
					if (Input.GetKeyDown(KeyCode.Return))
					{
						SetState(State.TRANSITION_GAMEOVER_MAINMENU);
					}
				}
				break;
			case State.TRANSITION_SPLASHS_MAINMENU:
				{
					textTitle.color = new Color(161.0f / 255.0f, 0.0f, 0.0f, 1.0f);
					textHighscore.color =
					textSession.color =
					textPlay.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

					textScore.color =
					textHigh.color =
					textGameover.color =
					textReset.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

					cam.transform.position = camStart.position;
					cam.transform.rotation = camStart.rotation;

					stateTimer += Time.deltaTime;
					if (stateTimer >= 2.0f)
					{
						SetState(State.MAINMENU);
					}
				}
				break;
			case State.TRANSITION_MAINMENU_INGAME:
				{
					if (stateTimer == 0.0f)
					{
						textTitle.color =
						textHighscore.color =
						textSession.color =
						textPlay.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

						textScore.color = textHigh.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

						camStart.GetComponent<Looker>().enabled = false;
					}

					stateTimer += Time.deltaTime;
					cam.transform.position = Vector3.Lerp(camStart.position, camGame.position, stateTimer / 2.0f);
					cam.transform.rotation = Quaternion.Slerp(camStart.rotation, camGame.rotation, stateTimer / 2.0f);
					if(stateTimer >= 2.0f)
					{
						player.Play();
						zombies.Play();
						streets[0].Initialize();
						cam.transform.position = camGame.position;
						cam.transform.rotation = camGame.rotation;
						SetState(State.INGAME);
						cam.Play();
					}
				}
				break;
			case State.TRANSITION_INGAME_GAMEOVER:
				{
					player.Stop();
					zombies.SetDead();
					textGameover.color = new Color(218.0f / 255.0f, 0.0f, 0.0f, 1.0f);
					PlayerPrefs.SetInt("highscore", (int)highscore);
					if(score > sessionscore)
					{
						sessionscore = score;
					}
					SetState(State.GAMEOVER);
				}
				break;
			case State.TRANSITION_GAMEOVER_MAINMENU:
				{
					textReset.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
					SceneManager.LoadScene("Game");
					SetState(State.MAINMENU);
				}
				break;
			case State.TRANSITION_MAINMENU_CREDITS:
				{
					SetState(State.CREDITS);
				}
				break;
			case State.TRANSITION_CREDITS_MAINMENU:
				{
					SetState(State.MAINMENU);
				}
				break;
		}
	}
}
