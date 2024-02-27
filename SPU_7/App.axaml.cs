using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Prism.DryIoc;
using Prism.Ioc;
using SPU_7.ViewModels;
using SPU_7.Views;

namespace SPU_7;

public partial class App : PrismApplication
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        base.Initialize();
    }

    protected override AvaloniaObject CreateShell()
    {
        return Container.Resolve<MainWindowView>();
    }
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        
    }
    
    protected override void OnInitialized()
    {
            
    }
}