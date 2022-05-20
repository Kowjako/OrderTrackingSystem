using OrderTrackingSystem.Logic.HelperClasses.DTOAttributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OrderTrackingSystem.Logic.DTO
{
    public sealed class ProductDTO
    {
        [Browsable(false)]
        public int Id { get; set; }

        [Display(Name = "UniqueName", ResourceType = typeof(Properties.Resources))]
        public string Nazwa { get; set; }

        [MoneyField(2)]
        [Display(Name = "Netto", ResourceType = typeof(Properties.Resources))]
        public decimal Netto { get; set; }

        [PercentageField]
        [Display(Name = "VAT", ResourceType = typeof(Properties.Resources))]
        public string VAT { get; set; }

        [Display(Name = "Seller", ResourceType = typeof(Properties.Resources))]
        public string Sprzedawca { get; set; }

        [Display(Name = "Category", ResourceType = typeof(Properties.Resources))]
        public string Kategoria { get; set; }

        [PercentageField]
        [Display(Name = "Discount", ResourceType = typeof(Properties.Resources))]
        public decimal Rabat { get; set; }

        [Browsable(false)]
        public int SellerId { get; set; }
        [Browsable(false)]
        public int CategoryId { get; set; }

        //[Display(Name = "Obrazek")]
        //public string Image { get; set; } = @"C:\Users\123\Desktop\nurofen.jpg";// BitmapFromUri(new Uri());

        //private static BitmapImage BitmapFromUri(Uri source)
        //{
        //    var bitmap = new BitmapImage();
        //    bitmap.BeginInit();
        //    bitmap.UriSource = source;
        //    bitmap.CacheOption = BitmapCacheOption.OnLoad;
        //    bitmap.EndInit();
        //    bitmap.Freeze();
        //    return bitmap;
        //}
    }
}
