using System;
using System.Collections.Generic;
using UnityEngine;

public class ScreenUIManager : MonoBehaviour
{
    [SerializeField]private Canvas[] uiArray;
    [SerializeField]private bool openStartingUI = true;
    [SerializeField]private Canvas startingUI;
    private void Start()
    {
        if(openStartingUI)
        {
            Open(startingUI);
        }
    }
    
    public void Open(Canvas ui)
    {
        if(ui == null)
        {
            return;
        }

        Debug.Log("Closing other UIs");
        for(int i = 0; i < uiArray.Length; i++)
        {
            Canvas currentUI = uiArray[i];
            currentUI.gameObject.SetActive(false);
        }
        
        ui.gameObject.SetActive(true);
    }


}
