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
        Restaurant newRestaurant = new Restaurant(Request.Form["restaurant"], Request.Form["cuisine_id"], Request.Form["description"], Request.Form["phone_number"]);
        newRestaurant.Save();
        List<Restaurant> allRestaurants = Restaurant.GetAll();
        return View["rest_results.cshtml", allRestaurants];
      };

      Get["cuisine/edit/{id}"] = parameters => {
      Cuisine SelectedCuisine = Cuisine.Find(parameters.id);
      return View["cuisine_edit.cshtml", SelectedCuisine];
      };

      Patch["cuisine/edit/{id}"] = parameters => {
      Cuisine SelectedCuisine = Cuisine.Find(parameters.id);
      SelectedCuisine.Update(Request.Form["cuisine-name"]);
      return View["updated_cuisine.cshtml", SelectedCuisine];
      };

      Delete["/restaurant/delete/{id}"] = parameters => {
      Restaurant SelectedRestaurant = Restaurant.Find(parameters.id);
      SelectedRestaurant.Delete();
      List<Restaurant> allRestaurants = Restaurant.GetAll();
      return View["rest_del_success.cshtml", allRestaurants];
      };

    }
  }
}
