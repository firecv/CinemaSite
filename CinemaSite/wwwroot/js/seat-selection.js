document.addEventListener("DOMContentLoaded", function () {
    const seatGrid = document.getElementById("seat-grid");
    const cartItems = document.getElementById("cart-items");

    const cart = new Map();
    const defaultTicketType = 1;

    function renderCart() {
        cartItems.innerHTML = "";

        if (cart.size == 0) {
            cartItems.display = "none";
        } else {
            cartItems.display = "";

            for (const item of cart.values) {
                cartItems.innerHTML = `
                    <div>
                        <p>Miejsce ${item.seatRow}${item.seatCol}</p>
                        <p>Siedzenie ${item.seatType}</p>

                    </div>
                `;
            }
        }
    }

    seatGrid.addEventListener("click", (e) => {
        const seat = e.target.closest(".seat-square");

        const seatId = Number(seat.dataset.seatId);
        const seatRow = Number(seat.dataset.row);
        const seatCol = Number(seat.dataset.column);
        const seatType = Number(seat.dataset.seat-type);

        if (seat == null || seat.classList.contains("taken")) return;

        if (seat.classList.toggle("selected")) {
            cart.set(seatId, { seatId, seatRow, seatCol, seatType, defaultTicketType});
        } else {
            cart.delete(seatId)
        }

        renderCart();
    });
});