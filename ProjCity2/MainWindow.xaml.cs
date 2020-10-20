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
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (PgContext context = new PgContext())
            {
                foreach (Mattresses mtrs in context.Mattresses)
                    MattressesListInsert(mtrs);
                foreach (Sizes size in context.Sizes)
                    SizesCBInsert(size);
                foreach (Series series in context.Series)
                    SeriesCBInsert(series);
                foreach (Tables table in context.Tables)
                    TablesCBInsert(table);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void cmbSizes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void LenghtOrWidth_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    CreateDocument();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            CreateDocument();
        }

        private void cmbSeries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void listBoxMattressList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //MattressObject obj = new MattressObject((Mattresses)listBoxMattressList.SelectedItem, (Sizes)cmbSizes.SelectedItem, null, null, 1);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddMattressObject((Mattresses)listBoxMattressList.SelectedItem, (Sizes)cmbSizes.SelectedItem, txtCustomLenght.Text, txtCustomWidth.Text, txtNumbers.Text, txtOrderId.Text, txtDateOfOrder.Text, (Tables)cmbTables.SelectedItem);
            }
            catch
            {
                MessageBox.Show("Матрас или размер не выбраны.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
