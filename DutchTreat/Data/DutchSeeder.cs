using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _ctx;
        private readonly IHostingEnvironment _hosting;
        private readonly UserManager<StoreUser> _userManager;

        public DutchSeeder(DutchContext ctx,IHostingEnvironment hosting,UserManager<StoreUser> userManager)
        { 
            _ctx = ctx;
            _hosting = hosting;
            _userManager = userManager;
        }

        public async Task Seed()
        {
            _ctx.Database.EnsureCreated();

            var user = await _userManager.FindByEmailAsync("Randhir@gmail.com");

            if (user == null)
            {
                user = new StoreUser()
                {
                    FirstName = "Randhir",
                    LastName = "Kumar",
                    UserName = "Randhir@gmail.com",
                    Email = "Randhir@gmail.com"
                };

                var result = await _userManager.CreateAsync(user,"Randho123!");

                if (result != IdentityResult.Success)
                {
                    throw  new InvalidOperationException("Failed to create default User");
                }
            }
            //if (_ctx.Products.Any())
            //{
                var filepath = Path.Combine(_hosting.ContentRootPath,"Data/art.json");
                var json = File.ReadAllText(filepath);
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
                _ctx.Products.AddRange(products);

            var order = new Order()
            {
                OrderDate = DateTime.Now,
                OrderNumber = "12345",
                User = user,
                    Items = new List<OrderItem>
                    {
                        new OrderItem()
                        {
                            Product =products.First(),
                            Quantity = 5,
                            UnitPrice = products.First().Price
                        }
                    }
                };
                _ctx.Orders.Add(order);
                _ctx.SaveChanges();

            //}
        }

    }
}
