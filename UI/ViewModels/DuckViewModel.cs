using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CORE;
using Microsoft.Win32;
using Strzelecki_Baranowski.DuckApp.INTERFACES;

namespace Strzelecki_Baranowski.DuckApp.UI
{

    

    public partial class DuckViewModel : ObservableValidator
    {

        private IDuck _duck;

        [ObservableProperty]
        private ProducerViewModel? _producerVM;
        //public event PropertyChangedEventHandler? PropertyChanged;
        [ObservableProperty]
        private string _producerName;


        private string _priceInput;

        public DuckViewModel(IDuck duck, ProducerViewModel? producer = null)
        {
            _duck = duck;
            _producerVM = producer;
            _producerName = _producerVM?.Name ?? "Brak producenta";
            _priceInput = _duck.Price.ToString("0.##") ?? "0";
            //TODO zdaniem copilota tutaj validate all nwm czy musi być
            //ValidateAllProperties(); //no chyba nie

            //TODO ogarnąć dodawanie od nowa


            //_name = _duck?.Name ?? string.Empty;
            //_iD = _duck?.ID ?? -1;
            //_producerID = _duck?.ProducerID ?? 0;
            //// _producerName = _producerVM.Name;
            //_photo = _duck?.Photo ?? string.Empty;
            //_price = _duck?.Price ?? 0.0;
            //_description = _duck?.Description ?? string.Empty;
            //_category = _duck?.Category ?? Category.None;

            //if (_producerVM != null)
            //{
            //    _producerVM.PropertyChanged += Producer_PropertyChanged;
            //}

            if (_producerVM != null)
            {
                _producerVM.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == nameof(ProducerViewModel.Name))
                    {
                        // Aktualizujemy nasze "zdjęcie" (pole tekstowe)
                        ProducerName = _producerVM.Name;
                    }
                };
            }



        }

        [Required(ErrorMessage = "Name field cannot be empty!")]
        [MinLength(1, ErrorMessage = "Name field cannot be shorter than 1 letter!")]
        [MaxLength(250, ErrorMessage = "Name field cannot be longer than 250 letters!")]
        public string Name  //TODO czy nazwy powinny być unikalne?
        {
            get => _duck.Name;
            set
            {

                if (_duck.Name != value)
                {
                    ValidateProperty(value, nameof(Name));

                    _duck.Name = value;
                    OnPropertyChanged(); // Powiadamiamy widok
                }
            }
        }


        [Required(ErrorMessage = "Price field cannot be empty!")]
        //[Range(0.0, double.MaxValue, ErrorMessage = "Price must be a non-negative number")]
        [RegularExpression(@"^\d+([.,]\d{0,2})?$", ErrorMessage = "The price must be a non-negative number (max. 2 decimal places)."/*"Price cannot have more than 2 decimal places"*/)]
        public string Price
        {
            get => _priceInput; //_duck.Price;


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

            //set
            //{

            //        if (Math.Abs(_duck.Price - value) > 0.001)
            //        {
            //            ValidateProperty(value, nameof(Price));
            //            _duck.Price = value;
            //            OnPropertyChanged();
            //        }

            //}
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


        public string FullPhotoPath
        {
            get
            {
                if (string.IsNullOrEmpty(Photo))
                    return null;

                // Jeśli ścieżka jest już absolutna (np. http lub C:\), zwróć ją bez zmian
                if (Path.IsPathRooted(Photo))
                    return Photo;

                // Jeśli względna -> doklej folder uruchomieniowy aplikacji
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


        


        //[ObservableProperty]
        //[Required(ErrorMessage = "This field cannot be empty!")]
        //[MinLength(1, ErrorMessage = "This field cannot be shorter than 1 letter!")]
        //[MaxLength(250, ErrorMessage = "This field cannot be longer than 250 letters!")]
        //private string _name;
        //[ObservableProperty]
        //private int _iD;
        //[ObservableProperty]
        //private int _producerID;



        //[ObservableProperty]
        //private string _photo;

        //[ObservableProperty]
        //[Required(ErrorMessage = "This field cannot be empty!")]
        //[Range(0.0,double.MaxValue, ErrorMessage = "Prize must be a positive number")]
        //[RegularExpression(@"^\d+([.]\d{1,2})?$", ErrorMessage = "Prize cannot have more than 2 decimal places")]
        //private double _price;
        //[ObservableProperty]
        //[MaxLength(10000, ErrorMessage = "This field cannot be longer than 10000 letters!")]
        //private string _description;
        //[ObservableProperty]
        //private Category _category;


        //partial void OnNameChanged(string value)
        //{
        //    ValidateProperty(nameof(Name));
        //}

        //partial void OnPriceChanged(double value)
        //{
        //    ValidateProperty(nameof(Price));
        //}

        //partial void OnDescriptionChanged(string value)
        //{
        //    ValidateProperty(nameof(Description));
        //}




        //public string ProducerName => _producerVM?.Name ?? "Brak producenta";


        //private void Producer_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        //{
        //    // Jeśli zmieniła się nazwa producenta...
        //    if (e.PropertyName == nameof(ProducerViewModel.Name))
        //    {
        //        // ...zaktualizuj naszą lokalną kopię (to odświeży widok Hyperlinku)
        //        ProducerName = _producerVM!.Name;
        //    }
        //}



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



            //return (DuckViewModel)this.MemberwiseClone();
        }

        public void UpdateFrom(DuckViewModel source)
        {
            ValidateAllProperties();

            this.Name = source.Name;
            //this.Price = source.Price;
            this._priceInput = source.GetDuck().Price.ToString("0.##") ?? "0";
            OnPropertyChanged(nameof(Price));
            this.Description = source.Description;
            //TODO - zmiana producerId powinna powodować zmianę _producerVM
            this.ProducerVM = source.ProducerVM;
            this.ProducerID = source.ProducerVM!.ID;
            this.ProducerName = source.ProducerVM.Name;
            this.Category = source.Category;
            this.Photo = source.Photo;

            //if (_duck != null)
            //{
            //    this._duck.Name = source.Name;
            //    this._duck.Price = source.Price;
            //    this._duck.Description = source.Description;
            //    this._duck.ProducerID = source.ProducerVM!.ID;
            //    this._duck.Photo = source.Photo;
            //    this._duck.Category = source.Category;
            //}


            //OnPropertyChanged(nameof(Name));
            //OnPropertyChanged(nameof(Price));
            //OnPropertyChanged(nameof(Description));
            //OnPropertyChanged(nameof(Category));
            //OnPropertyChanged(nameof(Photo));
            //OnPropertyChanged(nameof(ProducerName));
            //OnPropertyChanged(nameof(ProducerID));


        }


        public IDuck GetDuck()
        {
            return _duck;
        }

        [RelayCommand]
        private void SelectPhoto()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Wybierz zdjęcie kaczki";
            openFileDialog.Filter = "Pliki obrazów|*.jpg;*.jpeg;*.png;*.bmp|Wszystkie pliki|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;

                try
                {
                    string destFolder = "photos";//Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "photos");

                    if (!Directory.Exists(destFolder))
                        Directory.CreateDirectory(destFolder);

                    string extension = Path.GetExtension(selectedFilePath);
                    string newFileName = Guid.NewGuid().ToString() + extension;
                    string destPath = Path.Combine(destFolder, newFileName);

                    File.Copy(selectedFilePath, destPath);

                    this.Photo = destPath.Replace("\\", "/"); //$"pack://siteoforigin:,,,/photos/{newFileName}";//


                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Uploading the image failed: " + ex.Message);
                }
            }
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
