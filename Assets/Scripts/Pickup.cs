using UnityEngine;
using UnityEngine.InputSystem;

public class Pickup : MonoBehaviour
{
    public float distanciaMaxima = 0f;
    public LayerMask pickupLayer;
    public Transform puntoAgarre;

    public float distanciaMin = 0f;
    public float distanciaMax = 0f;
    private float distanciaActual;

    public float kp = 500f;
    public float kd = 20f;

    private Rigidbody objetoActual;
    private Camera cam;

    private Vector3 velocidadAntesDeSoltar;

    void Start()
    {
        cam = Camera.main;
        distanciaActual = puntoAgarre.localPosition.z;
    }

    void Update()
    {
        // --- SCROLL ---
        float scroll = Mouse.current.scroll.ReadValue().y;
        if (scroll != 0)
        {
            distanciaActual = Mathf.Clamp(distanciaActual + scroll * 0.1f, distanciaMin, distanciaMax);
            puntoAgarre.localPosition = new Vector3(0, 0, distanciaActual);
        }

        // --- PICK / DROP ---
        if (Mouse.current.leftButton.isPressed)
        {
            if (objetoActual == null)
                IntentarRecogerObjeto();
        }
        else
        {
            SoltarObjeto();
        }
    }

    void IntentarRecogerObjeto()
    {
        Ray rayo = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Debug.DrawRay(rayo.origin, rayo.direction * distanciaMaxima, Color.blue, 1f);

        if (Physics.Raycast(rayo, out RaycastHit hit, distanciaMaxima, pickupLayer))
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();

            if (rb != null)
            {
                objetoActual = rb;
                objetoActual.useGravity = false;
                objetoActual.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                objetoActual.interpolation = RigidbodyInterpolation.Interpolate;
            }
        }
    }

    void FixedUpdate()
    {
        if (objetoActual != null)
        {
            Vector3 objetivo = puntoAgarre.position;
            Vector3 error = objetivo - objetoActual.position;

            // Control derivativo (evita órbitas)
            Vector3 d = -objetoActual.linearVelocity;

            // Fuerza PID
            Vector3 fuerza = error * kp + d * kd;

            objetoActual.AddForce(fuerza);

            // Guardar velocidad antes de soltar
            velocidadAntesDeSoltar = objetoActual.linearVelocity;
        }
    }

    void SoltarObjeto()
    {
        if (objetoActual != null)
        {
            objetoActual.useGravity = true;

            // Lanza el objeto manteniendo velocidad
            objetoActual.linearVelocity = velocidadAntesDeSoltar;

            objetoActual = null;
        }
    }
}




