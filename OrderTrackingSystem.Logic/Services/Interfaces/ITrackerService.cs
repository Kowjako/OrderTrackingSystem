using OrderTrackingSystem.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Services.Interfaces
{
    public interface ITrackerService
    {
        /// <summary>
        /// Metoda zwraca elementy zamówione i wysłane dla danego nabywcy
        /// </summary>
        /// <param name="customerId">Id nabywcy</param>
        Task<List<TrackableItemDTO>> GetItemsForCustomer(int customerId, int pageNumber = 0);
        /// <summary>
        /// Metoda zwraca wszystkie statusy w których znajdowała się dana przesyłka
        /// </summary>
        /// <param name="orderId">Id zamówienia</param>
        Task<List<ParcelStateDTO>> GetParcelState(int orderId);
        /// <summary>
        /// Metoda dodaje kolejny status dla podanego zamówienia, data ustawiania
        /// nowego statusu jest datą bieżącą
        /// </summary>
        /// <param name="orderId">Id zamówienia</param>
        /// <param name="newState">Nowy status</param>
        /// <returns></returns>
        Task AddNewStateForOrder(int orderId, OrderState newState);
        /// <summary>
        /// Metoda do pobierania kolejnych stron elementów zamówionych i 
        /// wysłanych - korzysta z paginowania
        /// </summary>
        /// <param name="customerId">Id nabywcy</param>
        /// <param name="pageNumber">Numer strony</param>
        /// <returns></returns>
        Task<List<TrackableItemDTO>> FetchNextPage(int customerId, int pageNumber);
    }
}
