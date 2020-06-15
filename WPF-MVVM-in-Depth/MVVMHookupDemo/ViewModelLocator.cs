using System;
using System.ComponentModel;
using System.Windows;

namespace MVVMHookupDemo
{
    public class ViewModelLocator
    {
        public static bool GetAutoWriteViewModel(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoWriteViewModelProperty);
        }

        public static void SetAutoWriteViewModel(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoWriteViewModelProperty, value);
        }

        // Using a DependencyProperty as the backing store for AutoWriteViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AutoWriteViewModelProperty =
            DependencyProperty.RegisterAttached("AutoWriteViewModel", typeof(bool),
                typeof(ViewModelLocator), new PropertyMetadata(false, AutoWireViewModelChanged));

        private static void AutoWireViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;

            var viewTypeName = d.GetType().FullName;
            var viewModelTypeNAme = viewTypeName + "Model";
            var viewModelType = Type.GetType(viewModelTypeNAme);
            var viewModel = Activator.CreateInstance(viewModelType);
            ((FrameworkElement)d).DataContext = viewModel;
        }
    }
}