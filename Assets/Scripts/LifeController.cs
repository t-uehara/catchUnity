using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LifeController : MonoBehaviour {

	public int life;

	[SerializeField]
	private Image[] marks;
	[SerializeField]
	private Sprite ball;
	[SerializeField]
	private Sprite ball_ng;

	void Start () {
		reset ();
	}
	
	void Update () {}

	public void reset() {
		life = 3;
		changeLifePicToOk ();
	}

	public int getLife() {
		return life;
	}

	public void reduceLife() {
		life -= 1;
		if(life >= 0) {	
			changeLifePicToNg (2 - life);
		}
	}

	public void changeLifePicToNg(int index) {
		marks [index].sprite = ball_ng;
	}

	public void changeLifePicToOk() {
		for (int i = 0; i < 3; i++) {
			marks [i].sprite = ball;
		}
	}
}
