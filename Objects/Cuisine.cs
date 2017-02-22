using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace RestaurantApp
{
    public class Cuisine
    {
        private int _id;
        private string _type;

        public Cuisine(string Type, int Id = 0)
        {
            _id = Id;
            _type = Type;
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
                bool typeEquality = this.GetType() == newCuisine.GetType();
                return (idEquality && typeEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.GetType().GetHashCode();
        }

        public int GetId()
        {
            return _id;
        }

        public string GetType()
        {
            return _type;
        }

        public void SetType(string newType)
        {
            _type = newType;
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
                string cuisineType = rdr.GetString(1);

                Cuisine newCuisine = new Cuisine(cuisineType, cuisineId);
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

            SqlCommand cmd = new SqlCommand("INSERT INTO cuisine (type) OUTPUT INSERTED.id VALUES (@CuisineType);", conn);

            SqlParameter typeParameter = new SqlParameter("@CuisineType", this.GetType());

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
            string foundCuisineType = null;


            while(rdr.Read())
            {
                foundCuisineId = rdr.GetInt32(0);
                foundCuisineType = rdr.GetString(1);
            }

            Cuisine foundCuisine = new Cuisine(foundCuisineType, foundCuisineId);

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
