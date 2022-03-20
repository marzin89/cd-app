'use strict';

// Variabler
const select = document.getElementById('toggle-search-form');
const nameInput = document.getElementById('search-name-input');
const artistInput = document.getElementById('search-artist-input');

// Händelsehanterare
select.addEventListener('change', toggle)

// Anpassar formuläret beroende på om namn eller artist har valts i dropdown-listan
function toggle() {
    if (select.selectedIndex == 0) {
        nameInput.style.display = 'block';
        artistInput.style.display = 'none';

    } else if (select.selectedIndex == 1) {
        nameInput.style.display = 'none';
        artistInput.style.display = 'block';
    }
}