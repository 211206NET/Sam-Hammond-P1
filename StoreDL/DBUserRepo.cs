using Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Serilog;
namespace StoreDL;

public class DBUserRepo:IURepo{
    
    private string _connectionString;

public DBUserRepo(string connectionString){
    _connectionString = connectionString;
    //_connectionString = File.ReadAllText("connectionString.txt");
}

public List<Customer> GetAllUsers(){
    List<Customer> GetAllCustomers = new List<Customer>();
    using SqlConnection connection = new SqlConnection(_connectionString);
    
    string UserSelect = "Select * From Customer";
    string storeOrderSelect = "Select * From StoreOrder";
    string CustomerOrderSelect = "Select * From CustomerOrder";
    DataSet UserSet = new DataSet();
    
    //adapters for the tables
    using SqlDataAdapter storeOrderAdapter = new SqlDataAdapter(storeOrderSelect, connection);
    using SqlDataAdapter CustomerOrderAdapter = new SqlDataAdapter(CustomerOrderSelect, connection);
    using SqlDataAdapter CustomerAdapter = new SqlDataAdapter(UserSelect, connection);
    
    //filling each table using the adapters
    storeOrderAdapter.Fill(UserSet,"StoreOrder");
    CustomerOrderAdapter.Fill(UserSet,"CustomerOrder");
    CustomerAdapter.Fill(UserSet, "Customer");
    
    DataTable customerTable = UserSet.Tables["Customer"];
    DataTable CustomerOrderTable = UserSet.Tables["CustomerOrder"];
    DataTable StoreOrderTable = UserSet.Tables["StoreOrder"];
        if(customerTable!= null){   
            foreach(DataRow row in customerTable.Rows){
                
                Customer user = new Customer(row);
                GetAllCustomers.Add(user);
            
                if (CustomerOrderTable != null){
                    user.Cart = CustomerOrderTable.AsEnumerable().Where(r => (int) r["CustomerId"] == user.CustomerId && (int) r["CustomerOrderID"] == 0 ).Select(
                    r => new CustomerOrder(r)
                    ).ToList();
                }
                if (StoreOrderTable != null){
                    user.FinishedOrders = StoreOrderTable.AsEnumerable().Where(r => (int) r["CustomerId"] == user.CustomerId).Select(
                        r => new StoreOrder(r)
                    ).ToList();
                }
                if(CustomerOrderTable != null){
                    foreach(StoreOrder storeOrder in user.FinishedOrders!){
                        storeOrder.Orders = CustomerOrderTable!.AsEnumerable().Where(r => (int) r["CustomerOrderID"] == storeOrder.orderID).Select(
                            r => new CustomerOrder(r)
                        ).ToList();
                        }
                    }
            }
    }
    return GetAllCustomers;
}
/// <summary>
/// Add user takes in info from MainMenu UI and adds it to the database
/// </summary>
/// <param name="userToAdd"></param>
public void AddUser(Customer userToAdd){
        //Establishing new connection
        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();
        
        string sqlCmd = "INSERT INTO Customer (CustomerId, Username, Password) VALUES (@ID, @username, @pass)"; 
        using SqlCommand cmdAddUser= new SqlCommand(sqlCmd, connection);
        //Adding paramaters
        Random rnd = new Random();
        int customerid = rnd.Next(100000);
        cmdAddUser.Parameters.AddWithValue("@ID",customerid);
        cmdAddUser.Parameters.AddWithValue("@username", userToAdd.UserName);
        cmdAddUser.Parameters.AddWithValue("@pass", userToAdd.PassWord);
        //Executing command
        cmdAddUser.ExecuteNonQuery();
        connection.Close();
        Log.Information("new user added to database {username}", userToAdd.UserName);
    }

/// <summary>
/// Get active user grabs the user that is signed in currently so we can use it when assigning product orders and order history. 
/// </summary>
/// <param name="CustomerId"></param>
/// <returns></returns>
    public Customer GetActiveUser(int CustomerId){
    List<Customer> allUsers = GetAllUsers();
        foreach(Customer user in allUsers){
            
            if(user.CustomerId == CustomerId){
                return user;
            }
        }
        // if you don't find the customer make a new one
        return new Customer();
        
    }


