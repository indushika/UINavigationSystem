using System;
using Cysharp.Threading.Tasks;
using MessagePipe;
using MonsterFactory.Services.UIManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;

namespace MonsterFactory.Services
{
    public class GameInitializer : IInitializable
    {
        [Inject] private IUIManager uiManager; 


        public void Initialize()
        {
            InitializeUI();
        }

        private async void InitializeUI()
        {
            string key = MFUIConfigKeyMapper.MainMenuConfigKey;
            MFUIConfig uiConfig = await Addressables.LoadAssetAsync<MFUIConfig>(key).ToUniTask();
            try
            {
                uiManager.CreateUIPanel(uiConfig);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}