using UnityEngine;
using System.Collections;

public class StageSelectScreen : MonoBehaviour 
{

    private GameManager GM;
    
    private const float SPEED = 8.0f;
    private const float MOVE_SPEED = 50.0f;
    
    public Transform WhiteBG;
    public SpriteRenderer p1Pick;
    public SpriteRenderer p2Pick;
    public SpriteRenderer backBG;
    public SpriteRenderer ScreenTransition;
    public StageList StageList;
    public GameObject StageSelector;
    public TextMesh StageChoiceText;
    public AudioClip Hover;
    public AudioClip Select;
    
    private bool _startGame;
    private bool _shrinkStage;
    private bool _moveStage;
    private bool _fadeIn;
    private bool _fadeOut;
    private AudioSource _bgMusic;
    private AudioSource _soundEffects;
    private bool _stagePicked;
    private StageChoice _stageChoice;
    private GameObject[] _stages;
    private float _stageAccel;
    private float _stageVel;
    
    //Player pick sprites animation variables
    private float _p1Start;
    private float _p2Start;
    private float _p1End;
    private float _p2End;
    private float _startVel1;
    private float _startVel2;
    private float _pickTime;
    private float _pick1Accel;
    private float _pick2Accel;
    private float _p1Vel;
    private float _p2Vel;

	// Use this for initialization
	void Start () 
    {
        _p1Start = -1000.0f;
        _p2Start = 1000.0f;
        _p1End = -300.0f;
        _p2End = 300.0f;
        _startVel1 = 5000.0f;
        _startVel2 = -5000.0f;
        _pickTime = 4.0f;
        _pick1Accel = 0.0f;
        _pick2Accel = 0.0f;
        _p1Vel = 0.0f;
        _p2Vel = 0.0f;
    
        p1Pick.transform.position = new Vector3(_p1Start, p1Pick.transform.position.y, p1Pick.transform.position.z);
        p2Pick.transform.position = new Vector3(_p2Start, p2Pick.transform.position.y, p2Pick.transform.position.z);
    
        if (GameObject.FindGameObjectWithTag("GameManager") != null)
        {
            GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            
            p1Pick.color = GM.P1CharChoice;
            p2Pick.color = GM.P2CharChoice;
        }
        else
        {
            Instantiate(Resources.Load("GameManager"));
            GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }
        
        _bgMusic = transform.GetComponent<AudioSource>();
        _soundEffects = transform.Find("SoundEffects").GetComponent<AudioSource>();
        
        _stages = StageList.Stages;
        _stageChoice = _stages[0].transform.GetComponent<StageChoice>();
        
        _stageAccel = 0.0f;
        _stageVel = 0.0f;

        ScreenTransition.color = new Color(ScreenTransition.color.r,
                                           ScreenTransition.color.g,
                                           ScreenTransition.color.b,
                                           1);
        _fadeOut = true;
        _fadeIn = false;
    }
    
    void Update()
    {
        PlayerInput();
    }
    
	// Update is called once per frame
	void FixedUpdate () 
    {
        if (_startGame)
        {
            _bgMusic.volume -= 0.75f * Time.deltaTime;
            
            for (int i = 0; i < _stages.Length; i++)
            {
                _stages[i].transform.position = new Vector3(_stages[i].transform.position.x, 
                                                            _stages[i].transform.position.y + (_stageVel * Time.deltaTime), 
                                                            _stages[i].transform.position.z);
            }

            if (!_shrinkStage)
                WhiteBG.GetComponent<SpriteRenderer>().color = new Color (1, 1, 1, 
                                                                          WhiteBG.GetComponent<SpriteRenderer>().color.a + (2.5f * Time.deltaTime));

            if (WhiteBG.GetComponent<SpriteRenderer>().color.a >= 1)
            {
                _shrinkStage = true;
                _moveStage = true;
                transform.Find("PreTransition").gameObject.SetActive(false);
            }
        }

        if (_shrinkStage)
        {
            WhiteBG.localScale = new Vector3 (WhiteBG.localScale.x, WhiteBG.localScale.y - (SPEED * Time.deltaTime),
                                              WhiteBG.localScale.z);
        }
        if (_moveStage)
        {
            WhiteBG.position = new Vector3 (WhiteBG.position.x, WhiteBG.position.y - (MOVE_SPEED * Time.deltaTime),
                                            WhiteBG.position.z);

            if (WhiteBG.position.y <= -64.0f)
            {
                WhiteBG.position = new Vector3 (WhiteBG.position.x, -64.0f, WhiteBG.position.z);
                _moveStage = false;
            }
        }
        
        if (WhiteBG.GetComponent<SpriteRenderer>().bounds.extents.y * 2 <= 64.0f)
        {
            WhiteBG.localScale = new Vector3 (WhiteBG.localScale.x, 0.711f, WhiteBG.localScale.z);

            if (GM.StageSelection.Equals("Bridge"))
                Application.LoadLevel("GameScreen");
        }
        
        StageSelector.transform.position = new Vector3(_stageChoice.transform.position.x,
                                                    _stageChoice.transform.position.y,
                                                    StageSelector.transform.position.z);
                                                    
        p1Pick.transform.position = new Vector3(p1Pick.transform.position.x + (_p1Vel * Time.deltaTime),
                                     p1Pick.transform.position.y, p1Pick.transform.position.z);
        p2Pick.transform.position = new Vector3(p2Pick.transform.position.x + (_p2Vel * Time.deltaTime),
                                     p2Pick.transform.position.y, p2Pick.transform.position.z);
        
        for (int i = 0; i < _stages.Length; i++)
        {
            if (_stages[i].transform.position.y < -600.0f)
            {
                _stageVel = 0;
                _stageAccel = 0;
            }
        }
        _stageVel += _stageAccel * Time.deltaTime;
        _p1Vel += _pick1Accel * Time.deltaTime;
        _p2Vel += _pick2Accel * Time.deltaTime;
        
        StageChoiceText.text = _stageChoice.transform.name;
        
        if (p1Pick.transform.position.x >= _p1End)
        {
            _p1Vel = 0.0f;
            _pick1Accel = 0.0f;
            p1Pick.transform.position = new Vector3(_p1End, p1Pick.transform.position.y, p1Pick.transform.position.z);
        }
        if (p2Pick.transform.position.x <= _p2End)
        {
            _p2Vel = 0.0f;
            _pick2Accel = 0.0f;
            p2Pick.transform.position = new Vector3(_p2End, p2Pick.transform.position.y, p2Pick.transform.position.z);
        }
        
        backBG.sprite = _stageChoice.bgSprite;

        //Screen transition code
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

    }
    