    public void AddCustomerOrder(int CustomerId, int productid, int quantity){
        Random rnd = new Random();
        int CustomerOrderID = rnd.Next(100000);
        
        using SqlConnection connection = new SqlConnection(_connectionString);
        Product selectedProduct = GetProductWithID(productid);
        Random rnd2 = new Random();
        int RandomID = rnd2.Next(100000);
        connection.Open();
        string sqlCmd = "INSERT INTO CustomerOrder (CustomerID,StoreID,CustomerOrderID,ProdID,ProdName,Total,Quantity, ID) VALUES (@CustomerID,@storeID,@customorderid,@productid,@ProdName,@totalprice,@quantity, @ID)";
        using SqlCommand cmdcartorder= new SqlCommand(sqlCmd, connection);
        cmdcartorder.Parameters.AddWithValue("@CustomerID", (int)CustomerId);
        cmdcartorder.Parameters.AddWithValue("@storeID", selectedProduct.StoreID);
        cmdcartorder.Parameters.AddWithValue("@customorderid", 0);
        cmdcartorder.Parameters.AddWithValue("@productid", (int)productid);
        cmdcartorder.Parameters.AddWithValue("@ProdName", selectedProduct.ProductName);
        cmdcartorder.Parameters.AddWithValue("@ID", (int)RandomID);
        cmdcartorder.Parameters.AddWithValue("@quantity", (int)quantity);
        decimal TotalPrice = selectedProduct.Price * quantity;
        cmdcartorder.Parameters.AddWithValue("@totalprice", (decimal)TotalPrice);
        
        cmdcartorder.ExecuteNonQuery();
        connection.Close();
        Log.Information("new customer order added to database{customer}{totalprice}",CustomerId,TotalPrice);
    }
    public Product GetProductWithID(int productID){
        string query = "SELECT * From Product WHERE Id = @Id";
        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();
        using SqlCommand cmd = new SqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@Id", productID);
        using SqlDataReader reader = cmd.ExecuteReader();
        Product selectedProduct = new Product();
        if (reader.Read())
        {
            selectedProduct.ItemID = reader.GetInt32(0);
            selectedProduct.ProductName = reader.GetString(1);
            selectedProduct.Description = reader.GetString(2);
            selectedProduct.Price = reader.GetDecimal(3);
            selectedProduct.Quantity = reader.GetInt32(4);
            selectedProduct.StoreID = reader.GetInt32(5);
        }
        connection.Close();
        return selectedProduct;
    }

/// <summary>
/// This is used to take the Customer orderid which is set to 0 by default and update the number to what the store its purchased from so we can refrence it later 
/// </summary>
/// <param name="CustomerID"></param>
/// <param name="CustomerOrderID"></param>
    public void UpdateCustomerOrder(int CustomerID, int CustomerOrderID, int StoreID){
    
    using SqlConnection connection = new SqlConnection(_connectionString);
    connection.Open();
    string sqlEditCmd = $"UPDATE CustomerOrder SET CustomerOrderID = @CustomerOrderID, StoreID = @StoreId, ID = @CustomerOrderID WHERE CustomerID = @CustomerID AND CustomerOrderID = @0";
    using SqlCommand cmdEditProd= new SqlCommand(sqlEditCmd, connection);
    cmdEditProd.Parameters.AddWithValue("@CustomerOrderID", CustomerOrderID);
    cmdEditProd.Parameters.AddWithValue("@CustomerID", CustomerID);
    cmdEditProd.Parameters.AddWithValue("@0", 0);
    cmdEditProd.Parameters.AddWithValue("@StoreId", StoreID);

        cmdEditProd.ExecuteNonQuery();
    connection.Close();
    Log.Information("Customer order has been updated {CustomerOrderID}{CustomerID}",CustomerOrderID,CustomerID);
    
    }

    public Customer GetCustomerID(int CustomerID)
    {
        List<Customer> allcustomerID = GetAllUsers();
        foreach (Customer customer in allcustomerID)
        {
            if (customer.CustomerId == CustomerID)
            {
                return customer;
            }
        }
        return new Customer();
    }
    public Customer GetCustomerUsername(string username)
    {
        List<Customer> allcustomerUsername = GetAllUsers();
        foreach (Customer customer in allcustomerUsername)
        {
            if (customer.UserName == username)
            {
                return customer;
            }
        }
        return new Customer();
    }
  

