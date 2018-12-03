using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingService.Models;

namespace VendingService.Helpers
{
    public class InventoryManager
    {
        private Dictionary<string, VendingItem> _inventory = new Dictionary<string, VendingItem>();
        private HashSet<int> _rows = new HashSet<int>();
        private HashSet<int> _cols = new HashSet<int>();

        public int RowCount
        {
            get
            {
                return _rows.Count;
            }
        }

        public int ColCount
        {
            get
            {
                return _cols.Count;
            }
        }

        /// <summary>
        /// Helps manage the information needed for a vending machine inventory location
        /// </summary>
        /// <param name="items">List of all the items to be loaded in the vending machine</param>
        public InventoryManager(List<VendingItem> items)
        {
            foreach(var item in items)
            {
                _rows.Add(item.Inventory.Row);
                _cols.Add(item.Inventory.Column);
                _inventory.Add(item.Inventory.Key, item);
            }
        }

        /// <summary>
        /// Returns the vending item for the row and col.
        /// The indexes are 1 based
        /// </summary>
        /// <param name="row">The 1 based row for the item</param>
        /// <param name="col">The 1 based col for the item</param>
        /// <returns>VendingItem</returns>
        public VendingItem GetVendingItem(int row, int col)
        {
            string key = $"{row},{col}";
            return _inventory[key];
        }

        

        /// <summary>
        /// Determines if the product is left in the machine
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public bool IsSlotEmpty(int row, int col)
        {
            return GetVendingItem(row, col).Inventory.Qty <= 0;
        }

    }
}
