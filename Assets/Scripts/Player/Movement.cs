using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float gravity = 20f;
    [Range(0, 10), SerializeField] float airControl = 5f;

    CharacterController controller;
    PlayerInput playerInput;

    // Referências para as ações dentro do Asset
    InputAction moveAction;
    InputAction jumpAction;

    Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        // Buscamos as ações pelo nome definido no Input Action Asset

        moveAction = playerInput.currentActionMap.FindAction("Move");
        jumpAction = playerInput.actions.FindAction("Jump");
    }

    void Update()
    {
        // 1. Leitura de Input
        Vector2 inputVector = moveAction.ReadValue<Vector2>();
        Vector3 moveInput = new Vector3(inputVector.x, 0, inputVector.y);
        moveInput = transform.TransformDirection(moveInput) * moveSpeed;

        // 2. Lógica de Chão e Pulo
        if (controller.isGrounded)
        {
            moveDirection.x = moveInput.x;
            moveDirection.z = moveInput.z;

            if (moveDirection.y < 0) moveDirection.y = -2f;

            // jumpAction.triggered verifica se o botão foi apertado NESTE frame
            if (jumpAction.triggered)
            {
                moveDirection.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
            }
        }
        else
        {
            // Movimentação no ar (Lerp para suavizar)
            float verticalVelocity = moveDirection.y;
            moveDirection = Vector3.Lerp(moveDirection, moveInput, airControl * Time.deltaTime);
            moveDirection.y = verticalVelocity;
        }

        // 3. Gravidade e Movimento final
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}