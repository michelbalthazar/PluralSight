using MVVMHookupDemo.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Zza.Data;

namespace MVVMHookupDemo.Customers
{
    public class CustomerListViewModel
    {
        private ICustomersRepository _repository = new CustomersRepository();
        
        public CustomerListViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(
                new System.Windows.DependencyObject())) return;

            Customers = new ObservableCollection<Customer>( _repository.GetCustomersAsync().Result);
        }

        public ObservableCollection<Customer> Customers { get; set; }
    }
}