using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Input;
using Strzelecki_Baranowski.DuckApp.INTERFACES;

namespace Strzelecki_Baranowski.DuckApp.UI
{
    public class DuckListViewModel
    {
        public ObservableCollection<DuckViewModel> Ducks { get; set; } = new();
        public DuckListViewModel(IEnumerable<IDuck> duckList, ProducerListViewModel producerList)
        {
            var producerDict = producerList.Producers.ToDictionary(p => p.ID);

            foreach (var duck in duckList)
            {
                producerDict.TryGetValue(duck.ProducerID, out var producerVm);
                Ducks.Add(new DuckViewModel(duck, producerVm));
            }
        }

    }

    }
