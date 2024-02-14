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
//form

function changeTheme() {
  var element = document.body;
  element.classList.toggle("darkMode");
}
//tabs change
//var btnContainer = document.getElementByClassName("headerTabs");
//var btns = btnContainer.getElementsByTagName("a");
//for (var i = 0; i < btns.length; i++) {
//    btns[i].addEventListener("click", function () {

//        (document.querySelector('.nav-link-active')) ? document.querySelector('.nav-link-active').classList.remove('nav-link-active') : '';
//        this.classList.add('nav-link-active');
//    });
//} 
// Add event listeners to the navigation links
// Add event listeners to the navigation links
// Add event listeners to the navigation links
//document.querySelectorAll('.nav-link').forEach(link => {
//    link.addEventListener('click', event => {
//        // Remove the active class from all links
//        document.querySelectorAll('.nav-link').forEach(link => {
//            link.classList.remove('active');
//        });

//        // Add the active class to the clicked link
//        event.target.classList.add('active');

//        // Store the active tab in a view data variable
//        const tabId = event.target.id.replace('Link', '');
//        ViewData["ActiveTab"] = tabId;
//    });
//});

// Add the active class to the dashboard link by default
document.querySelector('#dashboardLink').classList.add('active');

// Set the active tab based on the view data variable
const activeTab = ViewData["ActiveTab"];
if (activeTab) {
    document.querySelector(`#${activeTab}Link`).classList.add('active');
}
