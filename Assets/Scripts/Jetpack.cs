using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Jetpack : MonoBehaviour
{
    public DetectarSuelo detectorSuelo;
    public Rigidbody rb;
    public Image jetpackUI;

    public float fuerzaJetpack = 15f;

    public float duracionCombustible = 1f;  // Se gasta en 1s
    public float velocidadRecarga = 2f;     // 0.5s para recargar
    public float retardoRecarga = 0.5f;     // Si se gasta del todo

    private float combustible = 1f;
    private bool agotado = false;
    private bool puedeRecargar = false;
    private float temporizadorRecarga = 0f;

    void Update()
    {
        if (jetpackUI != null)
            jetpackUI.fillAmount = combustible;

        if (detectorSuelo.Grounded)
        {
            if (agotado)
            {
                temporizadorRecarga += Time.deltaTime;

                if (temporizadorRecarga >= retardoRecarga)
                {
                    agotado = false;
                    puedeRecargar = true;
                }
            }
            else
            {
                puedeRecargar = true;
            }

            if (puedeRecargar)
            {
                combustible += velocidadRecarga * Time.deltaTime;
                combustible = Mathf.Clamp01(combustible);
            }
        }
        else
        {
            puedeRecargar = false;
            temporizadorRecarga = 0f;
        }
    }

    void FixedUpdate()
    {
        if (!detectorSuelo.Grounded &&
            Keyboard.current.spaceKey.isPressed &&
            combustible > 0f)
        {
            rb.AddForce(Vector3.up * fuerzaJetpack, ForceMode.Acceleration);

            combustible -= Time.fixedDeltaTime / duracionCombustible;
            combustible = Mathf.Clamp01(combustible);

            if (combustible <= 0f)
                agotado = true;
        }
    }
}
