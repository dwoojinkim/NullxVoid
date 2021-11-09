using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    public bool CollidedRight { get; set; }
    public bool CollidedLeft { get; set; }
    public bool Stunned { get; set; }
    public bool DisableInput { get; set; }
    public bool Spawning { get; set; }
    public bool Dead { get; set; }

    public bool IsPressing
    {
        get { return _isPressing; }
        set { _isPressing = value; }
    }

    public float Velocity
    {
        get { return _velocity; }
        set { _velocity = value; }
    }

    public float Charge
    {
        get { return _charge; }
        set { _charge = value; }
    }

    public float CurrentBoost
    {
        get { return _currentBoost; }
        set { _currentBoost = value; }
    }
    
    public bool BoostArmor { get; set; }

    public ParticleSystem ShootEffect
    {
        get { return _shootParticles; }
        set { _shootParticles = value; }
    }
    public ParticleSystem SwapEffect
    {
        get { return _swapParticles; }
        set { _swapParticles = value; }
    }
    public ParticleSystem DeathEffect
    {
        get { return _deathParticles; }
        set { _deathParticles = value; }
    }
    
    public ParticleSystem SlideEffect
    {
        get { return _slideParticles; }
        set { _slideParticles = value; }
    }
    
    public bool FiringProjectile { get; set; }
    public bool RightShoot { get; set; }
    public bool LeftShoot { get; set; }
    public bool LookingLeft { get; set; }

    //Debug
    public bool Mode
    {
        get { return _boost; }
        set { _boost = value; }
    }
    
    private GameManager GM;

    private const int MAX_PROJECTILES = 3;
    private const float MAX_RECOIL = 350f;  //Max amount of additional recoil with max amount of projectile charge.
    private const float DECEL = 2000;       //Normal deceleration rate
    private const float MAX_INV_TIME = 3;   //Max time invulnerable after spawning
    private const float DEATH_TIME = 1.5f;

    public Vector3 startPosition = new Vector3(300, 0, 0);
    public float speed = 500.0f;
    public ButtonMap buttonMap;
    public AudioClip Boost;
    public AudioClip Chargeup1;
    public AudioClip Chargeup2;
    public AudioClip Death;
    public AudioClip Deflect;
    public AudioClip Hit;
    public AudioClip Shoot;
    public AudioClip Swap;
    
    private ParticleSystem _chargeParticles;
    private ParticleSystem _shootParticles;
    private ParticleSystem _slideParticles;
    private ParticleSystem _hitParticles;
    private ParticleSystem _swapParticles;
    private ParticleSystem _deathParticles;
    private ParticleSystem _sparkEffect;
    private Animator _anim;
    private SpriteRenderer _playerSprite;
    
    private int _numProjectiles;
    private float _velocity;
    private float _charge;
    private float _pushSpeed;
    private float _totalStunDuration;
    private float _currentStunDuration;
    private float _pushDecel;
    private float _spawnInvTime; //Time left of spawn invulnerability
    private float _deathTime;   //Time spent dead
    private Level _level;
    private bool _isPressing;   //Whether or not the player is inputting movement
    private bool _boost;    //True: boost ability enabled; False: projectile ability enabled;
    private bool _rightHit; //Hit on the right with projectile
    private bool _leftHit;  //Hit on the left with projectile
    private bool _charging;
    private bool _sparked;
  
    private AudioSource _audio;
    private AudioSource _secondAudio;
    private TextMesh _debugText;

    //Variables in order to implement a projectile deflection system
    //implemented similarly as Just Guards in SCV
    private float _lastRightPress;  //Timestamp of when right movement key was last pressed
    private float _lastLeftPress;   //Timestamp of when left movement key was last pressed
    private float _rightPressTime;  //Total time the right movement key has been pressed
    private float _leftPressTime;   //Total time the left movement key has been pressed
    private float _hitTime;         //Timestamp of when player is hit by projectile

    private float _currentBoost;    //Current amount of time left in the boost
    private bool _rightBoost;
    private bool _leftBoost;
    private Projectile _collidedProjectile;

    void Start()
    {
        if (GameObject.FindGameObjectWithTag("GameManager") != null)
            GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    
        _velocity = 0.0f;
        _isPressing = false;

        CollidedRight = false;
        CollidedLeft = false;

        _level = transform.parent.GetComponent<Level>();

        _boost = true;

        _swapParticles = transform.Find("Effects/SwapEffect").GetComponent<ParticleSystem>();
        _swapParticles.startColor = transform.GetComponentInChildren<SpriteRenderer>().color;
        
        _deathParticles = transform.Find("Effects/DeathEffect").GetComponent<ParticleSystem>();
        _deathParticles.startColor = transform.GetComponentInChildren<SpriteRenderer>().color;
        
        _sparkEffect = transform.Find("Effects/SparkEffect").GetComponent<ParticleSystem>();
        
        _anim = transform.Find("PlayerSprite2").GetComponent<Animator>();

        _charge = 0;
        _pushSpeed = 1000.0f;
        _totalStunDuration = 0.0f;
        _currentStunDuration = 0.0f;
        _currentBoost = 0.0f;
        _pushDecel = 0.0f;

        FiringProjectile = false;

        _rightHit = false;
        _leftHit = false;
        _charging = false;
        _leftBoost = false;
        _rightBoost = false;
        _sparked = false;

        _audio = GetComponent<AudioSource>();
        _secondAudio = transform.FindChild("SecondAudio").GetComponent<AudioSource>();

        _debugText = transform.FindChild("DebugText").GetComponent<TextMesh>();
        _debugText.color = Color.black;
        _debugText.text = "";

        _numProjectiles = MAX_PROJECTILES;
        
        if (name.Contains("1"))
            LookingLeft = false;
        else if (name.Contains("2"))
            LookingLeft = true;
            
        _playerSprite = transform.FindChild("PlayerSprite").GetComponent<SpriteRenderer>();
    }

    //Input needs to be in Update or else there will be dropped input
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
            _boost = !_boost;

        if (!GM.EndRound && !GM.EndOfMatch)
        {
            if (!Stunned && !DisableInput && !Dead)
                PlayerInput();
        }
        else if (LookingLeft)
            _anim.SetInteger("Direction", 0);
        else
            _anim.SetInteger("Direction", 1);
    }

    void FixedUpdate()
    {
        if (!GM.EndRound && !GM.EndOfMatch)
        {
            if (!_boost)
                CheckStun();
            else
            {
                if (!CollidedRight && !CollidedLeft)
                    _sparked = false;
            }
        }
        
        if (Spawning)
        {
            _spawnInvTime -= Time.deltaTime;
            
            _playerSprite.color = new Color(_playerSprite.color.r,
                                            _playerSprite.color.g,
                                            _playerSprite.color.b,
                                            0.5f);
                                                                       
            if (_spawnInvTime <= 0)
                Spawning = false;
        }
        else
            _playerSprite.color = new Color(_playerSprite.color.r,
                                            _playerSprite.color.g,
                                            _playerSprite.color.b,
                                            1f);
                                                                       
        if (Dead)
        {
            _deathTime -= Time.deltaTime;
            if (_deathTime <= 0)
            {
                Dead = false;
                ResetPosition();
            }
            

        }

    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!Spawning)
        {
            if (other.gameObject.name.Contains("Player"))
            {
                if (!other.gameObject.GetComponent<Player>().Spawning)
                {
                    //Collided to the right
                    if (other.transform.position.x > transform.position.x)
                    {
                        CollidedRight = true;
                        CollidedLeft = false;
        
                        if (Stunned)
                            other.transform.GetComponent<Player>().Stunned = true;
                    }
                    //Collided to the left
                    else
                    {
                        CollidedRight = false;
                        CollidedLeft = true;
        
                        if (Stunned)
                            other.transform.GetComponent<Player>().Stunned = true;
                    }
                }
            }
        }

        /*
        if (!GM.EndRound && other.transform.name.Contains("Projectile"))
        {
            _collidedProjectile = other.transform.GetComponent<Projectile>();

            if (name.Equals("Player1") && _collidedProjectile.Owner == 2)
            {
                ProjectileCollision(_collidedProjectile);
            } else if (name.Equals("Player2") && _collidedProjectile.Owner == 1)
            {
                ProjectileCollision(_collidedProjectile);
            }
        }
        */
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!Spawning)
        {
            if (other.gameObject.name.Contains("Player"))
            {
                if (!other.gameObject.GetComponent<Player>().Spawning)
                {
                        //Collided to the right
                    if (other.transform.position.x > transform.position.x)
                    {
                        CollidedRight = true;
                        CollidedLeft = false;
                    }
                    //Collided to the left
                    else
                    {
                        CollidedRight = false;
                        CollidedLeft = true;
                    }
                }
            }
        }

        /*
        if (!GM.EndRound && other.transform.name.Contains("Projectile"))
        {
            _collidedProjectile = other.transform.GetComponent<Projectile>();

            if (name.Equals("Player1") && _collidedProjectile.Owner == 2)
            {
                ProjectileCollision(_collidedProjectile);
            } else if (name.Equals("Player2") && _collidedProjectile.Owner == 1)
            {
                ProjectileCollision(_collidedProjectile);
            }
        }
        */
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        CollidedRight = false;
        CollidedLeft = false;
    }

    private void PlayerInput()
    {
        //Left Swap
        if (GM.GetLeft(name))
        {
            if (CollidedRight && !GM.GetRight(name))
                _level.Swap(gameObject.name);

            if (!_charging)
            {
                _lastLeftPress = Time.time;
                _leftPressTime = 0;
            }
        }
        //Right Swap
        if (GM.GetRight(name))
        {
            if (CollidedLeft && !GM.GetLeft(name))
                _level.Swap(gameObject.name);

            if (!_charging)
            {
                _lastRightPress = Time.time;
                _rightPressTime = 0;
            }
        }
    
        _charging = false;
    
        if (!_charging)
        {
            //When left and right are pressed at the same time
            if ((GM.GetLeft(name) && GM.GetRight(name)) || (!GM.GetLeft(name) && !GM.GetRight(name)))
            {
                if (_velocity != 0)
                    _velocity = 0.0f;

                _isPressing = false;
                
                if (LookingLeft)
                    _anim.SetInteger("Direction", 0);
                else
                    _anim.SetInteger("Direction", 1);
            }
      //Moving left
      else if (GM.GetLeft(name))
            {
                _velocity = -speed;

                _isPressing = true;

                if (CollidedRight)
                {
                    CollidedRight = false;
                    _sparked = false;
                }
                
                LookingLeft = true;

                _leftPressTime += Time.deltaTime;
                
                _anim.SetInteger("Direction", 2);
            }
      //Moving right
      else if (GM.GetRight(name))
            {
                _velocity = speed;

                _isPressing = true;

                if (CollidedLeft)
                {
                    CollidedLeft = false;
                    _sparked = false;
                }
                
                LookingLeft = false;

                _rightPressTime += Time.deltaTime;
                
                _anim.SetInteger("Direction", 3);
            }
        }

/*
        //Ability input
        if (GM.GetAbility(name))
        {
            if (!_boost)
            {
                //if (_numProjectiles > 0)  //Commented out for unlimited projectiles
                ChargeProjectile();
            }
        } else
        {
            if (!_boost)
            {
                if (_charge > 0)
                    ShootProjectile();
            }
        }
        */
    }

    private void ChargeProjectile()
    {
        _charge += Time.deltaTime;
        _charging = true;

        if (!_chargeParticles.enableEmission && _charge > 0.1f)
            _chargeParticles.enableEmission = true;

        if (_charge > 1)
        {
            _chargeParticles.startColor = Color.green;
            _chargeParticles.emissionRate = 50;
        } else if (_charge > 0.5f)
        {
            _chargeParticles.startColor = Color.magenta;
            _chargeParticles.emissionRate = 25;
        } else if (_charge > 0.25f)
        {
            _chargeParticles.startColor = Color.blue;
            _chargeParticles.emissionRate = 10;
        } else
        {
            _chargeParticles.startColor = Color.cyan;
            _chargeParticles.emissionRate = 5;
        }

        if (_audio.clip == null)
        {
            _audio.clip = Chargeup1;
            _audio.Play();
        } else if (_audio.clip == Chargeup1 && !_audio.isPlaying)
        {
            _audio.clip = Chargeup2;
            _audio.Play();
            _audio.loop = true;
        }
    }

    private void ShootProjectile()
    {
        if (_charge > 0)
        {
            FiringProjectile = true;

            _numProjectiles--;

            _secondAudio.clip = Shoot;
            _secondAudio.Play();

            _currentStunDuration = 0;
            
            if (LookingLeft)
            {
                LeftShoot = true;
                _pushSpeed = 1000;

                if (_charge <= 1)
                    _pushSpeed += _charge * MAX_RECOIL;
                else
                    _pushSpeed += MAX_RECOIL;

                _slideParticles.enableEmission = true;
            }
            else
            {
                RightShoot = true;
                _pushSpeed = -1000;

                if (_charge <= 1)
                    _pushSpeed -= _charge * MAX_RECOIL;
                else
                    _pushSpeed -= MAX_RECOIL;
                
                _slideParticles.enableEmission = true;
            }
        }
        
        _charging = false;

        if (_chargeParticles.enableEmission)
            _chargeParticles.enableEmission = false;
        _chargeParticles.Clear();

        _audio.Stop();
        _audio.clip = null;
        _audio.loop = false;
        _currentStunDuration = 0.0f;
    }