    public bool CustomerLogin(string Username, string Password){
        
            string checkusername = Username;
            List<Customer> users = GetAllUsers();
            bool exists = false;
            string loginpassword = "";
            foreach(Customer customer in users){
                if(checkusername==customer.UserName){
                    loginpassword= customer.PassWord;
                    exists=true;
                }
            }
            if(exists){
                if(loginpassword == Password){
                return true;
                }
            }
            else{
                return false;
            }
    return false;
    }

    public void Checkout(int CustomerId)
    {
        List<CustomerOrder> shoppingCart = GetAllItems(CustomerId);

        var timeUtc = DateTime.UtcNow;
        var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        string currTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone).ToString();
        double currTimeSeconds = DateTime.Now.Subtract(DateTime.MinValue).TotalSeconds;
        Random rnd = new Random();
        int orderNum = rnd.Next(100000);
        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();
        decimal CustomerTotal = 0;
        int StoreID = 0;
        foreach (CustomerOrder cOrder in shoppingCart)
        {
            UpdateCustomerOrder(CustomerId, orderNum, cOrder.storeID);
            CustomerTotal += cOrder.TotalPrice;
            StoreID = cOrder.storeID;
         }

        string sqlInsertCmd = "INSERT INTO StoreOrder (OrderID, CustomerID, storeID, TotalAmount, OrderDate) VALUES (@orderId, @CustomerId, @storeId, @total, @OrderDate)";
        //Creates the new sql command
        using SqlCommand cmd = new SqlCommand(sqlInsertCmd, connection);

        cmd.Parameters.AddWithValue("@orderId", orderNum);
        cmd.Parameters.AddWithValue("@CustomerId", CustomerId);
        cmd.Parameters.AddWithValue("@storeId", StoreID);
        cmd.Parameters.AddWithValue("@total", CustomerTotal);
        cmd.Parameters.AddWithValue("@OrderDate", currTime);

