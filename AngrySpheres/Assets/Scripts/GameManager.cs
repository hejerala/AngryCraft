using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject FrogsLeftText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //Counts the ammount of frogs in the game and updates the text to reflect it
        int numFrogs = FindObjectsOfType<Frog>().Length;
        FrogsLeftText.GetComponent<Text>().text = "Frogs Left: " + numFrogs;
        //When it reaches 0 the game restarts
        if (numFrogs == 0)
        {
            SceneManager.LoadScene("GameScene");
        }
        //The game goes to the main manu if escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MenuScene");
        }
    }
}
