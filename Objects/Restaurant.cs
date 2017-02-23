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
            }
            else
            {
                return 0;
            }
        }

        public string UserViewDelivery()
        {
            if (this._delivery == true)
            {
                return "Yes";
            }
            else
            {
                return "No";
            }
        }

        // get all method for Restaurant list

        public static List<Restaurant> GetAll()
        {
            List<Restaurant> AllRestaurants = new List<Restaurant>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM restaurant ORDER BY name;", conn);
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
        // method to find based on ID
        public static Restaurant Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM restaurant WHERE id = @RestaurantId;", conn);
            SqlParameter restaurantIdParameter = new SqlParameter();
            restaurantIdParameter.ParameterName = "@RestaurantId";
            restaurantIdParameter.Value = id.ToString();
            cmd.Parameters.Add(restaurantIdParameter);
            SqlDataReader rdr = cmd.ExecuteReader();

            int foundRestaurantId = 0;
            string foundRestaurantName = null;
            string foundRestaurantLocation = null;
            bool foundRestaurantDelivery = false;
            int foundRestaurantCuisineId = 0;

            while(rdr.Read())
            {
                foundRestaurantId = rdr.GetInt32(0);
                foundRestaurantName = rdr.GetString(1);
                foundRestaurantLocation = rdr.GetString(2);
                if (rdr.GetByte(3) == 1)
                {
                    foundRestaurantDelivery = true;
                }
                else
                {
                    foundRestaurantDelivery = false;
                }
                foundRestaurantCuisineId = rdr.GetInt32(4);
            }

            Restaurant foundRestaurant = new Restaurant(foundRestaurantName, foundRestaurantLocation, foundRestaurantDelivery, foundRestaurantCuisineId, foundRestaurantId);

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


        // method to search based on name
        public static Restaurant SearchName(string Name)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM restaurant WHERE name = @RestaurantName;", conn);
            SqlParameter restaurantNameParameter = new SqlParameter();
            restaurantNameParameter.ParameterName = "@RestaurantName";
            restaurantNameParameter.Value = Name;
            cmd.Parameters.Add(restaurantNameParameter);
            SqlDataReader rdr = cmd.ExecuteReader();

            int foundRestaurantId = 0;
            string foundRestaurantName = null;
            string foundRestaurantLocation = null;
            bool foundRestaurantDelivery = false;
            int foundRestaurantCuisineId = 0;

            while(rdr.Read())
            {
                foundRestaurantId = rdr.GetInt32(0);
                foundRestaurantName = rdr.GetString(1);
                foundRestaurantLocation = rdr.GetString(2);
                if (rdr.GetByte(3) == 1)
                {
                    foundRestaurantDelivery = true;
                }
                else
                {
                    foundRestaurantDelivery = false;
                }
                foundRestaurantCuisineId = rdr.GetInt32(4);
            }

            Restaurant foundRestaurant = new Restaurant(foundRestaurantName, foundRestaurantLocation, foundRestaurantDelivery, foundRestaurantCuisineId, foundRestaurantId);

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


// method to find cuisine name based on cuisine_id
        public string CuisineName()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT type FROM cuisine WHERE id = @CuisineId;", conn);
            SqlParameter cuisineIdParameter = new SqlParameter();
            cuisineIdParameter.ParameterName = "@CuisineId";
            cuisineIdParameter.Value = this.GetCuisineId().ToString();
            cmd.Parameters.Add(cuisineIdParameter);
            SqlDataReader rdr = cmd.ExecuteReader();

            string cuisineName = "";

            while(rdr.Read())
            {
                cuisineName = rdr[0].ToString();
            }

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return cuisineName;
        }

        // method to search based on name
        public static List<Restaurant> SearchDelivery(int deliveryValue)
        {
            List<Restaurant> deliveryRestaurant = new List<Restaurant>{};
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM restaurant WHERE delivery = @RestaurantDelivery;", conn);
            SqlParameter restaurantNameParameter = new SqlParameter();
            restaurantNameParameter.ParameterName = "@RestaurantDelivery";
            restaurantNameParameter.Value = deliveryValue;
            cmd.Parameters.Add(restaurantNameParameter);
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
                deliveryRestaurant.Add(newRestaurant);
            }
            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return deliveryRestaurant;
        }

// update restaurant name
        public void Update(string newRestaurantName)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE restaurant SET name = @newRestaurantName OUTPUT INSERTED.name WHERE id = @RestaurantId;", conn);

            SqlParameter newRestaurantNameParameter = new SqlParameter("@newRestaurantName", newRestaurantName);
            cmd.Parameters.Add(newRestaurantNameParameter);

            SqlParameter restaurantIdParameter = new SqlParameter("@RestaurantId", this.GetId());
            cmd.Parameters.Add(restaurantIdParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._name = rdr.GetString(0);
            }

            if (rdr !=null)
            {
                rdr.Close();
            }

            if (conn != null)
            {
                conn.Close();
            }
        }

        public void Delete()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM restaurant WHERE id = @RestaurantId;", conn);

            SqlParameter restaurantIdParameter = new SqlParameter("@RestaurantId", this.GetId());

            cmd.Parameters.Add(restaurantIdParameter);
            cmd.ExecuteNonQuery();

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
