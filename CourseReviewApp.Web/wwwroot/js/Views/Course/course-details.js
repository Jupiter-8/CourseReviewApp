
//--- Load reviews partial view
function loadReviews(courseId, sortOrder, filterValue, loadMore, numberOfReviews) {
    $(document).ready(function () {
        $("#reviews-load-spinner").removeAttr("hidden");
        $.ajax({
            type: 'GET',
            url: "/Review/GetReviews",
            data: {
                "courseId": courseId,
                "sortOrder": sortOrder,
                "filterValue": filterValue,
                "loadMore": loadMore,
                "numberOfReviews": numberOfReviews
            },
            statusCode: {
                204: function () {
                    $("#reviews-container").html("");
                    $("#reviews-load-spinner").attr("hidden", true);
                    $("#load-reviews-msg").text("No reviews.");
                    $("#load-reviews-msg").removeAttr("hidden");
                }
            }
        })
            .done(function (data) {
                $("#load-reviews-msg").attr("hidden", true);
                $("#reviews-load-spinner").attr("hidden", true);
                $("#reviews-section").html(data);
                $("#load-reviews-btn").removeAttr("hidden");
                if (sortOrder != '')
                    $("#sort-span").text(sortOrder);
            })
            .fail(function () {
                $("#load-reviews-btn").attr("disabled", true);
                $("#reviews-load-spinner").attr("hidden", true);
                $("#load-reviews-msg").text("Unable to load reviews.");
                $("#load-reviews-msg").removeAttr("hidden");
            });
    });
};

//--- Load reviews partial view
function readMore() {
    $(document).ready(function () {
        var dotsSpan = $("#dots-span");
        var moreSpan = $("#more-span");
        var moreBtn = $("#read-more-btn");

        if (dotsSpan.css("display") === "none") {
            dotsSpan.css("display", "inline");
            moreBtn.text("Read more");
            moreSpan.css("display", "none");
        } else {
            dotsSpan.css("display", "none");
            moreBtn.text("Read less");
            moreSpan.css("display", "inline");
        }
    });
};
