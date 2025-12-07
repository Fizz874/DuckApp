using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Strzelecki_Baranowski.DuckApp.INTERFACES;

namespace Strzelecki_Baranowski.DuckApp.UI
{
    public partial class ProducerViewModel : ObservableObject
    {

        private IProducer? _producer;

        public ProducerViewModel(IProducer? producer)
        {
            _producer = producer ;
            _name = _producer?.Name ?? string.Empty;
            _iD = _producer?.ID ?? -1;
            _website = _producer?.Website ?? string.Empty;

        }

        public ProducerViewModel Clone()
        {
            return (ProducerViewModel)this.MemberwiseClone();
        }

        public IProducer GetProd()
        {
            return _producer;
        }


        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private int _iD;

        [ObservableProperty]
        public string _website;


    }
}
