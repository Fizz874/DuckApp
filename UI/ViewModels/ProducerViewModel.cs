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

        public void UpdateFrom(ProducerViewModel source)
        {

            this.Name = source.Name;
            this.Website = source.Website;
            
            if (_producer != null)
            {
                _producer.Name = source.Name;
                _producer.Website = source.Website;
            }
        }



        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private int _iD;

        [ObservableProperty]
        public string _website;


    }
}
