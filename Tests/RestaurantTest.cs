using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RestaurantApp
{
    public class RestaurantTest : IDisposable
    {
        public RestaurantTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=restaurant_test;Integrated Security=SSPI;";
        }
        // this will test that no doubles are created
        [Fact]
        public void Equals_TestIfEqual_true()
        {
            {
                //Arrange, Act
                Restaurant restaurant1 = new Restaurant("sudocipe", "seattle", true, 1);
                Restaurant restaurant2 = new Restaurant("sudocipe", "seattle", true, 1);

                //Assert
                Assert.Equal(restaurant1, restaurant2);
            }
        }
        // this will test the get all method
        [Fact]
        public void GetAll_ReturnAllRestuarants_list()
        {
            Restaurant restaurant1 = new Restaurant("sudocipe", "seattle", false, 1);
            Restaurant restaurant2 = new Restaurant("Dough", "seattle", true, 2);
            restaurant1.Save();
            restaurant2.Save();

            List<Restaurant> testRestaurantList = new List<Restaurant> {restaurant1, restaurant2};
            List<Restaurant> resultRestaurantList = Restaurant.GetAll();

            Assert.Equal(testRestaurantList, resultRestaurantList);
        }
        // this will test the save method
        [Fact]
        public void Save_TestIfSaved_True()
        {
            //Arrange
            Restaurant restaurant1 = new Restaurant("sudocipe", "seattle", false, 1);
            restaurant1.Save();

            List<Restaurant> testRestaurantList = new List<Restaurant> {restaurant1};
            List<Restaurant> resultRestaurantList = Restaurant.GetAll();

            //Assert
            Assert.Equal(testRestaurantList, resultRestaurantList);
        }

        // this will test the GetId method
        [Fact]
        public void GetId_TestIfGetId_id()
        {
            //Arrange
            Restaurant restaurant1 = new Restaurant("sudocipe", "seattle", false, 1);
            restaurant1.Save();

            //Act
            Restaurant savedRestaurant = Restaurant.GetAll()[0];
            int result = savedRestaurant.GetId();
            int testId = restaurant1.GetId();

            //Assert
            Assert.Equal(testId, result);
        }

// find based on id
        [Fact]
       public void Find_FindRestaurantById_true()
       {
           Restaurant testRestaurant = new Restaurant("sudocipe", "seattle", false, 1);
           testRestaurant.Save();

           Restaurant foundRestaurant = Restaurant.Find(testRestaurant.GetId());
           Assert.Equal(testRestaurant, foundRestaurant);
       }

        // this will allow multiple tests to run at once
        public void Dispose()
        {
            Restaurant.DeleteAll();
        }
    }
}
