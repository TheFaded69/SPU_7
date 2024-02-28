using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SPU_7.Views.ScriptViews.OperationResultViews;

public partial class ManualValidationResultView : UserControl
{
    public ManualValidationResultView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}