// Function to hide the preloader
function hidePreloader() {
  document.body.classList.add("preloader-deactive");
}

window.addEventListener('load', function () {
  setTimeout(hidePreloader, 600);
});




// Scroll to the top of the document when the button is clicked
document.querySelector('.scroll-top').addEventListener('click', function () {
  window.scrollTo({
    top: 0,
    behavior: 'smooth'
  });
});


// Initialize Bootstrap Tooltip
var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
  return new bootstrap.Tooltip(tooltipTriggerEl)
})




// Product Details Page

document.querySelectorAll('.thumbnail').forEach(thumbnail => {
  thumbnail.addEventListener('click', function () {
    const src = this.src;
    document.getElementById('modalImage').src = src;
  });
});



//   Scroll To Top Code

window.onscroll = function () {
  const scrollTopButton = document.querySelector('.scroll-top');
  if (document.body.scrollTop > 100 || document.documentElement.scrollTop > 100) {
    scrollTopButton.style.display = "block";
  } else {
    scrollTopButton.style.display = "none";
  }
};


