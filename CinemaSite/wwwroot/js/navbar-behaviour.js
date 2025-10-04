document.addEventListener("DOMContentLoaded", function () {
    const openBurgerButton = document.getElementById("open-hamburger");
    const burgerMenu = document.getElementById("dropdown-hamburger");

    openBurgerButton.addEventListener("click", () => {
        burgerMenu.classList.toggle("popout");
    });
});