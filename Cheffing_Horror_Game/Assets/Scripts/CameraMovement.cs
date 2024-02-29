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

    private bool flashlightOn = false;

    [SerializeField] private float interactionDistance = 10.0f;
    [SerializeField] private LayerMask pickupLayer;
    [SerializeField] private GameObject guidanceText;
    [SerializeField] private bool itemPickedUp=false;
    [SerializeField] private Transform pickUpAttachPoint;
    [SerializeField] private GameObject currentItem;
    [Header("Items to pick up")]
    [SerializeField] private GameObject torch;
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
        ToggleFlashlight();DetachObjectToArm();
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
        transform.localRotation = cameraRotation;
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

    private void ToggleFlashlight()
    {
        if(!itemPickedUp) { return; }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!flashlightOn)
            {
                flashLight.enabled = true; flashlightOn = true;
            }
            else
            {
                flashLight.enabled = false; flashlightOn = false;
            }
        }
       
    }

    private void DetectObjectPickUps()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance,pickupLayer))
        {
            guidanceText.SetActive(true);

            Debug.Log("Found a " + hit.collider.name);

            if (Input.GetKeyDown(KeyCode.E))
            {
                itemPickedUp = !itemPickedUp;
                TogglePickingUpItems(hit, itemPickedUp);

            }

           

        }
        else
        {
            guidanceText.SetActive(false);
           
        }

      

    }

    private void TogglePickingUpItems(RaycastHit hit,bool isPickedUp)
    {
        if(isPickedUp)
        {

            Debug.Log($"Picked up {hit.collider.gameObject.name}");

            AttachObjectToArm(hit.collider.gameObject);
        }
      
    }

    private void AttachObjectToArm(GameObject itemPicked)
    {
        itemPicked.transform.parent = pickUpAttachPoint;
        itemPicked.transform.position = pickUpAttachPoint.position;
        currentItem= itemPicked;
    }

    private void DetachObjectToArm()
    {
        
        if(currentItem!=null)
        {
            if (!itemPickedUp) return;
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (currentItem = GameObject.Find("Torch"))
                {
                    flashLight.enabled = false; flashlightOn = false;
                }
                itemPickedUp = !itemPickedUp;
                currentItem.transform.parent = null;
                currentItem = null;

                
               
            }
        }
    }
}
