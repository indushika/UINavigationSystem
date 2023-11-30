 using System.Collections;
using System.Collections.Generic;
 using MonsterFactory.Services.UIManagement;
 using UnityEngine;

 namespace MonsterFactory.Services.UIManagement
 {
     public abstract class MFUIPanel : MonoBehaviour
     {
         public MFUIConfig UIConfig => uiConfig;

         private MFUIConfig uiConfig;
         public abstract void OnInitialized(); 
         public void Initialize(MFUIConfig uiConfig) 
         {
             this.uiConfig = uiConfig; 
             OnInitialized();
         }
         
         public void Open()
         {
             Show();
         }

         public void Close()
         {
             Hide();
         }

         public void Destroy()
         {
             Destroy(gameObject);
         }

         private void Show()
         {
             gameObject.SetActive(true);
         }

         private void Hide()
         {
             gameObject.SetActive(false);
         }
     }
 }
