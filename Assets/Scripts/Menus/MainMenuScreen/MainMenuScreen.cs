using UnityEngine;
using System.Collections;

public class MainMenuScreen : MonoBehaviour 
{
    private GameManager GM;

	public Button StoryButton;
    public Button VersusButton;
    public Button OptionsButton;
	public Button ExitButton;
    public Button PvP;
    public Button PvC;
    public Button CvC;
	public AudioClip Hover;
	public AudioClip Choose;
    public SpriteRenderer ScreenTransition;
	
	private Button _selectedButton;
	//private AudioSource _bgMusic;
	private AudioSource _menuAudio;
    private bool _secondaryOptions;
    private bool _fadeOut;
    private bool _fadeIn;

	// Use this for initialization
	void Start () 
	{
		_selectedButton = VersusButton;
		_selectedButton.Selected = true;
        PvP.Selected = true;
		
		//_bgMusic = transform.GetComponent<AudioSource>();
		_menuAudio = transform.Find("MenuAudio").GetComponent<AudioSource>();
        
        if (GameObject.FindGameObjectWithTag("GameManager") == null)
        {
            Instantiate(Resources.Load("GameManager"));
            GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }
        else
            GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        ScreenTransition.color = new Color(ScreenTransition.color.r,
                                           ScreenTransition.color.g,
                                           ScreenTransition.color.b,
                                           1);
        _fadeOut = true;
        _fadeIn = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (!_fadeIn && !_fadeOut)
		    PlayerInput();
	}
    
    void FixedUpdate()
    {
        if (_secondaryOptions)
        {
            transform.Find("Options").gameObject.SetActive(false);
            transform.Find("VersusOptions").gameObject.SetActive(true);
        }
        else
        {
            transform.Find("Options").gameObject.SetActive(true);
            transform.Find("VersusOptions").gameObject.SetActive(false);
        }

        if (_fadeOut)
            ScreenTransition.color = new Color(ScreenTransition.color.r,
                                               ScreenTransition.color.g,
                                               ScreenTransition.color.b,
                                               ScreenTransition.color.a - (1.25f * Time.deltaTime));

        if (_fadeIn)
            ScreenTransition.color = new Color(ScreenTransition.color.r,
                                               ScreenTransition.color.g,
                                               ScreenTransition.color.b,
                                               ScreenTransition.color.a + (1.25f * Time.deltaTime));

        if (ScreenTransition.color.a <= 0)
            _fadeOut = false;

        if (_fadeIn && ScreenTransition.color.a >= 1)
            Application.LoadLevel("CharacterSelectScreen");

    }
	
	private void PlayerInput()
	{
		if (GM.GetUpDown("1"))
		{
			_selectedButton.Selected = false;
			_selectedButton = _selectedButton.GetUp();
			_selectedButton.Selected = true;
			
			_menuAudio.clip = Hover;
			_menuAudio.Play();
		}
		else if (GM.GetDownDown("1"))
		{
			_selectedButton.Selected = false;
			_selectedButton = _selectedButton.GetDown();
			_selectedButton.Selected = true;
			
			_menuAudio.clip = Hover;
			_menuAudio.Play();
		}
		
		if (GM.GetAcceptDown("1"))
		{
			_menuAudio.clip = Choose;
			_menuAudio.Play();
			
			if (_selectedButton.name.Contains("Versus"))
            {
				_secondaryOptions = true;
                
                _selectedButton.Selected = false;
                _selectedButton = PvP;
                _selectedButton.Selected = true;
            }
            else if (_selectedButton.name.Contains("Story"))
                Application.LoadLevel("ControlsScreen");
			else if (_selectedButton.name.Contains("Exit"))
				Application.Quit();
            else if (_selectedButton.name.Contains("PvP"))
            {
                GM.PlayMode = GameManager.PlayerMode.PvP;  
                //Application.LoadLevel("CharacterSelectScreen");
                _fadeIn = true;
            }
            else if (_selectedButton.name.Contains("PvC"))
            {
                GM.PlayMode = GameManager.PlayerMode.PvC;  
                //Application.LoadLevel("CharacterSelectScreen");
                _fadeIn = true;
            }
            else if (_selectedButton.name.Contains("CvC"))
            {
                GM.PlayMode = GameManager.PlayerMode.CvC;  
                //Application.LoadLevel("CharacterSelectScreen");
                _fadeIn = true;
            }
        }
	}
}
