using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace FavoriteRestaurants
{
  public class HomeModule: NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        List<Restaurant> allRestaurants = Restaurant.GetAll();
        return View["index.cshtml", allRestaurants];
      };

      Post["/cuisine"] = _ => {
        Cuisine newcuisine = new Cuisine(Request.Form["cuisine"]);
        newcuisine.Save();
        return View["cuisine.cshtml", newcuisine];
      };

      Get["/delete_all"] = _ => {
        Cuisine.DeleteAll();
        return View["index.cshtml"];
      };

      Post["/add_eats/"] = _ => {
        Restaurant newRestaurant = new Restaurant(Request.Form["restaurant"], Request.Form["cuisine_id"]);
        newRestaurant.Save();
        List<Restaurant> allRestaurants = Restaurant.GetAll();
        return View["rest_results.cshtml", allRestaurants];
      };
    }
  }
}
