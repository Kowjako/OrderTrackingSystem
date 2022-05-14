using OrderTrackingSystem.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Services.Interfaces
{
    public interface IMailService
    {
        /// <summary>
        /// Zwraca wiadomości wysłane przez danego nabywcę
        /// </summary>
        /// <param name="senderId">Id nabywcy</param>
        Task<List<MailDTO>> GetSendMailsForCustomer(int senderId);
        /// <summary>
        /// Zwraca wiadomości odebrane przez danego nabywcę
        /// </summary>
        /// <param name="receiverId">Id nabywcy</param>
        Task<List<MailDTO>> GetReceivedMailsForCustomer(int receiverId);
        /// <summary>
        /// Zwraca wiadomości odebrane przez danego sprzedawcę
        /// </summary>
        /// <param name="sellerId">Id sprzedawcy</param>
        Task<List<MailDTO>> GetReceivedMailsForSeller(int sellerId);
        /// <summary>
        /// Zwraca wiadomości wysłane przez danego sprzedawcę
        /// </summary>
        /// <param name="sellerId">Id sprzedawcy</param>
        Task<List<MailDTO>> GetSendMailsForSeller(int sellerId);
        /// <summary>
        /// Zapisanie nowego maila do bazy
        /// </summary>
        /// <param name="mail">Nowa encja wiadomości</param>
        /// <param name="relatedOrders">Powiązane zamówienia</param>
        Task SendMail(MailDTO mail, string[] relatedOrders = null);
        /// <summary>
        /// Metoda do generowania automatycznej wiadomości po wysyłce paczki
        /// </summary>
        /// <param name="receiverId">Id odbiorcy</param>
        /// <param name="sellerId">Id nadawcy</param>
        /// <param name="relatedSend">Numer przesyłki</param>
        Task GenerateAutomaticMessageAfterSend(int receiverId, int sellerId, string relatedSend = null);
        /// <summary>
        /// Metoda do generowania automatycznej wiadmości po założeniu reklamacji
        /// </summary>
        /// <param name="receiverId">Id odibiorcy</param>
        /// <param name="sellerId">Id nadawcy</param>
        /// <param name="orderId">Id przesyłki</param>
        Task SendComplaintMessage(int receiverId, int sellerId, int orderId);
    }
}
