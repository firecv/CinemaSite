document.addEventListener("DOMContentLoaded", function () {
    const openBurgerButton = document.getElementById("hamburger-button");
    const closeBurgerButton = document.getElementById("hamburger-button2");
    const burgerMenu = document.getElementById("dropdown-hamburger");

    openBurgerButton.addEventListener("click", () => {
        burgerMenu.classList.toggle("popout");
    });
    closeBurgerButton.addEventListener("click", () => {
        burgerMenu.classList.toggle("popout");
    });
});