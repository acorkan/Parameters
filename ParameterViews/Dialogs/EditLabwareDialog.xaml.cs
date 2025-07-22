using System.Windows;

namespace ParameterViews.Dialogs
{
    /// <summary>
    /// Interaction logic for ParamEditDialog.xaml
    /// </summary>
    public partial class EditLabwareDialog : Window
    {
        public EditLabwareDialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(DataContext is ParameterViews.Interfaces.ICanCloseDialog vm)
            {
                vm.CloseDialog += () =>
                {
                    Close();
                };
                Closing += (s, e) =>
                {
                    e.Cancel = !vm.CanCloseDialog;
                };
            }
        }
    }
}
