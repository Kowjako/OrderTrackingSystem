using OrderTrackingSystem.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Services.Interfaces
{
    public interface IConfigurationService
    {
        /// <summary>
        /// Zwraca Id aktualnie zalogowanego użytkownika który utworzył sesję
        /// </summary>
        Task<int> GetCurrentSessionId();
        /// <summary>
        /// Metoda tworząca sesję dla wprowadzonych danych logowania
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="password">Hasło użytkownika</param>
        /// <returns>Zwraca dwa parametry isSuccess - czy logowanie się powiodło, oraz accType - (1 - nabywca, 0 - sprzedawca)</returns>
        Task<(bool isSuccess, bool accType)> MakeSessionForCredentials(string login, string password);
        /// <summary>
        /// Metoda zwraca wszystkie dostępne punkty odbioru
        /// </summary>
        Task<List<PickupDTO>> GetPickupPoints();
        /// <summary>
        /// Metoda generująca unikalny numer dla zamówienia albo wysyłki
        /// </summary>
        string GenerateElementNumber();
        /// <summary>
        /// Zwraca dane dla aktualnego statusu
        /// </summary>
        /// <param name="state">Status szczegóły jakiego chcemy pobrać</param>
        /// <returns>Zwraca krotkę: name - nazwa statusu, description - opis statusu</returns>
        (string name, string description) GetStatusDetails(OrderState state);
        /// <summary>
        /// Zwraca wszystkie statusy przesyłek dostępne w systemie
        /// </summary>
        /// <returns>Zwraca krotkę: string - nazwa statusu, OrderState - status przedstawiony poprzez enum</returns>
        IEnumerable<Tuple<string, OrderState>> GetAllStates();
        /// <summary>
        /// Zwraca predefiniowane procesy dla sprzedawcy
        /// </summary>
        /// <returns>Lista predefiniowanych procesow</returns>
        Task<List<ProcessDTO>> GetAutoProcesses();
        /// <summary>
        /// Metoda dodaje do bazy nowy proces udostępniony sprzedawcom
        /// </summary>
        /// <param name="NewSellerProcess">Encja procesy z informacjami podstawowymi</param>
        /// <param name="_sqlProcessScript">Skrypt do tworzenia procedury</param>
        /// <returns></returns>
        Task AddNewSellerProcess(ProcessDTO NewSellerProcess, string _sqlProcessScript);
    }
}
