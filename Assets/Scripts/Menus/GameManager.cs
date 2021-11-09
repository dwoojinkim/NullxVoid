using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
    public enum PlayerMode { PvP, PvC, CvC }
    public enum GameType { Versus }

    private const float DEADZONE = 0.25f;

    public string StageSelection { get; set; }
    public Color P1CharChoice { get; set; }
    public Color P2CharChoice { get; set; }
    public string P1Char { get; set; }
    public string P2Char { get; set; }
    public Sprite StageBG { get; set; }
    public PlayerMode PlayMode { get; set; }    //Whether it's Player v Player, Player v CPU, or CPU v CPU
    public GameType GameT { get; set; }
    
    public bool EndRound { get; set; }
    public bool EndOfMatch { get; set; }
    
    private KeyCode[] _buttonMap1;
    private string[] _controllerMap1;
    private KeyCode[] _buttonMap2;
    private string[] _controllerMap2;
    
    #region "GetAxisDown" Variables
    //Player 1
    private bool _leftAxisDown1;
    private bool _rightAxisDown1;
    private bool _upAxisDown1;
    private bool _downAxisDown1;
    
    //Player 2
    private bool _leftAxisDown2;
    private bool _rightAxisDown2;
    private bool _upAxisDown2;
    private bool _downAxisDown2;
    #endregion
    
    #region "GetAxisUp" Variables
    //Player 1
    private bool _leftAxisUp1;
    private bool _rightAxisUp1;
    private bool _upAxisUp1;
    private bool _downAxisUp1;
    
    //Player 2
    private bool _leftAxisUp2;
    private bool _rightAxisUp2;
    private bool _upAxisUp2;
    private bool _downAxisUp2;
    #endregion
    
	// Use this for initialization
	void Awake () 
    {
        DontDestroyOnLoad (transform.gameObject);
        
        StageSelection = "Bridge";
        P1CharChoice = Color.white;
        P2CharChoice = Color.blue;
        P1Char = "Null";
        P2Char = "Void";
        PlayMode = PlayerMode.PvP;
        GameT = GameType.Versus;
        
        // index 0 = Left, 1 = Right, 2 = Ability, 3 = Down, 4 = Accept, 5 = Cancel
        _buttonMap1 = new KeyCode[]{KeyCode.A, KeyCode.D, KeyCode.W, KeyCode.S, KeyCode.Space, KeyCode.Escape };
        _buttonMap2 = new KeyCode[]{KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.Return, KeyCode.Backspace };
        
        // index 0 = Horiz. Dpad, 1 = Horiz. Analog Stick, 2 = Ability, 3 = Vert Dpad, 4 = Horiz. Analog,
        _controllerMap1 = new string[] { "P1_DPAD_HOR", "P1_JOYSTICK_HOR", "P1_ABILITY", "P1_DPAD_VERT", "P1_JOYSTICK_VERT" };
        _controllerMap2 = new string[] { "P2_DPAD_HOR", "P2_JOYSTICK_HOR", "P2_ABILITY", "P2_DPAD_VERT", "P2_JOYSTICK_VERT" };
	}
	
	// Update is called once per frame
	void Update () 
    {
	    CheckAxisDown();
	}
    
    #region Player Input
    
    #region UP
    public bool GetUp(string playerName)
    {
        if (playerName.Contains("1"))
        {
            if (Input.GetKey(_buttonMap1[2]) || Input.GetAxis(_controllerMap1[3]) > DEADZONE || Input.GetAxis(_controllerMap1[4]) > DEADZONE)
            {
                _upAxisDown1 = true;
                _downAxisDown1 = false;
                return true;
            }    
        }
        else if (playerName.Contains("2"))
        {
            if (Input.GetKey(_buttonMap2[2]) || Input.GetAxis(_controllerMap2[3]) > DEADZONE || Input.GetAxis(_controllerMap2[4]) > DEADZONE)
            {
                _upAxisDown2 = true;
                _downAxisDown2 = false;
                return true;
            }   
        }
        
        return false;
    }
    public bool GetUpDown(string playerName)
    {
        if (playerName.Contains("1"))
        {
            if (Input.GetKeyDown(_buttonMap1[2]) || ((Input.GetAxis(_controllerMap1[3]) > DEADZONE || Input.GetAxis(_controllerMap1[4]) > DEADZONE) && !_upAxisDown1))
            {
                _upAxisDown1 = true;
                _downAxisDown1 = false;
                return true;
            }    
        }
        else if (playerName.Contains("2"))
        {
            if (Input.GetKeyDown(_buttonMap2[2]) || ((Input.GetAxis(_controllerMap2[3]) > DEADZONE || Input.GetAxis(_controllerMap2[4]) > DEADZONE) && !_upAxisDown2))
            {
                _upAxisDown2 = true;
                _downAxisDown2 = false;
                return true;
            }   
        }
        
        return false;
    }
    public bool GetUpUp(string playerName)
    {
        if (playerName.Contains("1"))
            return _upAxisUp1 || Input.GetKeyUp(_buttonMap1[2]);
        else if (playerName.Contains("2"))
            return _upAxisUp2 || Input.GetKeyUp(_buttonMap2[2]);
        
        return false;
    }
    #endregion
    
    #region DOWN
    public bool GetDown(string playerName)
    {
        if (playerName.Contains("1"))
        {
            if (Input.GetKey(_buttonMap1[3]) || Input.GetAxis(_controllerMap1[3]) < -DEADZONE || Input.GetAxis(_controllerMap1[4]) < -DEADZONE)
            {
                _downAxisDown1 = true;
                _upAxisDown1 = false;
                return true;
            }    
        }
        else if (playerName.Contains("2"))
        {
            if (Input.GetKey(_buttonMap2[3]) || Input.GetAxis(_controllerMap2[3]) < -DEADZONE || Input.GetAxis(_controllerMap2[4]) < -DEADZONE)
            {
                _downAxisDown2 = true;
                _upAxisDown2 = false;
                return true;
            }   
        }
        
        return false;
    }
    public bool GetDownDown(string playerName)
    {
        if (playerName.Contains("1"))
        {
            if (Input.GetKeyDown(_buttonMap1[3]) || ((Input.GetAxis(_controllerMap1[3]) < -DEADZONE || Input.GetAxis(_controllerMap1[4]) < -DEADZONE) && !_downAxisDown1))
            {
                _downAxisDown1 = true;
                _upAxisDown1 = false;
                return true;
            }    
        }
        else if (playerName.Contains("2"))
        {
            if (Input.GetKeyDown(_buttonMap2[3]) || ((Input.GetAxis(_controllerMap2[3]) < -DEADZONE || Input.GetAxis(_controllerMap2[4]) < -DEADZONE) && !_downAxisDown2))
            {
                _downAxisDown2 = true;
                _upAxisDown2 = false;   
                return true;
            }   
        }
        
        return false;
    }
    public bool GetDownUp(string playerName)
    {
        if (playerName.Contains("1"))
            return _downAxisUp1 || Input.GetKeyUp(_buttonMap1[3]);
        else if (playerName.Contains("2"))
            return _downAxisUp2 || Input.GetKeyUp(_buttonMap2[3]);
        
        return false;
    }
    #endregion
    
    #region LEFT
    public bool GetLeft(string playerName)
    {
        if (playerName.Contains("1"))
        {
            if (Input.GetKey(_buttonMap1[0]) || Input.GetAxis(_controllerMap1[0]) < -DEADZONE || 
                Input.GetAxis(_controllerMap1[1]) < -DEADZONE || AIInput.GetKey("Left1"))
            {
                _leftAxisDown1 = true;
                _rightAxisDown1 = false;
                return true;
            }    
        }
        else if (playerName.Contains("2"))
        {
            if (Input.GetKey(_buttonMap2[0]) || Input.GetAxis(_controllerMap2[0]) < -DEADZONE || 
                Input.GetAxis(_controllerMap2[1]) < -DEADZONE || AIInput.GetKey("Left2"))
            {
                _leftAxisDown2 = true;
                _rightAxisDown2 = false;   
                return true;
            }   
        }
        
        return false;
    }
    public bool GetLeftDown(string playerName)
    {
        if (playerName.Contains("1"))
        {
            if (Input.GetKeyDown(_buttonMap1[0]) || ((Input.GetAxis(_controllerMap1[0]) < -DEADZONE || Input.GetAxis(_controllerMap1[1]) < -DEADZONE) && !_leftAxisDown1)
                || AIInput.GetKeyDown("Left1"))
            {
                _leftAxisDown1 = true;
                _rightAxisDown1 = false;
                return true;
            }    
        }
        else if (playerName.Contains("2"))
        {
            if (Input.GetKeyDown(_buttonMap2[0]) || ((Input.GetAxis(_controllerMap2[0]) < -DEADZONE || Input.GetAxis(_controllerMap2[1]) < -DEADZONE) && !_leftAxisDown2)
                || AIInput.GetKeyDown("Left2"))
            {
                _leftAxisDown2 = true;
                _rightAxisDown2 = false;   
                return true;
            }   
        }
        
        return false;
    }
    public bool GetLeftUp(string playerName)
    {
        if (playerName.Contains("1"))
            return _leftAxisUp1 || Input.GetKeyUp(_buttonMap1[0]) || AIInput.GetKeyUp("Left1");
        else if (playerName.Contains("2"))
            return _leftAxisUp2 || Input.GetKeyUp(_buttonMap2[0]) || AIInput.GetKeyUp("Left2");
        
        return false;
    }
    #endregion
    
    #region RIGHT
    public bool GetRight(string playerName)
    {
        if (playerName.Contains("1"))
        {
            if (Input.GetKey(_buttonMap1[1]) || Input.GetAxis(_controllerMap1[0]) > DEADZONE ||
                Input.GetAxis(_controllerMap1[1]) > DEADZONE || AIInput.GetKey("Right1"))
            {
                _rightAxisDown1 = true;
                _leftAxisDown1 = false;
                return true;
            }    
        }
        else if (playerName.Contains("2"))
        {
            if (Input.GetKey(_buttonMap2[1]) || Input.GetAxis(_controllerMap2[0]) > DEADZONE || 
                Input.GetAxis(_controllerMap2[1]) > DEADZONE || AIInput.GetKey("Right2"))
            {
                _rightAxisDown2 = true;
                _leftAxisDown2 = false;
                return true;
            }   
        }
        
        return false;
    }
    public bool GetRightDown(string playerName)
    {
        if (playerName.Contains("1"))
        {
            if (Input.GetKeyDown(_buttonMap1[1]) || ((Input.GetAxis(_controllerMap1[0]) > DEADZONE || Input.GetAxis(_controllerMap1[1]) > DEADZONE) && !_rightAxisDown1)
                || AIInput.GetKeyDown("Right1"))
            {
                _rightAxisDown1 = true;
                _leftAxisDown1 = false;
                return true;
            }    
        }
        else if (playerName.Contains("2"))
        {
            if (Input.GetKeyDown(_buttonMap2[1]) || ((Input.GetAxis(_controllerMap2[0]) > DEADZONE || Input.GetAxis(_controllerMap2[1]) > DEADZONE) && !_rightAxisDown2)
                || AIInput.GetKeyDown("Right2"))
            {
                _rightAxisDown2 = true;
                _leftAxisDown2 = false;   
                return true;
            }   
        }
        
        return false;
    }
    public bool GetRightUp(string playerName)
    {
        if (playerName.Contains("1"))
            return _rightAxisUp1 || Input.GetKeyUp(_buttonMap1[1]) || AIInput.GetKeyUp("Right1");
        else if (playerName.Contains("2"))
            return _rightAxisUp2 || Input.GetKeyUp(_buttonMap2[1]) || AIInput.GetKeyUp("Right2");
        
        return false;
    }
    #endregion
    
    #region Ability Input
    public bool GetAbility(string playerName)
    {
        if (playerName.Contains("1"))
        {
            if (Input.GetKey(_buttonMap1[2]) || Input.GetButton(_controllerMap1[2]) || AIInput.GetKey("Ability1"))
                return true;
        }
        else if (playerName.Contains("2"))
        {
            if (Input.GetKey(_buttonMap2[2]) || Input.GetButton(_controllerMap2[2]) || AIInput.GetKey("Ability2"))
                return true;
        }
        
        return false;
    }
    public bool GetAbilityDown(string playerName)
    {
        if (playerName.Contains("1"))
        {
            if (Input.GetKeyDown(_buttonMap1[2]) || Input.GetButtonDown(_controllerMap1[2]) || AIInput.GetKeyDown("Ability1"))
                return true;
        }
        else if (playerName.Contains("2"))
        {
            if (Input.GetKeyDown(_buttonMap2[2]) || Input.GetButtonDown(_controllerMap2[2]) || AIInput.GetKeyDown("Ability2"))
                return true;
        }
            
        return false;
    }
    public bool GetAbilityUp(string playerName)
    {
        if (playerName.Contains("1"))
        {
            if (Input.GetKeyUp(_buttonMap1[2]) || Input.GetButtonUp(_controllerMap1[2]))
                return true;
        }
        else if (playerName.Contains("2"))
        {
            if (Input.GetKeyUp(_buttonMap2[2]) || Input.GetButtonUp(_controllerMap2[2]))
                return true;
        }
        
        return false;
    }
    #endregion
    
    public bool GetAccept(string playerName)
    {
        if (playerName.Contains("1"))
        {
            if (Input.GetKey(_buttonMap1[4]) || Input.GetButton(_controllerMap1[2]))
                return true;
        }
        else if (playerName.Contains("2"))
        {
            if (Input.GetKey(_buttonMap2[4]) || Input.GetButton(_controllerMap2[2]))
                return true;
        }
        
        return false;
    }
    public bool GetAcceptDown(string playerName)
    {
        if (playerName.Contains("1"))
        {
            if (Input.GetKeyDown(_buttonMap1[4]) || Input.GetButtonDown(_controllerMap1[2]))
                return true;
        }
        else if (playerName.Contains("2"))
        {
            if (Input.GetKeyDown(_buttonMap2[4]) || Input.GetButtonDown(_controllerMap2[2]))
                return true;
        }
        
        return false;
    }
    #endregion
    
    //Helper method for "GetAxisDown"
    private void CheckAxisDown()
    {
        //Player 1 Check
        if (Input.GetAxis(_controllerMap1[3]) < DEADZONE && Input.GetAxis(_controllerMap1[3]) > -DEADZONE && 
            Input.GetAxis(_controllerMap1[4]) < DEADZONE && Input.GetAxis(_controllerMap1[4]) > -DEADZONE)
        {
            if (_upAxisUp1)
                _upAxisUp1 = false;
            if (_downAxisUp1)
                _downAxisUp1 = false;
            
            if (_upAxisDown1)
                _upAxisUp1 = true;
            if (_downAxisDown1)
                _downAxisUp1 = true;   
            
            _upAxisDown1 = false;
            _downAxisDown1 = false;
        }
        if (Input.GetAxis(_controllerMap1[0]) < DEADZONE && Input.GetAxis(_controllerMap1[0]) > -DEADZONE && 
            Input.GetAxis(_controllerMap1[1]) < DEADZONE && Input.GetAxis(_controllerMap1[1]) > -DEADZONE)
        {
            if (_rightAxisUp1)
                _rightAxisUp1 = false;
            if (_leftAxisUp1)
                _leftAxisUp1 = false;
            
            if (_rightAxisDown1)
                _rightAxisUp1 = true;
            if (_leftAxisDown1)
                _leftAxisUp1 = true; 
            
            _rightAxisDown1 = false;
            _leftAxisDown1 = false;
        }
        
        //Player 2 Check
        if (Input.GetAxis(_controllerMap2[3]) < DEADZONE && Input.GetAxis(_controllerMap2[3]) > -DEADZONE && 
            Input.GetAxis(_controllerMap2[4]) < DEADZONE && Input.GetAxis(_controllerMap2[4]) > -DEADZONE)
        {
            if (_upAxisUp2)
                _upAxisUp2 = false;
            if (_downAxisUp2)
                _downAxisUp2 = false;
            
            if (_upAxisDown2)
                _upAxisUp2 = true;
            if (_downAxisDown2)
                _downAxisUp2 = true; 
            
            _upAxisDown2 = false;
            _downAxisDown2 = false;
        }
        if (Input.GetAxis(_controllerMap2[0]) < DEADZONE && Input.GetAxis(_controllerMap2[0]) > -DEADZONE && 
            Input.GetAxis(_controllerMap2[1]) < DEADZONE && Input.GetAxis(_controllerMap2[1]) > -DEADZONE)
        {
            if (_rightAxisUp2)
                _rightAxisUp2 = false;
            if (_leftAxisUp2)
                _leftAxisUp2 = false;
            
            if (_rightAxisDown2)
                _rightAxisUp2 = true;
            if (_leftAxisDown2)
                _leftAxisUp2 = true; 
            
            _rightAxisDown2 = false;
            _leftAxisDown2 = false;
        }
    }
}
