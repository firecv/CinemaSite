document.addEventListener("DOMContentLoaded", function () {
    const input = document.getElementById("name-search");
    const rows = Array.from(document.querySelectorAll("#movies-list tr[data-title]"));
    const searchString = normalizeText(input.value);

    function normalizeText(string) {
        if (string == null) {
            return "";
        }
        return string.toLowerCase().normalize("NFD").replace(/[\u0300-\u036f]/g, "");
        //https://www.codu.co/articles/remove-accents-from-a-javascript-string-skgp1inb
    }

    function search() {
        input.addEventListener("input", () => {
            rows.forEach(row => {
                const title = normalizeText(row.dataset.title);

                row.style.display = title.includes(searchString) ? "" : "none";
            });
        });
    }

    input.addEventListener("input", search);
    input.addEventListener("keyup", search);
    search();
});