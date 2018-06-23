using ClassLibrary.BindingModel;
using System.Threading.Tasks;

namespace ClassLibrary.Interface
{
    public interface IReportService
    {
        Task SaveToPdf(ReportBindingModel mdoel);

    }
}
