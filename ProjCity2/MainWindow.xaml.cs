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
            Sizes temp = (Sizes)cmbSizes.SelectedItem;
            txtCustomLenght.Clear();
            txtCustomWidth.Clear();
            cmbSizes.SelectedItem = temp;
        }

        private void LenghtOrWidth_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txt1 = txtCustomLenght.Text;
            string txt2 = txtCustomWidth.Text;
            cmbSizes.SelectedItem = null;
            txtCustomLenght.Text = txt1;
            txtCustomWidth.Text = txt2;
            txtCustomLenght.Select(txtCustomLenght.Text.Length, 0);
            txtCustomWidth.Select(txtCustomWidth.Text.Length, 0);
        }

        private void txtSearcher_TextChanged(object sender, TextChangedEventArgs e)
        {
            listBoxMattressList.Items.Clear();
            if (txtSearcher.Text != null)
            {
                using (PgContext context = new PgContext())
                    foreach (var item in context.Mattresses.Where(m => m.mattressName.StartsWith(txtSearcher.Text)).OrderBy(m => m.mattressName))
                        listBoxMattressList.Items.Add(item);
            }
            else
            {
                using (PgContext context = new PgContext())
                    foreach (var item in context.Mattresses.OrderBy(m => m.mattressName))
                        listBoxMattressList.Items.Add(item);
            }
        }
        #endregion

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
            listBoxTypesList.ScrollIntoView(listBoxTypesList.Items[listBoxTypesList.Items.Count - 1]);
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
            if (listBoxTypesList.Items.Count == 0)
            {
                CreateDocument = CreateTotalOrderDocument;
                cmbTables.IsEnabled = true;
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CreateDocument();
                //wordApp.Print();
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
