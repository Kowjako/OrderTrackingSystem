using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Services.Interfaces
{
    public interface IComplaintService
    {
        /// <summary>
        /// Metoda ktora zwraca foldery reklamacyjne wykonując dodatkowo kompozycję - foldery podrzędne znajdują się
        /// w kolekcji folderu nadrzędnego
        /// </summary>
        Task<List<ComplaintFolderDTO>> GetComplaintFolders();
        /// <summary>
        /// Metoda ktora zwraca foldery reklamacyjne bez wykonywania kompozycji - foldery podrzedne znajduja się 
        /// bezpośrednio w kolekcji wszystkich folderów
        /// </summary>
        Task<List<ComplaintFolderDTO>> GetComplaintFoldersWithoutComposing();
        /// <summary>
        /// Metoda zwracająca wszystkie reklamacje dla danego nabywcy
        /// </summary>
        /// <param name="customerId">Id użytkownika którego reklamacje chcemy pobrać</param>
        Task<List<ComplaintsDTO>> GetComplaintsForCustomer(int customerId);
        /// <summary>
        /// Metoda ktora zwraca definicje wzorców reklamacyjnych
        /// </summary>
        Task<List<ComplaintDefinitionDTO>> GetComplaintDefinitions();
        /// <summary>
        /// Metoda służy do zapisywania wzorca reklamacyjnego i przypisanie wzorca do określonego folderu
        /// </summary>
        /// <param name="complaint">Wzorzec utworzony</param>
        /// <param name="folder">Folder do którego ma być przypisany wzrozec</param>
        Task SaveComplaintTemplate(ComplaintDefinitionDTO complaint, ComplaintFolderDTO folder);
        /// <summary>
        /// Metoda dodająca nowy folder do folderów reklamacyjnych
        /// </summary>
        /// <param name="name">Nazwa nowego folderu</param>
        /// <param name="parentFolder">Folder nadrzędny w którym ma być umieszczony nowy folder</param>
        Task AddNewFolder(string name, ComplaintFolderDTO parentFolder);
        /// <summary>
        /// Metoda usuwa folder, oraz jeżeli istnieją folderu podrzędne, te foldery również są usuwane
        /// </summary>
        /// <param name="complaintFolder">Folder który ma być usunięty</param>
        Task DeleteWithAncestor(ComplaintFolderDTO complaintFolder);
        /// <summary>
        /// Metoda usuwa folder, a wszystkie folderu podrzędne przenosi do rodzica usuwanego folderu
        /// </summary>
        /// <param name="complaintFolder">Folder do usunięcia</param>
        Task DeleteAndMoveToAncestor(ComplaintFolderDTO complaintFolder);
        /// <summary>
        /// Metoda tworzącą nową reklamację na podstawie zamówienia
        /// </summary>
        /// <param name="complaintDefinitionId">Wzorzec na podstawie którego wygenerowana reklamacja</param>
        /// <param name="orderId">ID zamówienia</param>
        Task RegisterNewComplaint(int complaintDefinitionId, int orderId);
        /// <summary>
        /// Metoda zwracająca reklamacje zgłoszone dla danego sprzedawcy
        /// </summary>
        /// <param name="sellerId">ID sprzedawcy</param>
        Task<List<ComplaintsDTO>> GetComplaintsForSeller(int sellerId);
    }
}
