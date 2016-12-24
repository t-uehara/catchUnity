using UnityEngine;
using System.Collections;

public class CatchPointController : MonoBehaviour {
	private GameObject parent;

	void Start() {
		// 親要素を取得
		parent = gameObject.transform.parent.gameObject;
	}

	// ボールと衝突したら捕球する
	void OnCollisionEnter(Collision hit) {
		if(hit.gameObject.CompareTag("Ball")) {
			parent.GetComponent<PersonController>().Catch (hit.gameObject);
		}
	}		
}
