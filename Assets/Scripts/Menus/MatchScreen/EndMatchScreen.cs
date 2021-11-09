using UnityEngine;
using System.Collections;

public class EndMatchScreen : MonoBehaviour {

    private const float WAIT_TIME = 6.25f;

	public GUIText P1Score;
	public GUIText P2Score;
	public GUIText DrawScore;

    private GameManager GM;

	private int _p1Score;	//Number of matches won by player 1
	private int _p2Score;	//Number of matches won by player 2
	private int _drawScore;	//Number of draws between player 1 and 2
	private SpriteRenderer _screenBG;
	private Vector3 _enabledPos;	//Position of screen when match screen is up
	private Vector3 _disabledPos;	//Position of screen when match screen is not up
	private bool _enabled; 			//Whether the match screen is showing or not
	private bool _p1Picked;			//Whether Player 1 has chosen an end match option
	private bool _p2Picked;			//Whether Player 2 has chosen an end match option
	private string _p1SelectedOption;
	private string _p2SelectedOption;
    private float _timer;           //Amount of time before players can choose an end of match option
	
	private MatchOptions _p1MatchOptions;
	private MatchOptions _p2MatchOptions;
    private GameObject _p1Options;
    private GameObject _p2Options;
    private GameObject _p1WinnerText;
    private GameObject _p2WinnerText;
	private Level _level;
	private AudioSource _audio;

	// Use this for initialization
	void Start () 
	{
        if (GameObject.FindGameObjectWithTag("GameManager") != null)
        {
            GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            P1Score.color = GM.P1CharChoice;
            P2Score.color = GM.P2CharChoice;
        }
        else
            Instantiate(Resources.Load("GameManager")); 
        
		_p1Score = 0;
		_p2Score = 0;
		_drawScore = 0;

		_screenBG = transform.Find("ScreenBG").GetComponent<SpriteRenderer>();

		_enabledPos = transform.position;
		_disabledPos = new Vector3 (_screenBG.bounds.extents.x * 2, transform.position.y, transform.position.z);

		transform.position = _disabledPos;
        
        _timer = 0;
		
		_p1MatchOptions = transform.Find("P1Info").GetComponent<MatchOptions>();
		_p2MatchOptions = transform.Find("P2Info").GetComponent<MatchOptions>();
        _p1Options = _p1MatchOptions.transform.Find("Options").gameObject;
        _p2Options = _p2MatchOptions.transform.Find("Options").gameObject;
        
        _p1WinnerText = _p1MatchOptions.transform.Find("WinnerText").gameObject;
        _p2WinnerText = _p2MatchOptions.transform.Find("WinnerText").gameObject;
        _p1WinnerText.SetActive(false);
        _p2WinnerText.SetActive(false);
		
		_level = transform.parent.Find("Level").GetComponent<Level>();
		
		_p1SelectedOption = "";
		_p2SelectedOption = "";
		
		_audio = transform.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (_enabled)
		{
			P1Score.text = _p1Score.ToString();
			P2Score.text = _p2Score.ToString();
			DrawScore.text = _drawScore.ToString();
		
            if (_timer <= 0)
            {
			    PlayerInput();
                _p1Options.gameObject.SetActive(true);
                _p2Options.gameObject.SetActive(true);
            }    
            else
                _timer -= Time.deltaTime;
		}
		
		if (_p1Picked && _p2Picked)
			CheckSelections();
	}

	private void PlayerInput()
	{
		if (!_p1Picked)
		{
			//Player 1 Up input
			if (GM.GetUpDown("1"))
				_p1MatchOptions.SelectUp();
			//Player 1 Down input
			if (GM.GetDownDown("1"))
				_p1MatchOptions.SelectDown();

			//Accept input for a match option - Player 1
			if (GM.GetAcceptDown("1"))
			{
				_p1SelectedOption = _p1MatchOptions.Select();
				_p1Picked = true;
			}
		}
		
		if (!_p2Picked)
		{
			//Player 2 Up input
			if (GM.GetUpDown("2"))
				_p2MatchOptions.SelectUp();
			//Player 2 Down input
			if (GM.GetDownDown("2"))
				_p2MatchOptions.SelectDown();
			
			//Accept input for a match option - Player 2
			if (GM.GetAcceptDown("2"))
			{
				_p2SelectedOption = _p2MatchOptions.Select();
				_p2Picked = true;
			}
		}
	}
	
	private void CheckSelections()
	{
		if (_p1SelectedOption.Equals("Rematch") && _p2SelectedOption.Equals("Rematch"))
		{
			_level.Rematch();
		}
		else if (_p1SelectedOption.Equals("ChangeChar") || _p2SelectedOption.Equals("ChangeChar"))
		{
			Application.LoadLevel("CharacterSelectScreen");
		}
		else if (_p1SelectedOption.Equals("ChangeStage") || _p2SelectedOption.Equals("ChangeStage"))
		{
			Application.LoadLevel("StageSelectScreen");
		}
	}

	public void BeginScreen(string winner)
	{
		if (winner.Equals("Player1"))
		{
			_p1Score++;
            _p1WinnerText.SetActive(true);
		}
		else if (winner.Equals("Player2"))
		{
			_p2Score++;
            _p2WinnerText.SetActive(true);
		}
		else if (winner.Equals("Draw"))
		{
			_drawScore++;
		}

		transform.position = _enabledPos;
		_enabled = true;
		_p1MatchOptions.Reset();
		_p2MatchOptions.Reset();
		_audio.Play();
        _timer = WAIT_TIME;
        _p1Options.gameObject.SetActive(false);
        _p2Options.gameObject.SetActive(false);
	}

	public void EndScreen()
	{
		transform.position = _disabledPos;
		_enabled = false;
		_p1Picked = false;
		_p2Picked = false;
		_p1SelectedOption = "";
		_p2SelectedOption = "";
		_audio.Stop();
        _p1WinnerText.SetActive(false);
        _p2WinnerText.SetActive(false);
	}
}
