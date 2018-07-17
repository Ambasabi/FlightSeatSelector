using assignment_6.ViewModels;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace assignment_6
{
    /// <summary>
    /// Interaction logic for AddPassengerWindow.xaml
    /// </summary>
    public partial class AddPassengerWindow : Window
    {
        /// <summary>
        /// constructor
        /// </summary>
        public AddPassengerWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// handles the close window button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                this.Hide();
                e.Cancel = true;
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// opens a passenger window. assignes the passenger to a tempt passenger. gets the information from the textbox. closes the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveNewPassenger_Click(object sender, RoutedEventArgs e)
        {
            if(txtFirstName.Text == "" || txtLastName.Text == "")
            {                
                MessageBox.Show("Please enter a passenger's first and last name.");
                return;
            }
            PassengerViewModel theNewPassenger = ((MainWindowViewModel)Application.Current.MainWindow.DataContext).TEMP_NEW_PASSENGER;
            theNewPassenger.SFirstName = txtFirstName.Text.Replace("'", "''");
            theNewPassenger.SLastName = txtLastName.Text.Replace("'", "''");

            this.Close();
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)Application.Current.MainWindow.DataContext).areWeAddingPassenger = false;
            this.Close();
            return;
        }

  
    }
}
