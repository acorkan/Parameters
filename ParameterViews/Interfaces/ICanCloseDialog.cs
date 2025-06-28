namespace ParameterViews.Interfaces
{
    /// <summary>
    /// Implement this in VM and set in View Loaded override.
    /// 
    /// Implemenbt like this:
    ///  
    /// private void Window_Loaded(object sender, RoutedEventArgs e)
    /// {
    ///     if(DataContext is ICanCloseDialog vm)
    ///     {
    ///         vm.Close += () =>
    ///         {
    ///             Close();
    ///         };
    ///         Closing += (s, e) =>
    ///         {
    ///             e.Cancel = !vm.CanClose;
    ///         };
    ///     }
    /// }
    /// 
    /// </summary>
    public interface ICanCloseDialog
    {
        /// <summary>
        /// A delegate that will close the window.
        /// This is set in the dialogs Loaded event.
        /// </summary>
        Action CloseDialog { set; get; }
        /// <summary>
        /// The Dialog will test this in its Closing event.
        /// </summary>
        bool CanCloseDialog { get; }
    }
}
