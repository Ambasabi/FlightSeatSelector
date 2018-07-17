//using assignment_6.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment_6.ViewModels
{
    public class FlightViewModel : ViewModelBase
    {
     
        /// <summary>
        /// data access object uised for this 
        /// </summary>
        private clsDataAccess _dataAccess = new clsDataAccess();

        /// <summary>
        /// Globale datatable object
        /// </summary>
        private DataTable dt = new DataTable();

        /// <summary>
        /// public reference to the ID of the selected flight
        /// </summary>
        private string id;
        /// <summary>
        /// public reference to the ID of the selected flight
        /// </summary>
        public string ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                OnPropertyChanged("ID");
            }
        }
        /// <summary>
        /// private reference to the selected flight number
        /// </summary>
        private string flightNumber;
        /// <summary>
        /// public reference to the selected flight number
        /// </summary>
        public string FlightNumber
        {
            get
            {
                return flightNumber;
            }
            set
            {
                flightNumber = value;
                OnPropertyChanged("FlightNumber");
            }
        }
        /// <summary>
        /// private reference to the type of aircraft
        /// </summary>
        private string aircraftType;
        /// <summary>
        /// public reference to the type of aircraft that is selected
        /// </summary>
        public string AircraftType
        {
            get
            {
                return aircraftType;
            }
            set
            {
                aircraftType = value;
                OnPropertyChanged("AircraftType");
            }
        }

        /// <summary>
        /// Override ToString so that the flight number is displayed.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return FlightNumber + " - " + AircraftType;
        }
    }
}
