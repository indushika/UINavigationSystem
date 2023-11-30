using System.Collections;
using System.Collections.Generic;
using MonsterFactory.Services.UIManagement;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MFUIPanel
{
    [SerializeField] private Text testText;
    private MainMenuConfig mainMenuConfig; 
    
    public override void OnInitialized()
    {
        if (UIConfig is MainMenuConfig _mainMenuConfig)
        {
            mainMenuConfig = _mainMenuConfig; 
        }
        else
        {
            Debug.LogError("Incorrect configuration. Configuration should be of type: " + nameof(MainMenuConfig));
        }
        PopulateData();
    }
    
    private void PopulateData()
    {
        testText.text = mainMenuConfig.testTextData; 
    }

}
