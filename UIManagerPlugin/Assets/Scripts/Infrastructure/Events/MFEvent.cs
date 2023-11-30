using MessagePipe;
using MonsterFactory.Services.UIManagement;
using VContainer;

namespace MonsterFactory.Events
{
    /// <summary>
    /// Event Registration helper is just an easy way for us to keep track of events for each lifetime scope
    /// Create a new method for each lifetime scope you create.
    /// </summary>
    public static partial class EventRegistrationHelper
    {
        public static void RegisterGlobalEventClasses(IContainerBuilder builder, MessagePipeOptions options)
        {
            EventRegistrationHelper.builder = builder;
            EventRegistrationHelper.options = options;

            //Register Event types here
            RegisterEvent<TestEvent>();



            #region UIEvents

            RegisterEvent<UICreatedEvent>();
            RegisterEvent<UIClosedEvent>();
            RegisterEvent<UIDestroyedEvent>();
            
            #endregion


            EventRegistrationHelper.builder = null;
            EventRegistrationHelper.options = null;
        }
    }
}

namespace MonsterFactory.Events
{
    public class MFBaseEvent
    {
    }


    public class TestEvent : MFBaseEvent
    {
    }
    

    #region UIManagement

    public class UIBaseEvent : MFBaseEvent
    {
        public readonly MFUIConfig UIConfig;

        public UIBaseEvent(MFUIConfig uiConfig)
        {
            UIConfig = uiConfig; 
        }
    }

    public class UICreatedEvent : UIBaseEvent
    {
        public UICreatedEvent(MFUIConfig uiConfig) : base(uiConfig)
        {
        }
    }

    public class UIClosedEvent : UIBaseEvent
    {
        public UIClosedEvent(MFUIConfig uiConfig) : base(uiConfig)
        {
        }
    }
    public class UIDestroyedEvent : MFBaseEvent
    {
        
    }

    #endregion
}