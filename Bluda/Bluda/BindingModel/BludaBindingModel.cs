using System.Collections.Generic;

namespace ClassLibrary.BindingModel
{
    public class BludaBindingModel
    {
        public int Id { get; set; }
        public string NameBluda { get; set; }
        public string TypeBluda { get; set; }
        public List<ProductBindingModel> ListProduct { get; set; }
    }
}
