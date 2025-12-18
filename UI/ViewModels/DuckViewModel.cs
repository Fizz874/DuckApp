using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Strzelecki_Baranowski.DuckApp.CORE;
using Microsoft.Win32;
using Strzelecki_Baranowski.DuckApp.INTERFACES;

namespace Strzelecki_Baranowski.DuckApp.UI
{
    public partial class DuckViewModel : ObservableValidator
    {
        private IDuck _duck;

        public DuckViewModel(IDuck duck, ProducerViewModel? producer = null)
        {
            _duck = duck;
            _producerVM = producer;
            _producerName = _producerVM?.Name ?? "No producer";
            _priceInput = _duck.Price.ToString("0.##") ?? "0";

            if (_producerVM != null)
            {
                _producerVM.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == nameof(ProducerViewModel.Name))
                    {
                        ProducerName = _producerVM.Name;
                    }
                };
            }
        }

        [Required(ErrorMessage = "You have to choose a producer!")]
        [ObservableProperty]
        private ProducerViewModel? _producerVM;
        partial void OnProducerVMChanged(ProducerViewModel? value)
        {
            if (value != null)
            {
                ProducerID = value.ID;
                ProducerName = value.Name; 
            }
            else
            {
                ProducerID = 0;
            }
        }

        [ObservableProperty]
        private string _producerName;

        private string _priceInput;

        [Required(ErrorMessage = "Name field cannot be empty!")]
        [MinLength(1, ErrorMessage = "Name field cannot be shorter than 1 letter!")]
        [MaxLength(250, ErrorMessage = "Name field cannot be longer than 250 letters!")]
        public string Name
        {
            get => _duck.Name;
            set
            {

                if (_duck.Name != value)
                {
                    ValidateProperty(value, nameof(Name));

                    _duck.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        [Required(ErrorMessage = "Price field cannot be empty!")]
        [RegularExpression(@"^\d+([.,]\d{0,2})?$", ErrorMessage = "The price must be a non-negative number (max. 2 decimal places).")]
        public string Price
        {
            get => _priceInput;
            set
            {
                if (_priceInput != value)
                {
                    _priceInput = value;

                    ValidateProperty(value, nameof(Price));

                    if (string.IsNullOrEmpty(value)) return;

                    string decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                    string normalizedInput = value.Replace(".", decimalSeparator).Replace(",", decimalSeparator);

                    if (double.TryParse(normalizedInput, out double parsedValue))
                    {
                        if (parsedValue >= 0)
                        {
                            _duck.Price = parsedValue;


                        }
                    }
                }
            }
        }

        [MaxLength(10000, ErrorMessage = "Description cannot be longer than 10000 letters!")]
        public string Description
        {
            get => _duck.Description;
            set
            {
                if (_duck.Description != value)
                {
                    ValidateProperty(value, nameof(Description));
                    _duck.Description = value;
                    OnPropertyChanged();
                }
            }
        }

        public Category Category
        {
            get => _duck.Category;
            set
            {
                if (_duck.Category != value)
                {
                    _duck.Category = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Photo
        {
            get => _duck.Photo;
            set
            {
                if (_duck.Photo != value)
                {
                    _duck.Photo = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(FullPhotoPath));   
                }
            }
        }

        public string? FullPhotoPath
        {
            get
            {
                if (string.IsNullOrEmpty(Photo))
                    return null;

                if (Path.IsPathRooted(Photo))
                    return Photo;

                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Photo);
            }
        }

        public int ID
        {
            get => _duck.ID;
            set
            {

                if (_duck.ID != value)
                {
                    _duck.ID = value;
                    OnPropertyChanged();
                }

            }
        }

        public int ProducerID
        {
            get => _duck.ProducerID;
            set
            {
                if (_duck.ProducerID != value)
                {
                    _duck.ProducerID = value;
                    OnPropertyChanged();
                }
            }
        }

        public DuckViewModel Clone(IDuck newModel)
        {
            newModel.ID = this.ID;
            newModel.Name = this.Name;
            newModel.Price = double.Parse(this.Price);
            newModel.Description = this.Description;
            newModel.Category = this.Category;
            newModel.Photo = this.Photo;
            newModel.ProducerID = this.ProducerID;

            var clonedVM = new DuckViewModel(newModel);
            clonedVM.ProducerVM = this.ProducerVM;
            return clonedVM;
        }

        public void UpdateFrom(DuckViewModel source)
        {
            ValidateAllProperties();

            this.Name = source.Name;
            this._priceInput = source.GetDuck().Price.ToString("0.##") ?? "0";
            OnPropertyChanged(nameof(Price));
            this.Description = source.Description;
            this.ProducerVM = source.ProducerVM;
            this.ProducerID = source.ProducerVM!.ID;
            this.ProducerName = source.ProducerVM.Name;
            this.Category = source.Category;
            this.Photo = source.Photo;
        }

        public IDuck GetDuck()
        {
            return _duck;
        }

        [RelayCommand]
        private void SelectPhoto()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Choose duck photo";
            openFileDialog.Filter = "Image files|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;

                try
                {
                    string destFolder = "photos";

                    if (!Directory.Exists(destFolder))
                        Directory.CreateDirectory(destFolder);

                    string extension = Path.GetExtension(selectedFilePath);
                    string newFileName = Guid.NewGuid().ToString() + extension;
                    string destPath = Path.Combine(destFolder, newFileName);

                    File.Copy(selectedFilePath, destPath);

                    this.Photo = destPath.Replace("\\", "/");
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Uploading the image failed: " + ex.Message);
                }
            }
        }

        public void Validate()
        {
            this.ValidateAllProperties();
        }
    }
}
