using UnityEngine;
using System.Collections;

public class CharacterSelectScreen : MonoBehaviour 
{
    private GameManager GM;
    
	private const float SPEED = 8.0f;

	public Transform WhiteBG;
    public GameObject P1Selector;
    public GameObject P2Selector;
    public SpriteRenderer P1Pick;
    public SpriteRenderer P2Pick;
    public CharacterList _charList;
    public AudioClip Hover;
    public AudioClip Select;
    public TextMesh P1PickText;
    public TextMesh P2PickText;
    public SpriteRenderer ScreenTransition;

	private bool _startGame;
	//private AudioSource _bgMusic;
	//private AudioSource _soundEffects;
    private GameObject[] _characters;
    private Character _p1Char;
    private Character _p2Char;
    private bool _p1Picked;
    private bool _p2Picked;
    private bool _fadeIn;
    private bool _fadeOut;
    
    // Use this for initialization
	void Start () 
	{
        if (GameObject.FindGameObjectWithTag("GameManager") != null)
            GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        else
        {
            Instantiate(Resources.Load("GameManager"));
            GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }
    
		//_bgMusic = transform.GetComponent<AudioSource>();
		//_soundEffects = transform.Find("SoundEffects").GetComponent<AudioSource>();
        
        _characters = _charList.Characters;
        
        _p1Char = _characters[0].transform.GetComponent<Character>();
        _p2Char = _characters[4].transform.GetComponent<Character>();
        _p1Char.Hovered = true;
        _p2Char.Hovered = true;
        _p1Picked = false;
        _p2Picked = false;
        P1Pick.enabled = false;
        P2Pick.enabled = false;
        
        P1Selector.transform.position = new Vector3(_p1Char.transform.position.x,
                                                    _p1Char.transform.position.y,
                                                    P1Selector.transform.position.z);
        P2Selector.transform.position = new Vector3(_p2Char.transform.position.x,
                                                    _p2Char.transform.position.y,
                                                    P2Selector.transform.position.z);

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
        P1Selector.transform.position = new Vector3(_p1Char.transform.position.x,
                                                    _p1Char.transform.position.y,
                                                    P1Selector.transform.position.z);
        P2Selector.transform.position = new Vector3(_p2Char.transform.position.x,
                                                    _p2Char.transform.position.y,
                                                    P2Selector.transform.position.z);
                                                    
        P1Pick.color = _p1Char.transform.GetComponent<SpriteRenderer>().color;
        P2Pick.color = _p2Char.transform.GetComponent<SpriteRenderer>().color;
        
        P1PickText.text = _p1Char.Name2;
        P2PickText.text = _p2Char.Name2;

        
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
            Application.LoadLevel("StageSelectScreen");
    }
    
