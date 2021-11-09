using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour
{
    private const float SWAP_OFFSET = 2.0f;
    private const int MAX_STOCK = 5;

    private GameManager GM;

    public SpriteRenderer StageSprite {get; set;}

    public Player player1;
    public Player player2;
    public GUIText p1ScoreText;
    public GUIText p2ScoreText; 
    private SpriteRenderer stageSprite;
    public SpriteRenderer backgroundSprite;
    public EndMatchScreen matchScreen;
    public AudioClip[] bgMusicList;
    
    private Vector3 _tempPos;
    private float _playerSpriteWidth;
    private float _stageWidth;
    private int _p1Score;
    private int _p2Score;
    private TextMesh debugText;
    private float _midpoint;    //Midpoint between both players
    private ProjectilePool _projPool;
    private WinnerBanner _winnerBanner;
    private Color _p1ScoreTextColor;
    private Color _p2ScoreTextColor;
    private AudioSource _bgMusic;
    private CameraMovement _camera;
    private TextMesh _p1ScoreFlash;
    private TextMesh _p2ScoreFlash;

    // Use this for initialization
    void Awake()
    {
        if (GameObject.FindGameObjectWithTag("GameManager") != null)
        {
            GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            player1.transform.Find("PlayerSprite").GetComponent<SpriteRenderer>().color = GM.P1CharChoice;
            player2.transform.Find("PlayerSprite").GetComponent<SpriteRenderer>().color = GM.P2CharChoice;
            
            backgroundSprite.sprite = GM.StageBG;
        }
        else
        {
            Instantiate(Resources.Load("GameManager"));
            GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }
    
        CheckStageSelection();
    
        _tempPos = Vector3.zero;
        
        _p1Score = MAX_STOCK;
        _p2Score = MAX_STOCK;
        
        debugText = transform.FindChild("DebugText").GetComponent<TextMesh>();
        debugText.text = "";
        
        _projPool = GetComponent<ProjectilePool>();

        //stageSprite = transform.Find("StageSprite").GetComponent<SpriteRenderer>();

        //_winnerBanner = stageSprite.transform.parent.GetComponentInChildren<WinnerBanner>();

        _p1ScoreTextColor = player1.GetComponentInChildren<SpriteRenderer>().color;
        _p2ScoreTextColor = player2.GetComponentInChildren<SpriteRenderer>().color;
        //p1ScoreText.color = _p1ScoreTextColor;
        //p2ScoreText.color = _p2ScoreTextColor;
        
        _bgMusic = transform.GetComponent<AudioSource>();
        _bgMusic.clip = bgMusicList [Random.Range(0, bgMusicList.Length)];
        _bgMusic.Play();
        
        _camera = Camera.main.GetComponent<CameraMovement>();
        
        //_p1ScoreFlash = p1ScoreText.transform.Find("Flash").GetComponent<TextMesh>();
        //_p2ScoreFlash = p2ScoreText.transform.Find("Flash").GetComponent<TextMesh>();
    }
    
    void Start()
    {   
        if (GameObject.FindGameObjectWithTag("GameManager") != null)
        {
            if (GM.PlayMode == GameManager.PlayerMode.PvC)
            {
                player2.gameObject.AddComponent<PlayerAI>();
            }
            else if (GM.PlayMode == GameManager.PlayerMode.CvC)
            {
                player1.gameObject.AddComponent<PlayerAI>();
                player2.gameObject.AddComponent<PlayerAI>();
            }
            
            player1.gameObject.AddComponent(GM.P1Char);
            player2.gameObject.AddComponent(GM.P2Char);
        }
        
        //Add Da stage prefab Hee-yuh
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.LoadLevel(0);   //Back to title screen
    }

    void FixedUpdate()
    {
        if (!GM.EndRound && !GM.EndOfMatch)
        {
            PlayerCollisions();

            if (Mathf.Abs(player1.transform.position.x - player2.transform.position.x) < (player1.GetComponentInChildren<SpriteRenderer>().bounds.extents.x * 2.0f))
            {
                if (player1.CollidedRight)
                {
                    _midpoint = (player1.transform.position.x + player2.transform.position.x) / 2.0f;

                    //Adjust player position if they are inside each other
                    player1.transform.position = new Vector3(_midpoint - player1.GetComponentInChildren<SpriteRenderer>().bounds.extents.x,
                                                             player1.transform.position.y, player1.transform.position.z);
                    player2.transform.position = new Vector3(_midpoint + player2.GetComponentInChildren<SpriteRenderer>().bounds.extents.x,
                                                             player2.transform.position.y, player2.transform.position.z);
                } else if (player1.CollidedLeft)
                {
                    _midpoint = (player1.transform.position.x + player2.transform.position.x) / 2.0f;
                    
                    //Adjust player position if they are inside each other
                    player1.transform.position = new Vector3(_midpoint + player1.GetComponentInChildren<SpriteRenderer>().bounds.extents.x,
                                                             player1.transform.position.y, player1.transform.position.z);
                    player2.transform.position = new Vector3(_midpoint - player2.GetComponentInChildren<SpriteRenderer>().bounds.extents.x,
                                                             player2.transform.position.y, player2.transform.position.z);
                }
            }

            //Player movement
            player1.transform.position = new Vector3(player1.transform.position.x + (player1.Velocity * Time.deltaTime),
                                                    player1.transform.position.y, player1.transform.position.z);
            player2.transform.position = new Vector3(player2.transform.position.x + (player2.Velocity * Time.deltaTime),
                                                    player2.transform.position.y, player2.transform.position.z);

            //PlayerProjectiles();
        }

        if (!GM.EndOfMatch)
            CheckWinLose();

        UpdateScoreText();
    }

    private void PlayerCollisions()
    {
        if ((player1.CollidedRight && player2.CollidedLeft) || (player2.CollidedRight && player1.CollidedLeft))
        {
            if (player1.Velocity == -player2.Velocity)
            {
                if (player1.Velocity != 0 && player2.Velocity != 0)
                {
                    if (Mathf.Abs(player1.Velocity) > 500 && Mathf.Abs(player2.Velocity) > 500)
                    {
                        player1.SparkEffect(50);
                        player2.SparkEffect(50);
                    }
                    player1.SparkEffect(10);
                    player2.SparkEffect(10);
                }
                player1.Velocity = 0;
                player2.Velocity = 0;
            } else if (Mathf.Abs(player1.Velocity) > Mathf.Abs(player2.Velocity))
                SetVelocity();
            else if (Mathf.Abs(player1.Velocity) < Mathf.Abs(player2.Velocity))
                SetVelocity();
        }
    }

    public void Swap(string swapper)
    {
        if (swapper.Equals("Player1"))
        {
            if (player1.transform.position.x < player2.transform.position.x && player2.Velocity < 0)
            {
                if (player2.GetComponent<Boost>() == null || (player2.GetComponent<Boost>() != null && !player2.GetComponent<Boost>().BoostArmor))
                {
                    SwapPositions();
                
                    player1.SwapEffect.Emit(80);
                    player1.SwapSound();
                }
            }    
            else if (player1.transform.position.x > player2.transform.position.x && player2.Velocity > 0)
            {
                if (player2.GetComponent<Boost>() == null || (player2.GetComponent<Boost>() != null && !player2.GetComponent<Boost>().BoostArmor))
                {
                    SwapPositions();
                
                    player1.SwapEffect.Emit(80);
                    player1.SwapSound();
                }
            }

            player1.AddProjectile();
        } else if (swapper.Equals("Player2"))
        {
            if (player2.transform.position.x < player1.transform.position.x && player1.Velocity < 0)
            {
                if (player1.GetComponent<Boost>() == null || (player1.GetComponent<Boost>() != null && !player1.GetComponent<Boost>().BoostArmor))
                {
                    SwapPositions();
                
                    player2.SwapEffect.Emit(80);
                    player2.SwapSound();
                }
            }
            else if (player2.transform.position.x > player1.transform.position.x && player1.Velocity > 0 && !player1.BoostArmor)
            {
                if (player1.GetComponent<Boost>() == null || (player1.GetComponent<Boost>() != null && !player1.GetComponent<Boost>().BoostArmor))
                {
                    SwapPositions();
                
                    player2.SwapEffect.Emit(80);
                    player2.SwapSound();
                }
            }

            player2.AddProjectile();
        }
    }

    private void SwapPositions()
    {
        _tempPos = player1.transform.position;
        if (player1.transform.position.x < player2.transform.position.x)
        {
            player1.transform.position = new Vector3(player2.transform.position.x + SWAP_OFFSET, player2.transform.position.y,
                                                     player2.transform.position.z);
            player2.transform.position = new Vector3(_tempPos.x - SWAP_OFFSET, _tempPos.y, _tempPos.z);

            player1.SwapEffect.transform.position = new Vector3(player1.transform.position.x - player1.GetComponentInChildren<SpriteRenderer>().bounds.extents.x,
                                                                 player1.SwapEffect.transform.position.y, player1.SwapEffect.transform.position.z);
        } else
        {
            player1.transform.position = new Vector3(player2.transform.position.x - SWAP_OFFSET, player2.transform.position.y,
                                                     player2.transform.position.z);
            player2.transform.position = new Vector3(_tempPos.x + SWAP_OFFSET, _tempPos.y, _tempPos.z);

            player1.SwapEffect.transform.position = new Vector3(player1.transform.position.x + player1.GetComponentInChildren<SpriteRenderer>().bounds.extents.x,
                                                                 player1.SwapEffect.transform.position.y, player1.SwapEffect.transform.position.z);
        }
        
        player1.CollidedRight = false;
        player1.CollidedLeft = false;
        
        player2.CollidedRight = false;
        player2.CollidedLeft = false;
    }

    private void CheckWinLose()
    {
        _playerSpriteWidth = player1.gameObject.GetComponentInChildren<SpriteRenderer>().bounds.extents.x;
        _stageWidth = StageSprite.bounds.extents.x;

        //if (!_winnerBanner.Moving)
        //{
        /*
            if ((player1.transform.position.x + _playerSpriteWidth <= -_stageWidth && player2.transform.position.x - _playerSpriteWidth >= _stageWidth) || 
                (player1.transform.position.x - _playerSpriteWidth >= _stageWidth && player2.transform.position.x + _playerSpriteWidth <= -_stageWidth))
            {
                //Tie/draw logic
                _p1Score--;
                _p2Score--;
                _winnerBanner.BannerColor = Color.black;
                _winnerBanner.LeftWin();
                player1.EndRound(false);
                player2.EndRound(false);
                player1.Reset();
                player2.Reset();
            
            //_p1ScoreFlash.color = new Color(_p1ScoreFlash.color.r, _p1ScoreFlash.color.g, _p1ScoreFlash.color.b, 1);
                //_p2ScoreFlash.color = new Color(_p2ScoreFlash.color.r, _p2ScoreFlash.color.g, _p2ScoreFlash.color.b, 1);
                
                //p1ScoreText.transform.GetComponentInChildren<ParticleSystem>().Emit(100);
                //p2ScoreText.transform.GetComponentInChildren<ParticleSystem>().Emit(100);
                
                if (_p1Score > 0 && _p2Score > 0)
                    _camera.Shake(40, 1f);
            } else
            {*/
                //Player 2 Left Win
                if ((player1.transform.position.x + _playerSpriteWidth) <= -_stageWidth && !player1.Dead)
                {
                    _p1Score--;
                    //_winnerBanner.LeftWin();
                    //_winnerBanner.BannerColor = player2.GetComponentInChildren<SpriteRenderer>().color;
                    player1.EndRound(false);
                    player2.EndRound(true);
                    player1.Reset();
                
                //_p2ScoreFlash.color = new Color(_p2ScoreFlash.color.r, _p2ScoreFlash.color.g, _p2ScoreFlash.color.b, 1);
                    //p2ScoreText.transform.GetComponentInChildren<ParticleSystem>().Emit(100);
                    
                    if (_p1Score > 0)
                        _camera.Shake(40, 1f);
                }
                //Player 2 Right Win
                else if (player1.transform.position.x - _playerSpriteWidth >= _stageWidth && !player1.Dead)
                {
                    _p1Score--;
                    //_winnerBanner.RightWin();
                    //_winnerBanner.BannerColor = player2.GetComponentInChildren<SpriteRenderer>().color;
                    player1.EndRound(false);
                    player2.EndRound(true);
                    player1.Reset();
                
                    //_p2ScoreFlash.color = new Color(_p2ScoreFlash.color.r, _p2ScoreFlash.color.g, _p2ScoreFlash.color.b, 1);
                    //p2ScoreText.transform.GetComponentInChildren<ParticleSystem>().Emit(100);
                    
                    if (_p1Score > 0)
                        _camera.Shake(40, 1f);
                }

                //Player 1 Left Win
                else if (player2.transform.position.x + _playerSpriteWidth <= -_stageWidth && !player2.Dead)
                {
                    _p2Score--;
                    //_winnerBanner.LeftWin();
                    //_winnerBanner.BannerColor = player1.GetComponentInChildren<SpriteRenderer>().color;
                    player1.EndRound(true);
                    player2.EndRound(false);
                    player2.Reset();
                
                    //_p1ScoreFlash.color = new Color(_p1ScoreFlash.color.r, _p1ScoreFlash.color.g, _p1ScoreFlash.color.b, 1);
                    //p1ScoreText.transform.GetComponentInChildren<ParticleSystem>().Emit(100);
                    
                    if (_p2Score > 0)
                        _camera.Shake(40, 1f);
                }
                //Player 1 Right Win
                else if (player2.transform.position.x - _playerSpriteWidth >= _stageWidth && !player2.Dead)
                {
                    _p2Score--;
                    //_winnerBanner.RightWin();
                    //_winnerBanner.BannerColor = player1.GetComponentInChildren<SpriteRenderer>().color;
                    player1.EndRound(true);
                    player2.EndRound(false);
                    player2.Reset();
                
                    //_p1ScoreFlash.color = new Color(_p1ScoreFlash.color.r, _p1ScoreFlash.color.g, _p1ScoreFlash.color.b, 1);
                    //p1ScoreText.transform.GetComponentInChildren<ParticleSystem>().Emit(100);
                    
                    if (_p2Score > 0)
                        _camera.Shake(40, 1f);
                }
            //}
        //}

        if (_p1Score == 0 && _p2Score == 0)
        {
            EndMatch("Draw");
            _bgMusic.Stop();
        } else if (_p2Score == 0)
        {
            //End of match logic
            EndMatch("Player1");
            _bgMusic.Stop();
        } else if (_p1Score == 0)
        {
            EndMatch("Player2");
            _bgMusic.Stop();
        } else
        {
            //if (_winnerBanner.RtWin && _winnerBanner.transform.position.x <= 0.0f)
            //    ResetStage();
            //else if (_winnerBanner.LtWin && _winnerBanner.transform.position.x >= 0.0f)
            //    ResetStage();
        }
    }

    private void ResetStage()
    {
        player1.Reset();
        player2.Reset();
        
        StageSprite.transform.parent.GetComponent<Stage>().Reset();
        stageSprite.transform.localScale = new Vector3(30, 1, 1);
    }

    private void SetVelocity()
    {
        if (player1.Velocity > 0 && player2.Velocity > 0)
        {
            if (player1.Velocity > player2.Velocity)
                player2.Velocity = player1.Velocity;
            else
                player1.Velocity = player2.Velocity;
        } else if (player1.Velocity < 0 && player2.Velocity < 0)
        {
            if (player1.Velocity < player2.Velocity)
                player2.Velocity = player1.Velocity;
            else
                player1.Velocity = player2.Velocity;
        } else
        {
            if (!player1.IsPressing || !player2.IsPressing)
            {
                //If player runs into opponent standing still
                if (player1.Velocity == 0 && player1.CollidedRight && player2.Velocity < 0)
                {
                    player1.Velocity = player2.Velocity;
                    player2.Velocity = player1.Velocity;
                }
                else if (player1.Velocity == 0 && player1.CollidedLeft && player2.Velocity > 0)
                {
                    player1.Velocity = player2.Velocity;
                    player2.Velocity = player1.Velocity;
                }
                else if (player2.Velocity == 0 && player2.CollidedRight && player1.Velocity < 0)
                {
                    player2.Velocity = player1.Velocity;
                    player1.Velocity = player2.Velocity;
                }
                else if (player2.Velocity == 0 && player2.CollidedLeft && player1.Velocity > 0)
                {
                    player2.Velocity = player1.Velocity;
                    player1.Velocity = player2.Velocity;
                }
                //If player moves away from opponent standing still (prevent them from stick to each other
                else if (player1.Velocity == 0 && player1.CollidedRight && player2.Velocity > 0)
                {
                    //player1.Velocity = 0;
                    //player2.Velocity = player2.Velocity;
                }
                else if (player1.Velocity == 0 && player1.CollidedLeft && player2.Velocity < 0)
                {
                    //player1.Velocity = 0;
                    //player2.Velocity = player2.Velocity;
                }
                else if (player2.Velocity == 0 && player2.CollidedRight && player1.Velocity > 0)
                {
                    //player1.Velocity = player1.Velocity;
                    //player2.Velocity = 0;
                }
                else if (player2.Velocity == 0 && player2.CollidedLeft && player1.Velocity < 0)
                {
                    //player1.Velocity = player1.Velocity;
                    //player2.Velocity = 0;
                }
                else
                {
                    player1.Velocity = player1.Velocity + player2.Velocity;
                    player2.Velocity = player1.Velocity;
                }
            }
            else
            {
                if (!player1.RightShoot && !player1.LeftShoot && !player2.RightShoot && !player2.LeftShoot)
                {
                    player1.Velocity = player1.Velocity + player2.Velocity;
                    player2.Velocity = player1.Velocity;
                }
                else if (player1.RightShoot || player1.LeftShoot || player2.RightShoot || player2.LeftShoot)
                {
                    player1.Velocity = player1.Velocity + player2.Velocity;
                    player2.Velocity = player1.Velocity;
                }
            }
        }
    }

    /*
    private void PlayerProjectiles()
    {
        if (player1.FiringProjectile)
        {
            Projectile projectile = GetComponent<ProjectilePool>().RequestObject();
            
            if (player1.LookingLeft)
                projectile.MoveLeft = true;
            else
                projectile.MoveLeft = false;
            
            projectile.enabled = true;
            projectile.Owner = 1;
            projectile.ClearParticles();
            
            if (player1.Charge > 1)
                projectile.PowerLevel = 1;
            else
                projectile.PowerLevel = player1.Charge;
            
            if (projectile.MoveLeft)
            {
                player1.ShootEffect.transform.eulerAngles = new Vector3(0, -90, 0);
                projectile.transform.position = new Vector3(player1.transform.position.x,// - player1.GetComponentInChildren<SpriteRenderer>().bounds.extents.x,
                                                            transform.position.y, transform.position.z);

                projectile.trail.GetComponent<ParticleSystem>().Clear();
            } else
            {
                player1.ShootEffect.transform.eulerAngles = new Vector3(0, 90, 0);
                projectile.transform.position = new Vector3(player1.transform.position.x,// + player1.GetComponentInChildren<SpriteRenderer>().bounds.extents.x,
                                                            transform.position.y, transform.position.z);

                projectile.trail.GetComponent<ParticleSystem>().Clear();
            }

            player1.ShootEffect.Emit(40);
            
            player1.Charge = 0;
            player1.FiringProjectile = false;
        }
        
        if (player2.FiringProjectile)
        {
            Projectile projectile = GetComponent<ProjectilePool>().RequestObject();
            
            if (player2.LookingLeft)
                projectile.MoveLeft = true;
            else
                projectile.MoveLeft = false;
            
            projectile.enabled = true;
            projectile.Owner = 2;
            projectile.ClearParticles();
            
            if (player2.Charge > 1)
                projectile.PowerLevel = 1;
            else
                projectile.PowerLevel = player2.Charge;
            
            if (projectile.MoveLeft)
            {
                player2.ShootEffect.transform.eulerAngles = new Vector3(0, -90, 0);
                projectile.transform.position = new Vector3(player2.transform.position.x,// - player2.GetComponentInChildren<SpriteRenderer>().bounds.extents.x,
                                                            transform.position.y, transform.position.z);

                projectile.trail.GetComponent<ParticleSystem>().Clear();
            } else
            {
                player2.ShootEffect.transform.eulerAngles = new Vector3(0, 90, 0);
                projectile.transform.position = new Vector3(player2.transform.position.x,// + player2.GetComponentInChildren<SpriteRenderer>().bounds.extents.x,
                                                            transform.position.y, transform.position.z);

                projectile.trail.GetComponent<ParticleSystem>().Clear();
            }
            
            player2.ShootEffect.Emit(40);
            
            player2.Charge = 0;
            player2.FiringProjectile = false;
        } 
    }*/

    private void UpdateScoreText()
    {
        p1ScoreText.text = _p1Score.ToString();
        p2ScoreText.text = _p2Score.ToString();

        //_p1ScoreFlash.text = _p1Score.ToString();
        //_p2ScoreFlash.text = _p2Score.ToString();
        
        //if (_p1ScoreFlash.color.a > 0)
        //    _p1ScoreFlash.color = new Color(_p1ScoreFlash.color.r, _p1ScoreFlash.color.g, _p1ScoreFlash.color.b, _p1ScoreFlash.color.a - (0.45f * Time.deltaTime));
            
        //if (_p2ScoreFlash.color.a > 0)
        //    _p2ScoreFlash.color = new Color(_p2ScoreFlash.color.r, _p2ScoreFlash.color.g, _p2ScoreFlash.color.b, _p2ScoreFlash.color.a - (0.45f * Time.deltaTime));
    }

    private void EndMatch(string winner)
    {
        matchScreen.BeginScreen(winner);
        GM.EndOfMatch = true;
    }
    
    public void Rematch()
    {
        matchScreen.EndScreen();
        GM.EndOfMatch = false;
        GM.EndRound = false;
        ResetStage();
        _p1Score = MAX_STOCK;
        _p2Score = MAX_STOCK;
        
        _bgMusic.clip = bgMusicList [Random.Range(0, bgMusicList.Length)];
        _bgMusic.Play();
    }
    
    public void CheckStageSelection()
    {
        GameObject LevelPrefab = (GameObject)Instantiate(Resources.Load("Stages/" + GM.StageSelection + "/" + GM.StageSelection));
        //gameObject.AddComponent(GM.StageSelection);
        
        LevelPrefab.transform.parent = this.transform;
        LevelPrefab.name = GM.StageSelection;
    }
}
