using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private GameObject creditPopup;

    private void Start()
    {
        InitUI();
    }

    private void InitUI()
    {
        HideCreditPopup();
    }

    public void StartButtonClick()
    {
        SceneManager.LoadScene("Main");
    }

    public void ShowCreditPopup()
    {
        creditPopup.SetActive(true);
    }
    
    public void HideCreditPopup()
    {
        creditPopup.SetActive(false);
    }
}
