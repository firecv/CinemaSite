document.addEventListener("DOMContentLoaded", function () {

    const modalWindow = document.getElementById("modal-movie");
    const modalInfo = document.getElementById("modal-movie-info");
    const xButton = document.getElementById("xbutton");

    const modalTitle = document.getElementById("modal-title");
    const modalPoster = document.getElementById("modal-poster");
    const modalSummary = document.getElementById("modal-summary");
    const modalTrailer = document.getElementById("modal-trailer");

    
    const modalIdEditHidden = document.getElementById("edit-id");
    const modalTitleEdit = document.getElementById("edit-title");
    const modalSummaryEdit = document.getElementById("edit-summary");
    const modalTrailerEdit = document.getElementById("edit-trailer");
    const modalKidsEdit = document.getElementById("edit-kids");

    function updateModal(movieRow) {
        modalTitle.textContent = movieRow.dataset.title;
        modalPoster.src = movieRow.dataset.poster;
        modalPoster.alt = "Plakat dla " + movieRow.dataset.title;
        modalSummary.textContent = movieRow.dataset.summary;
        modalTrailer.src = "https://www.youtube.com/embed/" + movieRow.dataset.trailer;

        modalIdEditHidden.value = movieRow.dataset.id;
        modalTitleEdit.value = movieRow.dataset.title;
        modalSummaryEdit.value = movieRow.dataset.summary;
        modalTrailerEdit.value = movieRow.dataset.trailer;

        if (movieRow.dataset.forkids == "true" || movieRow.dataset.forkids == "True" || movieRow.dataset.forkids == "1") {
            modalKidsEdit.checked = true;
        } else {
            modalKidsEdit.checked = false;
        }

        modalWindow.hidden = false;
    }

    function closeModal() {
        modalWindow.hidden = true;
        modalTrailer.src = "";
    }

    const moviesList = document.getElementById("movies-list");
    moviesList.addEventListener("click", (e) => {

        if (e.target.closest(".screening-box")) return;

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
});