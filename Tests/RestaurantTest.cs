using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace FavoriteRestaurants
{
  public class RestaurantTest : IDisposable
  {
    public RestaurantTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=restaurant_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      Restaurant.DeleteAll();
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Restaurant.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfNameIsTheSame()
    {
      //Arrange, Act
      Restaurant Restaurant1 = new Restaurant("Larrys Hoagies");
      Restaurant Restaurant2 = new Restaurant("Larrys Hoagies");

      //Assert
      Assert.Equal(Restaurant1.GetName(), Restaurant2.GetName());
    }

    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      //Arrange
      Restaurant testRestaurant = new Restaurant("Mow the lawn");

      //Act
      testRestaurant.Save();
      List<Restaurant> result = Restaurant.GetAll();
      List<Restaurant> testList = new List<Restaurant>{testRestaurant};

      //Assert
      Assert.Equal(testList, result);
    }
  }
}
