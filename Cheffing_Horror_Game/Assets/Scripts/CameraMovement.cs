using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraMovement : MonoBehaviour
{
    public float mouseSensitivity = 120f;

    public float movementSpeed = 10f;
    //public Transform playerBody;
    float xRotation=0,yRotation = 0f;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb= GetComponent<Rigidbody>();  
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMoseRotationMovement(); UpdateTranslationMovement();
    }

    private void UpdateMoseRotationMovement()
    {
        float mouseX=Input.GetAxis("Mouse X")*mouseSensitivity*Time.deltaTime;

        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation += mouseX;
        xRotation= Mathf.Clamp(xRotation, -360f, 360f);

       
        yRotation += mouseY;
        yRotation = Mathf.Clamp(yRotation, -90f,90f);


        transform.localRotation = Quaternion.Euler(-yRotation, xRotation, 0f);
        //this.transform.Rotate(Vector3.up * mouseX);
    }

    private void UpdateTranslationMovement()
    {
        float horizontalTrans = Input.GetAxis("Horizontal"),verticalTrans=Input.GetAxis("Vertical");

        transform.Translate(horizontalTrans*movementSpeed, 0, verticalTrans*movementSpeed);
    }
}
