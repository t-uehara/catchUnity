using UnityEngine;
using System.Collections;

public abstract class PersonController : MonoBehaviour {
	public bool hasBall = true;

	public abstract void Throw ();
	public abstract void Catch (GameObject obj);

}
