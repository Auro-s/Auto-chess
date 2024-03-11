using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClassInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject InfoClass;
    public TextMeshProUGUI classInfo;
    public string classText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (classInfo != null)
        {
            InfoClass.SetActive(true);
            classInfo.text = classText;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (classInfo != null)
        {
            InfoClass.SetActive(false);
        }
    }
}
