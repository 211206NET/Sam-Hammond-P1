using Models;
using StoreDL;
namespace StoreBL;
public interface IUBL{

List<Customer> GetAllUsers();
void AddUser(Customer userToAdd);
Customer GetActiveUser(int CustomerId);
void UpdateCustomerOrder(int CustomerID, int CustomerOrderID, int StoreID);
Customer GetCustomerID(int CustomerID);
Customer GetCustomerUsername(string username);
bool CustomerLogin(string Username, string Password);
void AddCustomerOrder(int CustomerId, int productid, int quantity);
public void Checkout(int CustomerId);
public bool IsDuplicate(string username); 
public List<CustomerOrder> GetAllCustomerOrders(int CustomerID);
public List<StoreOrder> GetAllStoreOrders(int CustomerID);
}