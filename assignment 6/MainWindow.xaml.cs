using assignment_6.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace assignment_6
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// declares new dataset to hold return value
        /// </summary>
        DataSet ds = new DataSet(); 
        /// <summary>
        /// creates new data access to db
        /// </summary>
        clsDataAccess db = new clsDataAccess();
        int iRet = 0;

        /// <summary>
        /// statement to load all proper passenger information depending on flight selected
        /// </summary>
        private const string tSQL = "SELECT p.Passenger_ID, First_Name, Last_Name, fp.Flight_ID , Seat_Number from Passenger p " +
        "inner join Flight_Passenger_Link  fp on p.Passenger_ID = fp.Passenger_ID " +
        "where Flight_ID = 1";
        /// <summary>
        /// statement to load all proper passenger information depending on flight selected
        /// </summary>
        private const string sSQL = "SELECT p.Passenger_ID, First_Name, Last_Name, fp.Flight_ID , Seat_Number from Passenger p " +
        "inner join Flight_Passenger_Link  fp on p.Passenger_ID = fp.Passenger_ID " +
        "where Flight_ID = 2";
        /// <summary>
        /// Bool to store whether we are changing seats or not
        /// </summary>
        public bool areWeChangingSeats = false;
        

        /// <summary>
        /// constructor. populates the different flights that are available into the combo box
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            try
            {
                Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

                //create instance of reservations
                clsDataAccess db = new clsDataAccess();
                
                string sSQL;    //Holds an SQL statement
                int iRet = 0;   //Number of return values
                DataSet ds = new DataSet(); //Holds the return values
                FlightViewModel cFlight;

                //sql statment to get flight info
                sSQL = "SELECT Flight_ID, Flight_Number, Aircraft_Type FROM FLIGHT";


                //extract flight info into dataset
                ds = db.ExecuteSQLStatement(sSQL, ref iRet);

                //loop through data and create flight class
                for (int i = 0; i < iRet; i++)
                {
                    cFlight = new FlightViewModel();
                    cFlight.ID = ds.Tables[0].Rows[i][0].ToString();
                    cFlight.FlightNumber = ds.Tables[0].Rows[i][1].ToString();
                    cFlight.AircraftType = ds.Tables[0].Rows[i][2].ToString();

                    FlightCombo.Items.Add(cFlight);

                }
                //disable all controls
                DisableControls();
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        #region Methods
        /// <summary>
        /// Method to disable all buttons except choose flight
        /// </summary>
        private void DisableControls()
        {
            try
            {
                ((MainWindowViewModel)Application.Current.MainWindow.DataContext).AreControlsEnabled = false;
                PassengerCombo.IsEnabled = false;
                ChangeSeatButton.IsEnabled = false;
                DeletePassButton.IsEnabled = false;
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Method to enable all buttons
        /// </summary>
        private void EnableControls()
        {
            try
            {
                ((MainWindowViewModel)Application.Current.MainWindow.DataContext).AreControlsEnabled = true;
                PassengerCombo.IsEnabled = true;
            }
            catch (Exception ex)
            {

                new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }
        /// <summary>
        /// disables the proper controls when changing seats
        /// </summary>
        private void DisableControlsChangeSeat()
        {
            try
            {
                DisableControls();
                DeletePassButton.IsEnabled = false;
                ChangeSeatButton.IsEnabled = false;
            }
            catch (Exception ex)
            {

                new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// enables the proper controls when changing seats
        /// </summary>
        private void EnableControlsChangeSeat()
        {
            try
            {
                EnableControls();
                FlightCombo.IsEnabled = true;
                DeletePassButton.IsEnabled = true;
                ChangeSeatButton.IsEnabled = true;
            }
            catch (Exception ex)
            {

                new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        #endregion

        #region Buttons
        /// <summary>
        /// Handles click event for Add Passenger Button, launches the add passenger window. Disables all controls except selecting a seat that is blue. triggers a bool that
        /// says we are adding a passenger
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddPassButton_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)Application.Current.MainWindow.DataContext).areWeAddingPassenger = true;
            try
            {
                ((MainWindowViewModel)Application.Current.MainWindow.DataContext).TEMP_NEW_PASSENGER = new PassengerViewModel();
                MainWindowViewModel mwvm = ((MainWindowViewModel)Application.Current.MainWindow.DataContext);

           
                AddPassengerWindow AddPassengerW = new AddPassengerWindow();
                AddPassengerW.ShowDialog();
                if (!((MainWindowViewModel)Application.Current.MainWindow.DataContext).areWeAddingPassenger)
                {
                    EnableControls();
                    return;
                }
                MessageBox.Show("Please select a seat.");

                DisableControlsChangeSeat();
                
                if (FlightCombo.SelectedIndex == 0) //airbus
                {
                    foreach (Button superButton in AirbusA380.Children.OfType<Button>())
                    {
                        if (superButton.Background == Brushes.Red || superButton.Background == Brushes.Green)
                        {
                            superButton.IsEnabled = false;
                        }
                    }

                }
                else //boeing
                {
                    foreach (Button superButton in Boeing767.Children.OfType<Button>())
                    {
                        if (superButton.Background == Brushes.Red || superButton.Background == Brushes.Green)
                        {
                            superButton.IsEnabled = false;
                        }
                    }
                }
                ((MainWindowViewModel)Application.Current.MainWindow.DataContext).areWeAddingPassenger = true;
            }
            catch (Exception ex)
            {
                new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// change combo which grid is displayed based on selection. enables controls. fills appropriate passenger combo box.. re-loads information
        /// when the selected flight changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FlightCombo_SelectionChanged(object sender, EventArgs e)
        {
            //Number of return values
            DataSet ds = new DataSet(); //Holds the return values

            try
            {
                MainWindowViewModel mwvm = ((MainWindowViewModel)Application.Current.MainWindow.DataContext);
                string sql = string.Empty;
                //switch statement to see which flight is selected
                switch (FlightCombo.SelectedValue.ToString())
                {
                    //case for airbus
                    case "102 - Airbus A380":
                        AirbusA380.Visibility = System.Windows.Visibility.Visible;
                        Boeing767.Visibility = System.Windows.Visibility.Hidden;
                        FlightViewModel fvm = new FlightViewModel();
                        sql = tSQL;
                        break;
                    //case for boeing
                    case "412 - Boeing 767":
                        AirbusA380.Visibility = System.Windows.Visibility.Hidden;
                        Boeing767.Visibility = System.Windows.Visibility.Visible;
                        sql = sSQL;
                        break;

                    default:
                        break;
                }
                //enables controls
                EnableControls();
                //clears the list of available flights
                mwvm.AvailableFlights.Clear();
                //receives the proper SQL statment from the switch and re-loads the proper flight
                updateButtonColor(sql, mwvm.SelectedFlight);
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// Used to pull the most recent flight information from the database. this keeps all buttons the proper color
        /// </summary>
        /// <param name="sqlStatement"></param>
        /// <param name="TheSelectedFlight"></param>
        private void updateButtonColor(string sqlStatement, FlightViewModel TheSelectedFlight)
        {
            try
            {
                MainWindowViewModel mwvm = ((MainWindowViewModel)Application.Current.MainWindow.DataContext);
                //Pulls in the sql statement that was passed
                ds = db.ExecuteSQLStatement(sqlStatement, ref iRet);
                assignPassengers(iRet, ds);
                //Sets TheGrid to the appropriate flight so we can look through and load the correct colors. If it is not airbus, then boeing
                Grid TheGrid = TheSelectedFlight.AircraftType == "Airbus A380" ? AirbusA380 : Boeing767;
                foreach (Button btn in TheGrid.Children.OfType<Button>())
                {
                    SolidColorBrush theColorWeDoneGonnaUse = null;
                    if (mwvm.AvailablePassengers.Any(x => Convert.ToInt16(x.SSeat) == Convert.ToInt16(btn.Content)))
                    {
                        theColorWeDoneGonnaUse = Brushes.Red;
                    }
                    else
                    {
                        theColorWeDoneGonnaUse = Brushes.Blue;
                    }
                    btn.Background = theColorWeDoneGonnaUse;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Loads the selcted flights passengers into the proper combo box
        /// </summary>
        /// <param name="numberOfPassengers"></param>
        /// <param name="passengerData"></param>
        private void assignPassengers(int numberOfPassengers, DataSet passengerData)
        {
            try
            {
                MainWindowViewModel mwvm = ((MainWindowViewModel)Application.Current.MainWindow.DataContext);
                //save current selected dude's id
                string selectedDudeId = mwvm.SelectedPassenger == null ? string.Empty : mwvm.SelectedPassenger.ID;
                mwvm.AvailablePassengers.Clear();
                //load the passengers
                for (int i = 0; i < numberOfPassengers; i++)
                {
                    PassengerViewModel passenger = new PassengerViewModel();
                    passenger.ID = passengerData.Tables[0].Rows[i][0].ToString();
                    passenger.SFirstName = passengerData.Tables[0].Rows[i]["First_Name"].ToString();
                    passenger.SLastName = passengerData.Tables[0].Rows[i]["Last_Name"].ToString();
                    passenger.SFlight = passengerData.Tables[0].Rows[i][3].ToString();
                    passenger.SSeat = passengerData.Tables[0].Rows[i][4].ToString();
                    mwvm.AvailablePassengers.Add(passenger);
                }
                //restore original selection
                if (!string.IsNullOrEmpty(selectedDudeId))
                {
                    mwvm.SelectedPassenger = mwvm.AvailablePassengers.FirstOrDefault(x => x.ID == selectedDudeId);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// If the airbus flight is selected and a button is clicked, load all controls. highlight all filled spots red. highlight everything else blue
        /// if passenger is selected that exists, change seat to green. change seat back to red when new passenger is clicked
        /// if we are adding a new passenger, grab the flight number and seat number. add them to the database. re-enable the controls that were disabled.
        /// update the button color from the database again to properly display the new passenger. sets their seat to green since they are selected currently
        /// set adding passenger to false again to disable the adding passenger check
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAirbusSeat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindowViewModel mwvm = ((MainWindowViewModel)Application.Current.MainWindow.DataContext);
  
                Button MyButton = (Button)sender;
                //if we are adding a passenger
                if (((MainWindowViewModel)Application.Current.MainWindow.DataContext).areWeAddingPassenger)
                {
                    //if the button clicked is blue
                    if (MyButton.Background == Brushes.Blue)
                    {
                        //grab the passengers seat
                        mwvm.TEMP_NEW_PASSENGER.SSeat = MyButton.Content.ToString();
                        mwvm.TEMP_NEW_PASSENGER.SFlight = mwvm.SelectedFlight.FlightNumber;
                        //pass the flight ID into the add to database method
                        mwvm.TEMP_NEW_PASSENGER.AddToDataBase(((FlightViewModel)FlightCombo.SelectedItem).ID);
                        //enable all buttons again
                        foreach (Button superButton in AirbusA380.Children.OfType<Button>())
                        {
                            if (superButton.Background == Brushes.Red || superButton.Background == Brushes.Green)
                            {
                                superButton.IsEnabled = true;

                            }
                        }
                        //pull from database again to properly refresh. sets selected button to green. enables controls. set adding passenger to false
                        updateButtonColor(tSQL, mwvm.SelectedFlight);
                        MyButton.Background = Brushes.Green;
                        ((MainWindowViewModel)Application.Current.MainWindow.DataContext).AreControlsEnabled = true;
                        PassengerCombo.IsEnabled = true;
                        ((MainWindowViewModel)Application.Current.MainWindow.DataContext).areWeAddingPassenger = false;
                        return;
                    }
                    else
                    {
                        MessageBox.Show("This seat is not available, please try again!");
                    }
                    return;
                }
                //When a passenger is selected and the change seat button is hit
                if (areWeChangingSeats)
                {
                    //if the button is blue
                    if (MyButton.Background == Brushes.Blue)
                    {
                        if (mwvm.SelectedPassenger == null)
                            return;

                        //grabs the passenger ID and flight
                        string currentPassengerId = mwvm.SelectedPassenger.ID;
                        string currentFlightId = mwvm.SelectedPassenger.SFlight;
                        //calls the query to update the table
                        mwvm.SelectedPassenger.ChangePassengerSeat(MyButton.Content.ToString(), currentPassengerId, currentFlightId);
                        //enables the buttons after the change is made
                        foreach (Button superButton in AirbusA380.Children.OfType<Button>())
                        {
                            if (superButton.Background == Brushes.Red || superButton.Background == Brushes.Green)
                            {
                                superButton.IsEnabled = true;

                            }
                        }

                        FlightViewModel SelectedFlightAirbus = mwvm.SelectedFlight;
                        //Pulls the information from the database again to refresh the controls
                        updateButtonColor(tSQL, SelectedFlightAirbus);
                        //changes the button that was clicked to green
                        MyButton.Background = Brushes.Green;
                        //loads the passengers seat into a textbox
                        txtPassengerSeat.Text = MyButton.Content.ToString();
                        this.EnableControlsChangeSeat();
                        //enables the buttons again
                        foreach (Button superButton in AirbusA380.Children.OfType<Button>())
                        {
                            if (superButton.Background == Brushes.Red || superButton.Background == Brushes.Green)
                            {
                                superButton.IsEnabled = true;
                            }
                        }
                    }
                    else
                    {
                        //something bad happened because we got here and we don't have blue
                    }
                    //we are no longer changing seats
                    areWeChangingSeats = false;
                }
                else
                {
                    //enables change seat and delete button
                    ChangeSeatButton.IsEnabled = true;
                    DeletePassButton.IsEnabled = true;
                    //changes all green buttons to red again so we do not have multiple green buttons after selecting multipel passengers
                    foreach (Button superButton in AirbusA380.Children.OfType<Button>())
                    {
                        if (superButton.Background == Brushes.Green)
                        {
                            superButton.Background = Brushes.Red;
                        }
                    }
                    //Loads the passenger into the passenger combo box when a read seat is selected and changes the color to green. displays the seat number
                    string SSeatNumber;
                    PassengerViewModel Passenger = new PassengerViewModel();
                    if (MyButton.Background == Brushes.Red)
                    {
                        MyButton.Background = Brushes.Green;
                        SSeatNumber = MyButton.Content.ToString();

                        for (int i = 0; i < mwvm.AvailablePassengers.Count; i++)
                        {
                            Passenger = mwvm.AvailablePassengers[i];
                            if (SSeatNumber == Passenger.SSeat)
                            {
                                PassengerCombo.SelectedIndex = i;
                                mwvm.SelectedPassenger = mwvm.AvailablePassengers[i];
                                txtPassengerSeat.Text = SSeatNumber;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }//end btnAirbusSeat_Click

        /// <summary>
        /// If the boeing flight is selected and a button is clicked, load all controls. highlight all filled spots red. highlight everything else blue
        /// if passenger is selected that exists, change seat to green. change seat back to red when new passenger is clicked
        /// if we are adding a new passenger, grab the flight number and seat number. add them to the database. re-enable the controls that were disabled.
        /// update the button color from the database again to properly display the new passenger. sets their seat to green since they are selected currently
        /// set adding passenger to false again to disable the adding passenger check
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBoeingSeat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindowViewModel mwvm = ((MainWindowViewModel)Application.Current.MainWindow.DataContext);

                Button MyButton = (Button)sender;
                //if bool are we adding passenger is true
                if (((MainWindowViewModel)Application.Current.MainWindow.DataContext).areWeAddingPassenger)
                {
                    //if the button clicked is blue
                    if (MyButton.Background == Brushes.Blue)
                    {
                        //grab the seat number
                        mwvm.TEMP_NEW_PASSENGER.SSeat = MyButton.Content.ToString();
                        // mwvm.TEMP_NEW_PASSENGER.SFlight = mwvm.SelectedFlight.FlightNumber;
                        //pass the selected flight ID into the add method so an insert can be complete properly
                        mwvm.TEMP_NEW_PASSENGER.AddToDataBase(((FlightViewModel)FlightCombo.SelectedItem).ID);
                        //enable controls again
                        foreach (Button superButton in Boeing767.Children.OfType<Button>())
                        {
                            if (superButton.Background == Brushes.Red || superButton.Background == Brushes.Green)
                            {
                                superButton.IsEnabled = true;

                            }
                        }
                        //enable all controls. refresh the seats from database again. change the new passengers seat to green. sets adding passenger to false
                        updateButtonColor(sSQL, mwvm.SelectedFlight);
                        MyButton.Background = Brushes.Green;
                        ((MainWindowViewModel)Application.Current.MainWindow.DataContext).AreControlsEnabled = true;
                        PassengerCombo.IsEnabled = true;
                        ((MainWindowViewModel)Application.Current.MainWindow.DataContext).areWeAddingPassenger = false;
                        return;
                    }
                    else
                    {
                        MessageBox.Show("This seat is not available, please try again!");
                    }
                    return;
                }
                //When a passenger is selected and the change seat button is hit
                if (areWeChangingSeats)
                {
                    //if the button is blue
                    if (MyButton.Background == Brushes.Blue)
                    {
                        if (mwvm.SelectedPassenger == null)
                            return;

                        //grabs the passenger ID and flight
                        string currentPassengerId = mwvm.SelectedPassenger.ID;
                        string currentFlightId = mwvm.SelectedPassenger.SFlight;
                        //calls the query to update the table
                        mwvm.SelectedPassenger.ChangePassengerSeat(MyButton.Content.ToString(), currentPassengerId, currentFlightId);
                        //enables the buttons after the change is made
                        foreach (Button superButton in Boeing767.Children.OfType<Button>())
                        {
                            if (superButton.Background == Brushes.Red || superButton.Background == Brushes.Green)
                            {
                                superButton.IsEnabled = true;
                            }
                        }
                        FlightViewModel SelectedFlightAirbus = mwvm.SelectedFlight;
                        //Pulls the information from the database again to refresh the controls
                        updateButtonColor(sSQL, SelectedFlightAirbus);
                        //changes the button that was clicked to green
                        MyButton.Background = Brushes.Green;
                        //loads the passengers seat into a textbox
                        txtPassengerSeat.Text = MyButton.Content.ToString();
                        this.EnableControlsChangeSeat();
                        //enables the buttons again
                        foreach (Button superButton in Boeing767.Children.OfType<Button>())
                        {
                            if (superButton.Background == Brushes.Red || superButton.Background == Brushes.Green)
                            {
                                superButton.IsEnabled = true;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select a valid seat");
                    }
                    //we are no longer changing seats
                    areWeChangingSeats = false;
                }
                else
                {
                    //enables change seat and delete button
                    ChangeSeatButton.IsEnabled = true;
                    DeletePassButton.IsEnabled = true;
                    //changes all green buttons to red again so we do not have multiple green buttons after selecting multipel passengers
                    foreach (Button superButton in Boeing767.Children.OfType<Button>())
                    {
                        if (superButton.Background == Brushes.Green)
                        {
                            superButton.Background = Brushes.Red;
                        }
                    }
                    //Loads the passenger into the passenger combo box when a read seat is selected and changes the color to green. displays the seat number
                    string SSeatNumber;
                    PassengerViewModel Passenger = new PassengerViewModel();
                    if (MyButton.Background == Brushes.Red)
                    {
                        MyButton.Background = Brushes.Green;
                        SSeatNumber = MyButton.Content.ToString();

                        for (int i = 0; i < mwvm.AvailablePassengers.Count; i++)
                        {
                            Passenger = mwvm.AvailablePassengers[i];
                            if (SSeatNumber == Passenger.SSeat)
                            {
                                PassengerCombo.SelectedIndex = i;
                                mwvm.SelectedPassenger = mwvm.AvailablePassengers[i];
                                txtPassengerSeat.Text = SSeatNumber;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }//end btnBoeingSeat_Click
    
        /// <summary>
        /// Checks to see which passenger is selected. selects their button and turns it green to show they are selected. loads the seat number into the text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PassengerCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                MainWindowViewModel mwvm = ((MainWindowViewModel)Application.Current.MainWindow.DataContext);

                ChangeSeatButton.IsEnabled = true;
                DeletePassButton.IsEnabled = true;
                if (mwvm.SelectedPassenger != null)
                {
                    txtPassengerSeat.Text = mwvm.SelectedPassenger.SSeat;
                }
                //if flight is airbus
                if (FlightCombo.SelectedIndex == 0)
                {
                    //gets rid of extra green selections to ensure only one is selected at a time
                    foreach (Button superButton in AirbusA380.Children.OfType<Button>())
                    {
                        if (superButton.Background == Brushes.Green)
                        {
                            superButton.Background = Brushes.Red;
                        }
                    }

                    if (mwvm.SelectedPassenger != null)
                    {
                        //Turns the correct button green
                        foreach (Button MyButton in AirbusA380.Children.OfType<Button>())
                        {

                            if (MyButton.Content.ToString() == mwvm.SelectedPassenger.SSeat)
                            {
                                MyButton.Background = Brushes.Green;
                            }
                        }
                    }
                }
                //else if flight is boeing
                else if (FlightCombo.SelectedIndex == 1)
                {
                    //turn extra green back to red
                    foreach (Button superButton in Boeing767.Children.OfType<Button>())
                    {
                        if (superButton.Background == Brushes.Green)
                        {
                            superButton.Background = Brushes.Red;
                        }
                    }

                    if (mwvm.SelectedPassenger != null)
                    {
                        //highlight appropriate seat green
                        foreach (Button MyButton in Boeing767.Children.OfType<Button>())
                        {

                            if (MyButton.Content.ToString() == mwvm.SelectedPassenger.SSeat)
                            {
                                MyButton.Background = Brushes.Green;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// If the change seat button is clicked, disable all buttons except for blue ones. trigger are we changing seats bool to pass into proper
        /// flight click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeSeatButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button MyButton = (Button)sender;
                DisableControlsChangeSeat();
                //if flight is airbus
                if (FlightCombo.SelectedIndex == 0)
                {
                    //disable all buttons except blue
                    foreach (Button superButton in AirbusA380.Children.OfType<Button>())
                    {
                        if (superButton.Background == Brushes.Red || superButton.Background == Brushes.Green)
                        {
                            superButton.IsEnabled = false;
                        }
                    }

                }
                //else flight is boeing
                else
                {
                    //disable all buttons except for blue
                    foreach (Button superButton in Boeing767.Children.OfType<Button>())
                    {
                        if (superButton.Background == Brushes.Red || superButton.Background == Brushes.Green)
                        {
                            superButton.IsEnabled = false;
                        }
                    }
                }
                //bool are we changing seats is true
                areWeChangingSeats = true;
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        } //end changeseatbutton_click

        /// <summary>
        /// gets the selected flight ID and passes it into the removeFromDatabase method. deletes the passenger. updates the buttons to refresh from the database
        /// so all data is current
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeletePassButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindowViewModel mwvm = ((MainWindowViewModel)Application.Current.MainWindow.DataContext);
                //pass flight ID into remove from database function. first deletes them from Flight_Passenger_Link
                mwvm.SelectedPassenger.RemoveFromDatabase(((FlightViewModel)FlightCombo.SelectedItem).ID);
                //Passes in the appropriate passenger and grabs their ID. deletes them from the Passenger table
                mwvm.AvailablePassengers.Remove(((PassengerViewModel)PassengerCombo.SelectedItem));
                //If airbus, refresh all controls with proper SQL statement
                if (FlightCombo.SelectedIndex == 0)
                {
                    updateButtonColor(tSQL, mwvm.SelectedFlight);
                }
                //else boeing. refresh all controls with proper SQL statement
                else
                {
                    updateButtonColor(sSQL, mwvm.SelectedFlight);
                }
                return;
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }//end 
    }
}
