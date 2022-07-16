using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Services.Interfaces
{
    public interface ICustomerService
    {
        /// <summary>
        /// Zwraca aktualnego nabywcę który utworzył sesję
        /// </summary>
        Task<Customers> GetCurrentCustomer();
        /// <summary>
        /// Zwraca aktualnego sprzedawcę który utworzył sesję
        /// </summary>
        Task<Sellers> GetCurrentSeller();
        /// <summary>
        /// Uaktualnia dane dla nabywcy
        /// </summary>
        /// <param name="customer">Uaktualniona encja nabywcy</param>
        Task UpdateCustomer(Customers customer);
        /// <summary>
        /// Uaktualnia dane dla sprzedawcy
        /// </summary>
        /// <param name="seller">Uaktualniona encja sprzedawcy</param>
        Task UpdateSeller(Sellers seller);
        /// <summary>
        /// Zwraca dane nabywcy określonego poprzez identyfikator
        /// </summary>
        /// <param name="customerId">Id nabywcy</param>
        Task<CustomerDTO> GetCustomer(int customerId);
        /// <summary>
        /// Zwraca dane nabywcy określonego poprzez imię
        /// </summary>
        /// <param name="name">Imię i naziwsko wyszukiwanego nabywcy</param>
        /// <returns></returns>
        Task<CustomerDTO> GetCustomerByName(string name);
        /// <summary>
        /// Zwraca nabywcę określonego poprzez email
        /// </summary>
        /// <param name="email">Email nabywcy</param>
        Task<CustomerDTO> GetCustomerByMail(string email);
        /// <summary>
        /// Zwraca dane sprzedawcy określonego poprzez nazwę
        /// </summary>
        /// <param name="name">Nazwa sklepu (sprzedawcy)</param>
        Task<CustomerDTO> GetSellerByName(string name);
        /// <summary>
        /// Zwraca dane sprzedawcy określnoego poprzez identyfikator
        /// </summary>
        /// <param name="sellerId">Id sprzedawcy</param>
        Task<CustomerDTO> GetSeller(int sellerId);
        /// <summary>
        /// Dodaje nowego nabywcę do bazy danych
        /// </summary>
        /// <param name="customer">Nowa encja nabywcy</param>
        /// <param name="localizationId">Id przypisywanej lokalizacji</param>
        /// <param name="credentials">Krotka (login, password) dane autoryzacyjne</param>
        Task AddNewCustomer(Customers customer, int localizationId, (string login, string password) credentials);
        /// <summary>
        /// Dodaje nowego sprzedawcę do bazy danych
        /// </summary>
        /// <param name="seller">Nowa encja sprzedawcy</param>
        /// <param name="localizationId">Id przypisywanej lokalizacji</param>
        /// <param name="credentials">Krotka (login, password) dane autoryzacyjne</param>
        Task AddNewSeller(Sellers seller, int localizationId, (string login, string password) credentials);
        /// <summary>
        /// Zwraca wszystkich dostepnych klientow
        /// </summary>
        /// <returns></returns>
        Task<List<CustomerDTO>> GetAllCustomers();
    }
}
