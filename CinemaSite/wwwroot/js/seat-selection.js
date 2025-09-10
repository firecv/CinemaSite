document.addEventListener("DOMContentLoaded", function () {
    const seatGrid = document.getElementById("seat-grid");
    //const seats = Array.from(document.querySelectorAll(".seat-square"));

    seatGrid.addEventListener("click", (e) => {
        const seat = e.target.closest(".seat-square");

        if (seat == null || seat.classList.contains("taken")) return;


        seat.classList.toggle("selected");

        //ADD TO CART


    });
});