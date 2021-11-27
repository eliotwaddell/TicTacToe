using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlankTile : MonoBehaviour
{
    Color default_color = Color.white;
    Color hover_color; //= Color.grey;

    Renderer renderer;

    public bool clicked = false;

    bool activated = false;

    bool clickable = false;

    void Start()
    {
        hover_color = new Color(0.9f, 0.9f, 0.9f);
        renderer = GetComponent<Renderer>();
        renderer.material.color = default_color;
    }

    // Update is called once per frame
    void OnMouseOver()
    {
        if(activated)
        {
            renderer.material.color = hover_color;
        }
    }

    void OnMouseExit()
    {
        renderer.material.color = default_color;
    }

    void OnMouseDown()
    {
        if(clickable)
        {
            clicked = true;
        }
    }

    void OnMouseUp()
    {
        clicked = false;
    }

    public void Activate()
    {
        activated = true;
        clickable = true;
    }

    public void Deactivate()
    {
        activated = false;
        clickable = false;
    }
}
