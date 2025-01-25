using TMPro;
using UnityEngine;

public class SpeedDebug : MonoBehaviour
{
    public Rigidbody objectRigidbody;
    public TextMeshProUGUI speedText;

    void Update()
    {
        // Calculate speed as the magnitude of the velocity.
        float speed = objectRigidbody.linearVelocity.magnitude;

        // Update the UI text.
        speedText.text = $"Speed: {speed:F2}";
    }
}
