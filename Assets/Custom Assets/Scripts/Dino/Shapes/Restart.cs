using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour {

    public KeyCode restartKeycode = KeyCode.Backspace;
    public KeyCode menuKeycode = KeyCode.Escape;
    public AkBank bank;

    public Rewired.Player pInput;

    public static Restart instance { get; private set; }

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        pInput = Rewired.ReInput.players.GetPlayer(0);
    }
	
	void Update ()
    {
        if (!GameManager.instance.isRunning)
        {
            return;
        }

        if (pInput.GetButtonDown(Globals.BtnAction4))
        {
            RestartCheckPoint();
        }

		if (Input.GetKeyDown(restartKeycode) || pInput.GetButtonDown(Globals.BtnBack))
        {
            RestartScene();
        }

        if (Input.GetKeyDown(menuKeycode))
        {
            RestartMenu();
        }
	}

    void RestartCheckPoint()
    {
        GameManager.instance.Restart(false);
    }

    public void RestartScene()
    {
        GameManager.instance.Restart(true);
    }

    public void RestartMenu()
    {
        AkSoundEngine.PostEvent("UI_Button_Quit_Game_Play", gameObject);
        //UnloadBanks();
        SceneManager.LoadScene(0);
        MapManager.instance.ResetVar();
        GameManager.instance.ResetVar(true);
    }
    /*
    public void UnloadBanks()
    {
        bank.UnloadBank(bank.gameObject);
    }
    */
}
