using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CORE;
using Strzelecki_Baranowski.DuckApp.INTERFACES;

namespace Strzelecki_Baranowski.DuckApp.UI
{

    

    public partial class DuckViewModel : ObservableObject
    {

        private IDuck? _duck;
        private readonly ProducerViewModel? _producerVM;
        //public event PropertyChangedEventHandler? PropertyChanged;


        public DuckViewModel(IDuck? duck, ProducerViewModel? producer = null)
        {
            _duck = duck;
            _producerVM = producer;
            _producerName = _producerVM?.Name ?? "Brak producenta";



            _name = _duck?.Name ?? string.Empty;
            _iD = _duck?.ID ?? -1;
            _producerID = _duck?.ProducerID ?? 0;
            // _producerName = _producerVM.Name;
            _photo = _duck?.Photo ?? string.Empty;
            _price = _duck?.Price ?? 0.0;
            _description = _duck?.Description ?? string.Empty;


        }

        [ObservableProperty]
        private string _name;
        [ObservableProperty]
        private int _iD;
        [ObservableProperty]
        private int _producerID;
        [ObservableProperty]
        private string _producerName; 
        [ObservableProperty]
        private string _photo;
        [ObservableProperty]
        private double _price;
        [ObservableProperty]
        private string _description;
        [ObservableProperty]
        private Category _category;

        public DuckViewModel Clone()
        {
            return (DuckViewModel) this.MemberwiseClone();
        }

        public IDuck? GetDuck()
        {
            return _duck;
        }


        // Zakładając, że poniższy kod znajduje się wewnątrz klasy ViewModel,
        // która posiada pole _duck (model danych) i zdarzenie PropertyChanged.

        //public string Name
        //{
        //    get { return _duck.Name; }
        //    set
        //    {
        //        if (_duck.Name != value && _duck.Name != null)
        //        {
        //            _duck.Name = value;
        //            if (PropertyChanged != null)
        //            {
        //                PropertyChanged(this, new PropertyChangedEventArgs("Name"));
        //                //PropertyChanged(this, new PropertyChangedEventArgs("DisplayText"));
        //            }
        //        }
        //    }
        //}

        // === DODANY SETTER dla Description ===
        //public string Description
        //{
        //    get => _duck.Description;
        //    set
        //    {
        //        if (_duck.Description != value)
        //        {
        //            _duck.Description = value;
        //            if (PropertyChanged != null)
        //            {
        //                PropertyChanged(this, new PropertyChangedEventArgs("Description"));
        //            }
        //        }
        //    }
        //}

        //public int ID => _duck.ID; // Pozostawiamy tylko do odczytu
        //public int ProducerID => _duck.ProducerID; // Pozostawiamy tylko do odczytu

        //public string ProducerName
        //{
        //    get => _producerVM?.Name ?? "Unknown";
        //    set
        //    {
        //        if (_producerVM != null && _producerVM.Name != value)
        //        {
        //            _producerVM.Name = value;
        //            if (PropertyChanged != null)
        //            {
        //                PropertyChanged(this, new PropertyChangedEventArgs("ProducerName"));
        //                //PropertyChanged(this, new PropertyChangedEventArgs("DisplayText"));
        //            }
        //        }
        //    }
        //}


        // === DODANY SETTER dla Price ===
        //public double Price
        //{
        //    get => _duck.Price;
        //    set
        //    {
        //        if (_duck.Price != value)
        //        {
        //            _duck.Price = value;
        //            if (PropertyChanged != null)
        //            {
        //                PropertyChanged(this, new PropertyChangedEventArgs("Price"));
        //            }
        //        }
        //    }
        //}

        // === DODANY SETTER dla Photo ===
        //public string Photo
        //{
        //    get => _duck.Photo;
        //    set
        //    {
        //        if (_duck.Photo != value)
        //        {
        //            _duck.Photo = value;
        //            if (PropertyChanged != null)
        //            {
        //                PropertyChanged(this, new PropertyChangedEventArgs("Photo"));
        //            }
        //        }
        //    }
        //}


        //TODO dokończyć


        //protected void OnPropertyChanged(string propertyName) =>
        //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));




    }
}
