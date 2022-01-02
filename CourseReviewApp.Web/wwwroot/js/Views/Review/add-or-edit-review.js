var contentsValidation = $("#contents-val");
var contentsCount = $("#contents-count");
var confirmBtn = $("#confirm-btn");

//--- Firefox does not support FA icons in select list
$(document).ready(function () {
    if (navigator.userAgent.indexOf("Firefox") != -1) {
        $("#option-1").html("&#9733;&#9734;&#9734;&#9734;&#9734;");
        $("#option-2").html("&#9733;&#9733;&#9734;&#9734;&#9734;");
        $("#option-3").html("&#9733;&#9733;&#9733;&#9734;&#9734;");
        $("#option-4").html("&#9733;&#9733;&#9733;&#9733;&#9734;");
        $("#option-5").html("&#9733;&#9733;&#9733;&#9733;&#9733;");
    }
});

//--- Contents chars limit
$("#contents").on("keyup", function () {
    var contentLength = parseInt($(this).val().replace(/(\r\n|\n|\r)/g, "  ").length, 10);
    if (contentLength > 1000) {
        contentsCount.html(0);
        contentsValidation.removeAttr("hidden");
        contentsValidation.text(`Contents is ${contentLength} characters long, max is 1000.`);
        confirmBtn.attr("disabled", true);
    }
    else {
        contentsValidation.attr("hidden", true);
        contentsCount.html(1000 - contentLength);
        confirmBtn.removeAttr("disabled");
    }
});

$("#contents").next().click(function () {
    $(document).ready(function () {
        contentsCount.html(1000);
    });
})
