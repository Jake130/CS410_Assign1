using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    private bool canDoubleJump = true;
    public float speed = 10.0f;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public float groundCheckDistance = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Get and store the Rigidbody
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);
    }

    private void Update() {
        if (isGrounded() && !Input.GetButtonDown("Jump")) {
            //print("Grounded!");
            canDoubleJump = true;
        }

        if (Input.GetButtonDown("Jump")) {
            if (isGrounded()) {
                Vector3 upVector = new Vector3(0.0f, 400.0f, 0.0f);
                rb.AddForce(upVector);
            } else if (canDoubleJump) {
                Vector3 upVector = new Vector3(0.0f, 400.0f, 0.0f);
                rb.AddForce(upVector);
                canDoubleJump = false;
            }
        }
    }

    //Called once per FIXED frame-rate
    private void FixedUpdate() {
        //Apply force dynamically
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement*speed);
    }

    //Called when move input detected
    void OnMove(InputValue movementValue) {
        //Convert movementValue to a vector
        Vector2 movementVector = movementValue.Get<Vector2>();
        //Debug.Log(movementVector);
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void OnTriggerEnter(Collider other) {
        //If we hit a pickup, disable it
        if (other.CompareTag("Pickup")) {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
    }

    bool isGrounded() {
        Vector3 rayOrigin = transform.position + Vector3.down * 0.5f;
        //Debug.DrawRay(rayOrigin, Vector3.down * groundCheckDistance, Color.red);
        return Physics.Raycast(rayOrigin, Vector3.down, groundCheckDistance);
    }


    void SetCountText () {
        countText.text = "Count: " + count.ToString();
        if (count >=12) {
            winTextObject.SetActive(true);
        }
    }
}
