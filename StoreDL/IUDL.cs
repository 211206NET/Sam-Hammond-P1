using Models;
namespace StoreDL;
public interface IURepo{


List<Customer> GetAllUsers();
void AddUser(Customer userToAdd);
Customer GetActiveUser(int CustomerId);
void AddCustomerOrder(int CustomerId, CustomerOrder currentcartorder);
void UpdateCustomerOrder(int CustomerID, int CustomerOrderID);
Customer GetCustomerID(int CustomerID);
}