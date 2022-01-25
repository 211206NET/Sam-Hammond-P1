using Xunit;
using Models;
//using StoreWebAPI.Controllers;
using StoreBL;
using StoreDL;

//using CustomExceptions;
using System.Collections.Generic;


namespace Tests;

public class ControllerTests
{
    [Fact]
    public void AddStoreController(){

    Storefront TestStore = new Storefront();
    Assert.NotNull(TestStore);

    }
    [Fact]
    public void AddUser(){

    Customer TestUser = new Customer();
    Assert.NotNull(TestUser);

    }

    [Fact]
    public void StoreShouldSetValidData()
    {
        //Arrange
        Storefront TestStore = new Storefront();
            string Name = "Testname";
            string Address = "UnitTestingAve";
            int StoreID = 4121312;
            
        //Act
        TestStore.Name = Name;
        TestStore.Address = Address;
        TestStore.StoreID = StoreID;

        //Assert
        Assert.Equal(Name, TestStore.Name);
        Assert.Equal(Address, TestStore.Address);
        Assert.Equal(StoreID, TestStore.StoreID);

    }
    [Fact]
    public void UserShouldSetValidData()
    {
        //Arrange
        Customer TestUser = new Customer();
        string name = "testuser";
        string password ="testpassword";
        int customerId = 141234;
        
        
        TestUser.UserName = name;
        TestUser.PassWord = password;
        TestUser.CustomerId = customerId;

        //Assert
        Assert.Equal(name, TestUser.UserName);
        Assert.Equal(password, TestUser.PassWord);
        Assert.Equal(customerId, TestUser.CustomerId);

    }

    

    


    
}


