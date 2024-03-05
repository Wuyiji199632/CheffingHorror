using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class WorldManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private bool paused = false;

    
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
}
