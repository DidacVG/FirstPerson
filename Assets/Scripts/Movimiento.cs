using UnityEngine;
using UnityEngine.InputSystem;

public class Movimiento : MonoBehaviour
{
    public DetectarSuelo detectorSuelo;
    public float velocidadMovimiento = 5f;
    public float multiplicadorSprint = 2f;
    public float velocidadAtrasMultiplicador = 0.5f;
    public float velocidadLateralMultiplicador = 0.75f;
    public float fuerzaSalto = 5f;

    private Rigidbody rb;
    private Vector2 moveInput = Vector2.zero;
    private bool Sprint = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Reset input cada frame
        moveInput = Vector2.zero;

        // Leer teclado (nuevo Input System)
        if (Keyboard.current.wKey.isPressed) moveInput.y += 1f;
        if (Keyboard.current.sKey.isPressed) moveInput.y -= 1f;
        if (Keyboard.current.aKey.isPressed) moveInput.x -= 1f;
        if (Keyboard.current.dKey.isPressed) moveInput.x += 1f;

        // Correr con Shift
        Sprint = Keyboard.current.leftShiftKey.isPressed && moveInput.y > 0f;

        // Salto
        if (detectorSuelo.Grounded && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            Debug.Log("brinca");
        }
    }

    void FixedUpdate()
    {
        Vector3 direccion = transform.forward * moveInput.y + transform.right * moveInput.x;

        if (direccion.sqrMagnitude > 1f)
            direccion.Normalize();

        float velocidadActual = velocidadMovimiento;

        if (Sprint)
        {
            velocidadActual *= multiplicadorSprint;
        }

        if (moveInput.y < 0f)
        {
            velocidadActual *= velocidadAtrasMultiplicador;
        }

        if (moveInput.x != 0f)
        {
            velocidadActual *= velocidadLateralMultiplicador;
        }

        Vector3 movimiento = direccion * velocidadActual * Time.fixedDeltaTime;
        Vector3 nuevaPos = rb.position + movimiento;

        rb.MovePosition(nuevaPos);
    }
}

