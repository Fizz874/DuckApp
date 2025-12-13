using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Strzelecki_Baranowski.DuckApp.INTERFACES;

namespace Strzelecki_Baranowski.DuckApp.UI
{
    public partial class ProducerViewModel : ObservableValidator
    {

        private IProducer _producer;

        public ProducerViewModel(IProducer producer)
        {
            _producer = producer ;
            //_name = _producer?.Name ?? string.Empty;
            //_iD = _producer?.ID ?? -1;
            //_website = _producer?.Website ?? string.Empty;

        }

        public ProducerViewModel Clone(IProducer newModel)
        {
            newModel.ID = this.ID;
            newModel.Name = this.Name;
            newModel.Website = this.Website;
            var clonedVM = new ProducerViewModel(newModel);

            return clonedVM;

            //return (ProducerViewModel)this.MemberwiseClone();
        }

        public IProducer GetProd()
        {
            return _producer;
        }

        public void UpdateFrom(ProducerViewModel source)
        {

            this.Name = source.Name;
            this.Website = source.Website;
            this.ID = source.ID;

            //OnPropertyChanged(nameof(Name));
            //OnPropertyChanged(nameof(Website));
            //OnPropertyChanged(nameof(ID));
            //if (_producer != null)
            //{
            //    _producer.Name = source.Name;
            //    _producer.Website = source.Website;
            //}
        }


        [Required(ErrorMessage ="Name field cannot be empty!")]
        [MinLength(1, ErrorMessage = "Name field cannot be shorter than 1 letter!")]
        [MaxLength(250, ErrorMessage = "Name field cannot be longer than 250 letters!")]

        public string Name
        {
            get => _producer.Name;
            set
            {

                if (_producer.Name != value)
                {
                    ValidateProperty(value, nameof(Name));

                    _producer.Name = value;
                    OnPropertyChanged(); 
                }
            }
        }

        [Url]
        public string Website
        {
            get => _producer.Website;
            set
            {

                if (_producer.Website != value)
                {
                    ValidateProperty(value, nameof(Website));

                    _producer.Website = value;
                    OnPropertyChanged();
                }
            }
        }

        public int ID
        {
            get => _producer.ID;
            set
            {

                if (_producer.ID != value)
                {
                    _producer.ID = value;
                    OnPropertyChanged();
                }

            }
        }


        [RelayCommand]
        private void OpenLink(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                // Logika otwierania przeglądarki przeniesiona do ViewModelu
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
        }


        public void Validate()
        {
            this.ValidateAllProperties();
        }

        //[ObservableProperty]
        //private string _name;

        //[ObservableProperty]
        //private int _iD;

        //[ObservableProperty]
        //public string _website;


    }
}
