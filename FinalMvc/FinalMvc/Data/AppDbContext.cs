namespace FinalMvc.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using FinalMvc.Models;
    using System.Collections.Generic;
    using System;

  
        public class AppDbContext : IdentityDbContext<AppUser>
        {
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Showcasemenu> Showcasemenus { get; set; }

        //public DbSet<SliderImage> SliderImages { get; set; }

        public DbSet<Setting> Settings { get; set; }
        public DbSet<Chefsteam> Chefsteams { get; set; }

        public DbSet<VideoPreview> VideoPreviews { get; set; }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<CoreFeature> CoreFeatures { get; set; }

        public DbSet<AboutSection> AboutSections { get; set; }
        public DbSet<AppointmentSection> AppointmentSections { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Food> Foods { get; set; }

        public DbSet<FoodCategory> FoodCategories { get; set; }


        public DbSet<Table> Tables { get; set; }

        public DbSet<Reservation> Reservations { get; set; }










        //public DbSet<Category> Categories { get; set; }
        //public DbSet<Accessory> Accessories { get; set; }




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
