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

	// Player
	private PlayerController player = null;
	private CameraController cam = null;

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
		//TODO load highscore
		highscore = (uint)PlayerPrefs.GetInt("highscore", 0);
		Initialize();
		state = State.SPLASHS;
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
		Debug.Assert(streets[0] != null);
		Debug.Assert(streets[1] != null);

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
		textHigh.text = "HIGH:   " + highscore;
		textHighscore.text = "HIGHSCORE: " + score;
		textSession.text = "SESSION BEST: " + sessionscore;
		score = 0;

		// Initialize streets
		streets[0].Initialize();

		// Initialize state
		state = State.MAINMENU;
	}

	void Update()
	{
		switch (state)
		{
			case State.SPLASHS:
				{
					state = State.TRANSITION_SPLASHS_MAINMENU;
				}
				break;
			case State.MAINMENU:
				{
					textPlay.color = new Color(1.0f, 1.0f, 1.0f, 0.6f + 0.4f * Mathf.Sin(Time.time * 5.0f));
					if (Input.GetKeyDown(KeyCode.Return))
					{
						state = State.TRANSITION_MAINMENU_INGAME;
					}
					else if(Input.GetKeyDown(KeyCode.Escape))
					{
						Application.Quit();
					}
				}
				break;
			case State.CREDITS:
				{
					state = State.TRANSITION_CREDITS_MAINMENU;
				}
				break;
			case State.INGAME:
				{
					score = (uint)Mathf.FloorToInt(Mathf.Max(0.0f, player.transform.position.x));
					textScore.text = "SCORE: " + score;
					if (score > highscore)
					{
						highscore = score;
						textHigh.text = "HIGH:   " + score;
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
						state = State.TRANSITION_INGAME_GAMEOVER;
					}
				}
				break;
			case State.GAMEOVER:
				{
					textReset.color = new Color(1.0f, 1.0f, 1.0f, 0.6f + 0.4f * Mathf.Sin(Time.time * 5.0f));
					if (Input.GetKeyDown(KeyCode.Return))
					{
						state = State.TRANSITION_GAMEOVER_MAINMENU;
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

					state = State.MAINMENU;
				}
				break;
			case State.TRANSITION_MAINMENU_INGAME:
				{
					textTitle.color =
					textHighscore.color =
					textSession.color =
					textPlay.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

					textScore.color = textHigh.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
					player.Play();
					streets[0].Initialize();

					state = State.INGAME;
				}
				break;
			case State.TRANSITION_INGAME_GAMEOVER:
				{
					player.Stop();
					textGameover.color = new Color(218.0f / 255.0f, 0.0f, 0.0f, 1.0f);
					PlayerPrefs.SetInt("highscore", (int)highscore);
					if(score > sessionscore)
					{
						sessionscore = score;
					}
					state = State.GAMEOVER;
				}
				break;
			case State.TRANSITION_GAMEOVER_MAINMENU:
				{
					textReset.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
					SceneManager.LoadScene("Game");
					state = State.MAINMENU;
				}
				break;
			case State.TRANSITION_MAINMENU_CREDITS:
				{
					state = State.CREDITS;
				}
				break;
			case State.TRANSITION_CREDITS_MAINMENU:
				{
					state = State.MAINMENU;
				}
				break;
		}
	}
}
