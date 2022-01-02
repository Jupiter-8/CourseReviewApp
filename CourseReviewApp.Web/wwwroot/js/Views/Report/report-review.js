var contentsValidation = $("#contents-val");
var contentsCount = $("#contents-count");
var confirmBtn = $("#confirm-btn");

//--- Contents chars limit
$("#contents").on("keyup", function () {
    var contentLength = parseInt($(this).val().replace(/(\r\n|\n|\r)/g, "  ").length, 10);
    if (contentLength > 400) {
        contentsCount.html(0);
        contentsValidation.removeAttr("hidden");
        contentsValidation.text(`Report contents is ${contentLength} characters long, max is 400.`);
        confirmBtn.attr("disabled", true);
    }
    else {
        contentsValidation.attr("hidden", true);
        contentsCount.html(400 - contentLength);
        confirmBtn.removeAttr("disabled");
    }
});

$("#contents").next().click(function () {
    $(document).ready(function () {
        contentsCount.html(400);
    });
})
