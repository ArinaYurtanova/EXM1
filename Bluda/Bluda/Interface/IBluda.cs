using ClassLibrary.BindingModel;
using ClassLibrary.ViewModel;
using System.Collections.Generic;

namespace ClassLibrary.Interface
{
    public interface IBluda
    {
        void Add(BludaBindingModel model);
        void Delete(int id);
        void Update(BludaBindingModel model);
        BludaViewModel GetElement(int id);
        List<BludaViewModel> GetList();
        List<BludaViewModel> GetListOrder(ReportBindingModel model);
    }
}
