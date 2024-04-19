using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using EventTrigger = UnityEngine.EventSystems.EventTrigger;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour //This is the class that controls the environmentally related gameplay mechanics
{
    [SerializeField] private GameObject pauseMenu;

    public bool paused = false;

    public GameObject currentItemInfo;

    public bool cursorPointingToItem = false;

    public static WorldManager Instance { get; private set; }

    public Dictionary<string, GameObject> displayedItemInfos = new Dictionary<string, GameObject>();

    public List<GameObject> itemInfos = new List<GameObject>();

    public List<GameObject> alienSelections = new List<GameObject>();

    public GameObject alienSelectionPage,alienMainProfilePage,alienProfilePage;

    private CameraMovement player;


    [SerializeField] private Button confirmSelectionBtn;

    public GameObject alienTube, alienSelected; //Alien to come up need further logics to pop in 

    private Animator alienPlatformAnim;

    public bool alienComesUp = false;

    [SerializeField] private GameObject settingsMenuInGame;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keep the instance alive across scenes.
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Ensure there's only one instance by destroying any duplicates.
        }

        foreach (var itemInfo in itemInfos)
        {
            if (!displayedItemInfos.ContainsKey(itemInfo.name))
            {
                displayedItemInfos.Add(itemInfo.name, itemInfo);
            }
        }

        //Fill in the slots for buttons in the game

        
    }

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);

        alienPlatformAnim= alienTube.GetComponent<Animator>();

        player =GameObject.FindGameObjectWithTag("Player").GetComponent<CameraMovement>();            

        SoundManager.Instance.ChangeToInGameBGM(); alienSelectionPage.SetActive(false); alienMainProfilePage.SetActive(true); alienProfilePage.SetActive(false);

        //InvokeRepeating(nameof(SetCursorVisibility), 1, 1);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&&!player.selectionPageOpened)
        {
            ManipulatePauseMenu();
        }
      
    }
    private void LateUpdate()
    {
        SetCursorVisibility();
    }
    private void SetCursorVisibility()
    {
        Cursor.visible = paused || player.notepadOpened;
    }
    private async Task ManipulatePauseMenu()
    {

        await Task.Run(() =>
        {
            // Your background task here
                     
        });



        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            paused = !paused;
            pauseMenu.SetActive(paused);
            Time.timeScale = paused ? 0 : 1;
            Cursor.lockState =!paused?CursorLockMode.Locked: CursorLockMode.None;
           
            StartCoroutine(PauseGameAndSetUpUI());
        });

    }
    private void AddHoverSoundToButton(Button button)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>() ?? button.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) => { SoundManager.Instance.PlayMouseHoverSound(); });

        trigger.triggers.Add(entry);
    }

    private IEnumerator PauseGameAndSetUpUI() // Used for playing sound effect for the buttons
    {
        yield return new WaitUntil(() => paused);


        if (pauseMenu.activeInHierarchy)
        {
            SoundManager.Instance.SettingsBtn = GameObject.Find("BTN_Settings").GetComponent<Button>();

            SoundManager.Instance.MainMenuBtn = GameObject.Find("BTN_MainMenu").GetComponent<Button>();

            SoundManager.Instance.QuitBtn = GameObject.Find("BTN_Quit").GetComponent<Button>();

            AddHoverSoundToButton(SoundManager.Instance.SettingsBtn);
            AddHoverSoundToButton(SoundManager.Instance.MainMenuBtn);
            AddHoverSoundToButton(SoundManager.Instance.QuitBtn);


            SoundManager.Instance.SettingsBtn.onClick.AddListener(() => SoundManager.Instance.PlaySelectionSound());

            SoundManager.Instance.MainMenuBtn.onClick.AddListener(() => SoundManager.Instance.PlaySelectionSound());

            SoundManager.Instance.QuitBtn.onClick.AddListener(() => SoundManager.Instance.PlaySelectionSound());

            SoundManager.Instance.QuitBtn.onClick.AddListener(()=>QuitGame());


        }
      
   
       yield return new WaitUntil(() => !paused);

        SoundManager.Instance.SettingsBtn = null;

        SoundManager.Instance.MainMenuBtn = null;

        SoundManager.Instance.QuitBtn = null;


    }

    public IEnumerator PlayConfirmationSound()
    {
        yield return new WaitUntil(() => player.selectionPageOpened&&!paused);

        if (alienProfilePage.activeInHierarchy)
        {
            AddHoverSoundToButton(confirmSelectionBtn);

            confirmSelectionBtn.onClick.AddListener(()=> SoundManager.Instance.PlaySelectionSound());
        }
    }

    public void ShowItemInstruction(string itemName, bool showing)
    {
        if (displayedItemInfos.TryGetValue(itemName, out var itemInfo))
        {
            if(currentItemInfo==null)
                currentItemInfo = itemInfo;
            else
            {
                currentItemInfo.SetActive(showing);
            }

            if (showing)
            {
                // Optionally, hide all other item instructions if displaying a new one
                foreach (var otherItemInfo in displayedItemInfos.Values)
                {
                    if (otherItemInfo != itemInfo)
                    {
                        otherItemInfo.SetActive(false);
                    }
                }
            }
            else return;

         
        }
        else
        {
            Debug.LogWarning($"Item instruction info for {itemName} not found.");
        }
    }


    public IEnumerator AlienComesUpForInvestigation()
    {
        yield return new WaitUntil(() => alienComesUp);

        alienPlatformAnim.SetTrigger("Up");


    }

    public void DisplaySelectedAlienProfile() // Need more complex logics after all the alien creatures are created
    {
        alienProfilePage.SetActive(true);

        alienMainProfilePage.SetActive(false);
    }


    public void ConfirmAlienSelection() //Need further updates as there are multiple aliens to select from. The logics of this should not be smplified
    {
        Debug.Log("Alien selection confirmed!");

        player.selectionPageOpened = false; alienSelectionPage.SetActive(player.selectionPageOpened);

        Time.timeScale = player.selectionPageOpened ? 0.0f : 1.0f;
        Cursor.visible = player.selectionPageOpened ? true : false;


        Cursor.lockState = !player.selectionPageOpened && !WorldManager.Instance.paused ? CursorLockMode.Locked : CursorLockMode.None;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowSettingsPage() => settingsMenuInGame.SetActive(true);

    public void GoBackToPauseMenu()
    {
        settingsMenuInGame.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void GoBackToMainMenu() => SceneManager.LoadScene("MainMenu");




}
