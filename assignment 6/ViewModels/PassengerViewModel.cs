using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace assignment_6.ViewModels
{
    class PassengerViewModel : ViewModelBase
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
        /// private reference to the passengers id
        /// </summary>
        private string id;
        /// <summary>
        /// public reference to the passengers id
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
        /// private reference to the passengers first name
        /// </summary>
        private string sFirstName;

        /// <summary>
        /// public reference to the passengers first name
        /// </summary>
        public string SFirstName
        {
            get
            {
                return sFirstName;
            }
            set
            {
                sFirstName = value;
                OnPropertyChanged("SFirstName");
            }
        }
        /// <summary>
        /// private reference to the passengers last name
        /// </summary>
        private string sLastName;
        /// <summary>
        /// public reference to the passengers last name
        /// </summary>
        public string SLastName
        {
            get
            {
                return sLastName;
            }
            set
            {
                sLastName = value;
                OnPropertyChanged("SLastName");
            }
        }
        /// <summary>
        /// private reference to the passengers seat
        /// </summary>
        private string sSeat;
        /// <summary>
        /// public reference to the passengers seat
        /// </summary>
        public string SSeat
        {
            get
            {
                return sSeat;
            }
            set
            {
                sSeat = value;
                OnPropertyChanged("SSeat");
            }
        }
        /// <summary>
        /// private reference to the passengers flight
        /// </summary>
        private string sFlight;
        /// <summary>
        /// public reference to the passengers flight
        /// </summary>
        public string SFlight
        {
            get
            {
                return sFlight;
            }
            set
            {
                sFlight = value;
                OnPropertyChanged("SFlight");
            }
        }
        /// <summary>
        /// method that will update the cusstomers seat. takes in a steat number, passenger id, and flight ID
        /// </summary>
        /// <param name="SSeat"></param>
        /// <param name="ID"></param>
        /// <param name="SFlight"></param>
        public void ChangePassengerSeat(string SSeat, string ID, string SFlight)
        {
            try
            {
                string sql = "UPDATE Flight_Passenger_Link SET Seat_Number = " + SSeat + " WHERE Passenger_ID = " + ID + " AND Flight_ID = " + SFlight;
                _dataAccess.ExecuteNonQuery(string.Format(sql, SSeat, ID, SFlight));
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }
        /// <summary>
        /// method that inserts a new passenger into the flight passenger link table
        /// </summary>
        /// <param name="flightId"></param>
        private void InsertPassenger(string flightId)
        {
            try
            {
                if (string.IsNullOrEmpty(flightId) || string.IsNullOrEmpty(this.ID) || string.IsNullOrEmpty(this.SSeat))
                {
                    throw new ArgumentNullException("ONE OR MORE ARGS ARE NULL");
                }
                string INSERT_PASSENGER = "INSERT INTO Flight_Passenger_Link(Flight_ID, Passenger_ID, Seat_Number) " + "VALUES( {0} , {1} , {2})";
                _dataAccess.ExecuteNonQuery(string.Format(INSERT_PASSENGER, flightId, this.ID, this.SSeat));
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// overload to string to load combo box with the passengers name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return SFirstName + " " + SLastName;
        }
        /// <summary>
        /// method that completes adding the customer to the db. first we insert them into passenger table. then we select the most recent entry from the database
        /// to get the customers passenger id. then assign the passenger ID to a variable. then insert the passenger pased on flgiht ID
        /// </summary>
        /// <param name="flightId"></param>
        public void AddToDataBase(string flightId)
        {
            try
            {
                //add new dude
                const string SQL_ADD_PASSENGER = "INSERT INTO Passenger (First_Name, Last_Name) VALUES ('{0}', '{1}')";
                _dataAccess.ExecuteNonQuery(string.Format(SQL_ADD_PASSENGER, this.SFirstName, this.SLastName));
                MessageBox.Show(string.Format("{0} {1} has been added as a passenger.", this.SFirstName.Replace("''", "'"), this.SLastName.Replace("''", "'")));

                //grabs the recently entered dudes id
                string GRAB_RECENT_ENTRY = "SELECT TOP 1 Passenger_ID FROM Passenger ORDER BY Passenger_ID DESC";

                int iRet = 0;
                //new data access and dataset object
                clsDataAccess db = new clsDataAccess();
                DataSet ds = db.ExecuteSQLStatement(GRAB_RECENT_ENTRY, ref iRet);

                PassengerViewModel passenger = new PassengerViewModel();
                //grabs the entry from above
                string TopEntry = ds.Tables[0].Rows[0][0].ToString();
                //assigns the id to the actual passenger in the application
                this.ID = TopEntry;
                //inserts the passenger based on flight 
                this.InsertPassenger(flightId);
                //adds the passenger to the appropriate combo box and sets the temp passenger to null
                MainWindowViewModel mwvm = ((MainWindowViewModel)Application.Current.MainWindow.DataContext);
                mwvm.AvailablePassengers.Add(this);
                mwvm.SelectedPassenger = this;
                mwvm.TEMP_NEW_PASSENGER = null;
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// method that deletes the customer from the database.
        /// deletes first from the flight passenger link table by being passed a flight ID and passenger ID. then deletes from the passenger table
        /// using the same ID
        /// </summary>
        /// <param name="flightId"></param>
        public void RemoveFromDatabase(string flightId)
        {
            try
            {
                const string SQL_DELETE_FLIGHTLINK = "DELETE FROM FLIGHT_PASSENGER_LINK " + "WHERE Flight_ID = {0} AND " + "Passenger_Id = {1}";
                const string SQL_DELETE_PASSENGER = "DELETE FROM PASSENGER " + "WHERE Passenger_ID = {0}";
                _dataAccess.ExecuteNonQuery(string.Format(SQL_DELETE_FLIGHTLINK, flightId, this.ID));
                _dataAccess.ExecuteNonQuery(string.Format(SQL_DELETE_PASSENGER, this.ID));
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
   
    }
    
}