    private void PlayerInput()
    {
        if (GM.GetAcceptDown("1") && !_startGame)
        {
            _startGame = true;
            _soundEffects.Play();
        }
        
        //Player 1 Input
        if (!_stagePicked)
        {
            if (GM.GetUpDown("1"))
            {
                _stageChoice.Hovered = false;
                _stageChoice = GetNextStage("Up", _stageChoice);
                if (_stageChoice.Hovered)
                    _stageChoice = GetNextStage("Up", _stageChoice);
                _stageChoice.Hovered = true;
                
                StageSelector.GetComponent<AudioSource>().clip = Hover;
                StageSelector.GetComponent<AudioSource>().Play();
            }
            if (GM.GetDownDown("1"))
            {
                _stageChoice.Hovered = false;
                _stageChoice = GetNextStage("Down", _stageChoice);
                if (_stageChoice.Hovered)
                    _stageChoice = GetNextStage("Down", _stageChoice);
                _stageChoice.Hovered = true;
                
                StageSelector.GetComponent<AudioSource>().clip = Hover;
                StageSelector.GetComponent<AudioSource>().Play();
            }
            if (GM.GetLeftDown("1"))
            {
                _stageChoice.Hovered = false;
                _stageChoice = GetNextStage("Left", _stageChoice);
                if (_stageChoice.Hovered)
                    _stageChoice = GetNextStage("Left", _stageChoice);
                _stageChoice.Hovered = true;
                
                StageSelector.GetComponent<AudioSource>().clip = Hover;
                StageSelector.GetComponent<AudioSource>().Play();
            }    
            if (GM.GetRightDown("1"))
            {
                _stageChoice.Hovered = false;
                _stageChoice = GetNextStage("Right", _stageChoice);
                if (_stageChoice.Hovered)
                    _stageChoice = GetNextStage("Right", _stageChoice);
                _stageChoice.Hovered = true;
                
                StageSelector.GetComponent<AudioSource>().clip = Hover;
                StageSelector.GetComponent<AudioSource>().Play();
            }    
            if (GM.GetAcceptDown("1"))
            {
                _stagePicked = true;
                GM.StageSelection = _stageChoice.transform.name;
                GM.StageBG = _stageChoice.bgSprite;
                
                StageSelector.GetComponent<AudioSource>().clip = Select;
                StageSelector.GetComponent<AudioSource>().Play();
                
                MoveStageSelections();
            }
        }
    }
    
    private StageChoice GetNextStage(string direction, StageChoice currentStage)
    {
        if (direction.Equals("Up"))
        {
            if (currentStage.Up != null)
                return currentStage.Up;
            else
                return currentStage;
        }
        if (direction.Equals("Down"))
        {
            if (currentStage.Down != null)
                return currentStage.Down;
            else
                return currentStage;
        }
        if (direction.Equals("Left"))
        {
            if (currentStage.Left != null)
                return currentStage.Left;
            else
                return currentStage;
        }
        if (direction.Equals("Right"))
        {
            if (currentStage.Right != null)
                return currentStage.Right;
            else
                return currentStage;
        }
        else
        {
            return null;
        }
    }
    
    private void MoveStageSelections()
    {
        _stageVel = 500.0f;
        _stageAccel = -3000.0f;
        
        _pick1Accel = -2 * ((_p1Start - _p1End) - (_startVel1 * _pickTime)) / (_pickTime * _pickTime);
        _pick2Accel = -2 * ((_p2Start - _p2End) - (_startVel2 * _pickTime)) / (_pickTime * _pickTime);
    }
}
