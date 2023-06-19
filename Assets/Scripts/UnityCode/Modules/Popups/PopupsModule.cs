using Build1.PostMVC.Core.Modules;
using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Core.MVCS.Injection;

namespace Modules.Popups
{
    public sealed class PopupsModule : Module
    {
        [Inject] public ICommandBinder CommandBinder { get; set; }
        
        [PostConstruct]
        public void PostConstruct()
        {
        }
    }
}