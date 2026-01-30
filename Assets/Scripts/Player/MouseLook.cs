using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [SerializeField] float turnSpeed = 90f;
    [SerializeField] float headUpperAngleLimit = 85f;
    [SerializeField] float headLowerAngleLimit = -80f;

    // current rotation from our start
    float yaw = 0f; // guinada
    float pitch = 0f;

    //stores the orientation of the head and body when the game is started
    Quaternion bodyStartOrientation;
    Quaternion headStartOrientation;

    // A reference to the ehad object to rotate up and down
    Transform head;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        head = GetComponentInChildren<Camera>().transform;

        bodyStartOrientation = transform.localRotation;
        headStartOrientation = head.transform.localRotation;
    }

    void Update()
    {
        // var horizontal = Input.GetAxis("Mouse X") * Time.deltaTime * turnSpeed;
        // var vertical = Input.GetAxis("Mouse Y") * Time.deltaTime * turnSpeed;

        float horizontal = Mouse.current.delta.x.ReadValue() * turnSpeed * Time.fixedDeltaTime;
        float vertical = Mouse.current.delta.y.ReadValue() * turnSpeed * Time.fixedDeltaTime;

        // update the yaw and pitch values
        yaw += horizontal;
        pitch -= vertical; // Invertido para o movimento vertical padrão (mover mouse para cima olha para cima)

        // clamp pitch so that it can't look directly down or up
        pitch = Mathf.Clamp(pitch, headLowerAngleLimit, headUpperAngleLimit);

        // Compute a rotation for the body by rotating around the y-axis by
        // the number of yaw degrees, and for the head around the x-axis by 
        //the number of pitch degrees
        var bodyRotation = Quaternion.AngleAxis(yaw, Vector3.up);
        var headRotation = Quaternion.AngleAxis(pitch, Vector3.right);

        // Create new rotations for the body and head by combining 
        // them with theis start rotations
        transform.localRotation = bodyRotation * bodyStartOrientation;
        head.localRotation = headRotation * headStartOrientation;


    }
}
