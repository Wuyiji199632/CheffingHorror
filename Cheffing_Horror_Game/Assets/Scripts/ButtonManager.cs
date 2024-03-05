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

    public GameObject controlPage,audioControlPage,graphicsControlPage;

    bool controlPageOpened=false,audioControlPageOpened=false,graphicsControlPageOpened=false;

    private void Start()
    {
        firstSelectionPage.SetActive(true);
        settingsPage.SetActive(false);
        CreditsPage.SetActive(false);
        controlPage.SetActive(false);audioControlPage.SetActive(false); graphicsControlPage.SetActive(false);
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
        controlPage.SetActive(controlPageOpened);
    }
    public void EnableAudioPage()
    {
        audioControlPageOpened=!audioControlPageOpened;
        audioControlPage.SetActive(audioControlPageOpened);
    }
    public void EnableGraphicsPage()
    {
        graphicsControlPageOpened=!graphicsControlPageOpened;
        graphicsControlPage.SetActive(graphicsControlPageOpened);

    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
