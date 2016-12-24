using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;	// debug


public class CanvasController : MonoBehaviour {
	public Text ballInfo;
	public Text debugInfo;
	public Text distanceInfo;
	public Text scoreInfo;

	[SerializeField]
	private Image result;
	[SerializeField]
	private Image gameover;

	[SerializeField]
	private Sprite perfect;
	[SerializeField]
	private Sprite good;
	[SerializeField]
	private Sprite bad;


	public void Start() {
		this.oneSecondTime = 0f;
	}

	private void Update ()
	{
		// FPS
		if (this.oneSecondTime >= 1f) {
			this.fps = this.fpsCounter;
			this.fixedFps = this.fixedFpsCounter;

			// reset
			this.fpsCounter = 0;
			this.fixedFpsCounter = 0;
			this.oneSecondTime = 0f; 
		} else {
			this.fpsCounter ++;
			this.oneSecondTime += Time.deltaTime;
		}

		// structure debug string
		this.debugString = "";
		int count = CanvasController.displayLog.Count;

		for (int i=0; i<CanvasController.displayLog.Count; i++) {
			this.debugString += CanvasController.displayLog[i];
			this.debugString += "\n";
		}
		CanvasController.displayLog.Clear ();
	}

	private void FixedUpdate ()
	{
		this.fixedFpsCounter++;
	}

	private void OnGUI ()
	{
		debugInfo.text = "FPS: " + this.fps + "  FixedUpdate: " + this.fixedFps + "\n" + this.debugString;
	}

	// ボール情報の表示
	public void setBallInfo(BallController.BallTypes type) {
		switch (type) {
		case BallController.BallTypes.Curve:
			ballInfo.text = "カーブ";
			break;
		case BallController.BallTypes.Fastball:
			ballInfo.text = "ストレート";
			break;
		case BallController.BallTypes.Slider:
			ballInfo.text = "スライダー";
			break;
		case BallController.BallTypes.Shoot:
			ballInfo.text = "シュート";
			break;
		case BallController.BallTypes.SuperFastball:
			ballInfo.text = "豪速球";
			break;
		default:
			break;
		}
	}

	// 距離情報の表示
	public void setDistance(float distance) {
		distanceInfo.text = distance.ToString();
		if (distance < 0.05) {
			distanceInfo.text += "\r\nperfect";
		} else if (distance < 0.12) {
			distanceInfo.text += "\r\ngood";
		} else {
			distanceInfo.text += "\r\nmiss";
		}
	}

	// 捕球結果の表示
	public void showResult(Manager.result type) {
		switch (type) {
		case Manager.result.PERFECT:
			result.sprite = this.perfect;
			result.gameObject.SetActive (true);
			break;
		case Manager.result.GOOD:
			result.sprite = this.good;
			result.gameObject.SetActive (true);
			break;
		case Manager.result.BAD:
			result.sprite = this.bad;
			result.gameObject.SetActive (true);
			break;
		default:
			break;
		}
	}

	// スコア情報の表示
	public void setScoreInfo(int score) {
		scoreInfo.text = "score:" + score;
	}

	// 捕球結果を隠す
	public void hideResult() {
		result.gameObject.SetActive (false);
	}

	// ゲームオーバー表示
	public void showGameover() {
		gameover.gameObject.SetActive (true);
	}

	// ゲームオーバー表示を隠す
	public void hideGameover () {
		gameover.gameObject.SetActive (false);
	}

	/* debug */

	static public List<string> displayLog = new List<string>();

	// FPS
	private int fps;
	private int fpsCounter; 

	// Fixed FPS
	private int fixedFps; 
	private int fixedFpsCounter;

	// Debug Log
	private string debugString;

	// Timer
	private float oneSecondTime;
}
