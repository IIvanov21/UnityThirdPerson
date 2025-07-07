using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class UIControls : MonoBehaviour
{
    [SerializeField] GameObject androidUI;
    [SerializeField] Button serverBtn, hostBtn, clientBtn;

    private void Start()
    {
#if UNITY_ANDROID || UNITY_EDITOR_WIN
        androidUI.SetActive(true);
#endif
        serverBtn.onClick.AddListener(() => { NetworkManager.Singleton.StartServer(); });
        hostBtn.onClick.AddListener(() => { NetworkManager.Singleton.StartHost(); });
        clientBtn.onClick.AddListener(() => { NetworkManager.Singleton.StartClient(); });

    }


}
