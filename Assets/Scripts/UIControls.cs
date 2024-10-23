using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControls : MonoBehaviour
{
    [SerializeField] GameObject androidUI;

    private void Start()
    {
#if UNITY_ANDROID
        androidUI.SetActive(true);
        
#endif
    }
}
