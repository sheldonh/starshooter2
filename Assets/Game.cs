using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {

    int score;
    public int winScore;
    public Text scoreBoard;
    public Text controls;
    public Text help;
    public Text heading;
    public GameObject ship;
    public GameObject asteroidField;
    public GameObject pathMaker;
    public AudioClip gameWonSound;
    public GameObject shipPrefab;

    enum State { NewGame, Playing, Paused, GameOver, GameWon };
    State state;

    // Use this for initialization
    void Start () {
        SetStateNewGame();
	}
	
	// Update is called once per frame
	void Update () {
        switch (state)
        {
            case State.Playing:
                if (Input.GetKeyUp("space"))
                    SetStatePaused();
                break;
            case State.NewGame:
            case State.GameOver:
            case State.GameWon:
                if (Input.GetKeyUp("space"))
                {
                    score = 0;
                    if (ship != null)
                        Destroy(ship);
                    ship = Instantiate<GameObject>(shipPrefab);
                    ship.GetComponent<Ship>().game = this;
                    asteroidField.GetComponent<AsteroidField>().ship = ship;
                    pathMaker.GetComponent<PathMaker>().ship = ship;
                    asteroidField.GetComponent<AsteroidField>().Reset();
                    SetStatePlaying();
                }
                else if (Input.GetKeyUp("escape"))
                {
                    Application.Quit();
                }
                break;
            case State.Paused:
                if (Input.GetKeyUp("space"))
                    SetStatePlaying();
                else if (Input.GetKeyUp("escape"))
                {
                    Application.Quit();
                }
                break;
        }
        scoreBoard.text = score.ToString() + "/" + winScore.ToString();
    }

    public bool ScoreUp(int scoreUp)
    {
        if (state == State.Playing)
        {
            score += scoreUp;
            if (score >= winScore)
            {
                SetStateGameWon();
                return true;
            }
        }
        return false;
    }

    void SetStateNewGame ()
    {
        state = State.NewGame;
        //ship.GetComponent<Pausible>().Pause();
        //asteroidField.GetComponent<AsteroidField>().Pause();

        scoreBoard.enabled = false;
        controls.enabled = true;

        help.text = "Press SPACE to start or ESC to quit.";
        help.enabled = true;
        heading.text = "NEW GAME";
        heading.enabled = true;
    }

    void SetStatePlaying ()
    {
        state = State.Playing;
        controls.enabled = false;

        ship.GetComponent<Pausible>().Resume();
        asteroidField.GetComponent<AsteroidField>().Resume();
        scoreBoard.enabled = true;
        help.enabled = false;
        heading.enabled = false;
    }

    void SetStatePaused ()
    {
        state = State.Paused;
        ship.GetComponent<Pausible>().Pause();
        asteroidField.GetComponent<AsteroidField>().Pause();

        scoreBoard.enabled = true;
        controls.enabled = true;

        help.text = "Press SPACE to resume or ESC to quit.";
        help.enabled = true;
        heading.text = "PAUSED";
        heading.enabled = true;
    }

    public void SetStateGameOver ()
    {
        state = State.GameOver;

        if (ship != null)
            Destroy(ship);

        scoreBoard.enabled = true;
        controls.enabled = true;

        help.text = "Press SPACE to restart or ESC to quit.";
        help.enabled = true;
        heading.text = "GAME OVER";
        heading.enabled = true;
    }

    public void SetStateGameWon()
    {
        state = State.GameWon;

        ship.GetComponent<Pausible>().Pause();
        asteroidField.GetComponent<AsteroidField>().Pause();

        scoreBoard.enabled = true;
        controls.enabled = true;

        help.text = "Press SPACE to play again or ESC to quit.";
        help.enabled = true;
        heading.text = "YOU WON";
        heading.enabled = true;
        GetComponent<AudioSource>().PlayOneShot(gameWonSound);
    }
}
