using UnityEditor;
using UnityEngine;

public class turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationpoint;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 5f;

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        //Handles.DrawWireDisc(transform);
    }
}
