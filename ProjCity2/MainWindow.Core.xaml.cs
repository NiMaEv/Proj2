using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityModels;

namespace ProjCity2
{
    public partial class MainWindow
    {
        private void MattressesListInsert(Mattresses mtrs) => listBoxMattressList.Items.Add(mtrs);

        private void SizesCBInsert(Sizes size) => cmbSizes.Items.Add(size);

        private void SeriesCBInsert(Series series) => cmbSeries.Items.Add(series);
    }
}
