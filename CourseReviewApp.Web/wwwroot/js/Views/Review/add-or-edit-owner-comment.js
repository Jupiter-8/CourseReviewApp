var contentsValidation = $("#contents-val");
var contentsCount = $("#contents-count");
var confirmBtn = $("#confirm-btn");

//--- Contents chars limit
$("#contents").on("keyup", function () {
    var contentLength = parseInt($(this).val().replace(/(\r\n|\n|\r)/g, "  ").length, 10);
    if (contentLength > 500) {
        contentsCount.html(0);
        contentsValidation.removeAttr("hidden");
        contentsValidation.text(`Contents is ${contentLength} characters long, max is 500.`);
        confirmBtn.attr("disabled", true);
    }
    else {
        contentsValidation.attr("hidden", true);
        contentsCount.html(500 - contentLength);
        confirmBtn.removeAttr("disabled");
    }
});

$("#contents").next().click(function () {
    $(document).ready(function () {
        contentsCount.html(500);
    });
})
