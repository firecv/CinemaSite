document.addEventListener("DOMContentLoaded", function () {
    const seatGrid = document.getElementById("seat-grid");
    const cartContainer = document.getElementById("cart-container");
    const cartItems = document.getElementById("cart-items");
    const totalPriceDisplay = document.getElementById("total");

    const cart = new Map();

    function sumCartItems() {
        const cartItemPrices = document.querySelectorAll(".ttDropdown");
        let sumPrice = 0;

        for (const item of cartItemPrices) {
            const selOption = item.selectedOptions[0];
            sumPrice += Number(selOption.dataset.price ?? 0);
        }

        totalPriceDisplay.style.display = "";
        totalPriceDisplay.textContent = `Razem: ${(sumPrice / 100).toFixed(2)}zł`;
    }

    function renderCart() {
        if (cart.size == 0) {
            cartContainer.style.display = "none";
        } else {
            const cartRows = document.createDocumentFragment();
            for (const item of cart.values()) {
                const tr = document.createElement("tr");
                const td1 = document.createElement("td");
                const td2 = document.createElement("td");
                const td3 = document.createElement("td");
                const td4 = document.createElement("td");

                const seatNumText = document.createElement("span");
                seatNumText.textContent = `${item.fixedSeatRow}${item.seatCol}`; //doesn't work with " ", do not change
                seatNumText.classList.add("item-seatNumText");

                const seatTypeText = document.createElement("span");
                seatTypeText.textContent = `${item.seatTypeName}`;
                seatTypeText.classList.add("item-seatTypeText");

                const ttDropdown = document.getElementsByTagName("template")[0].content.firstElementChild.cloneNode(true);
                ttDropdown.dataset.seatId = String(item.seatId); //template *with ticket type data* is in the cshtml file, but id is added here
                ttDropdown.addEventListener("change", sumCartItems);
                ttDropdown.classList.add("item-ttDropdown");

                const xButton = document.createElement("button");
                xButton.type = "button";
                xButton.className = "unselect";
                xButton.dataset.seatId = String(item.seatId);
                xButton.textContent = "X";
                xButton.classList.add("item-xButton");
                xButton.classList.add("form-button");

                const hiddenSeatIdPasser = document.createElement("input");
                hiddenSeatIdPasser.type = "hidden";
                hiddenSeatIdPasser.name = "seatIdsPost";
                hiddenSeatIdPasser.value = String(item.seatId); //string temporarily, html restriction

                td1.append(seatNumText, hiddenSeatIdPasser);
                td2.appendChild(seatTypeText);
                td3.appendChild(ttDropdown);
                td4.appendChild(xButton);
                tr.append(td1, td2, td3, td4);
                cartRows.appendChild(tr);
            }

            cartItems.replaceChildren(cartRows);
            cartContainer.style.display = "";
            sumCartItems();
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
    });

    renderCart();
});