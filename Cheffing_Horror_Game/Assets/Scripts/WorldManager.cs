using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;
public class WorldManager : MonoBehaviour //This is the class that controls the environmentally related gameplay mechanics
{
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private bool paused = false;

    public GameObject currentItemInfo;

    public bool cursorPointingToItem = false;

    public static WorldManager Instance { get; private set; }

    public Dictionary<string, GameObject> displayedItemInfos = new Dictionary<string, GameObject>();

    public List<GameObject> itemInfos = new List<GameObject>();

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
       
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ManipulatePauseMenu();
        }
      
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
    private IEnumerator PauseGameAndSetUpUI()
    {
        yield return new WaitUntil(() => paused);

        SoundManager.Instance.SettingsBtn = GameObject.Find("BTN_Settings").GetComponent<Button>();

        SoundManager.Instance.MainMenuBtn = GameObject.Find("BTN_MainMenu").GetComponent<Button>();

        SoundManager.Instance.QuitBtn = GameObject.Find("BTN_Quit").GetComponent<Button>();

        yield return new WaitUntil(() => !paused);

        SoundManager.Instance.SettingsBtn = null;

        SoundManager.Instance.MainMenuBtn = null;

        SoundManager.Instance.QuitBtn = null;


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

}
