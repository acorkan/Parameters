using ParameterViews.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ParameterViews.UserControls
{
    /// <summary>
    /// Interaction logic for ParamGridViewCtrl.xaml
    /// </summary>
    public partial class ParamGridViewCtrl : UserControl
    {
        public ParamGridViewCtrl()
        {
            InitializeComponent();
        }
    }

    internal class RoleTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate ret = null;
            string itemName = item.GetType().Name;
            Trace.WriteLine($"Item: {itemName}");
            var parentListView = GetAncestorOfType<ListView>(container as FrameworkElement);
            var resources = parentListView.Resources;
            foreach (var item1 in resources.Keys)
            {
                string s = item1.ToString();
                Trace.WriteLine($"Res Item: {s}");
                if (s.Contains(itemName))
                {
                    ret = resources[item1] as DataTemplate;
                }
            }
            return ret;
        }

        public T GetAncestorOfType<T>(FrameworkElement child) where T : FrameworkElement
        {
            var parent = VisualTreeHelper.GetParent(child);
            if (parent != null && !(parent is T))
                return (T)GetAncestorOfType<T>((FrameworkElement)parent);
            return (T)parent;
        }
    }
}