/*
    private void ProjectileCollision(Projectile projectile)
    {
        _hitTime = Time.time;

        if (projectile.transform.position.x > transform.position.x)
            _rightHit = true;
        else
            _leftHit = true;

        if (_rightHit)
        {
            if (_rightPressTime < 0.05f && _hitTime - _lastRightPress < 0.15f && !GM.GetRight(name))
            {
                projectile.Deflect();
                _secondAudio.clip = Deflect;
                _secondAudio.Play();
                _rightHit = false;
            } else if (_hitTime - _lastLeftPress < 0.25f)
            {
                //Projectile swap code
                SwapProjectiles();
                _rightHit = false;
            } else
            {
                _pushSpeed = -750;
                
                if (projectile.PowerLevel <= 1)
                    _pushSpeed -= projectile.PowerLevel * MAX_RECOIL;
                else
                    _pushSpeed -= MAX_RECOIL;

                //_hitParticles.transform.position = new Vector3(transform.position.x + transform.GetComponentInChildren<SpriteRenderer>().bounds.extents.x,
                //                transform.position.y, transform.position.z);
                //_hitParticles.Emit(100);
                
                if (projectile.Explosion != null)
                    projectile.Explosion.Emit(50);
                    
                _charge = 0.0f;
                _charging = false;
                _chargeParticles.enableEmission = false;
                _chargeParticles.Clear();
                _audio.Stop();
                _audio.clip = null;

                _secondAudio.clip = Hit;
                _secondAudio.Play();

                RightShoot = false;
                LeftShoot = false;

                projectile.Reset();
            }
        }
        if (_leftHit)
        {
            if (_leftPressTime < 0.05f && _hitTime - _lastLeftPress < 0.15f && !GM.GetLeft(name))
            {
                projectile.Deflect();
                _secondAudio.clip = Deflect;
                _secondAudio.Play();
                _leftHit = false;
            } 
            else if (_hitTime - _lastRightPress < 0.25f)
            {
                //Projectile swap code
                SwapProjectiles();
                _leftHit = false;
            } 
            else
            {
                _pushSpeed = 750;
                
                if (projectile.PowerLevel <= 1)
                    _pushSpeed += projectile.PowerLevel * MAX_RECOIL;
                else
                    _pushSpeed += MAX_RECOIL;

                _hitParticles.transform.position = new Vector3(transform.position.x - transform.GetComponentInChildren<SpriteRenderer>().bounds.extents.x,
                                                       transform.position.y, transform.position.z);
                _hitParticles.Emit(100);
                if (projectile.Explosion != null)
                    projectile.Explosion.Emit(50);
                    
                _charge = 0.0f;
                _charging = false;
                _chargeParticles.enableEmission = false;
                _chargeParticles.Clear();
                _audio.Stop();
                _audio.clip = null;
                
                _secondAudio.clip = Hit;
                _secondAudio.Play();
                
                RightShoot = false;
                LeftShoot = false;
                
                projectile.Reset();
            }
        }
    }
    
    private void SwapProjectiles()
    {
        Vector3 _tempPos = transform.position;
        
        transform.position = new Vector3(_collidedProjectile.transform.position.x, _collidedProjectile.transform.position.y,
                                         _collidedProjectile.transform.position.z);
        _collidedProjectile.transform.position = new Vector3(_tempPos.x, _tempPos.y, _tempPos.z);
        
        if (transform.position.x < _collidedProjectile.transform.position.x)
            SwapEffect.transform.position = new Vector3(transform.position.x - transform.GetComponentInChildren<SpriteRenderer>().bounds.extents.x,
                                                        SwapEffect.transform.position.y, SwapEffect.transform.position.z);
        else
            SwapEffect.transform.position = new Vector3(transform.position.x + transform.GetComponentInChildren<SpriteRenderer>().bounds.extents.x,
                                                        SwapEffect.transform.position.y, SwapEffect.transform.position.z);
                                                        
        SwapEffect.Emit(100);
        SwapSound();
    }
*/
    private void CheckStun()
    {
        #region Old Stun Code
        /*
        if ((_rightHit || RightShoot) && _totalStunDuration > 0)
        {
            _currentStunDuration += Time.deltaTime;

            _velocity = -_pushSpeed;
            
            //_pushSpeed += _pushDecel * Time.deltaTime;

            _slideParticles.enableEmission = true;
            _slideParticles.transform.eulerAngles = new Vector3(0, 0, 0);

            if (_currentStunDuration >= _totalStunDuration)
            {
                _currentStunDuration = 0;
                _totalStunDuration = 0;
                _rightHit = false;
                RightShoot = false;
                _velocity = 0;
                _slideParticles.enableEmission = false;
                //_pushSpeed = 0;
            }
        } else if ((_leftHit || LeftShoot) && _totalStunDuration > 0)
        {
            _currentStunDuration += Time.deltaTime;

            _velocity = _pushSpeed;
            
            //_pushSpeed -= _pushDecel * Time.deltaTime;

            _slideParticles.enableEmission = true;
            _slideParticles.transform.eulerAngles = new Vector3(0, 180, 0);

            if (_currentStunDuration >= _totalStunDuration)
            {
                _currentStunDuration = 0;
                _totalStunDuration = 0;
                _leftHit = false;
                LeftShoot = false;
                _velocity = 0;
                _slideParticles.enableEmission = false;
                //_pushSpeed = 0;
            }
        }
        */
        #endregion

        //Projectile hit
        if (_leftHit && _pushSpeed > 0)
        {
            _velocity = _pushSpeed;

            if (GM.GetLeft(name))
                _pushDecel = DECEL * 1.25f;
            else
                _pushDecel = DECEL;

            _pushSpeed -= _pushDecel * Time.deltaTime;

            if (_pushSpeed <= 0)
            {
                _leftHit = false;
                _slideParticles.enableEmission = false; 
                _velocity = 0;
                _pushSpeed = 0;
            }
        }
        else if (_rightHit && _pushSpeed < 0)
        {
            _velocity = _pushSpeed;
            
            if (GM.GetRight(name))
                _pushDecel = DECEL * 1.25f;
            else
                _pushDecel = DECEL;
            
            _pushSpeed += _pushDecel * Time.deltaTime;
            
            if (_pushSpeed >= 0)
            {
                _rightHit = false;
                _slideParticles.enableEmission = false; 
                _velocity = 0;
                _pushSpeed = 0;
            }
        }
        //Projectile Recoil
        else if (LeftShoot && _pushSpeed > 0)
        {
            _velocity = _pushSpeed;

            if (GM.GetLeft(name))
                _pushDecel = DECEL * 2;
            else
                _pushDecel = DECEL;

            _pushSpeed -= _pushDecel * Time.deltaTime;

            if (GM.GetAbility(name))
                ChargeProjectile();
            else
                ShootProjectile();

            if (_pushSpeed <= 0)
            {
                LeftShoot = false;
                _slideParticles.enableEmission = false; 
                _velocity = 0;
                _pushSpeed = 0;
            }
        }
        else if (RightShoot && _pushSpeed < 0)
        {
            _velocity = _pushSpeed;

            if (GM.GetRight(name))
                _pushDecel = DECEL * 2;
            else
                _pushDecel = DECEL;
            
            if (GM.GetAbility(name))
                ChargeProjectile();
            else
                ShootProjectile();
            
            _pushSpeed += _pushDecel * Time.deltaTime;
            
            if (_pushSpeed >= 0)
            {
                RightShoot = false;
                _slideParticles.enableEmission = false; 
                _velocity = 0;
                _pushSpeed = 0;
            }
        }
    }

    public void Reset()
    {
        _rightHit = false;
        _leftHit = false;
        RightShoot = false;
        LeftShoot = false;
        _leftBoost = false;
        _rightBoost = false;
        _charging = false;
        Stunned = false;
        _velocity = 0;

        _currentBoost = 0;
        transform.Find("PlayerSprite").transform.localScale = new Vector3(1, 1, 1);
        
        if (name.Contains("1"))
            LookingLeft = false;
        else if (name.Contains("2"))
            LookingLeft = true;

        if (transform.GetComponent<PlayerAI>() != null)
            transform.GetComponent<PlayerAI>().Reset();
            
        if (transform.GetComponent<ChargeShot>() != null)
            transform.GetComponent<ChargeShot>().Reset();
            
        _deathTime = DEATH_TIME;
        Dead = true;
    }
    
    public void Respawn()
    {
        _rightHit = false;
        _leftHit = false;
        RightShoot = false;
        LeftShoot = false;
        _leftBoost = false;
        _rightBoost = false;
        _charging = false;
        Stunned = false;
        _velocity = 0;
        
        _currentBoost = 0;
        transform.Find("PlayerSprite").transform.localScale = new Vector3(1, 1, 1);
        
        if (name.Contains("1"))
            LookingLeft = false;
        else if (name.Contains("2"))
            LookingLeft = true;
        
        if (transform.GetComponent<PlayerAI>() != null)
            transform.GetComponent<PlayerAI>().Reset();
        
        if (transform.GetComponent<ChargeShot>() != null)
            transform.GetComponent<ChargeShot>().Reset();
        
        _deathTime = DEATH_TIME;
        Dead = true;
    }
    
    public void ResetPosition()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnInvTime = MAX_INV_TIME;
        Spawning = true;
    }

    public void EndRound(bool won)
    {
        _charge = 0;
        _totalStunDuration = 0;
        //_chargeParticles.Clear();
        _velocity = 0.0f;
        FiringProjectile = false;

        _audio.Stop();
        _audio.clip = null;
        _audio.loop = false;
        _secondAudio.Stop();

        //_chargeParticles.enableEmission = false;

        _numProjectiles = MAX_PROJECTILES;
    
        if (!won)
        {
            _deathParticles.Play();
            
            if (_audio.isPlaying)
            {
                _secondAudio.clip = Death;
                _secondAudio.Play();
            }
            else
            {
                _audio.clip = Death;
                _audio.Play();
            }
            transform.Find("PlayerSprite").transform.localScale = new Vector3(0, 0, 0);
        }
    }

    public void AddProjectile()
    {
        if (_numProjectiles < MAX_PROJECTILES)
        {
            _numProjectiles++;
        }
    }
  
    public void SwapSound()
    {
        if (!_audio.isPlaying)
        {
            _audio.clip = Swap;
            _audio.Play();
        }
        else
        {
            _secondAudio.clip = Swap;
            _secondAudio.Play();
        }
    }
    
    public void SparkEffect(int emitAmount)
    {
        if (!_sparked)
        {
            _sparkEffect.Emit(emitAmount);
            _sparked = true;
        }
    }
    
    /*
    you hold the projectile button, it starts charging, you can still move around, you release the button and it fires,
    the direction it fires depends on the last direction you pressed. if you are still holding a direction after you fire,
    you influence the recoil by going further back or less back
    if a projectile is coming toward you, you can boost forward to armor through it or attempt to swap with it which gives you a boost instead (it could also just swap you)
    spamming projectiles is inherently harmful because of the recoil
    charging projectiles is better but takes a bit more time
    and has a higher recoil
    */
}

