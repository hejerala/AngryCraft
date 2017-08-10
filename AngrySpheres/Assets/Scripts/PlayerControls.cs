using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

    public GameObject bulletPrefab;
    public GameObject editIcon;
    public GameObject cubePrefab;
    public GameObject frogPrefab;

    private GameObject selectedGameObject;

    protected Rigidbody parentRb;
    private BoxCollider parentBxc;
    private bool isHolding = false;
    public float speed = 5.0f;
    public float zoomSpeed = 15.0f;
    public float jumpHeight = 8.0f;
    private float groundedRaycastDistance = 0.1f;
    public float selectObjectRaycastDistance = 50.0f;
    private bool inEditMode = false;
    public float bulletInstantiateDistance = 2.0f;
    public float objectInstantiateDistance = 5.0f;

    // Use this for initialization
    void Start () {
        parentRb = transform.parent.GetComponent<Rigidbody>();
        parentBxc = transform.parent.GetComponent<BoxCollider>();
    }
	
	// Update is called once per frame
	void Update () {
        //Hides the cursor and locks it to the center of the screen
        //Ctrl + P Play/Stop
        //Ctrl + Shift + P Pause/Unpause
        Cursor.lockState = CursorLockMode.Locked;
        //Input.GetAxis("Mouse X"); returns the delta movement of my mouse in the last frame
        //When I move my mouse left/down its negative, right/up its positive
        float mouseXInput = Input.GetAxis("Mouse X");
        float mouseYInput = Input.GetAxis("Mouse Y");
        //Takes the movement on the scroll wheel it only does something if the user is holding an object
        float mouseWheelInput = Input.GetAxis("Mouse ScrollWheel");
        if (isHolding)
            MoveSelectedObject(mouseWheelInput);
        //We make sure we rotate the parent in one axis, so this transforms rotation angle remains around one axis only
        transform.Rotate(-mouseYInput, 0.0f, 0.0f);
        transform.parent.Rotate(0.0f, mouseXInput, 0.0f);

        //We also get the movement from the corresponding axis and call the movement functions
        float horizontalInput = Input.GetAxis("Horizontal");
        MoveHorizontal(horizontalInput);
        float verticalInput = Input.GetAxis("Vertical");
        MoveVertical(verticalInput);

        //Checks if the object is grounded, if so it calls the jump function when the user presses any of the shift keys
        if (IsGrounded())
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
                Jump();

        //Toggles the edition mode on and off
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (inEditMode && !isHolding) {
                editIcon.SetActive(false);
                inEditMode = false;
            }
            else if (!isHolding) {
                editIcon.SetActive(true);
                inEditMode = true;
            }
        }
        //Checks if you are on edit mode or not
        if (inEditMode) {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                //Creates and places a crate
                Instantiate(cubePrefab, transform.position + (transform.forward * objectInstantiateDistance), transform.rotation);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                //Creates and places a frog
                Instantiate(frogPrefab, transform.position + (transform.forward * objectInstantiateDistance), transform.rotation);
            }

            if (Input.GetMouseButtonDown(0) && !isHolding) {
                //Debug.DrawRay(transform.position + (transform.forward * bulletInstantiateDistance), transform.forward * selectObjectRaycastDistance, Color.red);
                //return !(hitInfo.collider == null);
                //Shoots a ray cast to hold the first object collided (the clicked object)
                RaycastHit hitInfo;
                if (Physics.Raycast(transform.position + (transform.forward * bulletInstantiateDistance), transform.forward, out hitInfo, selectObjectRaycastDistance))
                {
                    //Debug.Log(hitInfo.collider.name);
                    //Checks if the selected object is an editable one (only crates and frogs have this tag)
                    if (hitInfo.collider.gameObject.tag == "Editable")
                    {
                        //Takes the object, strips it of physics and places it as a child of the camera so it will move and rotate accordingly
                        selectedGameObject = hitInfo.rigidbody.gameObject;
                        selectedGameObject.GetComponent<Rigidbody>().isKinematic = true;
                        selectedGameObject.transform.SetParent(transform);
                        isHolding = true;
                        //Debug.Log(selectedGameObject.name);
                    }
                }
            }

            if (Input.GetMouseButtonUp(0) && isHolding) {
                //When the user finishes moving the object its placed on the world and applies physics to it
                selectedGameObject.GetComponent<Rigidbody>().isKinematic = false;
                selectedGameObject.transform.SetParent(null);
                isHolding = false;
            }

            if (Input.GetKeyDown(KeyCode.E) && isHolding) {
                //If the user is holding an object then you can rotate it
                selectedGameObject.transform.Rotate(0.0f, 0.0f, 90.0f);
            }

        } else {//If Edit mode
            //Outside of edit mode you can only shoot
            if (Input.GetMouseButtonDown(0))
            {
                //Creates and shoots a bullet
                Instantiate(bulletPrefab, transform.position + (transform.forward * bulletInstantiateDistance), transform.rotation);
            }
        }//Else Edit mode
    }


    protected void MoveHorizontal(float input) {
        //Moves the parent object either right or left from wherever the camera is facing
        if (input > 0) {
            transform.parent.position += transform.right * Time.deltaTime * speed;
        }
        else if (input < 0) {
            transform.parent.position -= transform.right * Time.deltaTime * speed;
        }
    }

    protected void MoveVertical(float input) {
        //Moves the parent object either forward or backward from wherever the camera is facing
        if (input > 0) {
            transform.parent.position += transform.forward * Time.deltaTime * speed;
        }
        else if (input < 0) {
            transform.parent.position -= transform.forward * Time.deltaTime * speed;
        }
    }

    protected void Jump() {
        //Use physics instead of transform because transform causes a jitter when colliding
        //Dont reset velocity on Y or you wont fall correctly. Always keep the Y
        parentRb.velocity = new Vector3(parentRb.velocity.x, jumpHeight, parentRb.velocity.z);
    }

    protected void MoveSelectedObject(float input) {
        //It moves the selected object closer of further from the camera
        if (input > 0) {
            selectedGameObject.transform.position += transform.forward * Time.deltaTime * zoomSpeed;
        }
        else if (input < 0) {
            selectedGameObject.transform.position -= transform.forward * Time.deltaTime * zoomSpeed;
        }
    }

    protected bool IsGrounded() {
        //Physics.Raycast
        //Debug.DrawRay(new Vector3(parentBxc.transform.position.x, parentBxc.transform.position.y, parentBxc.transform.position.z), Vector3.down * (groundedRaycastDistance + (parentBxc.transform.localScale.y / 2)), Color.red);
        RaycastHit hitInfo;
        return Physics.Raycast(new Vector3(parentBxc.transform.position.x, parentBxc.transform.position.y, parentBxc.transform.position.z), Vector3.down, out hitInfo, groundedRaycastDistance + (parentBxc.transform.localScale.y / 2));
    }


}
