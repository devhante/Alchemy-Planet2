using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Planet : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        anim.SetBool("Swell", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        anim.SetBool("Swell", false);
    }
}
