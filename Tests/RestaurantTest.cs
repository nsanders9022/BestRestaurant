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

            List<Restaurant> testRestaurantList = new List<Restaurant> {restaurant2, restaurant1};
            List<Restaurant> resultRestaurantList = Restaurant.GetAll();
            foreach (Restaurant restaurant in testRestaurantList)
            {
                Console.WriteLine("test: " + restaurant.GetName());

            }
            foreach (Restaurant restaurant in resultRestaurantList)
            {
                Console.WriteLine("result: " + restaurant.GetName());
            }

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

        // search all restaurants by name
        [Fact]
        public void SearchName_FindRestaurantByName_Restaurant()
        {
            Restaurant testRestaurant  = new Restaurant("sudocipe", "seattle", false, 1);
            testRestaurant.Save();

            Restaurant searchedRestaurant = Restaurant.SearchName(testRestaurant.GetName());

            Assert.Equal(testRestaurant, searchedRestaurant);
        }

        [Fact]
        public void CuisineName_GetNameBasedOnId_string()
        {
            Cuisine newCuisine = new Cuisine("indian");
            newCuisine.Save();
            Restaurant testRestaurant  = new Restaurant("sudocipe", "seattle", false, newCuisine.GetId());
            testRestaurant.Save();

            string testString = "indian";
            string resultString = testRestaurant.CuisineName();

            Assert.Equal(testString, resultString);
        }

        [Fact]
        public void Update_UpdateName_null()
        {
            Restaurant testRestaurant  = new Restaurant("sudocipe", "seattle", false, 1);
            testRestaurant.Save();

            string newName = "Jim's";

            testRestaurant.Update(newName);

            string result = testRestaurant.GetName();

            Assert.Equal(newName, result);
        }

        public void Delete_DeleteRestaurantFromDatabase_null()
        {
            Restaurant testRestaurant1 = new Restaurant("sudocipe", "seattle", false, 1);
            Restaurant testRestaurant2 = new Restaurant("Dough", "seattle", true, 1);
            Restaurant testRestaurant3 = new Restaurant("Sugar Cookies", "Portland", true, 1);

            testRestaurant1.Save();
            testRestaurant2.Save();
            testRestaurant3.Save();

            testRestaurant2.Delete();

            List<Restaurant> resultRestaurant = Restaurant.GetAll();
            List<Restaurant> testRestaurantList = new List<Restaurant> {testRestaurant1, testRestaurant3};

            Assert.Equal(resultRestaurant, testRestaurantList);
        }

        public void SearchDelivery_deliveryPlaces_list()
        {
            Restaurant testRestaurant1 = new Restaurant("sudocipe", "seattle", false, 1);
            Restaurant testRestaurant2 = new Restaurant("Dough", "seattle", true, 1);
            Restaurant testRestaurant3 = new Restaurant("Sugar Cookies", "Portland", true, 1);
            Restaurant testRestaurant4 = new Restaurant("Luke's Diner", "Portland", false, 1);
            testRestaurant1.Save();
            testRestaurant2.Save();
            testRestaurant3.Save();
            testRestaurant4.Save();

            List<Restaurant> resultRestaurant = Restaurant.SearchDelivery(1);
            List<Restaurant> testRestaurant = new List<Restaurant>{testRestaurant2, testRestaurant3};
            Assert.Equal(resultRestaurant, testRestaurant);

        }

        // this will allow multiple tests to run at once
        public void Dispose()
        {
            Restaurant.DeleteAll();
        }
    }
}
