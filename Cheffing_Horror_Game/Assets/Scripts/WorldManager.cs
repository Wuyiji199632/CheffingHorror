using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;
public class WorldManager : MonoBehaviour
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
    }

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        //InvokeRepeating(nameof(ManipulatePauseMenu), 0.5f, 0.5f);
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
        });

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
