
//--- Vote on review helpfullness
function voteForReviewHelpfullness( reviewId, counterId, buttonId, messageId ) {
    $(document).ready(function () {
        $.ajax({
            type: "POST",
            url: "/Review/VoteForReviewHelpfullness",
            data: { "id": reviewId }
        })
            .done(function (response) {
                var counterValue = $(`#${counterId}`).text();
                var num = parseInt(counterValue, 10);
                if (response == "True") {
                    $(`#${buttonId}`).removeClass("btn-outline-danger text-dark").addClass("btn-danger text-white");
                    $(`#${messageId}`).removeAttr("hidden");
                    $(`#${counterId}`).text(num + 1);
                }
                else {
                    $(`#${buttonId}`).removeClass("btn-danger text-white").addClass("btn-outline-danger text-dark");
                    $(`#${messageId}`).attr("hidden", "");
                    $(`#${counterId}`).text(num - 1);
                }
            })
            .fail(function () {
                $("#msg-modal-body").html("Error while saving your vote.");
                $("#msg-modal").modal("show");
            });
    });
};
