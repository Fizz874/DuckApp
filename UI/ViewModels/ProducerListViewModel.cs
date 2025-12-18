using System.Collections.Generic;
using System.Collections.ObjectModel;
using Strzelecki_Baranowski.DuckApp.INTERFACES;

namespace Strzelecki_Baranowski.DuckApp.UI
{
    public class ProducerListViewModel
    {

        public ObservableCollection<ProducerViewModel> Producers { get; set; } = [];
        public ProducerListViewModel(IEnumerable<IProducer> list)
        {
            foreach (var item in list)
            {
                Producers.Add(new ProducerViewModel(item));
            }
        }


    }
}
