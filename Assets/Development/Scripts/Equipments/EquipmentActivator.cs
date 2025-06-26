using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles tracking of elements that are in contact with the equipment.
/// </summary>
public class EquipmentActivator : MonoBehaviour
{
    private HashSet<GameObject> collidingElements = new();
    private HashSet<GameObject> collidingConnectors = new();

    private HashSet<GameObject> collidingEquipments = new();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out ElementIntegrator _))
        {
            collidingElements.Add(collision.gameObject);
        }

        if (gameObject.TryGetComponent(out EquipmentAttribute equipmentAttribute) && equipmentAttribute.equipmentData.equipmentEffect == EquipmentEffect.ScaleReduction)
        {
            if (collision.gameObject.TryGetComponent(out ConnectorIntegrator _))
            {
                collidingConnectors.Add(collision.gameObject);
            }

            if (collision.gameObject.TryGetComponent(out EquipmentAttribute _))
            {
                collidingEquipments.Add(collision.gameObject);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // collidingElements.Remove(collision.gameObject);
        if (collision.gameObject.TryGetComponent(out ElementIntegrator _))
        {
            collidingElements.Remove(collision.gameObject);
        }

        if (gameObject.TryGetComponent(out EquipmentAttribute equipmentAttribute) && equipmentAttribute.equipmentData.equipmentEffect == EquipmentEffect.ScaleReduction)
        {
            if (collision.gameObject.TryGetComponent(out ConnectorIntegrator _))
            {
                collidingConnectors.Remove(collision.gameObject);
            }

            if (collision.gameObject.TryGetComponent(out EquipmentAttribute _))
            {
                collidingEquipments.Remove(collision.gameObject);
            }
        }
    }

    /// <summary>
    /// Provides a list of contacted elements.
    /// </summary>
    public IEnumerable<GameObject> GetCollidingElements()
    {
        return collidingElements;
    }

    public IEnumerable<GameObject> GetCollidingConnectors()
    {
        return collidingConnectors;
    }

    public IEnumerable<GameObject> GetCollidingEquipments()
    {
        return collidingEquipments;
    }

    /// <summary>
    /// Clears the list of contacted elements.
    /// </summary>
    public void ClearCollidingElements()
    {
        collidingElements.Clear();
    }

    public void ClearCollidingConnectors()
    {
        collidingConnectors.Clear();
    }

    public void ClearCollidingEquipments()
    {
        collidingEquipments.Clear();
    }
}
