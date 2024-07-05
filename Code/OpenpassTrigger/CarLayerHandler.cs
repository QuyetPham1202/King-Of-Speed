using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLayerHandler : MonoBehaviour
{

    public SpriteRenderer carOutLineSpriteRenderer;
    List<SpriteRenderer> defaultLayerSpriteRenderers = new List<SpriteRenderer>();
    List<Collider2D> overpassColliderList = new List<Collider2D>();
    List<Collider2D> underpassColliderList = new List<Collider2D>();

    Collider2D carConllider;
    bool isDrivingOnOverPass = false;
    private void Awake()
    {
        foreach (SpriteRenderer spriteRenderer in gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            if (spriteRenderer.sortingLayerName == "Default")
                defaultLayerSpriteRenderers.Add(spriteRenderer);
        }
        foreach (GameObject overpassColliderGameobject in GameObject.FindGameObjectsWithTag("OverpassCollider"))
        {

            overpassColliderList.Add(overpassColliderGameobject.GetComponent<Collider2D>());
        }
        foreach (GameObject overpassColliderGameobject in GameObject.FindGameObjectsWithTag("UnderpassCollider"))
        {

            underpassColliderList.Add(overpassColliderGameobject.GetComponent<Collider2D>());
        }
       carConllider = GetComponentInChildren<Collider2D>();

        carConllider.gameObject.layer = LayerMask.NameToLayer("ObjectUnderPass");
    }
    void Start()
    {
        UpdateSortingAndCollisionLayers();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateSortingAndCollisionLayers()
    {
        if (isDrivingOnOverPass)
        {
            SetSortingLayer("RaceTrackDvepass");
            carOutLineSpriteRenderer.enabled = false;

        }
        else
        {
            SetSortingLayer("Default");
            carOutLineSpriteRenderer.enabled = true;
        }

        SetCollisionWithOverPass();
    }

  void SetCollisionWithOverPass()
    {
        foreach ( Collider2D collider2D in overpassColliderList)
        {
            Physics2D.IgnoreCollision(carConllider, collider2D, !isDrivingOnOverPass);
        }
        foreach (Collider2D collider2D in underpassColliderList)
        {
            if (isDrivingOnOverPass)
                Physics2D.IgnoreCollision(carConllider, collider2D, true);
            else
                Physics2D.IgnoreCollision(carConllider, collider2D, false);
        }
    }
    void SetSortingLayer(string laerName)
    {
        foreach (SpriteRenderer spriteRenderer in defaultLayerSpriteRenderers)
        {
            spriteRenderer.sortingLayerName = laerName;
        }
    }
    public bool IsDrivingOnOverpass()
    {
        return isDrivingOnOverPass;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("UnderpassTrigger"))
        {
    
            isDrivingOnOverPass = false;
            carConllider.gameObject.layer = LayerMask.NameToLayer("ObjectUnderPass");

            UpdateSortingAndCollisionLayers();
        }
        else if (collision.CompareTag("OverpassTrigger"))
        {
            
            isDrivingOnOverPass |= true;
            carConllider.gameObject.layer = LayerMask.NameToLayer("ObjectOverPass");

            UpdateSortingAndCollisionLayers();
        }
    }
}
