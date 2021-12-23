var table;

$(document).ready(function () {
    table = $("#observed-courses-table").DataTable();
});

function removeCourseFromObservedList(courseId, courseName, button) {
    $(document).ready(function () {
        $.ajax({
            type: "POST",
            url: "/Course/RemoveCourseFromObservedList",
            data: { "id": courseId }
        })
            .done(function () {
                table.row($(button).closest("tr")).remove().draw();
                $("#msg-modal-body").html(`Course ${courseName} has been removed from the observed courses list.`);
                $("#msg-modal").modal("show");
            })
            .fail(function () {
                $("#msg-modal-body").html("Error while removing course from the observed courses list.");
                    $("#msg-modal").modal("show");
                });
            
    });
};
 
