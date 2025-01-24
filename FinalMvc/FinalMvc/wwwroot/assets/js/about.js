

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
