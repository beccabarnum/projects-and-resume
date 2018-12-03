using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingService.Models;

namespace VendingService.Interfaces
{
    public interface IVendingService
    {
        //Vending
        List<VendingItem> GetVendingItems();
        
        //Category
        int AddCategoryItem(CategoryItem item);
        bool UpdateCategoryItem(CategoryItem item);
        void DeleteCategoryItem(int categoryId);
        CategoryItem GetCategoryItem(int categoryId);
        List<CategoryItem> GetCategoryItems();

        //Inventory
        int AddInventoryItem(InventoryItem item);
        bool UpdateInventoryItem(InventoryItem item);
        void DeleteInventoryItem(int inventoryId);
        InventoryItem GetInventoryItem(int inventoryId);
        List<InventoryItem> GetInventoryItems();

        //Product
        int AddProductItem(ProductItem item);
        bool UpdateProductItem(ProductItem item);
        void DeleteProductItem(int productId);
        ProductItem GetProductItem(int productId);
        List<ProductItem> GetProductItems();

        //VendingTransaction
        int AddTransactionSet(VendingTransaction vendTrans, List<TransactionItem> transItems);
        int AddVendingTransaction(VendingTransaction item);
        VendingTransaction GetVendingTransaction(int id);
        List<VendingTransaction> GetVendingTransactions();

        //TransactionItem
        int AddTransactionItem(TransactionItem item);
        TransactionItem GetTransactionItem(int transactionItemId);
        List<TransactionItem> GetTransactionItems(int vendingTransactionId);
        List<TransactionItem> GetTransactionItems();
        List<TransactionItem> GetTransactionItemsForYear(int year);

    }
}
