using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Strzelecki_Baranowski.DuckApp.INTERFACES;

namespace Strzelecki_Baranowski.DuckApp.UI
{
    public class ProducerViewModel : INotifyPropertyChanged
    {

        private IProducer _producer;
        public event PropertyChangedEventHandler? PropertyChanged;


        public ProducerViewModel(IProducer producer)
        {
            _producer = producer;
        }


        public string Name
        {
            get { return _producer.Name; }
            set
            {
                if (_producer.Name != value && _producer.Name != null)
                {
                    _producer.Name = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                        //PropertyChanged(this, new PropertyChangedEventArgs("DisplayText"));
                    }
                }
            }
        }

        public int ID => _producer.ID;
        public string Website => _producer.Website;


    }
}
