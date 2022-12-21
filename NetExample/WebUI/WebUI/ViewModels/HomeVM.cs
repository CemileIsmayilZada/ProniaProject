using Core.Entities;

namespace WebUI.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<SlideItem> SlideItem { get; set; }
        public IEnumerable<ShippingItem> ShippingItem{ get; set; }
    }
}
