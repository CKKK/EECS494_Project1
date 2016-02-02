using UnityEngine;

public class speechmachine : MonoBehaviour {
	
	int frames = 0;
	public int frameRate = 1;
	public int sentence_ind = 0;
	public static speechmachine instance;
	// Use this for initialization
	void Start () {
		if (instance == null) {
			instance = this;
		}
		GetComponent<TextMesh>().text = "";
	}
	
	void FixedUpdate () {
		frames += frameRate;
		
		// Reset animation loop.
		if(frames >= 270)
			frames = 0;
		
		Animate();
	}
	
	void Animate()
	{
		switch (sentence_ind) {
		case 0:
			if(frames < 30)
			{
				GetComponent<TextMesh>().text = "To";
				//GetComponent<TextMesh>().alignment = TextAlignment.Center;
			}
			else if (frames < 60)
			{
				GetComponent<TextMesh>().text = "To the ";
				//GetComponent<TextMesh>().alignment = TextAlignment.Left;
			}
			else if (frames < 90)
			{
				GetComponent<TextMesh>().text = "To the left, ";
				//GetComponent<TextMesh>().alignment = TextAlignment.Center;
			}
			else if (frames < 120)
			{
				GetComponent<TextMesh>().text = "To the left, May";
				//GetComponent<TextMesh>().alignment = TextAlignment.Right;
			}
			else if (frames < 150)
			{
				GetComponent<TextMesh>().text = "To the left, May the";
				//GetComponent<TextMesh>().alignment = TextAlignment.Right;
			}
			else if (frames < 180)
			{
				GetComponent<TextMesh>().text = "To the left, May the Force";
				//GetComponent<TextMesh>().alignment = TextAlignment.Right;
			}
			else if (frames < 210)
			{
				GetComponent<TextMesh>().text = "To the left, May the Force\n be ";
				//GetComponent<TextMesh>().alignment = TextAlignment.Right;
			}
			else if (frames < 240)
			{
				GetComponent<TextMesh>().text = "To the left, May the Force \nbe with";
				//GetComponent<TextMesh>().alignment = TextAlignment.Right;
			}
			else if (frames < 270)
			{
				GetComponent<TextMesh>().text = "To the left, May the Force\n be with you";
				//GetComponent<TextMesh>().alignment = TextAlignment.Right;
			}
			break;
		case 1:
			if(frames < 30)
			{
				GetComponent<TextMesh>().text = "Use";
				//GetComponent<TextMesh>().alignment = TextAlignment.Center;
			}
			else if (frames < 60)
			{
				GetComponent<TextMesh>().text = "Use your";
				//GetComponent<TextMesh>().alignment = TextAlignment.Left;
			}
			else if (frames < 90)
			{
				GetComponent<TextMesh>().text = "Use your light";
				//GetComponent<TextMesh>().alignment = TextAlignment.Center;
			}
			else if (frames < 120)
			{
				GetComponent<TextMesh>().text = "Use your lightsaber";
				//GetComponent<TextMesh>().alignment = TextAlignment.Right;
			}
			else if (frames < 150)
			{
				GetComponent<TextMesh>().text = "Use your lightsaber \nto";
				//GetComponent<TextMesh>().alignment = TextAlignment.Right;
			}
			else if (frames < 180)
			{
				GetComponent<TextMesh>().text = "Use your lightsaber \nto destroy the";
				//GetComponent<TextMesh>().alignment = TextAlignment.Right;
			}
			else if (frames < 210)
			{
				GetComponent<TextMesh>().text = "Use your lightsaber \nto destroy the dark";
				//GetComponent<TextMesh>().alignment = TextAlignment.Right;
			}
			else if (frames < 240)
			{
				GetComponent<TextMesh>().text = "Use your lightsaber \nto destroy the dark side";
				//GetComponent<TextMesh>().alignment = TextAlignment.Right;
			}
			else if (frames < 270)
			{
				GetComponent<TextMesh>().text = "Use your lightsaber \nto destroy the dark side.";
				//GetComponent<TextMesh>().alignment = TextAlignment.Right;
			}
			break;

		}

	}
}
