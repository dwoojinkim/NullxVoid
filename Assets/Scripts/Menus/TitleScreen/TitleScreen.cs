using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour 
{
    private GameManager GM;

    public SpriteRenderer TransitionSprite;

	//private AudioSource _bgMusic;
	private AudioSource _menuAudio;
    private bool _screenTransitionIn;
    private bool _screenTransitionOut;

	// Use this for initialization
	void Start () 
	{
        if (GameObject.FindGameObjectWithTag("GameManager") == null)
            Instantiate(Resources.Load("GameManager"));
        
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        TransitionSprite.color = new Color(0, 0, 0, 1);

        _screenTransitionOut = true;
    }
    
	// Update is called once per frame
	void Update () 
	{
        if (!_screenTransitionOut && !_screenTransitionIn)
		    PlayerInput();
	}
    
    void FixedUpdate()
    {
        if (_screenTransitionIn)
        {
            TransitionSprite.color = new Color(TransitionSprite.color.r,
                                                      TransitionSprite.color.g,
                                                      TransitionSprite.color.b,
                                                      TransitionSprite.color.a + (1.25f * Time.deltaTime));
            
            if (TransitionSprite.color.a >= 1)
                Application.LoadLevel("MainMenuScreen");
        }
        if (_screenTransitionOut)
        {
            TransitionSprite.color = new Color(TransitionSprite.color.r,
                                               TransitionSprite.color.g,
                                               TransitionSprite.color.b,
                                               TransitionSprite.color.a - (1.25f * Time.deltaTime));

            if (TransitionSprite.color.a <= 0)
                _screenTransitionOut = false;
        }


    }
	
	private void PlayerInput()
	{
		if (GM.GetAcceptDown("1"))
		{
            _screenTransitionIn = true;
        }
	}
}
