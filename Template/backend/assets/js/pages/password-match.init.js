var password = document.getElementById("password-input"),
  confirm_password = document.getElementById("confirm-password-input");
function validatePassword() {
  password.value != confirm_password.value
    ? confirm_password.setCustomValidity("Passwords Don't Match")
    : confirm_password.setCustomValidity("");
}
password.onchange = validatePassword;
var myInput = document.getElementById("password-input"),
  letter = document.getElementById("pass-lower"),
  capital = document.getElementById("pass-upper"),
  number = document.getElementById("pass-number"),
  length = document.getElementById("pass-length");
(myInput.onfocus = function () {
  document.getElementById("password-contain").style.display = "block";
}),
  (myInput.onblur = function () {
    document.getElementById("password-contain").style.display = "none";
  }),
  (myInput.onkeyup = function () {
    myInput.value.match(/[a-z]/g)
      ? (letter.classList.remove("invalid"), letter.classList.add("valid"))
      : (letter.classList.remove("valid"), letter.classList.add("invalid")),
      myInput.value.match(/[A-Z]/g)
        ? (capital.classList.remove("invalid"), capital.classList.add("valid"))
        : (capital.classList.remove("valid"), capital.classList.add("invalid"));
    myInput.value.match(/[0-9]/g)
      ? (number.classList.remove("invalid"), number.classList.add("valid"))
      : (number.classList.remove("valid"), number.classList.add("invalid")),
      8 <= myInput.value.length
        ? (length.classList.remove("invalid"), length.classList.add("valid"))
        : (length.classList.remove("valid"), length.classList.add("invalid"));
  });
