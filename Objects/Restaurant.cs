using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace RestaurantApp
{
    public class Restaurant
    {
        private int _id;
        private string _name;
        private string _location;
        private bool _delivery;
        private int _cuisineId;

        public Restaurant(string Name, string Location, bool Delivery, int CuisineId, int Id = 0)
        {
            _name = Name;
            _location = Location;
            _delivery = Delivery;
            _cuisineId = CuisineId;
            _id = Id;
        }

// ensures no doubles are created in table
        public override bool Equals(System.Object otherResturant)
        {
            if (!(otherResturant is Restaurant))
            {
                return false;
            }
            else
            {
                Restaurant newRestaurant = (Restaurant) otherResturant;
                bool idEquality = (this.GetId() == newRestaurant.GetId());
                bool nameEquality = (this.GetName() == newRestaurant.GetName());
                bool locationEquality = (this.GetLocation() == newRestaurant.GetLocation());
                bool deliveryEquality = (this.GetDelivery() == newRestaurant.GetDelivery());
                bool cuisineIdEquality = (this.GetCuisineId() == newRestaurant.GetCuisineId());

                return (idEquality && nameEquality && locationEquality && deliveryEquality && cuisineIdEquality);
            }
        }
        public override int GetHashCode()
        {
             return this.GetName().GetHashCode();
        }
// get id
        public int GetId()
        {
            return _id;
        }
// get and set name
        public string GetName()
        {
            return _name;
        }

        public void SetName(string Name)
        {
            _name = Name;
        }
// get location
        public string GetLocation()
        {
            return _location;
        }
// get delivery
        public bool GetDelivery()
        {
            return _delivery;
        }
// get cuisine id
        public int GetCuisineId()
        {
            return _cuisineId;
        }
        public int TranslateDelivery()
        {
            if (this._delivery == true)
            {
                return 1;
            } else {
                return 0;
            }
        }

// get all method for Restaurant list

        public static List<Restaurant> GetAll()
        {
            List<Restaurant> AllRestaurants = new List<Restaurant>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM restaurant;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int restaurantId = rdr.GetInt32(0);
                string restaurantName = rdr.GetString(1);
                string restaurantLocation = rdr.GetString(2);
                bool restaurantDelivery;
                if (rdr.GetByte(3) == 1)
                {
                    restaurantDelivery = true;
                }
                else
                {
                    restaurantDelivery = false;
                }
                int restaurantCusineId = rdr.GetInt32(4);
                Restaurant newRestaurant = new Restaurant(restaurantName, restaurantLocation, restaurantDelivery, restaurantCusineId, restaurantId);
                AllRestaurants.Add(newRestaurant);
            }
            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return AllRestaurants;
        }
// save method for each restaurant
        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO restaurant (name, location, delivery, cuisine_id) OUTPUT INSERTED.id VALUES (@RestaurantName, @RestaurantLocation, @RestaurantDelivery, @RestaurantCuisineId);", conn);

            SqlParameter nameParameter = new SqlParameter("@RestaurantName", this.GetName());

            SqlParameter locationParameter = new SqlParameter("@RestaurantLocation", this.GetLocation());

            SqlParameter deliveryParameter = new SqlParameter("@RestaurantDelivery", this.TranslateDelivery());

            SqlParameter cuisineIdParameter = new SqlParameter("@RestaurantCuisineId", this.GetCuisineId());

            cmd.Parameters.Add(nameParameter);
            cmd.Parameters.Add(locationParameter);
            cmd.Parameters.Add(deliveryParameter);
            cmd.Parameters.Add(cuisineIdParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

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
// method to run multiple tests at once
        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM restaurant;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }


    }
}
