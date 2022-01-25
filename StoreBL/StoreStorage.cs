using Models;
using StoreDL;
namespace StoreBL;

public class StoreStorage: ISBL{

    private DBStoreRepo _dl;
    private string _connectionString;
    
    
    public StoreStorage()
    {
        _connectionString = File.ReadAllText("connectionString.txt");
        _dl = new DBStoreRepo(_connectionString);
    }
    public List<Storefront> GetAllStores() 
    {
        return _dl.GetAllStores();
    }

    public void AddStore(Storefront storeToAdd)
    {
        _dl.AddStore(storeToAdd);
    }
    


    public void AddProduct(int StoreIndex, Product ProductToAdd){

    _dl.AddProduct(StoreIndex,ProductToAdd);        
    }   
    

    public List<Product> GetAllProduct(int StoreIndex){

      return _dl.GetAllProduct(StoreIndex);
      
    }
    public void AddStoreOrder(StoreOrder newStoreOrder){
        _dl.AddStoreOrder(newStoreOrder);
    }
    public void UpdateProduct(int ItemID, int Quantity){

        _dl.UpdateProduct(ItemID, Quantity);
    }

    public List<StoreOrder> GetAllOrders(int StoreIndex)
    {
        throw new NotImplementedException();
    }

    public int CreateID()
    {
        throw new NotImplementedException();
    }

    public Storefront GetStoreID(int StoreID){
        return _dl.GetStoreID(StoreID);
    }
    public Product GetProductWithID(int productID){
        return _dl.GetProductWithID(productID);
    }

}