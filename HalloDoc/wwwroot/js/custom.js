//Country Code


// Alert while submitting form
//form

//function changeTheme() {

//    if ($("body").hasClass("darkMode") ||
//        $(".mainSection").hasClass("darkMode") ||
//        $(".header2").hasClass("bg-dark") ||
//        $(".header").hasClass("bg-dark") ||
//        $(".subHeader").hasClass("bg-dark") ||
//        $(".PatientInfo").hasClass("") ||


//        $(".mobileviewdashboard").hasClass("darkMode")  




//    ) {
//        $("body").removeClass("darkMode");
//        $(".mainSection").removeClass("darkMode");
//        $(".header2").removeClass("bg-dark");
//        $(".header2").addClass("bg-white");
//        $(".header").removeClass("bg-dark");
//        $(".header").addClass("bg-white");
//        $(".header").removeClass("bg-dark");
//        $(".subHeader").addClass("bg-white");
//        $(".subHeader").removeClass("bg-dark");


//        $(".mobileviewdashboard").removeClass("darkMode");


//    } else {
//        $("body").addClass("darkMode");
//        $(".mainSection").addClass("darkMode");
//        $(".header2").addClass("bg-dark");
//        $(".header2").removeClass("bg-white");

//        $(".header").addClass("bg-dark");
//        $(".header").removeClass("bg-white");
//        $(".subHeader").addClass("bg-dark");
//        $(".subHeader").removeClass("bg-white");

//        $(".mobileviewdashboard").addClass("darkMode");



//    }



//}

function changeTheme() {
    if ($("body").hasClass("darkMode") ||
        $(".mainSection").hasClass("darkMode") ||
        $(".header2").hasClass("bg-dark") ||
        $(".header").hasClass("bg-dark") ||
        $(".subHeader").hasClass("bg-dark") ||
        $(".PatientInfo").hasClass("darkMode") ||
        $(".mobileviewdashboard").hasClass("darkMode")||
        $(".table").hasClass("table-dark")

        

    ) {
        $("body").removeClass("darkMode");
        $(".mainSection").removeClass("darkMode");
        $(".header2").removeClass("bg-dark");
        $(".header2").addClass("bg-white");
        $(".header").removeClass("bg-dark");
        $(".header").addClass("bg-white");
        $(".header").removeClass("bg-dark");
        $(".subHeader").addClass("bg-white");
        $(".subHeader").removeClass("bg-dark");
        $(".table").removeClass("table-dark");


        $(".mobileviewdashboard").removeClass("darkMode");

        document.cookie = "theme=light";
    } else {
        $("body").addClass("darkMode");
        $(".mainSection").addClass("darkMode");
        $(".header2").addClass("bg-dark");
        $(".header2").removeClass("bg-white");

        $(".header").addClass("bg-dark");
        $(".header").removeClass("bg-white");
        $(".subHeader").addClass("bg-dark");
        $(".subHeader").removeClass("bg-white");
        $(".table").addClass("table-dark");

        $(".mobileviewdashboard").addClass("darkMode");



        document.cookie = "theme=dark";
    }
}

