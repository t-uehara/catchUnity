using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {
	public GameObject pitcher;
	private PitcherController pitcherCtrl;
	public GameObject catcher;
	private CatcherController catcherCtrl;
	public GameObject cursor;
	public GameObject canvas;
	private CanvasController canvasCtrl;
	public GameObject life;
	private LifeController lifeCtrl;

	private bool isGameover;
	private bool isPitching;

	public enum result {
		PERFECT,
		GOOD,
		BAD
	}

	public int score;

	void Start () {
		pitcherCtrl = pitcher.GetComponent<PitcherController> ();
		catcherCtrl = catcher.GetComponent<CatcherController> ();
		lifeCtrl = life.GetComponent<LifeController>();
		canvasCtrl = canvas.GetComponent<CanvasController>();
		initializeGame ();
	}

	// ゲーム初期化処理
	public void initializeGame() {
		// フラグを初期化
		isGameover = false;
		isPitching = false;
		// ボールの状態を初期化
		pitcherCtrl.hasBall = true;
		catcherCtrl.hasBall = false;
		catcherCtrl.DestroyBall ();
		// スコアを0に
		score = 0;
		canvasCtrl.setScoreInfo(score);
		// オブジェクトを非表示に
		canvas.GetComponent<CanvasController>().hideGameover(); // ゲームオーバー表示
		cursor.SetActive (false); // カーソル
		// ライフを0に
		lifeCtrl.reset ();
		lifeCtrl.changeLifePicToOk ();
	}

	
	void Update () {
		// イベントの受付
		// debug Zキー：ピッチャー投げる
		if (Input.GetKeyDown (KeyCode.Z)) {
			pitcherCtrl.Throw ();
		}
		// debug Xキー：キャッチャー投げる
		if (Input.GetKeyDown (KeyCode.X)) {
			catcherCtrl.Throw ();
		}
		if (Input.touchCount > 0) {
			// タッチ検出
			Touch touch = Input.GetTouch (0);
			if (isPitching) {
				// 投球中
				if (touch.phase == TouchPhase.Moved) { // カーソル移動
					cursor.GetComponent<Transform> ().position = CalcCursorPos(Input.mousePosition);
				}
			} else {
				// 待機中
				if (touch.phase == TouchPhase.Began) { // 処理開始
					// gameover中なら初期化
					if (isGameover) {
						initializeGame ();
						return;
					}
					StartPitch ();
				}
			}
		} else {
			// マウスクリック検出
			if (Input.GetMouseButtonDown (0)) {
				if (isPitching) {
					// 投球中
					cursor.GetComponent<Transform> ().position = CalcCursorPos(Input.mousePosition);
				} else {
					// 待機中
					// gameover中なら初期化
					if (isGameover) {
						initializeGame ();
						return;
					}
					StartPitch ();
				}
			}
			// マウスボタン押下検出
			else if (Input.GetMouseButton (0)) {
				if (isPitching) {
					// 投球中
					cursor.GetComponent<Transform> ().position = CalcCursorPos(Input.mousePosition);
				}
			}
		}
	}
		
	// 投球開始処理（ピッチャー、キャッチャー両方）
	public void StartPitch() {
		isPitching = true;
		if (pitcherCtrl.hasBall) {
			// ピッチャーがボールを持っている場合
			// カーソルを表示
			cursor.GetComponent<Transform> ().position = CalcCursorPos(Input.mousePosition);
			cursor.SetActive (true);
			// ピッチャーの投球処理を実行
			pitcherCtrl.Throw ();
		}
		if (catcherCtrl.hasBall) {
			// キャッチャーがボールを持っている場合
			// カーソル、結果を隠す
			cursor.SetActive (false);
			canvasCtrl.hideResult ();
			// ピッチャーの捕球待ちを開始
			StartCoroutine(pitcherCtrl.CatchAction ());
			// キャッチャーの投球処理を実行
			catcherCtrl.Throw ();
		}
	}

	// 投球完了処理 (ピッチャー捕球時)
	public void EndPitch(){
		// 投球中フラグをfalseにする
		isPitching = false;
	}

	// 捕球判定　(キャッチャー捕球時)
	public void CatchedByCatcher(Vector2 ballPos, BallController.BallTypes ballType) {
		// 投球中フラグをfalseにする
		isPitching = false;

		// カーソルとボールの距離を計算
		Vector2 cursorPos = new Vector2 (cursor.GetComponent<Transform> ().position.x, cursor.GetComponent<Transform> ().position.y);
		float distance = Vector2.Distance (ballPos, cursorPos);

		// 距離を表示（デバッグ用）
		canvasCtrl.setDistance (distance);

		// 球種から基礎点を加算
		int baseScore = 0;
		switch (ballType) {
		case BallController.BallTypes.Fastball:
			baseScore = 100;
			break;
		case BallController.BallTypes.Curve:
		case BallController.BallTypes.Slider:
		case BallController.BallTypes.Shoot:
			baseScore = 150;
			break;
		case BallController.BallTypes.SuperFastball:
			baseScore = 200;
			break;
		}

		// 距離から結果を判定し、演出を描画
		if (distance < 0.05) {
			// PERFECT!
			score += baseScore * 150 / 100;
			canvasCtrl.showResult (result.PERFECT);
		} else if (distance < 0.12) {
			// GOOD!
			score += baseScore;
			canvasCtrl.showResult (result.GOOD);
		} else {
			// MISS...
			life.GetComponent<LifeController>().reduceLife();
			if (lifeCtrl.getLife () == 0) {
				// ライフが0ならGAMEOVER
				isGameover = true;
				canvas.GetComponent<CanvasController>().showGameover();
			} else {
				canvasCtrl.showResult (result.BAD);
			}
		}

		// スコア表示
		canvasCtrl.setScoreInfo(score);

	}

	private Vector3 CalcCursorPos(Vector3 position) {
		// スクリーン座標をワールド座標に変換した位置座標
		Vector3 screenToWorldPointPosition;
		// Z軸修正(カメラと対象の位置)
		position.z = 0.45f;
		// マウス位置座標をスクリーン座標からワールド座標に変換する
		screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(position);
		// ワールド座標に変換されたマウス座標を代入
		gameObject.transform.position = screenToWorldPointPosition;
		// Z軸修正
		screenToWorldPointPosition.z = 0;
		return screenToWorldPointPosition;
	}
}
