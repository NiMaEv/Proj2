using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityModels;

namespace ProjCity2
{
    public partial class EditWindow
    {
        private MainWindow mainWindow;
        private MattressObjectV2 mtrObj;

        public EditWindow(MainWindow mainWindow, MattressObjectV2 obj)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;
            mtrObj = obj;

            lbOrderInfo.Content += mtrObj.OrderInfo;
            lbTableName.Content += mtrObj.TableName;
            lbMattressName.Content += mtrObj.Name;
            lbSize.Content += mtrObj.Size;
            lbNumbers.Content += mtrObj.Numbers.ToString();

            this.mainWindow.GetOrdersList().ForEach(str => cmbOrders.Items.Add(str));
            using(PgContext context = new PgContext())
            {
                foreach (var size in context.Sizes)
                    cmbSizes.Items.Add(size);
                foreach (var table in context.Tables)
                    cmbTables.Items.Add(table);
            }
        }

        public void EditObj()
        {
            string orderForCreating;
            if (!(cmbOrders.SelectedItem == null & (txtOrderId.Text.Length == 0 & txtDateOfOrder.Text.Length == 0)))
            {
                if (cmbOrders.SelectedItem != null)
                    orderForCreating = (string)cmbOrders.SelectedItem;
                else
                {
                    if (txtOrderId.Text.Length != 0 & txtDateOfOrder.Text.Length != 0)
                        orderForCreating = txtOrderId.Text + " : " + txtDateOfOrder.Text;
                    else
                        throw new Exception("Поля кода или даты заказа не должны быть пусты.");
                }  
            }
            else
                orderForCreating = mtrObj.OrderInfo;

            string tableNameForCreating;
            if (cmbTables.SelectedItem != null)
                tableNameForCreating = cmbTables.SelectedItem.ToString();
            else
                tableNameForCreating = mtrObj.TableName;

            int lenghtForCreating, widthForCreating;
            if (!(cmbSizes.SelectedItem == null & (txtCustomLenght.Text.Length == 0 & txtCustomWidth.Text.Length == 0)))
            {                
                if (cmbSizes.SelectedItem != null)
                {
                    Sizes tempSize = (Sizes)cmbSizes.SelectedItem;
                    lenghtForCreating = tempSize.lenght;
                    widthForCreating = tempSize.width;
                }
                else
                {
                    if (txtCustomLenght.Text.Length != 0 & txtCustomWidth.Text.Length != 0)
                    {
                        lenghtForCreating = Convert.ToInt32(txtCustomLenght.Text);
                        widthForCreating = Convert.ToInt32(txtCustomWidth.Text);
                    }
                    else
                        throw new Exception("Не указана длинна или ширина матраса.");
                } 
            }
            else
            {
                lenghtForCreating = mtrObj.GetLenght();
                widthForCreating = mtrObj.GetWidth();
            }

            int numbersForCreating;
            if (txtNumbers.Text.Length != 0)
                numbersForCreating = Convert.ToInt32(txtNumbers.Text);
            else
                numbersForCreating = mtrObj.Numbers;

            mainWindow.RemoveGlobalTypesListObject(mtrObj);
            mainWindow.AddObjectInGlobalTypesList(new MattressObjectV2(orderForCreating, tableNameForCreating, mtrObj.Mattresses, lenghtForCreating, widthForCreating, numbersForCreating));
        }
    }
}
