document.addEventListener("DOMContentLoaded", function () {

    const modalWindow = document.getElementById("modal-movie");
    const closeModalButton = document.getElementById("xbutton");

    const modalTitle = document.getElementById("modal-title");
    const modalPoster = document.getElementById("modal-poster");
    const modalSummary = document.getElementById("modal-summary");
    const modalTrailer = document.getElementById("modal-trailer");

    function updateModal(movieRow) {
        modalTitle.textContent = movieRow.dataset.title;
        modalPoster.src = movieRow.dataset.poster;
        modalPoster.alt = "Plakat dla " + movieRow.dataset.title;
        modalSummary.textContent = movieRow.dataset.summary;
        modalTrailer.src = "https://www.youtube.com/embed/" + movieRow.dataset.trailer;

        modalWindow.hidden = false;
    }

    function closeModal() {
        modalWindow.hidden = true;
        modalTrailer.src = "";
    }

    document.querySelectorAll(".movie-row").forEach(row => {
        row.addEventListener("click", () => updateModal(row));
    })
    closeModalButton.addEventListener("click", closeModal);
});