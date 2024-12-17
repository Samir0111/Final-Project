

document.addEventListener("DOMContentLoaded", () => {
    const menuToggle = document.getElementById("menuToggle");
    const menuIcon = document.getElementById("menuIcon");
    const closeIcon = document.getElementById("closeIcon");
    const sideMenu = document.getElementById("sideMenu");
    const overlay = document.getElementById("overlay");
    

    // Function to open the menu
    function openMenu() {
        sideMenu.classList.add("active");
        overlay.classList.add("active");
        menuIcon.classList.add("d-none"); // Hide hamburger
        closeIcon.classList.remove("d-none"); // Show close icon
    }

    // Function to close the menu
    function closeMenu() {
        sideMenu.classList.remove("active");
        overlay.classList.remove("active");
        closeIcon.classList.add("d-none"); // Hide close icon
        menuIcon.classList.remove("d-none"); // Show hamburger
    }

    // Open menu on hamburger click
    menuToggle.addEventListener("click", openMenu);

    // Close menu on close icon click
    closeIcon.addEventListener("click", closeMenu);

    // Close menu on overlay click
    overlay.addEventListener("click", closeMenu);
});


document.addEventListener('DOMContentLoaded', function () {
    const accountToggle = document.getElementById('accountToggle');
    const accountDropdown = document.getElementById('accountDropdown');

    accountToggle.addEventListener('click', function (event) {
        event.preventDefault(); // Prevent default link behavior
        accountDropdown.classList.toggle('d-none'); // Toggle the dropdown visibility
    });
});


window.addEventListener("scroll", function () {
    const header = document.querySelector("header");
    if (window.scrollY > 50) { 
        header.classList.add("sticky"); // Snap to top
    } else {
        header.classList.remove("sticky"); // Restore initial spacing
    }
});

//sssssssssssssssssssssssssss


document.addEventListener("DOMContentLoaded", () => {
    const slides = document.querySelectorAll(".slide");
    const sliderNav = document.querySelector(".slider-nav");

    let currentIndex = 0;

    // Generate dots based on slides
    slides.forEach((_, index) => {
        const dot = document.createElement("span");
        dot.classList.add("dot");
        if (index === 0) dot.classList.add("active");
        dot.setAttribute("data-index", index);
        sliderNav.appendChild(dot);
    });

    const dots = document.querySelectorAll(".dot");

    function showSlide(index) {
        slides.forEach((slide, i) => {
            slide.classList.toggle("active", i === index);
            dots[i].classList.toggle("active", i === index);
        });
    }

    // Event Listener for Dots
    dots.forEach((dot) => {
        dot.addEventListener("click", () => {
            currentIndex = parseInt(dot.getAttribute("data-index"));
            showSlide(currentIndex);
        });
    });

    // Initialize first slide
    showSlide(currentIndex);
});
