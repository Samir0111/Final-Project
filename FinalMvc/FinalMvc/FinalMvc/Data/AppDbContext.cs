namespace FinalMvc.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using FinalMvc.Models;
    using System.Collections.Generic;
    using System;

  
        public class AppDbContext : IdentityDbContext<AppUser>
        {
        //public DbSet<Slider> Sliders { get; set; }
        //public DbSet<SliderImage> SliderImages { get; set; }

        public DbSet<Setting> Settings { get; set; }


        //public DbSet<Category> Categories { get; set; }
        //public DbSet<Accessory> Accessories { get; set; }

        //public DbSet<AccessoryBullet> AccessoryBullets { get; set; }



        //public DbSet<CategoryImage> CategoryImages { get; set; }

        //public DbSet<Testimonial> Testimonials { get; set; }

        //public DbSet<Advertisement> Advertisements { get; set; }
        //public DbSet<Brand> Brands { get; set; }
        //public DbSet<BlogArticle> BlogArticles { get; set; }

        //public DbSet<News> News { get; set; }
        //public DbSet<BlogNews> BlogNews { get; set; }


        //public DbSet<BlogCategory> BlogCategories { get; set; }

        //public DbSet<ProductImage> ProductImages { get; set; }

        //public DbSet<Product> Products { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> option) : base(option) { }
        }
    

}
