using UnityEngine;
using System.Collections;

public class ModelFunctionality : MonoBehaviour {
	
	//Colors
	public Color player1Color = Color.green;
	public Color player2Color = Color.blue;
	public Color enemyColor = Color.red;
	
	// Use this for initialization
	void Start () {
		UpdateModelColor(gameObject.tag);
	}
	
	public void UpdateModelColor(string tag)
	{
		if (tag == "Player1")
		{
			gameObject.renderer.material.color = player1Color;
		}
		else if (tag == "Player2")
		{
			gameObject.renderer.material.color = player2Color;
		}
		else if (tag == "Enemy")
		{
			gameObject.renderer.material.color = enemyColor;
		}
	}
	
}
