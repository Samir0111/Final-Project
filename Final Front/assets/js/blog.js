

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


document.addEventListener('DOMContentLoaded', () => {
    const carouselInner = document.querySelector('#newsSlider .carousel-inner');
    const indicators = document.querySelector('#newsSlider .carousel-indicators');
    const slides = carouselInner.querySelectorAll('.carousel-item');

    indicators.innerHTML = '';

    slides.forEach((slide, index) => {
        const button = document.createElement('button');
        button.type = 'button';
        button.dataset.bsTarget = '#newsSlider';
        button.dataset.bsSlideTo = index;
        button.setAttribute('aria-label', `Slide ${index + 1}`);

        if (index === 0) {
            button.classList.add('active');
            button.setAttribute('aria-current', 'true');
        }

        indicators.appendChild(button);
    });
});



      document.addEventListener('DOMContentLoaded', () => {
        const stars = document.querySelectorAll('.star-rating .star');
        const commentInput = document.getElementById('testimonial-comment');
        const usernameInput = document.getElementById('testimonial-username');
        const submitButton = document.getElementById('submit-testimonial');

        const ratingError = document.getElementById('rating-error');
        const commentError = document.getElementById('comment-error');
        const usernameError = document.getElementById('username-error');

        const testimonialContainer = document.getElementById('testimonial-container');

        let selectedRating = 0;

        stars.forEach(star => {
          star.addEventListener('click', function () {
            selectedRating = this.getAttribute('data-value');

            stars.forEach(s => s.classList.remove('selected'));

            for (let i = 0; i < selectedRating; i++) {
              stars[i].classList.add('selected');
            }
            ratingError.classList.add('d-none');
          });
        });

        const validateField = (input, errorElement, errorMessage) => {
          if (!input.value.trim()) {
            errorElement.textContent = errorMessage;
            errorElement.classList.remove('d-none');
            return false;
          } else {
            errorElement.classList.add('d-none');
            return true;
          }
        };

        const validateRating = () => {
          if (selectedRating === 0) {
            ratingError.classList.remove('d-none');
            return false;
          }
          return true;
        };

        submitButton.addEventListener('click', (e) => {
          e.preventDefault();

          const isCommentValid = validateField(commentInput, commentError, 'Comment is required.');
          const isUsernameValid = validateField(usernameInput, usernameError, 'Username is required.');
          const isRatingValid = validateRating();

          if (isCommentValid && isUsernameValid && isRatingValid) {
            testimonialContainer.innerHTML = '<div class="success-message">SENT SUCCESSFULLY!</div>';
          }
        });

        commentInput.addEventListener('input', () => validateField(commentInput, commentError, 'Comment is required.'));
        usernameInput.addEventListener('input', () => validateField(usernameInput, usernameError, 'Username is required.'));
      });