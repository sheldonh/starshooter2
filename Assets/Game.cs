using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {

    int score;
    public int winScore;
    public Text scoreBoard;
    public Text help;
    public Text heading;
    public GameObject ship;
    public GameObject asteroidField;

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
                    ship.GetComponent<Ship>().Reset();
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
        Debug.Log("Game state: NewGame");
        state = State.NewGame;
        ship.GetComponent<Pausible>().Pause();
        //asteroidField.GetComponent<AsteroidField>().Pause();

        scoreBoard.enabled = false;

        help.text = "Press SPACE to start or ESC to quit.";
        help.enabled = true;
        heading.text = "NEW GAME";
        heading.enabled = true;
    }

    void SetStatePlaying ()
    {
        Debug.Log("Game state: Playing");
        state = State.Playing;
        ship.GetComponent<Pausible>().Resume();
        asteroidField.GetComponent<AsteroidField>().Resume();
        scoreBoard.enabled = true;
        help.enabled = false;
        heading.enabled = false;
    }

    void SetStatePaused ()
    {
        Debug.Log("Game state: Paused");
        state = State.Paused;
        ship.GetComponent<Pausible>().Pause();
        asteroidField.GetComponent<AsteroidField>().Pause();

        scoreBoard.enabled = true;
        help.text = "Press SPACE to resume or ESC to quit.";
        help.enabled = true;
        heading.text = "PAUSED";
        heading.enabled = true;
    }

    public void SetStateGameOver ()
    {
        Debug.Log("Game state: GameOver");
        state = State.GameOver;

        //ship.GetComponent<Pausible>().Pause();
        //asteroidField.GetComponent<AsteroidField>().Pause();
        scoreBoard.enabled = true;
        help.text = "Press SPACE to restart or ESC to quit.";
        help.enabled = true;
        heading.text = "GAME OVER";
        heading.enabled = true;
    }

    public void SetStateGameWon()
    {
        Debug.Log("Game state: GameWon");
        state = State.GameWon;

        ship.GetComponent<Pausible>().Pause();
        asteroidField.GetComponent<AsteroidField>().Pause();
        scoreBoard.enabled = true;
        help.text = "Press SPACE to play again or ESC to quit.";
        help.enabled = true;
        heading.text = "YOU WON";
        heading.enabled = true;
    }
}
