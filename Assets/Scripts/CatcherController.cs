using UnityEngine;
using System.Collections;

public class CatcherController : PersonController {
	public GameObject ball;
	public Transform pitchPoint;
	public GameObject catchPoint;

	public float speed = 16f;
	public float elevation = 60f;
	public float xSpeed = 0.3f;

	private GameObject catchedBall;

	// ボールを投げ返す 
	public override void Throw() {
		// ボールを持っていない場合、何もしない
		if(!hasBall) return;
		// 保持していたボールを削除
		DestroyBall();
		// ピッチングポイントに、ボールのインスタンスを生成
		GameObject ballObj = (GameObject)Instantiate (ball, pitchPoint.position, pitchPoint.rotation);
		// ボールに初速を与える
		Vector3 ballV = new Vector3 (
			                xSpeed,
			                speed * Mathf.Sin (elevation * Mathf.Deg2Rad),
			                speed * Mathf.Cos (elevation * Mathf.Deg2Rad)
		                );
		ballObj.GetComponent<Rigidbody> ().velocity = ballV;
		// 投球音
		pitchPoint.GetComponent<AudioSource> ().Play ();
		// ボールを手放す
		hasBall = false;
	}

	// ボールを捕球する
	public override void Catch(GameObject obj) {
		obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
		obj.GetComponent<Rigidbody>().useGravity = false;
		obj.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
		obj.GetComponent<BallController>().Catched ();
		catchPoint.GetComponent<AudioSource> ().Play ();
		// ボールを保持する
		hasBall = true;
		catchedBall = obj;
		// スピード調整
		Time.timeScale = 1f;
		// 距離チェック
		GameObject.Find ("Manager").GetComponent<Manager>().CatchedByCatcher(
			new Vector2(obj.GetComponent<Transform> ().position.x, obj.GetComponent<Transform> ().position.y),
				catchedBall.GetComponent<BallController>().getBallType()
		);
	}

	// 持っているボールを破棄する
	public void DestroyBall() {
		Destroy(catchedBall);
	}
}
