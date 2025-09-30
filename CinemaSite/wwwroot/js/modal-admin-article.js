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

    let allRows = document.createDocumentFragment();

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

        allRows = document.createDocumentFragment();

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

    xButtonDelete.addEventListener("click", closeDelModal);
    deleteModal.addEventListener("click", closeDelModal);
    confirmDeleteModal.addEventListener("click", e => { e.stopPropagation(); });
});