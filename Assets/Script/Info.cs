using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Info : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject InfoClass;
    public TextMeshProUGUI classInfo;

    public GameObject ItemClass;
    public TextMeshProUGUI itemInfo;

    public string classText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (classInfo != null)
        {
            InfoClass.SetActive(true);
            classInfo.text = classText;
        }
        if (itemInfo != null)
        {
            ItemClass.SetActive(true);
            itemInfo.text = classText;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (classInfo != null)
        {
            InfoClass.SetActive(false);
        }
        if (itemInfo != null)
        {
            ItemClass.SetActive(false);
        }
    }
    public void StatDisplayOFF()
    {
        ItemClass.SetActive(false);
    }
}
