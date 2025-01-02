

document.addEventListener("DOMContentLoaded", () => {
    const menuToggle = document.getElementById("menuToggle");
    const menuIcon = document.getElementById("menuIcon");
    const closeIcon = document.getElementById("closeIcon");
    const sideMenu = document.getElementById("sideMenu");
    const overlay = document.getElementById("overlay");
    

    function openMenu() {
        sideMenu.classList.add("active");
        overlay.classList.add("active");
        menuIcon.classList.add("d-none"); 
        closeIcon.classList.remove("d-none");}

    function closeMenu() {
        sideMenu.classList.remove("active");
        overlay.classList.remove("active");
        closeIcon.classList.add("d-none");
        menuIcon.classList.remove("d-none"); 
    }

    menuToggle.addEventListener("click", openMenu);

    closeIcon.addEventListener("click", closeMenu);

    overlay.addEventListener("click", closeMenu);
});


document.addEventListener('DOMContentLoaded', function () {
    const accountToggle = document.getElementById('accountToggle');
    const accountDropdown = document.getElementById('accountDropdown');

    accountToggle.addEventListener('click', function (event) {
        event.preventDefault(); 
        accountDropdown.classList.toggle('d-none'); 
    });
});


window.addEventListener("scroll", function () {
    const header = document.querySelector("header");
    if (window.scrollY > 50) { 
        header.classList.add("sticky"); 
    } else {
        header.classList.remove("sticky"); 
    }
});

//sssssssssssssssssssssssssss



document.getElementById("subscribe-button").addEventListener("click", function () {
    const emailInput = document.getElementById("newsletter-email").value;
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/; 

    if (emailRegex.test(emailInput)) {
        document.getElementById("newsletter-form").classList.add("d-none");

        document.getElementById("subscription-message").classList.remove("d-none");

        document.getElementById("error-message").classList.add("d-none");
    } else {
        document.getElementById("error-message").classList.remove("d-none");


        document.getElementById("subscription-message").classList.add("d-none");
    }
});
 




//main

    const mealsData = [
      { id: 1, name: "Crispy Fried Chicken", category: "Main Course", price: 13.00, image: "./assets/imgs/foodpichome1.png" },
      { id: 2, name: "Grilled Salmon", category: "Main Course", price: 25.00, image: "./assets/imgs/foodpichome2.png" },
      { id: 3, name: "Chocolate Cake", category: "Desserts", price: 7.50, image: "./assets/imgs/foodpichome3.png" },
      { id: 4, name: "Caesar Salad", category: "Salads", price: 10.00, image: "./assets/imgs/foodpichome4.png" },
      { id: 5, name: "Tomato Soup", category: "Soups", price: 6.00, image: "./assets/imgs/foodpichome5.png" },
      { id: 6, name: "Pancakes", category: "Desserts", price: 8.00, image: "./assets/imgs/foodpichome6.png" },
      { id: 7, name: "Steak", category: "Main Course", price: 30.00, image: "./assets/imgs/foodpichome7.png" },
      { id: 8, name: "Mixed Veg Salad", category: "Salads", price: 9.50, image: "./assets/imgs/foodpichome8.png" },
      { id: 8, name: "Mixed Veg Salad", category: "Salads", price: 9.50, image: "./assets/imgs/foodpichome8.png" },

    ];
  
    let currentPage = 1;
    const itemsPerPage = 4;
  
    function renderMeals(data) {
      const container = document.getElementById('mealsContainer');
      container.innerHTML = '';
      data.forEach(meal => {
        container.innerHTML += `
          <div class="col-lg-6 col-md-6 col-sm-12 d-flex align-items-center mb-4">
            <img src="${meal.image}" alt="${meal.name}" class="food-img me-3">
            <div class="meal-details">
              <h4 class="text-light mb-1">${meal.name}</h4>
              <p class="text-secondary mb-0">${meal.category}</p>
            </div>
            <div class="meal-price ms-auto">
              <span class="new-price bg-warning text-dark py-1 px-3 rounded">$${meal.price.toFixed(2)}</span>
            </div>
          </div>
        `;
      });
    }
  
    function paginateMeals() {
      const pagination = document.getElementById('pagination');
      pagination.innerHTML = '';
      const totalPages = Math.ceil(mealsData.length / itemsPerPage);
  
      for (let i = 1; i <= totalPages; i++) {
        pagination.innerHTML += `<button class="btn btn-outline-light mx-1 ${i === currentPage ? 'active' : ''}" onclick="changePage(${i})">${i}</button>`;
      }
  
      const start = (currentPage - 1) * itemsPerPage;
      const end = start + itemsPerPage;
      renderMeals(mealsData.slice(start, end));
    }
  
    function changePage(page) {
      currentPage = page;
      paginateMeals();
    }
  
    document.getElementById('priceFilter').addEventListener('change', (event) => {
      const filter = event.target.value;
      if (filter === 'low-to-high') {
        mealsData.sort((a, b) => a.price - b.price);
      } else if (filter === 'high-to-low') {
        mealsData.sort((a, b) => b.price - a.price);
      }
      paginateMeals();
    });
  
    paginateMeals();
