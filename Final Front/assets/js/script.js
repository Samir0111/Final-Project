

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
document.addEventListener("DOMContentLoaded", () => {
    const slides = document.querySelectorAll(".slide");
    const prevArrow = document.querySelector(".arrow-left");
    const nextArrow = document.querySelector(".arrow-right");
    let currentIndex = 0;

    function showSlide(index) {
        slides.forEach((slide, i) => {
            slide.classList.toggle("active", i === index);
        });
        currentIndex = index;
    }

    prevArrow.addEventListener("click", () => {
        const prevIndex = (currentIndex - 1 + slides.length) % slides.length;
        showSlide(prevIndex);
    });

    nextArrow.addEventListener("click", () => {
        const nextIndex = (currentIndex + 1) % slides.length;
        showSlide(nextIndex);
    });

    showSlide(currentIndex);
});



// document.addEventListener("DOMContentLoaded", () => {
//     const foodImages = document.querySelectorAll(".food-img");
//     const lightbox = document.getElementById("lightbox");
//     const lightboxImg = document.getElementById("lightboxImg");
//     const lightboxClose = document.getElementById("lightboxClose");

//     foodImages.forEach((img) => {
//         img.addEventListener("click", () => {
//             const fullSrc = img.getAttribute("data-src");
//             lightboxImg.setAttribute("src", fullSrc);
//             lightbox.classList.add("active");
//         });
//     });

//     lightboxClose.addEventListener("click", () => {
//         lightbox.classList.remove("active");
//     });

//     lightbox.addEventListener("click", (e) => {
//         if (e.target === lightbox) {
//             lightbox.classList.remove("active");
//         }
//     });
// });
