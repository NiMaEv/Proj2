using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModels
{
    public partial class Series
    {
        public override string ToString() => seriesName;

        public override bool Equals(object obj)
        {
            if (obj is Series)
            {
                Series tempObj = (Series)obj;
                if (tempObj.seriesName.Equals(this.seriesName))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
}
