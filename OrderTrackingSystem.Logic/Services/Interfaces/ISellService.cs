using OrderTrackingSystem.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Services.Interfaces
{
    public interface ISellService
    {
        /// <summary>
        /// Zwraca wysyłki które zostały wykonane przez danego nabywcę
        /// </summary>
        /// <param name="customerId">Id nabywcy</param>
        Task<List<SellDTO>> GetSellsForCustomer(int customerId);
        /// <summary>
        /// Metoda zapisuje nową wysyłke, oraz zapisuje dane koszyka i przypisuje ten
        /// koszyk do aktualnej wysyłki.
        /// </summary>
        /// <param name="order">Nowa encja wysyłki</param>
        /// <param name="products">Koszyk produktów</param>
        Task SaveSell(SellDTO order, List<CartProductDTO> products);
    }
}
