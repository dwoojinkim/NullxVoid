using UnityEngine;
using System.Collections;

public class PlayerAI2 : MonoBehaviour 
{
    private GameManager GM;

    private Player _otherPlayer;
    private float _leftPressTime;
    private float _rightPressTime;
    private float _abilityPressTime;
    private SpriteRenderer _stageSprite;
    private float _stayAwayDistance;

    //Priority Values
    private float _leftPriority;
    private float _rightPriority;
    private float _leftSwapPriority;
    private float _rightSwapPriority;
    private float _leftBoostPriority;
    private float _rightBoostPriority;
    private float _totalPriority;

    private float _leftPercent;
    private float _rightPercent;
    private float _leftSwapPercent;
    private float _rightSwapPercent;
    private float _leftBoostPercent;
    private float _rightBoostPercent;

    private float _randNum;

	// Use this for initialization
	void Start () 
    {
        if (GameObject.FindGameObjectWithTag("GameManager") != null)
            GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        
	    if (name.Contains("1"))
            _otherPlayer = transform.parent.Find("Player2").GetComponent<Player>();
        else if (name.Contains("2"))
            _otherPlayer = transform.parent.Find("Player1").GetComponent<Player>();
            
        _stageSprite = transform.parent.Find("Stage/StageSprite").GetComponent<SpriteRenderer>();
        
        _stayAwayDistance = 150.0f;
	}
	
	// Update is called once per frame
	void Update () 
    {

	}
    
    void FixedUpdate()
    {
        if (GM.StageSelection.Equals("Bridge"))
            BridgeAI();

        AIInput.ReleaseKey(name + "Ability");

        ChooseAction();

        //_leftPriority += Time.deltaTime;
        //_rightPriority += Time.deltaTime;
        //_leftSwapPriority += Time.deltaTime;
        //_rightSwapPriority += Time.deltaTime;
        //_leftBoostPriority += Time.deltaTime;
        //_rightBoostPriority += Time.deltaTime;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.Contains("Player"))
        {  
            if (_otherPlayer.IsPressing)
            {
                if (this.transform.position.x > other.transform.position.x)
                    _rightSwapPriority += 10;
                else
                    _leftSwapPriority += 10;
            }
        }     
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.name.Contains("Player"))
        {  
            if (_otherPlayer.IsPressing)
            {
                if (this.transform.position.x > other.transform.position.x)
                    _rightSwapPriority += 15;
                else
                    _leftSwapPriority += 15;
            }
        }   
    }
    
    private void BridgeAI()
    {
        //AI is to the right of opponent
        if (transform.position.x > _otherPlayer.transform.position.x)
        {
            if (Mathf.Abs(transform.position.x - _stageSprite.bounds.extents.x) < _stayAwayDistance)
            {
                _leftPriority += 100;
                _rightPriority = 0;
            }
            else
            {
                _leftPriority += 10 * Time.deltaTime;
                _rightPriority -= 50 * Time.deltaTime;

                if (_rightPriority < 0)
                    _rightPriority = 0;
            }

            _leftBoostPriority += 0.5f * Time.deltaTime;
            _rightBoostPriority -= 10 * Time.deltaTime;

            if (_rightBoostPriority < 0)
                _rightBoostPriority = 0;
        }
        else
        {
            if (Mathf.Abs(transform.position.x + _stageSprite.bounds.extents.x) < _stayAwayDistance)
            {
                _rightPriority += 100;
                _leftPriority = 0;
            }
            else
            {
                _rightPriority += 10 * Time.deltaTime;
                _leftPriority -= 50 * Time.deltaTime;
                
                if (_leftPriority < 0)
                    _leftPriority = 0;
            }

            _rightBoostPriority += 0.5f * Time.deltaTime;
            _leftBoostPriority -= 10 * Time.deltaTime;

            if (_leftBoostPriority < 0)
                _leftBoostPriority = 0;
        }
    }

    private void ChooseAction()
    {
        _totalPriority = _leftPriority + _rightPriority + _leftSwapPriority + 
                         _rightSwapPriority + _leftBoostPriority + _rightBoostPriority;

        if (_totalPriority > 0)
        {
            _randNum = Random.value;
            
            _leftPercent = _leftPriority / _totalPriority;
            _rightPercent = _rightPriority / _totalPriority;
            _leftSwapPercent = _leftSwapPriority / _totalPriority;
            _rightSwapPercent = _rightSwapPriority / _totalPriority;
            _leftBoostPercent = _leftBoostPriority / _totalPriority;
            _rightBoostPercent = _rightBoostPriority / _totalPriority;

            if (_randNum < _leftPercent)
                MoveLeft();
            else if (_randNum < _leftPercent + _rightPercent)
                MoveRight();
            else if (_randNum < _leftPercent + _rightPercent + _leftSwapPercent)
                SwapLeft();
            else if (_randNum < _leftPercent + _rightPercent + _leftSwapPercent + _rightSwapPercent)
                SwapRight();
            else if (_randNum < _leftPercent + _rightPercent + _leftSwapPercent + _rightSwapPercent + _leftBoostPercent)
                BoostLeft();
            else
                BoostRight();
        }
    }

    private void MoveLeft()
    {
        AIInput.PressKey(name + "Left");
        AIInput.ReleaseKey(name + "Right");

        _leftPriority -= 10 * Time.deltaTime;

        if (_leftPriority < 0)
            _leftPriority = 0;
    }

    private void MoveRight()
    {
        AIInput.PressKey(name + "Right");
        AIInput.ReleaseKey(name + "Left");

        _rightPriority -= 10 * Time.deltaTime;

        if (_rightPriority < 0)
            _rightPriority = 0;
    }

    private void SwapLeft()
    {
        AIInput.PressKey(name + "Left");
        AIInput.ReleaseKey(name + "Right");

        _leftSwapPriority = 0;
    }

    private void SwapRight()
    {
        AIInput.PressKey(name + "Right");
        AIInput.ReleaseKey(name + "Left");

        _rightSwapPriority = 0;
    }

    private void BoostLeft()
    {
        AIInput.PressKey(name + "Left");
        AIInput.ReleaseKey(name + "Right");
        AIInput.PressKey(name + "Ability");
        
        //_leftBoostPriority -= 100;
        
        //if (_leftBoostPriority < 0)
            _leftBoostPriority = 0;
    }

    private void BoostRight()
    {
        AIInput.PressKey(name + "Right");
        AIInput.ReleaseKey(name + "Left");
        AIInput.PressKey(name + "Ability");
        
        //_rightBoostPriority -= 100;
        
        //if (_rightBoostPriority < 0)
            _rightBoostPriority = 0;
    }

    public void Reset()
    {
        _leftPriority = 0;
        _rightPriority = 0;
        _leftSwapPriority = 0;
        _rightSwapPriority = 0;
        _leftBoostPriority = 0;
        _rightBoostPriority = 0;
    }
}
