using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment_6.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Event that gets called to notify the UI when properties change
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Used to notify the UI when properties change
        /// </summary>
        /// <param name="info"></param>
        public void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
