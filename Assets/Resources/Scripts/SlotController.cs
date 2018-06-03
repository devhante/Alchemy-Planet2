using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotController : MonoBehaviour
{
    int slotSize;

    GameObject[] slots;
    GameObject[] stars;
    Dictionary<string, int> materialNumbers;

    public Sprite[] materials;

    private void Awake()
    {
        // slot 크기 초기화
        slotSize = 4;

        // slot 배열,star 배열 할당
        slots = new GameObject[slotSize];
        stars = new GameObject[slotSize];
        materialNumbers = new Dictionary<string, int>();
        
        // 배열에 오브젝트 대입
        for (int i = 0; i < slotSize; i++)
        {
            slots[i] = transform.GetChild(i).gameObject;
            
            stars[i] = slots[i].transform.GetChild(0).gameObject;
            stars[i].SetActive(false);
        }

        // Slot의 Sprite 초기화
        SetSlotSprite();
    }

    private void SetSlotSprite()
    {
        int index;
        string name;

        for(int i = 0; i < slotSize; i++)
        {
            index = Random.Range(0, materials.Length);
            name = materials[index].name;
            if (materialNumbers.ContainsKey(name) == false) materialNumbers.Add(name, 0);

            if(materialNumbers[name] < MaterialSpawner.Instance.GetmaterialNumber(name))
            {
                slots[i].GetComponent<SpriteRenderer>().sprite = materials[index];
                materialNumbers[name]++;
            }
        }
    }
}