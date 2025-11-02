document.addEventListener("DOMContentLoaded", function () {

    const modalWindow = document.getElementById("modal-article");
    const modalInfo = document.getElementById("modal-article-info");
    const xButton = document.getElementById("xbutton");
    
    const modalIdEditHidden = document.getElementById("edit-id");
    const modalTitleEdit = document.getElementById("edit-title");
    const modalContentEdit = document.getElementById("edit-content");
    const modalPublicationEdit = document.getElementById("edit-publication");

    let allRows = document.createDocumentFragment();

    function updateModal(articleRow) {
        modalIdEditHidden.value = articleRow.dataset.id;
        modalTitleEdit.value = articleRow.dataset.title;
        modalContentEdit.value = articleRow.dataset.content;
        modalPublicationEdit.value = articleRow.dataset.publication;

        allRows = document.createDocumentFragment();

        modalWindow.hidden = false;
    }

    function closeModal() {
        modalWindow.hidden = true;
        modalPublication.src = "";
    }



    const newArticleButton = document.getElementById("new-article-button");
    const newArticleModal = document.getElementById("new-article-modal");
    const newArticleModalInfo = document.getElementById("new-article-modal-info");
    const xButtonNM = document.getElementById("new-article-xbutton");
    
    function openNewMovieModal() {
        newArticleModal.hidden = false;
    }

    function closeNewMovieModal() {
        newArticleModal.hidden = true;
    }

    newArticleButton.addEventListener("click", openNewMovieModal);
    xButtonNM.addEventListener("click", closeNewMovieModal);
    newArticleModal.addEventListener("click", closeNewMovieModal);
    newArticleModalInfo.addEventListener("click", e => { e.stopPropagation(); });

    const articlesList = document.getElementById("articles-list");
    articlesList.addEventListener("click", (e) => {

        if (e.target.closest(".control-box")) return;

        const articleSelected = e.target.closest(".article-row");

        if (articleSelected == null) return;
          
        updateModal(articleSelected);
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
    const articleToDelete = document.getElementById("article-to-delete");
    const confirmationMessage = document.getElementById("confirmation-message");
    const confirmDeleteModal = document.getElementById("modal-delete-confirm");


    function modalDelPopup(del) {
        articleToDelete.value = del.dataset.id;
        confirmationMessage.textContent = "Na pewno chcesz skasować zgłoszenie: " + del.dataset.title + "?";
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