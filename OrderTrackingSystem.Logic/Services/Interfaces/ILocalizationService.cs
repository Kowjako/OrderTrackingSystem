using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Services.Interfaces
{
    public interface ILocalizationService
    {
        /// <summary>
        /// Zwraca lokalizację określona poprzez identyfikator
        /// </summary>
        /// <param name="id">Id lokalizacji</param>
        Task<LocalizationDTO> GetLocalizationById(int id);
        /// <summary>
        /// Uaktualnia dane lokalizacji
        /// </summary>
        /// <param name="localization">Uaktualniona encja lokalizacji</param>
        Task UpdateLocalization(Localizations localization);
        /// <summary>
        /// Dodaje nową lokalizację do bazy danych
        /// </summary>
        /// <param name="localization">Nowa encja lokalizacji</param>
        Task AddNewLocalization(Localizations localization);
    }
}
