using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ButtonManager : MonoBehaviour
{
    [Header("Main Menu Pages")]
    public GameObject settingsPage;

    public GameObject startPage,firstSelectionPage;

    public GameObject CreditsPage;

    public GameObject controlPage,audioControlPage,graphicsControlPage;

    bool controlPageOpened=false,audioControlPageOpened=false,graphicsControlPageOpened=false;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        startPage.SetActive(true);
        firstSelectionPage.SetActive(false);
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
    
    public void LoadFirstSelectionPage()
    {
        
        startPage.SetActive(false);
        settingsPage.SetActive(false);
        firstSelectionPage.SetActive(true);
        CreditsPage.SetActive(false);
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
        startPage.SetActive(false);
        firstSelectionPage.SetActive(true);
        settingsPage.SetActive(false);
        CreditsPage.SetActive(false);
        controlPage.SetActive(false); audioControlPage.SetActive(false); graphicsControlPage.SetActive(false);
    }

    public void EnableControlPage()
    {
        audioControlPage.SetActive(false); graphicsControlPage.SetActive(false);
        controlPageOpened =!controlPageOpened;
        controlPage.SetActive(controlPageOpened);
    }
    public void EnableAudioPage()
    {
        controlPage.SetActive(false); graphicsControlPage.SetActive(false);
        audioControlPageOpened =!audioControlPageOpened;
        //audioControlPage.SetActive(audioControlPageOpened);

        SoundManager.Instance.ShowAudioAdjustmentPanel(audioControlPage);
    }
    public void EnableGraphicsPage()
    {
        controlPage.SetActive(false); audioControlPage.SetActive(false);
        graphicsControlPageOpened =!graphicsControlPageOpened;
        graphicsControlPage.SetActive(graphicsControlPageOpened);

    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
