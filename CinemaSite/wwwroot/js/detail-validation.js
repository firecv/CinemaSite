/*alert("AAA");

document.addEventListener("DOMContentLoaded", () => {
    let validPassword = false;

    const submitBtn = document.getElementById("submit-button");
    const passwordBox = document.getElementById("password");
    const repeatPasswordBox = document.getElementById("repeat-password");

    var checkPassword = function () {
        if (passwordBox.value == repeatPasswordBox.value && passwordBox.value.length > 0) {
            validPassword = true;
            submitBtn.disabled = false;
        } else {
            validPassword = false;
            submitBtn.disabled = true;
        }
    }

    checkPassword();

    passwordBox.addEventListener("input", checkPassword());
    passwordBox.addEventListener("keyup", checkPassword());
    repeatPasswordBox.addEventListener("input", checkPassword());
    repeatPasswordBox.addEventListener("keyup", checkPassword());
});*/