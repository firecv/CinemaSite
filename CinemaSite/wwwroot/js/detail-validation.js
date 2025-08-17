document.addEventListener("DOMContentLoaded", () => {

    let validPassword = false;

    const submitBtn = document.getElementById("submit-button");
    const username = document.getElementById("username");
    const email = document.getElementById("email");
    const phone = document.getElementById("phone");
    const password = document.getElementById("password");
    const repeatPassword = document.getElementById("repeat-password");
    const errorMessage = document.getElementById("error-message");

    const emailRegex = /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;
    // zrodlo: https://stackoverflow.com/questions/46155/how-can-i-validate-an-email-address-in-javascript
    const phoneRegex = /^[\+]?[0-9]{0,3}\W?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/im;
    // zrodlo: https://stackoverflow.com/questions/4338267/validate-phone-number-with-javascript (naprawione dla js)

    var updateErrorMessage = function (string message) {
        submitBtn.disabled = true;
        errorMessage.textContent = message;
        errorMessage.style.visibility = "visible";
    }

    var validateForm = function () {
        if (username.value.length < 6) {
            updateErrorMessage("Nazwa uzytkownika musi zawierać conajmniej 6 znaki.");
        } else if (username.value.length > 16) {
            updateErrorMessage("Nazwa uzytkownika może zawierać najwyżej 16 znaki.");
        } else if (!email.value.match(emailRegex)) {
            updateErrorMessage("Nieprawidłowy adres e-mail.");
        } else if (!phone.value.match(phoneRegex)) {
            updateErrorMessage("Nieprawidłowy numer telefonu.");
        } else if (password.value.length < 8) {
            updateErrorMessage("Hasło musi zawierać conajmniej 8 znaki.");
        } else if (password.value.length > 256) {
            updateErrorMessage("Hasło może zawierać najwyżej 256 znaki.");
        } else if (password.value != repeatPassword.value) {
            updateErrorMessage("Hasła się nie zgadzają.");
        } else {
            submitBtn.disabled = false;
            errorMessage.textContent = "";
            errorMessage.style.visibility = "hidden";
        }
    }

    validateForm();

    [username, email, phone, password, repeatPassword].forEach(function (field) {
        field.addEventListener("input", validateForm);
        field.addEventListener("keyup", validateForm);
    })
});