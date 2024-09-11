using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Vector3 offset;
    private float zCoord = 0;
    private SpriteRenderer spriteRenderer;
    private static int sortingOrder = 1;
    private bool isDragging = false;

    [SerializeField] private Texture2D cursorTextureHover;
    [SerializeField] private Texture2D cursorTextureClic;

    // Variables de configuration pour le curseur
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    void Start()
    {
        isDragging = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Initialisation de l'ordre de tri (sorting order) pour gérer la superposition des objets
        spriteRenderer.sortingOrder = sortingOrder;
    }

    void OnMouseEnter()
    {
        if (!isDragging) 
        {
            // Changement du curseur quand la souris survole l'objet
            Cursor.SetCursor(cursorTextureHover, hotSpot, cursorMode);
        }
    }

    void OnMouseExit()
    {
        if (!isDragging) 
        {
            // Réinitialisation du curseur quand la souris quitte l'objet
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
        }
    }

    void OnMouseDown()
    {
        // Changement du curseur lors du clic
        Cursor.SetCursor(cursorTextureClic, hotSpot, cursorMode);

        // Récupération de la coordonnée Z et calcul de l'offset pour maintenir la position de l'objet par rapport à la souris
        zCoord = Camera.main.WorldToScreenPoint(transform.position).z;
        offset = transform.position - GetMouseWorldPos();

        // Augmentation de l'ordre de tri pour mettre l'objet au-dessus des autres quand il est sélectionné
        spriteRenderer.sortingOrder = sortingOrder++;

        isDragging = true;
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord; // On garde la coordonnée Z originale pour éviter des mouvements sur cet axe
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + offset;
    }

    void OnMouseUp()
    {
        // Fin du drag, réinitialisation du curseur
        isDragging = false;
        Cursor.SetCursor(cursorTextureHover, Vector2.zero, cursorMode);
    }
}
