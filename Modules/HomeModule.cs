using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace RestaurantApp
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => {
                List<Cuisine> AllCuisines = Cuisine.GetAll();
                return View["index.cshtml", AllCuisines];
            };

            Get["/restaurants"] =_=> {
                List<Restaurant> AllRestaurants = Restaurant.GetAll();
                return View["restaurants.cshtml", AllRestaurants];
            };

            Get["/cuisines"] =_=> {
                List<Cuisine> AllCuisines = Cuisine.GetAll();
                return View["cuisines.cshtml", AllCuisines];
            };

            Get["/cuisines/new"] =_=> {
                return View["cuisines_form.cshtml"];
            };

            Post["/cuisines/new"] =_=> {
                Cuisine newCuisine = new Cuisine(Request.Form["cuisine-type"]);
                newCuisine.Save();
                return View["success.cshtml"];
            };

            Get["/restaurants/new"] =_=> {
                List<Cuisine> AllCuisines = Cuisine.GetAll();
                return View["restaurant_form.cshtml", allCuisines];
            };

            Post["/restaurants/new"] =_=> {
                Restaurant newRestaurant = new Restaurant(Request.Form["restaurant-name"], Request.Form["restaurant-location"], Request.Form["restaurant-delivery"]);
                newRestaurant.Save();
                return View["success.cshtml"];
            };
        }
    }
}
