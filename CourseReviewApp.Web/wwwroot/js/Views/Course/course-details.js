var isCourseObservedByUser = null;

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

function observeCourseButton(courseId, isObserved, button) {
    $(document).ready(function () {
        if (isCourseObservedByUser === null) {
            isCourseObservedByUser = isObserved;
        }
        if (isCourseObservedByUser === 'True') {
            removeCourseFromObservedList(courseId, button);
            isCourseObservedByUser = 'False';
        }
        else {
            addCourseToObservedList(courseId, button);
            isCourseObservedByUser = 'True';
        }
    });
}

function showMsgModal(message) {
    $("#msg-modal-body").html(message);
    $("#msg-modal").modal("show");
}

function removeCourseFromObservedList(courseId, button) {
    $.ajax({
        type: "POST",
        url: "/Course/RemoveCourseFromObservedList",
        data: { "id": courseId }
    })
        .done(function () {
            $(button).find("span").text(" Observe this course");
            $(button).removeClass("btn-info text-white shadow").addClass("btn-outline-info text-info");
        })
        .fail(function () {
            showMsgModal("Error while removing course to the observed courses list.");
        });
};

function addCourseToObservedList(courseId, button) {
    $.ajax({
        type: "POST",
        url: "/Course/AddCourseToObservedList",
        data: { "id": courseId }
    })
        .done(function () {
            $(button).removeClass("btn-outline-info text-info")
                     .addClass("btn-info text-white shadow");
            $(button).find("span").text(" Stop observing this course");
            showMsgModal("You will be notified when a new review will appear.");
        })
        .fail(function () {
            showMsgModal("Error while adding course to the observed courses list.");
        });
};