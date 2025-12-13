using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CORE;
using Strzelecki_Baranowski.DuckApp.BL;
using Strzelecki_Baranowski.DuckApp.INTERFACES;
using Strzelecki_Baranowski.DuckApp.UI;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Strzelecki_Baranowski.DuckApp.ViewModels
{
    public partial class MainViewModel : ObservableValidator
    {

        private readonly BLC _businessLogic;

        public IEnumerable<Category> CategoryValues
        {
            get
            {
                return Enum.GetValues(typeof(Category)).Cast<Category>();
            }
        }

        public DuckListViewModel DuckVM { get; set; } //TODO private?
        public ProducerListViewModel ProducerVM { get; set; }



        public MainViewModel(BLC businessLogic) {


            _businessLogic = businessLogic;


            var ducks = _businessLogic.GetAllDucks();
            var producers = _businessLogic.GetAllProducers();
            ProducerVM = new ProducerListViewModel(producers);
            DuckVM = new DuckListViewModel(ducks, ProducerVM);
            _list = DuckVM.Ducks;

        }

        [ObservableProperty]
        private IEnumerable _list;

        [ObservableProperty]
        private object? _selectedItem;

        [ObservableProperty]
        private Visibility _editVisibility = Visibility.Collapsed;

        [ObservableProperty]
        private Visibility _detailsVisibility = Visibility.Visible;


        //[ObservableProperty]
        //private DuckViewModel _dummyDuck;

        //[ObservableProperty]
        //private ProducerViewModel _dummyProducer;

        [ObservableProperty]
        private object _dummyObject;

        partial void OnDummyObjectChanged(object? oldValue, object? newValue)
        {
            // 1. WYREJESTROWANIE (ODPIĘCIE) STAREGO OBIEKTU
            // Rzutujemy starego object na INotifyDataErrorInfo, aby odpiąć zdarzenie.
            if (oldValue is INotifyDataErrorInfo oldDataErrorInfo)
            {
                oldDataErrorInfo.ErrorsChanged -= DummyObject_ErrorsChanged;
            }

            // 2. REJESTRACJA (PODPIĘCIE) NOWEGO OBIEKTU
            // Rzutujemy nowego object na INotifyDataErrorInfo, aby podpiąć zdarzenie.
            if (newValue is INotifyDataErrorInfo newDataErrorInfo)
            {
                newDataErrorInfo.ErrorsChanged += DummyObject_ErrorsChanged;
            }

            // 3. POWIADOMIENIE WIDOKU
            // Zawsze powiadamiamy, że lista błędów mogła się zmienić
            OnPropertyChanged(nameof(Bledy));
        }
        private void DummyObject_ErrorsChanged(object? sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Bledy));
        }

        [ObservableProperty]
        private string _listType = "ducks";

        [RelayCommand]
        private void ShowDucks()
        {
            ListType = "ducks";
            List = DuckVM.Ducks;
        }

        [RelayCommand]
        private void ShowProducers()
        {
            ListType = "producers";
            List = ProducerVM.Producers;
        }

        [RelayCommand]
        private void ShowSpecifiedProducer()
        {
            var duckVM = SelectedItem as DuckViewModel;
            if (duckVM == null)
             return;
            int producer = duckVM.ProducerID;
            
            //if (producer != -1) return;
            List = ProducerVM.Producers;
            ListType = "producers";
            SelectedItem = ProducerVM.Producers
            .FirstOrDefault(p => p.ID == producer);

        }

        [RelayCommand(CanExecute = nameof(CanEdit))]
        private void AddItem()
        {

            //DummyObject = new DuckViewModel(null, null);

            if (ListType == "ducks")
            {
               
                var newduck  = new DuckViewModel(_businessLogic.GetNewDuck(), /*ProducerVM.Producers.FirstOrDefault(x=>x.ID==0)*/null) ;
                newduck.ID = -1;
                newduck.Name = "";
                DummyObject = newduck;
            }
            else
            {
                DummyObject = new ProducerViewModel(_businessLogic.GetNewProducer());
            }

            EditVisibility = Visibility.Visible;
            DetailsVisibility = Visibility.Collapsed;
        }

        [RelayCommand(CanExecute = nameof(CanEdit))]
        private void DeleteItem() 
        {
            try
            {
                if (SelectedItem == null) return;
                //if (SelectedItem.GetType() == typeof(DuckViewModel))
                if (SelectedItem is DuckViewModel duckVM)
                {
                    //var duck = SelectedItem as DuckViewModel;
                    //if (duck == null) return;

                    var result = MessageBox.Show(
                        $"Do you really want do delete duck '{duckVM.Name}'?",
                        "Delete confirmation",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning); //TODO Tak i Nie  nie są po angielsku - co robić? własne okno?


                    if (result == MessageBoxResult.No) return;

                    DuckVM.Ducks.Remove(/*SelectedItem as DuckViewModel*/duckVM);
                    _businessLogic.DeleteDuck(duckVM.ID);

                }
                else if (SelectedItem is ProducerViewModel prodVM)
                {

                    var result = MessageBox.Show(
                        $"Do you really want do delete producer '{prodVM.Name}'?\n\nWarning: All ducks assigned to him will also be deleted!",
                        "Delete confirmation",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning); //TODO Tak i Nie  nie są po angielsku - co robić? własne okno?

                    if (result == MessageBoxResult.No) return;

                    var ducksToRemove = DuckVM.Ducks.Where(d => d.ProducerID == prodVM.ID).ToList();

                    foreach (var duck in ducksToRemove)
                    {
                        DuckVM.Ducks.Remove(duck);
                    }


                    ProducerVM.Producers.Remove(prodVM);


                    _businessLogic.DeleteProducer(prodVM.ID);

                    SelectedItem = null;


                    ////var prodVM = SelectedItem as ProducerViewModel; //TODO przy usuwaniu przeba zmienić odniesienia we wszystkich kaczkach
                    ////if (prodVM == null) return;

                    //int defaultProducerId = 0;

                    //var fallbackProducer = ProducerVM.Producers.FirstOrDefault(p => p.ID == defaultProducerId);


                    //var ducksToUpdate = DuckVM.Ducks.Where(d => d.ProducerID == prodVM.ID).ToList();

                    //foreach (var duck in ducksToUpdate)
                    //{
                    //    duck.ProducerVM = fallbackProducer;
                    //    duck.ProducerID = fallbackProducer?.ID ?? defaultProducerId;
                    //    duck.ProducerName = fallbackProducer?.Name ?? "Inny producent";
                    //}

                    //ProducerVM.Producers.Remove(prodVM);
                    //_businessLogic.DeleteProducer(prodVM.ID);
                }
                SelectedItem = null;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        [RelayCommand(CanExecute = nameof(CanEdit))]
        private void EditItem()
        {
            if (SelectedItem == null) return;
            //if (SelectedItem.GetType() == typeof(DuckViewModel))
            if (SelectedItem is DuckViewModel duckVM)
            {
                //var duckVM = SelectedItem as DuckViewModel;
                //if (duckVM == null) return;

                
                DummyObject = duckVM.Clone(_businessLogic.GetNewDuck());

            }
            else if (SelectedItem is ProducerViewModel prodVM)
            {
                //var prodVM = SelectedItem as ProducerViewModel;
                //if (prodVM == null) return;

                DummyObject = prodVM.Clone(_businessLogic.GetNewProducer());  
            }

                //Kopiowanie wartości do dummy
                EditVisibility = Visibility.Visible;
                DetailsVisibility = Visibility.Collapsed;
        }


        private bool CanEdit()
        {

            if(EditVisibility == Visibility.Visible) return false;
            return true;
        }


        [RelayCommand]
        private void Cancel()
        { 
          DetailsVisibility = Visibility.Visible; 
          EditVisibility = Visibility.Collapsed;
        }




        [RelayCommand]
        private void Save() //TODO dlaczego utrata focucu nie działa
        {
            try
            {
                
                if (DummyObject is DuckViewModel duck)
                {
                    duck.Validate(); 
                }
                else if (DummyObject is ProducerViewModel producer)
                {
                    producer.Validate();
                }

                if (Bledy.Count() > 0)
                {

                    string komunikat = string.Join("\n ", Bledy);
                    System.Windows.MessageBox.Show(
                    $"Cannot save. The form contains errors:\n\n{komunikat}",
                    "Validation error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning);
                    return;

                }




                switch (DummyObject)
                {



                    //if (SelectedItem == null) return;
                    //if (SelectedItem.GetType() == typeof(DuckViewModel))
                    //{

                    case DuckViewModel editedDuck:
                        //var editedDuck = DummyObject as DuckViewModel;
                        //if (editedDuck == null) return;

                        if (editedDuck.ID == -1)
                        {

                            //editedDuck.ProducerID = editedDuck.ProducerVM!.ID;
                            //editedDuck.ProducerName = editedDuck.ProducerVM.Name;

                            int iD = _businessLogic.AddNewDuck(editedDuck.GetDuck()/*editedDuck.Name, editedDuck.ProducerID, editedDuck.Price, editedDuck.Photo, editedDuck.Description, editedDuck.Category */);

                            editedDuck.ID = iD; 

                            DuckVM.Ducks.Add(editedDuck);
                            SelectedItem = editedDuck;


                        }
                        else
                        {





                            var ducks = DuckVM.Ducks;
                            for (int i = 0; i < ducks.Count; i++)
                            {
                                if (ducks[i].ID == editedDuck.ID)
                                {


                                    ducks[i].UpdateFrom(editedDuck);
                                    SelectedItem = ducks[i];
                                    break;
                                }
                            }
                            _businessLogic.UpdateDuck(editedDuck.GetDuck()); //TODO nie przenosić przed UPDATEFrom bo wtdy viewmodel nie łapie zmian XD



                        }


                        ShowDucks();


                        break;

                    //}
                    //else
                    //{
                    case ProducerViewModel editedProd:

                        //var editedProd = DummyObject as ProducerViewModel;
                        //if (editedProd == null) return;

                        if (editedProd.ID == -1)
                        {
                            int iD = _businessLogic.AddNewProducer(editedProd.GetProd()/*editedProd.Name, editedProd.Website*/);

                            editedProd.ID = iD; //TODO trzeba by aktualizować  _producer chyba że się go zupełnie pozbywamy (wtedy trzeba przerobić update w BLC)

                            ProducerVM.Producers.Add(editedProd);
                            SelectedItem = editedProd;

                        }
                        else
                        {





                            var prods = ProducerVM.Producers;
                            for (int i = 0; i < prods.Count; i++)
                            {
                                if (prods[i].ID == editedProd.ID)
                                {
                                    prods[i].UpdateFrom(editedProd);
                                    SelectedItem = prods[i];
                                    break;
                                }
                            }

                            _businessLogic.UpdateProducer(editedProd.GetProd());

                        }



                        ShowProducers();

                        break;
                }





                DetailsVisibility = Visibility.Visible;
                EditVisibility = Visibility.Collapsed;

            }
            catch (Exception ex)
            {
                //TODO jakiś komunikat może?
               MessageBox.Show("Saving error occurred: " + ex.Message);

                //throw ex;
            }

        }

        public IEnumerable<string> Bledy
        {
            get
            {
                switch (DummyObject)
                {
                    case DuckViewModel duckVM:
                        {
                            //var errors = new List<string>();
                            //var properties = duckVM.GetType().GetProperties();

                            //foreach (var property in properties)
                            //{

                            //    IEnumerable currentErrors = duckVM.GetErrors(property.Name);

                            //    if (currentErrors != null)
                            //    {
                            //        foreach (var error in currentErrors)
                            //        {
                            //            if (error != null) errors.Add(error.ToString());
                            //        }
                            //    }
                            //}
                            return duckVM.GetErrors().Select(x => x.ErrorMessage ?? "");//errors.Distinct();
                        }

                    case ProducerViewModel producerVM:
                        {
                            //var errors = new List<string>();
                            //var properties = producerVM.GetType().GetProperties();

                            //foreach (var property in properties)
                            //{
                            //    IEnumerable currentErrors = producerVM.GetErrors(property.Name);

                            //    if (currentErrors != null)
                            //    {
                            //        foreach (var error in currentErrors)
                            //        {
                            //            if (error != null) errors.Add(error.ToString());
                            //        }
                            //    }
                            //}
                            return producerVM.GetErrors().Select(x => x.ErrorMessage ?? "");//return errors.Distinct();
                        }

                    default:
                        return Enumerable.Empty<string>();
                }
            }
        
    }




    }
}
