using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModels
{
    public interface IEntityExtensions<T>
    {
        bool CompareTo(T other);

        int GetId();

        void SetId(int value);
    }

    public interface IEntityAdvanceExnensions
    {
        bool GetDependentEntities(out List<Mattresses> list);
    }

    public interface ICopying<T>
    {
        T GetCopy();
    }

    public interface IDuplication<T>
    {
        void Duplicate(T obj);
    }
}
