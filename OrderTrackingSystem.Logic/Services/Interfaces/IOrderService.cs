using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Services.Interfaces
{
    public interface IOrderService
    {
        /// <summary>
        /// Zwraca zamówienia należące do danego nabywcy
        /// </summary>
        /// <param name="customerId">Id nabywcy</param>
        Task<List<OrderDTO>> GetOrdersForCustomer(int customerId);
        /// <summary>
        /// Metoda tworzy nowe zamówienie dla klienta
        /// </summary>
        /// <param name="order">Nowa encja zamówienia</param>
        /// <param name="products">Produkty należące do danego zamówienia</param>
        /// <param name="amountToMinusBalance">Kwota do spisania z konta</param>
        /// <param name="voucher">Bon do zapłaty - parametr nieobowiązkowy</param>
        Task SaveOrder(OrderDTO order, List<CartProductDTO> products, decimal amountToMinusBalance, VoucherDTO voucher = null);
        /// <summary>
        /// Zwraca zamówienia na podstawie unikalnych kodów
        /// </summary>
        /// <param name="codes">Tablica kodów zamówień</param>
        Task<List<Orders>> GetOrdersListByCodes(string[] codes);
        /// <summary>
        /// Zwraca zamówienia dotyczące sprzedawcy określonego poprzez identyfikator
        /// </summary>
        /// <param name="sellerId">Id sprzedawcy</param>
        Task<List<OrderDTO>> GetOrdersFromCompany(int sellerId);
    }
}
