using UnityEngine;
using System.Collections;

public class CharacterList : MonoBehaviour 
{

    public GameObject[] Characters;
    
    private float _boundaryPos;     //Boundary position of the left/right most characters
    private int _columns;           //Max number of columns in the character list
    private float _rowOffset;       //Position offset
    private float _startYPos;

	// Use this for initialization
	void Start () 
    {
	    _boundaryPos = 500.0f;
        _columns = 5;
        _rowOffset = 150;
        _startYPos = -250;
        
        CalculatePositions();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}
    
    private void CalculatePositions()
    {
        int currentCharNum = 0;
    
        for (int i = 0; i < (float)Characters.Length / _columns; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                if (currentCharNum < Characters.Length)
                {
                    Characters[currentCharNum].transform.position = new Vector3(-_boundaryPos + (j * (_boundaryPos * 2 / (_columns - 1))), 
                                                                            _startYPos - (i * _rowOffset),
                                                                            Characters[j].transform.position.z);
                    
                    //Up
                    if (i - 1 >= 0)
                       Characters[currentCharNum].GetComponent<Character>().Up = Characters[(i - 1) * _columns + j].GetComponent<Character>();
                    else
                    {
                        if ((Mathf.CeilToInt((float)Characters.Length / _columns) - 1) * _columns + j < Characters.Length)
                            Characters[currentCharNum].GetComponent<Character>().Up = Characters[(Mathf.CeilToInt((float)Characters.Length / _columns) - 1) * _columns + j].GetComponent<Character>();
                        else
                            Characters[currentCharNum].GetComponent<Character>().Up = Characters[(Mathf.CeilToInt((float)Characters.Length / _columns) - 2) * _columns + j].GetComponent<Character>();
                    }
                    
                    //Down
                    if ((i + 1) <= (float)Characters.Length / _columns && (i + 1) * _columns + j < Characters.Length)
                        Characters[currentCharNum].GetComponent<Character>().Down = Characters[(i + 1) * _columns + j].GetComponent<Character>();
                    else
                        Characters[currentCharNum].GetComponent<Character>().Down = Characters[j].GetComponent<Character>();
                    
                    //Right
                    if (currentCharNum + 1 < Characters.Length && currentCharNum + 1 < ((i * _columns)) + _columns)
                        Characters[currentCharNum].GetComponent<Character>().Right = Characters[currentCharNum + 1].GetComponent<Character>();
                    else
                        Characters[currentCharNum].GetComponent<Character>().Right = Characters[i * _columns].GetComponent<Character>();
                    
                    //Left
                    if (currentCharNum - 1 >= 0 && currentCharNum - 1 >= i * _columns)
                        Characters[currentCharNum].GetComponent<Character>().Left = Characters[currentCharNum - 1].GetComponent<Character>();
                    else if ((i * _columns) + _columns - 1 < Characters.Length)
                        Characters[currentCharNum].GetComponent<Character>().Left = Characters[(i * _columns) + _columns - 1].GetComponent<Character>();
                    
                }
                currentCharNum++;                                                   
            }      
        }
    }
}
