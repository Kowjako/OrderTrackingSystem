using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Services.Interfaces
{
    public interface IProductService
    {
        /// <summary>
        /// Zwraca wszystkie produkty dostępne w systemie
        /// </summary>
        Task<List<ProductDTO>> GetAllProducts();
        /// <summary>
        /// Zapisuje produkty dodane do koszyka i przypisuje koszyk do zamówienia
        /// </summary>
        /// <param name="products">Lista produktrów w koszyku</param>
        /// <param name="orderId">Id zamówienia, którego dotyczy koszyk</param>
        Task SaveOrderProductsForCart(List<CartProductDTO> products, int orderId);
        /// <summary>
        /// Zapisuje produkty dodane do koszyka i przypisuje koszyk do wysyłki
        /// </summary>
        /// <param name="products">Lista produktów w koszyku</param>
        /// <param name="sellId">Id wysyłki, której dotyczy koszyk</param>
        Task SaveSellProductsForCart(List<CartProductDTO> products, int sellId);
        /// <summary>
        /// Metoda pobiera wszystkie bony dostępne aktualnie zalogowanemu użytkownikowi
        /// </summary>
        Task<List<VoucherDTO>> GetVouchersForCurrentCustomer();
        /// <summary>
        /// Metoda zwraca wszystkie kategorie produktów używając kompozycji - kategorie 
        /// podrzędne znajdują sie w kolekcji Child kategorii nadrzędnej
        /// </summary>
        Task<List<CategoryDTO>> GetProductCategories();
        /// <summary>
        /// Metoda zwraca wszystkie kategorie bez kompozycji
        /// </summary>
        Task<List<CategoryDTO>> GetProductSubCategories();
        /// <summary>
        /// Metoda dodaje nowy produkt do bazy danych
        /// </summary>
        /// <param name="product">Nowa encja produktu</param>
        Task SaveNewProduct(Products product);
        /// <summary>
        /// Metoda dodaje nowy produkt do bazy danych
        /// </summary>
        /// <param name="product">Nowa encja produktu</param>
        /// <param name="imageData">Tablica danych obrazku</param>
        /// <returns></returns>
        Task SaveNewProduct(Products product, byte[] imageData);
        /// <summary>
        /// Metoda generuje bony i udostepnia je klientom
        /// </summary>
        /// <param name="voucher">Bon</param>
        /// <param name="customerIds">Identyfikatory klientow</param>
        /// <returns></returns>
        Task GenerateVouchersForCustomer(VoucherDTO voucher, params int[] customerIds);
    }
}
