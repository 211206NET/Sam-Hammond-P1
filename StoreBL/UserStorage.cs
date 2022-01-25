using Models;
using StoreDL;
namespace StoreBL;

public class UserStorage :IUBL{

    private DBUserRepo _dl;
    private string _connectionString;

    public UserStorage()
    {
        _connectionString = File.ReadAllText("connectionString.txt");
        _dl = new DBUserRepo(_connectionString);
    }
    public List<Customer> GetAllUsers() 
    {
        return _dl.GetAllUsers();
    }

    public void AddUser(Customer UserToAdd)
    {
        _dl.AddUser(UserToAdd);
    }
    public void AddCustomerOrder(int CustomerId, int productid, int quantity){
        _dl.AddCustomerOrder(CustomerId, productid, quantity);
    }
    public void UpdateCustomerOrder(int CustomerID, int orderNum, int StoreID)
    {
        _dl.UpdateCustomerOrder(CustomerID,orderNum, StoreID);
    }
    public Customer GetActiveUser(int CustomerId){
        return _dl.GetActiveUser(CustomerId);
        
    }
    public Customer GetCustomerID(int CustomerID){
        return _dl.GetCustomerID(CustomerID);
    }
    public Customer GetCustomerUsername(string username){
        return _dl.GetCustomerUsername(username);
    }

    public bool CustomerLogin(string Username, string Password){
        return _dl.CustomerLogin(Username,Password);
    }
    public void Checkout(int CustomerId)
    {
        _dl.Checkout(CustomerId);
    }
    
    public bool IsDuplicate(string username)
    {
        return _dl.IsDuplicate(username);
    }
    public List<CustomerOrder> GetAllCustomerOrders(int CustomerID)
    {
        return _dl.GetAllCustomerOrders(CustomerID);
    }
    public List<StoreOrder> GetAllStoreOrders(int CustomerID)
    {
        return _dl.GetAllStoreOrders(CustomerID);
    }
}