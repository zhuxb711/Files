using Files.Uwp.Dialogs;
using Files.Shared.Enums;
using Files.Uwp.Filesystem;
using Files.Uwp.Helpers;
using Files.Uwp.ViewModels.Properties;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Files.Uwp.Views
{
    public sealed partial class PropertiesDetails : PropertiesTab
    {
        public PropertiesDetails()
        {
            InitializeComponent();
        }

        protected override void Properties_Loaded(object sender, RoutedEventArgs e)
        {
            base.Properties_Loaded(sender, e);

            if (BaseProperties is FileProperties fileProps)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                fileProps.GetSystemFileProperties();
                stopwatch.Stop();
                Debug.WriteLine(string.Format("System file properties were obtained in {0} milliseconds", stopwatch.ElapsedMilliseconds));
            }
        }

        // WINUI3
        private static ContentDialog SetContentDialogRoot(ContentDialog contentDialog)
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 8))
            {
                contentDialog.XamlRoot = App.Window.Content.XamlRoot;
            }
            return contentDialog;
        }

        public override async Task<bool> SaveChangesAsync(ListedItem item)
        {
            while (true)
            {
                using DynamicDialog dialog = DynamicDialogFactory.GetFor_PropertySaveErrorDialog();
                try
                {
                    if (BaseProperties is FileProperties fileProps)
                    {
                        await fileProps.SyncPropertyChangesAsync();
                    }
                    return true;
                }
                catch
                {
                    await SetContentDialogRoot(dialog).TryShowAsync();
                    switch (dialog.DynamicResult)
                    {
                        case DynamicDialogResult.Primary:
                            break;

                        case DynamicDialogResult.Secondary:
                            return true;

                        case DynamicDialogResult.Cancel:
                            return false;
                    }
                }
            }
        }

        private async void ClearPropertiesConfirmation_Click(object sender, RoutedEventArgs e)
        {
            ClearPropertiesFlyout.Hide();
            if (BaseProperties is FileProperties fileProps)
            {
                await fileProps.ClearPropertiesAsync();
            }
        }

        public override void Dispose()
        {
        }
    }
}