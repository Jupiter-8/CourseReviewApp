
//--- Show/hide select list with parent categories
$(document).ready(function () {
    $("#parent-categories-checkbox").change(function () {
        if ($(this).prop("checked")) {
            console.log("ts")
            $("#parent-categories-select").removeClass("d-none");
        } else {
            $("#parent-categories-select").addClass("d-none");
        }
    });
});