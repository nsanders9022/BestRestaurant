using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RestaurantApp
{
    public class CuisineTest : IDisposable
    {
        public CuisineTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=restaurant_test;Integrated Security=SSPI;";
        }

        // this will test that no doubles are created
        [Fact]
        public void Equals_TestIfCuisineEqual_true()
        {
            {
                //Arrange, Act
                Cuisine cuisine1 = new Cuisine("fusion");
                Cuisine cuisine2 = new Cuisine("fusion");

                //Assert
                Assert.Equal(cuisine1, cuisine2);
            }
        }

        [Fact]
        public void GetAll_ReturnAllCuisines_list()
        {
            Cuisine cuisine1 = new Cuisine("fusion");
            Cuisine cuisine2 = new Cuisine("american");
            cuisine1.Save();
            cuisine2.Save();

            List<Cuisine> testCuisineList = new List<Cuisine> {cuisine1, cuisine2};
            List<Cuisine> resultCuisineList = Cuisine.GetAll();
            Assert.Equal(testCuisineList, resultCuisineList);
        }

        // this will test the save method
        [Fact]
        public void Save_TestIfSaved_saved()
        {
            //Arrange

            Cuisine cuisine1 = new Cuisine("american fusion");
            cuisine1.Save();

            List<Cuisine> testCuisineList = new List<Cuisine> {cuisine1};
            List<Cuisine> resultCuisineList = Cuisine.GetAll();

            //Assert
            Assert.Equal(testCuisineList, resultCuisineList);
        }

        // get cuisine based on id
        [Fact]
        public void GetId_TestIfGetId_id()
        {
            //Arrange
            Cuisine cuisine1 = new Cuisine("mexican");
            cuisine1.Save();

            //Act
            Cuisine savedCuisine = Cuisine.GetAll()[0];
            int result = savedCuisine.GetId();
            int testId = cuisine1.GetId();

            //Assert
            Assert.Equal(testId, result);
        }

        // find based on id
        [Fact]
        public void Find_FindCuisineById_Q()
        {
           Cuisine testCuisine = new Cuisine("american");
           testCuisine.Save();

           Cuisine foundCuisine = Cuisine.Find(testCuisine.GetId());
           Assert.Equal(testCuisine, foundCuisine);
        }

        //get all restuarants that belong to a selected cuisine
        [Fact]
        public void GetRestaurantes_RetrievesAllRestaurantsWithinCuisine_list()
        {
            Cuisine testCuisine = new Cuisine("greasy food");
            testCuisine.Save();

            Restaurant restaurant1 = new Restaurant("sudocipe", "seattle", false, testCuisine.GetId());
            Restaurant restaurant2 = new Restaurant("Dough", "seattle", true, testCuisine.GetId());
            restaurant1.Save();
            restaurant2.Save();

            List<Restaurant> testRestaurantList = new List<Restaurant> {restaurant2, restaurant1};
            List<Restaurant> resultRestaurantList = testCuisine.GetRestaurants();

            Assert.Equal(testRestaurantList, resultRestaurantList);
        }

// update cuisine
        [Fact]
        public void Test_Update_UpdateCuisineInData()
        {
            // Arrange
            string oldCuisine = "Mexican";
            Cuisine testCuisine = new Cuisine(oldCuisine);
            testCuisine.Save();
            string newCuisine = "Italian";

            // Act
            testCuisine.Update(newCuisine);

            string result = testCuisine.GetCuisineType();

            // Assert
            Assert.Equal(newCuisine, result);
        }


        //test that category was deleted from the database
        [Fact]
        public void Delete_DeleteFromDatabase_null()
        {
            //Arrange
            string type1 = "German";
            Cuisine testCuisine1 = new Cuisine(type1);
            testCuisine1.Save();

            string type2 = "French";
            Cuisine testCuisine2 = new Cuisine(type2);
            testCuisine2.Save();

            Restaurant testRestaurant1 = new Restaurant("sudocipe", "seattle", false, testCuisine1.GetId());
            Restaurant testRestaurant2 = new Restaurant("Dough", "seattle", true, testCuisine2.GetId());
            testRestaurant1.Save();
            testRestaurant2.Save();

            //Act
            testCuisine1.Delete();
            List<Cuisine> resultCuisine = Cuisine.GetAll();
            List<Cuisine> testCuisineList = new List<Cuisine> {testCuisine2};

            List<Restaurant> resultRestaurant = Restaurant.GetAll();
            List<Restaurant> testRestaurantList = new List<Restaurant> {testRestaurant2};

            //Assert
            Assert.Equal(testCuisineList, resultCuisine);
            Assert.Equal(testRestaurantList, resultRestaurant);

        }

        // search all restaurants by name
        [Fact]
        public void SearchLocation_FindRestaurantByLocation_Restaurant()
        {

            Cuisine testCuisine = new Cuisine("mexican");
            testCuisine.Save();

            Restaurant restaurant1 = new Restaurant("sudocipe", "oregon", true, testCuisine.GetId());
            Restaurant restaurant2 = new Restaurant("Dough", "seattle", true, testCuisine.GetId());
            Restaurant restaurant3 = new Restaurant("Place", "seattle", false, testCuisine.GetId());
            restaurant1.Save();
            restaurant2.Save();
            restaurant3.Save();

            List<Restaurant> testRestaurantList = new List<Restaurant> {restaurant2, restaurant3};
            List<Restaurant> resultRestaurantList = testCuisine.SearchLocation("seattle");

            Assert.Equal(testRestaurantList, resultRestaurantList);
        }

        // this will allow multiple tests to run at once
        public void Dispose()
        {
            Cuisine.DeleteAll();
            Restaurant.DeleteAll();
        }
    }
}
