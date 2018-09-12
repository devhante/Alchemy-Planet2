using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageLights : MonoBehaviour
{
    public GameObject pageLightPrefab;
    public Sprite pageLightOffSprite;
    public Sprite pageLightOnSprite;
    public Vector2 offset;
    public int number;

    private List<Image> pageLights = new List<Image>();
    private int currentPageNumber = 1;

    private void Awake()
    {
        CreatePageLights();
        pageLights[0].sprite = pageLightOnSprite;
    }

    private void CreatePageLights()
    {
        for(int i = 0; i < number; i++)
        {
            Vector3 position = transform.position + new Vector3(offset.x * i, offset.y * i);
            pageLights.Add(Instantiate(pageLightPrefab, position, Quaternion.identity, transform).GetComponent<Image>());
        }
    }

    public void ChangeLightToLeft()
    {
        pageLights[currentPageNumber - 1].sprite = pageLightOffSprite;

        if (currentPageNumber == 1)
            currentPageNumber = number;
        else
            currentPageNumber--;

        pageLights[currentPageNumber - 1].sprite = pageLightOnSprite;
    }

    public void ChangeLightToRight()
    {
        pageLights[currentPageNumber - 1].sprite = pageLightOffSprite;

        if (currentPageNumber == number)
            currentPageNumber = 1;
        else
            currentPageNumber++;

        pageLights[currentPageNumber - 1].sprite = pageLightOnSprite;
    }
}
