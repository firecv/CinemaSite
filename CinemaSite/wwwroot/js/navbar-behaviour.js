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

window.addEventListener("resize", () => {
    document.body.classList.add("stop-anim");
    setTimeout(() => { document.body.classList.remove("stop-anim"); }, 500); //https://css-tricks.com/stop-animations-during-window-resizing/
});