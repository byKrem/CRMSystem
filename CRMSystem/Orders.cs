//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CRMSystem
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public partial class Orders : INotifyPropertyChanged
    {
        private int _id;
        private string _description;
        private System.DateTime _creationDate;
        private int _orderStatusId;
        private int _userId;
        private string _invoiceNumber;

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }
        public System.DateTime CreationDate
        {
            get { return _creationDate; }
            set
            {
                _creationDate = value;
                OnPropertyChanged();
            }
        }
        public int OrderStatusId
        {
            get { return _orderStatusId; }
            set
            {
                _orderStatusId = value;
                OnPropertyChanged();
            }
        }
        public int UserId
        {
            get { return _userId; }
            set
            {
                _userId = value;
                OnPropertyChanged();
            }
        }
        public string InvoiceNumber
        {
            get { return _invoiceNumber; }
            set
            {
                _invoiceNumber = value;
                OnPropertyChanged();
            }
        }

        public Orders()
        {
            this.ProductOrder = new HashSet<ProductOrder>();
        }

        private OrderStatus _orderStatus;
        public virtual OrderStatus OrderStatus
        {
            get { return _orderStatus; }
            set
            {
                _orderStatus = value;
                OnPropertyChanged();
            }
        }
        private ICollection<ProductOrder> _productOrder;
        public virtual ICollection<ProductOrder> ProductOrder
        {
            get { return _productOrder; }
            set
            {
                _productOrder = value;
                OnPropertyChanged();
            }
        }
        private Users _users;
        public virtual Users Users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}