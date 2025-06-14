using UnityEngine;

public class NewPlayerTeleporter : MonoBehaviour
{
    public Transform TeleportZoneObject;
    public SpawnAnomaly anomalySpawner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 localOffset = transform.InverseTransformPoint(other.transform.position);
            Quaternion relativeRotation = TeleportZoneObject.rotation * Quaternion.Inverse(transform.rotation);

            CharacterController cc = other.GetComponent<CharacterController>();
            if (cc != null)
            {
                cc.enabled = false;
                other.transform.position = TeleportZoneObject.TransformPoint(localOffset);
                other.transform.rotation = relativeRotation * other.transform.rotation;
                cc.enabled = true;
            }

            if (GameManager.Instance.anomalyActive)
            {
                // If anomaly active, reset floor to 9 when forward triggered
                GameManager.Instance.OnForwardTriggerWithAnomaly();
            }
            else
            {
                // Normal forward trigger behaviour (go next floor)
                GameManager.Instance.OnForwardTrigger();
            }
        }
    }

}
