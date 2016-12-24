using UnityEngine;
using System.Collections;

public class PitcherController : PersonController {
	public Transform pitchPoint;
	public GameObject catchPoint;
	public GameObject ball;
	public Canvas canvas;

	// 球種(外部設定用)
	public bool fastball = false;
	public bool curve = false;
	public bool slider = false;
	public bool shoot = false;
	public bool superFastball = false;

	void Start () {
	}

	// 投げる
	public override void Throw () {
		// ボール所持フラグがfalseなら、何もしない
		if(!hasBall) return;
		// ボール所持フラグがtrueなら、投球処理に移る
		StartCoroutine (throwAction ());
	}

	// 投げる処理（アニメーション＋ボールの生成）
	IEnumerator throwAction() {
		BallController.BallTypes type; // 球種
		float speed; // 球速
		float elevation; // 仰角
		float xSpeed; // x方向の速度（左右のコントロール）

		// ボール所持フラグをfalseにする
		hasBall = false;

		// ちょっと待つ
		yield return new WaitForSeconds(0.5f);

		// 投げモーション開始
		GetComponent<Animator>().SetTrigger("Throw");

		// 投げモーションが完了するまで待つ
		yield return new WaitForSeconds (1.2f);

		// スピード調整
		Time.timeScale = 0.8f;

		// ピッチングポイントに、ボールのインスタンスを生成
		GameObject ballObj = (GameObject)Instantiate (ball, pitchPoint.position, pitchPoint.rotation);

		// 球種と初速を決定する
		int rand;
		if (fastball || curve || slider || shoot || superFastball) {
			rand = -1;
		} else {
			rand = Random.Range (0, 5);
		}
		if (curve || rand == 1) {	// カーブ
			type = BallController.BallTypes.Curve;
			speed = Random.Range(16f, 17f);
			elevation = Random.Range(12.5f, 13.2f);
			xSpeed = Random.Range(-0.45f, -0.15f);
		} else if (slider || rand == 2) {	// スライダー
			type = BallController.BallTypes.Slider;
			speed = Random.Range(18f, 19f);
			elevation = Random.Range(4f, 6f);
			xSpeed = Random.Range(-0.3f, 0.3f);
		} else if (shoot || rand == 3) {	// シュート
			type = BallController.BallTypes.Shoot;
			speed = Random.Range(18f, 19f);
			elevation = Random.Range(4f, 6f);
			xSpeed = Random.Range(1.2f, 1.8f);
		} else if (superFastball || rand == 4) {	// 豪速球
			type = BallController.BallTypes.SuperFastball;
			speed = Random.Range(28f, 30f);
			elevation = Random.Range(-0.8f, 0.4f);
			xSpeed = Random.Range(0.7f, 1.8f);
		} else {	// ストレート
			type = BallController.BallTypes.Fastball;
			speed = Random.Range(21f, 23f);
			elevation = Random.Range(1.5f, 3.5f);
			xSpeed = Random.Range(0.7f, 1.1f);
		}

		// ボールに球種をセットする
		ballObj.GetComponent<BallController> ().setBallType (type);

		// キャンバスに球種を表示する
		canvas.GetComponent<CanvasController>().setBallInfo(type);

		// ボールに初速を与える
		Vector3 ballV = new Vector3(
			xSpeed,
			speed * Mathf.Sin(elevation * Mathf.Deg2Rad),
			speed * Mathf.Cos(elevation * Mathf.Deg2Rad) * -1
		);
		ballObj.GetComponent<Rigidbody> ().velocity = ballV;

		// 投球音を再生する
		pitchPoint.GetComponent<AudioSource>().Play();

		yield break;
	}

	// 捕球
	public override void Catch(GameObject obj) {
		// 捕球音を再生する
		catchPoint.GetComponent<AudioSource> ().Play ();

		// ボールの保持状態をtrueにする
		hasBall = true;

		// ボールのオブジェクトを削除する
		Destroy(obj);

		// 捕球処理
		GameObject.Find ("Manager").GetComponent<Manager>().EndPitch();
	}

	public IEnumerator CatchAction() {
		// 時間調整
		yield return new WaitForSeconds(0.01f);
		// 投げモーション開始
		GetComponent<Animator>().SetTrigger("Catch");
	}
}
