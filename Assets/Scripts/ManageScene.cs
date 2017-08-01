using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManageScene : MonoBehaviour {

    public InputField seedField;

	public void StartGame()
    {
        if (!seedField.text.Equals(""))
        {
            PlayerPrefs.SetInt("Seed", int.Parse(seedField.text));
            SceneManager.LoadScene("mainScene");
            Time.timeScale = 1;
        }
    }

    public void ReStartGame()
    {
        SceneManager.LoadScene("Start");
    }
}
