using OrderTrackingSystem.Logic.DTO.Pagination;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OrderTrackingSystem.Presentation.CustomControls
{
    internal sealed class UKDataGrid : DataGrid
    {
        #region Private members

        /// <summary>
        /// Aktualny numer załadowanej strony
        /// </summary>
        private int _actualPage = 0;

        /// <summary>
        /// Ilość rekordów domyślnie pobierana jako jedna strona
        /// </summary>
        private const int _pageSize = 100;

        /// <summary>
        /// Wysokość jednego rekordu
        /// </summary>
        private double _rowHeight = double.NaN;
        private int _lastVisibleRowId = 0;

        #endregion

        #region Properties

        /// <summary>
        /// Typ dowiązanej encji
        /// </summary>
        private Type _entityType;
        public Type EntityType => _entityType ?? (_entityType = ItemsSource.GetType().GetGenericArguments().Single());

        /// <summary>
        /// Ilość rekordów widoczna na jednym Viewporcie
        /// </summary>
        private int _visibleRows;
        public int VisibleRows
        {
            get => _visibleRows; 
            private set
            {
                _visibleRows = ItemsSource.OfType<object>().Count();
            }
        }

        #endregion

        #region Overrided members

        protected override void OnLoadingRow(DataGridRowEventArgs e)
        {
            base.OnLoadingRow(e);
            if(_rowHeight == double.NaN)
            {
                _rowHeight = e.Row.Height;
            }
            _lastVisibleRowId = ((IPagedEntity)e.Row.Item).RowNumber;
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
        }

        #endregion

    }
}
