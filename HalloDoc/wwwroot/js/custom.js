//Country Code
const phoneInputField = document.querySelector("#phone");
const phoneInput = window.intlTelInput(phoneInputField, {
  utilsScript:
    "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
});
const phoneInputField1 = document.querySelector("#phone2");
const phoneInput1 = window.intlTelInput(phoneInputField1, {
  utilsScript:
    "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
});
// Alert while submitting form
function submitForm(form) {
  swal({
      title: "Information",
      text: "When submitting a request, you must provide the correct contact information for the patient of the responsibly party.Failure to provide the correct email and phone number will delay service or be declined.",
      icon: "warning",
      button: true,

  })
  .then(function (isOkay) {
      if (isOkay) {
          form.submit();
      }
  });
  return false;
}
function changeTheme() {
  var element = document.body;
  element.classList.toggle("darkMode");
}