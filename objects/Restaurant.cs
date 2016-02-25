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
    private string _description;
    private int _phoneNumber;

    public Restaurant(string Name, int CuisineId, string Description, int PhoneNumber, int Id = 0)
    {
      _id = Id;
      _name = Name;
      _cuisineId = CuisineId;
      _description = Description;
      _phoneNumber = PhoneNumber;
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

    public string GetDescription()
    {
      return _description;
    }
    public int GetPhoneNumber()
    {
      return _phoneNumber;
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

      SqlCommand cmd = new SqlCommand("INSERT INTO restaurants (name, cuisine_id, description, phone_number) OUTPUT INSERTED.id VALUES (@RestaurantName, @CuisineId, @Description, @PhoneNumber);", conn);

      SqlParameter NameParameter = new SqlParameter();
      NameParameter.ParameterName = "@RestaurantName";
      NameParameter.Value = this.GetName();
      cmd.Parameters.Add(NameParameter);


      SqlParameter CuisineIdParameter = new SqlParameter();
      CuisineIdParameter.ParameterName = "@CuisineId";
      CuisineIdParameter.Value = this.GetCuisineId();
      cmd.Parameters.Add(CuisineIdParameter);

      SqlParameter DescriptionParameter = new SqlParameter();
      DescriptionParameter.ParameterName = "@Description";
      DescriptionParameter.Value = this.GetDescription();
      cmd.Parameters.Add(DescriptionParameter);

      SqlParameter PhoneNumberParameter = new SqlParameter();
      PhoneNumberParameter.ParameterName = "@PhoneNumber";
      PhoneNumberParameter.Value = this.GetPhoneNumber();
      cmd.Parameters.Add(PhoneNumberParameter);

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
        string newDescription = rdr.GetString(3);
        int newPhoneNumber = rdr.GetInt32(4);
        Restaurant newRestaurant = new Restaurant(RestaurantName, newCuisineId, newDescription, newPhoneNumber, RestaurantId);
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
      string foundName = null;
      string foundDescription = null;
      int foundPhoneNumber = 0;
      int foundCuisineId = 0;
      while(rdr.Read())
      {
        foundRestaurantId = rdr.GetInt32(0);
        foundName = rdr.GetString(1);
        foundCuisineId = rdr.GetInt32(2);
        foundDescription = rdr.GetString(3);
        foundPhoneNumber = rdr.GetInt32(4);
      }
      Restaurant foundRestaurant = new Restaurant(foundName, foundCuisineId, foundDescription, foundPhoneNumber,  foundRestaurantId);

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
    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM cuisines WHERE id = @CuisineId; DELETE FROM restaurants WHERE cuisine_id = @CuisineId;", conn);

      SqlParameter cuisineIdParameter = new SqlParameter();
      cuisineIdParameter.ParameterName = "@CuisineId";
      cuisineIdParameter.Value = this.GetId();

      cmd.Parameters.Add(cuisineIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
  }
}
