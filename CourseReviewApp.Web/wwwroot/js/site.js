// JavaScript, jQuery functions and scripts. Whole project scope.

//--- Clear value of input field
$(document).ready(function () {
    $(".btn-clear").click(function () {
        $(this).prev().val("");
    });
});

//--- Back to previous page
$(document).ready(function () {
    $(".btn-back").click(function () {
        window.history.back();
    });
});

$(".collapse-change-arrow").on("shown.bs.collapse", function () {
    $(".collapse-arrow-icon").removeClass("fas fa-chevron-down").addClass("fas fa-chevron-up");
}).on("hidden.bs.collapse", function () {
    $(".collapse-arrow-icon").removeClass("fas fa-chevron-up").addClass("fas fa-chevron-down");
});
