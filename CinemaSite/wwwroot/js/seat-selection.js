document.addEventListener("DOMContentLoaded", function () {
    const seatGrid = document.getElementById("seat-grid");
    const cartItems = document.getElementById("cart-items");

    const cart = new Map();

    function renderCart() {
        if (cart.size == 0) {
            //cartItems.innerHTML = "";
            cartItems.style.display = "none";
        } else {
            /*   DOM Version */

            const cartRows = document.createDocumentFragment();
            for (const item of cart.values()) {
                const tr = document.createElement("tr");
                const td = document.createElement("td");

                const seatNumText = document.createElement("span");
                seatNumText.textContent = `Miejsce ${item.fixedSeatRow}${item.seatCol}`; //doesn't work with " ", do not change

                const seatTypeText = document.createElement("span");
                seatTypeText.textContent = `Siedzenie ${item.seatTypeName}`;

                const ttDropdown = document.getElementsByTagName("template")[0].content.firstElementChild.cloneNode(true);
                ttDropdown.dataset.seatId = String(item.seatId); //template *with ticket type data* is in the cshtml file, but id is added here

                const xButton = document.createElement("button");
                xButton.type = "button";
                xButton.className = "unselect"; //TODO: event listener to unselect this seat
                xButton.dataset.seatId = String(item.seatId);
                xButton.textContent = "X";

                td.append(seatNumText, seatTypeText, ttDropdown, xButton);
                tr.appendChild(td);
                cartRows.appendChild(tr);
            }

            cartItems.replaceChildren(cartRows);

            /*   innerHTML Version
            
            const cartRows = [];
            for (const item of cart.values()) {
                cartRows.push(`
                    <tr>
                        <td style="display: flex; flex-direction: row;">
                            <span>Miejsce ${item.fixedSeatRow}${item.seatCol}</p>
                            <span>Siedzenie ${item.seatType}</p>
                            ${dropdownClone.outerHTML}
                            <button type="button" class="unselect" data-seat-id="${item.seatId}">X</button>
                        </td>
                    </tr>
                `);
            }

            cartItems.innerHTML = cartRows.join("");*/

            cartItems.style.display = "";
        }
    }


    seatGrid.addEventListener("click", (e) => {
        const seat = e.target.closest(".seat-square");

        if (seat == null || seat.classList.contains("taken")) return;

        const seatId = Number(seat.dataset.seatId);
        const seatRow = Number(seat.dataset.row);
        const seatCol = Number(seat.dataset.column);
        const seatType = Number(seat.dataset.seatType);
        const seatTypeName = seat.dataset.seatName;
        const ticketType = 1;

        const fixedSeatRow = String.fromCharCode('A'.charCodeAt(0) + seatRow - 1);

        if (seat.classList.toggle("selected")) {
            cart.set(seatId, { seatId, fixedSeatRow, seatCol, seatType, seatTypeName, ticketType});
        } else {
            cart.delete(seatId);
        }

        renderCart();
    });


    cartItems.addEventListener("click", (e) => {
        const xButton = e.target.closest(".unselect");

        if (xButton == null) return;

        const seatId = Number(xButton.dataset.seatId);
        const seatSelected = document.querySelector(`.seat-square.selected[data-seat-id="${seatId}"]`);

        seatSelected.classList.remove("selected");
        cart.delete(seatId);

        renderCart();
    })
});