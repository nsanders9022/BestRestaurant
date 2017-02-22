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

        // this will allow multiple tests to run at once
        public void Dispose()
        {
            Restaurant.DeleteAll();
        }
    }
}
