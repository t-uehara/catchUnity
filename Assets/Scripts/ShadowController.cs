using UnityEngine;
using System.Collections;

public class ShadowController : MonoBehaviour {
	Vector3 pos;

	void Start () {
		pos = transform.position;
		pos.y = 0;
		transform.position = pos;
	}

	void Update () {
		pos = transform.position;
		pos.y = 0;
		transform.position = pos;
	}
}
