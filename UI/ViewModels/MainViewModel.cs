using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Strzelecki_Baranowski.DuckApp.CORE;
using Strzelecki_Baranowski.DuckApp.BL;

namespace Strzelecki_Baranowski.DuckApp.UI
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
        public DuckListViewModel DuckVM { get; set; } 
        public ProducerListViewModel ProducerVM { get; set; }
        public MainViewModel(BLC businessLogic) {
            _businessLogic = businessLogic;

            var ducks = _businessLogic.GetAllDucks();
            var producers = _businessLogic.GetAllProducers();
            ProducerVM = new ProducerListViewModel(producers);
            DuckVM = new DuckListViewModel(ducks, ProducerVM);
            _list = DuckVM.Ducks;
            ActiveFilters = DuckFilters;
            ApplyFilters();

        }
        [ObservableProperty] ObservableCollection<FilterViewModel> _activeFilters;
        public ObservableCollection<FilterViewModel> DuckFilters { get; set; } = new ObservableCollection<FilterViewModel>() {
            new FilterViewModel {PropertyName = "ID", Type=FilterType.Number, Operator=FilterMode.Equal, Value="" },
            new FilterViewModel {PropertyName = "ProducerName", Type=FilterType.Text, Operator=FilterMode.Contains, Value="" },
            new FilterViewModel {PropertyName = "ProducerID", Type=FilterType.Number, Operator=FilterMode.Equal, Value="" },
            new FilterViewModel {PropertyName = "Price", Type=FilterType.Number, Operator=FilterMode.Equal, Value="" },
            new FilterViewModel {PropertyName = "Name", Type=FilterType.Text, Operator=FilterMode.Contains, Value="" },
            new FilterViewModel {PropertyName = "Description", Type=FilterType.Text, Operator=FilterMode.Contains, Value="" },
            new FilterViewModel {PropertyName = "Category", Type=FilterType.Text, Operator=FilterMode.Contains, Value="" },
        };

        public ObservableCollection<FilterViewModel> ProducerFilters { get; set; } = new ObservableCollection<FilterViewModel>() {
            new FilterViewModel {PropertyName = "ID", Type=FilterType.Number, Operator=FilterMode.Equal, Value="" },
            new FilterViewModel {PropertyName = "Name", Type=FilterType.Text, Operator=FilterMode.Contains, Value="" },
            new FilterViewModel {PropertyName = "Website", Type = FilterType.Text, Operator = FilterMode.Contains, Value="" }
        };

        [RelayCommand]
        public void ApplyFilters()
        {
            if (List == null) return;

            var previouslySelected = SelectedItem;
            var query = List.Cast<object>();
            foreach (var filter in ActiveFilters)
            {
                query = query.Where(item => filter.IsMatch(item));
            }

            DisplayedList.Clear();
            foreach (var item in query)
            {
                DisplayedList.Add(item);
            }

            if (previouslySelected != null && DisplayedList.Contains(previouslySelected))
            {
                SelectedItem = previouslySelected;
            }
        }

        [RelayCommand]
        public void ResetFilters()
        {
            foreach (var filter in ActiveFilters)
            {
                filter.Value = "";
                if (filter.Type == FilterType.Text)
                    filter.Operator = FilterMode.Contains;
                else
                    filter.Operator = FilterMode.Equal;
            }
            ApplyFilters();
        }


        [ObservableProperty]
        private IEnumerable _list;
        [ObservableProperty]
        private ObservableCollection<object> _displayedList = new ObservableCollection<object>();

        [ObservableProperty]
        private object? _selectedItem;

        [ObservableProperty]
        private Visibility _editVisibility = Visibility.Collapsed;
        [ObservableProperty]
        private Visibility _detailsVisibility = Visibility.Visible;
        [ObservableProperty]
        private string? _formsHeader;

        [ObservableProperty]
        private object? _dummyObject;
        partial void OnDummyObjectChanged(object? oldValue, object? newValue)
        {
            if (oldValue is INotifyDataErrorInfo oldDataErrorInfo)
            {
                oldDataErrorInfo.ErrorsChanged -= DummyObject_ErrorsChanged;
            }
            if (newValue is INotifyDataErrorInfo newDataErrorInfo)
            {
                newDataErrorInfo.ErrorsChanged += DummyObject_ErrorsChanged;
            }
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
            ActiveFilters = DuckFilters;
            ApplyFilters();
        }

        [RelayCommand]
        private void ShowProducers()
        {
            ListType = "producers";
            List = ProducerVM.Producers;
            ActiveFilters = ProducerFilters;
            ApplyFilters();
        }

        [RelayCommand]
        private void ShowSpecifiedProducer()
        {
            var duckVM = SelectedItem as DuckViewModel;
            if (duckVM == null)
             return;
            int producer = duckVM.ProducerID;
            
            List = ProducerVM.Producers;
            ListType = "producers";
            ActiveFilters = ProducerFilters;
            ResetFilters();
            SelectedItem = ProducerVM.Producers.FirstOrDefault(p => p.ID == producer);
        }

        [RelayCommand]
        private void ShowDucksOfrRoducer()
        {
            var producerVM = SelectedItem as ProducerViewModel;
            if (producerVM == null)
                return;

            List = DuckVM.Ducks;
            ListType = "ducks";
            ActiveFilters = DuckFilters;
            ResetFilters();
            ActiveFilters.FirstOrDefault(x => x.PropertyName == "ProducerID")!.Value = producerVM.ID.ToString();
            ApplyFilters();
        }

        [RelayCommand(CanExecute = nameof(CanEdit))]
        private void AddItem()
        {
            if (ListType == "ducks")
            {
                FormsHeader = "Add duck";
                var newduck  = new DuckViewModel(_businessLogic.GetNewDuck(),null) ;
                newduck.ID = -1;
                newduck.Name = "";
                DummyObject = newduck;
            }
            else
            {
                FormsHeader = "Add producer";
                var newproducer = new ProducerViewModel(_businessLogic.GetNewProducer());
                newproducer.ID = -1;
                newproducer.Name = "";
                DummyObject = newproducer;
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
                if (SelectedItem is DuckViewModel duckVM)
                {
                    var result = MessageBox.Show(
                        $"Do you really want do delete duck '{duckVM.Name}'?",
                        "Delete confirmation",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.No) return;

                    _businessLogic.DeleteDuck(duckVM.ID);
                    DuckVM.Ducks.Remove(duckVM);
                }
                else if (SelectedItem is ProducerViewModel prodVM)
                {
                    var result = MessageBox.Show(
                        $"Do you really want do delete producer '{prodVM.Name}'?\n\nWarning: All ducks assigned to him will also be deleted!",
                        "Delete confirmation",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.No) return;

                    _businessLogic.DeleteProducer(prodVM.ID);

                    var ducksToRemove = DuckVM.Ducks.Where(d => d.ProducerID == prodVM.ID).ToList();
                    foreach (var duck in ducksToRemove)
                    {
                        DuckVM.Ducks.Remove(duck);
                    }
                    ProducerVM.Producers.Remove(prodVM);

                    SelectedItem = null;
                }
                ApplyFilters();
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
            
            if (SelectedItem is DuckViewModel duckVM)
            {
                FormsHeader = "Edit duck";
                DummyObject = duckVM.Clone(_businessLogic.GetNewDuck());
            }
            else if (SelectedItem is ProducerViewModel prodVM)
            {
                FormsHeader = "Edit producer";
                DummyObject = prodVM.Clone(_businessLogic.GetNewProducer());  
            }

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
        private void Save()
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
                    case DuckViewModel editedDuck:
                        if (editedDuck.ID == -1)
                        {
                            int iD = _businessLogic.AddNewDuck(editedDuck.GetDuck());
                            editedDuck.ID = iD; 

                            DuckVM.Ducks.Add(editedDuck);
                            SelectedItem = editedDuck;
                        }
                        else
                        {
                            _businessLogic.UpdateDuck(editedDuck.GetDuck());
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
                        }
                        ShowDucks();
                        break;
                    case ProducerViewModel editedProd:
                        if (editedProd.ID == -1)
                        {
                            int iD = _businessLogic.AddNewProducer(editedProd.GetProd());
                            editedProd.ID = iD;

                            ProducerVM.Producers.Add(editedProd);
                            SelectedItem = editedProd;
                        }
                        else
                        {
                            _businessLogic.UpdateProducer(editedProd.GetProd());
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
                        }
                        ShowProducers();
                        break;
                }
                ApplyFilters();
                DetailsVisibility = Visibility.Visible;
                EditVisibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
               MessageBox.Show("Saving error occurred: " + ex.Message);
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
                            return duckVM.GetErrors().Select(x => x.ErrorMessage ?? "");
                        }
                    case ProducerViewModel producerVM:
                        {
                            return producerVM.GetErrors().Select(x => x.ErrorMessage ?? "");
                        }
                    default:
                        return Enumerable.Empty<string>();
                }
            }
        }   
    }
}
