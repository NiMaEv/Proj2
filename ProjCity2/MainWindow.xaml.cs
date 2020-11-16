using System;
using System.Collections.Generic;
using System.Linq;
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
using EntityModels;

namespace ProjCity2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void cmbSizes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void LenghtOrWidth_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void cmbSeries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CreateDocument();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region Events Of Adding.
        private void listBoxMattressList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                AddMattressObject();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                //txtCustomLenght.Clear();
                //txtCustomWidth.Clear();
                //txtNumbers.Clear();
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddMattressObject();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                //txtCustomLenght.Clear();
                //txtCustomWidth.Clear();
                //txtNumbers.Clear();
            }
        }
        #endregion

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CreateDocument();
                wordApp.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                wordApp.CloseWord();
            }
        }
    }
}
