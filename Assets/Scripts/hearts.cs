using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class hearts : MonoBehaviour {


	public GameObject heartPrefab;
	public static List<GameObject> heartImages = new List<GameObject>();
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		int diff = PlayerControl.instance.health_Count - heartImages.Count;
		int absVal = Mathf.Abs(diff);
		
		// Heart display // Credit to Austin Yager from 494 Quest
		for(int i = 0; i < absVal; i++)
		{
			// Add hearts.
			if(diff > 0)
			{
				GameObject newHeart = Instantiate(this.heartPrefab, Vector3.zero, Quaternion.identity) as GameObject;
				newHeart.transform.SetParent(this.gameObject.transform);
				newHeart.transform.localScale = new Vector3(1,1,0);
				newHeart.layer = 5;
				newHeart.GetComponent<RectTransform>().anchoredPosition = new Vector3(heartImages.Count * 12, 0, 0) + new Vector3(25, -32, 0);
				newHeart.GetComponent<RectTransform>().sizeDelta = new Vector2(10,3);
				heartImages.Add(newHeart);
			}
			
			// Remove hearts.
			else if (diff < 0)
			{
				GameObject heartToRemove = heartImages[heartImages.Count-1];
				heartImages.Remove(heartToRemove);
				Destroy(heartToRemove);
			}
		}		
	
	}


}
