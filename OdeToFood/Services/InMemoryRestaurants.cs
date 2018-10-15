using Microsoft.AspNetCore.Mvc;
using OdeToFood.Models;
using OdeToFood.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OdeToFood.Services
{
    public class InMemoryRestaurants: IRestaurantData
    {
        List<Restaurant> _restaurants; // OBS list är inte thread safe.. om många samtiga request görs då kan stora fel uppstå då List<> inte thread safe.
        
        public InMemoryRestaurants()
        {
            _restaurants = new List<Restaurant>
            {
                new Restaurant {Id=1, Name="Hemns Restaurant"},
                new Restaurant {Id=2, Name="Honers Restaurant"},
                new Restaurant {Id=3, Name="Kurdistan Restaurant"}
            };
        }

        public Restaurant Add(Restaurant restaurant)
        {
            restaurant.Id = _restaurants.Max(r => r.Id) + 1;
            _restaurants.Add(restaurant);
            return restaurant;
        }

        public Restaurant Get(int id)
        {
            return _restaurants.FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<Restaurant> GetAll()
        {
            return _restaurants.OrderBy(r => r.Name);
        }

        public Restaurant Update(Restaurant restaurant)
        {
            throw new NotImplementedException();
        }
    }
}
