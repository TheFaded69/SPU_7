using System;
using Avalonia.Media.Imaging;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Prism.Services.Dialogs;
using SPU_7.Views;

namespace SPU_7.ViewModels;

public class PicturePreviewViewModel : ViewModelBase, IDialogAware
{
    public PicturePreviewViewModel()
    {
        
    }

    private Bitmap _picture;
    
    public Bitmap Picture
    {
        get => _picture;
        set => SetProperty(ref _picture, value);
    }

    public bool CanCloseDialog()
        => true;

    public void OnDialogClosed()
    {

    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        Picture = parameters.GetValue<Bitmap>("Picture");
    }

    public event Action<IDialogResult>? RequestClose;

    public static void Show(IDialogService dialogService, Bitmap picture, Action positiveAction, Action negativeAction)
    {
        dialogService.ShowDialog(nameof(PicturePreviewView), new DialogParameters {{"Picture", picture}}, result => {
            switch (result.Result)
            {
                case ButtonResult.Abort:
                    break;
                case ButtonResult.Cancel:
                    break;
                case ButtonResult.Ignore:
                    break;
                case ButtonResult.No:
                    negativeAction?.Invoke();
                    break;
                case ButtonResult.None:
                    break;
                case ButtonResult.OK:
                    positiveAction?.Invoke();
                    break;
                case ButtonResult.Retry:
                    break;
                case ButtonResult.Yes:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        });
    }
}