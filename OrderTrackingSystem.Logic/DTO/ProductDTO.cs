using OrderTrackingSystem.Logic.HelperClasses.DTOAttributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OrderTrackingSystem.Logic.DTO
{
    #pragma warning disable CS1591
    public sealed class ProductDTO
    {
        [Display(Name = "UniqueName", ResourceType = typeof(Properties.Resources))]
        public string Name { get; set; }

        [MoneyField(2)]
        [Display(Name = "Netto", ResourceType = typeof(Properties.Resources))]
        public decimal PriceNetto { get; set; }

        [PercentageField]
        [Display(Name = "VAT", ResourceType = typeof(Properties.Resources))]
        public string TAX { get; set; }

        [Display(Name = "Seller", ResourceType = typeof(Properties.Resources))]
        public string Seller { get; set; }

        [Display(Name = "Category", ResourceType = typeof(Properties.Resources))]
        public string Category { get; set; }

        [PercentageField]
        [Display(Name = "Discount", ResourceType = typeof(Properties.Resources))]
        public decimal Discount { get; set; }

        #region Non-browsable

        [Browsable(false)]
        public int Id { get; set; }

        [Browsable(false)]
        public int SellerId { get; set; }

        [Browsable(false)]
        public int CategoryId { get; set; }

        #endregion

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
