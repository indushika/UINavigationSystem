# UIManager
Easy to use and highly customizable UI Navigation Management for Unity 
• This architecture makes use of Addressables, Scriptable Objects, DI (VContainer), Events (MessagePipe) and Unitask (MessagePipe) 
• Call UIManager from any class (given it's Injected to your class) to Create/Navigate to UI. 
• There are three main components; UIManager, MFUIConfig and MFUIPanels
  • UIManager; provides the API for navigation and creation. 
  • MFUIConfig; a scriptable object class, extend this to create all your configurations (i.e. Main Menu UI, create a MainMenuConfig, extending "MFUIConfig", all the data for your UI will be stored here) 
  • MFUIPanel; Base class for all UIPanels contains the logic for Open, Close, Initialize, etc. Extend this class to attach to your UIObjects (Canvas, Panels, etc). Set the create and set the specific UI Config related to this UI Type. 
