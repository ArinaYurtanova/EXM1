using ClassLibrary.BindingModel;
using ClassLibrary.ViewModel;
using System.Collections.Generic;

namespace ClassLibrary.Interface
{
    public interface IProduct
    {
        void Add(ProductBindingModel model);
        void Delete(int id);
        void Update(ProductBindingModel model);
        ProductViewModel GetElement(int id);
        List<ProductViewModel> GetList();
    }
}
