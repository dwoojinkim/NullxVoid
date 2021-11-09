using UnityEngine;
using System.Collections;

public class MatchOptions : MonoBehaviour 
{
	public Button rematch;
	public Button changeChar;
	public Button changeStage;
	public AudioClip Hover;
	public AudioClip Choose;

	private Button _selectedButton;
	private AudioSource _audio;

	// Use this for initialization
	void Start () 
	{
		_selectedButton = rematch;
		rematch.Selected = true;
		
		_audio = transform.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
	
	public void SelectUp()
	{
		_selectedButton.Selected = false;
		_selectedButton = _selectedButton.Up;
		_selectedButton.Selected = true;
		
		_audio.clip = Hover;
		_audio.Play();
	}
	
	public void SelectDown()
	{
		_selectedButton.Selected = false;
		_selectedButton = _selectedButton.Down;
		_selectedButton.Selected = true;
		
		_audio.clip = Hover;
		_audio.Play();
	}
	
	public string Select()
	{
		_selectedButton.Chosen = true;
		_selectedButton.Selected = false;
		
		_audio.clip = Choose;
		_audio.Play();
		
		return _selectedButton.name;
	}
	
	public void Reset()
	{
		_selectedButton.Chosen = false;
		_selectedButton.Selected = false;
		_selectedButton = rematch;
		_selectedButton.Selected = true;
	}
}
