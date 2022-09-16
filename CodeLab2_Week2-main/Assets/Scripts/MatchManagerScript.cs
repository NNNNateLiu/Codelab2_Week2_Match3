using UnityEngine;
using System.Collections;

public class MatchManagerScript : MonoBehaviour 
{
	protected GameManagerScript gameManager;

	public virtual void Start () 
	{
		gameManager = GetComponent<GameManagerScript>();
	}

	//called by GameManager every frame looping through every token if one has a horizontal match
	public virtual bool GridHasMatch()
	{
		bool match = false;
		
		for(int x = 0; x < gameManager.gridWidth; x++)
		{
			for(int y = 0; y < gameManager.gridHeight ; y++)
			{
				match = XCheck(x, y) || YCheck(x, y);
				// double "=" means, judge; single "=" means set value
				if (match == true)
				{
					break;
				}
			}
			if (match == true)
			{
				break;
			}
		}
		Debug.Log("match" + match);
		return match;
	}

	public bool XCheck(int x, int y)
	{
		bool flag1 = false;
		if(x < gameManager.gridWidth - 1 && x > 0)
		{
			flag1 = GridHasHorizontalMatch(x, y);
		}

		Debug.Log("Flag1" + flag1);
		return flag1;
	}

	public bool YCheck(int x, int y)
	{
		bool flag2 = false;
		if (y < gameManager.gridHeight - 1 && y > 0)
		{
			flag2 = GridHasVerticalMatch(x, y);
		}

		Debug.Log("Flag2" + flag2);
		return flag2;
	}

	//check if a grid has horizontal match
	public bool GridHasHorizontalMatch(int x, int y)
	{
		//get token on the grid, one to its right and two to its right
		GameObject token1 = gameManager.gridArray[x - 1, y];
		GameObject token2 = gameManager.gridArray[x, y];
		GameObject token3 = gameManager.gridArray[x + 1, y];
		
		//check if there is token on each of these grid
		if(token1 != null && token2 != null && token3 != null)
		{
			//check if the three token share the same sprite
			SpriteRenderer sr1 = token1.GetComponent<SpriteRenderer>();
			SpriteRenderer sr2 = token2.GetComponent<SpriteRenderer>();
			SpriteRenderer sr3 = token3.GetComponent<SpriteRenderer>();
			
			//if share the same sprite, then return true
			return (sr1.sprite == sr2.sprite && sr2.sprite == sr3.sprite);
		} 
		else 
		{
			//else return false
			return false;
		}
	}
	
	public bool GridHasVerticalMatch(int x, int y)
	{
		//get token on the grid, one to its right and two to its right
		GameObject token1 = gameManager.gridArray[x, y-1];
		GameObject token2 = gameManager.gridArray[x, y];
		GameObject token3 = gameManager.gridArray[x, y+1];
		
		//check if there is token on each of these grid
		if(token1 != null && token2 != null && token3 != null)
		{
			//check if the three token share the same sprite
			SpriteRenderer sr1 = token1.GetComponent<SpriteRenderer>();
			SpriteRenderer sr2 = token2.GetComponent<SpriteRenderer>();
			SpriteRenderer sr3 = token3.GetComponent<SpriteRenderer>();
			
			//if share the same sprite, then return true
			return (sr1.sprite == sr2.sprite && sr2.sprite == sr3.sprite);
		} 
		else 
		{
			//else return false
			return false;
		}
	}

	//check how many tokens in horizontal share same color
	public int GetHorizontalMatchLength(int x, int y)
	{
		//start from a token itself
		int matchLength = 1;
		
		GameObject first = gameManager.gridArray[x, y];
		
		//if the grid has a token
		if(first != null)
		{
			SpriteRenderer sr1 = first.GetComponent<SpriteRenderer>();
			
			//loop all tokens on the right
			for(int i = x + 1; i < gameManager.gridWidth; i++)
			{
				GameObject other = gameManager.gridArray[i, y];

				if(other != null)
				{
					SpriteRenderer sr2 = other.GetComponent<SpriteRenderer>();

					if(sr1.sprite == sr2.sprite)
					{
						//if the one on the right has the same color
						//add the 1 to the length
						matchLength++;
					} 
					else 
					{
						//break the loop if not in the same color
						break;
					}
				} 
				else 
				{
					//break the loop if the right one is empty
					break;
				}
			}
		}
		
		//get the length of the same color length horizontally
		return matchLength;
	}
	
	public int GetVerticalMatchLength(int x, int y)
	{
		//start from a token itself
		int matchLength = 1;
		
		GameObject first = gameManager.gridArray[x, y];
		
		//if the grid has a token
		if(first != null)
		{
			SpriteRenderer sr1 = first.GetComponent<SpriteRenderer>();
			
			//loop all tokens on the right
			for(int i = y + 1; i < gameManager.gridHeight; i++)
			{
				GameObject other = gameManager.gridArray[x, i];

				if(other != null)
				{
					SpriteRenderer sr2 = other.GetComponent<SpriteRenderer>();

					if(sr1.sprite == sr2.sprite)
					{
						//if the one on the right has the same color
						//add the 1 to the length
						matchLength++;
					} 
					else 
					{
						//break the loop if not in the same color
						break;
					}
				} 
				else 
				{
					//break the loop if the right one is empty
					break;
				}
			}
		}
		
		//get the length of the same color length horizontally
		return matchLength;
	}
		
	//remove matched tokens and get the number removed
	public virtual int RemoveMatches()
	{
		int numRemoved = 0;

		//loop through every token
		for(int x = 0; x < gameManager.gridWidth; x++)
		{
			for(int y = 0; y < gameManager.gridHeight ; y++)
			{
				//Question: Should we add a null check here?
				//if (gameManager.gridArray[x, y] == null) continue;
				
				//check how many tokens on the right have the same color with the current looping one
				int horizonMatchLength = GetHorizontalMatchLength(x, y);
				int verticalMatchLength = GetVerticalMatchLength(x, y);

				//if Length is more than 2, then destroy every token horizontally  
				if(horizonMatchLength > 2)
				{
					for(int i = x; i < x + horizonMatchLength; i++)
					{
						GameObject token = gameManager.gridArray[i, y]; 
						Destroy(token);

						gameManager.gridArray[i, y] = null;
						numRemoved++;
					}
				}
				if (verticalMatchLength > 2)
				{
					for(int i = y; i < y + verticalMatchLength; i++)
					{
							
						GameObject token = gameManager.gridArray[x, i];
						if (token != null)
						{
							Destroy(token);
						}
						gameManager.gridArray[x, i] = null;
						numRemoved++;
					}
				}
			}
		}
		
		return numRemoved;
	}
}
