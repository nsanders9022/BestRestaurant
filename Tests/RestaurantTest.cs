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

        public void Dispose()
        {
            Restaurant.DeleteAll();
        }
    }
}
