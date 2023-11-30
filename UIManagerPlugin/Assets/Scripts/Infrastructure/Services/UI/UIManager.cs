using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using MessagePipe;
using MonsterFactory.Events;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer;
using VContainer.Unity;

namespace MonsterFactory.Services.UIManagement
{
    public interface IUIManager 
    {
        public void CreateUIPanel(MFUIConfig uiConfig);
        public void NavigateToPreviousUIPanel(UILayer uiLayer); 
    }
    public class UIManager : IUIManager
    {
        private Dictionary<UILayer, List<MFUIPanel>> activeUIPanels;

        #region Publishers
        private IPublisher<UICreatedEvent> uiCreatedEventPublisher;
        private IPublisher<UIClosedEvent> uiClosedEventPublisher; 
        private IPublisher<UIDestroyedEvent> uiDestroyedEventPublisher;
        #endregion
        
        private IObjectResolver objectResolver;

        [Inject]
        public UIManager(IPublisher<UICreatedEvent> uiCreatedEventPublisher,
            IPublisher<UIClosedEvent> uiClosedEventPublisher,
            IPublisher<UIDestroyedEvent> uiDestroyedEventPublisher,
            IObjectResolver objectResolver)
        {
            this.uiCreatedEventPublisher = uiCreatedEventPublisher;
            this.uiClosedEventPublisher = uiClosedEventPublisher;
            this.uiDestroyedEventPublisher = uiDestroyedEventPublisher;
            this.objectResolver = objectResolver;
            activeUIPanels = new Dictionary<UILayer, List<MFUIPanel>>();
        }
        
        #region API

        public async void CreateUIPanel(MFUIConfig uiConfig)
        {
            GetOrCreateUIPanelListForUILayer(uiConfig.uiLayer, out List<MFUIPanel> uiPanelList);

            ResolvePrerequisiteConditions(uiConfig, uiPanelList);
            
            var result = await uiConfig.uiPanelAssetReference.LoadAssetAsync<GameObject>().ToUniTask();
            try
            {
                MFUIPanel uiPanel = result.GetComponent<MFUIPanel>(); 
                uiPanel.Initialize(uiConfig);
                uiPanelList.Add(uiPanel);
                activeUIPanels[uiConfig.uiLayer] = uiPanelList;
                    
                objectResolver.Instantiate(result);
                uiCreatedEventPublisher.Publish(new UICreatedEvent(uiConfig));
            }
            catch (Exception e)
            {
                Console.WriteLine("UI Panel Asset not loaded. " + e);
                throw;
            }
            
        }


        public void NavigateToPreviousUIPanel(UILayer uiLayer)
        {
            GetOrCreateUIPanelListForUILayer(uiLayer, out List<MFUIPanel> uiPanelsOfUILayer);
            HideActiveUIPanel(uiPanelsOfUILayer);
            if (uiPanelsOfUILayer.Count >0)
            {
                uiPanelsOfUILayer.Last().Open();
            }
        }

        #endregion

        #region Implementation

        private void HideActiveUIPanel(List<MFUIPanel> uiPanels)
        {
            if (uiPanels.Count > 0)
            {
                MFUIPanel currentUIPanel = uiPanels.Last();
                uiPanels.Remove(currentUIPanel);
                currentUIPanel.Close();
                uiClosedEventPublisher.Publish(new UIClosedEvent(currentUIPanel.UIConfig));
            }
        }

        private void RemoveActiveUIPanels(List<MFUIPanel> uiPanels)
        {
            int count = uiPanels.Count;
            if (count > 0)
            {
                for (int i = count - 1; i >= 0; i--)
                {
                    var uiPanel = uiPanels[i];
                    uiPanels.RemoveAt(i);
                    uiPanel.Destroy();
                }
                uiDestroyedEventPublisher.Publish(new UIDestroyedEvent());
            }
            
        }

        private void GetOrCreateUIPanelListForUILayer(UILayer uiLayer, out List<MFUIPanel> uiPanels)
        {
            if (activeUIPanels.TryGetValue(uiLayer, out uiPanels))
            {
                if (uiPanels == null)
                {
                    uiPanels = new List<MFUIPanel>();
                }
            }
            else
            {
                uiPanels = new List<MFUIPanel>();
                activeUIPanels.Add(uiLayer, uiPanels);
            }
        }
        
        private void ResolvePrerequisiteConditions(MFUIConfig uiConfig, List<MFUIPanel> uiPanelList)
        {
            if (uiConfig.removeAllUIPanels)
            {
                RemoveActiveUIPanels(uiPanelList);
            }
            else if (uiConfig.hidePreviousUIPanel)
            {
                HideActiveUIPanel(uiPanelList);
            }
        }

        #endregion

    }

    public enum UILayer
    {
        Popup = 0,
        Menu = 1,
        Gameplay = 2
    }
}

