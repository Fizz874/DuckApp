using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Strzelecki_Baranowski.DuckApp.BL;
using Strzelecki_Baranowski.DuckApp.INTERFACES;
using Strzelecki_Baranowski.DuckApp.UI;

namespace Strzelecki_Baranowski.DuckApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {

        private readonly BLC _businessLogic;



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
        private object _selectedItem;

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

            DummyObject = new DuckViewModel(null, null);

            if (ListType == "ducks")
            {
               
                DummyObject = new DuckViewModel(null, null);

            }
            else
            {
                DummyObject = new ProducerViewModel(null);
            }

            //Kopiowanie wartości do dummy
            EditVisibility = Visibility.Visible;
            DetailsVisibility = Visibility.Collapsed;
        }

        [RelayCommand(CanExecute = nameof(CanEdit))]
        private void DeleteItem()
        {
            try
            {
                if (SelectedItem == null) return;
                if (SelectedItem.GetType() == typeof(DuckViewModel))
                {
                    var duck = SelectedItem as DuckViewModel;
                    if (duck == null) return;

                    DuckVM.Ducks.Remove(SelectedItem as DuckViewModel);
                    _businessLogic.DeleteDuck(duck.ID);

                }
                else
                {
                    var prodVM = SelectedItem as ProducerViewModel;
                    if (prodVM == null) return;

                    ProducerVM.Producers.Remove(SelectedItem as ProducerViewModel);
                    _businessLogic.DeleteDuck(prodVM.ID);
                }


            }
            catch
            {
                //TODO ktoś coś
            }
        }


        [RelayCommand(CanExecute = nameof(CanEdit))]
        private void EditItem()
        {
            //rozróżnić czy producer czy kaczka
            if (SelectedItem == null) return;
            if (SelectedItem.GetType() == typeof(DuckViewModel))
            {
                var duckVM = SelectedItem as DuckViewModel;
                if (duckVM == null) return;

                
                DummyObject = duckVM.Clone();

            }
            else
            {
                var prodVM = SelectedItem as ProducerViewModel;
                if (prodVM == null) return;

                DummyObject = prodVM.Clone();  
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
        private void Save() //TODO dlaczego nie działa dodawanie + dlaczego utrata focucu nie działa
        {
            try
            {

              

                if (SelectedItem == null) return;
                if (SelectedItem.GetType() == typeof(DuckViewModel))
                {


                    var editedDuck = DummyObject as DuckViewModel;
                    if (editedDuck == null) return;

                    if (editedDuck.ID == -1)
                    {
                        int iD = _businessLogic.AddNewDuck(editedDuck.Name, editedDuck.ProducerID, editedDuck.Price, editedDuck.Photo, editedDuck.Description, editedDuck.Category );
                        
                        editedDuck.ID = iD;

                        DuckVM.Ducks.Add( editedDuck );

                    } else
                    {

                    

                        _businessLogic.UpdateDuck(editedDuck.GetDuck()); //TODO zaktualizować IDUCK'a a nie tylko VM


                        var ducks = DuckVM.Ducks;
                        for (int i = 0; i < ducks.Count; i++)
                        {
                            if (ducks[i].ID == editedDuck.ID)
                            {
                                ducks[i] = editedDuck;
                                break;
                            }
                        }

                    }
                }
                else
                {

                    var editedProd = DummyObject as ProducerViewModel;
                    if (editedProd == null) return;


                    if (editedProd.ID == -1)
                    {
                        int iD = _businessLogic.AddNewProducer(editedProd.Name, editedProd.Website);

                        editedProd.ID = iD;

                        ProducerVM.Producers.Add(editedProd);

                    }
                    else
                    {

                        

                        _businessLogic.UpdateProducer(editedProd.GetProd());


                        var prods = ProducerVM.Producers;
                        for (int i = 0; i < prods.Count; i++)
                        {
                            if (prods[i].ID == editedProd.ID)
                            {
                                prods[i] = editedProd;
                                break;
                            }
                        }
                    }
                }

                DetailsVisibility = Visibility.Visible;
                EditVisibility = Visibility.Collapsed;

            }
            catch (Exception ex)
            {
                //TODO jakiś komunikat może?
                throw ex;
            }
        }
}
}
