using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour 
{
    public bool Enabled;

	public bool Selected { get; set; }	//Hovered over button
	public bool Chosen { get; set; }	//Actual picked/selected/chosen button
	public Button Up;
	public Button Down;
	public Button Left;
	public Button Right;

	private GUIText textGUI;
    private TextMesh textMesh;
    private SpriteRenderer _buttonHighlight;
	
	// Use this for initialization
	void Awake () 
	{
        if (transform.Find("Text").GetComponent<GUIText>() != null)
            textGUI = transform.Find("Text").GetComponent<GUIText>();
        if (transform.Find("Text").GetComponent<TextMesh>() != null)
            textMesh = transform.Find("Text").GetComponent<TextMesh>();
            
        _buttonHighlight = transform.Find("ButtonHighlight").GetComponent<SpriteRenderer>();
		Selected = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
        if (Enabled)
        {
	    	if (Selected)
            {
                if (textMesh != null)
	    		    textMesh.color = Color.cyan;
                if (textGUI != null)
                    textGUI.color = Color.cyan;
                _buttonHighlight.color = new Color(1, 1, 1, 0.75f);
                //transform.localPosition = new Vector3(325, transform.localPosition.y, transform.localPosition.z);
            }
	    	else if (Chosen)
            {
	    		textMesh.color = Color.green;
                if (textGUI != null)
                    textGUI.color = Color.green;
            }
		    else
            {
                if (textMesh != null)
		    	    textMesh.color = Color.gray;
                if (textGUI != null)
                    textGUI.color = Color.gray;
                _buttonHighlight.color = new Color(1, 1, 1, 0);
                //transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
            }
        }
        else
        {
            if (textMesh != null)
                textMesh.color = Color.black;
            if (textGUI != null)
                textGUI.color = Color.black;
            _buttonHighlight.color = new Color(0, 0, 0, 0.75f);
            //transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
        }
	}

    public Button GetUp()
    {
        if (Up.Enabled)
            return Up;
        else
            return Up.GetUp();
    }
    public Button GetDown()
    {
        if (Down.Enabled)
            return Down;
        else
            return Down.GetDown();
    }

    public Button GetLeft()
    {
        if (Left.Enabled)
            return Left;
        else
            return Left.GetLeft();
    }

    public Button GetRight()
    {
        if (Right.Enabled)
            return Right;
        else
            return Right.GetRight();
    }
}
