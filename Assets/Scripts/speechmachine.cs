using UnityEngine;

public class speechmachine : MonoBehaviour {
	
	int frames = 0;
	public int frameRate = 1;
	
	// Use this for initialization
	void Start () {
		GetComponent<TextMesh>().text = "";
	}
	
	void FixedUpdate () {
		frames += frameRate;
		
		// Reset animation loop.
		if(frames >= 150)
			frames = 0;
		
		Animate();
	}
	
	void Animate()
	{
		if(frames < 30)
		{
			GetComponent<TextMesh>().text = "I";
			GetComponent<TextMesh>().alignment = TextAlignment.Center;
		}
		else if (frames < 60)
		{
			GetComponent<TextMesh>().text = "I am";
			GetComponent<TextMesh>().alignment = TextAlignment.Left;
		}
		else if (frames < 90)
		{
			GetComponent<TextMesh>().text = "I am watching";
			GetComponent<TextMesh>().alignment = TextAlignment.Center;
		}
		else if (frames < 120)
		{
			GetComponent<TextMesh>().text = "I am watching you!";
			GetComponent<TextMesh>().alignment = TextAlignment.Right;
		}
		else if (frames < 150)
		{
			GetComponent<TextMesh>().text = "";
			GetComponent<TextMesh>().alignment = TextAlignment.Right;
		}
	}
}
