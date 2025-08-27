document.addEventListener("DOMContentLoaded", function () {
    const movieRows = Array.from(document.querySelectorAll(".movie-row"));
    const searchBar = document.getElementById("title-search");
    const kidFilter = document.getElementById("kid-filter");
    const genreFilter = document.getElementById("genre-filter");

    function normalizeText(string) {
        if (string == null) {
            return "";
        }
        return string.toLowerCase().normalize("NFD").replace(/[\u0300-\u036f]/g, ""); // np. TWARÓG --> twarog
        //https://www.codu.co/articles/remove-accents-from-a-javascript-string-skgp1inb
    }

    function filter() {
        const searchValue = normalizeText(searchBar.value);
        const kidFilterOn = kidFilter.checked;
        const genreSelect = genreFilter.value;

        for (const m of movieRows) {
            const movieTitle = normalizeText(m.dataset.title);
            const kidFriendly = m.dataset.forKids == true;
            const movieRowGenres = m.dataset.genres.split(",");

            const matchTitleSubstring = searchValue == "" || movieTitle.includes(searchValue);
            const matchKidFilter = !kidFilterOn || kidFriendly;
            const matchGenreFilter = genreSelect == "" || movieRowGenres.includes(genreSelect);
            // event listeners at the bottom, don't forget again

            if (matchTitleSubstring && matchKidFilter && matchGenreFilter) {
                m.style.display = "";
            } else {
                m.style.display = "none";
            }
        }
    }

    let typingDelay;
    searchBar.addEventListener("input", () => {
        clearTimeout(typingDelay);
        typingDelay = setTimeout(filter, 250);
    });

    kidFilter.addEventListener("change", filter);
    genreFilter.addEventListener("change", filter);

    filter();
});