using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using Unity.VisualScripting;

public class CameraMovement : MonoBehaviour //The class that controls movement of the player
{
    public float mouseSensitivity = 220f;

    public float movementSpeed = 10f;
    //public Transform playerBody;
    float xRotation=0,yRotation = 0f;

    private Rigidbody rb;
    public RaycastHit hit;
    [SerializeField] Light flashLight;

    
    [SerializeField] private float interactionDistance = 20.0f;
    [SerializeField] private LayerMask pickupLayer,wallLayer,doorLayer,interactableLayer,renderedLayer;
    [SerializeField] private GameObject guidanceText,doorDetectionText,beginResearchText;
    [SerializeField] private bool itemPickedUp=false;
    [SerializeField] private Transform pickUpAttachPoint;
    [SerializeField] private GameObject currentItem;
    private const float backwardSpeed = 10.0f;
    public bool selectionPageOpened = false;
    
    public  bool notepadOpened = false;
    public GameObject notepad;
    [SerializeField] private GameObject pickupItemRenderCam;
    public List<PickUpItem> pickUpItems = new List<PickUpItem>();
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb= GetComponent<Rigidbody>();  
        flashLight.enabled = false;
        guidanceText = GameObject.Find("GuidanceText");
        
        guidanceText.SetActive(false); doorDetectionText.SetActive(false);notepad.SetActive(false); pickupItemRenderCam.SetActive(false);
       
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(DetectObjectPickUps());StartCoroutine(ManipulateDoors());StartCoroutine(InteractWithStaticObjs());StartCoroutine(TakingNotesOnNotepad());
        DetachObjectToArm();
       
    }

    private void FixedUpdate()
    {
        
        UpdateMouseRotationMovement(); UpdateTranslationMovement();
    }
    private void LateUpdate()
    {
        ToggleObjectFunctionalities(); WorldManager.Instance.StartCoroutine(WorldManager.Instance.PlayConfirmationSound());
    }

    private void UpdateMouseRotationMovement()
    {
        float mouseX=Input.GetAxis("Mouse X")*mouseSensitivity*Time.deltaTime;

        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation += mouseX;
        //xRotation= Mathf.Clamp(xRotation, -360f, 360f);
      
       
        yRotation += mouseY;
        yRotation = Mathf.Clamp(yRotation, -90f,90f);


        // Applying both pitch and yaw rotation in one operation to the camera
        Quaternion cameraRotation = Quaternion.Euler(-yRotation, xRotation, 0f);
        Camera.main.transform.localRotation = cameraRotation;
        transform.localRotation=Quaternion.Euler(0, xRotation, 0);
    }

    private void UpdateTranslationMovement()
    {
        float horizontalMovement = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        float verticalMovement = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;

        // Moving the camera based on the current rotation
        Vector3 movement =Camera.main.transform.right * horizontalMovement +Camera.main.transform.forward * verticalMovement;
        rb.velocity = movement;
        transform.Translate(rb.velocity, Space.World);
    }

    private void ToggleObjectFunctionalities()
    {
        if(!itemPickedUp) { return; }
        if (Input.GetMouseButtonDown(0))
        {
            PickUpItem item = currentItem.GetComponent<PickUpItem>();
            //Differentiate the functionalities in terms of item names
            if (currentItem.name == "Torch")
            {
               
                if (!item.itemFunctionOn)
                {
                    flashLight.enabled = true; item.itemFunctionOn = true;
                }
                else
                {
                    flashLight.enabled = false; item.itemFunctionOn = false;
                }

                item.PlayTorchSounds();
            }

            else if (currentItem.name == "Taser")
            {
               
                Debug.Log($"Current position for item attach point is {pickUpAttachPoint.localPosition}");
                if (!item.itemFunctionOn)
                {
                    Debug.Log($"Taser turned on!"); item.itemFunctionOn = true;
                    
                    
                    SoundManager.Instance.PlayZappingSound();
                   

                }
                else
                {
                    Debug.Log($"Taser turned off!"); item.itemFunctionOn = false;
                    
                    SoundManager.Instance.StopZappingSound();
                    
                }

                AdjustTransformsBasedOnItemName(currentItem);
            }

            else if(currentItem.name == "Rubber_Duck")
            {
                Debug.Log("We need to throw the duck!");

                ThrowDuck(currentItem);
            }
           
         
        }

           
    }
    private void ThrowDuck(GameObject currentItem)
    {

        // No need to check if itemPickedUp is false here, as it's already confirmed to be true
        currentItem.GetComponent<Collider>().isTrigger = false;
        currentItem.GetComponent<PickUpItem>().thrown = true;
       

        // Detach the item from the player
        currentItem.transform.parent = null;
        currentItem = null;

        // Set itemPickedUp to false since the item is now detached
        itemPickedUp = false;


    }

    private IEnumerator DetectObjectPickUps()
    {

        bool rayHit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionDistance, pickupLayer)&&!itemPickedUp&&!notepadOpened;
        if (rayHit && WorldManager.Instance.displayedItemInfos.ContainsKey(hit.collider.gameObject.name)) //Need to make sense that the name for the object of display info is the same as the object to pick up
        {
            guidanceText.GetComponent<TextMeshProUGUI>().text = "Press E to Pick Up";
            guidanceText.SetActive(!itemPickedUp);

            Debug.Log("Found a " + hit.collider.name);

            if (Input.GetKeyDown(KeyCode.E))
            {
                itemPickedUp = !itemPickedUp;
                yield return new WaitForSeconds(0.1f);
                TogglePickingUpItems(hit, itemPickedUp);
            }

            WorldManager.Instance.ShowItemInstruction(hit.collider.gameObject.name, true);
        }
        else
        {
            guidanceText.SetActive(false);

            if (WorldManager.Instance.currentItemInfo != null) // Ensure currentItemInfo has been assigned
            {
                WorldManager.Instance.ShowItemInstruction(WorldManager.Instance.currentItemInfo.name, false);
                WorldManager.Instance.currentItemInfo = null;
            }
        }

    }

    private IEnumerator ManipulateDoors()
    {
        yield return new WaitForSeconds(0.1f);

        RaycastHit hit;

        bool rayHit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionDistance, doorLayer)&&!notepadOpened;

        if(rayHit)
        {
            Debug.Log("Time to do something for the door!");
            doorDetectionText.GetComponent<TextMeshProUGUI>().text = hit.collider.gameObject.GetComponent<DoorObject>().opened? "Press TAB To Close Door" : "Press TAB To Open Door";
            doorDetectionText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                hit.collider.gameObject.GetComponent<DoorObject>().opened = !hit.collider.gameObject.GetComponent<DoorObject>().opened;
                DoorObject doorObject = hit.collider.gameObject.GetComponent<DoorObject>();               
               
                doorObject.PlayDoorSounds();
             
                if (hit.collider.gameObject.GetComponent<DoorObject>().opened)
                {
                    hit.collider.gameObject.GetComponent<Animator>().SetTrigger("Open");
                }
                else
                {
                    hit.collider.gameObject.GetComponent<Animator>().SetTrigger("Close");
                }

                
            }
          
        }
        else
        {
            doorDetectionText.SetActive(false);
        }
    }

   
    private IEnumerator InteractWithStaticObjs()
    {
        yield return new WaitForSeconds(0.1f);


        RaycastHit hit;

        bool rayHit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionDistance, interactableLayer);

        if (rayHit)
        {
            Debug.Log("Time to begin research");

            if (hit.collider.gameObject.CompareTag("Keyboard"))
            {
                beginResearchText.GetComponent<TextMeshProUGUI>().text = "Press T to open up alien selection page for research";

            }else if (hit.collider.gameObject.CompareTag("Keypad"))
            {
                beginResearchText.GetComponent<TextMeshProUGUI>().text = "Press TAB to begin research";

                if(Input.GetKeyDown(KeyCode.Tab))
                {
                    Debug.Log("Selected alien is coming up from the tube!");
                    WorldManager.Instance.alienComesUp = !WorldManager.Instance.alienComesUp;

                    StartCoroutine(WorldManager.Instance.AlienComesUpForInvestigation());
                }
            }

            beginResearchText.SetActive(true);

            yield return new WaitUntil(() => beginResearchText.activeSelf);

            if (Input.GetKeyDown(KeyCode.T))
            {
               
                selectionPageOpened = !selectionPageOpened;

                                        
            }
            
        }
        else
        {
            beginResearchText.SetActive(false); selectionPageOpened = false;

        }

       
        PauseForAlienSelection();

       
    }

    private IEnumerator TakingNotesOnNotepad()
    {
        yield return new WaitForSeconds(0.5f);

       
        if(Input.GetKeyDown(KeyCode.N))
        {
            notepadOpened = !notepadOpened;
          
        }

        if (notepadOpened)
            notepad.SetActive(true);
        else
            notepad.SetActive(false);
    }
    private void PauseForAlienSelection()
    {
        Time.timeScale = selectionPageOpened ? 0.0f : 1.0f;
        Cursor.visible = selectionPageOpened ? true : false;


        Cursor.lockState = !selectionPageOpened&&!WorldManager.Instance.paused ? CursorLockMode.Locked : CursorLockMode.None;

        WorldManager.Instance.alienSelectionPage.SetActive(selectionPageOpened);
    }
    private void TogglePickingUpItems(RaycastHit hit,bool isPickedUp)
    {
       
        if (isPickedUp)
        {

            Debug.Log($"Picked up {hit.collider.gameObject.name}");

            AttachObjectToArm(hit.collider.gameObject);

           
            

        }
        else
        {
           DetachObjectToArm(); 

        }

       StartCoroutine(RenderItemPickedUpCorrectlyWhenPickedUp());


       StartCoroutine(RenderItemPickedUpCorrectlyWhenNotPickedUp());

    }

    private IEnumerator RenderItemPickedUpCorrectlyWhenPickedUp()
    {
        yield return new WaitUntil(() => itemPickedUp);

        pickupItemRenderCam.SetActive(itemPickedUp);

        

    }

    private IEnumerator RenderItemPickedUpCorrectlyWhenNotPickedUp()
    {
        yield return new WaitUntil(() => !itemPickedUp);

        pickupItemRenderCam.SetActive(itemPickedUp);

       

    }



    private void AttachObjectToArm(GameObject itemPicked)
    {
        if (itemPicked == null) return;

        
        itemPicked.transform.parent = pickUpAttachPoint;
        itemPicked.transform.position = pickUpAttachPoint.position;
        AdjustTransformsBasedOnItemName(itemPicked);
        itemPicked.transform.localRotation= Quaternion.Euler(0,90,0); //Adjust as needed

        
       

        currentItem = itemPicked;
    }

    private void DetachObjectToArm()
    {

        if (currentItem != null && itemPickedUp && Input.GetKeyDown(KeyCode.E))
        {
            // No need to check if itemPickedUp is false here, as it's already confirmed to be true
            currentItem.GetComponent<Collider>().isTrigger = false;
            currentItem.GetComponent<Rigidbody>().isKinematic = false;
           
            if (currentItem.name == "Torch")//Turn off the torch
            {
                flashLight.enabled = false;
                currentItem.GetComponent<PickUpItem>().itemFunctionOn = false;
            }

            // Detach the item from the player
            currentItem.transform.parent = null;
            currentItem = null;

            // Set itemPickedUp to false since the item is now detached
            itemPickedUp = false;
        }
    }

    private void AdjustTransformsBasedOnItemName(GameObject currentItem)
    {
        if (currentItem == null) return;

        currentItem.GetComponent<Rigidbody>().isKinematic = true;
        switch (currentItem.name)
        {
            case "Torch":
               
                currentItem.transform.localPosition = new Vector3(0.151f, -0.488f, -0.735f);
                currentItem.transform.localRotation = Quaternion.Euler(0, 90, 0);
               
                break;
            case "Taser":                   
                currentItem.transform.localPosition = new Vector3(0, -0.7f,currentItem.GetComponent<PickUpItem>().itemFunctionOn?1.0f: 0);
                currentItem.GetComponent<Collider>().isTrigger = true;
                break;
            case "Rubber_Duck":
                currentItem.transform.localPosition = new Vector3(-0.4f, 0, -0.02f);
                break;
         
            default:             
                break;
        }
    }

    

   
}