    private void PlayerInput()
    {   
        if (_p1Picked && _p2Picked && (GM.GetAcceptDown("1") || GM.GetAcceptDown("2")))
        {
            _fadeIn = true;
        }
        
        //Player 1 Input
        if (!_p1Picked)
        {
            if (GM.GetUpDown("1"))
            {
                _p1Char.Hovered = false;
                _p1Char = GetNextChar("Up", _p1Char);
                if (_p1Char.Hovered)
                    _p1Char = GetNextChar("Up", _p1Char);
                _p1Char.Hovered = true;
                
                P1Selector.GetComponent<AudioSource>().clip = Hover;
                P1Selector.GetComponent<AudioSource>().Play();
            }
            if (GM.GetDownDown("1"))
            {
                _p1Char.Hovered = false;
                _p1Char = GetNextChar("Down", _p1Char);
                if (_p1Char.Hovered)
                    _p1Char = GetNextChar("Down", _p1Char);
                _p1Char.Hovered = true;
                
                P1Selector.GetComponent<AudioSource>().clip = Hover;
                P1Selector.GetComponent<AudioSource>().Play();
            }
            if (GM.GetLeftDown("1"))
            {
                _p1Char.Hovered = false;
                _p1Char = GetNextChar("Left", _p1Char);
                if (_p1Char.Hovered)
                    _p1Char = GetNextChar("Left", _p1Char);
                _p1Char.Hovered = true;
                
                P1Selector.GetComponent<AudioSource>().clip = Hover;
                P1Selector.GetComponent<AudioSource>().Play();
            }    
            if (GM.GetRightDown("1"))
            {
                _p1Char.Hovered = false;
                _p1Char = GetNextChar("Right", _p1Char);
                if (_p1Char.Hovered)
                    _p1Char = GetNextChar("Right", _p1Char);
                _p1Char.Hovered = true;
                
                P1Selector.GetComponent<AudioSource>().clip = Hover;
                P1Selector.GetComponent<AudioSource>().Play();
            }    
            if (GM.GetAcceptDown("1"))
            {
                _p1Picked = true;
                GM.P1CharChoice = P1Pick.color;
                GM.P1Char = _p1Char.Name;
                
                P1Selector.GetComponent<AudioSource>().clip = Select;
                P1Selector.GetComponent<AudioSource>().Play();
                
                if (_p1Char.transform.GetComponent<AudioSource>() != null)
                    _p1Char.transform.GetComponent<AudioSource>().Play();
                
                P1Pick.color = _p1Char.transform.GetComponent<SpriteRenderer>().color;
                P1Pick.enabled = true;
            }
        }
            
        //Player 2 Input
        if (!_p2Picked)
        {
            if (GM.GetUpDown("2"))
            {
                _p2Char.Hovered = false;
                _p2Char = GetNextChar("Up", _p2Char);
                if (_p2Char.Hovered)
                    _p2Char = GetNextChar("Up", _p2Char);
                _p2Char.Hovered = true;
                
                P2Selector.GetComponent<AudioSource>().clip = Hover;
                P2Selector.GetComponent<AudioSource>().Play();
            }
            if (GM.GetDownDown("2"))
            {
                _p2Char.Hovered = false;
                _p2Char = GetNextChar("Down", _p2Char);
                if (_p2Char.Hovered)
                    _p2Char = GetNextChar("Down", _p2Char);
                _p2Char.Hovered = true;
                
                P2Selector.GetComponent<AudioSource>().clip = Hover;
                P2Selector.GetComponent<AudioSource>().Play();
            }    
            if (GM.GetLeftDown("2"))
            {
                _p2Char.Hovered = false;
                _p2Char = GetNextChar("Left", _p2Char);
                if (_p2Char.Hovered)
                    _p2Char = GetNextChar("Left", _p2Char);
                _p2Char.Hovered = true;
                
                P2Selector.GetComponent<AudioSource>().clip = Hover;
                P2Selector.GetComponent<AudioSource>().Play();
            }    
            if (GM.GetRightDown("2"))
            {
                _p2Char.Hovered = false;
                _p2Char = GetNextChar("Right", _p2Char);
                if (_p2Char.Hovered)
                    _p2Char = GetNextChar("Right", _p2Char);
                _p2Char.Hovered = true;
                
                P2Selector.GetComponent<AudioSource>().clip = Hover;
                P2Selector.GetComponent<AudioSource>().Play();
            }    
            if (GM.GetAcceptDown("2"))
            {
                _p2Picked = true;
                GM.P2CharChoice = P2Pick.color;
                GM.P2Char = _p2Char.Name;
                
                P2Selector.GetComponent<AudioSource>().clip = Select;
                P2Selector.GetComponent<AudioSource>().Play();
                
                if (_p2Char.transform.GetComponent<AudioSource>() != null)
                    _p2Char.transform.GetComponent<AudioSource>().Play();
                
                P2Pick.color = _p2Char.transform.GetComponent<SpriteRenderer>().color;
                P2Pick.enabled = true;
            }
        }
    }
    
    private Character GetNextChar(string direction, Character currentChar)
    {
        if (direction.Equals("Up"))
        {
            if (currentChar.Up != null)
                return currentChar.Up;
            else
                return currentChar;
        }
        if (direction.Equals("Down"))
        {
            if (currentChar.Down != null)
                return currentChar.Down;
            else
                return currentChar;
        }
        if (direction.Equals("Left"))
        {
            if (currentChar.Left != null)
                return currentChar.Left;
            else
                return currentChar;
        }
        if (direction.Equals("Right"))
        {
            if (currentChar.Right != null)
                return currentChar.Right;
            else
                return currentChar;
        }
        else
        {
            return null;
        }
    }
}
