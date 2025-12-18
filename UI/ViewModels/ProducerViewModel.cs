using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
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
            _producer = producer;
        }

        public ProducerViewModel Clone(IProducer newModel)
        {
            newModel.ID = this.ID;
            newModel.Name = this.Name;
            newModel.Website = this.Website;
            var clonedVM = new ProducerViewModel(newModel);

            return clonedVM;
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
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
        }


        public void Validate()
        {
            this.ValidateAllProperties();
        }

    }
}
