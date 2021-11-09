using UnityEngine;
using System.Collections;

public class ButtonMap : MonoBehaviour 
{
	private KeyCode[] buttonMap;
    private string[] controllerMap;

	// Use this for initialization
	void Awake () 
	{
		// index 0 = Left, 1 = Right, 2 = Ability, 3 = Down, 4 = Accept
		if (transform.parent.gameObject.name.Contains("1"))
			buttonMap = new KeyCode[]{KeyCode.A, KeyCode.D, KeyCode.W, KeyCode.S, KeyCode.Space };
		else if (transform.parent.gameObject.name.Contains("2")) 
			buttonMap = new KeyCode[]{KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.Return };

        // index 0 = Horiz. Dpad, 1 = Horiz. Analog Stick, 2 = Ability, 3 = Vert Dpad, 4 = Horiz. Analog,
        if ( transform.parent.gameObject.name.Contains("1"))
          controllerMap = new string[] { "P1_DPAD_HOR", "P1_JOYSTICK_HOR", "P1_ABILITY", "P1_DPAD_VERT", "P1_JOYSTICK_VERT" };
        else if ( transform.parent.gameObject.name.Contains("2"))
          controllerMap = new string[] { "P2_DPAD_HOR", "P2_JOYSTICK_HOR", "P2_ABILITY", "P2_DPAD_VERT", "P2_JOYSTICK_VERT" };
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public KeyCode[] GetButtonMap()
	{
		return buttonMap;
	}

  	public string[] GetControllerMap()
  	{
    	return controllerMap;
  	}
}
