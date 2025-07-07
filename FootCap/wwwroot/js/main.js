function searchProducts() {
    const searchInput = document.getElementById('searchInput');
    const searchTerm = searchInput.value.toLowerCase();

    const products = document.querySelectorAll('.product');

    if (searchTerm === '') {
        products.forEach(product => {
            product.style.display = 'block';
        });
        return;
    }

    let productFound = false;

    products.forEach(product => {
        const productName = product.querySelector('h3').textContent.toLowerCase();

        if (productName.includes(searchTerm)) {
            product.style.display = 'block';
            product.scrollIntoView({ behavior: 'smooth'});
            productFound = true;
        } else {
            product.style.display = 'none';
        }
    });

    if (!productFound) {
        alert('No Found Product');
        searchInput.value = '';
        products.forEach(product => {
            product.style.display = 'block';
        });
    }
}

const producLink = document.querySelector('.produc');
const allProductSection = document.querySelector('.all-proudect');

if (producLink && allProductSection) {
    producLink.addEventListener('click', function (){
        allProductSection.scrollIntoView({ behavior: 'smooth', block: 'end' });
    });
}


const homeLink = document.querySelector('.home');
const contentSection = document.querySelector('.content');

homeLink.addEventListener('click', function(event) {
    event.preventDefault(); 
    contentSection.scrollIntoView({ behavior: 'smooth', block: 'start' });
});

// New JavaScript for menu toggle
const menuIcon = document.querySelector('.menu-icon');
const closeMenuIcon = document.querySelector('.close-menu-icon');
const sidebarMenu = document.querySelector('.sidebar-menu');
const overlay = document.querySelector('.overlay'); 

// Check if elements exist before adding event listener
if (menuIcon && sidebarMenu && closeMenuIcon) {
    menuIcon.addEventListener('click', () => {
        sidebarMenu.classList.add('active'); // Add 'active' class to show sidebar menu
        if (overlay) {
            overlay.classList.add('active'); // Show overlay
        }
    });

    closeMenuIcon.addEventListener('click', () => {
        sidebarMenu.classList.remove('active'); // Remove 'active' class to hide sidebar menu
        if (overlay) {
            overlay.classList.remove('active'); // Hide overlay
        }
    });

    // Close sidebar menu when clicking outside (on the overlay)
    if (overlay) {
        overlay.addEventListener('click', () => {
            sidebarMenu.classList.remove('active');
            overlay.classList.remove('active');
        });
    }

    // (Optional) Close sidebar menu when clicking on any link inside
    const sidebarLinks = document.querySelectorAll('.nav-links-sidebar ul li a');
    sidebarLinks.forEach(link => {
        link.addEventListener('click', () => {
            sidebarMenu.classList.remove('active');
            overlay.classList.remove('active');
        });
    });
}

// JavaScript for search bar toggle
function toggleSearchBar() {
    const searchBarContent = document.querySelector('.search-bar-content');
    const searchAndIconsContainer = document.querySelector('.search-and-icons'); 
    const searchInputElem = document.getElementById('searchInput');

    if (searchBarContent && searchAndIconsContainer && searchInputElem) {
        searchBarContent.classList.toggle('active');
        searchAndIconsContainer.classList.toggle('search-active'); 

        if (searchBarContent.classList.contains('active')) {
            searchInputElem.focus(); 
        } else {
            searchInputElem.value = ''; 
            searchProducts(); 
        }
    }
}

// Event listener for the main search icon to toggle the search bar
const searchIcon = document.getElementById('searchIcon');
if (searchIcon) {
    searchIcon.addEventListener('click', toggleSearchBar);
}

// Event listener for the search input to trigger searchProducts on keyup
const searchInput = document.getElementById('searchInput');
if (searchInput) {
    searchInput.addEventListener('keyup', (event) => {
        searchProducts(); 
    });
}

