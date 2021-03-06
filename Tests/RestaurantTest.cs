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
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=restaurantsByCuisine_test;Integrated Security=SSPI;";
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
      Restaurant Restaurant1 = new Restaurant("Larrys Hoagies",1,"awesome hoagies", 5555555);
      Restaurant Restaurant2 = new Restaurant("Larrys Hoagies",1,"awesome hoagies", 5555555);

      //Assert
      Assert.Equal(Restaurant1.GetName(), Restaurant2.GetName());
    }

    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      //Arrange
      Restaurant testRestaurant = new Restaurant("Larrys Hoagies",1,"awesome hoagies", 5555555);

      //Act
      testRestaurant.Save();
      List<Restaurant> result = Restaurant.GetAll();
      List<Restaurant> testList = new List<Restaurant>{testRestaurant};

      //Assert
      Assert.Equal(testList[0].GetName(), result[0].GetName());
    }
    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
      //Arrange
      Restaurant testRestaurant = new Restaurant("larrys Hoagies",1, "awesome hoagies", 5555555);

      //Act
      testRestaurant.Save();
      Restaurant savedRestaurant = Restaurant.GetAll()[0];

      int result = savedRestaurant.GetCuisineId();
      int testId = testRestaurant.GetCuisineId();

      //Assert
      Assert.Equal(testId, result);
    }
    [Fact]
    public void Test_Find_FindsRestaurantInDatabase()
    {
      //Arrange
      Restaurant testRestaurant = new Restaurant("Larrys Hoagies",1, "awesome hoagies", 5555555);
      testRestaurant.Save();

      //Act
      Restaurant foundRestaurant = Restaurant.Find(testRestaurant.GetId());

      //Assert
      Assert.Equal(testRestaurant.GetName(), foundRestaurant.GetName());
    }
    [Fact]
    public void Test_DeleteAll_DeletesAllRestaurantsInDatabase()
    {
      //Arrange
      RestaurantTest testRestaurant = new RestaurantTest();
      testRestaurant.Dispose();

      //Act
      int result = Restaurant.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_GetByCuisine_GetAllRestaurantsInDatabaseByCuisine()
    {
      //Arrange
      Restaurant testRestaurant = new Restaurant("Larrys Greek",2, "greek food", 5555555);
      testRestaurant.Save();
      Restaurant otherTestRestaurant = new Restaurant("Provencia",2, "better greek food", 7565679);
      testRestaurant.Save();
      otherTestRestaurant.Save();
      List<Restaurant> foundRestaurants = Restaurant.GetByCuisineId(2);
      //Act
      int firstId = foundRestaurants[0].GetCuisineId();
      int secondId = foundRestaurants[1].GetCuisineId();
      //Assert
      Assert.Equal(firstId, secondId);
    }
  }
}
