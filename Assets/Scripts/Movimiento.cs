using UnityEngine;
using UnityEngine.InputSystem;

public class Movimiento : MonoBehaviour
{
    public DetectarSuelo detectorSuelo;
    public float velocidadMovimiento = 0f;
    public float fuerzaSalto = 0f;

    private Rigidbody rb;
    private Vector2 moveInput = Vector2.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        moveInput = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) moveInput.y += 1f;
        if (Keyboard.current.sKey.isPressed) moveInput.y -= 1f;
        if (Keyboard.current.aKey.isPressed) moveInput.x -= 1f;
        if (Keyboard.current.dKey.isPressed) moveInput.x += 1f;

        if (detectorSuelo.Grounded && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            Debug.Log("brinca");
        }
    }

    void FixedUpdate()
    {
        if (detectorSuelo.Grounded)
        {
            Vector3 direccion = transform.forward * moveInput.y + transform.right * moveInput.x;

            if (direccion.sqrMagnitude > 1f)
                direccion = direccion.normalized;

            Vector3 movimiento = direccion * velocidadMovimiento * Time.fixedDeltaTime;
            Vector3 nuevaPos = rb.position + movimiento;
            nuevaPos.y = rb.position.y;

            rb.MovePosition(nuevaPos);
        }
        
    }
}

