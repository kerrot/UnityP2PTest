using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MonobitEngine;

public class SceneTest : MonobitEngine.MonoBehaviour {	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Jump")) {
			SceneManager.LoadScene (SceneManager.GetActiveScene().name);
			monobitView.RPC("Reconnect", MonobitTargets.Others);
		}
	}
}
