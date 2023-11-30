using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace MonsterFactory.Services.UIManagement
{
    public class MFUIConfig : ScriptableObject
    {
        public AssetReference uiPanelAssetReference;
        public UILayer uiLayer;

        public bool hidePreviousUIPanel;

        public bool removeAllUIPanels; 
    }
}

