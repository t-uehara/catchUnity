using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {
	// オブジェクトを削除するまでの時間
	public int lifeTime = 3;
	private bool isCatched = false;

	private BallTypes ballType;

	public enum BallTypes {
		Fastball,
		Curve,
		Slider,
		Shoot,
		SuperFastball
	}

	IEnumerator Start () {
		while(true){
			// 球種によって、継続的にボールに変化を与える
			switch (ballType) {
				case BallTypes.Curve:
					if (transform.position.z < 8) {
						Vector3 v = GetComponent<Rigidbody> ().velocity;
						v.x += 0.3f;
						v.y -= 0.3f;
						GetComponent<Rigidbody> ().velocity = v;
					}
					break;
				case BallTypes.Slider:
					if (transform.position.z < 7) {
						Vector3 v = GetComponent<Rigidbody> ().velocity;
						v.x += 0.5f;
						v.y += 0.3f;
						GetComponent<Rigidbody> ().velocity = v;
					}
					break;
				case BallTypes.Shoot:
					if (transform.position.z < 7) {
						Vector3 v = GetComponent<Rigidbody> ().velocity;
						v.x -= 0.6f;
						v.y += 0.3f;
						GetComponent<Rigidbody> ().velocity = v;
					}
					break;
				default:
					if (transform.position.z < 7) {
						Vector3 v = GetComponent<Rigidbody> ().velocity;
						v.y += 0.3f;
						GetComponent<Rigidbody> ().velocity = v;
					}
					break;
			}

			// 一定時間ごとに処理実行
			yield return new WaitForSeconds (0.1f);

			// 捕球されたら処理終了
			if (isCatched) {
				yield break;
			}
		}
	}

	void Update() {
		if (GetComponent<Transform> ().position.z < 0.198f) {
			Vector3 pos = GetComponent<Transform> ().position;
			pos.z = 0.199f;
			GetComponent<Transform> ().position = pos;
		}
	}

	// 捕球された時の処理
	public void Catched() {
		isCatched = true;
	}

	// 球種を設定
	public void setBallType(BallTypes type) {
		ballType = type;
	}

	// 球種を取得
	public BallTypes getBallType(){
		return ballType;
	}
}
