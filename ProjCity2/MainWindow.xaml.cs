using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public partial class MainWindow : Window
    {
        #region Events Of Input.
        private void cmbSizes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtCustomLenght.Clear();
            txtCustomWidth.Clear();
        }

        private void LenghtOrWidth_TextChanged(object sender, TextChangedEventArgs e) => cmbSizes.SelectedItem = null;

        private void cmbSeries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listBoxMattressList.Items.Clear();
            using (PgContext context = new PgContext())
                foreach (Mattresses mtrs in context.Mattresses)
                {
                    Series selectedSeries = (Series)cmbSeries.SelectedItem;
                    if (selectedSeries.seriesId.Equals(mtrs.seriesId))
                        listBoxMattressList.Items.Add(mtrs);
                }
        }
        #endregion

        //Temp
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
        //Temp

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
                txtCustomLenght.Clear();
                txtCustomWidth.Clear();
                txtNumbers.Clear();
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if(cmbTables.SelectedItem == null)
            {
                cmbTables.IsEnabled = false;
                CreateDocument = CreateMainOrderDocument;
            }
            try { AddMattressObject(); }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                txtCustomLenght.Clear();
                txtCustomWidth.Clear();
                txtNumbers.Clear();
            }
        }
        #endregion

        #region Events Of Function Buttons.
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxTypesList.SelectedItem != null)
            {
                EditWindow editWindow = new EditWindow(this, (MattressObjectV2)listBoxTypesList.SelectedItem);
                editWindow.Show();
            }
            else
                MessageBox.Show("Не выбран объект для изменения.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxTypesList.SelectedItem != null)
            {
                globalTypesList.Remove((MattressObjectV2)listBoxTypesList.SelectedItem);
                listBoxTypesList.Items.Remove(listBoxTypesList.SelectedItem);
            }
            else
                MessageBox.Show("Не выбран объект для удаления.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
        #endregion
    }
}
