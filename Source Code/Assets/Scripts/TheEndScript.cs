using UnityEngine;
using UnityEngine.SceneManagement;

public class TheEndScript : MonoBehaviour {
	
	public void TheEnd() {
        //Change to Main Menu scene
        SceneManager.LoadScene(0);
    }
}
