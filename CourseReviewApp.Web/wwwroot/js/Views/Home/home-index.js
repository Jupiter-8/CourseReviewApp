
window.onload = function () {
    loadCarouselPartial("/Review/LastAddedReviews", "last-added-reviews-load-spinner", "last-added-reviews-msg", "last-added-reviews");
    loadCarouselPartial("/Course/LastAddedCourses", "last-added-courses-load-spinner", "last-added-courses-msg", "last-added-courses");
    loadCarouselPartial("/Course/BestRatedCourses", "best-rated-courses-load-spinner", "best-rated-courses-msg", "best-rated-courses");
};

//--- Load carousel partial view
function loadCarouselPartial(methodUrl, spinnerId, messageId, containerId) {
    $.ajax({
        type: 'GET',
        url: methodUrl,
        statusCode: {
            204: function () {
                $(`#${spinnerId}`).attr("hidden", true);
                $(`#${messageId}`).text("No results.");
                $(`#${messageId}`).removeAttr("hidden");
            }
        }
    })
        .done(function (result) {
            $(`#${spinnerId}`).attr("hidden", true);
            $(`#${containerId}`).html(result);
        })
        .fail(function () {
            $(`#${spinnerId}`).attr("hidden", true);
            $(`#${messageId}`).text("Unable to load results.");
            $(`#${messageId}`).removeAttr("hidden");
        });
};