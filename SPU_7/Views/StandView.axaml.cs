using Avalonia;
using Avalonia.Controls;
using Prism.Ioc;
using SPU_7.Models.Services.ContentServices;

namespace SPU_7.Views
{
    public partial class StandView : UserControl
    {
        public StandView()
        {
            InitializeComponent();
        }
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);

            // Initialize the WindowNotificationManager with the "TopLevel". Previously (v0.10), MainWindow
            var notifyService = ContainerLocator.Current.Resolve<INotificationService>();
            notifyService.SetHostWindow(TopLevel.GetTopLevel(this));
        }
    }
}
