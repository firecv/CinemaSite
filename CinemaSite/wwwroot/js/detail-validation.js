alert("AAA");

let validPassword = false;

const submitBtn = document.getElementById("submit-button");
const passwordBox = document.getElementById("password");
const repeatPasswordBox = document.getElementById("repeat-password");

if (validPassword) {
    btn.disabled = false;
} else {
    btn.disabled = true;
}

var checkPassword = function () {
    if (passwordBox.value = repeatPasswordBox.value) {
        validPassword = true;
    } else {
        validPassword = false;
    }
}

checkPassword();

passwordBox.addEventListener("input", checkPassword());
repeatPasswordBox.addEventListener("input", checkPassword());