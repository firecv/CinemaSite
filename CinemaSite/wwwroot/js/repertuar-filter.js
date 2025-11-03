document.addEventListener("DOMContentLoaded", function () {

    /// FILTRY REPERTUARU

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
            const kidFriendly = m.dataset.forkids == "true";
            const movieRowGenres = m.dataset.genres.split(",");
            const matchTitleSubstring = searchValue == "" || movieTitle.includes(searchValue);
            const matchKidFilter = !kidFilterOn || kidFriendly;
            const matchGenreFilter = genreSelect == "" || movieRowGenres.includes(genreSelect);
            //event listeners at the bottom, don't forget again

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
        typingDelay = setTimeout(filter, 200);
    });

    kidFilter.addEventListener("change", filter);
    genreFilter.addEventListener("change", filter);

    filter();


    /// PRZELACZENIE MIEDZY DNIAMI

    document.querySelectorAll(".movie-row").forEach(movie => {
        const columns = movie.querySelectorAll("[data-index]");
        const forwardBtn = movie.querySelector(".forward");
        const backwardBtn = movie.querySelector(".backward");

        let firstDay = 0;
        let dayCount = 7;
        let maxScroll = 14 - dayCount;

        function scrollWindow() {
            columns.forEach(column => {
                const index = parseInt(column.dataset.index, 10);
                if (index >= firstDay && index < firstDay + dayCount) {
                    column.style.display = "";
                } else {
                    column.style.display = "none";
                }
            });
        }

        forwardBtn.addEventListener("click", () => {
            if (firstDay < maxScroll) {
                firstDay++;
                scrollWindow();
            } });
        backwardBtn.addEventListener("click", () => {
            if (firstDay > 0) {
                firstDay--;
                scrollWindow();
            }
        });


        window.addEventListener("resize", () => {
            winWidth = window.innerWidth;
            if (winWidth <= 700) {
                dayCount = 3;
            } else if (winWidth <= 900) {
                dayCount = 5;
            } else if (winWidth <= 1000) {
                dayCount = 6;
            } else if (winWidth <= 1100) {
                dayCount = 7;
            } else if (winWidth <= 1200) {
                dayCount = 4;
            } else if (winWidth <= 1300) {
                dayCount = 5;
            } else if (winWidth <= 1500) {
                dayCount = 6;
            } else {
                dayCount = 7;
            }
            maxScroll = 14 - dayCount;
            scrollWindow();
        });

        scrollWindow();
    });
});