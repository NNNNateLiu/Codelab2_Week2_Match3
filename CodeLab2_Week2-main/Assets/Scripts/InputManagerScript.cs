using UnityEngine;
using System.Collections;

//interaction with the game
public class InputManagerScript : MonoBehaviour {
	
	//reference to other component
	protected GameManagerScript gameManager;
	protected MoveTokensScript moveManager;
	
	//store the Token selected by mouse
	protected GameObject selected = null;

	public virtual void Start () {
		
		//get
		moveManager = GetComponent<MoveTokensScript>();
		gameManager = GetComponent<GameManagerScript>();
	}
		
	
	public virtual void SelectToken(){
		//click down left mouse button to get a token
		if(Input.GetMouseButtonDown(0)){
			//get the position of center point of the screen
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			
			//check if the position overlap with any token
			Collider2D collider = Physics2D.OverlapPoint(mousePos);

			//if mouse position overlap with a token
			if(collider != null)
			{
				//if did not select any token before
				if(selected == null)
				{
					//select this one
					selected = collider.gameObject;
				} 
				else 
				{
					//if selected a token before, then get the one selected before and the clicked one's positions
					Vector2 pos1 = gameManager.GetPositionOfTokenInGrid(selected);
					Vector2 pos2 = gameManager.GetPositionOfTokenInGrid(collider.gameObject);

					//if the are next to each other
					if(Mathf.Abs((pos1.x - pos2.x) + (pos1.y - pos2.y)) == 1)
					{
						//call SetupTokenExchange
						moveManager.SetupTokenExchange(selected, pos1, collider.gameObject, pos2, true);
					}
					//empty token selected
					selected = null;
				}
			}
		}

	}

}
