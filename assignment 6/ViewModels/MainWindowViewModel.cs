using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment_6.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// bool to store whether we are adding a passenger or not
        /// </summary>
        public bool areWeAddingPassenger = false;
        /// <summary>
        /// private reference to whether controls are disabled or not
        /// </summary>
        private bool areControlsEnabled = false;
        /// <summary>
        /// public reference to whether controls are disabled or not
        /// </summary>
        public bool AreControlsEnabled
        {
            get
            {
                return areControlsEnabled;
            }
            set
            {
                areControlsEnabled = value;
                OnPropertyChanged("AreControlsEnabled");
            }
        }
        /// <summary>
        /// new private reference to the selected flight
        /// </summary>
        private FlightViewModel selectedFlight = new FlightViewModel();
        /// <summary>
        /// public reference to the flight that is selected
        /// </summary>
        public FlightViewModel SelectedFlight
        {
            get
            {
                return selectedFlight;
            }
            set
            {
                selectedFlight = value;
                OnPropertyChanged("SelectedFlight");
            }
        }
        /// <summary>
        /// new private observable collection to hold available flights
        /// </summary>
        private ObservableCollection<FlightViewModel> availableFlights = new ObservableCollection<FlightViewModel>();
        /// <summary>
        /// public reference to available flights observable collection
        /// </summary>
        public ObservableCollection<FlightViewModel> AvailableFlights
        {
            get
            {
                return availableFlights;
            }
            set
            {
                availableFlights = value;
                OnPropertyChanged("AvailableFlights");
            }
        }

        /// <summary>
        /// new private collection to hold available passengers
        /// </summary>
        private ObservableCollection<PassengerViewModel> availablePassengers = new ObservableCollection<PassengerViewModel>();
        /// <summary>
        /// public reference to available passengers observable collection
        /// </summary>
        public ObservableCollection<PassengerViewModel>AvailablePassengers
        {
            get
            {
                return availablePassengers;
            }
            set
            {
                availablePassengers = value;
                OnPropertyChanged("AvailablePassengers");
                OnPropertyChanged("SSeat");
                OnPropertyChanged("SFirstName");
            }
        }

        /// <summary>
        /// creates a new private instance of the selected passenger
        /// </summary>
        private PassengerViewModel selectedPassenger = new PassengerViewModel();
        /// <summary>
        /// public reference to the instance of the selected passenger
        /// </summary>
        public PassengerViewModel SelectedPassenger
        {
            get
            {
                return selectedPassenger;
            }
            set
            {
                selectedPassenger = value;
                OnPropertyChanged("SelectedPassenger");
                OnPropertyChanged("SSeat");
                OnPropertyChanged("SFirstName");
            }
        }

        /// <summary>
        /// defines a publicly accessible temporary new passenger to use when adding a passenger
        /// </summary>
        public PassengerViewModel TEMP_NEW_PASSENGER { get; set; }
    }
}
