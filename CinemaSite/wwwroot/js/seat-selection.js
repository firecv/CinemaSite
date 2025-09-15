document.addEventListener("DOMContentLoaded", function () {
    const seatGrid = document.getElementById("seat-grid");
    const cartItems = document.getElementById("cart-items");

    const cart = new Map();

    function renderCart() {
        if (cart.size == 0) {
            cartItems.innerHTML = "";
            cartItems.style.display = "none";
        } else {
            cartItems.style.display = "";

            const cartRows = [];
            for (const item of cart.values()) {
                cartRows.push(`
                    <tr>
                        <td>
                            <p>Miejsce ${item.fixedSeatRow}${item.seatCol}</p>
                            <p>Siedzenie ${item.seatType}</p>
                        </td>
                    </tr>
                `);
            }

            cartItems.innerHTML = cartRows.join("");
        }
    }

    seatGrid.addEventListener("click", (e) => {
        const seat = e.target.closest(".seat-square");

        if (seat == null || seat.classList.contains("taken")) return;

        const seatId = Number(seat.dataset.seatId);
        const seatRow = Number(seat.dataset.row);
        const seatCol = Number(seat.dataset.column);
        const seatType = Number(seat.dataset.seatType);
        const ticketType = 1;

        const fixedSeatRow = String.fromCharCode('A'.charCodeAt(0) + seatRow - 1);

        if (seat.classList.toggle("selected")) {
            cart.set(seatId, { seatId, fixedSeatRow, seatCol, seatType, ticketType});
        } else {
            cart.delete(seatId)
        }

        renderCart();
    });
});