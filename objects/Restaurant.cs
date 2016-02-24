using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace FavoriteRestaurants
{
  public class Restaurant
  {
    private int _id;
    private string _name;
    private int _cuisineId;

    public Restaurant(string Name, int CuisineId, int Id = 0)
    {
      _id = Id;
      _name = Name;
      _cuisineId = CuisineId;
    }
    public override bool Equals(System.Object otherRestaurant)
    {
      if (!(otherRestaurant is Restaurant))
      {
        return false;
      }
      else
      {
        Restaurant newRestaurant = (Restaurant) otherRestaurant;
        bool NameEquality = (this.GetName() == newRestaurant.GetName());
        bool IdEquality = (this.GetId() == newRestaurant.GetId());
        bool CuisineIdEquality = (this.GetCuisineId() == newRestaurant.GetCuisineId());
        return (NameEquality && IdEquality && CuisineIdEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }
    public int GetCuisineId()
    {
      return _cuisineId;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM restaurants;", conn);
      cmd.ExecuteNonQuery();
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO restaurants (name, cuisine_id) OUTPUT INSERTED.id VALUES (@RestaurantName, @CuisineId);", conn);

      SqlParameter NameParameter = new SqlParameter();
      NameParameter.ParameterName = "@RestaurantName";
      NameParameter.Value = this.GetName();
      cmd.Parameters.Add(NameParameter);


      SqlParameter CuisineIdParameter = new SqlParameter();
      CuisineIdParameter.ParameterName = "@CuisineId";
      CuisineIdParameter.Value = this.GetCuisineId();
      cmd.Parameters.Add(CuisineIdParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static List<Restaurant> GetAll()
    {
      List<Restaurant> allRestaurants = new List<Restaurant>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM restaurants;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int RestaurantId = rdr.GetInt32(0);
        string RestaurantName = rdr.GetString(1);
        int newCuisineId = rdr.GetInt32(2);
        Restaurant newRestaurant = new Restaurant(RestaurantName, RestaurantId, newCuisineId);
        allRestaurants.Add(newRestaurant);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allRestaurants;
    }
    public static Restaurant Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM restaurants WHERE id = @RestaurantId;", conn);
      SqlParameter RestaurantIdParameter = new SqlParameter();
      RestaurantIdParameter.ParameterName = "@RestaurantId";
      RestaurantIdParameter.Value = id.ToString();
      cmd.Parameters.Add(RestaurantIdParameter);
      rdr = cmd.ExecuteReader();

      int foundRestaurantId = 0;
      string foundRestaurantDescription = null;
      while(rdr.Read())
      {
        foundRestaurantId = rdr.GetInt32(0);
        foundRestaurantDescription = rdr.GetString(1);
      }
      Restaurant foundRestaurant = new Restaurant(foundRestaurantDescription, foundRestaurantId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return foundRestaurant;
    }
  }
}
