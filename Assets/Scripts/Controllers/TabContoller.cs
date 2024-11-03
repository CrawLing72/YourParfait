using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabContoller : MonoBehaviour
{
    public Selectable[] inputFields;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GameObject current = EventSystem.current.currentSelectedGameObject;
            for (int i = 0; i < inputFields.Length; i++)
            {
                if (inputFields[i].gameObject == current)
                {
                    int nextIndex = (i + 1) % inputFields.Length;
                    inputFields[nextIndex].Select();
                    break;
                }
            }
        }
    }
}
