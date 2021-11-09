using UnityEngine;
using System.Collections;

public class PlayerAI : MonoBehaviour 
{
    private GameManager GM;
    
    private Player _otherPlayer;
    private float _leftPressTime;
    private float _rightPressTime;
    private float _abilityPressTime;
    private SpriteRenderer _stageSprite;
    private float _stayAwayDistance;
    private float _errorChance;     //Chance the AI makes the incorrect decision
    
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
        
        _errorChance = 0.85f;
    }
    
    // Update is called once per frame
    void Update () 
    {
        
    }
    
    void FixedUpdate()
    {
        
        AIInput.ReleaseKey(name + "Ability");
        
        if (GM.StageSelection.Equals("Bridge"))
            BridgeAI();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.Contains("Player"))
        {  
            if (_otherPlayer.IsPressing)
            {
                _randNum = Random.value;
                
                if (_randNum > _errorChance)
                {
                    if (this.transform.position.x > other.transform.position.x)
                        SwapRight();
                    else
                        SwapLeft();
                }
            }
        }     
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.name.Contains("Player"))
        {  
            if (_otherPlayer.IsPressing)
            {
                _randNum = Random.value;
            
                if (_randNum > _errorChance)
                {
                    if (this.transform.position.x > other.transform.position.x)
                    {
                        SwapRight();
                        
                        if (Mathf.Abs(transform.position.x - (_stageSprite.bounds.extents.x)) > 500)
                            BoostRight();
                    }
                    else
                    {
                        SwapLeft();
                        
                        if (Mathf.Abs(transform.position.x - (-_stageSprite.bounds.extents.x)) > 500)
                            BoostLeft();
                    }
                }
            }
        }   
    }
    
    private void BridgeAI()
    {
        //AI is to the right of opponent
        if (transform.position.x > _otherPlayer.transform.position.x)
        {
            _randNum = Random.value;
            
            if (Mathf.Abs(transform.position.x - (-_stageSprite.bounds.extents.x)) > 500 && Mathf.Abs(transform.position.x - _otherPlayer.transform.position.x) > 250
                && _randNum > _errorChance + 0.14f)
                BoostLeft();
            else
            {
                _randNum = Random.value;
                
                if (_randNum > _errorChance)
                    MoveLeft();
            }
        }
        //AI is to the left of the opponent
        else
        {
            _randNum = Random.value;
            
            if (Mathf.Abs(transform.position.x - (_stageSprite.bounds.extents.x)) > 500 && Mathf.Abs(transform.position.x - _otherPlayer.transform.position.x) > 250
                && _randNum > _errorChance + 0.14f)
                BoostRight();
            else
            {
                _randNum = Random.value;
                
                if (_randNum > _errorChance)
                    MoveRight();
            }
        }
    }
    
    private void MoveLeft()
    {
        AIInput.PressKey(name + "Left");
        AIInput.ReleaseKey(name + "Right");
    }
    
    private void MoveRight()
    {
        AIInput.PressKey(name + "Right");
        AIInput.ReleaseKey(name + "Left");
    }
    
    private void SwapLeft()
    {
        AIInput.PressKey(name + "Left");
        AIInput.ReleaseKey(name + "Right");
    }
    
    private void SwapRight()
    {
        AIInput.PressKey(name + "Right");
        AIInput.ReleaseKey(name + "Left");
    }
    
    private void BoostLeft()
    {
        AIInput.PressKey(name + "Left");
        AIInput.ReleaseKey(name + "Right");
        AIInput.PressKey(name + "Ability");
    }
    
    private void BoostRight()
    {
        AIInput.PressKey(name + "Right");
        AIInput.ReleaseKey(name + "Left");
        AIInput.PressKey(name + "Ability");
    }
    
    private void DoNothing()
    {
        AIInput.ReleaseKey(name + "Right");
        AIInput.ReleaseKey(name + "Left");
    }
    
    public void Reset()
    {

    }
}
