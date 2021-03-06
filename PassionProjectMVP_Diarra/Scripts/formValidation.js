//Course field validation
//If these conditions are not satisfied, the form cannot be submitted!
function verifyClass() {

    var classForm = document.forms.createClass;
    var className = document.getElementById("className");
    var startDate = document.getElementById("startDate");
    var endDate = document.getElementById("endDate");

    //name pattern, only normal characters
    var nameRegex = /^([A-Za-z]\s?)+$/;
    //Simplified Date pattern not fully respecting day counts!
    var dateRegex = /^(3[0-1]|2[0-9]|1[0-9]|0[0-9])-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)-\d{4}$/i; //Date

    //This function checks all the fields one by one
    function validation() {
        //Course name checking
        if (!nameRegex.test(className.value)) {
            className.style.backgroundColor = "red";
            className.focus();
            return false;
        } else {
            className.style.backgroundColor = "white";
        }
        //Start date checking
        if (!dateRegex.test(startDate.value)) {
            startDate.style.backgroundColor = "red";
            startDate.focus();
            return false;
        } else {
            startDate.style.backgroundColor = "white";
        }
        //End date  checking
        if (!dateRegex.test(endDate.value)) {
            endDate.style.backgroundColor = "red";
            endDate.focus();
            return false;
        } else {
            endDate.style.backgroundColor = "white";
        }
    }

    classForm.onsubmit = validation;

}


//This function verifies all the form fields of a pupil at creation and edition.
//If these conditions are not satisfied, the form cannot be submitted!
function verifyPupil() {

    var pupilForm = document.forms.createPupil;
    var pupilFname = document.getElementById("pupil_firstName");
    var pupilLname = document.getElementById("pupil_lastName");
    var pupilAge = document.getElementById("pupil_age");
    
    //name pattern, only normal characters
    var nameRegex = /^([A-Za-z])+$/;
    //Age pattern
    var ageRegex = /^(1[0-2]|[6-9])$/; //Pupil age 6-12

    //This function checks all the fields one by one
    function validation() {
        //Pupil first name checking
        if (!nameRegex.test(pupilFname.value)) {
            pupilFname.style.backgroundColor = "red";
            pupilFname.focus();
            return false;
        } else {
            pupilFname.style.backgroundColor = "white";
        }
        //Pupil last name checking
        if (!nameRegex.test(pupilLname.value)) {
            pupilLname.style.backgroundColor = "red";
            pupilLname.focus();
            return false;
        } else {
            pupilLname.style.backgroundColor = "white";
        }
        //Pupil  age
        if (!ageRegex.test(pupilAge.value)) {
            pupilAge.style.backgroundColor = "red";
            pupilAge.focus();
            return false;
        } else {
            pupilAge.style.backgroundColor = "white";
        }
    }

    pupilForm.onsubmit = validation;

}



//Location validation
//This function validate the location object field during creation and edition
function verifyLocation() {

    var locationForm = document.forms.createLocation;
    var city = document.getElementById("city");
    var country = document.getElementById("country");
    var incomeRange = document.getElementById("incomeRange");

    //Income range can only be low, medium or high
    var incomeRegex = /^(Low|Medium|High)$/i;

    //Country and city names pattern, only normal characters and space
    var nameRegex = /^([A-Za-z]\s?)+$/;  //city and country may have space

    //This function checks all the fields one by one
    function validation() {
        //City name checking
        if (!nameRegex.test(city.value)) {
            city.style.backgroundColor = "red";
            city.focus();
            return false;
        } else {
            city.style.backgroundColor = "white";
        }
        //Country name checking
        if (!nameRegex.test(country.value)) {
            country.style.backgroundColor = "red";
            country.focus();
            return false;
        } else {
            country.style.backgroundColor = "white";
        }
        //Income range
        if (!incomeRegex.test(incomeRange.value)) {
            incomeRange.style.backgroundColor = "red";
            incomeRange.focus();
            return false;
        } else {
            incomeRange.style.backgroundColor = "white";
        }
    }

    locationForm.onsubmit = validation;

}



//This function verifies all the form fields of modules at creation and edition.
//If these conditions are not satisfied, the form cannot be submitted!
function verifyModule() {

    var moduleForm = document.forms.createModule;
    var moduleName = document.getElementById("module_moduleName");
    var moduleDescription = document.getElementById("module_description");
    var moduleDelivery = document.getElementById("module_delivery");
    var moduleFees = document.getElementById("module_fees")

    //Name pattern, only normal characters and space
    var nameRegex = /^([A-Za-z]\s?)+$/;
    //Fees are decimal values
    var feesRegex = /^\d{1,5}([.]\d{0,2})?$/; //fees

    //This function checks all the fields one by one
    function validation() {
        //Module name checking
        if (!nameRegex.test(moduleName.value)) {
            moduleName.style.backgroundColor = "red";
            moduleName.focus();
            return false;
        } else {
            moduleName.style.backgroundColor = "white";
        }
        //Module description
        if (!nameRegex.test(moduleDescription.value)) {
            moduleDescription.style.backgroundColor = "red";
            moduleDescription.focus();
            return false;
        } else {
            moduleDescription.style.backgroundColor = "white";
        }
        //Module Delivery
        if (!nameRegex.test(moduleDelivery.value)) {
            moduleDelivery.style.backgroundColor = "red";
            moduleDelivery.focus();
            return false;
        } else {
            moduleDelivery.style.backgroundColor = "white";
        }
        //module fees
        if (!feesRegex.test(moduleFees.value)) {
            moduleFees.style.backgroundColor = "red";
            moduleFees.focus();
            return false;
        } else {
            moduleFees.style.backgroundColor = "white";
        }
    }

    moduleForm.onsubmit = validation;

}

//Deletion confirmation
function confirmation() {

    var deleteForm = document.forms.delete;

    function confirmDelete() {
        //If user clicks "Cancel", the deletion is cancelled
        var question = confirm("Do you really want to delete?");
        if (!question) {
            return false;
        }
    }
    deleteForm.onsubmit = confirmDelete;
}
