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
using System.Windows.Shapes;

namespace ProjCity2
{
    public partial class EditWindow : Window
    {
        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EditObj();
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) => this.Close();

        #region Events of Controls.
        private void cmbOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtOrderId.Clear();
            txtDateOfOrder.Clear();
        }

        private void txtOrderIdOrtxtDateOfOrder_TextChanged(object sender, TextChangedEventArgs e)=> cmbOrders.SelectedItem = null;

        private void cmbSizes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtCustomLenght.Clear();
            txtCustomWidth.Clear();
        }

        private void txtCustomLenghtOrtxtCustomWidth_TextChanged(object sender, TextChangedEventArgs e) => cmbSizes.SelectedItem = null;

        #endregion
    }
}
