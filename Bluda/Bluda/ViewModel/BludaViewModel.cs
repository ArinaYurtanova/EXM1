using System.Collections.Generic;

namespace ClassLibrary.ViewModel
{
    public class BludaViewModel
    {
        public int Id { get; set; }
        public List<ProductViewModel> Products { get; set; }
        public string NameBluda { get; set; }
        public string TypeBluda { get; set; }
        public string DateCreate { get; set; }
    }
}
