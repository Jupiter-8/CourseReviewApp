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

//--- Select user type
$(document).ready(function () {
    $("input[type=radio][name=user-type-radio-btn]").change(function () {
        if (this.value == "1") {
            $("#course-owner-fields").removeAttr("hidden");
            $("#user-type-input").val(1);
        }
        else {
            $("#user-type-input").val(2);
            $("#course-owner-fields").attr("hidden", true);
            $("#register-websiteurl-input").val("");
            $("#register-brandname-input").val("");
        }
    });
});
