document.addEventListener("DOMContentLoaded", function () {

    const modalWindow = document.getElementById("modal-movie");
    const modalInfo = document.getElementById("modal-movie-info");
    const xButton = document.getElementById("xbutton");

    /*const modalTitle = document.getElementById("modal-title");
    const modalPoster = document.getElementById("modal-poster");
    const modalSummary = document.getElementById("modal-summary");
    const modalTrailer = document.getElementById("modal-trailer");*/

    
    const modalIdEditHidden = document.getElementById("edit-id");
    const modalTitleEdit = document.getElementById("edit-title");
    const modalSummaryEdit = document.getElementById("edit-summary");
    const modalTrailerEdit = document.getElementById("edit-trailer");
    const modalKidsEdit = document.getElementById("edit-kids");

    const modalScreeningBox = document.getElementById("screening-form-interior");
    const screeningRow = document.getElementsByTagName("template")[0];

    const screeningJson = JSON.parse(document.getElementById("screenings-to-json").textContent); //parse = node, textContent = data
    const movieGenreJson = JSON.parse(document.getElementById("moviegenres-to-json").textContent);

    function updateModal(movieRow) {
        /*modalTitle.textContent = movieRow.dataset.title;
        modalPoster.src = movieRow.dataset.poster;
        modalPoster.alt = "Plakat dla " + movieRow.dataset.title;
        modalSummary.textContent = movieRow.dataset.summary;
        modalTrailer.src = "https://www.youtube.com/embed/" + movieRow.dataset.trailer;*/

        modalIdEditHidden.value = movieRow.dataset.id;
        modalTitleEdit.value = movieRow.dataset.title;
        modalSummaryEdit.value = movieRow.dataset.summary;
        modalTrailerEdit.value = movieRow.dataset.trailer;

        if (movieRow.dataset.forkids == "true" || movieRow.dataset.forkids == "True" || movieRow.dataset.forkids == "1") {
            modalKidsEdit.checked = true;
        } else {
            modalKidsEdit.checked = false;
        }

        const allGenreRows = document.querySelectorAll(".genreBox");
        const genresForMovie = movieGenreJson.filter(mg => mg.movie_id == movieRow.dataset.id);

        allGenreRows.forEach(gr => {
            const genreCheckbox = gr.querySelector(`input[name=genreCheckbox]`);
            genreCheckbox.checked = genresForMovie.some(g => g.genre_id == genreCheckbox.value);      
        });

        //from here it's just screening form handling/generating

        const screeningsForMovie = screeningJson.filter(sc => sc.movie_id == movieRow.dataset.id);

        const allRows = document.createDocumentFragment();

        screeningsForMovie.forEach(sm => {
            const row = screeningRow.content.firstElementChild.cloneNode(true);

            row.querySelector(`input[name="movieId"]`).value = movieRow.dataset.id;
            row.querySelector(`input[name="screeningId"]`).value = sm.screening_id;
            row.querySelector(`input[name="screeningTime"]`).value = sm.screening_time;
            row.querySelector(`select[name="hallId"]`).value = sm.hall_id;

            const dubbingCheckbox = row.querySelector(`input[name="dubCheckbox"]`);
            dubbingCheckbox.checked = sm.dubbing;

            const dubbingValue = row.querySelector(`input[name=isDubbing]`);

            dubbingValue.value = dubbingCheckbox.checked ? "true" : "false";

            dubbingCheckbox.addEventListener("change", () => { //A COMMENT SO IT'S EASIER TO FIND
                dubbingValue.value = dubbingCheckbox.checked ? "true" : "false";
            });

            allRows.appendChild(row);
        });

        modalScreeningBox.replaceChildren(allRows);

        modalWindow.hidden = false;
    }

    function closeModal() {
        modalWindow.hidden = true;
        modalTrailer.src = "";
    }



    const newMovieButton = document.getElementById("new-movie-button");
    const newMovieModal = document.getElementById("new-movie-modal");
    const newMovieModalInfo = document.getElementById("new-movie-modal-info");
    const xButtonNM = document.getElementById("new-movie-xbutton");
    
    function openNewMovieModal() {
        newMovieModal.hidden = false;

        /*const allGenreRows = document.querySelectorAll(".genreBoxNew");

        allGenreRows.forEach(gr => {
            const genreCheckbox = gr.querySelector(`input[name=genreCheckbox]`);
        });*/
    }

    function closeNewMovieModal() {
        newMovieModal.hidden = true;
    }

    newMovieButton.addEventListener("click", openNewMovieModal);
    xButtonNM.addEventListener("click", closeNewMovieModal);
    newMovieModal.addEventListener("click", closeNewMovieModal);
    newMovieModalInfo.addEventListener("click", e => { e.stopPropagation(); });

    const moviesList = document.getElementById("movies-list");
    moviesList.addEventListener("click", (e) => {

        if (e.target.closest(".control-box")) return;

        const movieSelected = e.target.closest(".movie-row");

        if (movieSelected == null) return;
          
        updateModal(movieSelected);
    });



    xButton.addEventListener("click", closeModal);
    document.addEventListener("keydown", function (e) { //https://lexingtonthemes.com/tutorials/how-to-create-a-image-gallery-with-tailwind-css-and-javascript/
        if (e.key === "Escape") closeModal();
    });

    modalWindow.addEventListener("click", closeModal);
    modalInfo.addEventListener("click", e => { e.stopPropagation(); });



    //deletion stuff

    const delButton = document.querySelectorAll(".delete-button");
    const deleteModal = document.getElementById("modal-delete");
    const xButtonDelete = document.getElementById("xbutton-delete");
    const movieToDelete = document.getElementById("movie-to-delete");
    const confirmationMessage = document.getElementById("confirmation-message");
    const confirmDeleteModal = document.getElementById("modal-delete-confirm");


    function modalDelPopup(del) {
        movieToDelete.value = del.dataset.id;
        confirmationMessage.textContent = "Are you sure you wish to permanently delete " + del.dataset.title + "?";
        deleteModal.hidden = false;
    }

    function closeDelModal() {
        deleteModal.hidden = true;
    }

    delButton.forEach(del => {
        del.addEventListener("click", (e) => {
            e.stopPropagation();
            modalDelPopup(del);
        });
    });

    deleteModal.addEventListener("click", closeDelModal);
    confirmDeleteModal.addEventListener("click", e => { e.stopPropagation(); });
});