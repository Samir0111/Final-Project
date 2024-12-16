

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
