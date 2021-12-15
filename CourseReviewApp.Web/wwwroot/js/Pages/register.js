
//--- Display or hide course owner fields
function displayHiddenRegisterFields( option ) {
    $(document).ready(function () {
        if (option === "Course owner") {
            $("#course-owner-fields").removeAttr("hidden");
        }
        else {
            $("#course-owner-fields").attr("hidden", true);
            $("#register-websiteurl-input").val("");
            $("#register-brandname-input").val("");
        }
    });
};

//--- Validate checkbox for accepting Terms and Conditions
$(document).ready(function () {
    $("#terms-accept-checkbox").change(function () {
        if ($(this).prop("checked")) {
            $("#terms-accept-input").val(1);
        } else {
            $("#terms-accept-input").val(0);
        }
    });
});