using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Strzelecki_Baranowski.DuckApp.INTERFACES;

namespace Strzelecki_Baranowski.DuckApp.UI
{

    

    public class DuckViewModel : INotifyPropertyChanged
    {

        private IDuck _duck;
        private readonly ProducerViewModel? _producerVM;
        public event PropertyChangedEventHandler? PropertyChanged;


        public DuckViewModel(IDuck duck, ProducerViewModel? producer = null)
        {
            _duck = duck;
            _producerVM = producer;

            //if (_producerVM != null)
            //{
            //    _producerVM.PropertyChanged += (s, e) =>
            //{
            //    if (e.PropertyName == nameof(ProducerViewModel.Name))
            //    {
            //        OnPropertyChanged(nameof(ProducerName));
            //    }
            //};
            //}


        }


        public string Name
        {
            get { return _duck.Name; }
            set
            {
                if (_duck.Name != value && _duck.Name != null)
                {
                    _duck.Name = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                        //PropertyChanged(this, new PropertyChangedEventArgs("DisplayText"));
                    }
                }
            }
        }

        public string Description => _duck.Description;

        public int ID => _duck.ID;
        public int ProducerID => _duck.ProducerID;

        //public string ProducerName => _producerVM?.Name ?? "Unknown";
        public string ProducerName
        {
            get => _producerVM?.Name ?? "Unknown";
            set
            {
                if (_producerVM != null && _producerVM.Name != value)
                {
                    _producerVM.Name = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("ProducerName"));
                        //PropertyChanged(this, new PropertyChangedEventArgs("DisplayText"));
                    }
                }
            }
        }


        public double Price => _duck.Price;

        public string Photo => _duck.Photo;



        //TODO dokończyć


        //protected void OnPropertyChanged(string propertyName) =>
        //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    



}
}
