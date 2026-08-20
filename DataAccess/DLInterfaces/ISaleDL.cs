using System.Collections.Generic;
using SweetBakery.Models;

namespace SweetBakery.DataAccess
{
    public interface ISaleDL
    {
        List<Sale> LoadAll();
        void SaveAll(List<Sale> sales);
    }
}