        cmd.ExecuteNonQuery();
        connection.Close();
        Log.Information("Store Order Added {orderId}{storeId}{total}");
  
    }
    public List<CustomerOrder> GetAllItems(int CustomerId)
    {
        List<CustomerOrder> GetAllCustomerOrders = new List<CustomerOrder>();
        using SqlConnection connection = new SqlConnection(_connectionString);

       
        string CustomerOrderSelect = "Select * From CustomerOrder WHERE CustomerID = CustomerId";
        
        DataSet CustomerOrderSet = new DataSet();
        SqlCommand sCmd = new SqlCommand(CustomerOrderSelect, connection);
        
        sCmd.Parameters.AddWithValue("@CustomerID", CustomerId);
        
        using SqlDataAdapter CustomerOrderAdapter = new SqlDataAdapter(sCmd);

        CustomerOrderAdapter.Fill(CustomerOrderSet, "CustomerOrder");

        DataTable CustomerOrderTable = CustomerOrderSet.Tables["CustomerOrder"];
        if (CustomerOrderTable  != null)
        {
            foreach (DataRow row in CustomerOrderTable.Rows)
            {
                CustomerOrder cOrder = new CustomerOrder(row);
                GetAllCustomerOrders.Add(cOrder);

            }
        }
        return GetAllCustomerOrders;
    }

    public bool IsDuplicate(string username)
    {
        string searchQuery = $"SELECT * FROM Customer WHERE Username= @username";

        using SqlConnection connection = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(searchQuery, connection);
        cmd.Parameters.AddWithValue("@username", username);

        connection.Open();

        using SqlDataReader reader = cmd.ExecuteReader();

        if (reader.HasRows)
        {
            //Query returned something, there exists a record that shares the same username 
            return true;
        }
        //no record was returned. No duplicate record in the db
        return false;
    }

    public List<CustomerOrder> GetAllCustomerOrders(int CustomerID) {

        List<CustomerOrder> shoppingCart = new List<CustomerOrder>();
        string selectProdCmd = "SELECT * FROM ProductOrder WHERE CustomerID = @CustomerID AND storeOrderID = @0";
        SqlConnection connection = new SqlConnection(_connectionString);
        SqlCommand cmd = new SqlCommand(selectProdCmd, connection);
        //Gets the current user's id from the username
        Customer currUser = GetCustomerID(CustomerID);
        int userID = (int)currUser.CustomerId!;
        cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
        cmd.Parameters.AddWithValue("@0", 0);

        DataSet productOrderSet = new DataSet();

        SqlDataAdapter adapter = new SqlDataAdapter(cmd);

        adapter.Fill(productOrderSet, "productOrder");

        DataTable productOrderTable = productOrderSet.Tables["productOrder"]!;
        if (productOrderTable != null)
        {
            //Adds each of the product orders we find to the cart to return
            foreach (DataRow row in productOrderTable.Rows)
            {
                CustomerOrder prodOrder = new CustomerOrder(row);
                shoppingCart.Add(prodOrder);
            }
        }

        return shoppingCart;

        }

    public List<StoreOrder> GetAllStoreOrders(int CustomerID)
    {

        List<StoreOrder> finishedOrders = new List<StoreOrder>();
        string selectssOrderCmd = "SELECT * FROM StoreOrder WHERE CustomerID = @CustomerID";
        string selectcOrderCmd = "SELECT * FROM CustomerOrder";
        SqlConnection connection = new SqlConnection(_connectionString);
        
        SqlCommand cmd1 = new SqlCommand(selectssOrderCmd, connection);
        SqlCommand cmd2 = new SqlCommand(selectcOrderCmd, connection);
        //Gets the current user's id from the username
       
        Customer currUser = GetCustomerID(CustomerID);
        int userID = (int)currUser.CustomerId!;
        cmd1.Parameters.AddWithValue("@CustomerID", CustomerID);
        cmd2.Parameters.AddWithValue("@CustomerID", CustomerID);

        DataSet OrderSet = new DataSet();

        SqlDataAdapter adapter1 = new SqlDataAdapter(cmd1);
        SqlDataAdapter adapter2 = new SqlDataAdapter(cmd2);

        adapter1.Fill(OrderSet, "StoreOrder");
        adapter2.Fill(OrderSet, "CustomerOrder");


        DataTable StoreOrderTable = OrderSet.Tables["StoreOrder"]!;
        DataTable CustomerOrderTable = OrderSet.Tables["CustomerOrder"]!;

        if (StoreOrderTable != null)
        {
            //Adds each of the product orders we find to the cart to return
            foreach (DataRow row in StoreOrderTable.Rows)
            {
                StoreOrder sOrder = new StoreOrder(row);

                finishedOrders.Add(sOrder);
            }
            if (CustomerOrderTable != null)
            {
                foreach (StoreOrder storeOrder in finishedOrders)
                {
                    storeOrder.Orders = CustomerOrderTable!.AsEnumerable().Where(r => (int)r["CustomerOrderID"] == storeOrder.orderID).Select(
                        r => new CustomerOrder(r)
                    ).ToList();
                }
            }
        }
        return finishedOrders;
    }
}



    




/*
    public DeleteCustomer(int CustomerID){

        string customerSelect = GetCustomerID(CustomerID.ToString);
        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        string sqlstoreorderdel = $"DELETE FROM ProductOrder WHERE storeOrderID = @0 AND storeID = @storeID1";
        string sqlproductdel = $"DELETE FROM ProductOrder WHERE storeID = @storeID2";
        string sqlcustomerorderrdel = $"DELETE FROM ProductOrder WHERE storeID = @storeID3";
        string sqlstoredel = $"DELETE FROM ProductOrder WHERE storeID = @storeID4";

        using SqlCommand cmdstoreorder = new SqlCommand(sqlstoreorderdel, connection);
        cmdstoreorder.Parameters.AddWithValue("storeID1", CustomerID);

        using SqlCommand cmdprod = new SqlCommand(sqlproductdel, connection);
        cmdprod.Parameters.AddWithValue("storeID2", CustomerID);

        using SqlCommand cmdcustomerorder = new SqlCommand(sqlcustomerorderrdel, connection);
        cmdcustomerorder.Parameters.AddWithValue("storeID3", CustomerID);

        using SqlCommand cmdstore = new SqlCommand(sqlstoredel, connection);
        cmdstore.Parameters.AddWithValue("storeID4", CustomerID);


        cmdstoreorder.ExecuteNonQuery();
        cmdprod.ExecuteNonQuery();
        cmdcustomerorder.ExecuteNonQuery();
        cmdstoreorder.ExecuteNonQuery();
        connection.Close();
    }
    */