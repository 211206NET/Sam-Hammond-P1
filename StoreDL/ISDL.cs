using Models;
namespace StoreDL;
public interface ISRepo{
List<Storefront> GetAllStores();

List<Product> GetAllProduct(int StoreID);

void UpdateProduct(int ItemID, int quantityadjust);

List<StoreOrder> GetAllOrders(int StoreIndex);
void AddStore(Storefront StoreToAdd);

void AddProduct(int StoreIndex, Product ProductToAdd);

void AddStoreOrder(StoreOrder newStoreOrder);
    
int CreateID();
Storefront GetStoreID(int StoreID);

}