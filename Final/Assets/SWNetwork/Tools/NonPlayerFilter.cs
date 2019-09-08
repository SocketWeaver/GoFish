using System.Collections;
using System.Collections.Generic;
using SWNetwork;
using UnityEngine;

public class NonPlayerFilter : MonoBehaviour
{
    public enum FilterType
    {
        Circle2D,
        Sphere
    }

    public FilterType filterType;

    public float Range = 1;

    CircleCollider2D circleCollider2D;
    SphereCollider sphereCollider;

    void Start()
    {
        if (filterType == FilterType.Circle2D)
        {
            circleCollider2D = gameObject.AddComponent<CircleCollider2D>();
            circleCollider2D.isTrigger = true;
            circleCollider2D.radius = Range;
        }
        else if (filterType == FilterType.Sphere)
        {
            sphereCollider = gameObject.AddComponent<SphereCollider>();
            sphereCollider.isTrigger = true;
            sphereCollider.radius = Range;
        }
    }

    private void Update()
    {
        if (filterType == FilterType.Circle2D)
        {
            circleCollider2D.radius = Range;
        }
        else if (filterType == FilterType.Sphere)
        {
            sphereCollider.radius = Range;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject otherGameObject = collision.gameObject;
        UpdateFilterVisiblePlayers(otherGameObject, false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject otherGameObject = collision.gameObject;
        UpdateFilterVisiblePlayers(otherGameObject, true);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject otherGameObject = other.gameObject;
        UpdateFilterVisiblePlayers(otherGameObject, true);
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject otherGameObject = other.gameObject;
        UpdateFilterVisiblePlayers(otherGameObject, false);
    }

    private void UpdateFilterVisiblePlayers(GameObject otherGameObject, bool visible)
    {
        NonPlayerFilterable filterable = otherGameObject.GetComponent<NonPlayerFilterable>();
        if (filterable != null)
        {
            if (visible)
            {
                filterable.AddVisiblePlayer(gameObject);
            }
            else
            {
                filterable.RemoveVisiblePlayer(gameObject);
            }
        }
    }
}
