using UnityEditor;
using UnityEngine;

public class LevelHelper : MonoBehaviour
{
    [SerializeField] public Transform playerSpawnPoint;
    [SerializeField] public Collider extractionPoint;

    private void OnDrawGizmosSelected()
    {
        if (playerSpawnPoint != null)
        {
            // Draw two red spheres to show where the player capsule will be spawned
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(playerSpawnPoint.position + Vector3.up * -0.5f, 0.5f);
            Gizmos.DrawWireSphere(playerSpawnPoint.position + Vector3.up * 0.5f, 0.5f);
            // Draw arrow from upper sphere to show capsule orientation
            Gizmos.DrawLine(playerSpawnPoint.position + Vector3.up * 0.5f, playerSpawnPoint.position + Vector3.up * 0.5f + playerSpawnPoint.forward);
            Vector3 arrowEnd = playerSpawnPoint.position + Vector3.up * 0.5f + playerSpawnPoint.forward;
            Vector3 arrowRight = Quaternion.LookRotation(playerSpawnPoint.forward) * Quaternion.Euler(0, 180 + 30, 0) * Vector3.forward * 0.2f;
            Vector3 arrowLeft = Quaternion.LookRotation(playerSpawnPoint.forward) * Quaternion.Euler(0, 180 - 30, 0) * Vector3.forward * 0.2f;
            Gizmos.DrawRay(arrowEnd, arrowRight);
            Gizmos.DrawRay(arrowEnd, arrowLeft);}
    }
}
