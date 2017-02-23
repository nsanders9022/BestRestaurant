using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace RestaurantApp
{
    public class Cuisine
    {
        private int _id;
        private string _type;

        public Cuisine(string CuisineType, int Id = 0)
        {
            _id = Id;
            _type = CuisineType;
        }

        public override bool Equals(System.Object otherCuisine)
        {
            if (!(otherCuisine is Cuisine))
            {
                return false;
            }
            else
            {
                Cuisine newCuisine = (Cuisine) otherCuisine;
                bool idEquality = this.GetId() == newCuisine.GetId();
                bool typeEquality = this.GetCuisineType() == newCuisine.GetCuisineType();
                return (idEquality && typeEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.GetCuisineType().GetHashCode();
        }

        public int GetId()
        {
            return _id;
        }

        public string GetCuisineType()
        {
            return _type;
        }

        public void SetCuisineType(string newCuisineType)
        {
            _type = newCuisineType;
        }

        // get all method for cusine list

        public static List<Cuisine> GetAll()
        {
            List<Cuisine> AllCuisines = new List<Cuisine>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM cuisine;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int cuisineId = rdr.GetInt32(0);
                string cuisineCuisineType = rdr.GetString(1);

                Cuisine newCuisine = new Cuisine(cuisineCuisineType, cuisineId);
                AllCuisines.Add(newCuisine);
            }
            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return AllCuisines;
        }
// save method for all cuisines
        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO cuisine (type) OUTPUT INSERTED.id VALUES (@CuisineCuisineType);", conn);

            SqlParameter typeParameter = new SqlParameter("@CuisineCuisineType", this.GetCuisineType());

            cmd.Parameters.Add(typeParameter);

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
        public static Cuisine Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM cuisine WHERE id = @CuisineId;", conn);
            SqlParameter cuisineIdParameter = new SqlParameter();
            cuisineIdParameter.ParameterName = "@CuisineId";
            cuisineIdParameter.Value = id.ToString();
            cmd.Parameters.Add(cuisineIdParameter);
            SqlDataReader rdr = cmd.ExecuteReader();

            int foundCuisineId = 0;
            string foundCuisineCuisineType = null;


            while(rdr.Read())
            {
                foundCuisineId = rdr.GetInt32(0);
                foundCuisineCuisineType = rdr.GetString(1);
            }

            Cuisine foundCuisine = new Cuisine(foundCuisineCuisineType, foundCuisineId);

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return foundCuisine;
        }

        // get all restaurants from a category

        public List<Restaurant> GetRestaurants()
        {
            List<Restaurant> restaurants = new List<Restaurant>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM restaurant WHERE cuisine_id = @CuisineTypeId ORDER BY name;", conn);
            SqlParameter typeIdParameter = new SqlParameter();
            typeIdParameter.ParameterName = "@CuisineTypeId";
            typeIdParameter.Value = this.GetId();
            cmd.Parameters.Add(typeIdParameter);
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
                restaurants.Add(newRestaurant);
            }
            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return restaurants;
        }

    // method to search based on delivery
           public static List<Restaurant> SearchLocation(string Location)
           {
               List<Restaurant> foundResaurantList = new List<Restaurant>{};
               SqlConnection conn = DB.Connection();
               conn.Open();

               SqlCommand cmd = new SqlCommand("SELECT * FROM restaurant WHERE location = @RestaurantLocation;", conn);
               SqlParameter restaurantLocationParameter = new SqlParameter();
               restaurantLocationParameter.ParameterName = "@RestaurantLocation";
               restaurantLocationParameter.Value = Location;
               cmd.Parameters.Add(restaurantLocationParameter);
               SqlDataReader rdr = cmd.ExecuteReader();

               while(rdr.Read())
               {
                   int foundRestaurantId = rdr.GetInt32(0);
                   string foundRestaurantName = rdr.GetString(1);
                   string foundRestaurantLocation = rdr.GetString(2);
                   bool foundRestaurantDelivery;
                   if (rdr.GetByte(3) == 1)
                   {
                       foundRestaurantDelivery = true;
                   }
                   else
                   {
                       foundRestaurantDelivery = false;
                   }
                   int foundRestaurantCuisineId = rdr.GetInt32(4);

                   Restaurant foundRestaurant = new Restaurant(foundRestaurantName, foundRestaurantLocation, foundRestaurantDelivery, foundRestaurantCuisineId, foundRestaurantId);

                   foundResaurantList.Add(foundRestaurant);
               }

               if (rdr != null)
               {
                   rdr.Close();
               }
               if (conn != null)
               {
                   conn.Close();
               }
               return foundResaurantList;
           }

        //Update category name method
        public void Update(string newCuisineType)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE cuisine SET type = @NewCuisineType OUTPUT INSERTED.type WHERE id = @CuisineId;", conn);

            SqlParameter newCuisineTypeParameter = new SqlParameter();
            newCuisineTypeParameter.ParameterName = "@newCuisineType";
            newCuisineTypeParameter.Value = newCuisineType;
            cmd.Parameters.Add(newCuisineTypeParameter);

            SqlParameter cuisineIdParameter = new SqlParameter();
            cuisineIdParameter.ParameterName = "@CuisineId";
            cuisineIdParameter.Value = this.GetId();
            cmd.Parameters.Add(cuisineIdParameter);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._type = rdr.GetString(0);
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

        // method to delete category
        public void Delete()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM cuisine WHERE id = @CuisineId; DELETE FROM restaurant WHERE cuisine_id = @CuisineId;", conn);

            SqlParameter cuisineIdParameter = new SqlParameter();
            cuisineIdParameter.ParameterName = "@CuisineId";
            cuisineIdParameter.Value = this.GetId();

            cmd.Parameters.Add(cuisineIdParameter);
            cmd.ExecuteReader();

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
            SqlCommand cmd = new SqlCommand("DELETE FROM cuisine;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

    }
}
