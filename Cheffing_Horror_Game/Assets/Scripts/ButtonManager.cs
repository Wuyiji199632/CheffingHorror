using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ButtonManager : MonoBehaviour
{
    [Header("Main Menu Pages")]
    public GameObject settingsPage;

    public GameObject firstSelectionPage;

    public GameObject CreditsPage;

    public GameObject ControlPage;

    bool controlPageOpened=false;

    private void Start()
    {
        firstSelectionPage.SetActive(true);
        settingsPage.SetActive(false);
        CreditsPage.SetActive(false);
        ControlPage.SetActive(false);
        SoundManager.Instance.Play("BGM");
    }


    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene"); //Test scene for the game 
    }

    public void LoadGameSaveData()
    {
        Debug.Log("Loading saved data...will implement later on!");
    }

    public void LoadSettingsMenu()
    {
        settingsPage.SetActive(true);
        firstSelectionPage.SetActive(false);
        CreditsPage.SetActive(false);
    }

    public void LoadCreditsMenu()
    {
        CreditsPage.SetActive(true);
        firstSelectionPage.SetActive(false);
        settingsPage.SetActive(false);
    }
    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void EnableControlPage()
    {
        controlPageOpened=!controlPageOpened;
        ControlPage.SetActive(controlPageOpened);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
