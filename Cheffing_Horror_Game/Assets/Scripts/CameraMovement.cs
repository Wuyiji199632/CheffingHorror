using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class CameraMovement : MonoBehaviour
{
    public float mouseSensitivity = 220f;

    public float movementSpeed = 10f;
    //public Transform playerBody;
    float xRotation=0,yRotation = 0f;

    private Rigidbody rb;

    [SerializeField] Light flashLight;

    private bool itemFunctionOn = false;

    [SerializeField] private float interactionDistance = 10.0f;
    [SerializeField] private LayerMask pickupLayer;
    [SerializeField] private GameObject guidanceText;
    [SerializeField] private bool itemPickedUp=false;
    [SerializeField] private Transform pickUpAttachPoint;
    [SerializeField] private GameObject currentItem;
    [SerializeField] private GameObject parentContainer;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb= GetComponent<Rigidbody>();  
        flashLight.enabled = false;
        guidanceText = GameObject.Find("GuidanceText");
        guidanceText.SetActive(false);
        InvokeRepeating("DetectObjectPickUps", 0.01f, 0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMouseRotationMovement(); UpdateTranslationMovement();
        ToggleObjectFunctionalities();DetachObjectToArm();
       
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
    }

    private void UpdateTranslationMovement()
    {
        float horizontalMovement = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        float verticalMovement = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;

        // Moving the camera based on the current rotation
        Vector3 movement = transform.right * horizontalMovement + transform.forward * verticalMovement;
        rb.velocity = movement;
        transform.Translate(rb.velocity, Space.World);
    }

    private void ToggleObjectFunctionalities()
    {
        if(!itemPickedUp) { return; }
        if (Input.GetMouseButtonDown(0))
        {
            //Differentiate the functionalities in terms of item names
            if (currentItem.name == "Torch")
            {
                if (!itemFunctionOn)
                {
                    flashLight.enabled = true; itemFunctionOn = true;
                }
                else
                {
                    flashLight.enabled = false; itemFunctionOn = false;
                }
            }
            else if (currentItem.name == "Taser")
            {
                if (!itemFunctionOn)
                {
                    Debug.Log($"Taser turned on!"); itemFunctionOn = true;
                }
                else
                {
                    Debug.Log($"Taser turned off!"); itemFunctionOn = false;
                }
            }
           
         
        }
       
    }

    private void DetectObjectPickUps()
    {
        RaycastHit hit;
        bool rayHit = Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance, pickupLayer);
        if (rayHit && WorldManager.Instance.displayedItemInfos.ContainsKey(hit.collider.gameObject.name))
        {
            guidanceText.SetActive(!itemPickedUp);

            Debug.Log("Found a " + hit.collider.name);

            if (Input.GetKeyDown(KeyCode.E))
            {
                itemPickedUp = !itemPickedUp;
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
    

    private void TogglePickingUpItems(RaycastHit hit,bool isPickedUp)
    {
       
        if (isPickedUp)
        {

            Debug.Log($"Picked up {hit.collider.gameObject.name}");

            AttachObjectToArm(hit.collider.gameObject);
        }
      
      
    }
    private void ReleaseItem()
    {
        currentItem.GetComponent<Rigidbody>().isKinematic = false;
       
        if (currentItem.name == "Torch")
        {
            flashLight.enabled = false; itemFunctionOn = false;
        }

        itemPickedUp = !itemPickedUp;
        currentItem.transform.parent = null;
        currentItem = null;
    }
    private void AttachObjectToArm(GameObject itemPicked)
    {
        if (itemPicked == null) return;

        
        itemPicked.transform.parent = pickUpAttachPoint;
        itemPicked.transform.position = pickUpAttachPoint.position;
        AdjustTransformsBasedOnItemName(itemPicked);
        itemPicked.transform.localRotation= Quaternion.Euler(0,90,0); //Adjust as needed

        itemPicked.gameObject.GetComponent<Rigidbody>().isKinematic = true;

        currentItem = itemPicked;
    }

    private void DetachObjectToArm()
    {

        if (currentItem != null && itemPickedUp && Input.GetKeyDown(KeyCode.E))
        {
            // No need to check if itemPickedUp is false here, as it's already confirmed to be true
            currentItem.GetComponent<Rigidbody>().isKinematic = false;

            if (currentItem.name == "Torch")
            {
                flashLight.enabled = false;
                itemFunctionOn = false;
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

        switch(currentItem.name)
        {
            case "Torch":

                break;
            case "Taser":
                currentItem.transform.localPosition = new Vector3(0, -0.7f, 0);
                             
                break;

                default:              
                break;
        }
    }
}
