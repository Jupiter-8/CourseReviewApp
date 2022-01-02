var shortDescValidation = $("#short-val");
var shortDescCount = $("#short-count");
var longDescValidation = $("#long-val");
var longDescCount = $("#long-count");
var confirmBtn = $("#confirm-btn");

//--- Populate subcategory select list
function getSubCategories(id, selected) {
    $(document).ready(function () {
        $("select#CategoryId").empty();
        $.getJSON(`/Category/GetSubCategories?id=${id}`)
            .done(function (data) {
                $.each(data, function (i, item) {
                    if (item.id === selected) {
                        $("select#CategoryId").append(`<option selected value="${item.id}">${item.name}</option>`);
                    }
                    else {
                        $("select#CategoryId").append(`<option value="${item.id}">${item.name}</option>`);
                    }
                });
            })
            .fail(function () {
                $("#subcategory-validation-msg").text("Unable to get subcategories.");
            });
    });
};

//--- Populate subcategory select list on category select list change
$(function () {
    $(document).ready(function () {
        $("select#ParentCategoryId").change(function () {
            var id = $(this).val();
            getSubCategories(id);
        })
    });
});

//--- Validate size and display uploaded image 
function uploadCourseImg(input) {
    $(document).ready(function () {
        $("#course-img").attr("src", "");
        $("#course-img-name").text("");
        if (input.files && input.files[0]) {
            if (input.files[0].size > 2097152) {
                $("#course-img-validation-msg").removeAttr("hidden");
                $("#course-img-validation-msg").text("Chosen image is too big, max size is 2MB.");
                $("#add-course-confirm-btn").attr("disabled", true);

            }
            else if (input.files[0].name.split('.').pop() != "jpg") {
                $("#course-img-validation-msg").removeAttr("hidden");
                $("#course-img-validation-msg").text("Only JPG images are allowed!");
                $("#add-course-confirm-btn").attr("disabled", true);
            }
            else {
                $("#course-img-validation-msg").attr("hidden", true);
                $("#add-course-confirm-btn").removeAttr("disabled");
                $("#delete-course-image-btn").removeAttr("hidden");
                var reader = new FileReader();
                reader.onload = function (event) {
                    $("#course-img").attr("src", event.target.result);
                    $("#course-img-name").text(input.files[0].name);
                };
                reader.readAsDataURL(input.files[0]);
            }
        }
    });
};

//--- Delete loaded course image
$(document).ready(function () {
    $("#delete-course-image-btn").click(function () {
        var imgName = $("#course-img-name").text();
        if (imgName != "" && imgName != "default_course_image.jpg") {
            $("#course-img").attr("src", "");
            $("#course-img-name").text("");
            $("#course-img-input").val("");
            $("#course-img-to-delete").val(true);
        }
    });
});

//--- Switch arrow in collapse
$(".collapse").on("shown.bs.collapse", function () {
    $("#collapse-icon").removeClass("fas fa-chevron-down").addClass("fas fa-chevron-up");
}).on("hidden.bs.collapse", function () {
    $("#collapse-icon").removeClass("fas fa-chevron-up").addClass("fas fa-chevron-down");
});

//--- Short desc chars limit
$("#short-desc").on("keyup", function () {
    var contentLength = parseInt($(this).val().replace(/(\r\n|\n|\r)/g, "  ").length, 10);
    if (contentLength > 100) {
        shortDescCount.html(0);
        shortDescValidation.removeAttr("hidden");
        shortDescValidation.text(`Short description is ${contentLength} characters long, max is 100.`);
        confirmBtn.attr("disabled", true);
    }
    else {
        shortDescValidation.attr("hidden", true);
        shortDescCount.html(100 - contentLength);
        confirmBtn.removeAttr("disabled");
    }
});

$("#short-desc").next().click(function () {
    $(document).ready(function () {
        shortDescCount.html(100);
    });
})

//--- Long desc chars limit
$("#long-desc").on("keyup", function () {
    var contentLength = parseInt($(this).val().replace(/(\r\n|\n|\r)/g, "  ").length, 10);
    if (contentLength > 4000) {
        longDescCount.html(0);
        longDescValidation.removeAttr("hidden");
        longDescValidation.text(`Long description is ${contentLength} characters long, max is 4000.`);
        confirmBtn.attr("disabled", true);
    }
    else {
        longDescValidation.attr("hidden", true);
        longDescCount.html(4000 - contentLength);
        confirmBtn.removeAttr("disabled");
    }
});

$("#long-desc").next().click(function () {
    $(document).ready(function () {
        longDescCount.html(4000);
    });
})